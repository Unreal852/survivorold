using Survivor.UI.World;

namespace Sandbox.Players;

public class SurvivorPlayer : Player
{
	private readonly ClothingContainer    _clothing = new();
	private          PlayerNameWorldPanel WorldPanel { get; set; }

	public SurvivorPlayer()
	{
		// Ignored	
	}

	public SurvivorPlayer( Client client )
	{
		_clothing.LoadFromClient( client );
	}

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		_clothing.DressEntity( this );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();
		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		EnableTouch = true;
		Health = 100;
	}

	public override void Respawn()
	{
		Prepare();
		base.Respawn();
	}

	public override void BuildInput( InputBuilder input )
	{
		base.BuildInput( input );
		if ( Input.Pressed( InputButton.Menu ) && WorldPanel != null )
		{
			WorldPanel.Delete();
			WorldPanel = new PlayerNameWorldPanel( this ) { Transform = Transform };
			Log.Info( "Spawned panel" );
		}
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );
	}

	public override void ClientSpawn()
	{
		WorldPanel ??= new PlayerNameWorldPanel( this );
		Log.Info( "Spawned" );
	}
}
