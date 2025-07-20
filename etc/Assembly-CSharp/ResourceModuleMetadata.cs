using System;
using System.Collections.Generic;

// Token: 0x020000D0 RID: 208
[MetadataFor(typeof(ResourceModule))]
[Serializable]
public class ResourceModuleMetadata : ComponentMetadata<ResourceModule>
{
	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060006E0 RID: 1760 RVA: 0x000204ED File Offset: 0x0001E6ED
	// (set) Token: 0x060006E1 RID: 1761 RVA: 0x000204F5 File Offset: 0x0001E6F5
	public List<ResourceModuleMetadata.SavedResource> InputStorage { get; set; }

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060006E2 RID: 1762 RVA: 0x000204FE File Offset: 0x0001E6FE
	// (set) Token: 0x060006E3 RID: 1763 RVA: 0x00020506 File Offset: 0x0001E706
	public List<ResourceModuleMetadata.SavedResource> OutputStorage { get; set; }

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0002050F File Offset: 0x0001E70F
	// (set) Token: 0x060006E5 RID: 1765 RVA: 0x00020517 File Offset: 0x0001E717
	public bool HasInputStorage { get; set; }

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00020520 File Offset: 0x0001E720
	// (set) Token: 0x060006E7 RID: 1767 RVA: 0x00020528 File Offset: 0x0001E728
	public bool HasOutputStorage { get; set; }

	// Token: 0x060006E8 RID: 1768 RVA: 0x00020534 File Offset: 0x0001E734
	public override void GetValues(ResourceModule module, MetadataContext context)
	{
		if (!module.RouteInputToOutput && module.HasInputContainer())
		{
			this.HasInputStorage = true;
			this.InputStorage = new List<ResourceModuleMetadata.SavedResource>();
			foreach (StoredResource storedResource in module.GetInputContainer().GetStoredResources())
			{
				this.InputStorage.Add(new ResourceModuleMetadata.SavedResource
				{
					ID = storedResource.ResourceData.ID,
					Amount = storedResource.AmountStored
				});
			}
		}
		if (module.HasOutputContainer())
		{
			this.HasOutputStorage = true;
			this.OutputStorage = new List<ResourceModuleMetadata.SavedResource>();
			foreach (StoredResource storedResource2 in module.GetOutputContainer().GetStoredResources())
			{
				this.OutputStorage.Add(new ResourceModuleMetadata.SavedResource
				{
					ID = storedResource2.ResourceData.ID,
					Amount = storedResource2.AmountStored
				});
			}
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0002066C File Offset: 0x0001E86C
	public override void SetValues(ResourceModule module, bool asPipette, MetadataContext context)
	{
		if (!asPipette)
		{
			if (this.HasInputStorage)
			{
				foreach (ResourceModuleMetadata.SavedResource savedResource in this.InputStorage)
				{
					ResourceData resourceData = Library.RequestData<ResourceData>(savedResource.ID);
					if (resourceData != null)
					{
						module.AddResource(ContainerType.Input, resourceData, savedResource.Amount);
					}
				}
			}
			if (this.HasOutputStorage)
			{
				foreach (ResourceModuleMetadata.SavedResource savedResource2 in this.OutputStorage)
				{
					ResourceData resourceData2 = Library.RequestData<ResourceData>(savedResource2.ID);
					if (resourceData2 != null)
					{
						module.AddResource(ContainerType.Output, resourceData2, savedResource2.Amount);
					}
				}
			}
		}
	}

	// Token: 0x020000D1 RID: 209
	[Serializable]
	public struct SavedResource
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00020760 File Offset: 0x0001E960
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x00020768 File Offset: 0x0001E968
		public string ID { readonly get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00020771 File Offset: 0x0001E971
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x00020779 File Offset: 0x0001E979
		public int Amount { readonly get; set; }
	}
}
