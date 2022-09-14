using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Survivor.Entities.Zombies;
using Survivor.Utils;

namespace Survivor.GameResources;

[GameResource( "Zombie Data", "zombie", "Describes a zombie" )]
public class ZombieData : GameResource
{
	private static Dictionary<ZombieType, ZombieData> Resources { get; } = new();

	public static ZombieData GetResource( ZombieType zombieType )
	{
		return Resources.ContainsKey( zombieType ) ? Resources[zombieType] : null;
	}

	protected override void PostLoad()
	{
		if ( !Resources.ContainsKey( Type ) )
			Resources.Add( Type, this );
		Log.Info( $"Loaded {FriendlyName} with ID {Type}" );
	}

	protected override void PostReload()
	{
		Resources.Clear();
		if ( Resources.ContainsKey( Type ) )
			Resources.Remove( Type );
		Resources.Add( Type, this );
	}

	[Category( "General" )]                       public ZombieType Type          { get; set; }
	[Category( "General" )]                       public string     FriendlyName  { get; set; }
	[Category( "Model" ), ResourceType( "vmdl" )] public string     Model         { get; set; } = "models/citizen/citizen.vmdl";
	[Category( "Model" )]                         public Color      RenderColor   { get; set; } = Color.Green;
	[Category( "Model" )]                         public float      Scale         { get; set; } = 1f;
	[Category( "Behaviour" )]                     public float      Health        { get; set; } = 100f;
	[Category( "Behaviour" )]                     public float      MoveSpeed     { get; set; } = 5f;
	[Category( "Behaviour" )]                     public float      AttackRange   { get; set; } = 1.5f;
	[Category( "Behaviour" )]                     public float      AttackDamages { get; set; }
	[Category( "Behaviour" )]                     public float      AttackSpeed   { get; set; } = 1f;
	[Category( "Misc" )]                          public string[]   Tags          { get; set; } = { "zombie" };

	public void Apply( BaseZombie zombie )
	{
		zombie.SetModel( Model );
		zombie.RenderColor = RenderColor;
		zombie.FriendlyName = FriendlyName;
		zombie.Scale = Scale;

		zombie.Health = Health;
		zombie.MoveSpeed = InchesUtils.FromMeters( MoveSpeed );
		zombie.AttackRange = InchesUtils.FromMeters( AttackRange );
		zombie.AttackDamages = AttackDamages;
		zombie.AttackSpeed = AttackSpeed;

		zombie.Tags.Add( Tags );
	}
}
