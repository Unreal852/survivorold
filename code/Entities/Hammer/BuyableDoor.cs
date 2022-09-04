using System.ComponentModel;
using Sandbox;
using Sandbox.Component;
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

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		if ( IsEnabled )
			Enable();
	}

	/// <summary>
	/// Enables this blocker.
	/// </summary>
	[Input]
	public void Enable()
	{
		IsEnabled = true;

		if ( Components.Get<NavBlocker>( true ) != null )
			return;

		// If you are looking here, take note that the blocker will not update as the entity moves.
		Components.Add( new NavBlocker( NavBlockerType.BLOCK ) );
	}

	/// <summary>
	/// Disables this blocker.
	/// </summary>
	[Input]
	public void Disable()
	{
		IsEnabled = false;
		Components.RemoveAny<NavBlocker>();
	}

	public bool OnUse( Entity user )
	{
		if ( !IsEnabled )
			return false;
		if ( user is SurvivorPlayer player )
		{
			if ( player.Money >= Cost )
			{
				player.Money -= Cost;
				Delete();
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
