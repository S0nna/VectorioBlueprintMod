using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E5 RID: 485
public class ResourceAmount : MonoBehaviour
{
	// Token: 0x06000EFD RID: 3837 RVA: 0x00045D38 File Offset: 0x00043F38
	public void Set(string resourceID, int cur, int max = -1)
	{
		ResourceData resourceData = Library.RequestData<ResourceData>(resourceID);
		if (resourceData != null)
		{
			base.transform.name = resourceID;
			this.icon.sprite = resourceData.IconSprite;
			this.preview = (max == -1);
			this.amount.text = (this.preview ? cur.ToString() : (cur.ToString() + "/" + max.ToString()));
			if (this.progressBar != null)
			{
				this.progressBar.minValue = 0f;
				this.progressBar.currentPercent = (float)cur;
				this.progressBar.maxValue = (float)max;
				this.progressBar.UpdateUI();
			}
		}
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x00045DF8 File Offset: 0x00043FF8
	public void CustomUpdate(int cur, int max = -1)
	{
		this.preview = (max == -1);
		this.amount.text = (this.preview ? cur.ToString() : (cur.ToString() + "/" + max.ToString()));
		if (this.progressBar != null)
		{
			this.progressBar.currentPercent = (float)cur;
			this.progressBar.maxValue = (float)max;
			this.progressBar.UpdateUI();
		}
	}

	// Token: 0x04000C40 RID: 3136
	public Image icon;

	// Token: 0x04000C41 RID: 3137
	public TextMeshProUGUI amount;

	// Token: 0x04000C42 RID: 3138
	public ProgressBar progressBar;

	// Token: 0x04000C43 RID: 3139
	protected bool preview;
}
