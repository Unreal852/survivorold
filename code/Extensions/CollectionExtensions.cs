using System.Collections.ObjectModel;
using Sandbox;

namespace Survivor.Extensions;

public static class CollectionExtensions
{
	public static T RandomElement<T>( this ReadOnlyCollection<T> collection )
	{
		return collection.Count == 0 ? default : collection[Game.Random.Int( 0, collection.Count - 1 )];
	}
}
