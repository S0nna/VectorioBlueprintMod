using System;

namespace Vectorio.Settings
{
	// Token: 0x0200026D RID: 621
	[Serializable]
	public class VectorSetting : BaseSetting
	{
		// Token: 0x06001206 RID: 4614 RVA: 0x00052656 File Offset: 0x00050856
		public VectorSetting()
		{
			base.Type = "Vector";
		}

		// Token: 0x04000F6E RID: 3950
		public int ValueX;

		// Token: 0x04000F6F RID: 3951
		public int ValueY;
	}
}
