using System;

// Token: 0x0200016B RID: 363
public class C_EntityCreationData : NetworkEventBase
{
	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0003403E File Offset: 0x0003223E
	// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x00034046 File Offset: 0x00032246
	public byte[] CreationData { get; set; }

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0003404F File Offset: 0x0003224F
	// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x00034057 File Offset: 0x00032257
	public string EntityID { get; set; }

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00034060 File Offset: 0x00032260
	// (set) Token: 0x06000BFB RID: 3067 RVA: 0x00034068 File Offset: 0x00032268
	public uint RuntimeID { get; set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00034071 File Offset: 0x00032271
	// (set) Token: 0x06000BFD RID: 3069 RVA: 0x00034079 File Offset: 0x00032279
	public float PosX { get; set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00034082 File Offset: 0x00032282
	// (set) Token: 0x06000BFF RID: 3071 RVA: 0x0003408A File Offset: 0x0003228A
	public float PosY { get; set; }
}
