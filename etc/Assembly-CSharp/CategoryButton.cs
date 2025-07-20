using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x02000147 RID: 327
public class CategoryButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000AC3 RID: 2755 RVA: 0x0002D510 File Offset: 0x0002B710
	public void Setup(Inventory.CategoryButtonData data, bool alternate)
	{
		this._category = data.category;
		this.icon.sprite = data.icon;
		this.title.text = data.title;
		this.background.color = (alternate ? this.colorOne : this.colorTwo);
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0002D567 File Offset: 0x0002B767
	public override void OnClick()
	{
		Singleton<Events>.Instance.onCategorySelected.Invoke((int)this._category);
		base.OnClick();
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0002D584 File Offset: 0x0002B784
	public void SetOriginalColor(Color color)
	{
		this._color = color;
		this.background.color = color;
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0002D599 File Offset: 0x0002B799
	public void ResetImageColor()
	{
		this.background.color = this._color;
	}

	// Token: 0x040006DF RID: 1759
	public Image background;

	// Token: 0x040006E0 RID: 1760
	public Image icon;

	// Token: 0x040006E1 RID: 1761
	public TextMeshProUGUI title;

	// Token: 0x040006E2 RID: 1762
	public TextMeshProUGUI unlocked;

	// Token: 0x040006E3 RID: 1763
	public Color colorOne;

	// Token: 0x040006E4 RID: 1764
	public Color colorTwo;

	// Token: 0x040006E5 RID: 1765
	private Category _category;

	// Token: 0x040006E6 RID: 1766
	private Color _color;
}
