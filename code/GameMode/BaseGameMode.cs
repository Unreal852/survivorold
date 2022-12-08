using Sandbox;
using Sandbox.Internal;
using Survivor.Extensions;
using Survivor.GameMode;
using Survivor.Players;
using Survivor.Players.Controllers;
using Survivor.UI.Hud;
using SWB_Base;

// ReSharper disable All

namespace Survivor.Gamemodes;

public abstract partial class BaseGameMode : BaseNetworkable
{
	public const int ZombiesPerWaves       = 5;
	public const int MaxSimultanousZombies = 20;

	private bool _inWave = false;
	private int  _spawnedEnemies;

	[Net]
	public int EnemiesRemaining { get; set; } = 0;

	[Net]
	public int CurrentWave { get; set; } = 0;

	[Net]
	public Difficulty Difficulty { get; set; } = Difficulty.Normal;

	[Net]
	public GameState State { get; set; } = GameState.Lobby;

	[Net]
	public TimeUntil Until { get; set; }

	public abstract string GameModeName { get; }

	protected BaseGameMode()
	{
		if ( Host.IsServer )
		{
			Event.Register( this );
			OnStartServer();
		}
	}

	public void SetGameState( GameState state )
	{
		if ( State == state )
		{
			Log.Warning( $"The state is already set to {state}" );
			return;
		}

		State = state;
		switch ( State )
		{
			case GameState.Starting:
				Until = 20;
				break;
			case GameState.Dev:
			case GameState.Playing:
				StartGame();
				break;
			default:
				break;
		}
	}

	public virtual void StartGame()
	{
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is SurvivorPlayer player )
				player.Respawn();
		}

		PlayerHudEntity.ShowGameHud( To.Everyone );

		Until = 10;
	}

	public virtual bool CanRespawn( SurvivorPlayer player )
	{
		return true;
	}

	public virtual void OnClientJoin( Client client )
	{
		var player = new SurvivorPlayer( client );
		player.Respawn();
		client.Pawn = player;
		if ( State == GameState.Playing )
			PlayerHudEntity.ShowGameHud( To.Single( client ) );
		else if ( State == GameState.Lobby && Client.All.Count >= SurvivorGame.VarMinimumPlayers )
			SetGameState( GameState.Starting );
	}

	public virtual void OnClientDisconnected( Client client, NetworkDisconnectionReason reason )
	{
	}

	public virtual void MovePlayerToSpawnPoint( Entity player )
	{
		Entity entity = State switch
		{
				GameState.Lobby => SurvivorGame.Current.PlayerLobbySpawnsPoints.RandomElement(),
				_               => SurvivorGame.Current.PlayerSpawnPoints.RandomElement(),
		};

		if ( entity == null )
			return;

		player.Transform = entity.Transform;
	}

	public virtual void OnDoPlayerDevCam( Client client )
	{
		if ( !client.IsListenServerHost )
			return;

		EntityComponentAccessor components = client.Components;
		var devCamera = components.Get<DevCamera>( true );
		if ( devCamera == null )
		{
			var component = new DevCamera();
			components = client.Components;
			components.Add( component );
		}
		else
			devCamera.Enabled = !devCamera.Enabled;
	}

	public virtual void OnDoPlayerNoclip( Client client )
	{
		if ( !client.IsListenServerHost && !client.HasPermission( "noclip" ) )
			return;

		if ( client.Pawn is not SurvivorPlayer pawn )
			return;
		if ( pawn.DevController is PlayerNoclipController )
			pawn.DevController = null;
		else
			pawn.DevController = new PlayerNoclipController();
	}

	public virtual void OnEnemyKilled( Entity killed, Entity killer )
	{
		EnemiesRemaining--;
		if ( EnemiesRemaining <= 0 )
		{
			_inWave = false;
			Until = 20;
		}
	}

	protected virtual void SpawnZombies( int amount )
	{
		var spawnedEnemies = SurvivorGame.Current.SpawnZombies( amount );
	}

	protected virtual void OnStartServer()
	{
	}

	[Event.Tick.Server]
	protected virtual void OnServerTick()
	{
		if ( State == GameState.Starting )
		{
			if ( Until )
				SetGameState( GameState.Playing );
		}

		if ( State == GameState.Playing )
		{
			if ( Until && !_inWave )
			{
				_inWave = true;
				CurrentWave++;
				SpawnZombies( CurrentWave * ZombiesPerWaves );
			}
		}
	}
}
