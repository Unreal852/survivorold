using Sandbox;
using Sandbox.UI;

namespace Survivor.UI.Hud;

public class PlayerHud : HudEntity<RootPanel>
{
	public PlayerHud()
	{
		if ( !IsClient )
			return;
		RootPanel.StyleSheet.Load( "UI/Hud/PlayerHud.scss" );
		RootPanel.AddChild<RemainingZombieLabel>();
		RootPanel.AddChild<WaveLabel>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<SurvivorScoreboard<SurvivorScoreboardEntry>>();
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		// I'm adding this here because otherwise i don't have access to Local.Client
		RootPanel.AddChild<PlayerInfosPanel>();
	}
}
