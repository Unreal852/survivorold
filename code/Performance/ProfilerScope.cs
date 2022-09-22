using System;

namespace Survivor.Performance;

public struct ProfilerScope : IDisposable
{
	private readonly ProfilerEntry _parent;
	private readonly ProfilerEntry _me;
	private readonly double        _startTime;

	public ProfilerScope( string name )
	{
		_parent = Profiler.Root;

		_me = _parent.GetOrCreateChild( name );
		_startTime = Profiler.Stopwatch.Elapsed.TotalMilliseconds;
		Profiler.Root = _me;
	}

	public void Dispose()
	{
		_me.Add( Profiler.Stopwatch.Elapsed.TotalMilliseconds - _startTime );
		Profiler.Root = _parent;
	}
}
