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
		if ( Type == ZombieType.Default )
			Log.Warning(
					$"The zombie data for '{(string.IsNullOrWhiteSpace( FriendlyName ) ? "NO_NAME" : FriendlyName)}' has the default zombie type. Please consider setting a proper type" );
	}

	protected override void PostReload()
	{
		_clothingContainer = null;
		if ( Resources.ContainsKey( Type ) )
			Resources.Remove( Type );
		Resources.Add( Type, this );
	}

	private ClothingContainer _clothingContainer;

	[Category( "General" )]                             public ZombieType Type          { get; set; }
	[Category( "General" )]                             public string     FriendlyName  { get; set; }
	[Category( "Model" ), ResourceType( "vmdl" )]       public string     Model         { get; set; } = "models/citizen/citizen.vmdl";
	[Category( "Model" )]                               public Color      RenderColor   { get; set; } = Color.Green;
	[Category( "Model" )]                               public float      Scale         { get; set; } = 1f;
	[Category( "Behaviour" )]                           public float      Health        { get; set; } = 100f;
	[Category( "Behaviour" )]                           public float      MoveSpeed     { get; set; } = 5f;
	[Category( "Behaviour" )]                           public float      AttackRange   { get; set; } = 1.5f;
	[Category( "Behaviour" )]                           public float      AttackDamages { get; set; } = 1f;
	[Category( "Behaviour" )]                           public float      AttackForce   { get; set; } = 1f;
	[Category( "Behaviour" )]                           public float      AttackSpeed   { get; set; } = 1f;
	[Category( "Clothes" ), ResourceType( "clothing" )] public string     Hat           { get; set; }
	[Category( "Clothes" ), ResourceType( "clothing" )] public string     Chest         { get; set; }
	[Category( "Clothes" ), ResourceType( "clothing" )] public string     Legs          { get; set; }
	[Category( "Clothes" ), ResourceType( "clothing" )] public string     Feet          { get; set; }
	[Category( "Misc" )]                                public string[]   Tags          { get; set; } = { "zombie" };

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
		zombie.AttackForce = AttackForce;
		zombie.AttackSpeed = AttackSpeed;

		if ( _clothingContainer == null )
		{
			_clothingContainer = new ClothingContainer();
			if ( !string.IsNullOrWhiteSpace( Hat ) )
				_clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Hat ) );
			if ( !string.IsNullOrWhiteSpace( Chest ) )
				_clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Chest ) );
			if ( !string.IsNullOrWhiteSpace( Legs ) )
				_clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Legs ) );
			if ( !string.IsNullOrWhiteSpace( Feet ) )
				_clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Feet ) );
		}

		if ( _clothingContainer.Clothing.Count >= 1 )
			_clothingContainer.DressEntity( zombie );

		zombie.Tags.Add( Tags );
	}
}
