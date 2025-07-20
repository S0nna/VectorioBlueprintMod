using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000AF RID: 175
public class DroneData : ComponentData<CargoDrone>
{
	// Token: 0x0600067F RID: 1663 RVA: 0x0001FB07 File Offset: 0x0001DD07
	public override void ApplyData(CargoDrone component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0001FB10 File Offset: 0x0001DD10
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatInt(this.maxActions, StatType.DroneMaxActions),
			new StatFloat(this.speed, StatType.DroneSpeed),
			new StatInt(this.storage, StatType.DroneCapacity)
		};
	}

	// Token: 0x040003EE RID: 1006
	public int maxActions;

	// Token: 0x040003EF RID: 1007
	public int storage;

	// Token: 0x040003F0 RID: 1008
	public float speed;

	// Token: 0x040003F1 RID: 1009
	public bool playAnimation;
}
