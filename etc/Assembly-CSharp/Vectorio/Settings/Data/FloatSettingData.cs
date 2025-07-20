using System;
using UnityEngine;

namespace Vectorio.Settings.Data
{
	// Token: 0x02000270 RID: 624
	[CreateAssetMenu(fileName = "New Float Setting", menuName = "Vectorio/Settings/Float")]
	public class FloatSettingData : BaseSettingData
	{
		// Token: 0x04000F75 RID: 3957
		public float minValue;

		// Token: 0x04000F76 RID: 3958
		public float maxValue;

		// Token: 0x04000F77 RID: 3959
		public float defaultValue;
	}
}
