using System;

namespace Vectorio.Entities
{
	// Token: 0x0200029B RID: 667
	[Serializable]
	public class StringVariableData : VariableData
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00056D19 File Offset: 0x00054F19
		// (set) Token: 0x060012DC RID: 4828 RVA: 0x00056D21 File Offset: 0x00054F21
		public override string DataType { get; set; } = "string";

		// Token: 0x060012DD RID: 4829 RVA: 0x00056D2A File Offset: 0x00054F2A
		public override object GetValue()
		{
			return this.value;
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00056D32 File Offset: 0x00054F32
		public override void SetValue(object value)
		{
			this.value = (string)value;
		}

		// Token: 0x04001074 RID: 4212
		public string value;
	}
}
