using Sandbox;
using Sandbox.Component;
using Sandbox.UI;
using SandboxEditor;
using Survivor.Entities.Hammer.Doors;
using Survivor.Players;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_animated_door" )]
[Category( "Map" ), Icon( "place" )]
[Title( "Buyable door" ), Description( "This entity defines a buyable door" )]
[HammerEntity, SupportsSolid, Model( Archetypes = ModelArchetype.animated_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class AnimatedDoor : AnimatedMapEntity, IBuyableDoor
{
	[Property]
	[Category( "Door" ), Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property, Net]
	[Category( "Door" ), Title( "Cost" ), Description( "The cost to unlock this door" )]
	public int Cost { get; set; } = 0;

	[Property, FGDType( "target_destination" )]
	[Category( "Door" ), Title( "Room" ), Description( "The room which this door will unlock. Multiple doors can unlock a single room" )]
	public string Room { get; set; } = "";

	[Property]
	[Category( "Door" ), Title( "Open Animation" ), Description( "The animation to play when the door is bought" )]
	public string OpenAnimParam { get; set; } = "open";

	[Net]
	public bool IsBought { get; private set; }

	public Entity DoorOwner
	{
		get => Owner;
		set => Owner = value;
	}

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
		if ( !IsEnabled || IsBought )
			return false;
		if ( user is SurvivorPlayer player )
		{
			if ( player.Money >= Cost )
			{
				player.Money -= Cost;
				OpenDoor( player );
			}
		}

		return false;
	}

	public void OpenDoor( SurvivorPlayer buyer )
	{
		IsBought = true;
		if ( Owner is Room room )
			room.IsBought = true;
		if ( buyer != null )
			ChatBox.AddChatEntry( To.Everyone, "Survivor", $"{buyer.Client.Name} opened {Room} for {Cost}$" );
		SetAnimParameter( OpenAnimParam, true );
	}

	public bool IsUsable( Entity user )
	{
		return !IsBought;
	}
}

public enum DoorOpenAction
{
	/// <summary>
	/// The door will move to the configured direction
	/// </summary>
	Move,

	/// <summary>
	/// The door will break, this requires a breakable model
	/// </summary>
	Destroy,

	/// <summary>
	/// The door will play an animation
	/// </summary>
	Animation
}
