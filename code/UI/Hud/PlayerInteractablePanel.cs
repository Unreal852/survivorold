using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Interaction;
using Survivor.Players;

namespace Survivor.UI.Hud;

public class PlayerInteractablePanel : Panel
{
	private readonly Label _useLabel;
	private readonly Image _glyphImage;
	private readonly Label _useMessageLabel;

	public PlayerInteractablePanel()
	{
		StyleSheet.Load( "UI/Hud/PlayerInteractablePanel.scss" );

		_useLabel = Add.Label( "", "value" );
		_glyphImage = Add.Image( "", "glyph" );
		_useMessageLabel = Add.Label( "", "message" );
	}

	public override void Tick()
	{
		if ( Local.Pawn is not SurvivorPlayer player )
			return;
		SetClass( "visible", player.Using != null );
		if ( !IsVisible )
			return;

		_useLabel.Text = "Use";
		_glyphImage.Texture = Input.GetGlyph( InputButton.Use, InputGlyphSize.Medium );
		if ( player.Using is IUsable usable )
			_useMessageLabel.Text = usable.UseMessage;
		else
			_useMessageLabel.Text = string.Empty;
	}
}
