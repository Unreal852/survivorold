using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;

namespace Survivor.UI.Hud;

public class PlayerInfosPanel : Panel
{
	private const    float MaxBarWidth = 350; // Same as in the .scss
	private readonly Label _healthBar;
	private readonly Label _staminaBar;
	private readonly Label _money;
	private readonly Label _name;
	private readonly Panel _avatarPanel;

	public PlayerInfosPanel()
	{
		StyleSheet.Load( "UI/Hud/PlayerInfosPanel.scss" );

		_avatarPanel = Add.Panel( "avatar" );
		_avatarPanel.Style.SetBackgroundImage( $"avatar:{Local.Client.PlayerId}" );

		//_nameLabel = Add.Label( title, "title" );

		_ = Add.Label( "", "health-bar-background" );
		_ = Add.Label( "", "stamina-bar-background" );
		_healthBar = Add.Label( "", "health-bar" );
		_staminaBar = Add.Label( "", "stamina-bar" );
		_money = Add.Label( "", "money" );
		_ = Add.Label( Local.DisplayName, "name" );
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
