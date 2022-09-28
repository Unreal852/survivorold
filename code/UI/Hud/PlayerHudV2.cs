using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Assets;
using Survivor.Players;
using Survivor.Utils;
using Survivor.Weapons;

namespace Survivor.UI.Hud;

public class PlayerHudV2 : Panel
{
	private const float                  MaxBarWidth = 450; // Same width as in the width in .scss *-bar classes.
	private       Label                  _moneyLabel;
	private       Label                  _healthLabel;
	private       Label                  _healthBarLabel;
	private       Label                  _staminaBarLabel;
	private       Label                  _currentWeaponName;
	private       Label                  _currentWeaponAmmo;
	private       Label                  _currentWeaponAmmoReserve;
	private       PlayerHudV2Inventory[] _playerInventory;

	public PlayerHudV2()
	{
		StyleSheet.Load( "UI/Hud/PlayerHudV2.scss" );

		var healthBarBackground = Add.Label( "", "player-health-bar" );
		healthBarBackground.SetClass( "background-bar", true );

		var staminaBarBackground = Add.Label( "", "player-stamina-bar" );
		staminaBarBackground.SetClass( "background-bar", true );

		_moneyLabel = Add.Label( "", "player-money" );
		_healthLabel = Add.Label( "", "player-health" );
		_healthBarLabel = Add.Label( "", "player-health-bar" );
		_staminaBarLabel = Add.Label( "", "player-stamina-bar" );
		_currentWeaponName = Add.Label( "", "current-weapon-name" );
		_currentWeaponAmmo = Add.Label( "", "current-weapon-ammo" );
		_currentWeaponAmmoReserve = Add.Label( "", "current-weapon-reserve" );
		_playerInventory = new PlayerHudV2Inventory[5];
		for ( int i = 0; i < _playerInventory.Length; i++ )
		{
			_playerInventory[i] = new PlayerHudV2Inventory( this, $"inventory-slot-{i + 1}" );
		}
	}

	public override void Tick()
	{
		if ( Local.Pawn is not SurvivorPlayer player )
			return;

		_moneyLabel.Text = $"$ {player.Money}";
		_healthLabel.Text = $"♥️ {player.Health.CeilToInt()}";
		_healthBarLabel.Style.Width = MaxBarWidth  * player.Health  / player.MaxHealth;
		_staminaBarLabel.Style.Width = MaxBarWidth * player.Stamina / player.MaxStamina;

		if ( player.ActiveChild is ABaseWeapon weapon )
		{
			_currentWeaponName.Text = weapon.Asset.DisplayName;
			_currentWeaponAmmo.Text = weapon.Primary.Ammo.ToString();
			_currentWeaponAmmoReserve.Text = weapon.Primary.AmmoReserve.ToString();
		}

		var inventory = player.Inventory;
		for ( int i = 0; i < _playerInventory.Length; i++ )
		{
			_playerInventory[i].Update( inventory.GetSlot( i ) );
		}
	}
}

public class PlayerHudV2Inventory : Panel
{
	public  ScenePanel  WeaponIconScene { get; set; }
	public  SceneWorld  SceneWorld      { get; set; }
	private SceneModel  WeaponModel;
	private WeaponAsset WeaponAsset;
	private string      _slotClass;

	public PlayerHudV2Inventory( Panel parent, string slotClass )
	{
		Parent = parent;
		_slotClass = slotClass;
	}

	public void Update( Entity entity )
	{
		if ( entity is not ABaseWeapon weapon )
			return;
		UpdateWeaponIcon( weapon.Asset );
	}

	public void UpdateWeaponIcon( WeaponAsset asset )
	{
		if ( asset == null || WeaponAsset?.WeaponType == asset.WeaponType )
			return;
		WeaponAsset = asset;

		WeaponModel?.Delete();
		WeaponModel = null;

		WeaponIconScene?.Delete();
		WeaponIconScene = null;

		SceneWorld?.Delete();
		SceneWorld = new SceneWorld();

		WeaponModel = new SceneModel( SceneWorld, asset.WorldModel, Transform.Zero ) { ColorTint = Color.White, Rotation = Rotation.From( 0, -90, 0 ) };
		WeaponModel.Update( RealTime.Delta );

		WeaponIconScene = Parent.Add.ScenePanel( SceneWorld, Vector3.Zero, Rotation.Identity, 40, _slotClass );

		WeaponIconScene.Camera.Position = WeaponModel.Position + asset.UiIconOffset - Vector3.Backward * InchesUtils.FromMeters( asset.UiIconScale );
		WeaponIconScene.Camera.Rotation = Rotation.From( new(0, 180, 0) );
		WeaponIconScene.Camera.AmbientLightColor = Color.Black;
		WeaponIconScene.Camera.EnablePostProcessing = false;
		WeaponIconScene.RenderOnce = true;
	}

	public override void OnHotloaded()
	{
		base.OnHotloaded();

		UpdateWeaponIcon( WeaponAsset );
	}
}
