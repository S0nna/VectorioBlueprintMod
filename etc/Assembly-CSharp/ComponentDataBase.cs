using System;
using UnityEngine;

// Token: 0x020000AA RID: 170
public abstract class ComponentDataBase : ScriptableObject
{
	// Token: 0x06000671 RID: 1649
	public abstract EntityComponent CreateComponent(Entity entity);
}
