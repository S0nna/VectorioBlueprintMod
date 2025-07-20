using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000165 RID: 357
[DefaultExecutionOrder(0)]
public class GameServer : Singleton<GameServer>
{
	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00033B5B File Offset: 0x00031D5B
	public static float TickInterval
	{
		get
		{
			return 1f / GameServer._tickRate;
		}
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00033B68 File Offset: 0x00031D68
	public void Setup()
	{
		if (!this._isSetup)
		{
			this._tickInterval = GameServer.TickInterval;
			this._serverManagers.Add(this.EnsureComponent<ServerSyncManager>());
			this._serverManagers.Add(this.EnsureComponent<ServerStateManager>());
			this._isSetup = true;
		}
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00033BA8 File Offset: 0x00031DA8
	public void Update()
	{
		if (Singleton<Gamemode>.Instance.IsPaused)
		{
			return;
		}
		float time = Time.time;
		float deltaTime = Time.deltaTime;
		bool flag = time >= this._nextTick;
		if (flag)
		{
			this._nextTick = time + this._tickInterval;
		}
		foreach (ServerManager serverManager in this._serverManagers)
		{
			serverManager.OnServerUpdate(deltaTime);
			if (flag)
			{
				serverManager.OnServerTick();
			}
		}
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00033C40 File Offset: 0x00031E40
	protected C EnsureComponent<C>() where C : ServerManager
	{
		C c = base.GetComponent<C>();
		if (c == null)
		{
			c = base.gameObject.AddComponent<C>();
		}
		c.Setup();
		return c;
	}

	// Token: 0x040007FF RID: 2047
	private bool _isSetup;

	// Token: 0x04000800 RID: 2048
	private static float _tickRate = 60f;

	// Token: 0x04000801 RID: 2049
	private float _nextTick;

	// Token: 0x04000802 RID: 2050
	private float _tickInterval;

	// Token: 0x04000803 RID: 2051
	private List<ServerManager> _serverManagers = new List<ServerManager>();
}
