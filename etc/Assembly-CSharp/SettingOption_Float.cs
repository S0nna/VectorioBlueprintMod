using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using Vectorio.Formatting;
using Vectorio.Settings;
using Vectorio.Settings.Data;

// Token: 0x02000190 RID: 400
public class SettingOption_Float : SettingOption_Base
{
	// Token: 0x06000D6F RID: 3439 RVA: 0x0003ADAC File Offset: 0x00038FAC
	public void Setup(FloatSettingData data, Color color)
	{
		this._data = data;
		this.sliderManager.mainSlider.minValue = this._data.minValue;
		this.sliderManager.mainSlider.maxValue = this._data.maxValue;
		FloatSetting floatSetting;
		if (Singleton<Settings>.Instance.TryGetSetting<FloatSetting>(this._data.ID, out floatSetting))
		{
			float num = Mathf.Clamp(floatSetting.Value, this._data.minValue, this._data.maxValue);
			this.sliderManager.mainSlider.value = num;
			this.valueText.text = Formatter.Round(num, 1);
		}
		else
		{
			this.sliderManager.mainSlider.value = this._data.defaultValue;
			this.valueText.text = Formatter.Round(this._data.defaultValue, 1);
		}
		this.sliderManager.UpdateUI();
		this.sliderManager.mainSlider.onValueChanged.AddListener(delegate(float newValue)
		{
			float num2 = Mathf.Clamp(newValue, this._data.minValue, this._data.maxValue);
			Singleton<Settings>.Instance.AddSetting<FloatSetting>(this._data.ID, new FloatSetting
			{
				Value = num2
			});
			this.valueText.text = Formatter.Round(num2, 1);
		});
		base.SetInfo(this._data, color);
	}

	// Token: 0x04000987 RID: 2439
	private FloatSettingData _data;

	// Token: 0x04000988 RID: 2440
	public SliderManager sliderManager;

	// Token: 0x04000989 RID: 2441
	public TextMeshProUGUI valueText;
}
