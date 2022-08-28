using Sandbox;
using Survivor.UI.World;

namespace Survivor.Player;

public class SurvivorPlayer : Sandbox.Player
{
	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );

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

	private PlayerNameWorldPanel WorldPanel { get; set; }

	public override void Respawn()
	{
		base.Respawn();
		Prepare();
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

	public override void Spawn()
	{
		base.Spawn();
		Prepare();
	}
}
