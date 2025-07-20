using System;
using FishNet.Managing;
using FishNet.Managing.Logging;
using FishNet.Transporting;
using FishySteamworks.Client;
using FishySteamworks.Server;
using Steamworks;
using UnityEngine;

namespace FishySteamworks
{
	// Token: 0x020002B8 RID: 696
	public class FishySteamworks : Transport
	{
		// Token: 0x06001379 RID: 4985 RVA: 0x00058F88 File Offset: 0x00057188
		~FishySteamworks()
		{
			this.Shutdown();
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00058FB4 File Offset: 0x000571B4
		public override void Initialize(NetworkManager networkManager, int transportIndex)
		{
			base.Initialize(networkManager, transportIndex);
			this._client = new ClientSocket();
			this._clientHost = new ClientHostSocket();
			this._server = new ServerSocket();
			this.CreateChannelData();
			this._client.Initialize(this);
			this._clientHost.Initialize(this);
			this._server.Initialize(this);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00059014 File Offset: 0x00057214
		private void OnDestroy()
		{
			this.Shutdown();
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0005901C File Offset: 0x0005721C
		private void Update()
		{
			this._clientHost.CheckSetStarted();
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00059029 File Offset: 0x00057229
		private void CreateChannelData()
		{
			this._mtus = new int[]
			{
				1048576,
				1200
			};
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00059048 File Offset: 0x00057248
		private bool InitializeRelayNetworkAccess()
		{
			bool result;
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				if (this.IsNetworkAccessAvailable())
				{
					this.LocalUserSteamID = SteamUser.GetSteamID().m_SteamID;
				}
				this._shutdownCalled = false;
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00059094 File Offset: 0x00057294
		public bool IsNetworkAccessAvailable()
		{
			bool result;
			try
			{
				InteropHelp.TestIfAvailableClient();
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000590C0 File Offset: 0x000572C0
		public override string GetConnectionAddress(int connectionId)
		{
			return this._server.GetConnectionAddress(connectionId);
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06001381 RID: 4993 RVA: 0x000590D0 File Offset: 0x000572D0
		// (remove) Token: 0x06001382 RID: 4994 RVA: 0x00059108 File Offset: 0x00057308
		public override event Action<ClientConnectionStateArgs> OnClientConnectionState;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06001383 RID: 4995 RVA: 0x00059140 File Offset: 0x00057340
		// (remove) Token: 0x06001384 RID: 4996 RVA: 0x00059178 File Offset: 0x00057378
		public override event Action<ServerConnectionStateArgs> OnServerConnectionState;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001385 RID: 4997 RVA: 0x000591B0 File Offset: 0x000573B0
		// (remove) Token: 0x06001386 RID: 4998 RVA: 0x000591E8 File Offset: 0x000573E8
		public override event Action<RemoteConnectionStateArgs> OnRemoteConnectionState;

		// Token: 0x06001387 RID: 4999 RVA: 0x0005921D File Offset: 0x0005741D
		public override LocalConnectionState GetConnectionState(bool server)
		{
			if (server)
			{
				return this._server.GetLocalConnectionState();
			}
			return this._client.GetLocalConnectionState();
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00059239 File Offset: 0x00057439
		public override RemoteConnectionState GetConnectionState(int connectionId)
		{
			return this._server.GetConnectionState(connectionId);
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00059247 File Offset: 0x00057447
		public override void HandleClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
		{
			Action<ClientConnectionStateArgs> onClientConnectionState = this.OnClientConnectionState;
			if (onClientConnectionState == null)
			{
				return;
			}
			onClientConnectionState(connectionStateArgs);
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0005925A File Offset: 0x0005745A
		public override void HandleServerConnectionState(ServerConnectionStateArgs connectionStateArgs)
		{
			Action<ServerConnectionStateArgs> onServerConnectionState = this.OnServerConnectionState;
			if (onServerConnectionState == null)
			{
				return;
			}
			onServerConnectionState(connectionStateArgs);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005926D File Offset: 0x0005746D
		public override void HandleRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
		{
			Action<RemoteConnectionStateArgs> onRemoteConnectionState = this.OnRemoteConnectionState;
			if (onRemoteConnectionState == null)
			{
				return;
			}
			onRemoteConnectionState(connectionStateArgs);
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00059280 File Offset: 0x00057480
		public override void IterateIncoming(bool server)
		{
			if (server)
			{
				this._server.IterateIncoming();
				return;
			}
			this._client.IterateIncoming();
			this._clientHost.IterateIncoming();
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x000592A7 File Offset: 0x000574A7
		public override void IterateOutgoing(bool server)
		{
			if (server)
			{
				this._server.IterateOutgoing();
				return;
			}
			this._client.IterateOutgoing();
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600138E RID: 5006 RVA: 0x000592C4 File Offset: 0x000574C4
		// (remove) Token: 0x0600138F RID: 5007 RVA: 0x000592FC File Offset: 0x000574FC
		public override event Action<ClientReceivedDataArgs> OnClientReceivedData;

		// Token: 0x06001390 RID: 5008 RVA: 0x00059331 File Offset: 0x00057531
		public override void HandleClientReceivedDataArgs(ClientReceivedDataArgs receivedDataArgs)
		{
			Action<ClientReceivedDataArgs> onClientReceivedData = this.OnClientReceivedData;
			if (onClientReceivedData == null)
			{
				return;
			}
			onClientReceivedData(receivedDataArgs);
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001391 RID: 5009 RVA: 0x00059344 File Offset: 0x00057544
		// (remove) Token: 0x06001392 RID: 5010 RVA: 0x0005937C File Offset: 0x0005757C
		public override event Action<ServerReceivedDataArgs> OnServerReceivedData;

		// Token: 0x06001393 RID: 5011 RVA: 0x000593B1 File Offset: 0x000575B1
		public override void HandleServerReceivedDataArgs(ServerReceivedDataArgs receivedDataArgs)
		{
			Action<ServerReceivedDataArgs> onServerReceivedData = this.OnServerReceivedData;
			if (onServerReceivedData == null)
			{
				return;
			}
			onServerReceivedData(receivedDataArgs);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000593C4 File Offset: 0x000575C4
		public override void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			this._client.SendToServer(channelId, segment);
			this._clientHost.SendToServer(channelId, segment);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000593E0 File Offset: 0x000575E0
		public override void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			this._server.SendToClient(channelId, segment, connectionId);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000593F0 File Offset: 0x000575F0
		public override int GetMaximumClients()
		{
			return this._server.GetMaximumClients();
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000593FD File Offset: 0x000575FD
		public override void SetMaximumClients(int value)
		{
			this._server.SetMaximumClients(value);
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0005940B File Offset: 0x0005760B
		public override void SetClientAddress(string address)
		{
			this._clientAddress = address;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00059414 File Offset: 0x00057614
		public override void SetServerBindAddress(string address, IPAddressType addressType)
		{
			this._serverBindAddress = address;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0005941D File Offset: 0x0005761D
		public override void SetPort(ushort port)
		{
			this._port = port;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00059426 File Offset: 0x00057626
		public override bool StartConnection(bool server)
		{
			if (server)
			{
				return this.StartServer();
			}
			return this.StartClient(this._clientAddress);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0005943E File Offset: 0x0005763E
		public override bool StopConnection(bool server)
		{
			if (server)
			{
				return this.StopServer();
			}
			return this.StopClient();
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00059450 File Offset: 0x00057650
		public override bool StopConnection(int connectionId, bool immediately)
		{
			return this.StopClient(connectionId, immediately);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0005945A File Offset: 0x0005765A
		public override void Shutdown()
		{
			if (this._shutdownCalled)
			{
				return;
			}
			this._shutdownCalled = true;
			this.StopConnection(false);
			this.StopConnection(true);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0005947C File Offset: 0x0005767C
		private bool StartServer()
		{
			if (!this.InitializeRelayNetworkAccess())
			{
				if (base.NetworkManager.CanLog(LoggingType.Error))
				{
					Debug.LogError("RelayNetworkAccess could not be initialized.");
				}
				return false;
			}
			if (!this.IsNetworkAccessAvailable())
			{
				if (base.NetworkManager.CanLog(LoggingType.Error))
				{
					Debug.LogError("Server network access is not available.");
				}
				return false;
			}
			this._server.ResetInvalidSocket();
			if (this._server.GetLocalConnectionState() != LocalConnectionState.Stopped)
			{
				if (base.NetworkManager.CanLog(LoggingType.Error))
				{
					Debug.LogError("Server is already running.");
				}
				return false;
			}
			bool flag = this._client.GetLocalConnectionState() > LocalConnectionState.Stopped;
			if (flag)
			{
				this._client.StopConnection();
			}
			bool flag2 = this._server.StartConnection(this._serverBindAddress, this._port, (int)this._maximumClients, this._peerToPeer);
			if (flag2 && flag)
			{
				this.StartConnection(false);
			}
			return flag2;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0005954D File Offset: 0x0005774D
		private bool StopServer()
		{
			return this._server != null && this._server.StopConnection();
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00059564 File Offset: 0x00057764
		private bool StartClient(string address)
		{
			if (this._server.GetLocalConnectionState() == LocalConnectionState.Stopped)
			{
				if (this._client.GetLocalConnectionState() != LocalConnectionState.Stopped)
				{
					if (base.NetworkManager.CanLog(LoggingType.Error))
					{
						Debug.LogError("Client is already running.");
					}
					return false;
				}
				if (this._clientHost.GetLocalConnectionState() != LocalConnectionState.Stopped)
				{
					this._clientHost.StopConnection();
				}
				if (!this.InitializeRelayNetworkAccess())
				{
					if (base.NetworkManager.CanLog(LoggingType.Error))
					{
						Debug.LogError("RelayNetworkAccess could not be initialized.");
					}
					return false;
				}
				if (!this.IsNetworkAccessAvailable())
				{
					if (base.NetworkManager.CanLog(LoggingType.Error))
					{
						Debug.LogError("Client network access is not available.");
					}
					return false;
				}
				this._client.StartConnection(address, this._port, this._peerToPeer);
			}
			else
			{
				this._clientHost.StartConnection(this._server);
			}
			return true;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00059634 File Offset: 0x00057834
		private bool StopClient()
		{
			bool flag = false;
			if (this._client != null)
			{
				flag |= this._client.StopConnection();
			}
			if (this._clientHost != null)
			{
				flag |= this._clientHost.StopConnection();
			}
			return flag;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00059670 File Offset: 0x00057870
		private bool StopClient(int connectionId, bool immediately)
		{
			return this._server.StopConnection(connectionId);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0005967E File Offset: 0x0005787E
		public override int GetMTU(byte channel)
		{
			if ((int)channel >= this._mtus.Length)
			{
				Debug.LogError(string.Format("Channel {0} is out of bounds.", channel));
				return 0;
			}
			return this._mtus[(int)channel];
		}

		// Token: 0x040010F4 RID: 4340
		[NonSerialized]
		public ulong LocalUserSteamID;

		// Token: 0x040010F5 RID: 4341
		[Tooltip("Address server should bind to.")]
		[SerializeField]
		private string _serverBindAddress = string.Empty;

		// Token: 0x040010F6 RID: 4342
		[Tooltip("Port to use.")]
		[SerializeField]
		private ushort _port = 7770;

		// Token: 0x040010F7 RID: 4343
		[Tooltip("Maximum number of players which may be connected at once.")]
		[Range(1f, 65535f)]
		[SerializeField]
		private ushort _maximumClients = 9001;

		// Token: 0x040010F8 RID: 4344
		[Tooltip("True if using peer to peer socket.")]
		[SerializeField]
		private bool _peerToPeer;

		// Token: 0x040010F9 RID: 4345
		[Tooltip("Address client should connect to.")]
		[SerializeField]
		private string _clientAddress = string.Empty;

		// Token: 0x040010FA RID: 4346
		private int[] _mtus;

		// Token: 0x040010FB RID: 4347
		private ClientSocket _client;

		// Token: 0x040010FC RID: 4348
		private ClientHostSocket _clientHost;

		// Token: 0x040010FD RID: 4349
		private ServerSocket _server;

		// Token: 0x040010FE RID: 4350
		private bool _shutdownCalled = true;

		// Token: 0x040010FF RID: 4351
		internal const int CLIENT_HOST_ID = 32767;
	}
}
