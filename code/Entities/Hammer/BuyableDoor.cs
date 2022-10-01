using Sandbox;
using Sandbox.Component;
using Sandbox.UI;
using SandboxEditor;
using Survivor.Interaction;
using Survivor.Players;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_door" )]
[Category( "Map" ), Icon( "place" )]
[Title( "Buyable door" ), Description( "This entity defines a buyable door" )]
[HammerEntity, SupportsSolid, Model( Archetypes = ModelArchetype.breakable_prop_model )]
[RenderFields, VisGroup( VisGroup.Physics )]
public partial class BuyableDoor : AnimatedMapEntity, IUsable
{
	private bool      _bought          = false;
	private TimeSince _timeSinceBought = new TimeSince();

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
	[Category( "Door" ), Title( "Open action" )]
	public DoorOpenAction OpenAction { get; set; } = DoorOpenAction.Move;

	[Property, ShowIf( nameof(OpenAction), DoorOpenAction.Move )]
	[Category( "Door" ), Title( "Open Direction" ),
	 Description( "The direction where the door goes to play its opening animation. (Setting 0 will disable animation and the door will be instantly deleted" )]
	public Vector3 OpenMoveDirection { get; set; } = Vector3.Down;

	[Property, ShowIf( nameof(OpenAction), DoorOpenAction.Animation )]
	[Category( "Door" ), Title( "Open Animation" ),
	 Description( "The animation to play when the door is bought" )]
	public string OpenAnimParam { get; set; } = "open";

	[Property, ShowIf( nameof(OpenAction), DoorOpenAction.Move )]
	[Category( "Door" ), Title( "Open Speed" ), Description( "The open animation speed" )]
	public float OpenSpeed { get; set; } = 500f;

	[Property, ShowIf( nameof(OpenAction), DoorOpenAction.Move )]
	[Category( "Door" ), Title( "Delete after" ), Description( "Delete this door after the set amount of time (in seconds)" )]
	public float DeleteAfter { get; set; } = 2.5f;

	public int    UseCost    => Cost;
	public string UseMessage => "Unlock";

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
		if ( !IsEnabled || _bought )
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

	public void OpenDoor( SurvivorPlayer opener )
	{
		if ( Owner is Room room )
			room.IsBought = true;
		_bought = true;
		_timeSinceBought = 0;
		if ( opener != null )
			ChatBox.AddChatEntry( To.Everyone, "Survivor", $"{opener.Client.Name} opened {Room} for {Cost}$" );
		if ( OpenAction == DoorOpenAction.Animation )
		{
			if ( Parent is not AnimatedEntity animEnt )
			{
				Log.Error( "Missing parent or the parent is not an animated entity" );
				return;
			}

			SetAnimParameter( OpenAnimParam, true );
		}
	}

	public override void TakeDamage( DamageInfo info )
	{
	}

	public bool IsUsable( Entity user )
	{
		return !_bought;
	}

	private void UnregisterAndDelete()
	{
		Event.Unregister( this );
		Delete();
	}

	[Event.Tick.Server]
	public void Simulate()
	{
		if ( IsValid && _bought )
		{
			if ( OpenAction == DoorOpenAction.Move )
			{
				if ( OpenMoveDirection == Vector3.Zero )
				{
					UnregisterAndDelete();
					return;
				}

				Position += OpenMoveDirection * OpenSpeed * Time.Delta;
				if ( _timeSinceBought >= DeleteAfter )
				{
					UnregisterAndDelete();
				}
			}
			else if ( OpenAction == DoorOpenAction.Destroy )
			{
				UnregisterAndDelete();
			}
		}
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
