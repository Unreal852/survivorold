using Sandbox;
using SandboxEditor;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library("survivor_distributor")]
[Category( "Map" ), Icon( "place" )]
[Title( "Distributor" ),  Description( "This entity defines a distributor" )]
[HammerEntity, SupportsSolid, Model( Model = "models/objects/distributeurv2.vmdl", Archetypes = ModelArchetype.animated_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class Distributor : AnimatedEntity, IUse
{
	// TODO: This class has been written just to test things, this should be rewrite
	// TODO: Use anim tags

	[Property]
	[Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property]
	[Title( "Cost" ), Description( "The cost to unlock this door" )]
	public int Cost { get; set; } = 0;

	[Net]
	public bool IsOpened { get; set; } = false;

	private float       FramesBetweenModelColorChanges { get; set; } = 10;
	private float       StayOpenedDuration             { get; set; } = 5;
	private float       DelayBetweenUses               { get; set; } = 3;
	private TimeSince   TimeSinceOpened                { get; set; } = 0;
	private TimeSince   TimeSinceClosed                { get; set; } = 0;
	private ModelEntity Prop                           { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
		SetAnimParameter( "closing", true );
	}

	public void Open()
	{
		if ( IsOpened || TimeSinceClosed < DelayBetweenUses )
			return;
		IsOpened = true;
		SetAnimParameter( "opening", IsOpened );
		var att = GetAttachment( "item_spawn" );
		if ( att.HasValue )
			Prop = new ModelEntity( "models/attachments/canette.vmdl" ) { Position = att.Value.Position, Scale = 0.7f };
		TimeSinceOpened = 0;
	}

	public void Close()
	{
		if ( !IsOpened || TimeSinceOpened < StayOpenedDuration )
			return;
		IsOpened = false;
		SetAnimParameter( "closing", true );
		TimeSinceClosed = 0;
	}

	public bool OnUse( Entity user )
	{
		Open();
		return false;
	}

	public bool IsUsable( Entity user )
	{
		return !IsOpened && TimeSinceOpened >= StayOpenedDuration;
	}

	[Event.Tick.Server]
	public void OnTick()
	{
		if ( !IsOpened && Prop != null && TimeSinceClosed >= 2 )
		{
			Prop.Delete();
			Prop = null;
			return;
		}
		
		if ( Prop is { IsValid: true } )
		{
			Prop.Transform = GetAttachment( "item_spawn" ) ?? Transform.Zero;
			Prop.Scale = 0.7f;
		}

		if ( !IsOpened || TimeSinceOpened <= 1 )
			return;

		Close();
	}
}
