using System;
using System.Text.RegularExpressions;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.Stats;

// Token: 0x0200019F RID: 415
public class StatModifierUI : MonoBehaviour
{
	// Token: 0x06000DB5 RID: 3509 RVA: 0x0003CBB0 File Offset: 0x0003ADB0
	public void Setup(StatType statType, Color color, Action onValueUpdated)
	{
		this._onValueUpdated = onValueUpdated;
		this.title.text = Regex.Replace(statType.ToString(), "([a-z])([A-Z])", "$1 $2");
		this.background.color = color;
		this._allySlider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueUpdated));
		this._enemySlider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueUpdated));
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x0003CC2F File Offset: 0x0003AE2F
	public void SetAllyValue(float value)
	{
		this._allySlider.mainSlider.value = value;
		this._allySlider.UpdateUI();
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0003CC4D File Offset: 0x0003AE4D
	public void SetEnemyValue(float value)
	{
		this._enemySlider.mainSlider.value = value;
		this._enemySlider.UpdateUI();
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0003CC6B File Offset: 0x0003AE6B
	public void OnValueUpdated(float value)
	{
		Action onValueUpdated = this._onValueUpdated;
		if (onValueUpdated == null)
		{
			return;
		}
		onValueUpdated();
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0003CC80 File Offset: 0x0003AE80
	public Tuple<float?, float?> GetModifiers()
	{
		float? item = null;
		float? item2 = null;
		float num = MathF.Round(this._allySlider.mainSlider.value, 1);
		if (num != 1f)
		{
			item = new float?(num);
		}
		num = MathF.Round(this._enemySlider.mainSlider.value, 1);
		if (num != 1f)
		{
			item2 = new float?(num);
		}
		return new Tuple<float?, float?>(item, item2);
	}

	// Token: 0x040009E1 RID: 2529
	private Action _onValueUpdated;

	// Token: 0x040009E2 RID: 2530
	public TextMeshProUGUI title;

	// Token: 0x040009E3 RID: 2531
	public Image background;

	// Token: 0x040009E4 RID: 2532
	public SliderManager _allySlider;

	// Token: 0x040009E5 RID: 2533
	public SliderManager _enemySlider;
}
