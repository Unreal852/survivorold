using Sandbox;
using SandboxEditor;
using Survivor.Players;
using Survivor.Weapons;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_weapon_stand" )]
[Title( "Weapon Stand" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines weapon stand" )]
[HammerEntity, SupportsSolid, Model( Model = "models/objects/tall_plate.vmdl", Archetypes = ModelArchetype.generic_actor_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class WeaponStand : ModelEntity, IUse
{
	[Property]
	[Title( "Enabled" ), Description( "Unchecking this will prevent this weapon from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property]
	[Title( "Cost" ), Description( "The cost to buy this weapon" )]
	public int Cost { get; set; } = 0;

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

		var prop = new ModelEntity( "models/weapons/assault_rifles/ak47/wm_ak47.vmdl" );
		prop.Transform = weapSpawnPos.Value;
		prop.PhysicsClear();
	}

	public bool OnUse( Entity user )
	{
		if ( !IsEnabled )
			return false;
		if ( user is SurvivorPlayer player && player.TryUse() )
		{
			if ( player.Money >= Cost )
			{
				player.Money -= Cost;
				player.Inventory.Add( new AK47(), true );
				Log.Info( player.Inventory );
				return true;
			}
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
