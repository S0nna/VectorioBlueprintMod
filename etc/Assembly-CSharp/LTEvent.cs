using System;

// Token: 0x02000042 RID: 66
public class LTEvent
{
	// Token: 0x06000238 RID: 568 RVA: 0x0000F6BC File Offset: 0x0000D8BC
	public LTEvent(int id, object data)
	{
		this.id = id;
		this.data = data;
	}

	// Token: 0x040001B5 RID: 437
	public int id;

	// Token: 0x040001B6 RID: 438
	public object data;
}
