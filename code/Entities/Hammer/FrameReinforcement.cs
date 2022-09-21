using System.Collections.Generic;
using Sandbox;
using Sandbox.Component;
using Sandbox.UI;
using SandboxEditor;
using Survivor.Interaction;
using Survivor.Players;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_frame_reinforcement" )]
[Title( "Zombie Spawn Reinforcement" ), Category( "Map" ), Icon( "select_all" ), Description( "" )]
[HammerEntity, Solid, AutoApplyMaterial, HideProperty( "enable_shadows" )]
[VisGroup( VisGroup.Trigger )]
public partial class FrameReinforcement : ModelEntity, IUsable
{
	[Property]
	[Category( "Frame" ), Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	private List<PartInfos> Parts      { get; set; } = new();
	public  int             UseCost    => 0;
	public  string          UseMessage => "Reinforce";

	public override void Spawn()
	{
		SetupPhysicsFromModel( PhysicsMotionType.Static );

		Tags.Add( "trigger" );
		//EnableDrawing = false;
		EnableAllCollisions = false;
		EnableHitboxes = true;
		EnableTraceAndQueries = true;
	}

	public bool OnUse( Entity user )
	{
		if ( Children.Count == 0 )
		{
			foreach ( var part in Parts )
			{
				var prop = new Prop() { Model = part.Model, Transform = part.Transform, Scale = part.Scale, Static = true };
				prop.SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
				prop.SetParent( this );
			}
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	[Event.Entity.PostSpawn]
	private void PostLoad()
	{
		foreach ( Entity entity in Children )
		{
			if ( entity is not Prop prop )
				continue;
			Parts.Add( new PartInfos( prop.Model, prop.Transform, prop.Scale ) );
			Log.Info( entity );
		}
	}
}

public class PartInfos
{
	public PartInfos( Model model, Transform transform, float scale )
	{
		Model = model;
		Transform = transform;
		Scale = scale;
	}

	public Model     Model     { get; }
	public Transform Transform { get; }
	public float     Scale     { get; }
}
