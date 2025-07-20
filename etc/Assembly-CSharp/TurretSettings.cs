using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class TurretSettings : EntitySettings
{
	// Token: 0x06000F6D RID: 3949 RVA: 0x00048DA0 File Offset: 0x00046FA0
	public override void Set(EntityComponent component)
	{
		Turret turret = component as Turret;
		if (turret != null)
		{
			this._turret = turret;
			this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(turret.Entity.GetData().model, this.modelSize, this.modelParent, null);
		}
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x00048DEC File Offset: 0x00046FEC
	public override void CustomUpdate()
	{
		foreach (TurretModeButton turretModeButton in this.buttons)
		{
			turretModeButton.CheckMode(this._turret.GetTargetMode());
		}
		if (this._cooldown > 0f)
		{
			this._cooldown -= Time.deltaTime;
			return;
		}
		switch (this._stage)
		{
		case 0:
			this.ring.sizeDelta = this.sizeOne;
			this._cooldown = 0.2f;
			this._stage = 1;
			return;
		case 1:
			this.ring.sizeDelta = this.sizeTwo;
			this._cooldown = 0.2f;
			this._stage = 2;
			return;
		case 2:
			this.ring.sizeDelta = this.sizeThree;
			this._cooldown = 0.2f;
			this._stage = 3;
			return;
		case 3:
			this.ring.sizeDelta = Vector2.zero;
			this._cooldown = 1f;
			this._stage = 0;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x00048F14 File Offset: 0x00047114
	public void SetMode(int mode)
	{
		this._turret.SetTargetMode(mode);
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x00048F22 File Offset: 0x00047122
	public override void Clear()
	{
		this._turret = null;
		Object.Destroy(this._model);
	}

	// Token: 0x04000D03 RID: 3331
	private Turret _turret;

	// Token: 0x04000D04 RID: 3332
	public Transform modelParent;

	// Token: 0x04000D05 RID: 3333
	public Vector2 modelSize;

	// Token: 0x04000D06 RID: 3334
	public Vector2 sizeOne;

	// Token: 0x04000D07 RID: 3335
	public Vector2 sizeTwo;

	// Token: 0x04000D08 RID: 3336
	public Vector2 sizeThree;

	// Token: 0x04000D09 RID: 3337
	public RectTransform ring;

	// Token: 0x04000D0A RID: 3338
	private GameObject _model;

	// Token: 0x04000D0B RID: 3339
	private float _cooldown;

	// Token: 0x04000D0C RID: 3340
	private int _stage;

	// Token: 0x04000D0D RID: 3341
	public List<TurretModeButton> buttons;
}
