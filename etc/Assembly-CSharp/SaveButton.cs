using System;
using TMPro;
using Vectorio.PhasmaUI;

// Token: 0x020001EC RID: 492
public class SaveButton : Button
{
	// Token: 0x06000F32 RID: 3890 RVA: 0x00047184 File Offset: 0x00045384
	public void Set(SaveData save)
	{
		this._save = save;
		this.title.text = save.Name;
		RegionData regionData = Library.RequestData<RegionData>(save.ActiveRegion);
		this.region.text = ((regionData != null) ? regionData.Name.ToUpper() : "UNKNOWN REGION");
		this.techs.text = save.GetProgressText(null);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x000471ED File Offset: 0x000453ED
	public void LoadSave()
	{
		Singleton<SavePreviewer>.Instance.PreviewSave(this._save);
	}

	// Token: 0x04000C67 RID: 3175
	protected SaveData _save;

	// Token: 0x04000C68 RID: 3176
	public TextMeshProUGUI title;

	// Token: 0x04000C69 RID: 3177
	public TextMeshProUGUI region;

	// Token: 0x04000C6A RID: 3178
	public TextMeshProUGUI techs;
}
