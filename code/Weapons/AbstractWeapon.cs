using System;
using Sandbox;
using Survivor.Assets;
using SWB_Base;

namespace Survivor.Weapons;

/**
 * Changes from SWB
 * WeaponBase.Getters.GetAvailableAmmo -> virtual
 * WeaponBase.TakeAmmo -> virtual
 * WeaponBase.AsyncBoltBack -> protected
 */
public abstract partial class AbstractWeapon : WeaponBase
{
	// TODO: Add duck anim data

	protected AbstractWeapon( string weaponAssetName )
	{
		Asset = ResourceLibrary.Get<WeaponAsset>( $"config/weapons/{weaponAssetName}.wpn" );
		if ( Asset == null )
		{
			Log.Error( $"No weapon asset found with name '{weaponAssetName}'" );
			return;
		}

		UpdateAsset( Asset );

		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		UISettings.ShowFireMode = false;
		UISettings.ShowWeaponIcon = false;
		UISettings.ShowAmmoCount = false;
	}

	public          WeaponAsset Asset         { get; private set; }
	public override bool        BulletCocking => Asset.BulletCocking;
	public override HoldType    HoldType      => Asset?.HoldType ?? HoldType.Pistol;
	public override int         FOV           => 90;
	public override int         ZoomWeaponFOV => 90;

	public void UpdateAsset( WeaponAsset asset )
	{
		if ( asset == null )
		{
			Log.Error( "The specified asset is null !!!" );
			return;
		}

		Asset = asset;
		General = Asset.GetWeaponInfos();
		Primary = Asset.GetPrimaryClipInfos();
	}

	public void RefillAmmoReserve()
	{
		Primary.AmmoReserve = Asset.MaxAmmo;
	}

	protected int TakeAmmoFromReserve( int amount )
	{
		var available = Math.Min( Primary.AmmoReserve, amount );
		Primary.AmmoReserve -= available;
		return available;
	}

	// public override bool CanAttack( ClipInfo clipInfo, TimeSince lastAttackTime, InputButton inputButton )
	// {
	// 	// Replace lastAttackTime >= GetRealRPM(clipInfo.RPM);
	// 	return base.CanAttack( clipInfo, lastAttackTime, inputButton );
	// }

	public override void Reload()
	{
		if ( IsReloading || IsAnimating || InBoltBack || IsShooting() )
			return;

		var maxClipSize = BulletCocking ? Primary.ClipSize + 1 : Primary.ClipSize;

		if ( Primary.Ammo >= maxClipSize || Primary.ClipSize == -1 )
			return;

		var isEmptyReload = General.ReloadEmptyTime > 0 && Primary.Ammo == 0;
		TimeSinceReload = -(isEmptyReload ? General.ReloadEmptyTime : General.ReloadTime);

		if ( !isEmptyReload && Primary.Ammo == 0 && General.BoltBackTime > -1 )
		{
			TimeSinceReload -= General.BoltBackTime;

			if ( IsServer )
				_ = AsyncBoltBack( General.ReloadTime, General.BoltBackAnim, General.BoltBackTime,
						General.BoltBackEjectDelay, Primary.BulletEjectParticle );
		}

		if ( Primary.AmmoReserve <= 0 && Primary.InfiniteAmmo != InfiniteAmmoType.reserve )
			return;

		IsReloading = true;

		// Player anim
		if ( Owner is AnimatedEntity animEntity )
			animEntity.SetAnimParameter( "b_reload", true );

		StartReloadEffects( isEmptyReload );
	}

	public override bool TakeAmmo( int amount = 1 )
	{
		if ( Primary.InfiniteAmmo == InfiniteAmmoType.clip )
			return true;

		if ( Primary.ClipSize == -1 )
		{
			if ( Owner is PlayerBase player )
			{
				return player.TakeAmmo( Primary.AmmoType, amount ) > 0;
			}

			return true;
		}

		if ( Primary.Ammo < amount )
			return false;

		Primary.Ammo -= amount;
		return true;
	}

	public override void OnReloadFinish()
	{
		IsReloading = false;
		var maxClipSize = BulletCocking && Primary.Ammo > 0 ? Primary.ClipSize + 1 : Primary.ClipSize;

		if ( Primary.InfiniteAmmo == InfiniteAmmoType.reserve )
		{
			Primary.Ammo = maxClipSize;
			return;
		}

		var ammo = TakeAmmoFromReserve( maxClipSize - Primary.Ammo );
		if ( ammo == 0 )
			return;
		Primary.Ammo += ammo;
	}

	public override int GetAvailableAmmo()
	{
		return Primary.InfiniteAmmo == InfiniteAmmoType.reserve ? Primary.ClipSize : Primary.AmmoReserve;
	}
}
