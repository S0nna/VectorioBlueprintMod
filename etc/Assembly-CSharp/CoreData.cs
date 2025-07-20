using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000AB RID: 171
public class CoreData : ComponentData<OutpostCore>
{
	// Token: 0x06000673 RID: 1651 RVA: 0x0001FA33 File Offset: 0x0001DC33
	public override void ApplyData(OutpostCore component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x040003E3 RID: 995
	public Sprite markerIcon;

	// Token: 0x040003E4 RID: 996
	public string markerText;

	// Token: 0x040003E5 RID: 997
	public Sprite lineSprite;

	// Token: 0x040003E6 RID: 998
	public Material lineMaterial;

	// Token: 0x040003E7 RID: 999
	public int lineSortingOrder;
}
