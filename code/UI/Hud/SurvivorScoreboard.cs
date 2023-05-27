using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public partial class SurvivorScoreboard<T> : Panel where T : SurvivorScoreboardEntry, new()
{
	private readonly Dictionary<IClient, T> _rows = new();
	private readonly Panel                 _canvas;

	public Panel Header { get; protected set; }

	public SurvivorScoreboard()
	{
		StyleSheet.Load( "UI/Hud/SurvivorScoreboard.scss" );

		AddClass( "scoreboard" );
		AddHeader();
		_canvas = Add.Panel( "canvas" );
	}

	public bool IsOpen => Input.Down( "Score" );

	public override void Tick()
	{
		base.Tick();

		SetClass( "open", IsOpen );

		if ( !IsVisible )
			return;

		//
		// Clients that were added
		//
		foreach ( var client in Game.Clients.Except( _rows.Keys ) )
		{
			var entry = AddClient( client );
			_rows[client] = entry;
		}

		foreach ( var client in _rows.Keys.Except( Game.Clients ) )
		{
			if ( _rows.TryGetValue( client, out var row ) )
			{
				row?.Delete();
				_rows.Remove( client );
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

	protected virtual T AddClient( IClient entry )
	{
		var p = _canvas.AddChild<T>();
		p.Client = entry;
		return p;
	}
}
