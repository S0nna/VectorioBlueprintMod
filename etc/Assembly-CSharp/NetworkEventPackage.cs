using System;
using System.Collections.Generic;

// Token: 0x02000167 RID: 359
[Serializable]
public struct NetworkEventPackage
{
	// Token: 0x06000BD5 RID: 3029 RVA: 0x00033CFB File Offset: 0x00031EFB
	public void Reset()
	{
		this.events = new List<NetworkEventBase>();
	}

	// Token: 0x04000804 RID: 2052
	public List<NetworkEventBase> events;
}
