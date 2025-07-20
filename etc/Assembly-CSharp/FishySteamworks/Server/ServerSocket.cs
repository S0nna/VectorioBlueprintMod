using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Managing.Logging;
using FishNet.Transporting;
using FishySteamworks.Client;
using Steamworks;
using UnityEngine;

namespace FishySteamworks.Server
{
	// Token: 0x020002B9 RID: 697
	public class ServerSocket : CommonSocket
	{
		// Token: 0x060013A6 RID: 5030 RVA: 0x000596E5 File Offset: 0x000578E5
		internal RemoteConnectionState GetConnectionState(int connectionId)
		{
			if (this._steamConnections.Second.ContainsKey(connectionId))
			{
				return RemoteConnectionState.Started;
			}
			return RemoteConnectionState.Stopped;
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x000596FD File Offset: 0x000578FD
		internal void ResetInvalidSocket()
		{
			if (this._socket == HSteamListenSocket.Invalid)
			{
				base.SetLocalConnectionState(LocalConnectionState.Stopped, true);
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0005971C File Offset: 0x0005791C
		internal bool StartConnection(string address, ushort port, int maximumClients, bool peerToPeer)
		{
			try
			{
				if (this._onRemoteConnectionStateCallback == null)
				{
					this._onRemoteConnectionStateCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnRemoteConnectionState));
				}
				this.PeerToPeer = peerToPeer;
				byte[] array = (!peerToPeer) ? base.GetIPBytes(address) : null;
				this.PeerToPeer = peerToPeer;
				this.SetMaximumClients(maximumClients);
				this._nextConnectionId = 0;
				this._cachedConnectionIds.Clear();
				base.SetLocalConnectionState(LocalConnectionState.Starting, true);
				SteamNetworkingConfigValue_t[] array2 = new SteamNetworkingConfigValue_t[0];
				if (this.PeerToPeer)
				{
					this._socket = SteamNetworkingSockets.CreateListenSocketP2P(0, array2.Length, array2);
				}
				else
				{
					SteamNetworkingIPAddr steamNetworkingIPAddr = default(SteamNetworkingIPAddr);
					steamNetworkingIPAddr.Clear();
					if (array != null)
					{
						steamNetworkingIPAddr.SetIPv6(array, port);
					}
					this._socket = SteamNetworkingSockets.CreateListenSocketIP(ref steamNetworkingIPAddr, 0, array2);
				}
			}
			catch
			{
				base.SetLocalConnectionState(LocalConnectionState.Stopped, true);
				return false;
			}
			base.SetLocalConnectionState(LocalConnectionState.Started, true);
			return true;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00059800 File Offset: 0x00057A00
		internal bool StopConnection()
		{
			if (this._socket != HSteamListenSocket.Invalid)
			{
				SteamNetworkingSockets.CloseListenSocket(this._socket);
				if (this._onRemoteConnectionStateCallback != null)
				{
					this._onRemoteConnectionStateCallback.Dispose();
					this._onRemoteConnectionStateCallback = null;
				}
				this._socket = HSteamListenSocket.Invalid;
			}
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped)
			{
				return false;
			}
			base.SetLocalConnectionState(LocalConnectionState.Stopping, true);
			base.SetLocalConnectionState(LocalConnectionState.Stopped, true);
			return true;
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0005986C File Offset: 0x00057A6C
		internal bool StopConnection(int connectionId)
		{
			if (connectionId == 32767)
			{
				if (this._clientHost != null)
				{
					this._clientHost.StopConnection();
					return true;
				}
				return false;
			}
			else
			{
				HSteamNetConnection socket;
				if (this._steamConnections.Second.TryGetValue(connectionId, out socket))
				{
					return this.StopConnection(connectionId, socket);
				}
				if (this.Transport.NetworkManager.CanLog(LoggingType.Error))
				{
					Debug.LogError(string.Format("Steam connection not found for connectionId {0}.", connectionId));
				}
				return false;
			}
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000598E0 File Offset: 0x00057AE0
		private bool StopConnection(int connectionId, HSteamNetConnection socket)
		{
			SteamNetworkingSockets.CloseConnection(socket, 0, string.Empty, false);
			this._steamConnections.Remove(connectionId);
			this._steamIds.Remove(connectionId);
			if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
			{
				Debug.Log(string.Format("Client with ConnectionID {0} disconnected.", connectionId));
			}
			this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Stopped, connectionId, this.Transport.Index));
			this._cachedConnectionIds.Enqueue(connectionId);
			return true;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00059968 File Offset: 0x00057B68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void OnRemoteConnectionState(SteamNetConnectionStatusChangedCallback_t args)
		{
			ulong steamID = args.m_info.m_identityRemote.GetSteamID64();
			if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting)
			{
				if (this._steamConnections.Count >= this.GetMaximumClients())
				{
					if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
					{
						Debug.Log(string.Format("Incoming connection {0} was rejected because would exceed the maximum connection count.", steamID));
					}
					SteamNetworkingSockets.CloseConnection(args.m_hConn, 0, "Max Connection Count", false);
					return;
				}
				EResult eresult = SteamNetworkingSockets.AcceptConnection(args.m_hConn);
				if (eresult == EResult.k_EResultOK)
				{
					if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
					{
						Debug.Log(string.Format("Accepting connection {0}", steamID));
						return;
					}
				}
				else if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
				{
					Debug.Log(string.Format("Connection {0} could not be accepted: {1}", steamID, eresult.ToString()));
					return;
				}
			}
			else
			{
				if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
				{
					int num;
					if (this._cachedConnectionIds.Count <= 0)
					{
						int nextConnectionId = this._nextConnectionId;
						this._nextConnectionId = nextConnectionId + 1;
						num = nextConnectionId;
					}
					else
					{
						num = this._cachedConnectionIds.Dequeue();
					}
					int num2 = num;
					this._steamConnections.Add(args.m_hConn, num2);
					this._steamIds.Add(args.m_info.m_identityRemote.GetSteamID(), num2);
					if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
					{
						Debug.Log(string.Format("Client with SteamID {0} connected. Assigning connection id {1}", steamID, num2));
					}
					this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Started, num2, this.Transport.Index));
					return;
				}
				if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer || args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally)
				{
					int connectionId;
					if (this._steamConnections.TryGetValue(args.m_hConn, out connectionId))
					{
						this.StopConnection(connectionId, args.m_hConn);
						return;
					}
				}
				else if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
				{
					Debug.Log(string.Format("Connection {0} state changed: {1}", steamID, args.m_info.m_eState.ToString()));
				}
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00059B98 File Offset: 0x00057D98
		internal void IterateOutgoing()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			foreach (HSteamNetConnection hConn in this._steamConnections.FirstTypes)
			{
				SteamNetworkingSockets.FlushMessagesOnConnection(hConn);
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00059BF4 File Offset: 0x00057DF4
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped || base.GetLocalConnectionState() == LocalConnectionState.Stopping)
			{
				return;
			}
			while (this._clientHostIncoming.Count > 0)
			{
				LocalPacket localPacket = this._clientHostIncoming.Dequeue();
				ArraySegment<byte> data = new ArraySegment<byte>(localPacket.Data, 0, localPacket.Length);
				this.Transport.HandleServerReceivedDataArgs(new ServerReceivedDataArgs(data, (Channel)localPacket.Channel, 32767, this.Transport.Index));
			}
			foreach (KeyValuePair<HSteamNetConnection, int> keyValuePair in this._steamConnections.First)
			{
				HSteamNetConnection key = keyValuePair.Key;
				int value = keyValuePair.Value;
				int num = SteamNetworkingSockets.ReceiveMessagesOnConnection(key, this.MessagePointers, 256);
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						ArraySegment<byte> data2;
						byte channel;
						base.GetMessage(this.MessagePointers[i], this.InboundBuffer, out data2, out channel);
						this.Transport.HandleServerReceivedDataArgs(new ServerReceivedDataArgs(data2, (Channel)channel, value, this.Transport.Index));
					}
				}
			}
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00059D24 File Offset: 0x00057F24
		internal void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			if (connectionId == 32767)
			{
				if (this._clientHost != null)
				{
					LocalPacket packet = new LocalPacket(segment, channelId);
					this._clientHost.ReceivedFromLocalServer(packet);
				}
				return;
			}
			HSteamNetConnection hsteamNetConnection;
			if (this._steamConnections.TryGetValue(connectionId, out hsteamNetConnection))
			{
				EResult eresult = base.Send(hsteamNetConnection, segment, channelId);
				if (eresult == EResult.k_EResultNoConnection || eresult == EResult.k_EResultInvalidParam)
				{
					if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
					{
						Debug.Log(string.Format("Connection to {0} was lost.", connectionId));
					}
					this.StopConnection(connectionId, hsteamNetConnection);
					return;
				}
				if (eresult != EResult.k_EResultOK && this.Transport.NetworkManager.CanLog(LoggingType.Error))
				{
					Debug.LogError("Could not send: " + eresult.ToString());
					return;
				}
			}
			else if (this.Transport.NetworkManager.CanLog(LoggingType.Error))
			{
				Debug.LogError(string.Format("ConnectionId {0} does not exist, data will not be sent.", connectionId));
			}
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00059E14 File Offset: 0x00058014
		internal string GetConnectionAddress(int connectionId)
		{
			CSteamID csteamID;
			if (this._steamIds.TryGetValue(connectionId, out csteamID))
			{
				return csteamID.ToString();
			}
			if (this.Transport.NetworkManager.CanLog(LoggingType.Error))
			{
				Debug.LogError(string.Format("ConnectionId {0} is invalid; address cannot be returned.", connectionId));
			}
			return string.Empty;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00059E6C File Offset: 0x0005806C
		internal void SetMaximumClients(int value)
		{
			this._maximumClients = Math.Min(value, 32766);
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00059E7F File Offset: 0x0005807F
		internal int GetMaximumClients()
		{
			return this._maximumClients;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00059E87 File Offset: 0x00058087
		internal void SetClientHostSocket(ClientHostSocket socket)
		{
			this._clientHost = socket;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00059E90 File Offset: 0x00058090
		internal void OnClientHostState(bool started)
		{
			FishySteamworks fishySteamworks = (FishySteamworks)this.Transport;
			CSteamID key = new CSteamID(fishySteamworks.LocalUserSteamID);
			if (!started && this._clientHostStarted)
			{
				base.ClearQueue(this._clientHostIncoming);
				this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Stopped, 32767, this.Transport.Index));
				this._steamIds.Remove(key);
			}
			else if (started)
			{
				this._steamIds[key] = 32767;
				this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Started, 32767, this.Transport.Index));
			}
			this._clientHostStarted = started;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00059F38 File Offset: 0x00058138
		internal void ReceivedFromClientHost(LocalPacket packet)
		{
			if (!this._clientHostStarted)
			{
				return;
			}
			this._clientHostIncoming.Enqueue(packet);
		}

		// Token: 0x04001105 RID: 4357
		private BidirectionalDictionary<HSteamNetConnection, int> _steamConnections = new BidirectionalDictionary<HSteamNetConnection, int>();

		// Token: 0x04001106 RID: 4358
		private BidirectionalDictionary<CSteamID, int> _steamIds = new BidirectionalDictionary<CSteamID, int>();

		// Token: 0x04001107 RID: 4359
		private int _maximumClients;

		// Token: 0x04001108 RID: 4360
		private int _nextConnectionId;

		// Token: 0x04001109 RID: 4361
		private HSteamListenSocket _socket = new HSteamListenSocket(0U);

		// Token: 0x0400110A RID: 4362
		private Queue<LocalPacket> _clientHostIncoming = new Queue<LocalPacket>();

		// Token: 0x0400110B RID: 4363
		private bool _clientHostStarted;

		// Token: 0x0400110C RID: 4364
		private Callback<SteamNetConnectionStatusChangedCallback_t> _onRemoteConnectionStateCallback;

		// Token: 0x0400110D RID: 4365
		private Queue<int> _cachedConnectionIds = new Queue<int>();

		// Token: 0x0400110E RID: 4366
		private ClientHostSocket _clientHost;
	}
}
