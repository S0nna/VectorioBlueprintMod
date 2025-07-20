using System;

// Token: 0x02000218 RID: 536
public class Layers
{
	// Token: 0x06000FD3 RID: 4051 RVA: 0x0004A95B File Offset: 0x00048B5B
	public static string BUILDING_LAYER(string factionID, bool opposite)
	{
		if (!opposite)
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ENEMY_BUILDING_LAYER;
			}
			return Layers.ALLY_BUILDING_LAYER;
		}
		else
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ALLY_BUILDING_LAYER;
			}
			return Layers.ENEMY_BUILDING_LAYER;
		}
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x0004A991 File Offset: 0x00048B91
	public static string BULLET_LAYER(string factionID, bool opposite)
	{
		if (!opposite)
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ENEMY_BULLET_LAYER;
			}
			return Layers.ALLY_BULLET_LAYER;
		}
		else
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ALLY_BULLET_LAYER;
			}
			return Layers.ENEMY_BULLET_LAYER;
		}
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x0004A9C7 File Offset: 0x00048BC7
	public static string BULLET_DETECTOR_LAYER(string factionID, bool opposite)
	{
		if (!opposite)
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ENEMY_BULLET_DETECTOR;
			}
			return Layers.ALLY_BULLET_DETECTOR;
		}
		else
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ALLY_BULLET_DETECTOR;
			}
			return Layers.ENEMY_BULLET_DETECTOR;
		}
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x0004A9FD File Offset: 0x00048BFD
	public static string UNIT_LAYER(string factionID, bool opposite)
	{
		if (!opposite)
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ENEMY_UNIT_LAYER;
			}
			return Layers.ALLY_UNIT_LAYER;
		}
		else
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ALLY_UNIT_LAYER;
			}
			return Layers.ENEMY_UNIT_LAYER;
		}
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x0004AA33 File Offset: 0x00048C33
	public static string TURRET_DETECTOR_LAYER(string factionID, bool opposite)
	{
		if (!opposite)
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ENEMY_TURRET_DETECTOR;
			}
			return Layers.ALLY_TURRET_DETECTOR;
		}
		else
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ALLY_TURRET_DETECTOR;
			}
			return Layers.ENEMY_TURRET_DETECTOR;
		}
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x0004AA69 File Offset: 0x00048C69
	public static string UNIT_DETECTOR_LAYER(string factionID, bool opposite)
	{
		if (!opposite)
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ENEMY_UNIT_DETECTOR;
			}
			return Layers.ALLY_UNIT_DETECTOR;
		}
		else
		{
			if (!Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				return Layers.ALLY_UNIT_DETECTOR;
			}
			return Layers.ENEMY_UNIT_DETECTOR;
		}
	}

	// Token: 0x04000DE5 RID: 3557
	public static string ALLY_BUILDING_LAYER = "ally building";

	// Token: 0x04000DE6 RID: 3558
	public static string ENEMY_BUILDING_LAYER = "enemy building";

	// Token: 0x04000DE7 RID: 3559
	public static string ALLY_BULLET_LAYER = "ally bullet";

	// Token: 0x04000DE8 RID: 3560
	public static string ENEMY_BULLET_LAYER = "enemy bullet";

	// Token: 0x04000DE9 RID: 3561
	public static string ALLY_UNIT_LAYER = "ally unit";

	// Token: 0x04000DEA RID: 3562
	public static string ENEMY_UNIT_LAYER = "enemy unit";

	// Token: 0x04000DEB RID: 3563
	public static string ALLY_TURRET_DETECTOR = "ally turret detector";

	// Token: 0x04000DEC RID: 3564
	public static string ENEMY_TURRET_DETECTOR = "enemy turret detector";

	// Token: 0x04000DED RID: 3565
	public static string ALLY_UNIT_DETECTOR = "ally unit detector";

	// Token: 0x04000DEE RID: 3566
	public static string ENEMY_UNIT_DETECTOR = "enemy unit detector";

	// Token: 0x04000DEF RID: 3567
	public static string ALLY_BULLET_DETECTOR = "ally bullet detector";

	// Token: 0x04000DF0 RID: 3568
	public static string ENEMY_BULLET_DETECTOR = "enemy bullet detector";

	// Token: 0x04000DF1 RID: 3569
	public static string SORTING_BUILDING_LAYER = "building";

	// Token: 0x04000DF2 RID: 3570
	public static string SORTING_BULLET_LAYER = "bullet";

	// Token: 0x04000DF3 RID: 3571
	public static string SORTING_UNIT_LAYER = "unit";

	// Token: 0x04000DF4 RID: 3572
	public static string SORTING_HOLOGRAM_LAYER = "hologram";

	// Token: 0x04000DF5 RID: 3573
	public static string SORTING_HIT_DETECTOR = "hit detector";

	// Token: 0x04000DF6 RID: 3574
	public static string SORTING_MAP_UNIT = "map unit";

	// Token: 0x04000DF7 RID: 3575
	public static string HOLOGRAM_LAYER = "hologram";

	// Token: 0x04000DF8 RID: 3576
	public static string DRONE_LAYER = "drone";

	// Token: 0x04000DF9 RID: 3577
	public static string OUTPOST_LAYER = "outpost";

	// Token: 0x04000DFA RID: 3578
	public static string PARTICLE_LAYER = "particles";
}
