using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
[Serializable]
public abstract class ResourceComponentData<T> : ComponentData<T> where T : EntityComponent
{
	// Token: 0x0400047E RID: 1150
	public StorageMode mode;

	// Token: 0x0400047F RID: 1151
	public int storage;

	// Token: 0x04000480 RID: 1152
	public bool useIndicator;

	// Token: 0x04000481 RID: 1153
	public Vector2 indicatorPosition;
}
