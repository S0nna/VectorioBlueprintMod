using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001BD RID: 445
public class HeatLevelUI : MonoBehaviour
{
	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0003F556 File Offset: 0x0003D756
	public Color GetLineColor
	{
		get
		{
			if (!this.bottomLine.gameObject.activeSelf)
			{
				return Color.white;
			}
			return this.bottomLine.color;
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0003F57B File Offset: 0x0003D77B
	public int GetLevel
	{
		get
		{
			return this._level;
		}
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0003F584 File Offset: 0x0003D784
	public void Setup(EnemySpawn spawn, int index, bool shown, bool previousShown, Color previousColor)
	{
		this._level = index;
		this.heatLevelText.text = "HEAT LEVEL " + this._level.ToString();
		this.topLine.gameObject.SetActive(previousShown);
		this._isToggled = !shown;
		this.Toggle(shown, previousColor);
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0003F5E0 File Offset: 0x0003D7E0
	public void Toggle(bool toggle, Color previousColor)
	{
		if (this._isToggled == toggle)
		{
			return;
		}
		this._isToggled = toggle;
		if (toggle)
		{
			this.heatIcon.color = this.normalColor;
			this.heatLevelText.color = Color.white;
			this.heatAmountText.color = this.heatColor;
			this.bottomLine.color = this.normalColor;
			this.topLine.color = previousColor;
			return;
		}
		this.heatIcon.color = this.hiddenColor;
		this.heatLevelText.color = this.hiddenColor;
		this.heatAmountText.color = this.hiddenColor;
		this.bottomLine.color = this.hiddenColor;
		this.topLine.color = previousColor;
	}

	// Token: 0x04000B01 RID: 2817
	public TextMeshProUGUI heatLevelText;

	// Token: 0x04000B02 RID: 2818
	public TextMeshProUGUI heatAmountText;

	// Token: 0x04000B03 RID: 2819
	public Image heatIcon;

	// Token: 0x04000B04 RID: 2820
	public Image topLine;

	// Token: 0x04000B05 RID: 2821
	public Image bottomLine;

	// Token: 0x04000B06 RID: 2822
	public Color normalColor;

	// Token: 0x04000B07 RID: 2823
	public Color heatColor;

	// Token: 0x04000B08 RID: 2824
	public Color hiddenColor;

	// Token: 0x04000B09 RID: 2825
	protected int _level;

	// Token: 0x04000B0A RID: 2826
	protected bool _isToggled;
}
