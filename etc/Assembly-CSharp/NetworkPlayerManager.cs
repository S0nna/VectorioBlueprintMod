using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000052 RID: 82
[DefaultExecutionOrder(1)]
public class NetworkPlayerManager : Singleton<NetworkPlayerManager>
{
	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000441 RID: 1089 RVA: 0x000166D4 File Offset: 0x000148D4
	public static NetworkConnection Client
	{
		get
		{
			return NetworkPlayerManager._client;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x000166DB File Offset: 0x000148DB
	// (set) Token: 0x06000443 RID: 1091 RVA: 0x000166E2 File Offset: 0x000148E2
	public static bool IS_HOST
	{
		get
		{
			return NetworkPlayerManager._isHost;
		}
		set
		{
			NetworkPlayerManager._isHost = value;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x000166EA File Offset: 0x000148EA
	public static bool ONLY_CLIENT_ON_SERVER
	{
		get
		{
			return NetworkPlayerManager._onlyClientOnServer;
		}
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x000166F1 File Offset: 0x000148F1
	public static void SET_LOCAL_FLAG(bool flag)
	{
		NetworkPlayerManager._onlyClientOnServer = flag;
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x000166F9 File Offset: 0x000148F9
	// (set) Token: 0x06000447 RID: 1095 RVA: 0x00016700 File Offset: 0x00014900
	public static bool LOCAL_CLIENT_READY
	{
		get
		{
			return NetworkPlayerManager._localClientReady;
		}
		set
		{
			NetworkPlayerManager._localClientReady = value;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000449 RID: 1097 RVA: 0x0001673A File Offset: 0x0001493A
	// (set) Token: 0x06000448 RID: 1096 RVA: 0x00016708 File Offset: 0x00014908
	public bool IsPlayerLoading
	{
		get
		{
			return this._playerLoading;
		}
		set
		{
			this._playerLoading = value;
			Singleton<Events>.Instance.onPauseStateUpdated.Invoke();
			Debug.Log("[GAMEMODE] Player loading state toggled to: " + this._playerLoading.ToString());
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00016744 File Offset: 0x00014944
	private void Start()
	{
		if (this._isSetup)
		{
			return;
		}
		if (this._networkManager == null)
		{
			this._networkManager = InstanceFinder.NetworkManager;
		}
		this._networkManager.SceneManager.OnClientLoadedStartScenes += this.OnStartLoadingPlayer;
		this._isSetup = true;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00016796 File Offset: 0x00014996
	private void OnDisable()
	{
		if (!this._isSetup)
		{
			return;
		}
		this._networkManager.SceneManager.OnClientLoadedStartScenes -= this.OnStartLoadingPlayer;
		this._isSetup = false;
		NetworkPlayerManager._isHost = false;
		NetworkPlayerManager._onlyClientOnServer = true;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x000167D0 File Offset: 0x000149D0
	public void OnStartLoadingPlayer(NetworkConnection connection, bool asServer)
	{
		if (!asServer)
		{
			Debug.Log("[NETWORK] Bypassing spawn player request for " + connection.ClientId.ToString());
			return;
		}
		NetworkObject pooledInstantiated = this._networkManager.GetPooledInstantiated(this._playerPrefab, Vector2.zero, Quaternion.identity, true);
		this._networkManager.ServerManager.Spawn(pooledInstantiated, connection, default(Scene));
		Debug.Log("[NETWORK] Player object spawned for client ID " + connection.ClientId.ToString());
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00016854 File Offset: 0x00014A54
	public void OnPlayerLoaded(NetworkConnection connection)
	{
		if (connection.IsHost)
		{
			Singleton<NetworkBootstrap>.Instance.CreateClientReceivers();
			Singleton<NetworkBootstrap>.Instance.CreateGameServer();
			NetworkPlayerManager._isHost = true;
		}
		else
		{
			NetworkSingleton<ClientSyncManager>.Instance.CreateClientReceivers(connection);
		}
		if (!connection.IsHost)
		{
			NetworkSingleton<ClientSyncManager>.Instance.Srv_SyncPlayerLoading(true);
			ServerSingleton<ServerStateManager>.Instance.Srv_RequestWorldState(connection);
			return;
		}
		Singleton<NetworkEvents>.Instance.onLocalClientReadyToLoad.Invoke(false);
	}

	// Token: 0x04000224 RID: 548
	private static NetworkConnection _client;

	// Token: 0x04000225 RID: 549
	private static bool _isHost = false;

	// Token: 0x04000226 RID: 550
	private static bool _onlyClientOnServer = true;

	// Token: 0x04000227 RID: 551
	public static bool _localClientReady = true;

	// Token: 0x04000228 RID: 552
	[SerializeField]
	private NetworkManager _networkManager;

	// Token: 0x04000229 RID: 553
	[SerializeField]
	private NetworkObject _playerPrefab;

	// Token: 0x0400022A RID: 554
	private bool _isSetup;

	// Token: 0x0400022B RID: 555
	private bool _playerLoading;
}
