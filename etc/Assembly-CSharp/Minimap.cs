using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001D4 RID: 468
public class Minimap : MonoBehaviour
{
	// Token: 0x06000E99 RID: 3737 RVA: 0x00041F54 File Offset: 0x00040154
	public void Start()
	{
		this.camera = base.GetComponent<Camera>();
		Singleton<Events>.Instance.onControllerReady.AddListener(new UnityAction<InputController>(this.SetControllerTarget));
		InputManager.OnMapActionPressed.AddListener(new UnityAction(this.ToggleMinimap));
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x00041F93 File Offset: 0x00040193
	public void ToggleMinimap()
	{
		this.minimapObj.SetActive(!this.minimapObj.activeSelf);
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x00041FB0 File Offset: 0x000401B0
	public void Update()
	{
		if (!this.fixedUpdate && this.target != null)
		{
			base.transform.position = new Vector3(this.target.position.x, this.target.position.y, 1f);
		}
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00042008 File Offset: 0x00040208
	public void FixedUpdate()
	{
		if (this.fixedUpdate && this.target != null)
		{
			base.transform.position = new Vector3(this.target.position.x, this.target.position.y, 1f);
		}
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00042060 File Offset: 0x00040260
	protected void SetControllerTarget(InputController controller)
	{
		this.SetTarget(controller.transform, true);
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x0004206F File Offset: 0x0004026F
	public void SetTarget(Transform target)
	{
		this.SetTarget(target, true);
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x00042079 File Offset: 0x00040279
	public void SetTarget(Transform target, bool fixedUpdate)
	{
		this.target = target;
		this.fixedUpdate = fixedUpdate;
	}

	// Token: 0x04000B9F RID: 2975
	public GameObject minimapObj;

	// Token: 0x04000BA0 RID: 2976
	protected Camera camera;

	// Token: 0x04000BA1 RID: 2977
	protected Transform target;

	// Token: 0x04000BA2 RID: 2978
	protected bool fixedUpdate;

	// Token: 0x04000BA3 RID: 2979
	protected Vector2 lastEdgeTaken;
}
