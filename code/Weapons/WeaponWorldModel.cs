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
		var glowComponent = Components.GetOrCreate<Glow>();
		glowComponent.Width = glow.GlowWidth;
		glowComponent.Color = glow.GlowColor;
		glowComponent.Enabled = enable;
	}

	public override void Spawn()
	{
		PhysicsClear();
	}
}
