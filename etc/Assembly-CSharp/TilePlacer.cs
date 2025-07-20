using System;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x02000136 RID: 310
public class TilePlacer : EntityComponent, IComponent<TilePlacer, TilePlacerData>
{
	// Token: 0x06000A38 RID: 2616 RVA: 0x0002ABE9 File Offset: 0x00028DE9
	public TilePlacerData GetData()
	{
		return this._data;
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x0002ABF1 File Offset: 0x00028DF1
	public void OnInitialize(TilePlacerData data)
	{
		this._data = data;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0002ABFC File Offset: 0x00028DFC
	public override void OnSpawn(bool fromSave)
	{
		Color color;
		Color map;
		if (this._data.useClaimColor)
		{
			color = Singleton<TileGrid>.Instance.GetRandomClaimColor();
			map = color;
		}
		else
		{
			Accent color2 = Singleton<ColorPalette>.Instance.Color;
			color = ((color2 != null) ? color2.primaryColor : this._data.defaultTileColor);
			map = ((color2 != null) ? color2.primaryColor : this._data.defaultMapColor);
		}
		TileDesign design = new TileDesign(this._data.tileData, color, map);
		Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(base.transform.position);
		Singleton<TileGrid>.Instance.SetTileDesign(design, new Vector3Int(vector2Int.x, vector2Int.y, 0), false);
		base.Entity.DestroyEntity(true);
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00003212 File Offset: 0x00001412
	public override void OnReset()
	{
	}

	// Token: 0x04000647 RID: 1607
	private TilePlacerData _data;
}
