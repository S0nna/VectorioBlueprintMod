using System;
using System.Collections.Generic;
using System.Linq;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class Friendlist : MonoBehaviour
{
	// Token: 0x06000BC5 RID: 3013 RVA: 0x00033918 File Offset: 0x00031B18
	public void UpdateFriendsList()
	{
		if (!this.generated)
		{
			this.friendList = this.SortFriendsList(Friends.Client.GetFriends(EFriendFlags.k_EFriendFlagAll).ToList<UserData>());
			foreach (UserData userData in this.friendList)
			{
			}
			this.generated = true;
		}
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00033990 File Offset: 0x00031B90
	private List<UserData> SortFriendsList(List<UserData> friends)
	{
		List<UserData> list = new List<UserData>();
		List<UserData> list2 = (from friend in friends
		orderby friend.Name
		select friend).ToList<UserData>();
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i].GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
			{
				list.Add(list2[i]);
				list2.Remove(list2[i]);
				i--;
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			if (list2[j].State.HasFlag(EPersonaState.k_EPersonaStateOnline))
			{
				list.Add(list2[j]);
				list2.Remove(list2[j]);
				j--;
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			if (list2[k].State.HasFlag(EPersonaState.k_EPersonaStateAway) || list2[k].State.HasFlag(EPersonaState.k_EPersonaStateSnooze) || list2[k].State.HasFlag(EPersonaState.k_EPersonaStateBusy))
			{
				list.Add(list2[k]);
				list2.Remove(list2[k]);
				k--;
			}
		}
		list.AddRange(list2);
		return list;
	}

	// Token: 0x040007FA RID: 2042
	private List<UserData> friendList = new List<UserData>();

	// Token: 0x040007FB RID: 2043
	public Transform joinList;

	// Token: 0x040007FC RID: 2044
	public bool generated;
}
