using System;

namespace Vectorio.Entities
{
	// Token: 0x02000298 RID: 664
	[Serializable]
	public class BoolVariableData : VariableData
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x00056C5C File Offset: 0x00054E5C
		// (set) Token: 0x060012CD RID: 4813 RVA: 0x00056C64 File Offset: 0x00054E64
		public override string DataType { get; set; } = "bool";

		// Token: 0x060012CE RID: 4814 RVA: 0x00056C6D File Offset: 0x00054E6D
		public override object GetValue()
		{
			return this.value;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00056C7A File Offset: 0x00054E7A
		public override void SetValue(object value)
		{
			this.value = (bool)value;
		}

		// Token: 0x0400106E RID: 4206
		public bool value;
	}
}
