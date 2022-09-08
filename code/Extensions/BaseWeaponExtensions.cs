using System.Collections.Generic;
using Sandbox;
using SWB_Base;

namespace Survivor.Extensions;

public static class BaseWeaponExtensions
{
	public static TraceResult TraceBulletEx( this BaseWeapon weapon, Vector3 start, Vector3 end, float radius = 2.0f )
	{
		var startsInWater = SurfaceUtil.IsPointWater( start );
		var withoutTags = new[] { "trigger", startsInWater ? "water" : string.Empty };

		var tr = Trace.Ray( start, end )
		              .UseHitboxes()
		              .WithoutTags( withoutTags )
		              .Ignore( weapon.Owner )
		              .Ignore( weapon )
		              .Size( radius )
		              .Run();

		return tr;
	}
}
