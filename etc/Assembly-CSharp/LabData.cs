using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000BD RID: 189
public class LabData : ResourceComponentData<Lab>
{
	// Token: 0x060006AB RID: 1707 RVA: 0x00020057 File Offset: 0x0001E257
	public override void ApplyData(Lab component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00020060 File Offset: 0x0001E260
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.cooldown, StatType.LabSpeed),
			new StatInt(this.storage, StatType.LabCapacity)
		};
	}

	// Token: 0x04000420 RID: 1056
	public float cooldown;

	// Token: 0x04000421 RID: 1057
	public int value;

	// Token: 0x04000422 RID: 1058
	public EffectData effectData;

	// Token: 0x04000423 RID: 1059
	public AudioClip researchingSound;
}
