using System;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x02000133 RID: 307
public class Shielder : EntityComponent, IComponent<Shielder, ShielderData>, ICallbackListener
{
	// Token: 0x06000A1C RID: 2588 RVA: 0x0002A572 File Offset: 0x00028772
	public ShielderData GetData()
	{
		return this._shielderData;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0002A57C File Offset: 0x0002877C
	public void OnInitialize(ShielderData data)
	{
		this._shielderData = data;
		this._shield = new GameObject("Shield").AddComponent<Shield>();
		this._shield.transform.position = base.transform.position;
		this._shield.Setup(this);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0002A5CC File Offset: 0x000287CC
	public override void OnSpawn(bool fromSave)
	{
		this._shield.transform.SetParent(null);
		if (base.Entity.Has_EComponent<AmmoBin>())
		{
			this._requiresAmmo = true;
			this._ammoBin = base.Entity.Get_EComponent<AmmoBin>(false);
			this._ammoBin.OnAmmoAdded += this.OnAmmoAdded;
		}
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0002A627 File Offset: 0x00028827
	private void OnAmmoAdded()
	{
		if (base.IsUpdating)
		{
			return;
		}
		Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, this._shielderData.burnTime, null));
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0002A659 File Offset: 0x00028859
	public void OnStartCallback(EntityCallbackEvent callback)
	{
		this._ammoBin.Consume(1);
		if (!this._shield.IsActive)
		{
			this._shield.EnableShield();
		}
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0002A680 File Offset: 0x00028880
	public void OnEndCallback(EntityCallbackEvent callback)
	{
		if (this._ammoBin.HasResource)
		{
			Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, this._shielderData.burnTime, null));
			return;
		}
		this._shield.DisableShield();
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0002A6D0 File Offset: 0x000288D0
	public override void OnReset()
	{
		this._shield.transform.SetParent(base.transform);
		this._shield.DisableShield();
		if (this._requiresAmmo)
		{
			this._ammoBin.OnAmmoAdded -= this.OnAmmoAdded;
		}
	}

	// Token: 0x0400063A RID: 1594
	private ShielderData _shielderData;

	// Token: 0x0400063B RID: 1595
	protected Shield _shield;

	// Token: 0x0400063C RID: 1596
	protected bool _requiresAmmo;

	// Token: 0x0400063D RID: 1597
	protected AmmoBin _ammoBin;
}
