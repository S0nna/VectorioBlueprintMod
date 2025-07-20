using System;
using System.Collections.Generic;

namespace Vectorio.Stats
{
	// Token: 0x02000265 RID: 613
	public class StatManager : Singleton<StatManager>
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x00052143 File Offset: 0x00050343
		public Dictionary<int, StatModifier> AllyModifiers
		{
			get
			{
				return this._allyModifiers;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0005214B File Offset: 0x0005034B
		public Dictionary<int, StatModifier> EnemyModifiers
		{
			get
			{
				return this._enemyModifiers;
			}
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00052153 File Offset: 0x00050353
		public void CreateStatFloat(ref StatFloat stat, StatType type, float value, EntityComponent component = null)
		{
			this.CreateStatFloat(ref stat, (int)type, value, component);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00052160 File Offset: 0x00050360
		public void CreateStatInt(ref StatInt stat, StatType type, int value, EntityComponent component = null)
		{
			this.CreateStatInt(ref stat, (int)type, value, component);
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00052170 File Offset: 0x00050370
		public void CreateStatFloat(ref StatFloat stat, int code, float value, EntityComponent component = null)
		{
			if (stat == null)
			{
				string factionID = ((component != null) ? component.FactionID : null) ?? "";
				stat = new StatFloat(value, code, factionID);
				this.RegisterStat(stat, ((component != null) ? component.FactionID : null) ?? "");
				if (component != null)
				{
					component.Entity.RegisterStat(stat);
				}
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x000521DC File Offset: 0x000503DC
		public void CreateStatInt(ref StatInt stat, int code, int value, EntityComponent component = null)
		{
			if (stat == null)
			{
				string factionID = ((component != null) ? component.FactionID : null) ?? "";
				stat = new StatInt(value, code, factionID);
				this.RegisterStat(stat, factionID);
				if (component != null)
				{
					component.Entity.RegisterStat(stat);
				}
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00052230 File Offset: 0x00050430
		private void RegisterStat(Stat stat, string factionID)
		{
			if (!this._registeredStats.ContainsKey(factionID))
			{
				this._registeredStats.Add(factionID, new Dictionary<int, List<Stat>>());
			}
			if (!this._registeredStats[factionID].ContainsKey(stat.Code))
			{
				this._registeredStats[factionID].Add(stat.Code, new List<Stat>());
				return;
			}
			this._registeredStats[factionID][stat.Code].Add(stat);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x000522B0 File Offset: 0x000504B0
		public void ApplyModifiersToStat(Stat stat, string factionID)
		{
			if (Singleton<FactionManager>.Instance.IsPlayerFaction(factionID))
			{
				if (this._allyModifiers.ContainsKey(stat.Code))
				{
					stat.GamemodeModifier = this._allyModifiers[stat.Code];
				}
			}
			else if (this._enemyModifiers.ContainsKey(stat.Code))
			{
				stat.GamemodeModifier = this._enemyModifiers[stat.Code];
			}
			if (this._upgradeModifiers.ContainsKey(factionID) && this._upgradeModifiers[factionID].ContainsKey(stat.Code))
			{
				stat.GamemodeModifier = this._upgradeModifiers[factionID][stat.Code];
			}
			if (this._environmentModifiers.ContainsKey(stat.Code))
			{
				stat.GamemodeModifier = this._environmentModifiers[stat.Code];
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0005238E File Offset: 0x0005058E
		public void AddAllyModifier(int code, StatModifier modifier)
		{
			if (this._allyModifiers.ContainsKey(code))
			{
				this._allyModifiers[code] = modifier;
				return;
			}
			this._allyModifiers.Add(code, modifier);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x000523B9 File Offset: 0x000505B9
		public void AddEnemyModifier(int code, StatModifier modifier)
		{
			if (this._enemyModifiers.ContainsKey(code))
			{
				this._enemyModifiers[code] = modifier;
				return;
			}
			this._enemyModifiers.Add(code, modifier);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x000523E4 File Offset: 0x000505E4
		public void RegisterUpgradeModifier(StatType type, StatModifier modifier, string factionID)
		{
			this.RegisterUpgradeModifier((int)type, modifier, factionID);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000523F0 File Offset: 0x000505F0
		public void RegisterUpgradeModifier(int code, StatModifier modifier, string factionID)
		{
			if (!this._upgradeModifiers.ContainsKey(factionID))
			{
				this._upgradeModifiers.Add(factionID, new Dictionary<int, StatModifier>());
			}
			if (this._upgradeModifiers[factionID].ContainsKey(code))
			{
				this._upgradeModifiers[factionID][code] = modifier;
				return;
			}
			this._upgradeModifiers[factionID].Add(code, modifier);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00052457 File Offset: 0x00050657
		public void RegisterEnvironmentModifier(StatType type, StatModifier modifier)
		{
			this.RegisterEnvironmentModifier((int)type, modifier);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00052461 File Offset: 0x00050661
		public void RegisterEnvironmentModifier(int code, StatModifier modifier)
		{
			if (this._environmentModifiers.ContainsKey(code))
			{
				this._environmentModifiers[code] = modifier;
				return;
			}
			this._environmentModifiers.Add(code, modifier);
		}

		// Token: 0x04000F2C RID: 3884
		private readonly Dictionary<string, Dictionary<int, List<Stat>>> _registeredStats = new Dictionary<string, Dictionary<int, List<Stat>>>();

		// Token: 0x04000F2D RID: 3885
		private Dictionary<int, StatModifier> _allyModifiers = new Dictionary<int, StatModifier>();

		// Token: 0x04000F2E RID: 3886
		private Dictionary<int, StatModifier> _enemyModifiers = new Dictionary<int, StatModifier>();

		// Token: 0x04000F2F RID: 3887
		private Dictionary<string, Dictionary<int, StatModifier>> _upgradeModifiers = new Dictionary<string, Dictionary<int, StatModifier>>();

		// Token: 0x04000F30 RID: 3888
		private Dictionary<int, StatModifier> _environmentModifiers = new Dictionary<int, StatModifier>();
	}
}
