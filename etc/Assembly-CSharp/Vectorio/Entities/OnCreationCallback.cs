using System;

namespace Vectorio.Entities
{
	// Token: 0x020002AA RID: 682
	[Serializable]
	public struct OnCreationCallback
	{
		// Token: 0x06001306 RID: 4870 RVA: 0x0005753F File Offset: 0x0005573F
		public OnCreationCallback(uint id, CallbackType type, byte index)
		{
			this.ID = id;
			this.Type = type;
			this.Index = index;
		}

		// Token: 0x040010A5 RID: 4261
		public uint ID;

		// Token: 0x040010A6 RID: 4262
		public CallbackType Type;

		// Token: 0x040010A7 RID: 4263
		public byte Index;
	}
}
