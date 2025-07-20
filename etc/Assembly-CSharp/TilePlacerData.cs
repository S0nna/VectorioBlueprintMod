using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000D9 RID: 217
public class TilePlacerData : ComponentData<TilePlacer>
{
	// Token: 0x06000700 RID: 1792 RVA: 0x000208FB File Offset: 0x0001EAFB
	public override void ApplyData(TilePlacer component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x040004AA RID: 1194
	public TileDesignData tileData;

	// Token: 0x040004AB RID: 1195
	public bool useClaimColor;

	// Token: 0x040004AC RID: 1196
	public Color defaultTileColor;

	// Token: 0x040004AD RID: 1197
	public Color defaultMapColor;
}
