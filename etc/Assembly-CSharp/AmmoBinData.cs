using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x0200009F RID: 159
public class AmmoBinData : ResourceComponentData<AmmoBin>
{
	// Token: 0x06000648 RID: 1608 RVA: 0x0001F7D4 File Offset: 0x0001D9D4
	public override void ApplyData(AmmoBin component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0001F7DD File Offset: 0x0001D9DD
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat((float)this.storage, StatType.GeneratorCapacity)
		};
	}

	// Token: 0x040003C1 RID: 961
	public ResourceData ammoRequired;

	// Token: 0x040003C2 RID: 962
	public float ammoIconSize = 1f;
}
