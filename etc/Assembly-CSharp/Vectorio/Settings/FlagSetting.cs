using System;

namespace Vectorio.Settings
{
	// Token: 0x02000269 RID: 617
	[Serializable]
	public class FlagSetting : BaseSetting
	{
		// Token: 0x06001202 RID: 4610 RVA: 0x0005260A File Offset: 0x0005080A
		public FlagSetting()
		{
			base.Type = "Flag";
		}

		// Token: 0x04000F6A RID: 3946
		public bool Value;
	}
}
