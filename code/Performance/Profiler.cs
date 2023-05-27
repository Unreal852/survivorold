using System;
using System.Diagnostics;
using Sandbox;

namespace Survivor.Performance;

public static class Profiler
{
	public static readonly Stopwatch     Stopwatch = Stopwatch.StartNew();
	public static          ProfilerEntry Root      = new();
	private static         TimeSince     _timeSince;

	public static IDisposable Scope( string name )
	{
		var scope = new ProfilerScope( name );
		return scope;
	}

	[Event.Hotload]
	private static void Hotloaded()
	{
		Root = new ProfilerEntry();
	}

	[GameEvent.Tick]
	private static void Frame()
	{
		if ( _timeSince >= 0.5f )
		{
			_timeSince = 0;
			DebugOverlay.ScreenText( Root.GetString(), 20, 0.5f );
		}

		Root.Wipe();
	}
}
