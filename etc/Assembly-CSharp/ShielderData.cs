using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000D4 RID: 212
public class ShielderData : ComponentData<Shielder>
{
	// Token: 0x060006F5 RID: 1781 RVA: 0x00020845 File Offset: 0x0001EA45
	public override void ApplyData(Shielder component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002084E File Offset: 0x0001EA4E
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.burnTime, StatType.ShielderBurnTime),
			new StatInt(this.range, StatType.ShielderRange)
		};
	}

	// Token: 0x0400049E RID: 1182
	public Sprite sprite;

	// Token: 0x0400049F RID: 1183
	public Material material;

	// Token: 0x040004A0 RID: 1184
	public AudioClip hitSound;

	// Token: 0x040004A1 RID: 1185
	public Color color;

	// Token: 0x040004A2 RID: 1186
	public int range;

	// Token: 0x040004A3 RID: 1187
	public float burnTime;
}
