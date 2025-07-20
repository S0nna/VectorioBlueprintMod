using System;

// Token: 0x0200013F RID: 319
[Flags]
[Serializable]
public enum GamemodeRules
{
	// Token: 0x04000698 RID: 1688
	AllowClientMovement = 2,
	// Token: 0x04000699 RID: 1689
	AllowFactionSwitching = 4,
	// Token: 0x0400069A RID: 1690
	AllowDeveloperTools = 8,
	// Token: 0x0400069B RID: 1691
	UseTileRestrictions = 16,
	// Token: 0x0400069C RID: 1692
	UseHub = 32,
	// Token: 0x0400069D RID: 1693
	UseReclaimers = 64,
	// Token: 0x0400069E RID: 1694
	UseBorder = 128,
	// Token: 0x0400069F RID: 1695
	UseResearch = 256,
	// Token: 0x040006A0 RID: 1696
	UseResources = 512,
	// Token: 0x040006A1 RID: 1697
	UseHeatSpawning = 1024,
	// Token: 0x040006A2 RID: 1698
	UseEntitySounds = 2048,
	// Token: 0x040006A3 RID: 1699
	UseEntityAnimations = 4096,
	// Token: 0x040006A4 RID: 1700
	UseWelcomeScreen = 8192
}
