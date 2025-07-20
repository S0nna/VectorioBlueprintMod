using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000A5 RID: 165
public class BulletData : ComponentData<Bullet>
{
	// Token: 0x06000663 RID: 1635 RVA: 0x0001F98C File Offset: 0x0001DB8C
	public override void ApplyData(Bullet component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x040003DA RID: 986
	public AudioClip sound;

	// Token: 0x040003DB RID: 987
	public bool useParticle = true;

	// Token: 0x040003DC RID: 988
	public ParticleSystem particle;

	// Token: 0x040003DD RID: 989
	public ParticleInfo.ColoringMode coloringType;
}
