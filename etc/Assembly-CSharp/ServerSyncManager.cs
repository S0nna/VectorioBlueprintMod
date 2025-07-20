using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class ServerSyncManager : ServerSingleton<ServerSyncManager>
{
	// Token: 0x06000C68 RID: 3176 RVA: 0x000356D5 File Offset: 0x000338D5
	public uint PeekRuntimeIDTracker()
	{
		return this._runtimeIDTracker;
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x000356E0 File Offset: 0x000338E0
	public uint RequestNewRuntimeID()
	{
		if (this._reusableIDs.Count > 0)
		{
			return this._reusableIDs.Dequeue();
		}
		uint runtimeIDTracker = this._runtimeIDTracker;
		this._runtimeIDTracker = runtimeIDTracker + 1U;
		return runtimeIDTracker;
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00035718 File Offset: 0x00033918
	public uint MapID(uint key)
	{
		if (!this._remappedIDs.ContainsKey(key))
		{
			uint num = this.RequestNewRuntimeID();
			this._remappedIDs.Add(key, num);
			return num;
		}
		return this._remappedIDs[key];
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00035755 File Offset: 0x00033955
	public void ClearIDMap()
	{
		this._remappedIDs.Clear();
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00035762 File Offset: 0x00033962
	public void ReleaseRuntimeID(uint id)
	{
		this._reusableIDs.Enqueue(id);
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00035770 File Offset: 0x00033970
	public void SetRuntimeID(uint id)
	{
		this._runtimeIDTracker = id;
		Debug.Log("[SERVER] Runtime ID tracker has been preloaded at " + id.ToString());
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x0003578F File Offset: 0x0003398F
	public override void Setup()
	{
		this._clientSyncManager = base.gameObject.GetComponent<ClientSyncManager>();
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x000357A4 File Offset: 0x000339A4
	public override void OnServerTick()
	{
		if (!base.IsHost)
		{
			Debug.Log("[SERVER] Access to server tick denied!");
			return;
		}
		int count = this._clientSyncManager.Players.Count;
		if (count > 1)
		{
			if (count != this._lastStateSize)
			{
				this._cachedStateInfo = new ClientSyncManager.ClientStateInformation
				{
					ids = new int[count],
					xCoords = new float[count],
					yCoords = new float[count]
				};
				this._lastStateSize = count;
			}
			int num = 0;
			foreach (KeyValuePair<int, ClientSyncManager.PlayerInfo> keyValuePair in this._clientSyncManager.Players)
			{
				if (this._clientSyncManager.Players.ContainsKey(keyValuePair.Key))
				{
					Vector2 actualPosition = keyValuePair.Value.ActualPosition;
					this._cachedStateInfo.ids[num] = keyValuePair.Key;
					this._cachedStateInfo.xCoords[num] = actualPosition.x;
					this._cachedStateInfo.yCoords[num] = actualPosition.y;
					num++;
				}
			}
			this._clientSyncManager.Rpc_SyncClients(this._cachedStateInfo);
		}
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00003212 File Offset: 0x00001412
	public override void OnServerUpdate(float time)
	{
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x00035910 File Offset: 0x00033B10
	public override void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyServerSyncManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyServerSyncManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize___Early();
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x00035929 File Offset: 0x00033B29
	public override void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LateServerSyncManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LateServerSyncManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize__Late();
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x00035942 File Offset: 0x00033B42
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x00035950 File Offset: 0x00033B50
	public override void Awake()
	{
		this.NetworkInitialize___Early();
		base.Awake();
		this.NetworkInitialize__Late();
	}

	// Token: 0x04000840 RID: 2112
	private ClientSyncManager _clientSyncManager;

	// Token: 0x04000841 RID: 2113
	private int _lastStateSize = -1;

	// Token: 0x04000842 RID: 2114
	private ClientSyncManager.ClientStateInformation _cachedStateInfo;

	// Token: 0x04000843 RID: 2115
	private uint _runtimeIDTracker = 1U;

	// Token: 0x04000844 RID: 2116
	private readonly Queue<uint> _reusableIDs = new Queue<uint>();

	// Token: 0x04000845 RID: 2117
	private Dictionary<uint, uint> _remappedIDs = new Dictionary<uint, uint>();

	// Token: 0x04000846 RID: 2118
	private bool dll_Excuted;

	// Token: 0x04000847 RID: 2119
	private bool dll_Excuted;
}
