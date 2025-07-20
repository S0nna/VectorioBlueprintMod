using System;

namespace Vectorio.Settings
{
	// Token: 0x0200026A RID: 618
	[Serializable]
	public class FloatSetting : BaseSetting
	{
		// Token: 0x06001203 RID: 4611 RVA: 0x0005261D File Offset: 0x0005081D
		public FloatSetting()
		{
			base.Type = "Float";
		}

		// Token: 0x04000F6B RID: 3947
		public float Value;
	}
}
