using System;

// Token: 0x02000099 RID: 153
public interface IUpdateable
{
	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000610 RID: 1552
	Entity Entity { get; }

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000611 RID: 1553
	// (set) Token: 0x06000612 RID: 1554
	bool IsUpdating { get; set; }

	// Token: 0x06000613 RID: 1555
	void Tick(float time);

	// Token: 0x06000614 RID: 1556
	void OnStartUpdating();

	// Token: 0x06000615 RID: 1557
	void OnStopUpdating();
}
