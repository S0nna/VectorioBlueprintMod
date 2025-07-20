using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000A6 RID: 166
public class ClaimerData : ComponentData<Claimer>
{
	// Token: 0x06000666 RID: 1638 RVA: 0x0001F9A4 File Offset: 0x0001DBA4
	public override void ApplyData(Claimer component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0001F9AD File Offset: 0x0001DBAD
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatInt(this.range, StatType.ReclaimerRange)
		};
	}

	// Token: 0x040003DE RID: 990
	public int range;
}
