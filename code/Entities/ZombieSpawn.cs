using Sandbox;
using SandboxEditor;

namespace Survivor.Entities;

[Library( "survivor_zombie_spawn" )]
[HammerEntity, EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
[Title( "Zombie Spawnpoint" ), Category( "Zombie" ), Icon( "place" )]
public partial class ZombieSpawn : SpawnPoint
{
	[Property( Title = "Model" )] public Model ModelToSpawn { get; set; } = null;
	[Property( Title = "Message" )] public string  Message    { get; set; } = "Default";
}
