using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class EntityStatus : EntityComponent
{
	// Token: 0x0600090F RID: 2319 RVA: 0x0002695C File Offset: 0x00024B5C
	public override void OnSpawn(bool fromSave)
	{
		if (this._icon == null)
		{
			this._icon = Object.Instantiate<SpriteRenderer>(LegacyLibrary.ENTITY_STATUS_PREFAB);
			this._icon.transform.SetParent(base.transform);
			this._icon.transform.localPosition = Vector2.zero;
			this._icon.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x000269C8 File Offset: 0x00024BC8
	public void Toggle(bool toggle, EntityStatus.Type type)
	{
		if (toggle)
		{
			this._activeTypes |= type;
		}
		else
		{
			this._activeTypes &= ~type;
		}
		this.UpdateIconVisibility();
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x000269F4 File Offset: 0x00024BF4
	private void UpdateIconVisibility()
	{
		if (this._icon == null)
		{
			Debug.Log("[ENTITY STATUS] Could not update icon status because it does not exist!");
			return;
		}
		if (this._activeTypes == EntityStatus.Type.None)
		{
			this._icon.gameObject.SetActive(false);
			return;
		}
		switch (this._activeTypes)
		{
		case EntityStatus.Type.Ammo:
			if (this._lastType != EntityStatus.Type.Power)
			{
				this._icon.sprite = LegacyLibrary.POWER_ICON;
				this._lastType = EntityStatus.Type.Power;
			}
			break;
		case EntityStatus.Type.Power:
			if (this._lastType != EntityStatus.Type.Power)
			{
				this._icon.sprite = LegacyLibrary.POWER_ICON;
				this._lastType = EntityStatus.Type.Power;
			}
			break;
		case EntityStatus.Type.NoFilter:
			if (this._lastType != EntityStatus.Type.NoFilter)
			{
				this._icon.sprite = LegacyLibrary.NO_FILTER;
				this._lastType = EntityStatus.Type.NoFilter;
			}
			break;
		}
		if (!this._icon.gameObject.activeSelf)
		{
			this._icon.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00026ADC File Offset: 0x00024CDC
	public bool IsTypeSet(EntityStatus.Type type)
	{
		return (this._activeTypes & type) > EntityStatus.Type.None;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00026AE9 File Offset: 0x00024CE9
	public override void OnReset()
	{
		this._activeTypes = EntityStatus.Type.None;
	}

	// Token: 0x040005A0 RID: 1440
	private SpriteRenderer _icon;

	// Token: 0x040005A1 RID: 1441
	private EntityStatus.Type _activeTypes;

	// Token: 0x040005A2 RID: 1442
	private EntityStatus.Type _lastType;

	// Token: 0x0200010F RID: 271
	[Flags]
	public enum Type : byte
	{
		// Token: 0x040005A4 RID: 1444
		None = 0,
		// Token: 0x040005A5 RID: 1445
		Ammo = 1,
		// Token: 0x040005A6 RID: 1446
		Power = 2,
		// Token: 0x040005A7 RID: 1447
		NoFilter = 4
	}
}
