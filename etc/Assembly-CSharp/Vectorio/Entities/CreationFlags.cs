using System;

namespace Vectorio.Entities
{
	// Token: 0x020002A5 RID: 677
	[Flags]
	public enum CreationFlags
	{
		// Token: 0x04001090 RID: 4240
		UseCosmetic = 1,
		// Token: 0x04001091 RID: 4241
		UseAccent = 2,
		// Token: 0x04001092 RID: 4242
		UseCosts = 4,
		// Token: 0x04001093 RID: 4243
		UseCallback = 8,
		// Token: 0x04001094 RID: 4244
		UseVariables = 16
	}
}
