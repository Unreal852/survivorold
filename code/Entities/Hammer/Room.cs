using System;
using Sandbox;
using SandboxEditor;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_room" )]
[Title( "Room" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines a room" )]
[HammerEntity, EditorModel( "models/editor/air_node.vmdl" )]
public partial class Room : Entity
{
	private ZombieSpawn[] _zombieSpawns;

	public Room()
	{
	}

	public bool IsBought  { get; set; }
	public bool HasSpawns => _zombieSpawns != null && _zombieSpawns.Length > 0;

	public void InitializeRoom( BuyableDoor[] buyableDoors, ZombieSpawn[] zombieSpawns )
	{
		_zombieSpawns = zombieSpawns ?? Array.Empty<ZombieSpawn>();
		foreach ( var zombieSpawn in _zombieSpawns )
			zombieSpawn.Owner = this;
		foreach ( var buyableDoor in buyableDoors )
			buyableDoor.Owner = this;
	}
}
