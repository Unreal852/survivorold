using Sandbox;
using Survivor.Gamemodes;
using Survivor.Rooms;

namespace Survivor.GameMode.GameModes;

public class SurvivorGameMode : BaseGameMode
{
	protected       RoomManager RoomsManager;
	public override string      GameModeName => "Survivor";

	public SurvivorGameMode()
	{
	}

	protected override void OnStartServer()
	{
		RoomsManager = new RoomManager();
		RoomsManager.LoadRooms();
	}

	protected override void OnServerTick()
	{
		base.OnServerTick();
	}
}
