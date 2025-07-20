using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000B4 RID: 180
public class FilterData : ResourceComponentData<Filter>
{
	// Token: 0x06000694 RID: 1684 RVA: 0x0001FE57 File Offset: 0x0001E057
	public override void ApplyData(Filter component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}
}
