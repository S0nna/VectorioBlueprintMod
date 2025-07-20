using System;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

namespace Vectorio.Networking
{
	// Token: 0x02000278 RID: 632
	public class LobbyButton : Vectorio.PhasmaUI.Button
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x00052769 File Offset: 0x00050969
		public FriendStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00052774 File Offset: 0x00050974
		public override void OnClick()
		{
			base.OnClick();
			if (this.isTestButton)
			{
				CSteamID cSteamID = new CSteamID(76561198086332058UL);
				Singleton<Lobby>.Instance.JoinSteamLobby(cSteamID);
				return;
			}
			if (this.isLocalButton)
			{
				Singleton<Lobby>.Instance.JoinLocalLobby();
				return;
			}
			if (this._canJoin)
			{
				CSteamID cSteamID2 = new CSteamID(this._lobbyID.m_SteamID);
				Singleton<Lobby>.Instance.JoinSteamLobby(cSteamID2);
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x000527E4 File Offset: 0x000509E4
		public void SetFriendLobby(CSteamID cSteamID)
		{
			this.title.text = SteamFriends.GetFriendPersonaName(cSteamID);
			this.playerIcon.sprite = NetUtilities.GetSteamImageAsSprite(SteamFriends.GetMediumFriendAvatar(cSteamID));
			this._canJoin = false;
			FriendGameInfo_t friendGameInfo_t;
			if (SteamFriends.GetFriendGamePlayed(cSteamID, out friendGameInfo_t))
			{
				if (friendGameInfo_t.m_gameID.AppID().m_AppId != 2082350U)
				{
					this.SetStatus(FriendStatus.InOtherGame, "<b>ADVENTURE</b> (The Abyss)", 1, 4);
					return;
				}
				if (!SteamMatchmaking.RequestLobbyData(friendGameInfo_t.m_steamIDLobby))
				{
					this.SetStatus(FriendStatus.InMenu, "<b>ADVENTURE</b> (The Abyss)", 1, 4);
					this._canJoin = false;
					return;
				}
				this._lobbyID = friendGameInfo_t.m_steamIDLobby;
				if (this._lobbyID.m_SteamID != 0UL)
				{
					Debug.Log(this._lobbyID.m_SteamID);
					this.SetStatus(FriendStatus.InGame, "<b>ADVENTURE</b> (The Abyss)", 1, 4);
					this._canJoin = true;
					return;
				}
				this.SetStatus(FriendStatus.InMenu, "<b>ADVENTURE</b> (The Abyss)", 1, 4);
				this._canJoin = false;
				return;
			}
			else
			{
				if (SteamFriends.GetFriendPersonaState(cSteamID) == EPersonaState.k_EPersonaStateOffline || SteamFriends.GetFriendPersonaState(cSteamID) == EPersonaState.k_EPersonaStateInvisible)
				{
					this.SetStatus(FriendStatus.Offline, "<b>ADVENTURE</b> (The Abyss)", 1, 4);
					return;
				}
				this.SetStatus(FriendStatus.Online, "<b>ADVENTURE</b> (The Abyss)", 1, 4);
				return;
			}
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00052900 File Offset: 0x00050B00
		public void SetStatus(FriendStatus status, string gamemodeText = "<b>ADVENTURE</b> (The Abyss)", int currentPlayers = 1, int maxPlayers = 4)
		{
			this._status = status;
			switch (status)
			{
			case FriendStatus.InGame:
				this.gamemode.text = gamemodeText;
				this.count.text = currentPlayers.ToString() + "/" + maxPlayers.ToString();
				this.count.gameObject.SetActive(true);
				this.countIcon.gameObject.SetActive(true);
				this.gamemode.color = this.playingLightColor;
				this.background.color = this.playingDarkColor;
				return;
			case FriendStatus.InMenu:
				this.gamemode.text = "IN MENU";
				this.count.gameObject.SetActive(false);
				this.countIcon.gameObject.SetActive(false);
				this.gamemode.color = this.menuLightColor;
				this.background.color = this.menuDarkColor;
				return;
			case FriendStatus.InOtherGame:
				this.gamemode.text = "IN OTHER GAME";
				this.count.gameObject.SetActive(false);
				this.countIcon.gameObject.SetActive(false);
				this.gamemode.color = this.onlineLightColor;
				this.background.color = this.onlineDarkColor;
				return;
			case FriendStatus.Online:
				this.gamemode.text = "ONLINE";
				this.count.gameObject.SetActive(false);
				this.countIcon.gameObject.SetActive(false);
				this.gamemode.color = this.onlineLightColor;
				this.background.color = this.onlineDarkColor;
				return;
			case FriendStatus.Offline:
				this.gamemode.text = "OFFLINE";
				this.count.gameObject.SetActive(false);
				this.countIcon.gameObject.SetActive(false);
				this.gamemode.color = this.offlineLightColor;
				this.background.color = this.offlineDarkColor;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000F8C RID: 3980
		private FriendStatus _status = FriendStatus.Offline;

		// Token: 0x04000F8D RID: 3981
		public bool isLocalButton;

		// Token: 0x04000F8E RID: 3982
		public bool isTestButton;

		// Token: 0x04000F8F RID: 3983
		public TextMeshProUGUI title;

		// Token: 0x04000F90 RID: 3984
		public TextMeshProUGUI gamemode;

		// Token: 0x04000F91 RID: 3985
		public TextMeshProUGUI count;

		// Token: 0x04000F92 RID: 3986
		public Image background;

		// Token: 0x04000F93 RID: 3987
		public Image playerIcon;

		// Token: 0x04000F94 RID: 3988
		public Image countIcon;

		// Token: 0x04000F95 RID: 3989
		public Sprite onlineBorder;

		// Token: 0x04000F96 RID: 3990
		public Sprite offlineBorder;

		// Token: 0x04000F97 RID: 3991
		public Color playingLightColor;

		// Token: 0x04000F98 RID: 3992
		public Color playingDarkColor;

		// Token: 0x04000F99 RID: 3993
		public Color menuLightColor;

		// Token: 0x04000F9A RID: 3994
		public Color menuDarkColor;

		// Token: 0x04000F9B RID: 3995
		public Color onlineLightColor;

		// Token: 0x04000F9C RID: 3996
		public Color onlineDarkColor;

		// Token: 0x04000F9D RID: 3997
		public Color offlineLightColor;

		// Token: 0x04000F9E RID: 3998
		public Color offlineDarkColor;

		// Token: 0x04000F9F RID: 3999
		private bool _canJoin;

		// Token: 0x04000FA0 RID: 4000
		private CSteamID _lobbyID;
	}
}
