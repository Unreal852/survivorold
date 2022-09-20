using Sandbox;
using Survivor.Assets;
using Survivor.Glow;

namespace Survivor.Weapons;

public sealed class WeaponWorldModel : ModelEntity
{
	public WeaponWorldModel()
	{
	}

	public WeaponWorldModel( WeaponAsset asset, bool glow = true ) : base( asset.WorldModel )
	{
		if ( glow )
		{
			var glowEffect = Components.Create<GlowEffect>();
			glowEffect.Active = true;
			glowEffect.Color = Color.Green;
		}
	}

	public override void Spawn()
	{
		PhysicsClear();
	}

	public WeaponAsset WeaponAsset { get; set; }
}
