using System;

// Token: 0x020000B8 RID: 184
[Serializable]
public class HotbarData
{
	// Token: 0x060006A0 RID: 1696 RVA: 0x0001FEDB File Offset: 0x0001E0DB
	public HotbarData(string entityID, string modelID, string factionID)
	{
		this.entityID = entityID;
		this.modelID = modelID;
		this.factionID = factionID;
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0001FF1C File Offset: 0x0001E11C
	public HotbarData()
	{
		this.entityID = "";
		this.modelID = "";
		this.factionID = "";
	}

	// Token: 0x04000410 RID: 1040
	public string entityID = "";

	// Token: 0x04000411 RID: 1041
	public string modelID = "";

	// Token: 0x04000412 RID: 1042
	public string factionID = "";
}
