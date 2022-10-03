using System;
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
	private int       _counter;
	private TimeSince _sinceCounterUpdate;

	[Net]
	public int EnemiesRemaining { get; set; } = 0;

	[Net]
	public int CurrentWave { get; set; } = 0;

	[Net]
	public Difficulty Difficulty { get; set; } = Difficulty.Normal;

	[Net]
	public GameState State { get; set; } = GameState.Lobby;

	[Net]
	public int Counter
	{
		get => Counter;
		set
		{
			if ( _counter == value )
				return;
			_counter = value;
			_sinceCounterUpdate = 0;
		}
	}

	public abstract string GameModeName { get; }

	protected BaseGameMode()
	{
	}

	public void SetCounter( int value )
	{
		if ( Counter == value )
			return;
		Counter = value;
		_sinceCounterUpdate = 0;
	}

	public void SetGameState( GameState state )
	{
		if ( State == state )
			return;
		State = state;
		switch ( State )
		{
			case GameState.Lobby:
				break;
			case GameState.Starting:
				{
					SetCounter( 10 );
				}
				break;
			case GameState.Playing:
				break;
			case GameState.Ending:
				break;
			case GameState.Ended:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public virtual void StartGame()
	{
		if ( State != GameState.Lobby || State != GameState.Starting )
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
				GameState.Lobby    => SurvivorGame.Current.PlayerLobbySpawnsPoints.RandomElement(),
				GameState.Starting => SurvivorGame.Current.PlayerLobbySpawnsPoints.RandomElement(),
				GameState.Playing  => SurvivorGame.Current.PlayerSpawnPoints.RandomElement(),
				_                  => null
		};

		if ( entity == null )
			return;

		player.Transform = entity.Transform;
		player.Rotation = entity.Rotation; // Trying things
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
		if ( !client.IsListenServerHost )
			return;


		if ( client.Pawn is not SurvivorPlayer pawn )
			return;
		if ( pawn.DevController is PlayerNoclipController )
			pawn.DevController = null;
		else
			pawn.DevController = new PlayerNoclipController();
	}

	[Event.Tick.Server]
	protected virtual void OnServerTick()
	{
		switch ( State )
		{
			case GameState.Starting:
				if ( _sinceCounterUpdate >= 1 )
				{
					if (Counter-- <= 0 )
					{
						StartGame();
						return;
					}

					_sinceCounterUpdate = 0;
				}

				break;
			default:
				break;
		}
	}
}
