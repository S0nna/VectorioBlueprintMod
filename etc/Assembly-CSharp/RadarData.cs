using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000C4 RID: 196
public class RadarData : ComponentData<Radar>
{
	// Token: 0x060006B7 RID: 1719 RVA: 0x0002019A File Offset: 0x0001E39A
	public override void ApplyData(Radar component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x000201A3 File Offset: 0x0001E3A3
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.range, StatType.RadarRange)
		};
	}

	// Token: 0x04000437 RID: 1079
	public float range;

	// Token: 0x04000438 RID: 1080
	public float rotationSpeed;
}
