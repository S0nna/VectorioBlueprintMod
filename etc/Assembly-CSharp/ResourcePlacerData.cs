using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000D2 RID: 210
public class ResourcePlacerData : ComponentData<ResourcePlacer>
{
	// Token: 0x060006EF RID: 1775 RVA: 0x00020782 File Offset: 0x0001E982
	public override void ApplyData(ResourcePlacer component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x04000496 RID: 1174
	public ResourceData resource;
}
