using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.Stats;

// Token: 0x020001FA RID: 506
public class StoredResourceInfo : MonoBehaviour
{
	// Token: 0x06000F62 RID: 3938 RVA: 0x00048B4C File Offset: 0x00046D4C
	public void Set(StoredResource resource, ref StatInt storage)
	{
		this._target = resource;
		this._storage = storage;
		this.title.text = resource.ResourceData.Name.ToUpper();
		this.icon.sprite = resource.ResourceData.IconSprite;
		this.barImage.color = resource.ResourceData.Accent.primaryColor;
		this.barBackground.color = resource.ResourceData.Accent.secondaryColor;
		this.progressBar.minValue = 0f;
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00048BE0 File Offset: 0x00046DE0
	public void CustomUpdate()
	{
		this.amount.text = this._target.AmountStored.ToString() + "/" + this._storage.Value.ToString();
		this.progressBar.currentPercent = (float)this._target.AmountStored;
		this.progressBar.maxValue = (float)this._storage.Value;
		this.progressBar.UpdateUI();
	}

	// Token: 0x04000CF3 RID: 3315
	public TextMeshProUGUI title;

	// Token: 0x04000CF4 RID: 3316
	public TextMeshProUGUI amount;

	// Token: 0x04000CF5 RID: 3317
	public Image icon;

	// Token: 0x04000CF6 RID: 3318
	public Image barImage;

	// Token: 0x04000CF7 RID: 3319
	public Image barBackground;

	// Token: 0x04000CF8 RID: 3320
	public ProgressBar progressBar;

	// Token: 0x04000CF9 RID: 3321
	private StoredResource _target;

	// Token: 0x04000CFA RID: 3322
	private StatInt _storage;
}
