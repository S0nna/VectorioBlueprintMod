using System;

// Token: 0x0200016F RID: 367
[Serializable]
public class EntityDestroyEvent : NetworkEventBase
{
	// Token: 0x04000823 RID: 2083
	public uint RuntimeID;

	// Token: 0x04000824 RID: 2084
	public uint DamagerID;

	// Token: 0x04000825 RID: 2085
	public bool Recycle;
}
