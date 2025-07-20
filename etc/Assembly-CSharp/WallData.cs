using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000DE RID: 222
public class WallData : ComponentData<Wall>
{
	// Token: 0x0600070B RID: 1803 RVA: 0x00020B5F File Offset: 0x0001ED5F
	public override void ApplyData(Wall component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00020B68 File Offset: 0x0001ED68
	public WallPiece RequestWallPiece(string model_id)
	{
		foreach (WallData.SubModel subModel in this.wallPieces)
		{
			if (subModel.model_id == model_id)
			{
				return subModel.piece;
			}
		}
		if (this.wallPieces.Count > 0)
		{
			return this.wallPieces[0].piece;
		}
		return null;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x040004D1 RID: 1233
	public List<WallData.SubModel> wallPieces;

	// Token: 0x020000DF RID: 223
	[Serializable]
	public class SubModel
	{
		// Token: 0x040004D2 RID: 1234
		public string model_id;

		// Token: 0x040004D3 RID: 1235
		[SerializeField]
		public WallPiece piece;
	}
}
