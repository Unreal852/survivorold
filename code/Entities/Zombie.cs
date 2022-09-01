using Sandbox;

namespace Survivor.Entities;

public class Zombie : AnimatedEntity
{
	public Zombie()
	{
		// Ignored
	}

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );
	}
}
