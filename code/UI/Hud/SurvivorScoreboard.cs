using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public partial class SurvivorScoreboard<T> : Panel where T : SurvivorScoreboardEntry, new()
{
	private Dictionary<Client, T> Rows = new();

	public Panel Header { get; protected set; }

	public SurvivorScoreboard()
	{
		StyleSheet.Load( "Resources/UI/Scoreboard/Scoreboard.scss" );
		AddClass( "scoreboard" );

		AddHeader();

		Canvas = Add.Panel( "canvas" );
	}

	public Panel Canvas { get; protected set; }
	public bool  IsOpen => Input.Down( InputButton.Score );

	public override void Tick()
	{
		base.Tick();

		SetClass( "open", IsOpen );

		if ( !IsVisible )
			return;

		//
		// Clients that were added
		//
		foreach ( var client in Client.All.Except( Rows.Keys ) )
		{
			var entry = AddClient( client );
			Rows[client] = entry;
		}

		foreach ( var client in Rows.Keys.Except( Client.All ) )
		{
			if ( Rows.TryGetValue( client, out var row ) )
			{
				row?.Delete();
				Rows.Remove( client );
			}
		}
	}

	private void AddHeader()
	{
		Header = Add.Panel( "header" );
		Header.Add.Label( "Name", "name" );
		Header.Add.Label( "Kills", "kills" );
		Header.Add.Label( "Deaths", "deaths" );
		Header.Add.Label( "Money", "money" );
		Header.Add.Label( "Ping", "ping" );
	}

	protected virtual T AddClient( Client entry )
	{
		var p = Canvas.AddChild<T>();
		p.Client = entry;
		return p;
	}
}
