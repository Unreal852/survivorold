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
		if ( !IsVisible )
			return;

		switch ( player.Using )
		{
			case null:
				_useLabel.Text = string.Empty;
				_glyphImage.Texture = null;
				_useMessageLabel.Text = string.Empty;
				return;
			case IUsable usable when usable.IsUsable( player ):
				_useLabel.Text = "Use";
				_glyphImage.Texture = Input.GetGlyph( InputButton.Use, InputGlyphSize.Medium );
				_useMessageLabel.Text = usable.UseMessage;
				return;
		}

		_useLabel.Text = "Use";
		_glyphImage.Texture = Input.GetGlyph( InputButton.Use, InputGlyphSize.Medium );
		_useMessageLabel.Text = string.Empty;
	}
}
