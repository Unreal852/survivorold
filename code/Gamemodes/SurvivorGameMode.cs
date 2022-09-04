using Survivor.Players;

namespace Survivor.Gamemodes;

public class SurvivorGameMode : BaseGameMode
{
	public override void OnServerTick()
	{
	}

	public override bool CanRespawn( SurvivorPlayer player )
	{
		throw new System.NotImplementedException();
	}
}
