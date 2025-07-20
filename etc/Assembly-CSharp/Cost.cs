using System;

// Token: 0x0200020A RID: 522
[Serializable]
public class Cost
{
	// Token: 0x06000FA4 RID: 4004 RVA: 0x00049E1C File Offset: 0x0004801C
	public Cost(ResourceData resource, int amount)
	{
		this.resource = resource;
		this.amount = amount;
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x00049E32 File Offset: 0x00048032
	public Cost(CostData costData)
	{
		this.resource = Library.RequestData<ResourceData>(costData.resourceID);
		this.amount = costData.amount;
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x00049E57 File Offset: 0x00048057
	public Cost()
	{
		this.resource = null;
		this.amount = 0;
	}

	// Token: 0x04000D60 RID: 3424
	public ResourceData resource;

	// Token: 0x04000D61 RID: 3425
	public int amount;
}
