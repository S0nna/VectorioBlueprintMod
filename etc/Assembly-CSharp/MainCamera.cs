using System;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class MainCamera : Singleton<MainCamera>
{
	// Token: 0x06000E7E RID: 3710 RVA: 0x0004124A File Offset: 0x0003F44A
	public override void Awake()
	{
		this._camera = base.GetComponent<Camera>();
		base.Awake();
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00041260 File Offset: 0x0003F460
	public bool IsOnScreen(Transform obj)
	{
		Vector2 vector = this._camera.WorldToScreenPoint(obj.position);
		return vector.x > 0f && vector.x < (float)Screen.width && vector.y > 0f && vector.y < (float)Screen.height;
	}

	// Token: 0x04000B5D RID: 2909
	private Camera _camera;
}
