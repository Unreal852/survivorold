using Sandbox;
using SandboxEditor;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_weapon_stand" )]
[Title( "Weapon Stand" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines weapon stand" )]
[HammerEntity, SupportsSolid, Model( Model = "models/objects/distributeur.vmdl", Archetypes = ModelArchetype.generic_actor_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class WeaponStand : ModelEntity
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
	}
}
