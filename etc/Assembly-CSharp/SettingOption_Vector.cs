using System;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Settings;
using Vectorio.Settings.Data;

// Token: 0x02000194 RID: 404
public class SettingOption_Vector : SettingOption_Base
{
	// Token: 0x06000D7A RID: 3450 RVA: 0x0003B088 File Offset: 0x00039288
	public void Setup(VectorSettingData data, Color color)
	{
		this._data = data;
		int num = -1;
		int num2 = -1;
		VectorSetting vectorSetting;
		if (Singleton<Settings>.Instance.TryGetSetting<VectorSetting>(data.ID, out vectorSetting))
		{
			num = vectorSetting.ValueX;
			num2 = vectorSetting.ValueY;
		}
		int index = 0;
		int num3 = 0;
		foreach (VectorSettingData.Option option in data.options)
		{
			this.horizontalSelector.CreateNewItem(option.optionText);
			this.horizontalSelector.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
			this._valueMap.Add(num3, new Vector2Int(option.optionValueX, option.optionValueY));
			if (num != -1 && num == option.optionValueX && num2 == option.optionValueY)
			{
				index = num3;
			}
			num3++;
		}
		this.horizontalSelector.index = index;
		this.horizontalSelector.UpdateUI();
		base.SetInfo(data, color);
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x0003B19C File Offset: 0x0003939C
	public void OnValueChanged(int value)
	{
		if (this._valueMap.ContainsKey(value))
		{
			Singleton<Settings>.Instance.AddSetting<VectorSetting>(this._data.ID, new VectorSetting
			{
				ValueX = this._valueMap[value].x,
				ValueY = this._valueMap[value].y
			});
		}
	}

	// Token: 0x0400098E RID: 2446
	private VectorSettingData _data;

	// Token: 0x0400098F RID: 2447
	public HorizontalSelector horizontalSelector;

	// Token: 0x04000990 RID: 2448
	private Dictionary<int, Vector2Int> _valueMap = new Dictionary<int, Vector2Int>();
}
