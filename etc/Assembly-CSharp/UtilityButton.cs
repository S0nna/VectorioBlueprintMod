using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x02000205 RID: 517
public class UtilityButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000F91 RID: 3985 RVA: 0x00049AA0 File Offset: 0x00047CA0
	public void ToggleUtility(int utility)
	{
		this._isEnabled = !this._isEnabled;
		if (this._isEnabled)
		{
			this.icon.sprite = this.enabledSprite;
			this.border.color = this.enabledLightColor;
			this.background.color = this.enabledDarkColor;
		}
		else
		{
			this.icon.sprite = this.disabledSprite;
			this.border.color = this.disabledLightColor;
			this.background.color = this.disabledDarkColor;
		}
		switch (utility)
		{
		case 0:
			Singleton<LogisticsManager>.Instance.ToggleXray(this._isEnabled);
			return;
		case 1:
			Singleton<LogisticsManager>.Instance.ToggleAutoEdit(this._isEnabled);
			return;
		case 2:
			Singleton<LogisticsManager>.Instance.ToggleRoutePreview(this._isEnabled);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x00049B73 File Offset: 0x00047D73
	public override void ToggleHover(bool toggle)
	{
		this.title.gameObject.SetActive(toggle);
		base.ToggleHover(toggle);
	}

	// Token: 0x04000D48 RID: 3400
	private bool _isEnabled;

	// Token: 0x04000D49 RID: 3401
	public TextMeshProUGUI title;

	// Token: 0x04000D4A RID: 3402
	public Image icon;

	// Token: 0x04000D4B RID: 3403
	public Image border;

	// Token: 0x04000D4C RID: 3404
	public Image background;

	// Token: 0x04000D4D RID: 3405
	public Sprite enabledSprite;

	// Token: 0x04000D4E RID: 3406
	public Sprite disabledSprite;

	// Token: 0x04000D4F RID: 3407
	public Color enabledLightColor;

	// Token: 0x04000D50 RID: 3408
	public Color enabledDarkColor;

	// Token: 0x04000D51 RID: 3409
	public Color disabledLightColor;

	// Token: 0x04000D52 RID: 3410
	public Color disabledDarkColor;

	// Token: 0x02000206 RID: 518
	public enum UtilityType
	{
		// Token: 0x04000D54 RID: 3412
		Xray,
		// Token: 0x04000D55 RID: 3413
		AutoEdit,
		// Token: 0x04000D56 RID: 3414
		RoutePreview
	}
}
