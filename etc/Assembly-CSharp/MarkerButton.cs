using System;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001CD RID: 461
public class MarkerButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000E81 RID: 3713 RVA: 0x000412C4 File Offset: 0x0003F4C4
	public void Set(BeaconSettings parent, IconData iconData, Color color)
	{
		this._parent = parent;
		this._icon = iconData;
		this.icon.sprite = iconData.sprite;
		this.background.color = color;
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x000412F1 File Offset: 0x0003F4F1
	public override void OnClick()
	{
		this._parent.SetIcon(this._icon);
		base.OnClick();
	}

	// Token: 0x04000B5E RID: 2910
	private BeaconSettings _parent;

	// Token: 0x04000B5F RID: 2911
	private IconData _icon;

	// Token: 0x04000B60 RID: 2912
	public Image icon;

	// Token: 0x04000B61 RID: 2913
	public Image background;
}
