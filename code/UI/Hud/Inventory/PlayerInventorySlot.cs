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

	}

	public void Update( SurvivorPlayer owner, Entity entity )
	{
		if ( entity is ABaseWeapon weapon )
		{
			if ( weapon.Asset.WeaponType != _weaponAsset?.WeaponType )
			{
				_weaponAsset = weapon.Asset;
				OnSlotEntityChanged();
			}

			SetClass( "active", entity == owner.ActiveChild );
			_ammoLabel.Text = (weapon.Primary.Ammo + weapon.Primary.AmmoReserve).ToString();
		}
		
		_glyphImage.Texture = _slot switch
		{
				0 => Input.GetGlyph( InputButton.Slot1 ),
				1 => Input.GetGlyph( InputButton.Slot2 ),
				2 => Input.GetGlyph( InputButton.Slot3 ),
				3 => Input.GetGlyph( InputButton.Slot4 ),
				4 => Input.GetGlyph( InputButton.Slot5 ),
				_ => Input.GetGlyph( InputButton.Slot1 )
		};
	}

	private void OnSlotEntityChanged()
	{
		if ( _weaponAsset == null )
			return;
		_nameLabel.Text = _weaponAsset.DisplayName;
		UpdateWeaponIcon();
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
