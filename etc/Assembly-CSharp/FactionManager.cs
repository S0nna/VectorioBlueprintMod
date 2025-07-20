using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B9 RID: 441
[DefaultExecutionOrder(0)]
public class FactionManager : Singleton<FactionManager>
{
	// Token: 0x06000E15 RID: 3605 RVA: 0x0003EE10 File Offset: 0x0003D010
	public void AddHub(string faction, Hub hub)
	{
		if (!this._factionHubs.ContainsKey(faction))
		{
			this._factionHubs.Add(faction, new List<Hub>());
		}
		if (this._factionHubs[faction].Contains(hub))
		{
			Debug.Log("[FACTION MANAGER] This hub is already registered to this faction!");
			return;
		}
		this._factionHubs[faction].Add(hub);
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x0003EE70 File Offset: 0x0003D070
	public void RemoveHub(string faction, Hub hub)
	{
		if (!this._factionHubs.ContainsKey(faction))
		{
			Debug.Log("[FACTION MANAGER] Could not remove hub because the faction does not exist!");
			return;
		}
		if (!this._factionHubs[faction].Contains(hub))
		{
			Debug.Log("[FACTION MANAGER] Could not remove hub because it is not registered to this faction!");
			return;
		}
		this._factionHubs[faction].Remove(hub);
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x0003EEC8 File Offset: 0x0003D0C8
	public List<Hub> GetHubs(string faction)
	{
		if (!this._factionHubs.ContainsKey(faction))
		{
			return new List<Hub>();
		}
		return this._factionHubs[faction];
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0003EEEA File Offset: 0x0003D0EA
	public string PlayerFactionID
	{
		get
		{
			return this._playerFactionID;
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0003EEF2 File Offset: 0x0003D0F2
	public void SetPlayerFaction(string id)
	{
		this._playerFactionID = id;
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x0003EEFB File Offset: 0x0003D0FB
	public bool IsPlayerFaction(string id)
	{
		return this._playerFactionID == id;
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x0003EF0C File Offset: 0x0003D10C
	public Accent GetFactionAccent(string id)
	{
		if (!this._factions.ContainsKey(id))
		{
			FactionData factionData = Library.RequestData<FactionData>(id);
			if (factionData != null)
			{
				this._factions.Add(factionData.ID, factionData);
			}
		}
		return this._factions[id].accent;
	}

	// Token: 0x04000ADC RID: 2780
	private Dictionary<string, FactionData> _factions = new Dictionary<string, FactionData>();

	// Token: 0x04000ADD RID: 2781
	private string _playerFactionID;

	// Token: 0x04000ADE RID: 2782
	private Dictionary<string, List<Hub>> _factionHubs = new Dictionary<string, List<Hub>>();
}
