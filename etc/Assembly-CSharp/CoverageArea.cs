using System;
using UnityEngine;

// Token: 0x0200022D RID: 557
[Serializable]
public class CoverageArea
{
	// Token: 0x0600104B RID: 4171 RVA: 0x0004CAED File Offset: 0x0004ACED
	public CoverageArea(Vector2Int startCell, Vector2Int endCell)
	{
		this.startX = startCell.x;
		this.startY = startCell.y;
		this.endX = endCell.x;
		this.endY = endCell.y;
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0004CB29 File Offset: 0x0004AD29
	public CoverageArea(int startX, int startY, int endX, int endY)
	{
		this.startX = startX;
		this.startY = startY;
		this.endX = endX;
		this.endY = endY;
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x0004CB4E File Offset: 0x0004AD4E
	public CoverageArea()
	{
		this.startX = 0;
		this.startY = 0;
		this.endX = 0;
		this.endY = 0;
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0004CB74 File Offset: 0x0004AD74
	public override string ToString()
	{
		return string.Format("Start X: {0}, Start Y: {1}, End X: {2}, End Y: {3}", new object[]
		{
			this.startX,
			this.startY,
			this.endX,
			this.endY
		});
	}

	// Token: 0x04000E4C RID: 3660
	public int startX;

	// Token: 0x04000E4D RID: 3661
	public int startY;

	// Token: 0x04000E4E RID: 3662
	public int endX;

	// Token: 0x04000E4F RID: 3663
	public int endY;
}
