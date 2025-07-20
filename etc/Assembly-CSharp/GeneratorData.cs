using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000B6 RID: 182
public class GeneratorData : ResourceComponentData<Generator>
{
	// Token: 0x0600069A RID: 1690 RVA: 0x0001FE79 File Offset: 0x0001E079
	public override void ApplyData(Generator component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0001FE82 File Offset: 0x0001E082
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat((float)this.storage, StatType.GeneratorCapacity)
		};
	}

	// Token: 0x0400040C RID: 1036
	public EffectData effect;

	// Token: 0x0400040D RID: 1037
	public int effectIndex;
}
