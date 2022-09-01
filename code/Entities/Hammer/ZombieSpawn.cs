using Sandbox;
using SandboxEditor;

namespace Survivor.Entities.Hammer;

[Library( "survivor_zombie_spawn" )]
[Title( "Zombie spawn point" ), Category( "Zombie" ), Icon( "place" ), Description( "This entity defines the spawn point of the zombies" )]
[HammerEntity, EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
public partial class ZombieSpawn : SpawnPoint
{
	[Title( "Enabled" ), Description( "Unchecking this will prevent zombies from spawning on this spawn point" )]
	public bool IsEnabled { get; set; }
}
