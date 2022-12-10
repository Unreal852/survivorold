using Sandbox;

namespace SWB_Base;

public class SpectateCamera : CameraMode
{
    public override void UpdateCamera()
    {
        if (Game.LocalPawn is not Player player)
            return;

        Camera.Position = player.EyePosition;

        //
    }
}
