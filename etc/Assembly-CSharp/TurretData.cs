using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000DA RID: 218
public class TurretData : ComponentData<Turret>
{
	// Token: 0x06000703 RID: 1795 RVA: 0x0002090C File Offset: 0x0001EB0C
	public override void ApplyData(Turret component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00020918 File Offset: 0x0001EB18
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.damage, StatType.TurretDamage),
			new StatFloat(this.cooldown, StatType.TurretCooldown),
			new StatFloat(this.range, StatType.TurretRange),
			new StatFloat(this.bulletSpread, StatType.TurretSpread),
			new StatInt(this.bulletPierces, StatType.ProjectilePierces),
			new StatInt(this.bulletAmount, StatType.ProjectileAmount),
			new StatFloat(this.bulletSpeed, StatType.ProjectileSpeed),
			new StatFloat(this.bulletLifetime, StatType.ProjectileLifetime),
			new StatFloat(this.rotationSpeed, StatType.BarrelSpeed)
		};
	}

	// Token: 0x040004AE RID: 1198
	public float damage = 1f;

	// Token: 0x040004AF RID: 1199
	public float cooldown = 0.7f;

	// Token: 0x040004B0 RID: 1200
	public float range = 10f;

	// Token: 0x040004B1 RID: 1201
	public bool explosive;

	// Token: 0x040004B2 RID: 1202
	public AudioClip shootSound;

	// Token: 0x040004B3 RID: 1203
	public float recoilAmplitude = 0.5f;

	// Token: 0x040004B4 RID: 1204
	public float recoilFrequency = 15f;

	// Token: 0x040004B5 RID: 1205
	public float rotationSpeed = 50f;

	// Token: 0x040004B6 RID: 1206
	public float xFirePosition;

	// Token: 0x040004B7 RID: 1207
	public float yFirePosition = 2.5f;

	// Token: 0x040004B8 RID: 1208
	public bool allowPredictiveAiming = true;

	// Token: 0x040004B9 RID: 1209
	public EntityData bullet;

	// Token: 0x040004BA RID: 1210
	public Accent bulletAccent;

	// Token: 0x040004BB RID: 1211
	public float bulletSpeed = 95f;

	// Token: 0x040004BC RID: 1212
	public float bulletSpeedOffset = 5f;

	// Token: 0x040004BD RID: 1213
	public float bulletLifetime = 1f;

	// Token: 0x040004BE RID: 1214
	public float bulletLifetimeOffset = 0.2f;

	// Token: 0x040004BF RID: 1215
	public float bulletSpread = 3f;

	// Token: 0x040004C0 RID: 1216
	public float bulletSize = 2f;

	// Token: 0x040004C1 RID: 1217
	public int bulletAmount = 1;

	// Token: 0x040004C2 RID: 1218
	public int bulletPierces;
}
