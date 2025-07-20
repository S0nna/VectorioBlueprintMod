using System;
using TMPro;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001DE RID: 478
public class PresetButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000ED9 RID: 3801 RVA: 0x0004464C File Offset: 0x0004284C
	public void Setup(PresetData presetData, NewWorldPanel panel)
	{
		this._presetData = presetData;
		this._panel = panel;
		this.icon.sprite = presetData.icon;
		this.title.text = presetData.Name;
		this.description.text = presetData.Description;
		this.background.color = presetData.backgroundColor;
		this.glowTop.color = presetData.glowColor;
		this.glowBottom.color = presetData.glowColor;
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x000446CD File Offset: 0x000428CD
	public override void OnClick()
	{
		this._panel.SetPreset(this._presetData);
		base.OnClick();
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x000446E6 File Offset: 0x000428E6
	public PresetData Preset()
	{
		return this._presetData;
	}

	// Token: 0x04000BDE RID: 3038
	private NewWorldPanel _panel;

	// Token: 0x04000BDF RID: 3039
	private PresetData _presetData;

	// Token: 0x04000BE0 RID: 3040
	public TextMeshProUGUI title;

	// Token: 0x04000BE1 RID: 3041
	public TextMeshProUGUI description;

	// Token: 0x04000BE2 RID: 3042
	public Image icon;

	// Token: 0x04000BE3 RID: 3043
	public Image background;

	// Token: 0x04000BE4 RID: 3044
	public Image glowTop;

	// Token: 0x04000BE5 RID: 3045
	public Image glowBottom;
}
