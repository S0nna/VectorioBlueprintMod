using System;

// Token: 0x020000E9 RID: 233
[MetadataFor(typeof(Generator))]
[Serializable]
public class GeneratorMetadata : ComponentMetadata<Generator>
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06000773 RID: 1907 RVA: 0x00021AF4 File Offset: 0x0001FCF4
	// (set) Token: 0x06000774 RID: 1908 RVA: 0x00021AFC File Offset: 0x0001FCFC
	public string ResourceID { get; set; } = "";

	// Token: 0x06000775 RID: 1909 RVA: 0x00021B05 File Offset: 0x0001FD05
	public override void GetValues(Generator generator, MetadataContext context)
	{
		ResourceData resourceBeingBurned = generator.ResourceBeingBurned;
		this.ResourceID = (((resourceBeingBurned != null) ? resourceBeingBurned.ID : null) ?? "");
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00021B28 File Offset: 0x0001FD28
	public override void SetValues(Generator component, bool asPipette, MetadataContext context)
	{
		if (!asPipette && this.ResourceID != null && this.ResourceID != "")
		{
			ResourceData resourceData = Library.RequestData<ResourceData>(this.ResourceID);
			if (resourceData != null)
			{
				component.BurnResource(resourceData);
			}
		}
	}
}
