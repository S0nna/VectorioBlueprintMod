using System;

namespace Vectorio.Stats
{
	// Token: 0x02000264 RID: 612
	public class StatInt : Stat
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00052085 File Offset: 0x00050285
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x000520AF File Offset: 0x000502AF
		public int Value
		{
			get
			{
				if (this._isDirty)
				{
					this._modifiedValue = this.Calculate();
					this._isDirty = false;
					return this._modifiedValue;
				}
				return this._modifiedValue;
			}
			set
			{
				this._baseValue = value;
				this._isDirty = true;
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x000520BF File Offset: 0x000502BF
		public StatInt(int originalValue, int code, string factionID)
		{
			this.Value = originalValue;
			this._code = code;
			Singleton<StatManager>.Instance.ApplyModifiersToStat(this, factionID);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000520E1 File Offset: 0x000502E1
		public StatInt(int originalValue, StatType code) : this(originalValue, (int)code, Singleton<FactionManager>.Instance.PlayerFactionID)
		{
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x000520F8 File Offset: 0x000502F8
		private int Calculate()
		{
			float num = (float)this._baseValue;
			num = this._gamemodeModifierSocket.Calculate(num);
			num = this._upgradeModifierSocket.Calculate(num);
			num = this._environmentModifierSocket.Calculate(num);
			num = this._buildingModifierSocket.Calculate(num);
			return (int)num;
		}

		// Token: 0x04000F2A RID: 3882
		private int _baseValue;

		// Token: 0x04000F2B RID: 3883
		private int _modifiedValue;
	}
}
