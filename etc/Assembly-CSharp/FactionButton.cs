using System;
using TMPro;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001B8 RID: 440
public class FactionButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000E13 RID: 3603 RVA: 0x0003ED3C File Offset: 0x0003CF3C
	public override void OnClick()
	{
		this._switch = !this._switch;
		FactionData factionData = this._switch ? this.factionTwo : this.factionOne;
		this.switchTitle.color = factionData.accent.primaryColor;
		this.border.color = factionData.accent.primaryColor;
		this.background.color = factionData.accent.secondaryColor;
		this.iconBackground.color = factionData.accent.primaryColor;
		this.icon.color = factionData.accent.secondaryColor;
		this.factionName.text = factionData.Name.ToUpper() + " FACTION";
		Singleton<Events>.Instance.onChangeFaction.Invoke(factionData);
	}

	// Token: 0x04000AD3 RID: 2771
	public FactionData factionOne;

	// Token: 0x04000AD4 RID: 2772
	public FactionData factionTwo;

	// Token: 0x04000AD5 RID: 2773
	public Image border;

	// Token: 0x04000AD6 RID: 2774
	public Image background;

	// Token: 0x04000AD7 RID: 2775
	public Image iconBackground;

	// Token: 0x04000AD8 RID: 2776
	public Image icon;

	// Token: 0x04000AD9 RID: 2777
	public TextMeshProUGUI switchTitle;

	// Token: 0x04000ADA RID: 2778
	public TextMeshProUGUI factionName;

	// Token: 0x04000ADB RID: 2779
	private bool _switch;
}
