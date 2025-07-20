using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Grid;

// Token: 0x02000084 RID: 132
[Serializable]
public class SaveData : SerializableData
{
	// Token: 0x060005D7 RID: 1495 RVA: 0x0001F000 File Offset: 0x0001D200
	public void CreateRegion(string id)
	{
		if (this.regions.ContainsKey(id))
		{
			Debug.Log("[SAVE DATA] Cannot create region! A region with ID " + id + " already exists!");
			return;
		}
		SaveData.Region value = new SaveData.Region
		{
			ID = id
		};
		this.regions.Add(id, value);
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001F04B File Offset: 0x0001D24B
	// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0001F053 File Offset: 0x0001D253
	public string FileName { get; set; }

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001F05C File Offset: 0x0001D25C
	// (set) Token: 0x060005DB RID: 1499 RVA: 0x0001F064 File Offset: 0x0001D264
	public GamemodeSaveData GamemodeData { get; set; }

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001F06D File Offset: 0x0001D26D
	// (set) Token: 0x060005DD RID: 1501 RVA: 0x0001F075 File Offset: 0x0001D275
	public float WorldTime { get; set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001F07E File Offset: 0x0001D27E
	// (set) Token: 0x060005DF RID: 1503 RVA: 0x0001F086 File Offset: 0x0001D286
	public string ActiveResearchTech { get; set; } = "";

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001F08F File Offset: 0x0001D28F
	// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0001F097 File Offset: 0x0001D297
	public List<CostData> researchTechResources { get; set; } = new List<CostData>();

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001F0A0 File Offset: 0x0001D2A0
	// (set) Token: 0x060005E3 RID: 1507 RVA: 0x0001F0A8 File Offset: 0x0001D2A8
	public List<string> completedResearchTechs { get; set; } = new List<string>();

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0001F0B1 File Offset: 0x0001D2B1
	// (set) Token: 0x060005E5 RID: 1509 RVA: 0x0001F0B9 File Offset: 0x0001D2B9
	public Dictionary<uint, SaveData.Hotbar> hotbars { get; set; } = new Dictionary<uint, SaveData.Hotbar>();

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0001F0C2 File Offset: 0x0001D2C2
	// (set) Token: 0x060005E7 RID: 1511 RVA: 0x0001F0CA File Offset: 0x0001D2CA
	public List<string> CompletedHints { get; set; } = new List<string>();

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001F0D4 File Offset: 0x0001D2D4
	public string GetProgressText(GamemodeData gamemodeData = null)
	{
		int count = this.completedResearchTechs.Count;
		string text = count.ToString() + " techs";
		if (gamemodeData == null)
		{
			if (this.GamemodeData == null)
			{
				return text;
			}
			gamemodeData = Library.RequestData<GamemodeData>(this.GamemodeData.ID);
			if (gamemodeData == null)
			{
				return text;
			}
		}
		float f = (float)count / (float)gamemodeData.GetTotalTechs() * 100f;
		return text + " (" + Mathf.RoundToInt(f).ToString() + "%)";
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001F160 File Offset: 0x0001D360
	// (set) Token: 0x060005EA RID: 1514 RVA: 0x0001F168 File Offset: 0x0001D368
	public int Seed { get; set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001F171 File Offset: 0x0001D371
	// (set) Token: 0x060005EC RID: 1516 RVA: 0x0001F179 File Offset: 0x0001D379
	public string ActiveRegion { get; set; } = "region_the_abyss";

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x0001F182 File Offset: 0x0001D382
	// (set) Token: 0x060005EE RID: 1518 RVA: 0x0001F18A File Offset: 0x0001D38A
	public Dictionary<string, SaveData.Region> regions { get; set; } = new Dictionary<string, SaveData.Region>();

	// Token: 0x060005EF RID: 1519 RVA: 0x0001F193 File Offset: 0x0001D393
	public bool HasRegion(string regionID)
	{
		return this.regions.ContainsKey(regionID);
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0001F1A4 File Offset: 0x0001D3A4
	public SaveData.Region GetRegion(string regionID)
	{
		SaveData.Region result;
		if (this.regions.TryGetValue(regionID, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x02000085 RID: 133
	[Serializable]
	public class Hotbar
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001F224 File Offset: 0x0001D424
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0001F22C File Offset: 0x0001D42C
		public int SelectedHotbar { get; set; }

		// Token: 0x04000360 RID: 864
		[SerializeField]
		public List<List<HotbarData>> hotbars = new List<List<HotbarData>>();
	}

	// Token: 0x02000086 RID: 134
	[Serializable]
	public class Region
	{
		// Token: 0x04000362 RID: 866
		[SerializeField]
		public string ID;

		// Token: 0x04000363 RID: 867
		[SerializeField]
		public Dictionary<string, List<TileData>> resources = new Dictionary<string, List<TileData>>();

		// Token: 0x04000364 RID: 868
		[SerializeField]
		public Dictionary<string, List<EntityMetadata>> entities = new Dictionary<string, List<EntityMetadata>>();

		// Token: 0x04000365 RID: 869
		[SerializeField]
		public Dictionary<string, List<EntityMetadata>> worldFeatures = new Dictionary<string, List<EntityMetadata>>();

		// Token: 0x04000366 RID: 870
		[SerializeField]
		public Dictionary<string, List<DecorationData>> decorations = new Dictionary<string, List<DecorationData>>();

		// Token: 0x04000367 RID: 871
		[SerializeField]
		public string[,] preview;
	}
}
