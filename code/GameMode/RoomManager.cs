using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Survivor.Entities.Hammer;
using Survivor.Entities.Hammer.Doors;

// resharper disable all

namespace Survivor.GameMode;

public class RoomManager
{
	private readonly Dictionary<string, Room> _rooms = new();

	public RoomManager()
	{
	}

	public void LoadRooms()
	{
		var rooms = Entity.All.OfType<Room>().ToDictionary( r => r.Name );
		var spawns = Entity.All.OfType<ZombieSpawnPoint>().ToArray();
		var doors = Entity.All.OfType<IBuyableDoor>().ToArray();
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
