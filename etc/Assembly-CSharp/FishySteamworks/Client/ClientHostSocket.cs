using System;
using System.Collections.Generic;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using FishySteamworks.Server;

namespace FishySteamworks.Client
{
	// Token: 0x020002BA RID: 698
	public class ClientHostSocket : CommonSocket
	{
		// Token: 0x060013B7 RID: 5047 RVA: 0x00059F8F File Offset: 0x0005818F
		internal void CheckSetStarted()
		{
			if (this._server != null && base.GetLocalConnectionState() == LocalConnectionState.Starting && this._server.GetLocalConnectionState() == LocalConnectionState.Started)
			{
				this.SetLocalConnectionState(LocalConnectionState.Started, false);
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00059FB8 File Offset: 0x000581B8
		internal bool StartConnection(ServerSocket serverSocket)
		{
			this._server = serverSocket;
			this._server.SetClientHostSocket(this);
			if (this._server.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return false;
			}
			this.SetLocalConnectionState(LocalConnectionState.Starting, false);
			return true;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00059FE6 File Offset: 0x000581E6
		protected override void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			base.SetLocalConnectionState(connectionState, server);
			if (connectionState == LocalConnectionState.Started)
			{
				this._server.OnClientHostState(true);
				return;
			}
			this._server.OnClientHostState(false);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0005A00D File Offset: 0x0005820D
		internal bool StopConnection()
		{
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped || base.GetLocalConnectionState() == LocalConnectionState.Stopping)
			{
				return false;
			}
			base.ClearQueue(this._incoming);
			this.SetLocalConnectionState(LocalConnectionState.Stopping, false);
			this.SetLocalConnectionState(LocalConnectionState.Stopped, false);
			this._server.SetClientHostSocket(null);
			return true;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0005A04C File Offset: 0x0005824C
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			while (this._incoming.Count > 0)
			{
				LocalPacket localPacket = this._incoming.Dequeue();
				ArraySegment<byte> data = new ArraySegment<byte>(localPacket.Data, 0, localPacket.Length);
				this.Transport.HandleClientReceivedDataArgs(new ClientReceivedDataArgs(data, (Channel)localPacket.Channel, this.Transport.Index));
				ByteArrayPool.Store(localPacket.Data);
			}
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0005A0BE File Offset: 0x000582BE
		internal void ReceivedFromLocalServer(LocalPacket packet)
		{
			this._incoming.Enqueue(packet);
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0005A0CC File Offset: 0x000582CC
		internal void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			if (this._server.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			LocalPacket packet = new LocalPacket(segment, channelId);
			this._server.ReceivedFromClientHost(packet);
		}

		// Token: 0x0400110F RID: 4367
		private ServerSocket _server;

		// Token: 0x04001110 RID: 4368
		private Queue<LocalPacket> _incoming = new Queue<LocalPacket>();
	}
}
