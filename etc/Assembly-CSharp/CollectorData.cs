using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000A7 RID: 167
public class CollectorData : ResourceComponentData<Collector>
{
	// Token: 0x06000669 RID: 1641 RVA: 0x0001F9CF File Offset: 0x0001DBCF
	public override void ApplyData(Collector component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0001F9D8 File Offset: 0x0001DBD8
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.cooldown, StatType.CollectorSpeed),
			new StatInt(this.storage, StatType.CollectorCapacity)
		};
	}

	// Token: 0x040003DF RID: 991
	public float cooldown;

	// Token: 0x040003E0 RID: 992
	public int value;

	// Token: 0x040003E1 RID: 993
	public bool useCollectSound;

	// Token: 0x040003E2 RID: 994
	public AudioClip collectSound;
}
