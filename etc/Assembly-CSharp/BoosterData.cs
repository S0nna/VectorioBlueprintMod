using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000A2 RID: 162
public class BoosterData : ComponentData<Booster>
{
	// Token: 0x06000656 RID: 1622 RVA: 0x0001F8D5 File Offset: 0x0001DAD5
	public override void ApplyData(Booster component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0001F8DE File Offset: 0x0001DADE
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.modifier, StatType.BoosterEffect),
			new StatInt(this.range, StatType.BoosterRange)
		};
	}

	// Token: 0x040003CD RID: 973
	public string variableID = "";

	// Token: 0x040003CE RID: 974
	public float modifier = 1f;

	// Token: 0x040003CF RID: 975
	public int range;

	// Token: 0x040003D0 RID: 976
	public bool isMultiplier;
}
