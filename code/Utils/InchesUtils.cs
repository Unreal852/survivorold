namespace Survivor.Utils;

public static class InchesUtils
{
	public static float ToMeters( in float inches )
	{
		return inches / 39.3700787f;
	}

	public static float FromMeters( in float meters )
	{
		return meters * 39.3700787f;
	}
}
