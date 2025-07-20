using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000151 RID: 337
public class InventoryPanel : MonoBehaviour
{
	// Token: 0x06000B03 RID: 2819 RVA: 0x0002F03C File Offset: 0x0002D23C
	public void Set(EntityData data, string model_id = "default")
	{
		this._entityData = data;
		if (this._stats != null && this._stats.Count > 0)
		{
			foreach (StatUI statUI in this._stats)
			{
				statUI.gameObject.SetActive(false);
			}
		}
		List<Color> list = new List<Color>();
		Color accentColor = this._entityData.model.GetAccentColor(AccentType.PrimaryColor);
		for (float num = 0.45f; num < 0.5f; num += 0.01f)
		{
			list.Add(new Color(accentColor.r * num, accentColor.g * num, accentColor.b * num, 1f));
		}
		this.title.text = this._entityData.Name.ToUpper();
		this.description.text = this._entityData.Description;
		if (this._entityData.UseNormalCost)
		{
			this.normalResourceIcon.sprite = this._entityData.NormalCost.resource.IconSprite;
			this.normalResourceIcon.gameObject.SetActive(true);
			this.normalResourceAmount.text = this._entityData.NormalCost.amount.ToString();
		}
		else
		{
			this.normalResourceIcon.gameObject.SetActive(false);
			this.normalResourceAmount.text = "";
		}
		if (this._entityData.UseSpecialCost)
		{
			this.specialResourceIcon.sprite = this._entityData.SpecialCost.resource.IconSprite;
			this.specialResourceIcon.gameObject.SetActive(true);
			this.specialResourceAmount.text = this._entityData.SpecialCost.amount.ToString();
		}
		else
		{
			this.specialResourceIcon.gameObject.SetActive(false);
			this.specialResourceAmount.text = "";
		}
		this.entityPreview.SetEntity(this._entityData);
		this.statList.Set(this._entityData);
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0002F25C File Offset: 0x0002D45C
	public void Toggle(bool toggle)
	{
		this._canvasGroup.alpha = (toggle ? 1f : 0f);
		this._canvasGroup.interactable = toggle;
		this._canvasGroup.blocksRaycasts = toggle;
	}

	// Token: 0x04000743 RID: 1859
	public Transform statParent;

	// Token: 0x04000744 RID: 1860
	public Image normalResourceIcon;

	// Token: 0x04000745 RID: 1861
	public Image specialResourceIcon;

	// Token: 0x04000746 RID: 1862
	public TextMeshProUGUI title;

	// Token: 0x04000747 RID: 1863
	public TextMeshProUGUI description;

	// Token: 0x04000748 RID: 1864
	public TextMeshProUGUI normalResourceAmount;

	// Token: 0x04000749 RID: 1865
	public TextMeshProUGUI specialResourceAmount;

	// Token: 0x0400074A RID: 1866
	public PreviewEntity entityPreview;

	// Token: 0x0400074B RID: 1867
	public StatList statList;

	// Token: 0x0400074C RID: 1868
	protected EntityData _entityData;

	// Token: 0x0400074D RID: 1869
	public StatUI statPrefab;

	// Token: 0x0400074E RID: 1870
	protected List<StatUI> _stats;

	// Token: 0x0400074F RID: 1871
	[SerializeField]
	protected CanvasGroup _canvasGroup;
}
