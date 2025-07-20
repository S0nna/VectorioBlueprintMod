using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000A3 RID: 163
public class BuilderDroneData : ComponentData<BuilderDrone>
{
	// Token: 0x06000659 RID: 1625 RVA: 0x0001F929 File Offset: 0x0001DB29
	public override void ApplyData(BuilderDrone component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0001F932 File Offset: 0x0001DB32
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.speed, StatType.DroneSpeed)
		};
	}

	// Token: 0x040003D1 RID: 977
	public float speed;
}
