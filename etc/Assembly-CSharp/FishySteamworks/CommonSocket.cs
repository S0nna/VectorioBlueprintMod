using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using FishNet.Managing.Logging;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using Steamworks;
using UnityEngine;

namespace FishySteamworks
{
	// Token: 0x020002B6 RID: 694
	public abstract class CommonSocket
	{
		// Token: 0x0600136F RID: 4975 RVA: 0x00058CA3 File Offset: 0x00056EA3
		internal LocalConnectionState GetLocalConnectionState()
		{
			return this._connectionState;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00058CAC File Offset: 0x00056EAC
		protected virtual void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			if (connectionState == this._connectionState)
			{
				return;
			}
			this._connectionState = connectionState;
			if (server)
			{
				this.Transport.HandleServerConnectionState(new ServerConnectionStateArgs(connectionState, this.Transport.Index));
				return;
			}
			this.Transport.HandleClientConnectionState(new ClientConnectionStateArgs(connectionState, this.Transport.Index));
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00058D08 File Offset: 0x00056F08
		internal virtual void Initialize(Transport t)
		{
			this.Transport = t;
			int num = this.Transport.GetMTU(0);
			num = Math.Max(num, this.Transport.GetMTU(1));
			this.InboundBuffer = new byte[num];
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00058D48 File Offset: 0x00056F48
		protected byte[] GetIPBytes(string address)
		{
			if (string.IsNullOrEmpty(address))
			{
				return null;
			}
			IPAddress ipaddress;
			if (!IPAddress.TryParse(address, out ipaddress))
			{
				if (this.Transport.NetworkManager.CanLog(LoggingType.Error))
				{
					Debug.LogError("Could not parse address " + address + " to IPAddress.");
				}
				return null;
			}
			return ipaddress.GetAddressBytes();
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00058D9C File Offset: 0x00056F9C
		protected EResult Send(HSteamNetConnection steamConnection, ArraySegment<byte> segment, byte channelId)
		{
			if (segment.Array.Length - 1 <= segment.Offset + segment.Count)
			{
				byte[] array = segment.Array;
				Array.Resize<byte>(ref array, array.Length + 1);
				array[array.Length - 1] = channelId;
			}
			else
			{
				segment.Array[segment.Offset + segment.Count] = channelId;
			}
			segment = new ArraySegment<byte>(segment.Array, segment.Offset, segment.Count + 1);
			GCHandle gchandle = GCHandle.Alloc(segment.Array, GCHandleType.Pinned);
			IntPtr pData = gchandle.AddrOfPinnedObject() + segment.Offset;
			int nSendFlags = (channelId == 1) ? 0 : 8;
			long num;
			EResult eresult = SteamNetworkingSockets.SendMessageToConnection(steamConnection, pData, (uint)segment.Count, nSendFlags, out num);
			if (eresult != EResult.k_EResultOK && this.Transport.NetworkManager.CanLog(LoggingType.Warning))
			{
				Debug.LogWarning(string.Format("Send issue: {0}", eresult));
			}
			gchandle.Free();
			return eresult;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00058E90 File Offset: 0x00057090
		internal void ClearQueue(ConcurrentQueue<LocalPacket> queue)
		{
			LocalPacket localPacket;
			while (queue.TryDequeue(out localPacket))
			{
				ByteArrayPool.Store(localPacket.Data);
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00058EB4 File Offset: 0x000570B4
		internal void ClearQueue(Queue<LocalPacket> queue)
		{
			while (queue.Count > 0)
			{
				ByteArrayPool.Store(queue.Dequeue().Data);
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00058ED4 File Offset: 0x000570D4
		protected void GetMessage(IntPtr ptr, byte[] buffer, out ArraySegment<byte> segment, out byte channel)
		{
			SteamNetworkingMessage_t steamNetworkingMessage_t = Marshal.PtrToStructure<SteamNetworkingMessage_t>(ptr);
			int cbSize = steamNetworkingMessage_t.m_cbSize;
			Marshal.Copy(steamNetworkingMessage_t.m_pData, buffer, 0, cbSize);
			SteamNetworkingMessage_t.Release(ptr);
			channel = buffer[cbSize - 1];
			segment = new ArraySegment<byte>(buffer, 0, cbSize - 1);
		}

		// Token: 0x040010EB RID: 4331
		private LocalConnectionState _connectionState;

		// Token: 0x040010EC RID: 4332
		protected bool PeerToPeer;

		// Token: 0x040010ED RID: 4333
		protected Transport Transport;

		// Token: 0x040010EE RID: 4334
		protected IntPtr[] MessagePointers = new IntPtr[256];

		// Token: 0x040010EF RID: 4335
		protected byte[] InboundBuffer;

		// Token: 0x040010F0 RID: 4336
		protected const int MAX_MESSAGES = 256;
	}
}
