using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vectorio.Utilities;

// Token: 0x02000067 RID: 103
public class InfiniteGrid : TileGrid
{
	// Token: 0x060004E2 RID: 1250 RVA: 0x00019E73 File Offset: 0x00018073
	public override float CalculateDistance(Vector2Int startCell, Vector2Int endCell, bool useCache)
	{
		return Vector2.Distance(Utilities.ConvertCellPositionToWorld(startCell), Utilities.ConvertCellPositionToWorld(endCell));
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00019E86 File Offset: 0x00018086
	public CellData RequestCell(Vector3Int coords)
	{
		return this.RequestCell(new Vector2Int(coords.x, coords.y));
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00019EA4 File Offset: 0x000180A4
	public CellData RequestCell(Vector2Int coords)
	{
		if (!this._infiniteGrid.ContainsKey(coords))
		{
			CellData cellData = new CellData();
			this._infiniteGrid.Add(coords, cellData);
			return cellData;
		}
		return this._infiniteGrid[coords];
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00019EE0 File Offset: 0x000180E0
	public override void SetCell(Vector2Int coords, Building building, bool setColor)
	{
		this.RequestCell(coords).SetOccupier(building);
		if (setColor)
		{
			Vector3Int tilePos = new Vector3Int(coords.x, coords.y, 0);
			Singleton<MapView>.Instance.SetBuildingTile(tilePos, building.GetModel.GetColor(AccentType.PrimaryColor));
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00019F2C File Offset: 0x0001812C
	public override void SetTileDesign(TileDesign design, Vector3Int coords, bool original = false)
	{
		this.primaryTilemap.SetTile(coords, base.FetchTile(design.data));
		this.primaryTilemap.SetTileFlags(coords, TileFlags.None);
		this.primaryTilemap.SetColor(coords, design.color);
		Singleton<MapView>.Instance.SetCellColor(coords, design.color);
		if (original)
		{
			this.RequestCell(coords).OriginalDesign = design;
			return;
		}
		this.RequestCell(coords).TileDesign = design;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00019FA0 File Offset: 0x000181A0
	protected override void UpdateTileDesign(Vector3Int coords)
	{
		TileDesign tileDesign = this.RequestCell(coords).GenerateDesign();
		this.primaryTilemap.SetTile(coords, base.FetchTile(tileDesign.data));
		this.primaryTilemap.SetTileFlags(coords, TileFlags.None);
		this.primaryTilemap.SetColor(coords, tileDesign.color);
		Singleton<MapView>.Instance.SetCellColor(coords, tileDesign.color);
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00003212 File Offset: 0x00001412
	public override void ReclaimCell(Claimer claimer, Vector2Int coords)
	{
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00003212 File Offset: 0x00001412
	public override void UnclaimCell(Claimer claimer, Vector2Int coords, Entity damager = null)
	{
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0001A002 File Offset: 0x00018202
	public override bool IsCellClaimed(Vector2Int coords)
	{
		return this.RequestCell(coords).GetClaimerAmount > 0;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0001A013 File Offset: 0x00018213
	public override CellData GetCell(Vector2Int coords, bool check = false)
	{
		return this.RequestCell(coords);
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0001A01C File Offset: 0x0001821C
	public override Building GetBuilding(Vector2Int coords)
	{
		return this.RequestCell(coords).GetOccupier;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0001A02C File Offset: 0x0001822C
	public override void DestroyCell(Vector2Int coords)
	{
		this.RequestCell(coords).ClearOccupier();
		Vector3Int cell = new Vector3Int(coords.x, coords.y, 0);
		Singleton<MapView>.Instance.ClearBuildingTile(cell);
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0001A066 File Offset: 0x00018266
	public override bool IsCellOccupied(Vector2Int coords)
	{
		return this.RequestCell(coords).IsOccupied;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0001A074 File Offset: 0x00018274
	public override T IsCellOccupiedByType<T>(Vector2Int coords)
	{
		CellData cellData = this.RequestCell(coords);
		if (cellData.GetOccupier != null)
		{
			return cellData.GetOccupier.GetComponent<T>();
		}
		return default(T);
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0001A0AC File Offset: 0x000182AC
	public override void SetResourceTile(Vector3Int coords, ResourceData resource)
	{
		this.primaryTilemap.SetTile(coords, base.FetchTile(resource.Tile));
		Singleton<MapView>.Instance.SetCellColor(coords, resource.Accent.primaryColor);
		this.RequestCell(coords).Resource = resource;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0001A0EC File Offset: 0x000182EC
	public override bool HasResource(Vector2Int coords, string resourceID, bool checkIfUnlocked)
	{
		CellData cellData = this.RequestCell(coords);
		return cellData.HasResource && cellData.Resource.ID == resourceID && (!checkIfUnlocked || Singleton<Research>.Instance.IsResourceUnlocked(cellData.Resource));
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0001A135 File Offset: 0x00018335
	public override ResourceData GetResource(Vector2Int cell)
	{
		return this.RequestCell(cell).Resource;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0001A143 File Offset: 0x00018343
	public override void CheckTileForAvailableDrone<T>(Vector2Int position, CoverageType type, ref List<T> drones)
	{
		this.RequestCell(position).GetDronesOfType<T>(type, ref drones);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0001A154 File Offset: 0x00018354
	public override DroneCoverage RegisterDroneCoverage(DroneCoverage coverage)
	{
		for (int i = coverage.area.startX; i <= coverage.area.endX; i++)
		{
			for (int j = coverage.area.startY; j <= coverage.area.endY; j++)
			{
				Vector2Int coords = new Vector2Int(i, j);
				this.RequestCell(coords).RegisterDroneCoverage(coverage);
			}
		}
		return coverage;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0001A1B8 File Offset: 0x000183B8
	public override void ClearDroneCoverage(DroneCoverage coverage)
	{
		if (coverage == null)
		{
			Debug.Log("[TILE GRID] No valid coverage map provided!");
			return;
		}
		for (int i = coverage.area.startX; i <= coverage.area.endX; i++)
		{
			for (int j = coverage.area.startY; j <= coverage.area.endY; j++)
			{
				Vector2Int coords = new Vector2Int(i, j);
				this.RequestCell(coords).ClearDroneCoverage(coverage);
			}
		}
	}

	// Token: 0x040002AD RID: 685
	public Dictionary<Vector2Int, CellData> _infiniteGrid = new Dictionary<Vector2Int, CellData>();
}
