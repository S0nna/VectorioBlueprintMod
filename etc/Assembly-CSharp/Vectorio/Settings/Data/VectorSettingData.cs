using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Settings.Data
{
	// Token: 0x02000273 RID: 627
	[CreateAssetMenu(fileName = "New Vector Setting", menuName = "Vectorio/Settings/Vector")]
	public class VectorSettingData : BaseSettingData
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x00052689 File Offset: 0x00050889
		public int GetDefaultX
		{
			get
			{
				return this.options[this.defaultIndex].optionValueX;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x000526A1 File Offset: 0x000508A1
		public int GetDefaultY
		{
			get
			{
				return this.options[this.defaultIndex].optionValueY;
			}
		}

		// Token: 0x04000F7C RID: 3964
		public List<VectorSettingData.Option> options;

		// Token: 0x04000F7D RID: 3965
		public int defaultIndex;

		// Token: 0x02000274 RID: 628
		[Serializable]
		public class Option
		{
			// Token: 0x04000F7E RID: 3966
			public string optionText;

			// Token: 0x04000F7F RID: 3967
			public int optionValueX;

			// Token: 0x04000F80 RID: 3968
			public int optionValueY;
		}
	}
}
