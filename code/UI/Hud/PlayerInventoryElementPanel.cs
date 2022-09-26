using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Weapons;

namespace Survivor.UI.Hud;

public class PlayerInventoryElementPanel : Panel
{
	private readonly Label _bullets;
	private readonly Label _bulletReserve;
	private readonly Label _name;
	private readonly Image _icon;
	private readonly Image _glyph;

	public PlayerInventoryElementPanel( Panel parent )
	{
		Parent = parent;
		_bullets = Add.Label( "?/?", "ammo-count" );
		_bulletReserve = Add.Label( "?/?", "ammo-reserve" );
		_name = Add.Label( "", "name" );
		_glyph = Add.Image( "", "glyph" );
		_icon = Add.Image( "", "icon" );
	}

	public void Update( Entity entity, in int slot )
	{
		_glyph.Texture = slot switch
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

		if ( entity is ABaseWeapon weapon )
		{
			_name.Text = weapon.Asset.DisplayName;
			_bullets.Text = weapon.Primary.Ammo.ToString();
			_bulletReserve.Text = weapon.GetAvailableAmmo().ToString();
		}
	}

	public void Clear()
	{
		_name.Text = string.Empty;
		_bullets.Text = string.Empty;
		_bulletReserve.Text = string.Empty;
		_icon.Texture = null;
		SetClass( "active", false );
	}
}
