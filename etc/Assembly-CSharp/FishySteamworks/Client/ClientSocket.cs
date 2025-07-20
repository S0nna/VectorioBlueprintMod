using System;
using System.Diagnostics;
using System.Threading;
using FishNet.Managing.Logging;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;

namespace FishySteamworks.Client
{
	// Token: 0x020002BB RID: 699
	public class ClientSocket : CommonSocket
	{
		// Token: 0x060013BF RID: 5055 RVA: 0x0005A11C File Offset: 0x0005831C
		private void CheckTimeout()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				if ((float)(stopwatch.ElapsedMilliseconds / 1000L) > this._connectTimeout)
				{
					this.StopConnection();
				}
				Thread.Sleep(50);
			}
			while (base.GetLocalConnectionState() == LocalConnectionState.Starting);
			stopwatch.Stop();
			this._timeoutThread.Abort();
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0005A174 File Offset: 0x00058374
		internal bool StartConnection(string address, ushort port, bool peerToPeer)
		{
			try
			{
				if (this._onLocalConnectionStateCallback == null)
				{
					this._onLocalConnectionStateCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnLocalConnectionState));
				}
				this.PeerToPeer = peerToPeer;
				byte[] array = (!peerToPeer) ? base.GetIPBytes(address) : null;
				if (!peerToPeer && array == null)
				{
					base.SetLocalConnectionState(LocalConnectionState.Stopped, false);
					return false;
				}
				base.SetLocalConnectionState(LocalConnectionState.Starting, false);
				this._connectTimeout = Time.unscaledTime + 8000f;
				this._timeoutThread = new Thread(new ThreadStart(this.CheckTimeout));
				this._timeoutThread.Start();
				this._hostSteamID = new CSteamID(ulong.Parse(address));
				SteamNetworkingIdentity steamNetworkingIdentity = default(SteamNetworkingIdentity);
				steamNetworkingIdentity.SetSteamID(this._hostSteamID);
				SteamNetworkingConfigValue_t[] array2 = new SteamNetworkingConfigValue_t[0];
				if (this.PeerToPeer)
				{
					this._socket = SteamNetworkingSockets.ConnectP2P(ref steamNetworkingIdentity, 0, array2.Length, array2);
				}
				else
				{
					SteamNetworkingIPAddr steamNetworkingIPAddr = default(SteamNetworkingIPAddr);
					steamNetworkingIPAddr.Clear();
					steamNetworkingIPAddr.SetIPv6(array, port);
					this._socket = SteamNetworkingSockets.ConnectByIPAddress(ref steamNetworkingIPAddr, 0, array2);
				}
			}
			catch
			{
				base.SetLocalConnectionState(LocalConnectionState.Stopped, false);
				return false;
			}
			return true;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0005A298 File Offset: 0x00058498
		private void OnLocalConnectionState(SteamNetConnectionStatusChangedCallback_t args)
		{
			if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
			{
				base.SetLocalConnectionState(LocalConnectionState.Started, false);
				return;
			}
			if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer || args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally)
			{
				if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
				{
					Debug.Log("Connection was closed by peer, " + args.m_info.m_szEndDebug);
				}
				this.StopConnection();
				return;
			}
			if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
			{
				Debug.Log("Connection state changed: " + args.m_info.m_eState.ToString() + " - " + args.m_info.m_szEndDebug);
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0005A358 File Offset: 0x00058558
		internal bool StopConnection()
		{
			if (this._timeoutThread != null && this._timeoutThread.IsAlive)
			{
				this._timeoutThread.Abort();
			}
			if (this._socket != HSteamNetConnection.Invalid)
			{
				if (this._onLocalConnectionStateCallback != null)
				{
					this._onLocalConnectionStateCallback.Dispose();
					this._onLocalConnectionStateCallback = null;
				}
				SteamNetworkingSockets.CloseConnection(this._socket, 0, string.Empty, false);
				this._socket = HSteamNetConnection.Invalid;
			}
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped || base.GetLocalConnectionState() == LocalConnectionState.Stopping)
			{
				return false;
			}
			base.SetLocalConnectionState(LocalConnectionState.Stopping, false);
			base.SetLocalConnectionState(LocalConnectionState.Stopped, false);
			return true;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0005A3F4 File Offset: 0x000585F4
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			int num = SteamNetworkingSockets.ReceiveMessagesOnConnection(this._socket, this.MessagePointers, 256);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					ArraySegment<byte> data;
					byte channel;
					base.GetMessage(this.MessagePointers[i], this.InboundBuffer, out data, out channel);
					this.Transport.HandleClientReceivedDataArgs(new ClientReceivedDataArgs(data, (Channel)channel, this.Transport.Index));
				}
			}
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0005A468 File Offset: 0x00058668
		internal void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			EResult eresult = base.Send(this._socket, segment, channelId);
			if (eresult == EResult.k_EResultNoConnection || eresult == EResult.k_EResultInvalidParam)
			{
				if (this.Transport.NetworkManager.CanLog(LoggingType.Common))
				{
					Debug.Log("Connection to server was lost.");
				}
				this.StopConnection();
				return;
			}
			if (eresult != EResult.k_EResultOK && this.Transport.NetworkManager.CanLog(LoggingType.Error))
			{
				Debug.LogError("Could not send: " + eresult.ToString());
			}
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0005A4EE File Offset: 0x000586EE
		internal void IterateOutgoing()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			SteamNetworkingSockets.FlushMessagesOnConnection(this._socket);
		}

		// Token: 0x04001111 RID: 4369
		private Callback<SteamNetConnectionStatusChangedCallback_t> _onLocalConnectionStateCallback;

		// Token: 0x04001112 RID: 4370
		private CSteamID _hostSteamID = CSteamID.Nil;

		// Token: 0x04001113 RID: 4371
		private HSteamNetConnection _socket;

		// Token: 0x04001114 RID: 4372
		private Thread _timeoutThread;

		// Token: 0x04001115 RID: 4373
		private float _connectTimeout = -1f;

		// Token: 0x04001116 RID: 4374
		private const float CONNECT_TIMEOUT_DURATION = 8000f;
	}
}
