using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class ParticleManager : Singleton<ParticleManager>
{
	// Token: 0x040002F1 RID: 753
	protected Dictionary<string, ParticleSystem> _particlePool;

	// Token: 0x02000070 RID: 112
	public class Particle
	{
		// Token: 0x040002F2 RID: 754
		public string id;

		// Token: 0x040002F3 RID: 755
		public ParticleSystem instance;
	}
}
