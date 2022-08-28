using System.Collections.Generic;

namespace Sandbox;

public static class CleanupManager
{
	private static readonly Dictionary<long, List<Entity>> PlayerEntities = new();

	public static void AddEntity( in long clientId, in Entity entity )
	{
		if ( !PlayerEntities.ContainsKey( clientId ) )
			PlayerEntities.TryAdd( clientId, new List<Entity>() );

		PlayerEntities[clientId].Add( entity );
	}

	public static void CleanAll()
	{
		foreach ( var entities in PlayerEntities.Values )
		{
			foreach ( Entity entity in entities )
			{
				if ( entity.IsValid )
					entity.Delete();
			}

			entities.Clear();
		}
	}
}
