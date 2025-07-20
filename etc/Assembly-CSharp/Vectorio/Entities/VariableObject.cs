using System;
using System.Collections.Generic;

namespace Vectorio.Entities
{
	// Token: 0x0200029F RID: 671
	[Serializable]
	public class VariableObject
	{
		// Token: 0x0400107E RID: 4222
		public string Type;

		// Token: 0x0400107F RID: 4223
		public List<VariableObject.StringVariable> stringVariables;

		// Token: 0x04001080 RID: 4224
		public List<VariableObject.IntVariable> intVariables;

		// Token: 0x04001081 RID: 4225
		public List<VariableObject.FloatVariable> floatVariables;

		// Token: 0x04001082 RID: 4226
		public List<VariableObject.BoolVariable> boolVariables;

		// Token: 0x020002A0 RID: 672
		[Serializable]
		public struct StringVariable
		{
			// Token: 0x04001083 RID: 4227
			public string value;

			// Token: 0x04001084 RID: 4228
			public byte key;
		}

		// Token: 0x020002A1 RID: 673
		[Serializable]
		public struct IntVariable
		{
			// Token: 0x04001085 RID: 4229
			public int value;

			// Token: 0x04001086 RID: 4230
			public byte key;
		}

		// Token: 0x020002A2 RID: 674
		[Serializable]
		public struct FloatVariable
		{
			// Token: 0x04001087 RID: 4231
			public float value;

			// Token: 0x04001088 RID: 4232
			public byte key;
		}

		// Token: 0x020002A3 RID: 675
		[Serializable]
		public struct BoolVariable
		{
			// Token: 0x04001089 RID: 4233
			public bool value;

			// Token: 0x0400108A RID: 4234
			public byte key;
		}
	}
}
