using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.Component;
using Sandbox.UI;
using SandboxEditor;
using Survivor.Interaction;
using Survivor.Players;
using Survivor.Utils;

namespace Survivor.Entities.Hammer;

[Library( "survivor_spawn_reinforcement" )]
[Title( "Zombie Spawn Reinforcement" ), Category( "Zombie" ), Icon( "select_all" ), Description( "" )]
[HammerEntity, Solid, AutoApplyMaterial, HideProperty( "enable_shadows" )]
[VisGroup( VisGroup.Trigger )]
public partial class ZombieSpawnReinforcement : ModelEntity, IUsable
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
		Log.Info( "Use" );
		if ( Children.Count == 0 )
		{
			foreach ( var part in Parts )
			{
				var prop = new Prop()
				{
						Model = part.Model,
						Position = user.EyePosition + user.EyeRotation.Forward * InchesUtils.FromMeters( 3 ),
						Rotation = Rotation.Random,
						Scale = part.Scale / 2,
						Static = true
				};
				prop.SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
				prop.SetParent( this );
			}
		}

		return true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	[Event.Tick.Server]
	private void OnServerUpdate()
	{
		if ( Children.Count == 0 )
			return;
		for ( int i = 0; i < Children.Count; i++ )
		{
			var part = Parts[i];
			var child = Children[i];
			if ( !child.IsValid || child.Tags.Has( "endPos" ) )
				continue;
			const float animSpeed = 4f;
			child.Transform = Transform.Lerp( child.Transform, part.Transform, animSpeed * Time.Delta, false );
			child.Scale = MathX.Lerp( child.Scale, part.Scale, animSpeed                 * Time.Delta, false );
			if ( child.Transform == part.Transform )
				child.Tags.Add( "endPos" );
		}
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
