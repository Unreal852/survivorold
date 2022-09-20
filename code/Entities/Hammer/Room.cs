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
	private BuyableDoor[] _roomDoors;
	private bool          _isBought;

	public Room()
	{
	}

	[Property]
	[Category( "Room" ), Title( "Open all doors when another door for this romm is bought" )]
	public bool OpenAllDoorsWhenBought { get; set; } = false;

	public bool HasSpawns => _zombieSpawns != null && _zombieSpawns.Length > 0;

	public bool IsBought
	{
		get => _isBought;
		set
		{
			if ( _isBought )
				return;
			_isBought = value;
			if ( _isBought && OpenAllDoorsWhenBought )
			{
				foreach ( var door in _roomDoors )
					door.OpenDoor( null );
			}
		}
	}

	public void InitializeRoom( BuyableDoor[] buyableDoors, ZombieSpawn[] zombieSpawns )
	{
		_roomDoors = buyableDoors    ?? Array.Empty<BuyableDoor>();
		_zombieSpawns = zombieSpawns ?? Array.Empty<ZombieSpawn>();
		foreach ( var zombieSpawn in _zombieSpawns )
			zombieSpawn.Owner = this;
		foreach ( var buyableDoor in buyableDoors )
			buyableDoor.Owner = this;
	}
}
