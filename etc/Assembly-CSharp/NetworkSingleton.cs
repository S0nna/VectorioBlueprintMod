using System;
using FishNet.Object;
using UnityEngine;

// Token: 0x0200016A RID: 362
[DefaultExecutionOrder(-1)]
public abstract class NetworkSingleton<T> : NetworkBehaviour where T : Component
{
	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00033F7C File Offset: 0x0003217C
	public static T Instance
	{
		get
		{
			return NetworkSingleton<T>.instance;
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00033F84 File Offset: 0x00032184
	public virtual void Awake()
	{
		this.NetworkInitialize___Early();
		this.Awake_UserLogic_NetworkSingleton`1_Assembly-CSharp.dll();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00033FA3 File Offset: 0x000321A3
	private void OnDestroy()
	{
		NetworkSingleton<T>.instance = default(T);
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x00033FB8 File Offset: 0x000321B8
	public virtual void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyNetworkSingleton`1Assembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyNetworkSingleton`1Assembly-CSharp.dll_Excuted = true;
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x00033FCB File Offset: 0x000321CB
	public virtual void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LateNetworkSingleton`1Assembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LateNetworkSingleton`1Assembly-CSharp.dll_Excuted = true;
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x00033FDE File Offset: 0x000321DE
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00033FEC File Offset: 0x000321EC
	public virtual void Awake_UserLogic_NetworkSingleton()
	{
		if (NetworkSingleton<T>.instance)
		{
			Debug.LogError("There is more than one " + typeof(T).Name + " singleton in the scene!");
			return;
		}
		NetworkSingleton<T>.instance = (this as T);
	}

	// Token: 0x04000810 RID: 2064
	private static T instance;

	// Token: 0x04000811 RID: 2065
	private bool NetworkInitialize___EarlyNetworkSingleton;

	// Token: 0x04000812 RID: 2066
	private bool NetworkInitialize__LateNetworkSingleton;
}
