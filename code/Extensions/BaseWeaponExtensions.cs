using Sandbox;
using SWB_Base;

namespace Survivor.Extensions;

public static class BaseWeaponExtensions
{
	public static TraceResult TraceBulletEx( this WeaponBase weapon, Vector3 start, Vector3 end, float radius = 2.0f )
	{
		var startsInWater = SurfaceUtil.IsPointWater( start );
		var withoutTags = new[] { startsInWater ? "water" : string.Empty };
		var withTags = new[] { "solid", "player", "zombie" };

		var tr = Trace.Ray( start, end )
					  //.UseHitboxes() Enabling this cause the ray to ignore zombies 
					  .WithoutTags( withoutTags )
					  .WithAnyTags( withTags )
					  .Ignore( weapon.Owner )
					  .Ignore( weapon )
					  .Size( radius )
					  .Run();

		return tr;
	}
}
