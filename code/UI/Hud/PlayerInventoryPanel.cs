using Sandbox;
using Sandbox.UI;
using Survivor.Players;
using Survivor.Weapons;

namespace Survivor.UI.Hud;

public class PlayerInventoryPanel : Panel
{
	private readonly PlayerInventoryElementPanel _primaryWeapon;
	private readonly PlayerInventoryElementPanel _secondaryWeapon;

	public PlayerInventoryPanel()
	{
		StyleSheet.Load( "UI/Hud/PlayerInventoryPanel.scss" );
		_primaryWeapon = new PlayerInventoryElementPanel( this );
		_secondaryWeapon = new PlayerInventoryElementPanel( this );
	}

	public override void Tick()
	{
		if ( Local.Pawn is not SurvivorPlayer player )
			return;
		UpdateInventoryElement( player, _primaryWeapon, 0 );
		UpdateInventoryElement( player, _secondaryWeapon, 1 );
	}

	private void UpdateInventoryElement( SurvivorPlayer player, PlayerInventoryElementPanel invElement, int slot )
	{
		Entity slotEntity = player.Inventory.GetSlot( slot );
		if ( slotEntity is not { IsValid: true } )
		{
			invElement.Clear();
			invElement.SetClass( "hidden", true );
			return;
		}

		invElement.SetClass( "hidden", false );
		invElement.SetClass( "active", player.ActiveChild == slotEntity );
		invElement.Update( slotEntity, in slot );
	}
}
