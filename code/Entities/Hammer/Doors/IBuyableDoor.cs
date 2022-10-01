using Sandbox;
using Survivor.Interaction;
using Survivor.Players;

namespace Survivor.Entities.Hammer.Doors;

public interface IBuyableDoor : IUsable
{
	public Entity DoorOwner { get; set; }
	public bool   IsBought  { get; }
	public int    Cost      { get; set; }
	public string Room      { get; set; }

	string IUsable.UsePrefix  => "Unlock";
	string IUsable.UseMessage => Room;
	int IUsable.   UseCost    => Cost;

	public void OpenDoor( SurvivorPlayer buyer );
}
