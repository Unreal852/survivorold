using System.Collections.Generic;
using Sandbox;
using Survivor.Entities.Zombies;
using Survivor.Utils;

namespace Survivor.Assets;

[GameResource( "Zombie", "zombie", "Describes a zombie" )]
public class ZombieAsset : GameResource
{
	private static Dictionary<ZombieType, ZombieAsset> Resources { get; } = new();

	public static ZombieAsset GetResource( ZombieType zombieType )
	{ 
		return Resources.ContainsKey( zombieType ) ? Resources[zombieType] : null;
	}

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
	[Category( "Clothes" )]                             public bool       UseClothes    { get; set; } = false;
	[Category( "Clothes" ), ResourceType( "clothing" )] public string     Head          { get; set; }
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


		if ( UseClothes )
		{
			// TODO: Cache clothes instead of doing ResourceLibrary.Get<Clothing> every times e.g: Only do that on PostLoad / PostReload
			var clothingContainer = new ClothingContainer();
			if ( !string.IsNullOrWhiteSpace( Head ) )
				clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Head ) );
			if ( !string.IsNullOrWhiteSpace( Chest ) )
				clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Chest ) );
			if ( !string.IsNullOrWhiteSpace( Legs ) )
				clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Legs ) );
			if ( !string.IsNullOrWhiteSpace( Feet ) )
				clothingContainer.Clothing.Add( ResourceLibrary.Get<Clothing>( Feet ) );
			clothingContainer.DressEntity( zombie );
		}

		zombie.Tags.Add( Tags );
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
		if ( Resources.ContainsKey( Type ) )
			Resources.Remove( Type );
		Resources.Add( Type, this );
	}
}
