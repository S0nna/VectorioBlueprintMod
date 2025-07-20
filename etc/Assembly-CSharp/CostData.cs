using System;

// Token: 0x020000AC RID: 172
[Serializable]
public class CostData
{
	// Token: 0x06000676 RID: 1654 RVA: 0x0001FA44 File Offset: 0x0001DC44
	public CostData(ResourceData resource, int amount)
	{
		this.resourceID = resource.ID;
		this.amount = amount;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0001FA5F File Offset: 0x0001DC5F
	public CostData(Cost cost)
	{
		this.resourceID = cost.resource.ID;
		this.amount = cost.amount;
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0001FA84 File Offset: 0x0001DC84
	public CostData()
	{
		this.resourceID = "";
		this.amount = 0;
	}

	// Token: 0x040003E8 RID: 1000
	public string resourceID;

	// Token: 0x040003E9 RID: 1001
	public int amount;
}
