using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000D7 RID: 215
public class TestLabData : ResourceComponentData<TestLab>
{
	// Token: 0x060006FC RID: 1788 RVA: 0x000208BD File Offset: 0x0001EABD
	public override void ApplyData(TestLab component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x000208C6 File Offset: 0x0001EAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.cooldown, StatType.LabSpeed),
			new StatInt(this.storage, StatType.LabCapacity)
		};
	}

	// Token: 0x040004A5 RID: 1189
	public float cooldown;

	// Token: 0x040004A6 RID: 1190
	public int value;

	// Token: 0x040004A7 RID: 1191
	public EffectData effectData;

	// Token: 0x040004A8 RID: 1192
	public AudioClip researchingSound;
}
