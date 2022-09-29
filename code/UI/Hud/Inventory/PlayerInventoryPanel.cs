using Sandbox.UI;
using Survivor.Players;

namespace Survivor.UI.Hud.Inventory;

public class PlayerInventoryPanel : Panel
{
	private readonly PlayerInventorySlot[] _inventorySlots;

	public PlayerInventoryPanel( int inventorySize )
	{
		StyleSheet.Load( "UI/Hud/Inventory/PlayerInventoryPanel.scss" );

		_inventorySlots = new PlayerInventorySlot[inventorySize];
		for ( int i = 0; i < inventorySize; i++ )
		{
			_inventorySlots[i] = new PlayerInventorySlot( this, i );
		}
	}

	public int Length => _inventorySlots.Length;

	public PlayerInventorySlot this[ int index ]
	{
		get => index >= 0 && _inventorySlots.Length < index ? _inventorySlots[index] : null;
	}

	public void Update( SurvivorPlayer player )
	{
		var inventory = player.Inventory;
		for ( int i = 0; i < _inventorySlots.Length; i++ )
			_inventorySlots[i].Update( player, inventory.GetSlot( i ) );
	}
}
