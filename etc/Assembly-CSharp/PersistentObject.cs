using System;
using UnityEngine;

// Token: 0x020001DC RID: 476
[DefaultExecutionOrder(0)]
public class PersistentObject : MonoBehaviour
{
	// Token: 0x06000ED3 RID: 3795 RVA: 0x00044501 File Offset: 0x00042701
	public void Awake()
	{
		base.gameObject.name = base.gameObject.name + " [PERSISTENT]";
		Object.DontDestroyOnLoad(base.gameObject);
		Object.Destroy(this);
	}
}
