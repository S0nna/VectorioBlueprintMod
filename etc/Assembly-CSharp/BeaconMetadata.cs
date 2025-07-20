using System;

// Token: 0x020000E0 RID: 224
[MetadataFor(typeof(Beacon))]
[Serializable]
public class BeaconMetadata : ComponentMetadata<Beacon>
{
	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000710 RID: 1808 RVA: 0x00020BF8 File Offset: 0x0001EDF8
	// (set) Token: 0x06000711 RID: 1809 RVA: 0x00020C00 File Offset: 0x0001EE00
	public string IconID { get; set; }

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000712 RID: 1810 RVA: 0x00020C09 File Offset: 0x0001EE09
	// (set) Token: 0x06000713 RID: 1811 RVA: 0x00020C11 File Offset: 0x0001EE11
	public string Title { get; set; }

	// Token: 0x06000714 RID: 1812 RVA: 0x00020C1A File Offset: 0x0001EE1A
	public override void GetValues(Beacon component, MetadataContext context)
	{
		this.IconID = component.GetIcon().ID;
		this.Title = component.GetTitle();
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00020C3C File Offset: 0x0001EE3C
	public override void SetValues(Beacon component, bool asPipette, MetadataContext context)
	{
		if (this.IconID != null)
		{
			IconData iconData = Library.RequestData<IconData>(this.IconID);
			if (iconData != null)
			{
				component.SetIcon(iconData);
			}
		}
		if (this.Title != null)
		{
			component.SetTitle(this.Title);
		}
	}
}
