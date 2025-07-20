using System;

// Token: 0x0200020E RID: 526
[Serializable]
public class EnemySpawn
{
	// Token: 0x04000D71 RID: 3441
	public EntityData unit;

	// Token: 0x04000D72 RID: 3442
	public int heatLevel;

	// Token: 0x04000D73 RID: 3443
	public float baseCooldown = 10f;

	// Token: 0x04000D74 RID: 3444
	public int reductionInterval = 10;

	// Token: 0x04000D75 RID: 3445
	public float reductionMultiplier = 0.9f;

	// Token: 0x04000D76 RID: 3446
	public float maxCooldown = 0.5f;
}
