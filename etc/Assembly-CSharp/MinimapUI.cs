using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D5 RID: 469
[DefaultExecutionOrder(0)]
public class MinimapUI : Singleton<MinimapUI>
{
	// Token: 0x06000EA1 RID: 3745 RVA: 0x0004208C File Offset: 0x0004028C
	public void Setup(RegionData data)
	{
		if (data != null)
		{
			Debug.Log("[MAP] Setting minimap coloring for " + data.Name);
			this.title.text = data.minimapBorderText;
			this.border.color = data.minimapBorderColor;
			this.background.color = data.minimapBorderColor;
			this.title.color = data.minimaptitleColor;
			return;
		}
		Debug.Log("[MAP] Could not load region data!");
	}

	// Token: 0x04000BA4 RID: 2980
	public TextMeshProUGUI title;

	// Token: 0x04000BA5 RID: 2981
	public Image border;

	// Token: 0x04000BA6 RID: 2982
	public Image background;
}
