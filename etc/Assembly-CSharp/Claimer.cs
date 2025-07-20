using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x020000FF RID: 255
public class Claimer : BuildingComponent, IComponent<Claimer, ClaimerData>
{
	// Token: 0x06000836 RID: 2102 RVA: 0x0002459F File Offset: 0x0002279F
	public ClaimerData GetData()
	{
		return this._claimerData;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x000245A7 File Offset: 0x000227A7
	public void OnInitialize(ClaimerData data)
	{
		this._claimerData = data;
		this._tileClaimRange = data.range;
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x000245BC File Offset: 0x000227BC
	public override void OnSpawn(bool fromSave)
	{
		this._claimedTiles = Utilities.CalculateTileRange(base.transform.position, this._tileClaimRange);
		Singleton<TileGrid>.Instance.ReclaimMultipleCells(this, this._claimedTiles, Singleton<Gamemode>.Instance.UseResearch);
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x000245FA File Offset: 0x000227FA
	public override void OnReset()
	{
		Singleton<TileGrid>.Instance.UnclaimMultipleCells(this, this._claimedTiles, null, Singleton<Gamemode>.Instance.UseResearch);
	}

	// Token: 0x0400055E RID: 1374
	private ClaimerData _claimerData;

	// Token: 0x0400055F RID: 1375
	protected List<Vector2Int> _claimedTiles;

	// Token: 0x04000560 RID: 1376
	protected int _tileClaimRange;
}
