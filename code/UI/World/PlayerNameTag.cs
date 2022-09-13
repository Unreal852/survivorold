using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.World;

public class PlayerNameTag : WorldPanel
{
	private Panel _avatar;
	private Label _nameLabel;

	public PlayerNameTag( string title, long? steamId )
	{
		StyleSheet.Load( "UI/World/PlayerNameTag.scss" );

		if ( steamId != null )
		{
			_avatar = Add.Panel( "avatar" );
			_avatar.Style.SetBackgroundImage( $"avatar:{steamId}" );
		}

		_nameLabel = Add.Label( title, "title" );

		// this is the actual size and shape of the world panel
		PanelBounds = new Rect( -500, -100, 1000, 200 );
	}
}
