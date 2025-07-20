using System;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x020002B1 RID: 689
	[AttributeUsage(AttributeTargets.Class)]
	public class ModifierID : Attribute
	{
		// Token: 0x0600133B RID: 4923 RVA: 0x000584E8 File Offset: 0x000566E8
		public ModifierID(string name)
		{
			this.name = name;
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x000584F7 File Offset: 0x000566F7
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x040010DD RID: 4317
		private string name;
	}
}
