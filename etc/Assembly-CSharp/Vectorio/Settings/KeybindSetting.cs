using System;

namespace Vectorio.Settings
{
	// Token: 0x0200026C RID: 620
	[Serializable]
	public class KeybindSetting : BaseSetting
	{
		// Token: 0x06001205 RID: 4613 RVA: 0x00052643 File Offset: 0x00050843
		public KeybindSetting()
		{
			base.Type = "Keybind";
		}

		// Token: 0x04000F6D RID: 3949
		public string Value;
	}
}
