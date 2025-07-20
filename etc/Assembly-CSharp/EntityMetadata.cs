using System;
using System.Collections.Generic;

// Token: 0x020000E8 RID: 232
[MetadataFor(typeof(Entity))]
[Serializable]
public class EntityMetadata
{
	// Token: 0x06000759 RID: 1881 RVA: 0x00021764 File Offset: 0x0001F964
	public EntityMetadata(Entity entity, List<ComponentMetadataWrapper> componentMetadataList, MetadataContext context)
	{
		this.Context = context;
		this.RuntimeID = new E_ID(entity.RuntimeID, context);
		this.EntityID = entity.GetData().ID;
		this.FactionID = entity.FactionID;
		this.PosX = entity.transform.position.x;
		this.PosY = entity.transform.position.y;
		this.EntityFlags = entity.Extract_EFlags();
		if (entity.GetModel.HasCustomModel)
		{
			this.ModelID = entity.GetModel.ID;
		}
		else
		{
			this.ModelID = "default";
		}
		if (entity.GetModel.HasAccent)
		{
			this.AccentData = entity.GetModel.GetAccentData();
		}
		else
		{
			this.AccentData = null;
		}
		this.Components = componentMetadataList;
		if (entity.HasLinkedEntity)
		{
			this.LinkedEntityID = new E_ID(entity.LinkedEntity.RuntimeID, context);
			return;
		}
		this.LinkedEntityID = null;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0002189C File Offset: 0x0001FA9C
	public EntityMetadata()
	{
		this.RuntimeID = new E_ID();
		this.EntityID = "";
		this.ModelID = "default";
		this.FactionID = "";
		this.EntityFlags = (EntityFlags)0;
		this.AccentData = null;
		this.PosX = 0f;
		this.PosY = 0f;
		this.Components = new List<ComponentMetadataWrapper>();
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00021944 File Offset: 0x0001FB44
	public override string ToString()
	{
		string text = string.Format("RuntimeID: {0}\nEntityID: {1}\nModelID: {2}\n", this.RuntimeID, this.EntityID, this.ModelID) + string.Format("FactionID: {0}\nEntityFlags: {1}\n", this.FactionID, this.EntityFlags) + string.Format("AccentData: {0}\nPosX: {1}\nPosY: {2}\n\nComponents:", this.AccentData, this.PosX, this.PosY);
		foreach (ComponentMetadataWrapper componentMetadataWrapper in this.Components)
		{
			text = string.Concat(new string[]
			{
				text,
				"- ",
				componentMetadataWrapper.Type.ToString(),
				": ",
				componentMetadataWrapper.ToString()
			});
		}
		return text;
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x0600075C RID: 1884 RVA: 0x00021A2C File Offset: 0x0001FC2C
	// (set) Token: 0x0600075D RID: 1885 RVA: 0x00021A34 File Offset: 0x0001FC34
	public MetadataContext Context { get; set; }

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600075E RID: 1886 RVA: 0x00021A3D File Offset: 0x0001FC3D
	// (set) Token: 0x0600075F RID: 1887 RVA: 0x00021A45 File Offset: 0x0001FC45
	public E_ID RuntimeID { get; set; } = new E_ID();

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000760 RID: 1888 RVA: 0x00021A4E File Offset: 0x0001FC4E
	// (set) Token: 0x06000761 RID: 1889 RVA: 0x00021A56 File Offset: 0x0001FC56
	public string EntityID { get; set; } = "";

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000762 RID: 1890 RVA: 0x00021A5F File Offset: 0x0001FC5F
	// (set) Token: 0x06000763 RID: 1891 RVA: 0x00021A67 File Offset: 0x0001FC67
	public EntityFlags EntityFlags { get; set; }

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000764 RID: 1892 RVA: 0x00021A70 File Offset: 0x0001FC70
	// (set) Token: 0x06000765 RID: 1893 RVA: 0x00021A78 File Offset: 0x0001FC78
	public string ModelID { get; set; } = "default";

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000766 RID: 1894 RVA: 0x00021A81 File Offset: 0x0001FC81
	// (set) Token: 0x06000767 RID: 1895 RVA: 0x00021A89 File Offset: 0x0001FC89
	public string FactionID { get; set; } = "";

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000768 RID: 1896 RVA: 0x00021A92 File Offset: 0x0001FC92
	// (set) Token: 0x06000769 RID: 1897 RVA: 0x00021A9A File Offset: 0x0001FC9A
	public AccentData AccentData { get; set; }

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x0600076A RID: 1898 RVA: 0x00021AA3 File Offset: 0x0001FCA3
	// (set) Token: 0x0600076B RID: 1899 RVA: 0x00021AAB File Offset: 0x0001FCAB
	public float PosX { get; set; }

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x0600076C RID: 1900 RVA: 0x00021AB4 File Offset: 0x0001FCB4
	// (set) Token: 0x0600076D RID: 1901 RVA: 0x00021ABC File Offset: 0x0001FCBC
	public float PosY { get; set; }

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x0600076E RID: 1902 RVA: 0x00021AC5 File Offset: 0x0001FCC5
	// (set) Token: 0x0600076F RID: 1903 RVA: 0x00021ACD File Offset: 0x0001FCCD
	public E_ID LinkedEntityID { get; set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000770 RID: 1904 RVA: 0x00021AD6 File Offset: 0x0001FCD6
	// (set) Token: 0x06000771 RID: 1905 RVA: 0x00021ADE File Offset: 0x0001FCDE
	public List<ComponentMetadataWrapper> Components { get; set; } = new List<ComponentMetadataWrapper>();

	// Token: 0x06000772 RID: 1906 RVA: 0x00021AE7 File Offset: 0x0001FCE7
	public bool Has_EFlag(EntityFlags flag)
	{
		return (this.EntityFlags & flag) == flag;
	}
}
