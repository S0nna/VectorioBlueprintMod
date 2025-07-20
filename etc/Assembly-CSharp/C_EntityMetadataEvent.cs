using System;

// Token: 0x0200016C RID: 364
[Serializable]
public class C_EntityMetadataEvent : NetworkEventBase
{
	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000C01 RID: 3073 RVA: 0x0003409B File Offset: 0x0003229B
	// (set) Token: 0x06000C02 RID: 3074 RVA: 0x000340A3 File Offset: 0x000322A3
	public uint RuntimeID { get; set; }

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000C03 RID: 3075 RVA: 0x000340AC File Offset: 0x000322AC
	// (set) Token: 0x06000C04 RID: 3076 RVA: 0x000340B4 File Offset: 0x000322B4
	public byte[] Metadata { get; set; }

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000C05 RID: 3077 RVA: 0x000340BD File Offset: 0x000322BD
	// (set) Token: 0x06000C06 RID: 3078 RVA: 0x000340C5 File Offset: 0x000322C5
	public bool AsPipette { get; set; }
}
