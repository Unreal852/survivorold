using Survivor.Gamemodes;
using Survivor.Rooms;

namespace Survivor.GameMode.GameModes;

public class SurvivorGameMode : BaseGameMode
{
	public override string GameModeName => "Survivor";

	public override void Spawn()
	{
		Components.GetOrCreate<RoomManager>().LoadRooms();
	}

	protected override void OnServerTick()
	{
	}
}
