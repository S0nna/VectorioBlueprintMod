using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000D6 RID: 214
public class StorageData : ResourceComponentData<Storage>
{
	// Token: 0x060006F9 RID: 1785 RVA: 0x0002088B File Offset: 0x0001EA8B
	public override void ApplyData(Storage component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00020894 File Offset: 0x0001EA94
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatInt(this.storage, StatType.StorageCapacity)
		};
	}

	// Token: 0x040004A4 RID: 1188
	public bool globalStorage = true;
}
