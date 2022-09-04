using Sandbox;
using SandboxEditor;

namespace Survivor.Entities.Hammer;

[Library( "survivor_player_lobby_spawn" )]
[Title( "Player lobby spawn point" ), Category( "Player" ), Icon( "place" ),
 Description( "This entity defines the spawn point of the players in the lobby (pre-gale)" )]
[HammerEntity, EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
public partial class PlayerLobbySpawn : Entity
{
	[Property, Title( "Enabled" ), Description( "Unchecking this will prevent players from spawning on this spawn point" )]
	public bool IsEnabled { get; set; } = true;
}
