using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000B7 RID: 183
public class HealthData : ComponentData<HealthComponent>
{
	// Token: 0x0600069D RID: 1693 RVA: 0x0001FEA5 File Offset: 0x0001E0A5
	public override void ApplyData(HealthComponent component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0001FEAE File Offset: 0x0001E0AE
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.health, this.type)
		};
	}

	// Token: 0x0400040E RID: 1038
	public float health;

	// Token: 0x0400040F RID: 1039
	public StatType type = StatType.BuildingHealth;
}
