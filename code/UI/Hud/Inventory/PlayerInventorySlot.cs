using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Assets;
using Survivor.Players;
using Survivor.Utils;
using Survivor.Weapons;

namespace Survivor.UI.Hud.Inventory;

public class PlayerInventorySlot : Panel
{
	private readonly int         _slot;
	private readonly Label       _ammoLabel;
	private readonly Label       _nameLabel;
	private          Image       _glyphImage;
	private          ScenePanel  _weaponIconPanel;
	private          SceneWorld  _sceneWorld;
	private          SceneModel  _weaponModel;
	private          WeaponAsset _weaponAsset;

	public PlayerInventorySlot( Panel parent, int slot )
	{
		Parent = parent;
		_slot = slot;
		_nameLabel = Add.Label( "", "name" );
		_ammoLabel = Add.Label( "", "ammo" );
		_glyphImage = Add.Image( "", "glyph" );
		_glyphImage.Texture = _slot switch
		{
				0 => Input.GetGlyph( InputButton.Slot1 ),
				1 => Input.GetGlyph( InputButton.Slot2 ),
				2 => Input.GetGlyph( InputButton.Slot3 ),
				3 => Input.GetGlyph( InputButton.Slot4 ),
				4 => Input.GetGlyph( InputButton.Slot5 ),
				5 => Input.GetGlyph( InputButton.Slot6 ),
				6 => Input.GetGlyph( InputButton.Slot7 ),
				7 => Input.GetGlyph( InputButton.Slot8 ),
				8 => Input.GetGlyph( InputButton.Slot9 ),
				9 => Input.GetGlyph( InputButton.Slot0 ),
				_ => Input.GetGlyph( InputButton.Slot1 ),
		};
	}

	public void Update( SurvivorPlayer owner, Entity entity )
	{
		if ( entity is ABaseWeapon weapon )
		{
			if ( weapon.Asset.WeaponType != _weaponAsset?.WeaponType )
			{
				_weaponAsset = weapon.Asset;
				OnSlotEntityChanged( owner, entity );
			}

			SetClass( "active", entity == owner.ActiveChild );
			_ammoLabel.Text = (weapon.Primary.Ammo + weapon.Primary.AmmoReserve).ToString();
		}
	}

	private void OnSlotEntityChanged( SurvivorPlayer owner, Entity entity )
	{
		if ( entity is ABaseWeapon weapon )
		{
			_nameLabel.Text = weapon.Asset.DisplayName;
			UpdateWeaponIcon();
		}
	}

	private void UpdateWeaponIcon()
	{
		if ( _weaponAsset == null )
			return;

		_weaponModel?.Delete();
		_weaponModel = null;

		_weaponIconPanel?.Delete();
		_weaponIconPanel = null;

		_sceneWorld?.Delete();
		_sceneWorld = new SceneWorld();

		_weaponModel = new SceneModel( _sceneWorld, _weaponAsset.WorldModel, Transform.Zero ) { ColorTint = Color.White, Rotation = Rotation.From( 0, -90, 0 ) };
		_weaponModel.Update( RealTime.Delta );

		_weaponIconPanel = Add.ScenePanel( _sceneWorld, Vector3.Zero, Rotation.Identity, 40, "icon" );

		_weaponIconPanel.Camera.Position = _weaponModel.Position + _weaponAsset.UiIconOffset - Vector3.Backward * InchesUtils.FromMeters( _weaponAsset.UiIconScale );
		_weaponIconPanel.Camera.Rotation = Rotation.From( new(0, 180, 0) );
		_weaponIconPanel.Camera.AmbientLightColor = Color.Black;
		_weaponIconPanel.Camera.EnablePostProcessing = false;
		_weaponIconPanel.RenderOnce = true;
	}

	public override void OnHotloaded()
	{
		base.OnHotloaded();

		UpdateWeaponIcon();
	}
}
