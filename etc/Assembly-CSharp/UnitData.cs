using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000DC RID: 220
public class UnitData : ComponentData<Unit>
{
	// Token: 0x06000707 RID: 1799 RVA: 0x00020A80 File Offset: 0x0001EC80
	public override void ApplyData(Unit component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00020A8C File Offset: 0x0001EC8C
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.damage, StatType.UnitDamage),
			new StatFloat(this.moveSpeed, StatType.UnitSpeed),
			new StatFloat((float)this.range, StatType.UnitRange),
			new StatFloat(this.physicalMass, StatType.UnitMass)
		};
	}

	// Token: 0x040004C3 RID: 1219
	public float damage = 1f;

	// Token: 0x040004C4 RID: 1220
	public float moveSpeed = 10f;

	// Token: 0x040004C5 RID: 1221
	public float rotateSpeed = 120f;

	// Token: 0x040004C6 RID: 1222
	public bool useUniqueAnimations;

	// Token: 0x040004C7 RID: 1223
	public ParticleSystem deathParticle;

	// Token: 0x040004C8 RID: 1224
	public AudioClip deathSound;

	// Token: 0x040004C9 RID: 1225
	public bool useAdvancedSettings;

	// Token: 0x040004CA RID: 1226
	public int range = 10;

	// Token: 0x040004CB RID: 1227
	public float colliderSize = 1.5f;

	// Token: 0x040004CC RID: 1228
	public float physicalMass = 1f;

	// Token: 0x040004CD RID: 1229
	public float physicalDrag = 1f;

	// Token: 0x040004CE RID: 1230
	public float knockbackValue = 100f;

	// Token: 0x040004CF RID: 1231
	public Sprite minimapIcon;

	// Token: 0x040004D0 RID: 1232
	public float minimapIconSize = 1f;
}
