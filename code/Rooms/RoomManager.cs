using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Survivor.Entities.Hammer;

// resharper disable all

namespace Survivor.Rooms;

public class RoomManager : EntityComponent
{
	private readonly Dictionary<string, Room> _rooms = new();

	public RoomManager()
	{
	}

	public void LoadRooms()
	{
		// TODO: This is not performant 
		var rooms = Entity.All.OfType<Room>().ToDictionary( r => r.Name );
		var spawns = Entity.All.OfType<ZombieSpawn>().ToArray();
		var doors = Entity.All.OfType<BuyableDoor>().ToArray();
		foreach ( var room in rooms.Values )
		{
			room.InitializeRoom(
					doors.Where( door => door.Room                == room.Name ).ToArray(),
					spawns.Where( zombieSpawn => zombieSpawn.Room == room.Name ).ToArray() );
			_rooms.Add( room.Name, room );
		}

		Log.Info( $"Loaded {rooms.Count} room(s) and {spawns.Length} zombies spawn(s)" );
	}
}
