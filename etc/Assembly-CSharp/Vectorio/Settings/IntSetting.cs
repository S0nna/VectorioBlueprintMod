using System;

namespace Vectorio.Settings
{
	// Token: 0x0200026B RID: 619
	[Serializable]
	public class IntSetting : BaseSetting
	{
		// Token: 0x06001204 RID: 4612 RVA: 0x00052630 File Offset: 0x00050830
		public IntSetting()
		{
			base.Type = "Int";
		}

		// Token: 0x04000F6C RID: 3948
		public int Value;
	}
}
