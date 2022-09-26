using Sandbox;
using Survivor.Assets;
using Survivor.Glow;

namespace Survivor.Weapons;

public sealed class WeaponWorldModel : ModelEntity
{
	public WeaponWorldModel()
	{
	}

	public WeaponWorldModel( WeaponAsset asset, bool glow = false ) : base( asset.WorldModel )
	{
		if ( glow )
			EnableGlow();
	}

	public WeaponAsset WeaponAsset { get; set; }
	public bool        HasGlow     { get; private set; }

	public void EnableGlow()
	{
		if ( HasGlow )
			return;
		var glowEffect = Components.Create<GlowEffect>();
		glowEffect.Active = true;
		glowEffect.Color = Color.Green;
		HasGlow = true;
	}

	public override void Spawn()
	{
		PhysicsClear();
	}
}
