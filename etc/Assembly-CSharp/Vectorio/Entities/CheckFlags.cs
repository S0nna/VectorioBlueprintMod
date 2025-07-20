using System;

namespace Vectorio.Entities
{
	// Token: 0x020002A6 RID: 678
	[Flags]
	public enum CheckFlags
	{
		// Token: 0x04001096 RID: 4246
		CheckForEntity = 1,
		// Token: 0x04001097 RID: 4247
		CheckBuildingComponents = 2,
		// Token: 0x04001098 RID: 4248
		CheckForClaim = 4,
		// Token: 0x04001099 RID: 4249
		CheckCosts = 8
	}
}
