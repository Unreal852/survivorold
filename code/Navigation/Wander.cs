using Sandbox;

namespace Survivor.Navigation;

public class Wander : NavSteer
{
	public float MinRadius { get; set; } = 200;
	public float MaxRadius { get; set; } = 500;

	public override void Tick( Vector3 position )
	{
		base.Tick( position );

		if ( Path.IsEmpty )
		{
			FindNewTarget( position );
		}
	}

	public virtual bool FindNewTarget( Vector3 center )
	{
		var t = NavMesh.GetPointWithinRadius( center, MinRadius, MaxRadius );
		if ( t.HasValue )
			TargetPosition = t.Value;
		return t.HasValue;
	}
}
