using Sandbox.Internal;
using Sandbox.Internal.Globals;

namespace Survivor.Utils;

public static class DebugOverlayExtensions
{
	public static void Arrow( this DebugOverlay overlay, Vector3 startPos, Vector3 endPos, Vector3 up, float width = 8.0f )
	{
		var lineDir = (endPos - startPos).Normal;
		var sideDir = lineDir.Cross( up );
		var radius = width * 0.5f;
		var p1 = startPos - sideDir * radius;
		var p2 = endPos   - lineDir * width - sideDir * radius;
		var p3 = endPos   - lineDir * width - sideDir * width;
		var p4 = endPos;
		var p5 = endPos - lineDir * width + sideDir * width;
		var p6 = endPos - lineDir * width + sideDir * radius;
		var p7 = startPos                 + sideDir * radius;

		GlobalGameNamespace.DebugOverlay.Line( p1, p2 );
		GlobalGameNamespace.DebugOverlay.Line( p2, p3 );
		GlobalGameNamespace.DebugOverlay.Line( p3, p4 );
		GlobalGameNamespace.DebugOverlay.Line( p4, p5 );
		GlobalGameNamespace.DebugOverlay.Line( p5, p6 );
		GlobalGameNamespace.DebugOverlay.Line( p6, p7 );
	}
}
