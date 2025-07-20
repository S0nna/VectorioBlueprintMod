using System;

namespace Vectorio.Grid
{
	// Token: 0x0200028A RID: 650
	[Serializable]
	public class DecorationData : TileData
	{
		// Token: 0x06001254 RID: 4692 RVA: 0x000543D7 File Offset: 0x000525D7
		public DecorationData(int x, int y, int tileColor, int mapColor)
		{
			base.X = x;
			base.Y = y;
			this.TileColor = tileColor;
			this.MapColor = mapColor;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x000543FC File Offset: 0x000525FC
		public DecorationData()
		{
			base.X = 0;
			base.Y = 0;
			this.TileColor = 0;
			this.MapColor = 0;
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001256 RID: 4694 RVA: 0x00054420 File Offset: 0x00052620
		// (set) Token: 0x06001257 RID: 4695 RVA: 0x00054428 File Offset: 0x00052628
		public int TileColor { get; set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x00054431 File Offset: 0x00052631
		// (set) Token: 0x06001259 RID: 4697 RVA: 0x00054439 File Offset: 0x00052639
		public int MapColor { get; set; }
	}
}
