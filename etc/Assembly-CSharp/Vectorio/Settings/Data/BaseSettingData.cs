using System;

namespace Vectorio.Settings.Data
{
	// Token: 0x0200026E RID: 622
	public abstract class BaseSettingData : BaseData
	{
		// Token: 0x04000F70 RID: 3952
		public SettingCategory category;

		// Token: 0x04000F71 RID: 3953
		public int sortingOrder;

		// Token: 0x04000F72 RID: 3954
		public bool useHeader;

		// Token: 0x04000F73 RID: 3955
		public string headerText;
	}
}
