using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using Vectorio.Networking;

// Token: 0x020001C7 RID: 455
public class LobbyList : MonoBehaviour
{
	// Token: 0x06000E63 RID: 3683 RVA: 0x000409AB File Offset: 0x0003EBAB
	public void Start()
	{
		this.LobbyListReceived = Callback<LobbyMatchList_t>.Create(new Callback<LobbyMatchList_t>.DispatchDelegate(this.OnLobbyListReceived));
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x000409C4 File Offset: 0x0003EBC4
	public void GenerateLobbyList()
	{
		SteamMatchmaking.RequestLobbyList();
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x000409CC File Offset: 0x0003EBCC
	public void GenerateFriendList()
	{
		this.GenerateFriendList(EFriendFlags.k_EFriendFlagImmediate);
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x000409D8 File Offset: 0x0003EBD8
	public void GenerateFriendList(EFriendFlags filter)
	{
		int friendCount = SteamFriends.GetFriendCount(filter);
		Debug.Log("[LOBBY] Found " + friendCount.ToString() + " friends, creating list...");
		if (this._lobbies != null && this._lobbies.Count > 0)
		{
			foreach (KeyValuePair<FriendStatus, List<LobbyButton>> keyValuePair in this._lobbies)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					Object.Destroy(keyValuePair.Value[i].gameObject);
				}
			}
			this._lobbies = new Dictionary<FriendStatus, List<LobbyButton>>();
		}
		this._lobbies = new Dictionary<FriendStatus, List<LobbyButton>>
		{
			{
				FriendStatus.InGame,
				new List<LobbyButton>()
			},
			{
				FriendStatus.InMenu,
				new List<LobbyButton>()
			},
			{
				FriendStatus.InOtherGame,
				new List<LobbyButton>()
			},
			{
				FriendStatus.Online,
				new List<LobbyButton>()
			},
			{
				FriendStatus.Offline,
				new List<LobbyButton>()
			}
		};
		if (friendCount == 0)
		{
			this.lobbiesObj.SetActive(false);
			this.noLobbiesObj.SetActive(true);
			return;
		}
		this.noLobbiesObj.SetActive(false);
		this.lobbiesObj.SetActive(true);
		for (int j = 0; j < friendCount; j++)
		{
			LobbyButton lobbyButton = Object.Instantiate<LobbyButton>(this.lobbyButtonPrefab);
			lobbyButton.transform.SetParent(this.lobbyListParent);
			lobbyButton.transform.localScale = Vector3.one;
			lobbyButton.SetFriendLobby(SteamFriends.GetFriendByIndex(j, filter));
			this._lobbies[lobbyButton.Status].Add(lobbyButton);
		}
		int num = 0;
		foreach (KeyValuePair<FriendStatus, List<LobbyButton>> keyValuePair2 in this._lobbies)
		{
			for (int k = 0; k < keyValuePair2.Value.Count; k++)
			{
				keyValuePair2.Value[k].transform.SetSiblingIndex(k + num);
				num++;
			}
			num += 1000;
		}
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x00003212 File Offset: 0x00001412
	private void OnLobbyListReceived(LobbyMatchList_t result)
	{
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x00003212 File Offset: 0x00001412
	public void ClearList()
	{
	}

	// Token: 0x04000B3D RID: 2877
	public LobbyButton lobbyButtonPrefab;

	// Token: 0x04000B3E RID: 2878
	public GameObject noLobbiesObj;

	// Token: 0x04000B3F RID: 2879
	public GameObject lobbiesObj;

	// Token: 0x04000B40 RID: 2880
	public Transform lobbyListParent;

	// Token: 0x04000B41 RID: 2881
	private Dictionary<FriendStatus, List<LobbyButton>> _lobbies;

	// Token: 0x04000B42 RID: 2882
	protected Callback<LobbyMatchList_t> LobbyListReceived;
}
