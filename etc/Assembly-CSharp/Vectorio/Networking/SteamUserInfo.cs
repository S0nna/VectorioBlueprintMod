using System;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vectorio.Networking
{
	// Token: 0x02000279 RID: 633
	[DefaultExecutionOrder(10)]
	public class SteamUserInfo : MonoBehaviour
	{
		// Token: 0x0600121A RID: 4634 RVA: 0x00052B08 File Offset: 0x00050D08
		public void Start()
		{
			if (!SteamAPI.IsSteamRunning())
			{
				Debug.Log("[STEAM] Cannot load user info because Steam is not initialized!");
				base.gameObject.SetActive(false);
				return;
			}
			this.playerName.text = SteamFriends.GetPersonaName();
			this.connection.text = "<b>Steam:</b> <color=#8BFF81>Connected";
			this.playerIcon.sprite = NetUtilities.GetSteamImageAsSprite(SteamFriends.GetLargeFriendAvatar(SteamUser.GetSteamID()));
		}

		// Token: 0x04000FA1 RID: 4001
		public Image bar;

		// Token: 0x04000FA2 RID: 4002
		public Image background;

		// Token: 0x04000FA3 RID: 4003
		public Image playerIcon;

		// Token: 0x04000FA4 RID: 4004
		public Image platformIcon;

		// Token: 0x04000FA5 RID: 4005
		public TextMeshProUGUI playerName;

		// Token: 0x04000FA6 RID: 4006
		public TextMeshProUGUI connection;
	}
}
