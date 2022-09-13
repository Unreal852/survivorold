using Sandbox;
using SandboxEditor;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_weapon_upgrade" )]
[Title( "Weapon Upgrader" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines a weapon upgrade" )]
[HammerEntity, SupportsSolid, Model( Model = "models/objects/weapon_upgrader.vmdl", Archetypes = ModelArchetype.animated_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class WeaponUpgrader : AnimatedEntity, IUse
{
	// TODO: This class has been written just to test things, this should be rewrite
	// TODO: Use anim tags

	[Property]
	[Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property]
	[Title( "Cost" ), Description( "The cost to unlock this door" )]
	public int Cost { get; set; } = 0;

	[Net] public bool        IsOpened                       { get; set; } = false;
	private      float       FramesBetweenModelColorChanges { get; set; } = 10;
	private      float       CurrentFrame                   { get; set; } = 0;
	private      float       StayOpenedDuration             { get; set; } = 5;
	private      float       DelayBetweenUses               { get; set; } = 3;
	private      TimeSince   TimeSinceOpened                { get; set; } = 0;
	private      TimeSince   TimeSinceClosed                { get; set; } = 0;

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
		return true;
	}

	public bool IsUsable( Entity user )
	{
		return !IsOpened && TimeSinceOpened >= StayOpenedDuration;
	}

	[Event.Tick.Server]
	public void OnTick()
	{
		if ( !IsOpened || TimeSinceOpened <= 1 || ++CurrentFrame < FramesBetweenModelColorChanges )
			return;
		CurrentFrame = 0;
		Close();
	}
}
