using Sandbox;
using Sandbox.UI;

namespace Survivor.UI.Hud;

public class PlayerHudEntity : HudEntity<RootPanel>
{
	public PlayerHudEntity()
	{
		if ( Host.IsServer )
			return;
		RootPanel.AddChild<GameInfosHud>();
		RootPanel.AddChild<PlayerInteractableHud>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<SurvivorScoreboard<SurvivorScoreboardEntry>>();
	}

	public override void ClientSpawn()
	{
		RootPanel.AddChild<PlayerHud>();
	}
}
