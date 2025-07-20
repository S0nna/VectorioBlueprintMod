using System;

// Token: 0x02000093 RID: 147
public interface IAnimateable
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000600 RID: 1536
	// (set) Token: 0x06000601 RID: 1537
	bool IsAnimating { get; set; }

	// Token: 0x06000602 RID: 1538
	bool Animate(float time);

	// Token: 0x06000603 RID: 1539
	void ResetAnimation();
}
