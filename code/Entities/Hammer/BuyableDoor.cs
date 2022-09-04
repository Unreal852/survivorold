using Sandbox;
using SandboxEditor;
using Survivor.Players;

namespace Survivor.Entities.Hammer;

[Library( "survivor_buyable_door" )]
[Title( "Buyable door" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines a buyable door" )]
[HammerEntity, Solid, PhysicsTypeOverrideMesh]
public partial class BuyableDoor : ModelEntity, IUse
{
	[Property, Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property, Title( "Cost" ), Description( "The cost to unlock this door" )]
	public int Cost { get; set; } = 0;

	[Property, Title( "Door Mesh" ), FGDType( "target_destination", "", "" )]
	public string DoorMesh { get; set; } = null;

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}

	public bool OnUse( Entity user )
	{
		if ( user is SurvivorPlayer player )
		{
			if ( player.Money >= Cost )
			{
				player.Money -= Cost;
				Delete();
			}
		}

		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
}
