using System;

namespace Vectorio.Entities
{
	// Token: 0x02000299 RID: 665
	[Serializable]
	public class FloatVariableData : VariableData
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x00056C9B File Offset: 0x00054E9B
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x00056CA3 File Offset: 0x00054EA3
		public override string DataType { get; set; } = "float";

		// Token: 0x060012D3 RID: 4819 RVA: 0x00056CAC File Offset: 0x00054EAC
		public override object GetValue()
		{
			return this.value;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00056CB9 File Offset: 0x00054EB9
		public override void SetValue(object value)
		{
			this.value = (float)value;
		}

		// Token: 0x04001070 RID: 4208
		public float value;
	}
}
