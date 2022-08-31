using Sandbox;

namespace Survivor.Entities;

public class Zombie : ModelEntity
{
	//TODO: Make differents zombie's class who implements the main class Zombie ?
	private int _health;

	public Zombie()
	{
		// Ignored
	}

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );
	}
}
