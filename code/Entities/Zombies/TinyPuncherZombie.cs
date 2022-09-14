using Sandbox;
using Survivor.Utils;

// ReSharper disable PartialTypeWithSinglePart

namespace Survivor.Entities.Zombies;

public sealed partial class TinyPuncherZombie : PuncherZombie
{
	public TinyPuncherZombie()
	{
		// Ignored
	}

	public override int DataId => 2;

	protected override void Prepare()
	{
		base.Prepare();
	}
}
