using Sandbox;
using Sandbox.Component;
using Survivor.Assets;
using Survivor.Interaction;

namespace Survivor.Weapons;

public sealed partial class WeaponWorldModel : ModelEntity
{
	public WeaponWorldModel()
	{
	}

	public WeaponWorldModel( WeaponAsset asset ) : base( asset.WorldModel )
	{
		WeaponAsset = asset;
	}

	public WeaponAsset WeaponAsset { get; set; }

	public void SetGlow( IGlow glow, bool enable )
	{
		if ( !enable )
		{
			Components.Get<Glow>()?.Remove();
			return;
		}

		var glowComponent = Components.GetOrCreate<Glow>();
		glowComponent.Width = glow.GlowWidth;
		glowComponent.Color = glow.GlowColor;
		glowComponent.Enabled = true;
	}

	public override void Spawn()
	{
		PhysicsClear();
	}

	public override void ClientSpawn()
	{
		//Components.Create<Glow>( false );
	}
}
