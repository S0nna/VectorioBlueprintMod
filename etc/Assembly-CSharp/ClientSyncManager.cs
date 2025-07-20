using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class ClientSyncManager : NetworkSingleton<ClientSyncManager>
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00032FAD File Offset: 0x000311AD
	public Dictionary<int, ClientSyncManager.PlayerInfo> Players
	{
		get
		{
			return this._players;
		}
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00032FB5 File Offset: 0x000311B5
	public void Start()
	{
		this._tickInterval = GameServer.TickInterval;
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00032FC4 File Offset: 0x000311C4
	public void Update()
	{
		if (this._players.Count < 2)
		{
			return;
		}
		this._lerpTime += Time.deltaTime;
		foreach (ClientSyncManager.PlayerInfo playerInfo in this._players.Values)
		{
			if (!playerInfo.Reference.IsOwner)
			{
				playerInfo.Reference.SetHologramPosition(Vector2.Lerp(playerInfo.LastPosition, playerInfo.NextPosition, this._lerpTime / this._tickInterval));
			}
		}
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0003306C File Offset: 0x0003126C
	public void RegisterPlayer(Player player)
	{
		if (this._players.ContainsKey(player.SyncID))
		{
			Debug.Log("[NETWORK] Could not register player with duplicate ID " + player.OwnerId.ToString() + "!");
			return;
		}
		this._players.Add(player.SyncID, new ClientSyncManager.PlayerInfo(player));
		NetworkPlayerManager.SET_LOCAL_FLAG(this._players.Count <= 1);
		Debug.Log("[NETWORK] New player with ID " + player.SyncID.ToString() + " added to sync list.");
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00033100 File Offset: 0x00031300
	public void UnregisterPlayer(Player player)
	{
		if (!this._players.ContainsKey(player.SyncID))
		{
			Debug.Log("[NETWORK] Could not unregister player with non-existant ID " + player.SyncID.ToString() + "!");
			return;
		}
		this._players.Remove(player.SyncID);
		NetworkPlayerManager.SET_LOCAL_FLAG(this._players.Count <= 1);
		Debug.Log("[NETWORK] Player with ID " + player.SyncID.ToString() + " removed from sync list.");
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0003318D File Offset: 0x0003138D
	[ServerRpc(RequireOwnership = false)]
	public void Srv_SendClientState(int id, Vector2 position)
	{
		this.RpcWriter___Server_Srv_SendClientState_215135682(id, position);
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x000331A0 File Offset: 0x000313A0
	[ObserversRpc]
	public void Rpc_SyncClients(ClientSyncManager.ClientStateInformation clientStateInformation)
	{
		this.RpcWriter___Observers_Rpc_SyncClients_41556503(clientStateInformation);
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x000331B7 File Offset: 0x000313B7
	[ServerRpc(RequireOwnership = false)]
	public void Srv_SyncPlayerLoading(bool toggle)
	{
		this.RpcWriter___Server_Srv_SyncPlayerLoading_1140765316(toggle);
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x000331C4 File Offset: 0x000313C4
	[ObserversRpc]
	public void Rpc_SyncPlayerLoading(bool toggle)
	{
		this.RpcWriter___Observers_Rpc_SyncPlayerLoading_1140765316(toggle);
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x000331DB File Offset: 0x000313DB
	[TargetRpc(ExcludeServer = true)]
	public void CreateClientReceivers(NetworkConnection connection)
	{
		this.RpcWriter___Target_CreateClientReceivers_328543758(connection);
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x000331FC File Offset: 0x000313FC
	public override void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyClientSyncManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyClientSyncManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize___Early();
		base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Srv_SendClientState_215135682));
		base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_SyncClients_41556503));
		base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_Srv_SyncPlayerLoading_1140765316));
		base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_SyncPlayerLoading_1140765316));
		base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_CreateClientReceivers_328543758));
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00033293 File Offset: 0x00031493
	public override void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LateClientSyncManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LateClientSyncManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize__Late();
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x000332AC File Offset: 0x000314AC
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x000332BC File Offset: 0x000314BC
	private void RpcWriter___Server_Srv_SendClientState_215135682(int id, Vector2 position)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.WriteInt32(id, AutoPackType.Packed);
		writer.WriteVector2(position);
		base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00033375 File Offset: 0x00031575
	public void RpcLogic___Srv_SendClientState_215135682(int id, Vector2 position)
	{
		if (this._players.ContainsKey(id))
		{
			this._players[id].ActualPosition = position;
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00033398 File Offset: 0x00031598
	private void RpcReader___Server_Srv_SendClientState_215135682(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		int id = PooledReader0.ReadInt32(AutoPackType.Packed);
		Vector2 position = PooledReader0.ReadVector2();
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_SendClientState_215135682(id, position);
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x000333E0 File Offset: 0x000315E0
	private void RpcWriter___Observers_Rpc_SyncClients_41556503(ClientSyncManager.ClientStateInformation clientStateInformation)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___ClientSyncManager/ClientStateInformationFishNet.Serializing.Generated(clientStateInformation);
		base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
		writer.Store();
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00033498 File Offset: 0x00031698
	public void RpcLogic___Rpc_SyncClients_41556503(ClientSyncManager.ClientStateInformation clientStateInformation)
	{
		if (!NetworkPlayerManager.LOCAL_CLIENT_READY)
		{
			return;
		}
		this._lerpTime = 0f;
		for (int i = 0; i < clientStateInformation.ids.Length; i++)
		{
			int key = clientStateInformation.ids[i];
			if (this._players.ContainsKey(key))
			{
				ClientSyncManager.PlayerInfo playerInfo = this._players[clientStateInformation.ids[i]];
				if (!playerInfo.Reference.IsOwner)
				{
					playerInfo.NextPosition = new Vector2(clientStateInformation.xCoords[i], clientStateInformation.yCoords[i]);
				}
			}
			else
			{
				Debug.Log("[CLIENT SYNC] Invalid player with ID " + key.ToString());
			}
		}
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00033538 File Offset: 0x00031738
	private void RpcReader___Observers_Rpc_SyncClients_41556503(PooledReader PooledReader0, Channel channel)
	{
		ClientSyncManager.ClientStateInformation clientStateInformation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ClientSyncManager/ClientStateInformationFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_SyncClients_41556503(clientStateInformation);
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x0003356C File Offset: 0x0003176C
	private void RpcWriter___Server_Srv_SyncPlayerLoading_1140765316(bool toggle)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.WriteBoolean(toggle);
		base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00033613 File Offset: 0x00031813
	public void RpcLogic___Srv_SyncPlayerLoading_1140765316(bool toggle)
	{
		this._clientsJoining += (toggle ? 1 : -1);
		if (toggle)
		{
			this.Rpc_SyncPlayerLoading(true);
			return;
		}
		if (this._clientsJoining == 0)
		{
			this.Rpc_SyncPlayerLoading(false);
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00033644 File Offset: 0x00031844
	private void RpcReader___Server_Srv_SyncPlayerLoading_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		bool toggle = PooledReader0.ReadBoolean();
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_SyncPlayerLoading_1140765316(toggle);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00033678 File Offset: 0x00031878
	private void RpcWriter___Observers_Rpc_SyncPlayerLoading_1140765316(bool toggle)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.WriteBoolean(toggle);
		base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
		writer.Store();
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00033730 File Offset: 0x00031930
	public void RpcLogic___Rpc_SyncPlayerLoading_1140765316(bool toggle)
	{
		if (toggle)
		{
			if (!Singleton<NetworkPlayerManager>.Instance.IsPlayerLoading)
			{
				Singleton<NetworkPlayerManager>.Instance.IsPlayerLoading = true;
				Singleton<NetworkEvents>.Instance.onPlayerStartedLoading.Invoke();
				return;
			}
		}
		else
		{
			Singleton<NetworkPlayerManager>.Instance.IsPlayerLoading = false;
			Singleton<NetworkEvents>.Instance.onPlayerStoppedLoading.Invoke();
		}
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00033784 File Offset: 0x00031984
	private void RpcReader___Observers_Rpc_SyncPlayerLoading_1140765316(PooledReader PooledReader0, Channel channel)
	{
		bool toggle = PooledReader0.ReadBoolean();
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_SyncPlayerLoading_1140765316(toggle);
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x000337B8 File Offset: 0x000319B8
	private void RpcWriter___Target_CreateClientReceivers_328543758(NetworkConnection connection)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, connection, true, true);
		writer.Store();
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x00033860 File Offset: 0x00031A60
	public void RpcLogic___CreateClientReceivers_328543758(NetworkConnection connection)
	{
		Singleton<NetworkBootstrap>.Instance.CreateClientReceivers();
		NetworkPlayerManager.IS_HOST = false;
		NetworkPlayerManager.SET_LOCAL_FLAG(false);
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00033878 File Offset: 0x00031A78
	private void RpcReader___Target_CreateClientReceivers_328543758(PooledReader PooledReader0, Channel channel)
	{
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___CreateClientReceivers_328543758(base.LocalConnection);
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x0003389E File Offset: 0x00031A9E
	public override void Awake()
	{
		this.NetworkInitialize___Early();
		base.Awake();
		this.NetworkInitialize__Late();
	}

	// Token: 0x040007ED RID: 2029
	private Dictionary<int, ClientSyncManager.PlayerInfo> _players = new Dictionary<int, ClientSyncManager.PlayerInfo>();

	// Token: 0x040007EE RID: 2030
	private float _lerpTime;

	// Token: 0x040007EF RID: 2031
	private float _tickInterval;

	// Token: 0x040007F0 RID: 2032
	private int _clientsJoining;

	// Token: 0x040007F1 RID: 2033
	private bool dll_Excuted;

	// Token: 0x040007F2 RID: 2034
	private bool dll_Excuted;

	// Token: 0x02000161 RID: 353
	public class PlayerInfo
	{
		// Token: 0x06000BBE RID: 3006 RVA: 0x000338B2 File Offset: 0x00031AB2
		public PlayerInfo(Player player)
		{
			this.reference = player;
			this.lastPosition = Vector2.zero;
			this.nextPosition = player.GetHologramPosition;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x000338D8 File Offset: 0x00031AD8
		public Player Reference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x000338E0 File Offset: 0x00031AE0
		public Vector2 LastPosition
		{
			get
			{
				return this.lastPosition;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x000338E8 File Offset: 0x00031AE8
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x000338F0 File Offset: 0x00031AF0
		public Vector2 NextPosition
		{
			get
			{
				return this.nextPosition;
			}
			set
			{
				this.lastPosition = this.nextPosition;
				this.nextPosition = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x00033905 File Offset: 0x00031B05
		// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x0003390D File Offset: 0x00031B0D
		public Vector2 ActualPosition
		{
			get
			{
				return this.actualPosition;
			}
			set
			{
				this.actualPosition = value;
			}
		}

		// Token: 0x040007F3 RID: 2035
		private Player reference;

		// Token: 0x040007F4 RID: 2036
		private Vector2 lastPosition;

		// Token: 0x040007F5 RID: 2037
		private Vector2 nextPosition;

		// Token: 0x040007F6 RID: 2038
		private Vector2 actualPosition;
	}

	// Token: 0x02000162 RID: 354
	public struct ClientStateInformation
	{
		// Token: 0x040007F7 RID: 2039
		public int[] ids;

		// Token: 0x040007F8 RID: 2040
		public float[] xCoords;

		// Token: 0x040007F9 RID: 2041
		public float[] yCoords;
	}
}
