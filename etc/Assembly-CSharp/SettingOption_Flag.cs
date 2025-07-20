using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Settings;
using Vectorio.Settings.Data;

// Token: 0x0200018F RID: 399
public class SettingOption_Flag : SettingOption_Base
{
	// Token: 0x06000D6B RID: 3435 RVA: 0x0003AC68 File Offset: 0x00038E68
	public void Setup(FlagSettingData data, Color color)
	{
		this._data = data;
		FlagSetting flagSetting;
		if (Singleton<Settings>.Instance.TryGetSetting<FlagSetting>(data.ID, out flagSetting))
		{
			this.switchManager.isOn = flagSetting.Value;
			this.switchManager.UpdateUI();
			this.status.text = (flagSetting.Value ? "Enabled" : "Disabled");
		}
		else
		{
			this.switchManager.isOn = data.defaultValue;
			this.status.text = (data.defaultValue ? "Enabled" : "Disabled");
		}
		this.switchManager.OnEvents.AddListener(new UnityAction(this.OnTrue));
		this.switchManager.OffEvents.AddListener(new UnityAction(this.OnFalse));
		base.SetInfo(data, color);
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x0003AD3D File Offset: 0x00038F3D
	public void OnTrue()
	{
		Singleton<Settings>.Instance.AddSetting<FlagSetting>(this._data.ID, new FlagSetting
		{
			Value = true
		});
		this.status.text = "Enabled";
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x0003AD70 File Offset: 0x00038F70
	public void OnFalse()
	{
		Singleton<Settings>.Instance.AddSetting<FlagSetting>(this._data.ID, new FlagSetting
		{
			Value = false
		});
		this.status.text = "Disabled";
	}

	// Token: 0x04000984 RID: 2436
	private FlagSettingData _data;

	// Token: 0x04000985 RID: 2437
	public SwitchManager switchManager;

	// Token: 0x04000986 RID: 2438
	public TextMeshProUGUI status;
}
