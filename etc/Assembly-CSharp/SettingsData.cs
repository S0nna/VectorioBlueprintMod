using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Settings;

// Token: 0x02000199 RID: 409
[Serializable]
public class SettingsData : SerializableData
{
	// Token: 0x06000DA1 RID: 3489 RVA: 0x0003C220 File Offset: 0x0003A420
	public SettingsData()
	{
		Debug.Log("[SAVE] Creating new settings data");
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x0003C248 File Offset: 0x0003A448
	public void AddSetting<T>(string id, T setting) where T : BaseSetting
	{
		if (string.IsNullOrEmpty(id))
		{
			Debug.Log("[S-DATA] ID cannot be null or empty!");
			return;
		}
		if (setting == null)
		{
			Debug.Log("[S-DATA] The provided setting was null!");
			return;
		}
		if (this.settings.ContainsKey(id))
		{
			this.settings[id] = setting;
			return;
		}
		this.settings.Add(id, setting);
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x0003C2B0 File Offset: 0x0003A4B0
	public bool TryGetSetting<T>(string id, out T typedSetting) where T : BaseSetting
	{
		BaseSetting baseSetting;
		if (this.settings.TryGetValue(id, out baseSetting))
		{
			T t = baseSetting as T;
			if (t != null)
			{
				typedSetting = t;
				return true;
			}
		}
		typedSetting = default(T);
		return false;
	}

	// Token: 0x040009AD RID: 2477
	public Dictionary<string, BaseSetting> settings = new Dictionary<string, BaseSetting>();

	// Token: 0x040009AE RID: 2478
	public Dictionary<string, string> keybinds = new Dictionary<string, string>();
}
