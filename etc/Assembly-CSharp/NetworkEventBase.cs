using System;

// Token: 0x02000171 RID: 369
[Serializable]
public class NetworkEventBase
{
	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00034112 File Offset: 0x00032312
	// (set) Token: 0x06000C15 RID: 3093 RVA: 0x0003411A File Offset: 0x0003231A
	public DateTime Timestamp { get; private set; }

	// Token: 0x06000C16 RID: 3094 RVA: 0x00034123 File Offset: 0x00032323
	public void Stamp()
	{
		this.Timestamp = DateTime.UtcNow;
	}
}
