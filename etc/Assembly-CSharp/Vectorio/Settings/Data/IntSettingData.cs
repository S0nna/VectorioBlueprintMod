using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Settings.Data
{
	// Token: 0x02000271 RID: 625
	[CreateAssetMenu(fileName = "New Int Setting", menuName = "Vectorio/Settings/Int")]
	public class IntSettingData : BaseSettingData
	{
		// Token: 0x0600120A RID: 4618 RVA: 0x00052671 File Offset: 0x00050871
		public int GetDefault()
		{
			return this.options[this.defaultIndex].optionValue;
		}

		// Token: 0x04000F78 RID: 3960
		public List<IntSettingData.Option> options;

		// Token: 0x04000F79 RID: 3961
		public int defaultIndex;

		// Token: 0x02000272 RID: 626
		[Serializable]
		public class Option
		{
			// Token: 0x04000F7A RID: 3962
			public string optionText;

			// Token: 0x04000F7B RID: 3963
			public int optionValue;
		}
	}
}
