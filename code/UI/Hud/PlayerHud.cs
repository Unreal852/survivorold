using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;
using Survivor.UI.Hud.Inventory;
using Survivor.Weapons;

namespace Survivor.UI.Hud;

public class PlayerHud : Panel
{
	private const    float                MaxBarWidth = 550; // Same width as in the width in .scss *-bar classes.
	private readonly Label                _moneyLabel;
	private readonly Label                _healthLabel;
	private readonly Label                _healthBarLabel;
	private readonly Label                _staminaBarLabel;
	private readonly Label                _currentWeaponNameLabel;
	private readonly Label                _currentWeaponAmmoLabel;
	private readonly Label                _currentWeaponAmmoReserveLabel;
	private readonly PlayerInventoryPanel _playerInventoryPanel;

	public PlayerHud()
	{
		StyleSheet.Load( "UI/Hud/PlayerHud.scss" );

		var healthBarBackground = Add.Label( "", "player-health-bar" );
		healthBarBackground.SetClass( "background-bar", true );

		var staminaBarBackground = Add.Label( "", "player-stamina-bar" );
		staminaBarBackground.SetClass( "background-bar", true );

		_moneyLabel = Add.Label( "", "player-money" );
		_healthLabel = Add.Label( "", "player-health" );
		_healthBarLabel = Add.Label( "", "player-health-bar" );
		_staminaBarLabel = Add.Label( "", "player-stamina-bar" );
		_currentWeaponNameLabel = Add.Label( "", "current-weapon-name" );
		_currentWeaponAmmoLabel = Add.Label( "", "current-weapon-ammo" );
		_currentWeaponAmmoReserveLabel = Add.Label( "", "current-weapon-reserve" );
		_playerInventoryPanel = new PlayerInventoryPanel( 5 );
		AddChild( _playerInventoryPanel );
	}

	public override void Tick()
	{
		if ( Game.LocalPawn is not SurvivorPlayer player )
			return;

		_moneyLabel.Text = $"$ {player.Money}";
		_healthLabel.Text = $"♥️ {player.Health.CeilToInt()}";
		_healthBarLabel.Style.Width = MaxBarWidth  * player.Health  / player.MaxHealth;
		_staminaBarLabel.Style.Width = MaxBarWidth * player.Stamina / player.MaxStamina;

		if ( player.ActiveChild is AbstractWeapon weapon )
		{
			_currentWeaponNameLabel.Text = weapon.Asset.DisplayName;
			_currentWeaponAmmoLabel.Text = weapon.Primary.Ammo.ToString();
			_currentWeaponAmmoReserveLabel.Text = weapon.Primary.AmmoReserve.ToString();
		}

		_playerInventoryPanel.Update( player );
	}
}
