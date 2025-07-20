using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017D RID: 381
public class CameraFollow : MonoBehaviour
{
	// Token: 0x06000C88 RID: 3208 RVA: 0x000363F0 File Offset: 0x000345F0
	public void Start()
	{
		CameraFollow.zPosition = 0;
		this.camera = base.GetComponent<Camera>();
		Singleton<Events>.Instance.onControllerReady.AddListener(new UnityAction<InputController>(this.SetControllerTarget));
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00036420 File Offset: 0x00034620
	public void Update()
	{
		if (this.target != null)
		{
			base.transform.position = new Vector3(this.target.position.x, this.target.position.y, (float)CameraFollow.zPosition);
		}
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x00036471 File Offset: 0x00034671
	protected void SetControllerTarget(InputController controller)
	{
		this.target = controller.transform;
	}

	// Token: 0x0400086B RID: 2155
	protected Camera camera;

	// Token: 0x0400086C RID: 2156
	protected Transform target;

	// Token: 0x0400086D RID: 2157
	public static int zPosition;
}
