using Sandbox;
using Sandbox.Internal;
using Survivor.GameMode;
using Survivor.Players;
using Survivor.Players.Controllers;

// ReSharper disable All

namespace Survivor.Gamemodes;

public abstract partial class BaseGameMode : Entity
{
	public          int        MinimumPlayers   { get; set; } = 1;
	[Net] public    int        EnemiesRemaining { get; set; } = 0;
	[Net] public    int        CurrentWave      { get; set; } = 0;
	[Net] public    Difficulty Difficulty       { get; set; } = Difficulty.Normal;
	public abstract string     GameModeName     { get; }

	protected BaseGameMode()
	{
	}

	public virtual bool CanRespawn( SurvivorPlayer player )
	{
		return true;
	}

	public virtual void OnClientJoin( SurvivorGame game, Client client )
	{
		var player = new SurvivorPlayer( client );
		player.Respawn();
		client.Pawn = player;
	}

	public virtual void OnClientDisconnected( SurvivorGame game, Client client, NetworkDisconnectionReason reason )
	{
	}

	public virtual void OnDoPlayerDevCam( SurvivorGame game, Client client )
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

	public virtual void OnDoPlayerNoclip( SurvivorGame game, Client player )
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
