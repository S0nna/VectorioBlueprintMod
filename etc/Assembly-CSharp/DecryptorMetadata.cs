using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
[MetadataFor(typeof(Decryptor))]
[Serializable]
public class DecryptorMetadata : ComponentMetadata<Decryptor>
{
	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000747 RID: 1863 RVA: 0x0002141A File Offset: 0x0001F61A
	// (set) Token: 0x06000748 RID: 1864 RVA: 0x00021422 File Offset: 0x0001F622
	public string TechID { get; set; }

	// Token: 0x06000749 RID: 1865 RVA: 0x0002142B File Offset: 0x0001F62B
	public override void GetValues(Decryptor component, MetadataContext context)
	{
		if (component != null && component.Tech != null)
		{
			this.TechID = component.Tech.ID;
			return;
		}
		Debug.Log("[DECRYPTOR] Couldn't save because of missing tech!");
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00021460 File Offset: 0x0001F660
	public override void SetValues(Decryptor component, bool asPipette, MetadataContext context)
	{
		if (this.TechID != null)
		{
			ResearchTechData researchTechData = Library.RequestData<ResearchTechData>(this.TechID);
			if (researchTechData != null)
			{
				component.SetTech(researchTechData);
				return;
			}
		}
		else
		{
			Debug.Log("[DECRYPTOR] No tech ID is present in metadata!");
		}
	}
}
