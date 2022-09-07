using Sandbox;

namespace Survivor;

public partial class SurvivorGame
{
	[ConVar.Server( "sv_gamemode", Help = "The game mode that will be played" )]
	public static string VarGameMode { get; set; }
	
	[ConVar.Server( "sv_difficulty", Help = "The game difficulty" )]
	public static string VarDifficulty { get; set; }
}
