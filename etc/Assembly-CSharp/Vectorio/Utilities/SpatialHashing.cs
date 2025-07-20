using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Utilities
{
	// Token: 0x02000284 RID: 644
	public class SpatialHashing
	{
		// Token: 0x06001232 RID: 4658 RVA: 0x00053497 File Offset: 0x00051697
		public List<Vector2Int> GetHash(byte index)
		{
			if (this._spatialHash.ContainsKey(index))
			{
				return this._spatialHash[index];
			}
			return new List<Vector2Int>();
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000534BC File Offset: 0x000516BC
		public SpatialHashing(int cellSize, int worldSize)
		{
			this.CellSize = cellSize;
			this.HalfCell = cellSize / 2;
			this.WorldSize = worldSize;
			this._spatialHash = new Dictionary<byte, List<Vector2Int>>();
			int num = Mathf.CeilToInt((float)this.WorldSize / (float)this.CellSize);
			this._centerCell = new Vector2Int(num / 2, num / 2);
			for (int i = 1; i < num; i++)
			{
				for (int j = 1; j < num; j++)
				{
					Vector2Int vector2Int = new Vector2Int(i, j);
					Vector2Int coords = Utilities.ConvertHashPositionToCell(vector2Int, this.CellSize);
					byte cellBiomeIndex = Singleton<TileGrid>.Instance.GetCellBiomeIndex(coords);
					if (!this._spatialHash.ContainsKey(cellBiomeIndex))
					{
						this._spatialHash[cellBiomeIndex] = new List<Vector2Int>();
					}
					this._spatialHash[cellBiomeIndex].Add(vector2Int);
				}
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00053588 File Offset: 0x00051788
		public bool TryGetCellsInBiome(byte biomeIndex, out List<Vector2Int> cells, int minRange = 0, int maxRange = 1000)
		{
			List<Vector2Int> list;
			if (this._spatialHash.TryGetValue(biomeIndex, out list))
			{
				this._boundary.MinCloseX = this._centerCell.x - minRange;
				this._boundary.MaxCloseX = this._centerCell.x + minRange;
				this._boundary.MinCloseY = this._centerCell.y - minRange;
				this._boundary.MaxCloseY = this._centerCell.y + minRange;
				this._boundary.MinFarX = this._centerCell.x - maxRange;
				this._boundary.MaxFarX = this._centerCell.x + maxRange;
				this._boundary.MinFarY = this._centerCell.y - maxRange;
				this._boundary.MaxFarY = this._centerCell.y + maxRange;
				cells = new List<Vector2Int>();
				foreach (Vector2Int item in list)
				{
					if ((item.x <= this._boundary.MinCloseX || item.x >= this._boundary.MaxCloseX || item.y <= this._boundary.MinCloseY || item.y >= this._boundary.MaxCloseY) && item.x >= this._boundary.MinFarX && item.x <= this._boundary.MaxFarX && item.y >= this._boundary.MinFarY && item.y <= this._boundary.MaxFarY)
					{
						cells.Add(item);
					}
				}
				return cells.Count > 0;
			}
			Debug.Log("[SPATIAL HASH] No biome with index " + biomeIndex.ToString() + " was setup!");
			cells = null;
			return false;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000537A0 File Offset: 0x000519A0
		public void RemoveCellFromBiome(byte biomeIndex, Vector2Int cell)
		{
			List<Vector2Int> list;
			if (this._spatialHash.TryGetValue(biomeIndex, out list))
			{
				list.Remove(cell);
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000537C8 File Offset: 0x000519C8
		public List<Vector2Int> GenerateRandomPositionsInCell(Vector2Int cell, int numberOfPositions, int minDistance, int maxDistance)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			minDistance = Mathf.Clamp(minDistance, 0, this.HalfCell);
			maxDistance = Mathf.Clamp(maxDistance, minDistance, this.HalfCell);
			int num = cell.x * this.CellSize;
			int num2 = cell.y * this.CellSize;
			for (int i = 0; i < numberOfPositions; i++)
			{
				Vector2Int vector2Int;
				switch (Random.Range(0, 4))
				{
				case 0:
					vector2Int = new Vector2Int(Random.Range(num - maxDistance, num + maxDistance), Random.Range(num2 + minDistance, num2 + maxDistance));
					break;
				case 1:
					vector2Int = new Vector2Int(Random.Range(num + minDistance, num + maxDistance), Random.Range(num2 - maxDistance, num2 + maxDistance));
					break;
				case 2:
					vector2Int = new Vector2Int(Random.Range(num - maxDistance, num + maxDistance), Random.Range(num2 - maxDistance, num2 - minDistance));
					break;
				case 3:
					vector2Int = new Vector2Int(Random.Range(num - maxDistance, num - minDistance), Random.Range(num2 - maxDistance, num2 + maxDistance));
					break;
				default:
					vector2Int = new Vector2Int(num, num2);
					break;
				}
				Vector2Int item = vector2Int;
				list.Add(item);
			}
			return list;
		}

		// Token: 0x04000FF0 RID: 4080
		public readonly int CellSize;

		// Token: 0x04000FF1 RID: 4081
		public readonly int HalfCell;

		// Token: 0x04000FF2 RID: 4082
		public readonly int WorldSize;

		// Token: 0x04000FF3 RID: 4083
		private readonly Vector2Int _centerCell;

		// Token: 0x04000FF4 RID: 4084
		private readonly Dictionary<byte, List<Vector2Int>> _spatialHash;

		// Token: 0x04000FF5 RID: 4085
		private SpatialHashing.Boundary _boundary;

		// Token: 0x02000285 RID: 645
		private struct Boundary
		{
			// Token: 0x06001237 RID: 4663 RVA: 0x000538F0 File Offset: 0x00051AF0
			public override string ToString()
			{
				return string.Format("MinCloseX: {0}, MaxCloseX: {1}, MinCloseY: {2}, MaxCloseY: {3}, ", new object[]
				{
					this.MinCloseX,
					this.MaxCloseX,
					this.MinCloseY,
					this.MaxCloseY
				}) + string.Format("MinFarX: {0}, MaxFarX: {1}, MinFarY: {2}, MaxFarY: {3}", new object[]
				{
					this.MinFarX,
					this.MaxFarX,
					this.MinFarY,
					this.MaxFarY
				});
			}

			// Token: 0x04000FF6 RID: 4086
			public int MinCloseX;

			// Token: 0x04000FF7 RID: 4087
			public int MaxCloseX;

			// Token: 0x04000FF8 RID: 4088
			public int MinCloseY;

			// Token: 0x04000FF9 RID: 4089
			public int MaxCloseY;

			// Token: 0x04000FFA RID: 4090
			public int MinFarX;

			// Token: 0x04000FFB RID: 4091
			public int MaxFarX;

			// Token: 0x04000FFC RID: 4092
			public int MinFarY;

			// Token: 0x04000FFD RID: 4093
			public int MaxFarY;
		}
	}
}
