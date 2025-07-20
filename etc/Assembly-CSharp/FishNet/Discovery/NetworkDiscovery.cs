using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FishNet.Managing;
using FishNet.Managing.Logging;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Discovery
{
	// Token: 0x020002AC RID: 684
	public sealed class NetworkDiscovery : MonoBehaviour
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001317 RID: 4887 RVA: 0x000577B0 File Offset: 0x000559B0
		// (remove) Token: 0x06001318 RID: 4888 RVA: 0x000577E8 File Offset: 0x000559E8
		public event Action<IPEndPoint> ServerFoundCallback;

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x0005781D File Offset: 0x00055A1D
		// (set) Token: 0x0600131A RID: 4890 RVA: 0x00057825 File Offset: 0x00055A25
		public bool IsAdvertising { get; private set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x0005782E File Offset: 0x00055A2E
		// (set) Token: 0x0600131C RID: 4892 RVA: 0x00057836 File Offset: 0x00055A36
		public bool IsSearching { get; private set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x0005783F File Offset: 0x00055A3F
		private float SearchTimeout
		{
			get
			{
				if (this.searchTimeout >= 1f)
				{
					return this.searchTimeout;
				}
				return 1f;
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0005785C File Offset: 0x00055A5C
		private void Awake()
		{
			if (base.TryGetComponent<NetworkManager>(out this._networkManager))
			{
				this.LogInformation("Using NetworkManager on " + base.gameObject.name + ".");
				this._secretBytes = Encoding.UTF8.GetBytes(this.secret);
				this._mainThreadSynchronizationContext = SynchronizationContext.Current;
				return;
			}
			this.LogError("No NetworkManager found on " + base.gameObject.name + ". Component will be disabled.");
			base.enabled = false;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x000578E0 File Offset: 0x00055AE0
		private void OnEnable()
		{
			if (!this.automatic)
			{
				return;
			}
			this._networkManager.ServerManager.OnServerConnectionState += this.ServerConnectionStateChangedEventHandler;
			this._networkManager.ClientManager.OnClientConnectionState += this.ClientConnectionStateChangedEventHandler;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0005792E File Offset: 0x00055B2E
		private void OnDisable()
		{
			this.Shutdown();
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0005792E File Offset: 0x00055B2E
		private void OnDestroy()
		{
			this.Shutdown();
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0005792E File Offset: 0x00055B2E
		private void OnApplicationQuit()
		{
			this.Shutdown();
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00057936 File Offset: 0x00055B36
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				this.AdvertiseServer();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				this.SearchForServers();
			}
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00057958 File Offset: 0x00055B58
		private void Shutdown()
		{
			if (this._networkManager != null)
			{
				this._networkManager.ServerManager.OnServerConnectionState -= this.ServerConnectionStateChangedEventHandler;
				this._networkManager.ClientManager.OnClientConnectionState -= this.ClientConnectionStateChangedEventHandler;
			}
			this.StopSearchingOrAdvertising();
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x000579B1 File Offset: 0x00055BB1
		private void ServerConnectionStateChangedEventHandler(ServerConnectionStateArgs args)
		{
			if (args.ConnectionState == LocalConnectionState.Started)
			{
				this.AdvertiseServer();
				return;
			}
			if (args.ConnectionState == LocalConnectionState.Stopped)
			{
				this.StopSearchingOrAdvertising();
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000579D1 File Offset: 0x00055BD1
		private void ClientConnectionStateChangedEventHandler(ClientConnectionStateArgs args)
		{
			if (this._networkManager.IsServer)
			{
				return;
			}
			if (args.ConnectionState == LocalConnectionState.Started)
			{
				this.StopSearchingOrAdvertising();
				return;
			}
			if (args.ConnectionState == LocalConnectionState.Stopped)
			{
				this.SearchForServers();
			}
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x000579FF File Offset: 0x00055BFF
		public void AdvertiseServer()
		{
			if (this.IsAdvertising)
			{
				this.LogWarning("Server is already being advertised.");
				return;
			}
			this._cancellationTokenSource = new CancellationTokenSource();
			this.AdvertiseServerAsync(this._cancellationTokenSource.Token).ConfigureAwait(false);
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x00057A38 File Offset: 0x00055C38
		public void SearchForServers()
		{
			if (this.IsSearching)
			{
				this.LogWarning("Already searching for servers.");
				return;
			}
			this._cancellationTokenSource = new CancellationTokenSource();
			this.SearchForServersAsync(this._cancellationTokenSource.Token).ConfigureAwait(false);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00057A71 File Offset: 0x00055C71
		public void StopSearchingOrAdvertising()
		{
			if (this._cancellationTokenSource == null)
			{
				this.LogWarning("Not searching or advertising.");
				return;
			}
			this._cancellationTokenSource.Cancel();
			this._cancellationTokenSource.Dispose();
			this._cancellationTokenSource = null;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00057AA4 File Offset: 0x00055CA4
		private Task AdvertiseServerAsync(CancellationToken cancellationToken)
		{
			NetworkDiscovery.<AdvertiseServerAsync>d__34 <AdvertiseServerAsync>d__;
			<AdvertiseServerAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AdvertiseServerAsync>d__.<>4__this = this;
			<AdvertiseServerAsync>d__.cancellationToken = cancellationToken;
			<AdvertiseServerAsync>d__.<>1__state = -1;
			<AdvertiseServerAsync>d__.<>t__builder.Start<NetworkDiscovery.<AdvertiseServerAsync>d__34>(ref <AdvertiseServerAsync>d__);
			return <AdvertiseServerAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00057AF0 File Offset: 0x00055CF0
		private Task SearchForServersAsync(CancellationToken cancellationToken)
		{
			NetworkDiscovery.<SearchForServersAsync>d__35 <SearchForServersAsync>d__;
			<SearchForServersAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SearchForServersAsync>d__.<>4__this = this;
			<SearchForServersAsync>d__.cancellationToken = cancellationToken;
			<SearchForServersAsync>d__.<>1__state = -1;
			<SearchForServersAsync>d__.<>t__builder.Start<NetworkDiscovery.<SearchForServersAsync>d__35>(ref <SearchForServersAsync>d__);
			return <SearchForServersAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00057B3B File Offset: 0x00055D3B
		private void LogInformation(string message)
		{
			if (this._networkManager.CanLog(LoggingType.Common))
			{
				Debug.Log("[NetworkDiscovery] " + message, this);
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00057B5C File Offset: 0x00055D5C
		private void LogWarning(string message)
		{
			if (this._networkManager.CanLog(LoggingType.Warning))
			{
				Debug.LogWarning("[NetworkDiscovery] " + message, this);
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00057B7D File Offset: 0x00055D7D
		private void LogError(string message)
		{
			if (this._networkManager.CanLog(LoggingType.Error))
			{
				Debug.LogError("[NetworkDiscovery] " + message, this);
			}
		}

		// Token: 0x040010BB RID: 4283
		private static readonly byte[] OkBytes = new byte[]
		{
			1
		};

		// Token: 0x040010BC RID: 4284
		private NetworkManager _networkManager;

		// Token: 0x040010BD RID: 4285
		[SerializeField]
		[Tooltip("Secret to use when advertising or searching for servers.")]
		private string secret;

		// Token: 0x040010BE RID: 4286
		private byte[] _secretBytes;

		// Token: 0x040010BF RID: 4287
		[SerializeField]
		[Tooltip("Port to use when advertising or searching for servers.")]
		private ushort port;

		// Token: 0x040010C0 RID: 4288
		[SerializeField]
		[Tooltip("How long (in seconds) to wait for a response when advertising or searching for servers.")]
		private float searchTimeout;

		// Token: 0x040010C1 RID: 4289
		[SerializeField]
		private bool automatic;

		// Token: 0x040010C2 RID: 4290
		private SynchronizationContext _mainThreadSynchronizationContext;

		// Token: 0x040010C3 RID: 4291
		private CancellationTokenSource _cancellationTokenSource;
	}
}
