using Sandbox;
using Sandbox.Component;
using Sandbox.UI;
using SandboxEditor;
using Survivor.Players;

namespace Survivor.Entities.Hammer.Doors;

[Library( "survivor_destructible_door" )]
[Category( "Map" ), Icon( "place" )]
[Title( "Destructible door" ), Description( "This entity defines a door that destroy itself when bought" )]
[HammerEntity, SupportsSolid, Model( Archetypes = ModelArchetype.breakable_prop_model )]
[RenderFields, VisGroup( VisGroup.Physics )]
public partial class DestructibleDoor : Prop, IBuyableDoor
{
	[Property]
	[Category( "Door" ), Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property, Net]
	[Category( "Door" ), Title( "Cost" ), Description( "The cost to unlock this door" )]
	public int Cost { get; set; } = 0;

	[Property, Net, FGDType( "target_destination" )]
	[Category( "Door" ), Title( "Room" ), Description( "The room which this door will unlock. Multiple doors can unlock a single room" )]
	public string Room { get; set; } = "";

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
		Break();
	}

	public override void TakeDamage( DamageInfo info )
	{
		// No damages
	}

	public bool IsUsable( Entity user )
	{
		return !IsBought;
	}
}
