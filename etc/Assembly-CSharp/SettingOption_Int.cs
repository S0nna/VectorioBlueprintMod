using System;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Settings;
using Vectorio.Settings.Data;

// Token: 0x02000192 RID: 402
public class SettingOption_Int : SettingOption_Base
{
	// Token: 0x06000D74 RID: 3444 RVA: 0x0003AF30 File Offset: 0x00039130
	public void Setup(IntSettingData data, Color color)
	{
		this._data = data;
		int num = -1;
		IntSetting intSetting;
		if (Singleton<Settings>.Instance.TryGetSetting<IntSetting>(data.ID, out intSetting))
		{
			num = intSetting.Value;
		}
		int index = 0;
		int num2 = 0;
		foreach (IntSettingData.Option option in data.options)
		{
			this.horizontalSelector.CreateNewItem(option.optionText);
			this.horizontalSelector.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
			this._valueMap.Add(num2, option.optionValue);
			if (num != -1 && num == option.optionValue)
			{
				index = num2;
			}
			num2++;
		}
		this.horizontalSelector.index = index;
		this.horizontalSelector.UpdateUI();
		base.SetInfo(data, color);
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0003B01C File Offset: 0x0003921C
	public void OnValueChanged(int value)
	{
		if (this._valueMap.ContainsKey(value))
		{
			Singleton<Settings>.Instance.AddSetting<IntSetting>(this._data.ID, new IntSetting
			{
				Value = this._valueMap[value]
			});
		}
	}

	// Token: 0x0400098A RID: 2442
	private IntSettingData _data;

	// Token: 0x0400098B RID: 2443
	public HorizontalSelector horizontalSelector;

	// Token: 0x0400098C RID: 2444
	private Dictionary<int, int> _valueMap = new Dictionary<int, int>();
}
