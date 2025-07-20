using System;
using TMPro;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class SectorText : MonoBehaviour
{
	// Token: 0x06000F48 RID: 3912 RVA: 0x00047B6F File Offset: 0x00045D6F
	public void Set(string title, string desc)
	{
		if (title != null)
		{
			this.title.text = title.ToUpper();
		}
		if (desc != null)
		{
			this.desc.text = desc.ToUpper();
		}
	}

	// Token: 0x04000C97 RID: 3223
	public TextMeshPro title;

	// Token: 0x04000C98 RID: 3224
	public TextMeshPro desc;
}
