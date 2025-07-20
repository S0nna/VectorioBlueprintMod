using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000B5 RID: 181
public class GatewayData : ComponentData<Gateway>
{
	// Token: 0x06000697 RID: 1687 RVA: 0x0001FE68 File Offset: 0x0001E068
	public override void ApplyData(Gateway component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x0400040A RID: 1034
	public RegionData region;

	// Token: 0x0400040B RID: 1035
	public ParticleSystem particle;
}
