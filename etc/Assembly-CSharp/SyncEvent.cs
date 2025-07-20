using System;

// Token: 0x02000174 RID: 372
[Serializable]
public class SyncEvent : NetworkEventBase
{
	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000C1A RID: 3098 RVA: 0x00034130 File Offset: 0x00032330
	// (set) Token: 0x06000C1B RID: 3099 RVA: 0x00034138 File Offset: 0x00032338
	public bool IsEntity { get; set; }

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00034141 File Offset: 0x00032341
	// (set) Token: 0x06000C1D RID: 3101 RVA: 0x00034149 File Offset: 0x00032349
	public uint RuntimeID { get; set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00034152 File Offset: 0x00032352
	// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0003415A File Offset: 0x0003235A
	public byte ComponentID { get; set; }

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00034163 File Offset: 0x00032363
	// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0003416B File Offset: 0x0003236B
	public byte[] Data { get; set; }
}
