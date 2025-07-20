using System;

// Token: 0x02000092 RID: 146
[Flags]
public enum ContainerFlags
{
	// Token: 0x040003A1 RID: 929
	None = 0,
	// Token: 0x040003A2 RID: 930
	Xray = 1,
	// Token: 0x040003A3 RID: 931
	GlobalStorage = 2,
	// Token: 0x040003A4 RID: 932
	GeneratesResources = 4,
	// Token: 0x040003A5 RID: 933
	UseFilter = 8,
	// Token: 0x040003A6 RID: 934
	RouteInputToOutput = 16
}
