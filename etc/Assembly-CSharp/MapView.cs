using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

// Token: 0x0200017F RID: 383
public class MapView : Singleton<MapView>
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0003652C File Offset: 0x0003472C
	public static bool IsEnabled
	{
		get
		{
			return MapView._isEnabled;
		}
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00036534 File Offset: 0x00034734
	public void Start()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 1f);
		Singleton<Events>.Instance.onToggleGrid.AddListener(new UnityAction<bool>(this.ToggleInverse));
		MapView._isEnabled = false;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x00036598 File Offset: 0x00034798
	public void ClearMultipleBuildingTiles(List<Vector2Int> cells)
	{
		foreach (Vector2Int v in cells)
		{
			this.ClearBuildingTile((Vector3Int)v);
		}
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x000365EC File Offset: 0x000347EC
	public void ClearBuildingTile(Vector3Int cell)
	{
		if (this.lowResBuildingTilemap.HasTile(cell))
		{
			this.lowResBuildingTilemap.SetTile(cell, null);
		}
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0003660C File Offset: 0x0003480C
	public void SetMultipleBuildingTiles(Vector2Int origin, List<Vector2Int> cells, Color color)
	{
		Vector3Int a = (Vector3Int)origin;
		foreach (Vector2Int v in cells)
		{
			this.SetBuildingTile(a + (Vector3Int)v, color);
		}
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x00036670 File Offset: 0x00034870
	public void SetBuildingTile(Vector3Int tilePos, Color color)
	{
		this.lowResBuildingTilemap.SetTile(tilePos, this.lowResTile);
		this.lowResBuildingTilemap.SetTileFlags(tilePos, TileFlags.None);
		this.lowResBuildingTilemap.SetColor(tilePos, color);
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x0003669E File Offset: 0x0003489E
	public void EnableFogTile(Vector3Int tilePos)
	{
		this.lowResFogTilemap.SetTile(tilePos, this.lowResTile);
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x000366B2 File Offset: 0x000348B2
	public void DisableFogTile(Vector3Int tilePos)
	{
		this.lowResFogTilemap.SetTile(tilePos, null);
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x000366C1 File Offset: 0x000348C1
	public void SetFogColor(Vector3Int tilePos, Color color)
	{
		this.lowResFogTilemap.SetTileFlags(tilePos, TileFlags.None);
		this.lowResFogTilemap.SetColor(tilePos, color);
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x000366DD File Offset: 0x000348DD
	public void SetCellColor(Vector3Int tilePos, Color color)
	{
		if (!this.lowResWorldTilemap.HasTile(tilePos))
		{
			this.lowResWorldTilemap.SetTile(tilePos, this.lowResTile);
			this.lowResWorldTilemap.SetTileFlags(tilePos, TileFlags.None);
		}
		this.lowResWorldTilemap.SetColor(tilePos, color);
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x0003671C File Offset: 0x0003491C
	public Color GetCellColor(Vector3Int tilePos)
	{
		if (this.lowResFogTilemap.HasTile(tilePos))
		{
			return this.lowResFogTilemap.GetColor(tilePos);
		}
		if (this.lowResBuildingTilemap.HasTile(tilePos))
		{
			return this.lowResBuildingTilemap.GetColor(tilePos);
		}
		if (this.lowResWorldTilemap.HasTile(tilePos))
		{
			return this.lowResWorldTilemap.GetColor(tilePos);
		}
		return Color.grey;
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x0003677F File Offset: 0x0003497F
	public void ClearCellColor(Vector3Int coords)
	{
		this.lowResWorldTilemap.SetTile(coords, null);
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x0003678E File Offset: 0x0003498E
	private void ToggleInverse(bool toggle)
	{
		this.ToggleMapView(!toggle);
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0003679A File Offset: 0x0003499A
	public void ToggleMapView(bool toggle)
	{
		CameraFollow.zPosition = (toggle ? 1 : 0);
		MapView._isEnabled = toggle;
	}

	// Token: 0x04000871 RID: 2161
	private static bool _isEnabled;

	// Token: 0x04000872 RID: 2162
	public Tilemap lowResWorldTilemap;

	// Token: 0x04000873 RID: 2163
	public Tilemap lowResBuildingTilemap;

	// Token: 0x04000874 RID: 2164
	public Tilemap lowResFogTilemap;

	// Token: 0x04000875 RID: 2165
	public TileBase lowResTile;
}
