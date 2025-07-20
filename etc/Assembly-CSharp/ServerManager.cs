using System;
using FishNet.Object;
using UnityEngine;

// Token: 0x02000175 RID: 373
[DefaultExecutionOrder(-1)]
public abstract class ServerManager : NetworkBehaviour
{
	// Token: 0x06000C23 RID: 3107
	public abstract void Setup();

	// Token: 0x06000C24 RID: 3108
	public abstract void OnServerTick();

	// Token: 0x06000C25 RID: 3109
	public abstract void OnServerUpdate(float time);

	// Token: 0x06000C27 RID: 3111 RVA: 0x00034174 File Offset: 0x00032374
	public virtual void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyServerManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyServerManagerAssembly-CSharp.dll_Excuted = true;
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x00034187 File Offset: 0x00032387
	public virtual void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LateServerManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LateServerManagerAssembly-CSharp.dll_Excuted = true;
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0003419A File Offset: 0x0003239A
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0003419A File Offset: 0x0003239A
	public virtual void Awake()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x04000830 RID: 2096
	private bool dll_Excuted;

	// Token: 0x04000831 RID: 2097
	private bool dll_Excuted;
}
