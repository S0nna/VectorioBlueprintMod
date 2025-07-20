using System;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x0200012F RID: 303
public class ResourcePlacer : EntityComponent, IComponent<ResourcePlacer, ResourcePlacerData>
{
	// Token: 0x060009FA RID: 2554 RVA: 0x00029B8D File Offset: 0x00027D8D
	public ResourcePlacerData GetData()
	{
		return this._data;
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00029B95 File Offset: 0x00027D95
	public void OnInitialize(ResourcePlacerData data)
	{
		this._data = data;
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00029BA0 File Offset: 0x00027DA0
	public override void OnSpawn(bool fromSave)
	{
		Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(base.transform.position);
		Singleton<TileGrid>.Instance.SetResourceTile(new Vector3Int(vector2Int.x, vector2Int.y, 0), this._data.resource);
		base.Entity.DestroyEntity(true);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00003212 File Offset: 0x00001412
	public override void OnReset()
	{
	}

	// Token: 0x0400061F RID: 1567
	private ResourcePlacerData _data;
}
