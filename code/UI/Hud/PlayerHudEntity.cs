using System.Linq;
using Sandbox;
using Sandbox.UI;

namespace Survivor.UI.Hud;

public partial class PlayerHudEntity : HudEntity<RootPanel>
{
	public static PlayerHudEntity Instance { get; private set; }

	public PlayerHudEntity()
	{
		if ( Game.IsServer )
			return;
		RootPanel.AddChild<LobbyHud>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<SurvivorScoreboard<SurvivorScoreboardEntry>>();
		Instance = this;
	}

	private void OnGameStart()
	{
		var lobbyHud = RootPanel.ChildrenOfType<LobbyHud>().FirstOrDefault();
		if ( lobbyHud is { IsValid: true } )
			lobbyHud.Delete( true );

		RootPanel.AddChild<PlayerHud>();
		RootPanel.AddChild<GameInfosHud>();
		RootPanel.AddChild<PlayerInteractableHud>();
	}

	[ClientRpc]
	public static void ShowGameHud()
	{
		if ( Instance == null )
		{
			Log.Error( "Missing hud instance, the server did not initialize it." );
			return;
		}

		Instance.OnGameStart();
	}
}
