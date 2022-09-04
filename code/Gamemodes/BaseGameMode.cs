using Sandbox;
using Survivor.Players;

namespace Survivor.Gamemodes;

public abstract partial class BaseGameMode : Entity
{
	[Net] public int        EnemiesRemaining { get; set; } = 0;
	public       int        MinimumPlayers   { get; set; } = 1;
	[Net] public Difficulty Difficulty       { get; set; } = Difficulty.Normal;

	protected BaseGameMode()
	{
	}

	[Event.Tick.Server]
	public abstract void OnServerTick();

	public abstract bool CanRespawn( SurvivorPlayer player );
}
