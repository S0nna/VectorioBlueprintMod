using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013D RID: 317
[CreateAssetMenu(fileName = "New Gamemode", menuName = "Vectorio/Gamemode")]
public class GamemodeData : BaseData
{
	// Token: 0x06000AA2 RID: 2722 RVA: 0x0002CA08 File Offset: 0x0002AC08
	public int GetTotalTechs()
	{
		int num = 0;
		foreach (ResearchTreeData researchTreeData in this.researchTrees)
		{
			num += researchTreeData.nodes.Count;
		}
		return num;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0002CA68 File Offset: 0x0002AC68
	public bool HasRule(GamemodeRules ruleToCheck)
	{
		return (this.gamemodeRules & ruleToCheck) == ruleToCheck;
	}

	// Token: 0x0400068F RID: 1679
	public RegionData defaultRegion;

	// Token: 0x04000690 RID: 1680
	public List<ResearchTreeData> researchTrees;

	// Token: 0x04000691 RID: 1681
	public GamemodeRules gamemodeRules;

	// Token: 0x04000692 RID: 1682
	public GamemodeOptions defaultOptions;
}
