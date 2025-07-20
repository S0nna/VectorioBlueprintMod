using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public abstract class BaseComponent : MonoBehaviour
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060007AF RID: 1967 RVA: 0x0002261F File Offset: 0x0002081F
	// (set) Token: 0x060007B0 RID: 1968 RVA: 0x00022627 File Offset: 0x00020827
	public bool IsUpdating
	{
		get
		{
			return this._isUpdating;
		}
		set
		{
			this._isUpdating = value;
		}
	}

	// Token: 0x060007B1 RID: 1969
	public abstract void OnReset();

	// Token: 0x04000526 RID: 1318
	private bool _isUpdating;
}
