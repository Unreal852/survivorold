using System.Diagnostics;
using System.Linq;
using Editor;
using Sandbox;
using Survivor.Interaction;
using Survivor.Players;
using Survivor.Utils;

namespace Survivor.Entities.Hammer;

[Library( "survivor_zombie_spawn_reinforcement" )]
[Category( "Zombie" ), Icon( "select_all" )]
[Title( "Zombie Spawn Reinforcement" ), Description( "" )]
[HammerEntity, Solid, AutoApplyMaterial, HideProperty( "enable_shadows" )]
[VisGroup( VisGroup.Trigger )]
public partial class ZombieSpawnReinforcement : ModelEntity, IUsable
{
	[Property]
	[Category( "Frame" ), Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	private PartInfos[] PartsInfos { get; set; }
	public  int         UseCost    => 0;
	public  string      UsePrefix  => "Hold";
	public  string      UseSuffix  => "to reinforce";

	public override void Spawn()
	{
		SetupPhysicsFromModel( PhysicsMotionType.Static );

		Tags.Add( "trigger" );
		//EnableDrawing = false;
		EnableAllCollisions = false;
		EnableHitboxes = true;
		EnableTraceAndQueries = true;
	}

	public bool IsUsable( Entity user )
	{
		return true;
		//return Children.Count == 0;
	}

	private UseProgress UseProgress     { get; }      = new();
	private PartInfos   CurrentUsedPart { get; set; } = null;
	private TimeSince   SinceLastPlaced { get; set; }

	public bool OnUse( Entity user )
	{
		if ( user is not SurvivorPlayer player )
			return false;
		UseProgress.UpdateProgress( player );
		if ( CurrentUsedPart != null )
			return true;
		if ( SinceLastPlaced < 0.2 ) // Small delay between places
			return true;
		CurrentUsedPart = FindMissingPart();
		var prop = CurrentUsedPart?.CreateProp( this, user.AimRay.Project( InchesUtils.FromMeters( 3 ) ) );
		if ( prop != null )
			return true;

		return false;
	}

	private PartInfos FindMissingPart()
	{
		return PartsInfos.FirstOrDefault( partsInfo => partsInfo.IsMissing );
	}

	[GameEvent.Tick.Server]
	private void OnServerUpdate()
	{
		if ( !UseProgress.CheckProgress() )
		{
			if ( CurrentUsedPart is { IsPlaced: false } )
				CurrentUsedPart.DeleteProp();
			CurrentUsedPart = null;
			return;
		}

		if ( CurrentUsedPart is { IsMissing: false, IsPlaced: false } )
		{
			CurrentUsedPart.UpdatePos( 2.0f );
			if ( CurrentUsedPart.IsPlaced )
			{
				CurrentUsedPart = null;
				SinceLastPlaced = 0;
			}
		}
	}

	[GameEvent.Entity.PostSpawn]
	private static void OnPostLoad()
	{
		var sw = Stopwatch.StartNew();
		foreach ( var spawnReinforcement in All.OfType<ZombieSpawnReinforcement>() )
		{
			var children = spawnReinforcement.Children.OfType<Prop>().ToArray();
			if ( children.Length == 0 )
				Log.Error( "Found a spawn reinforcement without any childs !" );
			spawnReinforcement.PartsInfos = new PartInfos[children.Length];
			for ( int i = 0; i < spawnReinforcement.PartsInfos.Length; i++ )
			{
				var prop = children[i];
				spawnReinforcement.PartsInfos[i] = new PartInfos( prop.Model, prop.Transform, prop.Scale, prop );
			}
		}

		sw.Stop();
		Log.Info( $"Loaded all spawn reinforcements in {sw.Elapsed.TotalMilliseconds:F1}ms" );
	}
}

public class PartInfos
{
	private const float     SecondsToPos    = 0.8f;
	private       float     _currentSeconds = 0.0f;
	private       Transform _originalPos;

	public PartInfos( Model model, Transform transform, float scale, Prop prop )
	{
		Model = model;
		Transform = transform;
		Scale = scale;
		LinkedProp = prop;
	}

	public Model     Model      { get; }
	public Transform Transform  { get; }
	public float     Scale      { get; }
	public Prop      LinkedProp { get; private set; }

	public bool IsMissing => LinkedProp is not { IsValid: true };
	public bool IsPlaced  => !IsMissing && LinkedProp.Position == Transform.Position;

	public Prop CreateProp( ZombieSpawnReinforcement parent, Vector3 position )
	{
		if ( !IsMissing )
			return LinkedProp;
		_originalPos = new Transform( position, Rotation.Random, Scale / 3 );
		_currentSeconds = 0.0f;
		LinkedProp = new Prop { Model = Model, Transform = _originalPos, Static = true };
		LinkedProp.SetParent( parent );
		return LinkedProp;
	}

	public void DeleteProp()
	{
		LinkedProp?.Delete();
		LinkedProp = null;
	}

	public void UpdatePos( float speed )
	{
		if ( IsPlaced )
			return;
		_currentSeconds += Time.Delta;
		float time = _currentSeconds / SecondsToPos;

		var newTrans = Transform.Lerp( _originalPos, Transform, time, true );
		LinkedProp.Transform = newTrans.WithScale( MathX.Lerp( Scale / 2, Scale, time ) );
		if ( IsPlaced )
		{
			LinkedProp.SetupPhysicsFromModel( PhysicsMotionType.Static );
		}
	}
}

public class UseProgress
{
	public UseProgress()
	{
	}

	public SurvivorPlayer CurrentUser               { get; private set; }
	public bool           IsProgressing             { get; private set; }
	public float          ProgressUpdateMaxInterval { get; private set; } = 0.1f;
	public TimeSince      SinceProgressUpdate       { get; private set; }

	public void UpdateProgress( SurvivorPlayer player )
	{
		if ( CurrentUser != player && IsProgressing )
			return;
		CurrentUser ??= player;
		IsProgressing = true;
		SinceProgressUpdate = 0;
	}

	public bool CheckProgress()
	{
		switch ( IsProgressing )
		{
			case false:
				return false;
			case true when CurrentUser == null:
				ResetProgress();
				return IsProgressing;
			case true when SinceProgressUpdate >= ProgressUpdateMaxInterval:
				ResetProgress();
				return IsProgressing;
			default:
				return IsProgressing;
		}
	}

	private void ResetProgress()
	{
		CurrentUser = null;
		IsProgressing = false;
		SinceProgressUpdate = 0;
	}
}
