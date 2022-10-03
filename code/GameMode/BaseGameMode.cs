using Sandbox;
using Sandbox.Internal;
using Survivor.Extensions;
using Survivor.GameMode;
using Survivor.Players;
using Survivor.Players.Controllers;
using Survivor.UI.Hud;

// ReSharper disable All

namespace Survivor.Gamemodes;

public abstract partial class BaseGameMode : Entity
{
	public int MinimumPlayers { get; set; } = 1;

	[Net]
	public int EnemiesRemaining { get; set; } = 0;

	[Net]
	public int CurrentWave { get; set; } = 0;

	[Net]
	public Difficulty Difficulty { get; set; } = Difficulty.Normal;

	[Net]
	public GameState State { get; set; } = GameState.Lobby;

	public abstract string GameModeName { get; }

	protected BaseGameMode()
	{
	}

	public virtual void StartGame()
	{
		if ( State != GameState.Lobby )
		{
			Log.Warning( "The game is already started" );
			return;
		}

		State = GameState.Playing;
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is SurvivorPlayer player )
				player.Respawn();
		}

		PlayerHudEntity.ShowGameHud( To.Everyone );
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
	}

	public virtual void OnClientDisconnected( Client client, NetworkDisconnectionReason reason )
	{
	}

	public virtual void MovePlayerToSpawnPoint( Entity player )
	{
		Entity entity = State switch
		{
				GameState.Lobby   => SurvivorGame.Current.PlayerLobbySpawnsPoints.RandomElement(),
				GameState.Playing => SurvivorGame.Current.PlayerSpawnPoints.RandomElement(),
				_                 => null
		};

		if ( entity == null )
			return;

		player.Transform = entity.Transform;
		player.Rotation = entity.Rotation; // Trying things
	}

	public virtual void OnDoPlayerDevCam( Client client )
	{
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

	public virtual void OnDoPlayerNoclip( Client player )
	{
		if ( player.Pawn is not SurvivorPlayer pawn )
			return;
		if ( pawn.DevController is PlayerNoclipController )
			pawn.DevController = null;
		else
			pawn.DevController = new PlayerNoclipController();
	}

	[Event.Tick.Server]
	public abstract void OnServerTick();
}
