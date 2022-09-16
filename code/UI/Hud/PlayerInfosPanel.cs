using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;

namespace Survivor.UI.Hud;

public class PlayerInfosPanel : Panel
{
	private const    float MaxBarWidth = 350; // Same width as in the .scss *-bar classes.
	private readonly Label _healthBar;
	private readonly Label _staminaBar;
	private readonly Label _money;

	public PlayerInfosPanel()
	{
		StyleSheet.Load( "UI/Hud/PlayerInfosPanel.scss" );

		Add.Panel( "avatar" ).Style.SetBackgroundImage( $"avatar:{Local.Client.PlayerId}" );
		_ = Add.Label( "", "health-bar-background" );
		_ = Add.Label( "", "stamina-bar-background" );
		_ = Add.Label( Local.DisplayName, "name" );
		_healthBar = Add.Label( "", "health-bar" );
		_staminaBar = Add.Label( "", "stamina-bar" );
		_money = Add.Label( "", "money" );
	}

	public override void Tick()
	{
		if ( Local.Pawn is not SurvivorPlayer player )
			return;

		_healthBar.Style.Width = MaxBarWidth  * player.Health  / player.MaxHealth;
		_staminaBar.Style.Width = MaxBarWidth * player.Stamina / player.MaxStamina;
		_money.Text = $"$ {player.Money}";
	}
}
