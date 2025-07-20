using System;

// Token: 0x020000E5 RID: 229
[Serializable]
public class ComponentMetadataWrapper
{
	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000742 RID: 1858 RVA: 0x00021409 File Offset: 0x0001F609
	// (set) Token: 0x06000743 RID: 1859 RVA: 0x00021411 File Offset: 0x0001F611
	public string Type { get; set; }

	// Token: 0x06000744 RID: 1860 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void GetValuesFromComponent(EntityComponent component, MetadataContext context)
	{
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void SetValuesToComponent(EntityComponent component, bool asPipette, MetadataContext context)
	{
	}
}
