using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000D3 RID: 211
public class ScytheData : ComponentData<Scythe>
{
	// Token: 0x060006F2 RID: 1778 RVA: 0x00020793 File Offset: 0x0001E993
	public override void ApplyData(Scythe component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002079C File Offset: 0x0001E99C
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatInt(this.damage, StatType.TurretDamage),
			new StatInt(this.range, StatType.TurretRange),
			new StatFloat(this.timeToCrest, StatType.ProjectileSpeed),
			new StatFloat(this.cooldown, StatType.TurretCooldown)
		};
	}

	// Token: 0x04000497 RID: 1175
	public int damage = 1;

	// Token: 0x04000498 RID: 1176
	public int range = 5;

	// Token: 0x04000499 RID: 1177
	public float timeToCrest = 1.5f;

	// Token: 0x0400049A RID: 1178
	public float cooldown = 1f;

	// Token: 0x0400049B RID: 1179
	public float rotationSpeed = 60f;

	// Token: 0x0400049C RID: 1180
	public float scytheSize = 2.5f;

	// Token: 0x0400049D RID: 1181
	public AudioClip hitSound;
}
