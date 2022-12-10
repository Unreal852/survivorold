using Sandbox;
using Sandbox.UI;
using SWB_Base.UI;

/* 
 * Weapon base UI
*/

namespace SWB_Base;

public partial class WeaponBase
{
    protected Panel healthDisplay;
    protected Panel ammoDisplay;
    protected Panel customizationMenu;

    protected Panel hitmarker;
    protected Panel crosshair;

    public override void CreateHudElements()
    {
        if (UISettings.HideAll) return;

        var showHUDCL = GetSetting<bool>("swb_cl_showhud", true);
        var showHUDSV = GetSetting<bool>("swb_sv_showhud", true);

        if (Game.RootPanel == null || !showHUDCL || !showHUDSV) return;

        if (UISettings.ShowCrosshair)
        {
            crosshair = CreateCrosshair();
            crosshair.Parent = Game.RootPanel;
        }

        if (UISettings.ShowHitmarker)
        {
            hitmarker = new Hitmarker
            {
                Parent = Game.RootPanel
            };
        }

        if (UISettings.ShowHealthCount || UISettings.ShowHealthIcon)
        {
            healthDisplay = new HealthDisplay(UISettings)
            {
                Parent = Game.RootPanel
            };
        }

        if (UISettings.ShowAmmoCount || UISettings.ShowWeaponIcon || UISettings.ShowFireMode)
        {
            ammoDisplay = new AmmoDisplay(UISettings)
            {
                Parent = Game.RootPanel
            };
        }
    }

    public virtual Panel CreateCrosshair()
    {
        return new Crosshair();
    }

    public override void DestroyHudElements()
    {
        if (healthDisplay != null) healthDisplay.Delete(true);
        if (ammoDisplay != null) ammoDisplay.Delete(true);
        if (hitmarker != null) hitmarker.Delete(true);
        if (crosshair != null) crosshair.Delete(true);
        if (customizationMenu != null) customizationMenu.Delete();
    }

    public void UISimulate(IClient player)
    {
        // Cutomization menu
        if (EnableCustomizationSV > 0 && Input.Pressed(InputButton.Menu) && AttachmentCategories != null)
        {
            if (customizationMenu == null)
            {
                customizationMenu = new CustomizationMenu();
                customizationMenu.Parent = Game.RootPanel;
            }
            else
            {
                customizationMenu.Delete();
                customizationMenu = null;
            }
        }

        IsCustomizing = customizationMenu != null;
    }
}
