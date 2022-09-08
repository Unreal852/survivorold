using Sandbox.UI.Construct;

public class Scoreboard : Sandbox.UI.Scoreboard<ScoreboardEntry>
{
	public Scoreboard()
	{
		StyleSheet.Load( "deathmatch_dep/ui/Scoreboard.scss" );
	}

	protected override void AddHeader()
	{
		Header = Add.Panel( "header" );
		Header.Add.Label( "Player", "name" );
		Header.Add.Label( "Kills", "kills" );
		Header.Add.Label( "Deaths", "deaths" );
		Header.Add.Label( "Ping", "ping" );
	}
}

public class ScoreboardEntry : Sandbox.UI.ScoreboardEntry
{
}
