using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000A1 RID: 161
public class BeaconData : ComponentData<Beacon>
{
	// Token: 0x06000653 RID: 1619 RVA: 0x0001F8BD File Offset: 0x0001DABD
	public override void ApplyData(Beacon component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x040003CA RID: 970
	public IconData defaultIcon;

	// Token: 0x040003CB RID: 971
	public string defaultText;

	// Token: 0x040003CC RID: 972
	public Vector2 defaultSize;
}
