using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000BC RID: 188
public class IlluminatorData : ComponentData<Illuminator>
{
	// Token: 0x060006A8 RID: 1704 RVA: 0x0001FFDA File Offset: 0x0001E1DA
	public override void ApplyData(Illuminator component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0001FFE3 File Offset: 0x0001E1E3
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.viewRadius, StatType.LightRadius),
			new StatFloat(this.viewAngle, StatType.LightAngle),
			new StatFloat(this.viewAngle, StatType.LightOpacity)
		};
	}

	// Token: 0x0400041A RID: 1050
	public bool rotates;

	// Token: 0x0400041B RID: 1051
	public float rotateSpeed;

	// Token: 0x0400041C RID: 1052
	public float viewRadius = 15f;

	// Token: 0x0400041D RID: 1053
	public float softenDistance = 3f;

	// Token: 0x0400041E RID: 1054
	[Range(0f, 360f)]
	public float viewAngle = 360f;

	// Token: 0x0400041F RID: 1055
	[Range(0f, 1f)]
	public float opacity = 1f;
}
