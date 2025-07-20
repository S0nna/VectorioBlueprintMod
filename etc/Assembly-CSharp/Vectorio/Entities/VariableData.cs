using System;

namespace Vectorio.Entities
{
	// Token: 0x0200029D RID: 669
	[Serializable]
	public class VariableData
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x0005748C File Offset: 0x0005568C
		// (set) Token: 0x060012FD RID: 4861 RVA: 0x00057494 File Offset: 0x00055694
		public virtual string DataType { get; set; }

		// Token: 0x060012FE RID: 4862 RVA: 0x00025334 File Offset: 0x00023534
		public virtual object GetValue()
		{
			return null;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00003212 File Offset: 0x00001412
		public virtual void SetValue(object value)
		{
		}

		// Token: 0x0400107C RID: 4220
		public byte key;
	}
}
