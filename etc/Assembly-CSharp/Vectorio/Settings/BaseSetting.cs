using System;
using Newtonsoft.Json;

namespace Vectorio.Settings
{
	// Token: 0x02000268 RID: 616
	[JsonConverter(typeof(SavedSettingConverter))]
	[Serializable]
	public class BaseSetting
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x000525F9 File Offset: 0x000507F9
		// (set) Token: 0x06001200 RID: 4608 RVA: 0x00052601 File Offset: 0x00050801
		public string Type { get; set; }
	}
}
