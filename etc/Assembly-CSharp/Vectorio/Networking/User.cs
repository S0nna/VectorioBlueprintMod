using System;
using Steamworks;

namespace Vectorio.Networking
{
	// Token: 0x02000276 RID: 630
	public class User
	{
		// Token: 0x06001214 RID: 4628 RVA: 0x00052755 File Offset: 0x00050955
		public User(CSteamID userID)
		{
			this._name = SteamFriends.GetFriendPersonaName(userID);
		}

		// Token: 0x04000F82 RID: 3970
		private string _name;

		// Token: 0x04000F83 RID: 3971
		private string _description;

		// Token: 0x04000F84 RID: 3972
		private User.Status _status;

		// Token: 0x04000F85 RID: 3973
		private CSteamID _lobby;

		// Token: 0x02000277 RID: 631
		public enum Status
		{
			// Token: 0x04000F87 RID: 3975
			InVectorioGame,
			// Token: 0x04000F88 RID: 3976
			InVectorioMenu,
			// Token: 0x04000F89 RID: 3977
			PlayingOtherGame,
			// Token: 0x04000F8A RID: 3978
			Online,
			// Token: 0x04000F8B RID: 3979
			Offline
		}
	}
}
