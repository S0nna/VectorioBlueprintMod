using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000082 RID: 130
[Serializable]
public class PlayerData : SerializableData
{
	// Token: 0x060005D4 RID: 1492 RVA: 0x0001EF8C File Offset: 0x0001D18C
	public PlayerData()
	{
		Debug.Log("[SAVE] Creating new player data");
	}

	// Token: 0x0400034F RID: 847
	public List<string> unlockedCosmetics = new List<string>();

	// Token: 0x04000350 RID: 848
	public List<string> discoveredEntities = new List<string>();
}
