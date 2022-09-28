using Sandbox;
using Sandbox.UI;

namespace Survivor.UI.Hud;

public class PlayerHud : HudEntity<RootPanel>
{
	public PlayerHud()
	{
		if ( !IsClient )
			return;
		RootPanel.AddChild<GameInfosPanel>();
		//RootPanel.AddChild<PlayerInventoryPanel>();
		RootPanel.AddChild<PlayerInteractablePanel>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<SurvivorScoreboard<SurvivorScoreboardEntry>>();
	}

	public override void ClientSpawn()
	{
		// I'm adding this here because otherwise i don't have access to Local.Client
		RootPanel.AddChild<PlayerHudV2>();
		RootPanel.AddChild<PlayerInfosPanel>();
	}
}
