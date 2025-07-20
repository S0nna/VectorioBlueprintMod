using System;
using UnityEngine;

// Token: 0x020001B1 RID: 433
[DefaultExecutionOrder(-1)]
public class DevTools : MonoBehaviour
{
	// Token: 0x06000DF5 RID: 3573 RVA: 0x0003E162 File Offset: 0x0003C362
	public void SetAbyss()
	{
		this.gamemodeID = "gamemode_adventure";
		this.regionID = "region_the_abyss";
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x0003E17A File Offset: 0x0003C37A
	public void SetPlains()
	{
		this.gamemodeID = "gamemode_adventure";
		this.regionID = "region_phantom_plains";
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x0003E192 File Offset: 0x0003C392
	public void SetCreative()
	{
		this.gamemodeID = "gamemode_creative";
		this.regionID = "region_creative";
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x0003E1AA File Offset: 0x0003C3AA
	public void Awake()
	{
		DevTools.ADVANCED_DEBUG = this.advancedDebug;
	}

	// Token: 0x04000A72 RID: 2674
	public static bool ADVANCED_DEBUG = false;

	// Token: 0x04000A73 RID: 2675
	public static bool UNLOCK_ALL_TECHS = false;

	// Token: 0x04000A74 RID: 2676
	public static bool SKIP_RESOURCE_CHECKS = false;

	// Token: 0x04000A75 RID: 2677
	public static bool INSTANT_CRAFT = false;

	// Token: 0x04000A76 RID: 2678
	public static bool INSTANT_FILL = false;

	// Token: 0x04000A77 RID: 2679
	public static bool INSTANT_BUILD = false;

	// Token: 0x04000A78 RID: 2680
	public static bool FORCE_LOCAL_CONNECTION = false;

	// Token: 0x04000A79 RID: 2681
	public static bool ENEMY_TEST = false;

	// Token: 0x04000A7A RID: 2682
	public static int HEAT_LEVEL;

	// Token: 0x04000A7B RID: 2683
	public static float HEAT_SPAWN_OFFSET;

	// Token: 0x04000A7C RID: 2684
	public static bool QUICK_START = false;

	// Token: 0x04000A7D RID: 2685
	public static bool ALLOW_ENEMY_EDITING = false;

	// Token: 0x04000A7E RID: 2686
	public static bool USE_RESEARCH_TESTING = false;

	// Token: 0x04000A7F RID: 2687
	public static ResearchTechData START_RESEARCH_TECH = null;

	// Token: 0x04000A80 RID: 2688
	public static string GAMEMODE_ID = "gamemode_adventure";

	// Token: 0x04000A81 RID: 2689
	public static string REGION_ID = "region_default";

	// Token: 0x04000A82 RID: 2690
	public bool advancedDebug;

	// Token: 0x04000A83 RID: 2691
	public bool unlockAllTechs;

	// Token: 0x04000A84 RID: 2692
	public bool skipResourceChecks;

	// Token: 0x04000A85 RID: 2693
	public bool instantCraft;

	// Token: 0x04000A86 RID: 2694
	public bool instantFill;

	// Token: 0x04000A87 RID: 2695
	public bool instantBuild;

	// Token: 0x04000A88 RID: 2696
	public bool allowEnemyEditing;

	// Token: 0x04000A89 RID: 2697
	public bool useResearchTesting;

	// Token: 0x04000A8A RID: 2698
	public ResearchTechData researchTech;

	// Token: 0x04000A8B RID: 2699
	public bool forceLocalConnection;

	// Token: 0x04000A8C RID: 2700
	public bool enemyTest;

	// Token: 0x04000A8D RID: 2701
	public int heatLevel = 1;

	// Token: 0x04000A8E RID: 2702
	public float heatSpawnOffset = 50f;

	// Token: 0x04000A8F RID: 2703
	public bool quickStart;

	// Token: 0x04000A90 RID: 2704
	public string gamemodeID = "gamemode_adventure";

	// Token: 0x04000A91 RID: 2705
	public string regionID = "region_default";
}
