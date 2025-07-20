using System;
using UnityEngine;

// Token: 0x02000176 RID: 374
[DefaultExecutionOrder(-1)]
public abstract class ServerSingleton<T> : ServerManager where T : Component
{
	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000C2B RID: 3115 RVA: 0x000341A8 File Offset: 0x000323A8
	public static T Instance
	{
		get
		{
			return ServerSingleton<T>.instance;
		}
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x000341B0 File Offset: 0x000323B0
	public override void Awake()
	{
		this.NetworkInitialize___Early();
		this.Awake_UserLogic_ServerSingleton`1_Assembly-CSharp.dll();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x000341CF File Offset: 0x000323CF
	private void OnDestroy()
	{
		ServerSingleton<T>.instance = default(T);
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x000341E4 File Offset: 0x000323E4
	public override void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyServerSingleton`1Assembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyServerSingleton`1Assembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize___Early();
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x000341FD File Offset: 0x000323FD
	public override void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LateServerSingleton`1Assembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LateServerSingleton`1Assembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize__Late();
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00034216 File Offset: 0x00032416
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00034224 File Offset: 0x00032424
	public virtual void Awake_UserLogic_ServerSingleton()
	{
		if (ServerSingleton<T>.instance)
		{
			Debug.LogError("There is more than one " + typeof(T).Name + " singleton in the scene!");
			return;
		}
		ServerSingleton<T>.instance = (this as T);
	}

	// Token: 0x04000832 RID: 2098
	private static T instance;

	// Token: 0x04000833 RID: 2099
	private bool NetworkInitialize___EarlyServerSingleton;

	// Token: 0x04000834 RID: 2100
	private bool NetworkInitialize__LateServerSingleton;
}
