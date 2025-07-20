using System;

// Token: 0x0200016E RID: 366
[Serializable]
public class EntityDamageEvent : NetworkEventBase
{
	// Token: 0x04000820 RID: 2080
	public uint EntityID;

	// Token: 0x04000821 RID: 2081
	public uint DamagerID;

	// Token: 0x04000822 RID: 2082
	public float Damage;
}
