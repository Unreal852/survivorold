using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;

namespace Survivor.UI.Hud;

public partial class SurvivorScoreboardEntry : Panel
{
	private readonly Label _playerName;
	private readonly Label _kills;
	private readonly Label _deaths;
	private readonly Label _money;
	private readonly Label _ping;

	public SurvivorScoreboardEntry()
	{
		AddClass( "entry" );

		_playerName = Add.Label( "PlayerName", "name" );
		_kills = Add.Label( "", "kills" );
		_deaths = Add.Label( "", "deaths" );
		_money = Add.Label( "", "money" );
		_ping = Add.Label( "", "ping" );
	}

	public  Client        Client          { get; set; }
	private RealTimeSince TimeSinceUpdate { get; set; } = 0;

	public override void Tick()
	{
		base.Tick();

		if ( !IsVisible )
			return;

		if ( !Client.IsValid() )
			return;

		if ( TimeSinceUpdate < 0.1f )
			return;

		TimeSinceUpdate = 0;
		UpdateData();
	}

	public virtual void UpdateData()
	{
		_playerName.Text = Client.Name;
		_kills.Text = Client.GetInt( "kills" ).ToString();
		_deaths.Text = Client.GetInt( "deaths" ).ToString();
		_money.Text = (Client.Pawn as SurvivorPlayer)?.Money.ToString() ?? "0";
		_ping.Text = Client.Ping.ToString();
		SetClass( "me", Client == Local.Client );
	}

	public virtual void UpdateFrom( Client client )
	{
		Client = client;
		UpdateData();
	}
}
