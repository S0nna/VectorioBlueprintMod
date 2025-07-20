using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000C5 RID: 197
public class ReactorData : ComponentData<Reactor>
{
	// Token: 0x060006BA RID: 1722 RVA: 0x000201C5 File Offset: 0x0001E3C5
	public override void ApplyData(Reactor component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x04000439 RID: 1081
	public float iconSize;
}
