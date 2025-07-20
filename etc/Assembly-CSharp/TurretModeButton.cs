using System;
using UnityEngine;
using Vectorio.PhasmaUI;

// Token: 0x020001FD RID: 509
public class TurretModeButton : Button
{
	// Token: 0x06000F6B RID: 3947 RVA: 0x00048D88 File Offset: 0x00046F88
	public void CheckMode(int mode)
	{
		this.activeObject.SetActive(this.mode == mode);
	}

	// Token: 0x04000D01 RID: 3329
	public int mode;

	// Token: 0x04000D02 RID: 3330
	public GameObject activeObject;
}
