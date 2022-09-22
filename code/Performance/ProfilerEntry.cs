using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sandbox;

namespace Survivor.Performance;

public class ProfilerEntry
{
	private readonly Dictionary<string, ProfilerEntry> _entries = new();
	private          string                            _name;
	private          int                               _calls;
	private          double                            _times;

	public ProfilerEntry GetOrCreateChild( string name )
	{
		if ( _entries.ContainsKey( name ) )
			return _entries[name];

		var e = new ProfilerEntry { _name = name };
		_entries.Add( name, e );
		return e;
	}

	public void Add( double v )
	{
		_calls++;
		_times += v;
	}

	public void Wipe()
	{
		_calls = 0;
		_times = 0;

		//if ( _children == null ) return;

		foreach ( var kvp in _entries )
			kvp.Value.Wipe();
	}

	public string GetString( int indent = 0 )
	{
		var str = $"{new string( ' ', indent * 2 )}{_times:0.00}ms  {_calls} - {_name}\n";

		if ( indent == 0 )
			str = "";

		if ( _entries.Count == 0 )
			return str;

		foreach ( var child in _entries.OrderByDescending( x => x.Value._times ) )
		{
			if ( child.Value._calls == 0 ) continue;
			str += child.Value.GetString( indent + 1 );
		}

		return str;
	}
}
