using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
[Serializable]
public class TileDesign
{
	// Token: 0x060005FD RID: 1533 RVA: 0x0001F417 File Offset: 0x0001D617
	public TileDesign(TileDesignData data, Color color, Color map)
	{
		this.data = data;
		this.color = color;
		this.map = map;
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0001F434 File Offset: 0x0001D634
	public TileDesign(TileDesignData data, Color color)
	{
		this.data = data;
		this.color = color;
		this.map = color;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0001F451 File Offset: 0x0001D651
	public TileDesign()
	{
		this.data = null;
		this.color = Color.white;
		this.map = Color.white;
	}

	// Token: 0x0400036A RID: 874
	public TileDesignData data;

	// Token: 0x0400036B RID: 875
	public Color color;

	// Token: 0x0400036C RID: 876
	public Color map;
}
