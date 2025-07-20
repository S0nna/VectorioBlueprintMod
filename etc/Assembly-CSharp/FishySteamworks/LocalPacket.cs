using System;
using FishNet.Utility.Performance;

namespace FishySteamworks
{
	// Token: 0x020002B7 RID: 695
	internal struct LocalPacket
	{
		// Token: 0x06001378 RID: 4984 RVA: 0x00058F34 File Offset: 0x00057134
		public LocalPacket(ArraySegment<byte> data, byte channel)
		{
			this.Data = ByteArrayPool.Retrieve(data.Count);
			this.Length = data.Count;
			Buffer.BlockCopy(data.Array, data.Offset, this.Data, 0, this.Length);
			this.Channel = channel;
		}

		// Token: 0x040010F1 RID: 4337
		public byte[] Data;

		// Token: 0x040010F2 RID: 4338
		public int Length;

		// Token: 0x040010F3 RID: 4339
		public byte Channel;
	}
}
