using System;

// Token: 0x02000170 RID: 368
[Serializable]
public class EntityMetadataEvent : NetworkEventBase
{
	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000C0D RID: 3085 RVA: 0x000340DF File Offset: 0x000322DF
	// (set) Token: 0x06000C0E RID: 3086 RVA: 0x000340E7 File Offset: 0x000322E7
	public uint RuntimeID { get; set; }

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000C0F RID: 3087 RVA: 0x000340F0 File Offset: 0x000322F0
	// (set) Token: 0x06000C10 RID: 3088 RVA: 0x000340F8 File Offset: 0x000322F8
	public EntityMetadata Metadata { get; set; }

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00034101 File Offset: 0x00032301
	// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00034109 File Offset: 0x00032309
	public bool AsPipette { get; set; }
}
