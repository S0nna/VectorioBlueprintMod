using System;

namespace Vectorio.Entities
{
	// Token: 0x0200029A RID: 666
	[Serializable]
	public class IntVariableData : VariableData
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x00056CDA File Offset: 0x00054EDA
		// (set) Token: 0x060012D7 RID: 4823 RVA: 0x00056CE2 File Offset: 0x00054EE2
		public override string DataType { get; set; } = "int";

		// Token: 0x060012D8 RID: 4824 RVA: 0x00056CEB File Offset: 0x00054EEB
		public override object GetValue()
		{
			return this.value;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00056CF8 File Offset: 0x00054EF8
		public override void SetValue(object value)
		{
			this.value = (int)value;
		}

		// Token: 0x04001072 RID: 4210
		public int value;
	}
}
