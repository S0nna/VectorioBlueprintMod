using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace FishNet.Discovery
{
	// Token: 0x020002B0 RID: 688
	public sealed class NetworkDiscoveryHud : MonoBehaviour
	{
		// Token: 0x06001337 RID: 4919 RVA: 0x0005825A File Offset: 0x0005645A
		private void Start()
		{
			if (this.networkDiscovery == null)
			{
				this.networkDiscovery = Object.FindObjectOfType<NetworkDiscovery>();
			}
			this.networkDiscovery.ServerFoundCallback += delegate(IPEndPoint endPoint)
			{
				this._addresses.Add(endPoint.Address.ToString());
			};
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0005828C File Offset: 0x0005648C
		private void OnGUI()
		{
			GUILayoutOption guilayoutOption = GUILayout.Height(30f);
			GUILayout.BeginArea(new Rect((float)Screen.width - 240f - 10f, 10f, 240f, (float)Screen.height - 20f));
			GUILayout.Box("Server", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Start", new GUILayoutOption[]
			{
				guilayoutOption
			}))
			{
				InstanceFinder.ServerManager.StartConnection();
			}
			if (GUILayout.Button("Stop", new GUILayoutOption[]
			{
				guilayoutOption
			}))
			{
				InstanceFinder.ServerManager.StopConnection(true);
			}
			GUILayout.EndHorizontal();
			GUILayout.Box("Advertising", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (this.networkDiscovery.IsAdvertising)
			{
				if (GUILayout.Button("Stop", new GUILayoutOption[]
				{
					guilayoutOption
				}))
				{
					this.networkDiscovery.StopSearchingOrAdvertising();
				}
			}
			else if (GUILayout.Button("Start", new GUILayoutOption[]
			{
				guilayoutOption
			}))
			{
				this.networkDiscovery.AdvertiseServer();
			}
			GUILayout.EndHorizontal();
			GUILayout.Box("Searching", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (this.networkDiscovery.IsSearching)
			{
				if (GUILayout.Button("Stop", new GUILayoutOption[]
				{
					guilayoutOption
				}))
				{
					this.networkDiscovery.StopSearchingOrAdvertising();
				}
			}
			else if (GUILayout.Button("Start", new GUILayoutOption[]
			{
				guilayoutOption
			}))
			{
				this.networkDiscovery.SearchForServers();
			}
			GUILayout.EndHorizontal();
			if (this._addresses.Count < 1)
			{
				GUILayout.EndArea();
				return;
			}
			GUILayout.Box("Servers", Array.Empty<GUILayoutOption>());
			this._serversListScrollVector = GUILayout.BeginScrollView(this._serversListScrollVector, Array.Empty<GUILayoutOption>());
			foreach (string text in this._addresses)
			{
				if (GUILayout.Button(text, Array.Empty<GUILayoutOption>()))
				{
					this.networkDiscovery.StopSearchingOrAdvertising();
					InstanceFinder.ClientManager.StartConnection(text);
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		// Token: 0x040010DA RID: 4314
		[SerializeField]
		private NetworkDiscovery networkDiscovery;

		// Token: 0x040010DB RID: 4315
		private readonly HashSet<string> _addresses = new HashSet<string>();

		// Token: 0x040010DC RID: 4316
		private Vector2 _serversListScrollVector;
	}
}
