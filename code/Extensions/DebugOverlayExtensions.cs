using Sandbox.Internal.Globals;

namespace Survivor.Extensions;

public static class DebugOverlayExtensions
{
	public static void Arrow( this DebugOverlay overlay, Vector3 startPos, Vector3 endPos, Vector3 up, Color color = default, float width = 8.0f )
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

		overlay.Line( p1, p2, color );
		overlay.Line( p2, p3, color );
		overlay.Line( p3, p4, color );
		overlay.Line( p4, p5, color );
		overlay.Line( p5, p6, color );
		overlay.Line( p6, p7, color );
	}
}
