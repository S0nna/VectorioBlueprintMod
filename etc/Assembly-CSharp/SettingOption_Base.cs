using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.Settings.Data;

// Token: 0x0200018E RID: 398
public class SettingOption_Base : MonoBehaviour
{
	// Token: 0x06000D68 RID: 3432 RVA: 0x0003AC2C File Offset: 0x00038E2C
	public void SetInfo(BaseSettingData data, Color color)
	{
		this.background.color = color;
		this.title.text = data.Name;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0003AC4B File Offset: 0x00038E4B
	public void SetInfo(string titleText, Color color)
	{
		this.background.color = color;
		this.title.text = titleText;
	}

	// Token: 0x04000982 RID: 2434
	public Image background;

	// Token: 0x04000983 RID: 2435
	public TextMeshProUGUI title;
}
