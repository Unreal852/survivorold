using Editor;
using Sandbox;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library("survivor_player_lobby_spawn")]
[Category( "Player" ), Icon( "place" )]
[Title( "Player lobby spawn point" ), Description( "This entity defines the spawn point of the players in the lobby (pre-game)" )]
[HammerEntity, EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
public partial class PlayerLobbySpawnPoint : Entity
{
	[Property, Title( "Enabled" ), Description( "Unchecking this will prevent players from spawning on this spawn point" )]
	public bool IsEnabled { get; set; } = true;
}
