using System;
using Sandbox;

namespace Survivor.GameResources;

[GameResource( "Zombie Data", "zombie", "Describes a zombie" )]
public class ZombieData : GameResource
{
	[Category( "General" )]                       public string FriendlyName { get; set; }
	[Category( "Model" ), ResourceType( "vmdl" )] public string Model        { get; set; }
	[Category( "Behaviour" )]                     public float  MoveSpeed    { get; set; }
}
