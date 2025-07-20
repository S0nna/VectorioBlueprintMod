using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
public abstract class EntitySettings : MonoBehaviour
{
	// Token: 0x06000791 RID: 1937
	public abstract void Set(EntityComponent component);

	// Token: 0x06000792 RID: 1938
	public abstract void CustomUpdate();

	// Token: 0x06000793 RID: 1939
	public abstract void Clear();
}
