using Sandbox;
using SandboxEditor;

namespace Survivor.Entities.Hammer;

[Library( "survivor_zombie_spawn" )]
[HammerEntity, EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
[Title( "Zombie Spawnpoint" ), Category( "Zombie" ), Icon( "place" )]
public partial class ZombieSpawn : SpawnPoint
{
	[Property( Title = "Model" )] public Model Model { get; set; } = null;
}
