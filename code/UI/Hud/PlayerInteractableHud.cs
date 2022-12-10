using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Interaction;
using Survivor.Players;

namespace Survivor.UI.Hud;

public class PlayerInteractableHud : Panel
{
	private readonly Label _useLabel;
	private readonly Image _glyphImage;
	private readonly Label _costLabel;
	private readonly Label _useMessageLabel;

	public PlayerInteractableHud()
	{
		StyleSheet.Load( "UI/Hud/PlayerInteractableHud.scss" );

		_useLabel = Add.Label( "", "value" );
		_glyphImage = Add.Image( "", "glyph" );
		_costLabel = Add.Label( "", "cost" );
		_useMessageLabel = Add.Label( "", "message" );
	}

	public override void Tick()
	{
		if ( Game.LocalPawn is not SurvivorPlayer player )
			return;
		SetClass( "visible", player.Using != null );
		if ( !IsVisible )
			return;

		_glyphImage.Texture = Input.GetGlyph( InputButton.Use, InputGlyphSize.Medium );
		if ( player.Using is IUsable usable )
		{
			_useLabel.Text = usable.UsePrefix;
			_useMessageLabel.Text = usable.UseMessage;
			if ( string.IsNullOrEmpty( usable.UseSuffix ) && usable.HasCost )
			{
				_costLabel.Text = $"{usable.UseCost} $";
				_costLabel.Style.FontColor = player.Money < usable.UseCost ? Color.Red : Color.Green;
			}
			else
			{
				_costLabel.Text = usable.UseSuffix;
				_costLabel.Style.FontColor = Color.White;
			}
		}
		else
		{
			_useLabel.Text = string.Empty;
			_useMessageLabel.Text = string.Empty;
			_costLabel.Text = string.Empty;
		}
	}
}
