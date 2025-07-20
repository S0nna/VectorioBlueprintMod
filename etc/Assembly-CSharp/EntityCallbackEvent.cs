using System;
using Vectorio.Entities;

// Token: 0x0200016D RID: 365
[Serializable]
public class EntityCallbackEvent : NetworkEventBase
{
	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000C08 RID: 3080 RVA: 0x000340CE File Offset: 0x000322CE
	// (set) Token: 0x06000C09 RID: 3081 RVA: 0x000340D6 File Offset: 0x000322D6
	public bool IsFinished { get; set; }

	// Token: 0x0400081B RID: 2075
	public uint EntityID;

	// Token: 0x0400081D RID: 2077
	public byte ComponentID;

	// Token: 0x0400081E RID: 2078
	public float Time;

	// Token: 0x0400081F RID: 2079
	public VariableContainer Variable;
}
