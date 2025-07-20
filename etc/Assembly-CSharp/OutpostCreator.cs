using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Grid;
using Vectorio.Utilities;

// Token: 0x020001D9 RID: 473
public class OutpostCreator : Singleton<OutpostCreator>
{
	// Token: 0x06000EB3 RID: 3763 RVA: 0x00042920 File Offset: 0x00040B20
	public void CreateOutpost(OutpostData outpost, Vector2Int tileCoords)
	{
		ServerSingleton<ServerSyncManager>.Instance.ClearIDMap();
		Vector2 position = Utilities.ConvertCellPositionToWorld(tileCoords);
		this._factionID = Singleton<HeatManager>.Instance.FactionData.ID;
		foreach (KeyValuePair<string, List<Vector2Int>> keyValuePair in outpost.resources)
		{
			ResourceData resourceData = Library.RequestData<ResourceData>(keyValuePair.Key);
			if (resourceData == null)
			{
				Debug.Log("[OUTPOST CREATOR] Invalid resource ID " + keyValuePair.Key);
			}
			else
			{
				foreach (Vector2Int vector2Int in keyValuePair.Value)
				{
					Vector3Int coords = new Vector3Int(tileCoords.x + vector2Int.x, tileCoords.y + vector2Int.y, 0);
					Singleton<TileGrid>.Instance.SetResourceTile(coords, resourceData);
				}
			}
		}
		this.CreateEntities(position, outpost.entities);
		this.ApplyMetadata(outpost.entities);
		foreach (KeyValuePair<string, List<DecorationData>> keyValuePair2 in outpost.tiles)
		{
			TileDesignData tileDesignData = Library.RequestData<TileDesignData>(keyValuePair2.Key);
			if (!(tileDesignData == null))
			{
				foreach (DecorationData decorationData in keyValuePair2.Value)
				{
					TileDesign design = new TileDesign(tileDesignData, Utilities.IntToColor(decorationData.TileColor), Utilities.IntToColor(decorationData.MapColor));
					Singleton<TileGrid>.Instance.SetTileDesign(design, new Vector3Int(tileCoords.x + decorationData.X, tileCoords.y + decorationData.Y, 0), false);
				}
			}
		}
		ServerSingleton<ServerSyncManager>.Instance.ClearIDMap();
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x00042B48 File Offset: 0x00040D48
	private void CreateEntities(Vector2 position, IEnumerable<OutpostData.Entity> entities)
	{
		foreach (OutpostData.Entity entity in entities)
		{
			EntityCreationData entityCreationData;
			if (EventBuilder.BuildCreationDataWithMetadata(entity.metadata, out entityCreationData, this._factionID, position.x + entity.posX, position.y + entity.posY))
			{
				entityCreationData.FromSave = true;
				Singleton<EntityManager>.Instance.QueueCreationEvent(entityCreationData);
			}
		}
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00042BCC File Offset: 0x00040DCC
	private void ApplyMetadata(IEnumerable<OutpostData.Entity> entities)
	{
		foreach (OutpostData.Entity entity in entities)
		{
			EntityMetadataEvent data = EventBuilder.BuildMetadataEvent(entity.metadata.RuntimeID.GetValue(), entity.metadata, false);
			Singleton<EntityManager>.Instance.QueueMetadataEvent(data, SyncType.None);
		}
	}

	// Token: 0x04000BC0 RID: 3008
	private string _factionID;
}
