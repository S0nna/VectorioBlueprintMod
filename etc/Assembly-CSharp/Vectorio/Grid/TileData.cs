using System;

namespace Vectorio.Grid
{
	// Token: 0x0200028B RID: 651
	[Serializable]
	public class TileData
	{
		// Token: 0x0600125A RID: 4698 RVA: 0x00054442 File Offset: 0x00052642
		public TileData(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00054458 File Offset: 0x00052658
		public TileData()
		{
			this.X = 0;
			this.Y = 0;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x0005446E File Offset: 0x0005266E
		// (set) Token: 0x0600125D RID: 4701 RVA: 0x00054476 File Offset: 0x00052676
		public int X { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x0005447F File Offset: 0x0005267F
		// (set) Token: 0x0600125F RID: 4703 RVA: 0x00054487 File Offset: 0x00052687
		public int Y { get; set; }
	}
}
