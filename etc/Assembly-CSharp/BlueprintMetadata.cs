using System;
using Vectorio.Entities;
using Vectorio.Serialization;

// Token: 0x020000E1 RID: 225
[MetadataFor(typeof(Blueprint))]
[Serializable]
public class BlueprintMetadata : ComponentMetadata<Blueprint>
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000717 RID: 1815 RVA: 0x00020C89 File Offset: 0x0001EE89
	// (set) Token: 0x06000718 RID: 1816 RVA: 0x00020C91 File Offset: 0x0001EE91
	public bool HasPipette { get; set; }

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000719 RID: 1817 RVA: 0x00020C9A File Offset: 0x0001EE9A
	// (set) Token: 0x0600071A RID: 1818 RVA: 0x00020CA2 File Offset: 0x0001EEA2
	public byte[] PipetteData { get; set; }

	// Token: 0x0600071B RID: 1819 RVA: 0x00020CAC File Offset: 0x0001EEAC
	public override void GetValues(Blueprint component, MetadataContext context)
	{
		EntityCreationData creationData = component.GetCreationData();
		if (creationData.HasPipette)
		{
			this.PipetteData = DataProcessor.SerializeAndCompressObject<EntityMetadata>(creationData.PipetteData);
			this.HasPipette = true;
		}
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00020CE0 File Offset: 0x0001EEE0
	public override void SetValues(Blueprint component, bool asPipette, MetadataContext context)
	{
		if (this.HasPipette)
		{
			EntityMetadata entityMetadata = DataProcessor.DecompressAndDeserializeObject<EntityMetadata>(this.PipetteData);
			if (entityMetadata != null)
			{
				component.SetPipetteData(entityMetadata);
			}
		}
	}
}
