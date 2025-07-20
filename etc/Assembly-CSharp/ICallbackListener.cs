using System;

// Token: 0x02000094 RID: 148
public interface ICallbackListener
{
	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000604 RID: 1540
	// (set) Token: 0x06000605 RID: 1541
	bool IsUpdating { get; set; }

	// Token: 0x06000606 RID: 1542
	void OnStartCallback(EntityCallbackEvent callback);

	// Token: 0x06000607 RID: 1543
	void OnEndCallback(EntityCallbackEvent callback);
}
