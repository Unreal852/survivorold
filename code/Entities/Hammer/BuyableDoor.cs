using System;
using System.ComponentModel;
using Sandbox;
using Sandbox.Component;
using Sandbox.UI;
using SandboxEditor;
using Survivor.Players;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_buyable_door" )]
[Title( "Buyable door" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines a buyable door" )]
[HammerEntity, Solid, PhysicsTypeOverrideMesh]
public partial class BuyableDoor : ModelEntity, IUse
{
	private bool      _bought = false;
	private TimeSince _timeSinceBought;

	[Property]
	[Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property]
	[Title( "Cost" ), Description( "The cost to unlock this door" )]
	public int Cost { get; set; } = 0;

	[Property, FGDType( "target_destination" )]
	[Title( "Room" ), Description( "The room which this door will unlock. Multiple doors can unlock a single room" )]
	public string Room { get; set; } = "";

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
				ChatBox.AddChatEntry( To.Everyone, "Survivor", $"{player.Client.Name} opened {Room} for {Cost}" );
				_bought = true;
				_timeSinceBought = 0;
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
			// TODO: This
			Position += Vector3.Down * 10;
			if ( _timeSinceBought >= 3 )
			{
				Delete();
			}
		}
	}
}
