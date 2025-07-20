using System;
using FishNet.Managing;
using FishNet.Transporting.Multipass;
using FishNet.Transporting.Tugboat;
using FishNet.Transporting.Yak;
using FishySteamworks;
using Steamworks;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class Lobby : Singleton<Lobby>
{
	// Token: 0x06000E58 RID: 3672 RVA: 0x00040628 File Offset: 0x0003E828
	public void Start()
	{
		this.LobbyCreated = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(this.OnLobbyCreated));
		this.JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.OnSteamLobbyJoinRequest));
		this.LobbyEntered = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(this.OnSteamLobbyEntered));
		Debug.Log("[LOBBY] Setup and listening for network events");
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x00040684 File Offset: 0x0003E884
	public void CreateSteamLobby(ELobbyType lobbyType, int maxPlayers)
	{
		SteamMatchmaking.CreateLobby(lobbyType, maxPlayers);
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x00040690 File Offset: 0x0003E890
	public void CreateLocalLobby(int maxPlayers)
	{
		Debug.Log("[LOBBY] Creating local lobby");
		this._multipassTransport.SetClientTransport<Tugboat>();
		this._multipassTransport.ClientTransport.SetClientAddress("localhost");
		this._multipassTransport.ClientTransport.SetMaximumClients(maxPlayers);
		this._networkManager.ServerManager.StartConnection();
		this._networkManager.ClientManager.StartConnection();
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x000406FA File Offset: 0x0003E8FA
	public void CreateOfflineLobby()
	{
		Debug.Log("[LOBBY] Creating offline lobby.");
		this._multipassTransport.SetClientTransport<Yak>();
		this._multipassTransport.ClientTransport.SetClientAddress("localhost");
		this._multipassTransport.ClientTransport.StartConnection(true);
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00040738 File Offset: 0x0003E938
	public void JoinLocalLobby()
	{
		this._multipassTransport.SetClientTransport<Tugboat>();
		this._multipassTransport.ClientTransport.SetClientAddress("localhost");
		this._multipassTransport.ClientTransport.StartConnection(false);
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x0004076C File Offset: 0x0003E96C
	public void JoinSteamLobby(CSteamID cSteamID)
	{
		if (Authenticator.BANNED_IDS.Contains(SteamUser.GetSteamID().m_SteamID))
		{
			return;
		}
		if (SteamMatchmaking.RequestLobbyData(cSteamID))
		{
			SteamMatchmaking.JoinLobby(cSteamID);
		}
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x00040794 File Offset: 0x0003E994
	private void OnLobbyCreated(LobbyCreated_t result)
	{
		if (result.m_eResult != EResult.k_EResultOK)
		{
			Debug.Log("[LOBBY] Could not create Steam lobby: " + result.m_eResult.ToString());
			return;
		}
		string text = SteamUser.GetSteamID().ToString();
		string str = SteamFriends.GetPersonaName().ToString();
		CSteamID steamIDLobby = new CSteamID(result.m_ulSteamIDLobby);
		SteamMatchmaking.SetLobbyData(steamIDLobby, "HostAddress", text);
		SteamMatchmaking.SetLobbyData(steamIDLobby, "LobbyName", str + "'s Lobby");
		SteamMatchmaking.SetLobbyData(steamIDLobby, "GamemodeID", Singleton<Gamemode>.Instance.GamemodeData.ID);
		this._multipassTransport.SetClientTransport<FishySteamworks>();
		this._multipassTransport.ClientTransport.SetClientAddress(text);
		this._multipassTransport.ClientTransport.StartConnection(true);
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00040862 File Offset: 0x0003EA62
	private void OnSteamLobbyJoinRequest(GameLobbyJoinRequested_t result)
	{
		if (Authenticator.BANNED_IDS.Contains(SteamUser.GetSteamID().m_SteamID))
		{
			return;
		}
		SteamMatchmaking.JoinLobby(result.m_steamIDLobby);
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x00040888 File Offset: 0x0003EA88
	private void OnSteamLobbyEntered(LobbyEnter_t result)
	{
		if (Authenticator.BANNED_IDS.Contains(SteamUser.GetSteamID().m_SteamID))
		{
			return;
		}
		CSteamID steamIDLobby = new CSteamID(result.m_ulSteamIDLobby);
		string lobbyData = SteamMatchmaking.GetLobbyData(steamIDLobby, "HostAddress");
		try
		{
			string lobbyData2 = SteamMatchmaking.GetLobbyData(steamIDLobby, "GamemodeID");
			GamemodeData gamemodeData = Library.RequestData<GamemodeData>(lobbyData2);
			if (gamemodeData != null)
			{
				Debug.Log("[LOBBY] Synced with lobby gamemode " + Singleton<Gamemode>.Instance.GamemodeData.ID);
				Singleton<Gamemode>.Instance.GamemodeData = gamemodeData;
			}
			else
			{
				Debug.Log("[LOBBY] The retrieved gamemode ID " + lobbyData2 + " is invalid!");
			}
		}
		catch
		{
			Debug.Log("[LOBBY] Could not extract gamemode data from Steam lobby.");
		}
		this._multipassTransport.SetClientTransport<FishySteamworks>();
		this._multipassTransport.ClientTransport.SetClientAddress(lobbyData);
		this._multipassTransport.ClientTransport.StartConnection(false);
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x00040970 File Offset: 0x0003EB70
	public void LeaveLobby()
	{
		if (this._networkManager.IsServer)
		{
			this._networkManager.ServerManager.StopConnection(true);
			return;
		}
		this._networkManager.ClientManager.StopConnection();
	}

	// Token: 0x04000B35 RID: 2869
	public const string HOST_ADDRESS = "HostAddress";

	// Token: 0x04000B36 RID: 2870
	public const string LOBBY_NAME = "LobbyName";

	// Token: 0x04000B37 RID: 2871
	public const string GAMEMODE_ID = "GamemodeID";

	// Token: 0x04000B38 RID: 2872
	[SerializeField]
	private NetworkManager _networkManager;

	// Token: 0x04000B39 RID: 2873
	[SerializeField]
	private Multipass _multipassTransport;

	// Token: 0x04000B3A RID: 2874
	protected Callback<LobbyCreated_t> LobbyCreated;

	// Token: 0x04000B3B RID: 2875
	protected Callback<GameLobbyJoinRequested_t> JoinRequest;

	// Token: 0x04000B3C RID: 2876
	protected Callback<LobbyEnter_t> LobbyEntered;
}
