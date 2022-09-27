using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Assets;
using Survivor.Utils;

namespace Survivor.UI.Hud;

public partial class PlayerInventoryElementPanel
{
	public  ScenePanel  WeaponIconScene { get; set; }
	public  SceneWorld  SceneWorld      { get; set; }
	private SceneModel  WeaponModel;
	private WeaponAsset WeaponAsset;

	public void CreateWeaponIcon( WeaponAsset asset )
	{
		if ( asset == null )
			return;
		if ( WeaponAsset?.WeaponType == asset.WeaponType )
			return;
		WeaponAsset = asset;

		WeaponModel?.Delete();
		WeaponModel = null;
		
		WeaponIconScene?.Delete();
		WeaponIconScene = null;

		SceneWorld?.Delete();
		SceneWorld = new SceneWorld();

		WeaponModel = new SceneModel( SceneWorld, asset.WorldModel, Transform.Zero )
		{
				ColorTint = Color.White, Rotation = Rotation.From( 0, -90, 0 )
		};
		WeaponModel.Update( RealTime.Delta );

		WeaponIconScene = Add.ScenePanel( SceneWorld, Vector3.Zero, Rotation.Identity, 40, "icon" );

		WeaponIconScene.Camera.Position = WeaponModel.Position + asset.UiIconOffset - Vector3.Backward * InchesUtils.FromMeters( asset.UiIconScale );
		WeaponIconScene.Camera.Rotation = Rotation.From( new(0, 180, 0) );
		WeaponIconScene.Camera.AmbientLightColor = Color.Black;
		WeaponIconScene.Camera.EnablePostProcessing = false;
		WeaponIconScene.RenderOnce = true;
	}

	public override void OnHotloaded()
	{
		base.OnHotloaded();

		CreateWeaponIcon( WeaponAsset );
	}
}
