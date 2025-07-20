using System;
using TMPro;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class CosmeticButton : MonoBehaviour
{
	// Token: 0x06000AC8 RID: 2760 RVA: 0x00003212 File Offset: 0x00001412
	public void OnPress()
	{
	}

	// Token: 0x040006E7 RID: 1767
	public GameObject unlocked;

	// Token: 0x040006E8 RID: 1768
	public GameObject locked;

	// Token: 0x040006E9 RID: 1769
	public TextMeshProUGUI title;

	// Token: 0x040006EA RID: 1770
	public TextMeshProUGUI description;

	// Token: 0x040006EB RID: 1771
	public TextMeshProUGUI lockCondition;

	// Token: 0x040006EC RID: 1772
	public Transform modelParent;

	// Token: 0x040006ED RID: 1773
	protected GameObject _model;

	// Token: 0x040006EE RID: 1774
	protected bool isUnlocked;
}
