using Sandbox;
using Sandbox.Component;
using Sandbox.UI;
using SandboxEditor;
using Survivor.Players;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_buyable_door" )]
[Title( "Buyable door" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines a buyable door" )]
[HammerEntity, SupportsSolid, Model( Archetypes = ModelArchetype.animated_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class BuyableDoor : ModelEntity, IUse
{
	// TODO: Door open direction

	private bool      _bought = false;
	private TimeSince _timeSinceBought;

	[Property]
	[Category( "Door" ), Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property]
	[Category( "Door" ), Title( "Cost" ), Description( "The cost to unlock this door" )]
	public int Cost { get; set; } = 0;

	[Property, FGDType( "target_destination" )]
	[Category( "Door" ), Title( "Room" ), Description( "The room which this door will unlock. Multiple doors can unlock a single room" )]
	public string Room { get; set; } = "";

	[Property]
	[Category( "Door" ), Title( "Open Direction" ),
	 Description( "The direction where the door goes to play its opening animation. (Setting 0 will disable animation and the door will be instantly deleted" )]
	public Vector3 OpenDirection { get; set; } = Vector3.Down;

	[Property]
	[Category( "Door" ), Title( "Open Speed" ), Description( "The open animation speed" )]
	public float OpenSpeed { get; set; } = 500f;

	[Property]
	[Category( "Door" ), Title( "Delete after" ), Description( "Delete this door after the set amount of time (in seconds)" )]
	public float DeleteAfter { get; set; } = 2.5f;

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
				if ( Owner is Room room )
					room.IsBought = true;
				_bought = true;
				_timeSinceBought = 0;
				ChatBox.AddChatEntry( To.Everyone, "Survivor", $"{player.Client.Name} opened {Room} for {Cost}$" );
				if ( OpenDirection == Vector3.Zero )
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

	[Event.Tick.Server]
	public void Simulate()
	{
		if ( IsValid && _bought )
		{
			Position += OpenDirection * OpenSpeed * Time.Delta;
			if ( _timeSinceBought >= DeleteAfter )
				Delete();
		}
	}
}
