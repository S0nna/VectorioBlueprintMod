using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000C7 RID: 199
public class RegionBoosterData : ComponentData<RegionBooster>
{
	// Token: 0x060006BE RID: 1726 RVA: 0x000201D6 File Offset: 0x0001E3D6
	public override void ApplyData(RegionBooster component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x000201DF File Offset: 0x0001E3DF
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.modifier, StatType.BoosterEffect),
			new StatFloat((float)this.range, StatType.BoosterRange)
		};
	}

	// Token: 0x0400043D RID: 1085
	public string variableID = "";

	// Token: 0x0400043E RID: 1086
	public float modifier = 1f;

	// Token: 0x0400043F RID: 1087
	public int range;

	// Token: 0x04000440 RID: 1088
	public bool isMultiplier;

	// Token: 0x04000441 RID: 1089
	public Sprite markerIcon;

	// Token: 0x04000442 RID: 1090
	public GameObject boostAreaPrefab;
}
