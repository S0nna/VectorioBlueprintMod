using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vectorio.Generation;
using Vectorio.Utilities;

// Token: 0x02000077 RID: 119
[DefaultExecutionOrder(0)]
public class TileGrid : Singleton<TileGrid>
{
	// Token: 0x06000552 RID: 1362 RVA: 0x0001C9F0 File Offset: 0x0001ABF0
	public TileBase FetchTile(TileDesignData data)
	{
		if (this._tileDesigns.ContainsKey(data))
		{
			return this._tileDesigns[data];
		}
		Tile tile = ScriptableObject.CreateInstance<Tile>();
		tile.sprite = data.tile;
		this._tileDesigns.Add(data, tile);
		return tile;
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001CA38 File Offset: 0x0001AC38
	public List<Vector3Int> GetResourceTilePositions
	{
		get
		{
			return this._resourceTiles;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001CA40 File Offset: 0x0001AC40
	public List<Vector3Int> GetDecoratedTilePositions
	{
		get
		{
			return this._decoratedTiles;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001CA48 File Offset: 0x0001AC48
	public bool IsGridSetup
	{
		get
		{
			return this._arraySetup;
		}
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0001CA50 File Offset: 0x0001AC50
	public void ToggleBuildingLayer(bool toggle)
	{
		this.secondaryTilemap.gameObject.SetActive(toggle);
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0001CA63 File Offset: 0x0001AC63
	public Color GetRandomClaimColor()
	{
		return this._reclaimedTileColor[Random.Range(0, this._reclaimedTileColor.Count)];
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001CA81 File Offset: 0x0001AC81
	public List<Color> GetReclaimerTileColors
	{
		get
		{
			return this._reclaimedTileColor;
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0001CA89 File Offset: 0x0001AC89
	public bool VerifyCoordinates(Vector2Int coords)
	{
		return coords.x > 0 && coords.x < this._regionSizeX && coords.y > 0 && coords.y < this._regionSizeY;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001CABF File Offset: 0x0001ACBF
	public bool VerifyCoordinates(Vector3Int coords)
	{
		return coords.x > 0 && coords.x < this._regionSizeX && coords.y > 0 && coords.y < this._regionSizeY;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0001CAF8 File Offset: 0x0001ACF8
	public void Setup(RegionData region)
	{
		this._reclaimedTileColor = new List<Color>();
		for (float num = 0.95f; num < 1f; num += 0.01f)
		{
			this._reclaimedTileColor.Add(new Color(region.claimedHexColor.r * num, region.claimedHexColor.g * num, region.claimedHexColor.b * num, 1f));
		}
		this.ToggleBuildingLayer(false);
		this.secondaryTilemap.gameObject.SetActive(false);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0001CB7C File Offset: 0x0001AD7C
	public void ClearTileMap()
	{
		for (int i = 0; i < this._gridData.GetLength(0); i++)
		{
			for (int j = 0; j < this._gridData.GetLength(1); j++)
			{
				this._gridData[i, j].Clear();
			}
		}
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0001CBCC File Offset: 0x0001ADCC
	public void SetGridBounds(int regionSizeX, int regionSizeY)
	{
		if (!this._arraySetup)
		{
			this._regionSizeX = regionSizeX;
			this._regionSizeY = regionSizeY;
			this._gridData = new CellData[regionSizeX, regionSizeY];
			for (int i = 0; i < regionSizeX; i++)
			{
				for (int j = 0; j < regionSizeY; j++)
				{
					this._gridData[i, j] = new CellData();
				}
			}
			this._arraySetup = true;
			if (this.enableDistanceCaching)
			{
				this._distanceCache = new DistanceCache(regionSizeX, regionSizeY);
			}
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0001CC44 File Offset: 0x0001AE44
	public virtual void SetCellBiomeIndex(Vector3Int coords, byte biomeIndex, Color color)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		this._gridData[coords.x, coords.y].SetBiomeIndex(biomeIndex);
		this.SetTileDesign(new TileDesign(Singleton<WorldGenerator>.Instance.RegionData.tile, color), coords, true);
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0001CC97 File Offset: 0x0001AE97
	public virtual byte GetCellBiomeIndex(Vector2Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return 0;
		}
		return this._gridData[coords.x, coords.y].GetBiomeIndex();
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0001CCC2 File Offset: 0x0001AEC2
	public virtual BiomeData GetCellBiomeData(Vector2Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return null;
		}
		return this._gridData[coords.x, coords.y].GetBiome();
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0001CCED File Offset: 0x0001AEED
	public virtual float CalculateDistance(Vector2Int startCell, Vector2Int endCell, bool useCache)
	{
		if (this.enableDistanceCaching && useCache)
		{
			return this._distanceCache.LookupDistance(startCell, endCell);
		}
		return Vector2.Distance(Utilities.ConvertCellPositionToWorld(startCell), Utilities.ConvertCellPositionToWorld(endCell));
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001CD18 File Offset: 0x0001AF18
	public virtual void SetCell(Vector2Int coords, Building building, bool setColor)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		this._gridData[coords.x, coords.y].SetOccupier(building);
		if (setColor)
		{
			Vector3Int vector3Int = new Vector3Int(coords.x, coords.y, 0);
			this.secondaryTilemap.SetTile(vector3Int, this.buildingTile);
			Singleton<MapView>.Instance.SetBuildingTile(vector3Int, building.GetModel.GetColor(AccentType.PrimaryColor));
		}
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0001CD90 File Offset: 0x0001AF90
	public void SetMultipleCells(List<Vector2Int> coords, Building building, bool setColor)
	{
		foreach (Vector2Int coords2 in coords)
		{
			this.SetCell(coords2, building, setColor);
		}
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001CDE0 File Offset: 0x0001AFE0
	public virtual void UpdateCell(Vector2Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		this._gridData[coords.x, coords.y].UpdateOccupier();
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0001CE0C File Offset: 0x0001B00C
	public void UpdateMultipleCells(List<Vector2Int> coords)
	{
		foreach (Vector2Int coords2 in coords)
		{
			this.UpdateCell(coords2);
		}
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0001CE5C File Offset: 0x0001B05C
	public virtual CellData GetCell(Vector2Int coords, bool verifyCoords = true)
	{
		if (verifyCoords && !this.VerifyCoordinates(coords))
		{
			return null;
		}
		return this._gridData[coords.x, coords.y];
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0001CE85 File Offset: 0x0001B085
	public virtual Building GetBuilding(Vector2Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return null;
		}
		return this._gridData[coords.x, coords.y].GetOccupier;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0001CEB0 File Offset: 0x0001B0B0
	public virtual void DestroyCell(Vector2Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		this._gridData[coords.x, coords.y].ClearOccupier();
		Vector3Int vector3Int = new Vector3Int(coords.x, coords.y, 0);
		this.secondaryTilemap.SetTile(vector3Int, null);
		Singleton<MapView>.Instance.ClearBuildingTile(vector3Int);
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0001CF14 File Offset: 0x0001B114
	public void DestroyMultipleCells(List<Vector2Int> coords)
	{
		foreach (Vector2Int coords2 in coords)
		{
			this.DestroyCell(coords2);
		}
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0001CF64 File Offset: 0x0001B164
	public virtual void SetTileDesign(TileDesign design, Vector3Int coords, bool original = false)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		CellData cellData = this._gridData[coords.x, coords.y];
		if (original)
		{
			if (cellData.HasOriginalDesign && cellData.OriginalDesign.data == design.data)
			{
				cellData.OriginalDesign.color = design.color;
			}
			else
			{
				cellData.OriginalDesign = design;
			}
		}
		else if (cellData.HasTileDesign && cellData.TileDesign.data == design.data)
		{
			cellData.TileDesign.color = design.color;
			Singleton<Events>.Instance.onTileDesignUpdate.Invoke(coords, design);
		}
		else
		{
			cellData.TileDesign = design;
			if (!this._decoratedTiles.Contains(coords))
			{
				this._decoratedTiles.Add(coords);
			}
			Singleton<Events>.Instance.onTileDesignPlaced.Invoke(coords, design);
		}
		this.UpdateTileDesign(coords);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0001D054 File Offset: 0x0001B254
	public virtual void ClearTileDesign(Vector3Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		if (this._gridData[coords.x, coords.y].HasTileDesign)
		{
			this._gridData[coords.x, coords.y].ClearTileDesign();
			Singleton<Events>.Instance.onTileDesignRemoved.Invoke(coords);
			if (this._decoratedTiles.Contains(coords))
			{
				this._decoratedTiles.Remove(coords);
			}
			this.UpdateTileDesign(coords);
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0001D0DC File Offset: 0x0001B2DC
	protected virtual void UpdateTileDesign(Vector3Int coords)
	{
		TileDesign tileDesign = this._gridData[coords.x, coords.y].GenerateDesign();
		this.primaryTilemap.SetTile(coords, this.FetchTile(tileDesign.data));
		this.primaryTilemap.SetTileFlags(coords, TileFlags.None);
		this.primaryTilemap.SetColor(coords, tileDesign.color);
		Singleton<MapView>.Instance.SetCellColor(coords, tileDesign.map);
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x0001D150 File Offset: 0x0001B350
	public virtual void ReclaimCell(Claimer claimer, Vector2Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		CellData cellData = this._gridData[coords.x, coords.y];
		cellData.AddClaimer(claimer);
		this.UpdateTileDesign(new Vector3Int(coords.x, coords.y, 0));
		Building getOccupier = cellData.GetOccupier;
		if (getOccupier != null && getOccupier != claimer && getOccupier.IsAlly(claimer.FactionID))
		{
			getOccupier.OnClaimed();
		}
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0001D1D0 File Offset: 0x0001B3D0
	public void ReclaimMultipleCells(Claimer claimer, List<Vector2Int> coords, bool updateResearch = true)
	{
		if (!updateResearch)
		{
			using (List<Vector2Int>.Enumerator enumerator = coords.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector2Int coords2 = enumerator.Current;
					this.ReclaimCell(claimer, coords2);
				}
				return;
			}
		}
		foreach (Vector2Int coords3 in coords)
		{
			this.ReclaimCell(claimer, coords3);
		}
		Singleton<Events>.Instance.onTileClaimUpdated.Invoke(coords.Count);
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0001D278 File Offset: 0x0001B478
	public virtual void UnclaimCell(Claimer claimer, Vector2Int coords, Entity damager = null)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		Vector3Int coords2 = new Vector3Int(coords.x, coords.y, 0);
		CellData cellData = this._gridData[coords.x, coords.y];
		cellData.RemoveClaimer(claimer);
		this.UpdateTileDesign(coords2);
		if (cellData.GetClaimerAmount == 0)
		{
			Building getOccupier = cellData.GetOccupier;
			if (getOccupier != null && getOccupier != claimer && getOccupier.IsAlly(claimer.FactionID))
			{
				getOccupier.OnUnclaimed(damager);
			}
			if (cellData.HasTileDesign)
			{
				this.ClearTileDesign(coords2);
			}
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0001D314 File Offset: 0x0001B514
	public void UnclaimMultipleCells(Claimer claimer, List<Vector2Int> coords, Entity damager = null, bool updateResearch = true)
	{
		if (!updateResearch)
		{
			using (List<Vector2Int>.Enumerator enumerator = coords.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector2Int coords2 = enumerator.Current;
					this.UnclaimCell(claimer, coords2, damager);
				}
				return;
			}
		}
		foreach (Vector2Int coords3 in coords)
		{
			this.UnclaimCell(claimer, coords3, damager);
		}
		Singleton<Events>.Instance.onTileClaimUpdated.Invoke(-coords.Count);
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
	public virtual bool IsCellClaimed(Vector2Int coords)
	{
		return !Singleton<Gamemode>.Instance.UseReclaimers || (this.VerifyCoordinates(coords) && this._gridData[coords.x, coords.y].GetClaimerAmount > 0);
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0001D3FC File Offset: 0x0001B5FC
	public virtual bool IsCellClaimed(Vector3Int coords)
	{
		return !Singleton<Gamemode>.Instance.UseReclaimers || (this.VerifyCoordinates(coords) && this._gridData[coords.x, coords.y].GetClaimerAmount > 0);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0001D438 File Offset: 0x0001B638
	public bool AreCellsClaimed(List<Vector2Int> coords)
	{
		if (!Singleton<Gamemode>.Instance.UseReclaimers)
		{
			return true;
		}
		foreach (Vector2Int coords2 in coords)
		{
			if (!this.IsCellClaimed(coords2))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0001D4A0 File Offset: 0x0001B6A0
	public bool IsClaimerValid(Vector2 position, int range)
	{
		if (!Singleton<Gamemode>.Instance.UseReclaimers)
		{
			return true;
		}
		foreach (Vector2Int coords in Utilities.CalculateTileRange(position, range + 1))
		{
			if (this.IsCellClaimed(coords))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0001D510 File Offset: 0x0001B710
	public virtual bool IsCellOccupied(Vector2Int coords)
	{
		return this.VerifyCoordinates(coords) && this._gridData[coords.x, coords.y].IsOccupied;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001D53C File Offset: 0x0001B73C
	public bool AreCellsOccupied(List<Vector2Int> cells)
	{
		foreach (Vector2Int coords in cells)
		{
			if (this.IsCellOccupied(coords))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0001D594 File Offset: 0x0001B794
	public virtual bool IsCellValid(Vector2Int coords)
	{
		return this.VerifyCoordinates(coords) && !this._gridData[coords.x, coords.y].IsOccupied;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0001D5C4 File Offset: 0x0001B7C4
	public bool AreCellsValid(List<Vector2Int> cells)
	{
		foreach (Vector2Int coords in cells)
		{
			if (!this.IsCellValid(coords))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0001D61C File Offset: 0x0001B81C
	public virtual T IsCellOccupiedByType<T>(Vector2Int coords) where T : BuildingComponent
	{
		if (!this.VerifyCoordinates(coords))
		{
			return default(T);
		}
		Building getOccupier = this._gridData[coords.x, coords.y].GetOccupier;
		if (getOccupier != null)
		{
			return getOccupier.GetComponent<T>();
		}
		return default(T);
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0001D674 File Offset: 0x0001B874
	public virtual void SetResourceTile(Vector3Int coords, ResourceData resource)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		this._gridData[coords.x, coords.y].Resource = resource;
		this.UpdateTileDesign(coords);
		if (!this._resourceTiles.Contains(coords))
		{
			this._resourceTiles.Add(coords);
		}
		Singleton<Events>.Instance.onResourceTilePlaced.Invoke(coords, resource);
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0001D6DC File Offset: 0x0001B8DC
	public virtual void ClearResourceTile(Vector3Int coords)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return;
		}
		this._gridData[coords.x, coords.y].Resource = null;
		this.UpdateTileDesign(coords);
		if (this._resourceTiles.Contains(coords))
		{
			this._resourceTiles.Remove(coords);
		}
		Singleton<Events>.Instance.onResourceTileRemoved.Invoke(coords);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001D744 File Offset: 0x0001B944
	public virtual bool HasAnyResource(Vector3Int cell)
	{
		return this.VerifyCoordinates(cell) && this._gridData[cell.x, cell.y].HasResource;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0001D770 File Offset: 0x0001B970
	public bool HasResources(List<Vector2Int> cells, string resourceID, bool checkIfUnlocked)
	{
		foreach (Vector2Int coords in cells)
		{
			if (!this.HasResource(coords, resourceID, checkIfUnlocked))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0001D7CC File Offset: 0x0001B9CC
	public virtual bool HasResource(Vector2Int coords, string resourceID, bool checkIfUnlocked)
	{
		if (!this.VerifyCoordinates(coords))
		{
			return false;
		}
		CellData cellData = this._gridData[coords.x, coords.y];
		return cellData.HasResource && cellData.Resource.ID == resourceID && (!checkIfUnlocked || Singleton<Research>.Instance.IsResourceUnlocked(cellData.Resource));
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0001D834 File Offset: 0x0001BA34
	public virtual bool HasResources(List<Vector2Int> cells, bool checkIfUnlocked)
	{
		foreach (Vector2Int vector2Int in cells)
		{
			if (!this.HasResource(new Vector3Int(vector2Int.x, vector2Int.y, 0), checkIfUnlocked))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001D8A0 File Offset: 0x0001BAA0
	public virtual bool HasResource(Vector3Int cell, bool checkIfUnlocked)
	{
		return this.VerifyCoordinates(cell) && this._gridData[cell.x, cell.y].HasResource && (!checkIfUnlocked || Singleton<Research>.Instance.IsResourceUnlocked(this._gridData[cell.x, cell.y].Resource));
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0001D907 File Offset: 0x0001BB07
	public virtual ResourceData GetResource(Vector2Int cell)
	{
		if (!this.VerifyCoordinates(cell))
		{
			return null;
		}
		return this._gridData[cell.x, cell.y].Resource;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001D934 File Offset: 0x0001BB34
	public virtual void CheckTilesForAvailableDrone<T>(List<Vector2Int> cells, CoverageType type, ref List<T> drones) where T : Drone
	{
		foreach (Vector2Int position in cells)
		{
			this.CheckTileForAvailableDrone<T>(position, type, ref drones);
		}
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0001D984 File Offset: 0x0001BB84
	public virtual void CheckTileForAvailableDrone<T>(Vector2Int position, CoverageType type, ref List<T> drones) where T : Drone
	{
		if (this.VerifyCoordinates(position))
		{
			this._gridData[position.x, position.y].GetDronesOfType<T>(type, ref drones);
		}
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0001D9B0 File Offset: 0x0001BBB0
	public virtual DroneCoverage RegisterDroneCoverage(DroneCoverage coverage)
	{
		for (int i = coverage.area.startX; i <= coverage.area.endX; i++)
		{
			for (int j = coverage.area.startY; j <= coverage.area.endY; j++)
			{
				Vector2Int coords = new Vector2Int(i, j);
				if (this.VerifyCoordinates(coords))
				{
					this._gridData[coords.x, coords.y].RegisterDroneCoverage(coverage);
				}
			}
		}
		return coverage;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0001DA30 File Offset: 0x0001BC30
	public virtual void ClearDroneCoverage(DroneCoverage coverage)
	{
		for (int i = coverage.area.startX; i <= coverage.area.endX; i++)
		{
			for (int j = coverage.area.startY; j <= coverage.area.endY; j++)
			{
				Vector2Int coords = new Vector2Int(i, j);
				if (this.VerifyCoordinates(coords))
				{
					this._gridData[coords.x, coords.y].ClearDroneCoverage(coverage);
				}
			}
		}
	}

	// Token: 0x04000310 RID: 784
	protected CellData[,] _gridData;

	// Token: 0x04000311 RID: 785
	private Dictionary<TileDesignData, TileBase> _tileDesigns = new Dictionary<TileDesignData, TileBase>();

	// Token: 0x04000312 RID: 786
	private List<Vector3Int> _resourceTiles = new List<Vector3Int>();

	// Token: 0x04000313 RID: 787
	private List<Vector3Int> _decoratedTiles = new List<Vector3Int>();

	// Token: 0x04000314 RID: 788
	protected int _regionSizeX;

	// Token: 0x04000315 RID: 789
	protected int _regionSizeY;

	// Token: 0x04000316 RID: 790
	protected DistanceCache _distanceCache;

	// Token: 0x04000317 RID: 791
	protected bool _arraySetup;

	// Token: 0x04000318 RID: 792
	[Header("Grid Variables")]
	public TileBase buildingTile;

	// Token: 0x04000319 RID: 793
	public Tilemap primaryTilemap;

	// Token: 0x0400031A RID: 794
	public Tilemap secondaryTilemap;

	// Token: 0x0400031B RID: 795
	public bool enableDistanceCaching;

	// Token: 0x0400031C RID: 796
	protected List<Color> _reclaimedTileColor = new List<Color>();
}
