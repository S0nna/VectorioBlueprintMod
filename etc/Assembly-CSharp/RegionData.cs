using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Generation;

// Token: 0x020000C8 RID: 200
[CreateAssetMenu(fileName = "New Region", menuName = "Vectorio/Region")]
public class RegionData : BaseData
{
	// Token: 0x060006C1 RID: 1729 RVA: 0x0002022C File Offset: 0x0001E42C
	public void GetMaxCells()
	{
		int num = Mathf.CeilToInt((float)this.worldSize / (float)this.cellSize);
		this.totalHashCells = num * num;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x00020257 File Offset: 0x0001E457
	public BiomeData GetBiomeData(byte index)
	{
		if (index != 0)
		{
			return this.biomes[(int)(index - 1)];
		}
		return this.defaultBiome;
	}

	// Token: 0x04000443 RID: 1091
	public string goalText;

	// Token: 0x04000444 RID: 1092
	public RegionData.GoalType goalType;

	// Token: 0x04000445 RID: 1093
	public string goalVariable;

	// Token: 0x04000446 RID: 1094
	public FactionData playerFaction;

	// Token: 0x04000447 RID: 1095
	public FactionData enemyFaction;

	// Token: 0x04000448 RID: 1096
	public int xSpawnPosition;

	// Token: 0x04000449 RID: 1097
	public int ySpawnPosition;

	// Token: 0x0400044A RID: 1098
	public TileDesignData tile;

	// Token: 0x0400044B RID: 1099
	public EntityData hub;

	// Token: 0x0400044C RID: 1100
	public bool isBeta;

	// Token: 0x0400044D RID: 1101
	public AudioClip regionTrack;

	// Token: 0x0400044E RID: 1102
	public List<ResearchTechData> defaultTechs;

	// Token: 0x0400044F RID: 1103
	public List<RegionData> ensureRegionTechs;

	// Token: 0x04000450 RID: 1104
	public bool useFreeEntities;

	// Token: 0x04000451 RID: 1105
	public List<EntityData> freeEntities;

	// Token: 0x04000452 RID: 1106
	public bool useFogOfWar;

	// Token: 0x04000453 RID: 1107
	public Texture2D fogTexture;

	// Token: 0x04000454 RID: 1108
	public Vector2 textureTiling;

	// Token: 0x04000455 RID: 1109
	public Color fogColor;

	// Token: 0x04000456 RID: 1110
	public int totalHashCells;

	// Token: 0x04000457 RID: 1111
	public int worldSize = 1000;

	// Token: 0x04000458 RID: 1112
	public int cellSize = 25;

	// Token: 0x04000459 RID: 1113
	public BiomeData defaultBiome;

	// Token: 0x0400045A RID: 1114
	public List<BiomeData> biomes = new List<BiomeData>();

	// Token: 0x0400045B RID: 1115
	public Color tileHexColor;

	// Token: 0x0400045C RID: 1116
	public Color claimedHexColor;

	// Token: 0x0400045D RID: 1117
	public string minimapBorderText;

	// Token: 0x0400045E RID: 1118
	public Color minimapBorderColor;

	// Token: 0x0400045F RID: 1119
	public Color minimaptitleColor;

	// Token: 0x04000460 RID: 1120
	public Color borderFillColor;

	// Token: 0x04000461 RID: 1121
	public Material borderMaterial;

	// Token: 0x04000462 RID: 1122
	public Color backdropColor;

	// Token: 0x04000463 RID: 1123
	public Sprite backdropSprite;

	// Token: 0x04000464 RID: 1124
	public Material backdropMaterial;

	// Token: 0x04000465 RID: 1125
	[SerializeField]
	public List<EnemySpawn> enemies = new List<EnemySpawn>();

	// Token: 0x04000466 RID: 1126
	public RegionData.Guardian guardian;

	// Token: 0x020000C9 RID: 201
	[Serializable]
	public class Guardian
	{
		// Token: 0x04000467 RID: 1127
		public EntityData data;

		// Token: 0x04000468 RID: 1128
		public string factionID = "";

		// Token: 0x04000469 RID: 1129
		public List<RegionData.Guardian.PatrolPoint> points;

		// Token: 0x020000CA RID: 202
		[Serializable]
		public class PatrolPoint
		{
			// Token: 0x0400046A RID: 1130
			public int xPos;

			// Token: 0x0400046B RID: 1131
			public int yPos;
		}
	}

	// Token: 0x020000CB RID: 203
	public enum GoalType
	{
		// Token: 0x0400046D RID: 1133
		CompleteTech,
		// Token: 0x0400046E RID: 1134
		ConstructEntity
	}
}
