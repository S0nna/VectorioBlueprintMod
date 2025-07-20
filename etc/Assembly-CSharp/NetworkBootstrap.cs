using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
[DefaultExecutionOrder(0)]
public class NetworkBootstrap : Singleton<NetworkBootstrap>
{
	// Token: 0x06000BD1 RID: 3025 RVA: 0x00033C99 File Offset: 0x00031E99
	public void CreateGameServer()
	{
		if (base.GetComponent<GameServer>() == null)
		{
			this.EnsureComponent<GameServer>().Setup();
		}
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x00033CB4 File Offset: 0x00031EB4
	public void CreateClientReceivers()
	{
		this.EnsureComponent<ClientSyncManager>();
		this.EnsureComponent<ClientStateManager>();
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00033CC4 File Offset: 0x00031EC4
	protected C EnsureComponent<C>() where C : Component
	{
		C c = base.GetComponent<C>();
		if (c == null)
		{
			c = base.gameObject.AddComponent<C>();
		}
		return c;
	}
}
