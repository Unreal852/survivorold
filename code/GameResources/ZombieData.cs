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
	private static Dictionary<int, ZombieData> Resources { get; } = new();

	public static ZombieData GetResource( int id )
	{
		return Resources.ContainsKey( id ) ? Resources[id] : null;
	}

	protected override void PostLoad()
	{
		if ( !Resources.ContainsKey( Id ) )
			Resources.Add( Id, this );
		Log.Info( $"Loaded {FriendlyName} with ID {Id}" );
	}

	protected override void PostReload()
	{
		Resources.Clear();
		if ( Resources.ContainsKey( Id ) )
			Resources.Remove( Id );
		Resources.Add( Id, this );
	}

	[Category( "General" )]                       public int      Id            { get; set; }
	[Category( "General" )]                       public string   FriendlyName  { get; set; }
	[Category( "Model" ), ResourceType( "vmdl" )] public string   Model         { get; set; }
	[Category( "Model" )]                         public Color    RenderColor   { get; set; }
	[Category( "Model" )]                         public float    Scale         { get; set; } = 1;
	[Category( "Behaviour" )]                     public float    Health        { get; set; }
	[Category( "Behaviour" )]                     public float    MoveSpeed     { get; set; }
	[Category( "Behaviour" )]                     public float    AttackRange   { get; set; }
	[Category( "Behaviour" )]                     public float    AttackDamages { get; set; }
	[Category( "Behaviour" )]                     public float    AttackSpeed   { get; set; }
	[Category( "Misc" )]                          public string[] Tags          { get; set; } = { "zombie" };

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
