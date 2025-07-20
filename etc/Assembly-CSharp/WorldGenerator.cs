using System;
using System.Collections.Generic;
using FOW;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Generation;

// Token: 0x02000078 RID: 120
[DefaultExecutionOrder(0)]
public class WorldGenerator : Singleton<WorldGenerator>
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001DAE2 File Offset: 0x0001BCE2
	public WorldSpawner WorldSpawner
	{
		get
		{
			return this._worldSpawner;
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0001DAEA File Offset: 0x0001BCEA
	public void AddFeature(byte biomeIndex, WorldFeature feature)
	{
		if (!this._features.ContainsKey(biomeIndex))
		{
			this._features.Add(biomeIndex, new List<WorldFeature>());
		}
		this._features[biomeIndex].Add(feature);
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001DB1D File Offset: 0x0001BD1D
	public float RegionMinBoundaryX
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001DB1D File Offset: 0x0001BD1D
	public float RegionMinBoundaryY
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001DB24 File Offset: 0x0001BD24
	public float RegionMaxBoundaryX
	{
		get
		{
			return this._regionMaxSizeX;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001DB2C File Offset: 0x0001BD2C
	public float RegionMaxBoundaryY
	{
		get
		{
			return this._regionMaxSizeY;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001DB34 File Offset: 0x0001BD34
	public Vector2 CenterWorldPos
	{
		get
		{
			return this._centerWorldPosition;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600058E RID: 1422 RVA: 0x0001DB3C File Offset: 0x0001BD3C
	public Vector2Int CenterTilePosition
	{
		get
		{
			return this._centerTilePosition;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600058F RID: 1423 RVA: 0x0001DB44 File Offset: 0x0001BD44
	public RegionData RegionData
	{
		get
		{
			return this._region;
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0001DB4C File Offset: 0x0001BD4C
	public BiomeData GetBiome(byte index)
	{
		return this._region.GetBiomeData(index);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0001DB5C File Offset: 0x0001BD5C
	public void Start()
	{
		if (this._tileGrid == null)
		{
			this._tileGrid = base.GetComponent<TileGrid>();
		}
		if (this.background != null)
		{
			this.background.SetActive(true);
		}
		if (this._worldSpawner == null)
		{
			this._worldSpawner = base.gameObject.GetComponent<WorldSpawner>();
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0001DBBC File Offset: 0x0001BDBC
	public void GenerateWorld(string regionID)
	{
		SaveData.Region region = SaveSystem.SaveData.GetRegion(regionID);
		if (region == null)
		{
			Debug.Log("[WORLD GENERATOR] The region " + regionID + " does not exist in save data!");
			return;
		}
		this._region = Library.RequestData<RegionData>(region.ID);
		if (this._region == null)
		{
			Debug.Log("[WORLD GENERATOR] The provided region ID " + region.ID + " is invalid!");
			this._region = new RegionData();
		}
		Random.InitState(SaveSystem.SaveData.Seed);
		this.seed = SaveSystem.SaveData.Seed;
		if (this._tileGrid == null)
		{
			this._tileGrid = base.GetComponent<TileGrid>();
		}
		this._centerWorldPosition = new Vector2((float)(this._region.xSpawnPosition * 5), (float)(this._region.ySpawnPosition * 5));
		this._centerTilePosition = new Vector2Int(this._region.xSpawnPosition, this._region.ySpawnPosition);
		this._regionMaxSizeX = (float)this._region.worldSize * 5f;
		this._regionMaxSizeY = (float)this._region.worldSize * 5f;
		this._tileGrid.SetGridBounds(this._region.worldSize, this._region.worldSize);
		if (this._region.useFogOfWar)
		{
			Singleton<FogOfWarHandler>.Instance.Enable(this._region, this._centerWorldPosition, this._centerWorldPosition);
		}
		else
		{
			Singleton<FogOfWarHandler>.Instance.Hide();
		}
		this._colorPalette = new List<Color>();
		for (float num = 0.95f; num < 1f; num += 0.01f)
		{
			this._colorPalette.Add(new Color(this._region.tileHexColor.r * num, this._region.tileHexColor.g * num, this._region.tileHexColor.b * num, 1f));
		}
		for (int i = 0; i < this._region.worldSize; i++)
		{
			for (int j = 0; j < this._region.worldSize; j++)
			{
				TileDesign design = new TileDesign(this._region.tile, this._colorPalette[Random.Range(0, this._colorPalette.Count)]);
				this._tileGrid.SetTileDesign(design, new Vector3Int(j, i, 0), true);
			}
		}
		byte b = 1;
		if (this._region.biomes != null && this._region.biomes.Count > 0)
		{
			foreach (BiomeData biome in this._region.biomes)
			{
				this.GenerateBiome(biome, b);
				b += 1;
			}
		}
		Color borderFillColor = this._region.borderFillColor;
		if (Singleton<Gamemode>.Instance.UseBorder)
		{
			if (this._border == null)
			{
				this._border = Object.Instantiate<Border>(this.border);
			}
			this._border.SetBorder(new Vector2Int(this._region.worldSize, this._region.worldSize), borderFillColor, this._region.borderMaterial, this._region.backdropColor, this._region.backdropSprite, this._region.backdropMaterial);
		}
		if (!RegionManager.FIRST_LOAD)
		{
			return;
		}
		this._worldSpawner.Setup(this);
		this.GenerateBiomeFeatures(this._region.defaultBiome.features, 0);
		b = 1;
		foreach (BiomeData biomeData in this._region.biomes)
		{
			this.GenerateBiomeFeatures(biomeData.features, b);
			b += 1;
		}
		foreach (KeyValuePair<byte, List<WorldFeature>> keyValuePair in this._features)
		{
			this.GenerateBiomeFeatures(keyValuePair.Value, keyValuePair.Key);
		}
		if (this._region.guardian != null && this._region.guardian.data != null)
		{
			Vector2 position = new Vector2((float)this._region.guardian.points[0].xPos * 5f, (float)this._region.guardian.points[0].yPos * 5f);
			EntityCreationData creationData = EventBuilder.BuildCreationData(this._region.guardian.data.ID, this._region.enemyFaction.ID, position, SyncType.ServerInitiated);
			EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(Singleton<FactionManager>.Instance.GetFactionAccent(this._region.enemyFaction.ID)));
			Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
		}
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0001E0CC File Offset: 0x0001C2CC
	public void GenerateBiome(BiomeData biome, byte biomeIndex)
	{
		List<Color> list = new List<Color>();
		for (float num = 0.95f; num < 1f; num += 0.01f)
		{
			list.Add(new Color(biome.color.r * num, biome.color.g * num, biome.color.b * num, 1f));
		}
		float num2 = (float)biome.biomeSize / (float)biome.noiseSize;
		BiomeData.NoiseType noiseType = biome.noiseType;
		float[,] array;
		if (noiseType == BiomeData.NoiseType.Island)
		{
			array = Noise.GenerateBiomeMap(this.seed, biome);
			List<CardinalDirection> directionList = this.GetDirectionList();
			Vector2Int randomDirectionalTile = this.GetRandomDirectionalTile(directionList[Random.Range(0, directionList.Count)], this.CenterTilePosition, biome.minSpawnDistance, biome.maxSpawnDistance);
			int num3 = Mathf.FloorToInt((float)(biome.biomeSize / 2));
			int num4 = -Mathf.Min(randomDirectionalTile.x - num3, 0);
			int num5 = -Mathf.Min(randomDirectionalTile.y - num3, 0);
			int num6 = Mathf.Max(randomDirectionalTile.x + num3 - this._region.worldSize, 0);
			int num7 = Mathf.Max(randomDirectionalTile.y + num3 - this._region.worldSize, 0);
			for (int i = num4; i < biome.biomeSize - num6; i++)
			{
				for (int j = num5; j < biome.biomeSize - num7; j++)
				{
					int num8 = Mathf.FloorToInt((float)i / num2);
					int num9 = Mathf.FloorToInt((float)j / num2);
					float num10 = array[num8, num9];
					Vector3Int coords = new Vector3Int(randomDirectionalTile.x - num3 + i, randomDirectionalTile.y - num3 + j, 0);
					Color color = Color.Lerp(this._colorPalette[Random.Range(0, this._colorPalette.Count)], list[Random.Range(0, list.Count)], num10);
					if (num10 > biome.threshold)
					{
						Singleton<TileGrid>.Instance.SetCellBiomeIndex(coords, biomeIndex, color);
					}
					else
					{
						TileDesign design = new TileDesign(this._region.tile, color);
						this._tileGrid.SetTileDesign(design, coords, true);
					}
				}
			}
			return;
		}
		if (noiseType != BiomeData.NoiseType.Directional)
		{
			return;
		}
		CardinalDirection cardinalDirection = (CardinalDirection)Random.Range(0, 4);
		array = Noise.GenerateDirectionalBiomeMap(this.seed, biome, this._region.worldSize, cardinalDirection);
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		int num11 = (cardinalDirection == CardinalDirection.East) ? (this._region.worldSize - length) : 0;
		int num12 = (cardinalDirection == CardinalDirection.North) ? (this._region.worldSize - length2) : 0;
		for (int k = 0; k < length; k++)
		{
			for (int l = 0; l < length2; l++)
			{
				float num10 = array[k, l];
				Vector3Int coords = new Vector3Int(num11 + k, num12 + l, 0);
				Color color = Color.Lerp(this._colorPalette[Random.Range(0, this._colorPalette.Count)], list[Random.Range(0, list.Count)], num10);
				if (num10 > biome.threshold)
				{
					Singleton<TileGrid>.Instance.SetCellBiomeIndex(coords, biomeIndex, color);
				}
				else
				{
					TileDesign design2 = new TileDesign(this._region.tile, color);
					this._tileGrid.SetTileDesign(design2, coords, true);
				}
			}
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0001E428 File Offset: 0x0001C628
	public void GenerateBiomeFeatures(List<WorldFeature> features, byte biomeIndex)
	{
		foreach (WorldFeature feature in features)
		{
			this._worldSpawner.SpawnFeature(biomeIndex, feature);
		}
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0001E47C File Offset: 0x0001C67C
	public Vector2Int GetRandomDirectionalTile(CardinalDirection direction, Vector2Int originCell, int minRange, int maxRange)
	{
		Vector2Int result;
		switch (direction)
		{
		case CardinalDirection.North:
			result = new Vector2Int(Random.Range(originCell.x - maxRange, originCell.x + maxRange), Random.Range(originCell.y + minRange, originCell.y + maxRange));
			break;
		case CardinalDirection.East:
			result = new Vector2Int(Random.Range(originCell.x + minRange, originCell.x + maxRange), Random.Range(originCell.y - maxRange, originCell.y + maxRange));
			break;
		case CardinalDirection.South:
			result = new Vector2Int(Random.Range(originCell.x - maxRange, originCell.x + maxRange), Random.Range(originCell.y - maxRange, originCell.y - minRange));
			break;
		case CardinalDirection.West:
			result = new Vector2Int(Random.Range(originCell.x - maxRange, originCell.x - minRange), Random.Range(originCell.y - maxRange, originCell.y + maxRange));
			break;
		default:
			result = new Vector2Int(Random.Range(originCell.x - maxRange, originCell.x + maxRange), Random.Range(originCell.y + minRange, originCell.y + maxRange));
			break;
		}
		return result;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0001E5CB File Offset: 0x0001C7CB
	public List<CardinalDirection> GetDirectionList()
	{
		return new List<CardinalDirection>
		{
			CardinalDirection.North,
			CardinalDirection.East,
			CardinalDirection.South,
			CardinalDirection.West
		};
	}

	// Token: 0x0400031D RID: 797
	[SerializeField]
	protected WorldSpawner _worldSpawner;

	// Token: 0x0400031E RID: 798
	[Header("World Overrides")]
	public int seed;

	// Token: 0x0400031F RID: 799
	public FogOfWarWorld fogOfWarPrefab;

	// Token: 0x04000320 RID: 800
	public Border border;

	// Token: 0x04000321 RID: 801
	public GameObject background;

	// Token: 0x04000322 RID: 802
	protected RegionData _region;

	// Token: 0x04000323 RID: 803
	protected Border _border;

	// Token: 0x04000324 RID: 804
	protected Dictionary<byte, List<WorldFeature>> _features = new Dictionary<byte, List<WorldFeature>>();

	// Token: 0x04000325 RID: 805
	protected TileGrid _tileGrid;

	// Token: 0x04000326 RID: 806
	protected List<Color> _colorPalette;

	// Token: 0x04000327 RID: 807
	protected Vector2 _centerWorldPosition;

	// Token: 0x04000328 RID: 808
	protected Vector2Int _centerTilePosition;

	// Token: 0x04000329 RID: 809
	protected float _regionMaxSizeX;

	// Token: 0x0400032A RID: 810
	protected float _regionMaxSizeY;
}
