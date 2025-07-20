using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x020001B2 RID: 434
public class DistanceCache
{
	// Token: 0x06000DFB RID: 3579 RVA: 0x0003E251 File Offset: 0x0003C451
	public DistanceCache(int gridWidth, int gridHeight)
	{
		this._table = new Dictionary<Vector2Int, float>[gridWidth, gridHeight];
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x00019E73 File Offset: 0x00018073
	private float CalculateDistance(Vector2Int startCell, Vector2Int endCell)
	{
		return Vector2.Distance(Utilities.ConvertCellPositionToWorld(startCell), Utilities.ConvertCellPositionToWorld(endCell));
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0003E268 File Offset: 0x0003C468
	public float LookupDistance(Vector2Int startCell, Vector2Int endCell)
	{
		if (this._table[startCell.x, startCell.y] != null)
		{
			if (this._table[startCell.x, startCell.y].ContainsKey(endCell))
			{
				return this._table[startCell.x, startCell.y][endCell];
			}
			this._table[startCell.x, startCell.y] = new Dictionary<Vector2Int, float>();
		}
		float num = this.CalculateDistance(startCell, endCell);
		this._table[startCell.x, startCell.y].Add(endCell, num);
		return num;
	}

	// Token: 0x04000A92 RID: 2706
	private Dictionary<Vector2Int, float>[,] _table;
}
