using System;
using UnityEngine;

// Token: 0x02000230 RID: 560
[DefaultExecutionOrder(-1)]
public class Singleton<T> : MonoBehaviour where T : Component
{
	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x0600105F RID: 4191 RVA: 0x0004CDB2 File Offset: 0x0004AFB2
	public static T Instance
	{
		get
		{
			return Singleton<T>.instance;
		}
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0004CDBC File Offset: 0x0004AFBC
	public virtual void Awake()
	{
		if (Singleton<T>.instance)
		{
			Debug.LogError("There is more than one " + typeof(T).Name + " singleton in the scene!");
			return;
		}
		Singleton<T>.instance = (this as T);
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x0004CE0E File Offset: 0x0004B00E
	private void OnDestroy()
	{
		Singleton<T>.instance = default(T);
	}

	// Token: 0x04000E56 RID: 3670
	private static T instance;
}
