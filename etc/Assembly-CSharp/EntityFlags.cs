using System;

// Token: 0x0200020F RID: 527
[Flags]
public enum EntityFlags
{
	// Token: 0x04000D78 RID: 3448
	IsDead = 1,
	// Token: 0x04000D79 RID: 3449
	IsEditable = 2,
	// Token: 0x04000D7A RID: 3450
	IsTargetable = 4,
	// Token: 0x04000D7B RID: 3451
	IsWorldFeature = 8,
	// Token: 0x04000D7C RID: 3452
	IsCostsApplied = 16,
	// Token: 0x04000D7D RID: 3453
	IsFreeEntity = 32,
	// Token: 0x04000D7E RID: 3454
	IsInvincible = 64,
	// Token: 0x04000D7F RID: 3455
	IsBlueprint = 128,
	// Token: 0x04000D80 RID: 3456
	IsProducingPower = 256,
	// Token: 0x04000D81 RID: 3457
	IsCloaked = 512
}
