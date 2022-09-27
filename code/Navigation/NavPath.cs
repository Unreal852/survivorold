using System.Collections.Generic;
using Sandbox;
using Survivor.Extensions;

namespace Survivor.Navigation;

public class NavPath
{
	public Vector3       TargetPosition;
	public List<Vector3> Points = new();
	public float         curFrame;
	public float         nextFrame;

	public bool IsEmpty => Points.Count <= 1;

	public void Update( ref Vector3 from, Vector3 to )
	{
		bool needsBuild = false;

		if ( !TargetPosition.AlmostEqual( to, 5 ) && ++curFrame >= 10 )
		{
			TargetPosition = to;
			needsBuild = true;
			curFrame = 0;
		}

		if ( needsBuild )
		{
			var fromFixed = NavMesh.GetClosestPoint( from );
			var toFixed = NavMesh.GetClosestPoint( to );

			if ( fromFixed == null || toFixed == null )
				return;

			Points.Clear();

			var path = NavMesh.PathBuilder( fromFixed.Value )
			                  .WithStepHeight( 30 ) // Same step as in the zombie class
			                  .WithMaxDropDistance( 10000 )
			                  .WithDropDistanceCostScale( 0.5f )
			                   //.WithMaxDetourDistance( 100 )
			                  .WithDuckHeight( 12 )
			                  .WithMaxClimbDistance( 80 ).Build( toFixed.Value );
			foreach ( var pathSegment in path.Segments )
				Points.Add( pathSegment.Position );
			
			Points.Add( NavMesh.GetClosestPoint( to ).Value );
		}

		if ( Points.Count <= 1 )
			return;

		var deltaToCurrent = from - Points[0];
		var deltaToNext = from    - Points[1];
		var delta = Points[1]     - Points[0];
		var deltaNormal = delta.Normal;

		if ( deltaToNext.WithZ( 0 ).Length < 20 )
		{
			Points.RemoveAt( 0 );
			return;
		}

		// If we're in front of this line then
		// remove it and move on to next one
		if ( deltaToNext.Normal.Dot( deltaNormal ) >= 1.0f )
			Points.RemoveAt( 0 );
	}

	public float Distance( int point, Vector3 from )
	{
		if ( Points.Count <= point )
			return float.MaxValue;

		return Points[point].WithZ( from.z ).Distance( from );
	}

	public Vector3 GetDirection( Vector3 position )
	{
		if ( Points.Count == 1 )
		{
			return (Points[0] - position).WithZ( 0 ).Normal;
		}

		return (Points[1] - position).WithZ( 0 ).Normal;
	}

	public void DebugDraw( float time, float opacity = 1.0f )
	{
		var lift = Vector3.Up * 2;
		DebugOverlay.Circle( lift + TargetPosition, Rotation.LookAt( Vector3.Up ), 20.0f, Color.White.WithAlpha( opacity ) );
		int i = 0;
		var lastPoint = Vector3.Zero;
		foreach ( var point in Points )
		{
			if ( i > 0 )
				DebugOverlay.Arrow( lastPoint + lift, point + lift, Vector3.Up, 5.0f );
			lastPoint = point;
			i++;
		}
	}
}
