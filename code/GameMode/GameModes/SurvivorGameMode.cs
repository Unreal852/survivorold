using Survivor.Gamemodes;
using Survivor.Players;
using Survivor.Rooms;

namespace Survivor.GameMode.GameModes;

public class SurvivorGameMode : BaseGameMode
{
	public override string GameModeName => "Survivor";

	public override void Spawn()
	{
		Components.GetOrCreate<RoomManager>().LoadRooms();
	}

	public override void OnServerTick()
	{
	}
}
