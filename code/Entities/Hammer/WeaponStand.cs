using Sandbox;
using SandboxEditor;
using Survivor.Assets;
using Survivor.Interaction;
using Survivor.Players;
using Survivor.Players.Inventory;
using Survivor.Weapons;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_weapon_stand" )]
[Category( "Map" ), Icon( "place" )]
[Title( "Weapon Stand" ), Description( "This entity defines weapon stand" )]
[HammerEntity, SupportsSolid, Model( Model = "models/objects/weapons_stand.vmdl", Archetypes = ModelArchetype.generic_actor_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class WeaponStand : ModelEntity, IUsable, IGlow
{
	[Property]
	[Category( "Weapon Stand" ), Title( "Enabled" ), Description( "Unchecking this will prevent this weapon from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property]
	[Category( "Weapon Stand" ), Title( "Weapon" ), Description( "The weapon sold by this weapon stand" )]
	public WeaponType Weapon { get; set; }

	[Property, Net]
	[Category( "Weapon Stand" ), Title( "Cost" ), Description( "The cost to buy this weapon" )]
	public int Cost { get; set; } = 0;

	[Property, Net]
	[Category( "Weapon Stand" ), Title( "Ammo Cost" ), Description( "The cost to buy ammo for this weapon" )]
	public int AmmoCost { get; set; } = 0;

	[Net]
	private WeaponAsset WeaponAsset { get; set; }

	[Net]
	private WeaponWorldModel WorldModel { get; set; }

	public string UsePrefix { get; } = "Buy";

	public Color GlowColor
	{
		get
		{
			if ( Local.Pawn is not SurvivorPlayer player )
				return Color.Yellow;
			return player.Money >= UseCost ? Color.Green : Color.Red;
		}
	}

	public int UseCost
	{
		get
		{
			if ( Local.Pawn is not SurvivorPlayer player )
				return 0;
			if ( player.Inventory is SurvivorPlayerInventory inventory && inventory.HasWeapon( WeaponAsset.WeaponType ) )
				return AmmoCost;
			return Cost;
		}
	}

	public string UseMessage
	{
		get
		{
			if ( Local.Pawn is not SurvivorPlayer player )
				return string.Empty;
			if ( player.Inventory.HasWeapon( WeaponAsset.WeaponType ) )
				return "Buy Ammo";
			if ( player.Inventory.AreWeaponsSlotsFull() && player.ActiveChild is AbstractWeapon weapon )
				return $"Replace {weapon.Asset.DisplayName} with {WeaponAsset.DisplayName}";
			return $"Buy {WeaponAsset.DisplayName}";
		}
	}

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		var weapSpawnPos = GetAttachment( "weapon" );
		if ( !weapSpawnPos.HasValue )
		{
			Log.Warning( "Missing weapon attachement" );
			return;
		}

		WeaponAsset = WeaponAsset.GetWeaponAsset( Weapon );
		if ( WeaponAsset == null )
		{
			Log.Error( $"No weapon asset found for '{Weapon}'" );
			Delete();
			return;
		}

		WorldModel = new WeaponWorldModel( WeaponAsset ) { Transform = weapSpawnPos.Value, Parent = this };
	}

	public bool OnUse( Entity user )
	{
		if ( !IsEnabled )
			return false;
		if ( user is SurvivorPlayer player && player.TryUse() )
		{
			if ( player.Inventory.HasWeapon( WeaponAsset.WeaponType ) && player.Money >= AmmoCost )
			{
				player.Money -= AmmoCost;
				player.Inventory.Add( WeaponAsset.CreateWeaponInstance(), true );
			}
			else if ( player.Money >= Cost )
			{
				player.Money -= Cost;
				player.Inventory.Add( WeaponAsset.CreateWeaponInstance(), true );
			}
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
	
	public void SetGlow( bool enableGlow )
	{
		WorldModel.SetGlow( this, enableGlow );
	}
}
