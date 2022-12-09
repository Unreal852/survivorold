using Editor;
using Sandbox;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library("survivor_zombie_spawn")]
[Category( "Zombie" ), Icon( "place" )]
[Title( "Zombie spawn point" ), Description( "This entity defines the spawn point of the zombies" )]
[HammerEntity, EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
public partial class ZombieSpawnPoint : Entity
{
	[Property, Title( "Enabled" ), Description( "Unchecking this will prevent zombies from spawning on this spawn point" )]
	public bool IsEnabled { get; set; } = true;

	[Property, FGDType( "target_destination" )]
	[Title( "Room" ), Description( "The room which this zombie spawn belongs to. If the room has not been bought, zombies will not spawn" )]
	public string Room { get; set; } = "";

	public bool CanSpawn => (Owner as Room)?.IsBought ?? string.IsNullOrWhiteSpace( Room );
}
