using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Serialization;
using Vectorio.Utilities;

namespace Vectorio.Generation
{
	// Token: 0x02000282 RID: 642
	public class WorldSpawner : MonoBehaviour
	{
		// Token: 0x0600122A RID: 4650 RVA: 0x00052D58 File Offset: 0x00050F58
		public void Setup(WorldGenerator worldGenerator)
		{
			this._worldGenerator = worldGenerator;
			this._spatialHash = new SpatialHashing(this._worldGenerator.RegionData.cellSize, this._worldGenerator.RegionData.worldSize);
			if (this._debug)
			{
				int num = this._worldGenerator.RegionData.cellSize / 2;
				int num2 = Mathf.CeilToInt((float)this._worldGenerator.RegionData.worldSize / (float)this._worldGenerator.RegionData.cellSize);
				Vector2Int lhs = new Vector2Int(num2 / 2, num2 / 2);
				foreach (Vector2Int rhs in this._spatialHash.GetHash(0))
				{
					int cellSize = this._worldGenerator.RegionData.cellSize;
					Vector2Int vector2Int = new Vector2Int(rhs.x * cellSize, rhs.y * cellSize);
					Vector2 v = Utilities.ConvertCellPositionToWorld(new Vector2Int(vector2Int.x - num, vector2Int.y - num));
					Vector2 v2 = Utilities.ConvertCellPositionToWorld(new Vector2Int(vector2Int.x + num - 1, vector2Int.y + num - 1));
					Object.Instantiate<GameObject>(LegacyLibrary.DEBUG_MARKER_ONE, v, Quaternion.identity);
					Object.Instantiate<GameObject>(LegacyLibrary.DEBUG_MARKER_TWO, v2, Quaternion.identity);
					if (lhs == rhs)
					{
						Vector2 v3 = Utilities.ConvertCellPositionToWorld(new Vector2Int(vector2Int.x, vector2Int.y));
						Object.Instantiate<GameObject>(LegacyLibrary.DEBUG_MARKER_THREE, v3, Quaternion.identity);
					}
				}
			}
			this._nodeDict = new Dictionary<NodeType, List<GameObject>>();
			foreach (WorldSpawner.ResourceNodePrefab resourceNodePrefab in this.nodes)
			{
				this._nodeDict.Add(resourceNodePrefab.nodeType, resourceNodePrefab.nodes);
			}
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00052F70 File Offset: 0x00051170
		public void SpawnFeature(byte biomeIndex, WorldFeature feature)
		{
			if (!feature.isEnabled)
			{
				return;
			}
			int minRange = feature.useMinDistance ? feature.minCellDistance : 0;
			int maxRange = feature.useMaxDistance ? feature.maxCellDistance : 1000;
			List<Vector2Int> list;
			if (!this._spatialHash.TryGetCellsInBiome(biomeIndex, out list, minRange, maxRange) || list.Count == 0)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[SPAWNER] No available cells in biome ",
					biomeIndex.ToString(),
					" for ",
					feature.featureName,
					"!"
				}));
				return;
			}
			for (int i = 0; i < ((feature.cellsToPick > list.Count) ? list.Count : feature.cellsToPick); i++)
			{
				Vector2Int cell = list[Random.Range(0, list.Count)];
				List<Vector2Int> list2 = null;
				bool flag = false;
				if (feature.hasResourceFeature && feature.resourceFeature != null)
				{
					list2 = this._spatialHash.GenerateRandomPositionsInCell(cell, feature.resourceFeature.amountToSpawn, feature.resourceFeature.minDistance, feature.resourceFeature.maxDistance);
					this.SpawnResource(feature.resourceFeature, list2);
					flag = true;
				}
				if (feature.hasEntityFeature && feature.entityFeature != null)
				{
					if (!feature.linkEntityToResource || list2 == null)
					{
						list2 = this._spatialHash.GenerateRandomPositionsInCell(cell, feature.entityFeature.amountToSpawn, feature.resourceFeature.minDistance, feature.resourceFeature.maxDistance);
					}
					this.SpawnEntity(feature.entityFeature, list2);
					flag = true;
				}
				if (feature.hasOutpostFeature && feature.outpostFeature != null)
				{
					list2 = this._spatialHash.GenerateRandomPositionsInCell(cell, feature.outpostFeature.amountToSpawn, feature.resourceFeature.minDistance, feature.resourceFeature.maxDistance);
					this.SpawnOutpost(feature.outpostFeature, list2);
					flag = true;
				}
				if (flag)
				{
					this._spatialHash.RemoveCellFromBiome(biomeIndex, cell);
					if (feature.stripNearbyCells)
					{
						for (int j = cell.x - feature.stripRange; j <= cell.x + feature.stripRange; j++)
						{
							for (int k = cell.y - feature.stripRange; k <= cell.y + feature.stripRange; k++)
							{
								Vector2Int cell2 = new Vector2Int(j, k);
								this._spatialHash.RemoveCellFromBiome(biomeIndex, cell2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000531D4 File Offset: 0x000513D4
		private void SpawnEntity(WorldFeature.EntityFeature feature, List<Vector2Int> positions)
		{
			List<VariableContainer> list = new List<VariableContainer>();
			foreach (VariableObject variableObject in feature.variables)
			{
				list.Add(new VariableContainer(variableObject));
			}
			foreach (Vector2Int cellPosition in positions)
			{
				Vector2 position = Utilities.ConvertCellPositionToWorld(cellPosition);
				EntityCreationData creationData = EventBuilder.BuildCreationData(feature.data.ID, feature.faction.ID, position, SyncType.None);
				EventBuilder.ApplyVariablesToCreationData(ref creationData, list);
				Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000532A4 File Offset: 0x000514A4
		private void SpawnResource(WorldFeature.ResourceFeature feature, List<Vector2Int> positions)
		{
			foreach (Vector2Int vector2Int in positions)
			{
				GameObject gameObject;
				if (!feature.useUniqueNodes)
				{
					List<GameObject> list = this._nodeDict[feature.nodeType];
					gameObject = list[Random.Range(0, list.Count)];
				}
				else
				{
					gameObject = feature.nodes[Random.Range(0, this.nodes.Count)];
				}
				foreach (object obj in gameObject.transform)
				{
					Transform transform = (Transform)obj;
					Vector3Int vector3Int = new Vector3Int(vector2Int.x + (int)transform.localPosition.x, vector2Int.y + (int)transform.localPosition.y, 0);
					if (!Singleton<TileGrid>.Instance.HasAnyResource(vector3Int))
					{
						Singleton<TileGrid>.Instance.SetResourceTile(vector3Int, feature.data);
					}
				}
			}
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000533D8 File Offset: 0x000515D8
		private void SpawnOutpost(WorldFeature.OutpostFeature feature, List<Vector2Int> positions)
		{
			if (feature.outposts.Count == 0)
			{
				return;
			}
			TextAsset textAsset = feature.outposts[Random.Range(0, feature.outposts.Count)];
			if (feature.outposts == null || textAsset.bytes == null)
			{
				return;
			}
			OutpostData outpost = DataProcessor.DecompressAndDeserializeObject<OutpostData>(textAsset.bytes);
			foreach (Vector2Int tileCoords in positions)
			{
				Singleton<OutpostCreator>.Instance.CreateOutpost(outpost, tileCoords);
			}
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00053474 File Offset: 0x00051674
		public void OnEntityCreated(Entity entity)
		{
			entity.Set_EFlag_IsWorldFeature(true);
			entity.IsSaveable = false;
		}

		// Token: 0x04000FE9 RID: 4073
		protected WorldGenerator _worldGenerator;

		// Token: 0x04000FEA RID: 4074
		private SpatialHashing _spatialHash;

		// Token: 0x04000FEB RID: 4075
		[SerializeField]
		public List<WorldSpawner.ResourceNodePrefab> nodes;

		// Token: 0x04000FEC RID: 4076
		private Dictionary<NodeType, List<GameObject>> _nodeDict = new Dictionary<NodeType, List<GameObject>>();

		// Token: 0x04000FED RID: 4077
		private bool _debug;

		// Token: 0x02000283 RID: 643
		[Serializable]
		public class ResourceNodePrefab
		{
			// Token: 0x04000FEE RID: 4078
			public NodeType nodeType;

			// Token: 0x04000FEF RID: 4079
			public List<GameObject> nodes;
		}
	}
}
