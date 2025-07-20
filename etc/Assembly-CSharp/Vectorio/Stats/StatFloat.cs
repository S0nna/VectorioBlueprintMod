using System;

namespace Vectorio.Stats
{
	// Token: 0x02000263 RID: 611
	public class StatFloat : Stat
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x00051FC1 File Offset: 0x000501C1
		// (set) Token: 0x060011E4 RID: 4580 RVA: 0x00051FEB File Offset: 0x000501EB
		public float Value
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

		// Token: 0x060011E5 RID: 4581 RVA: 0x00051FFB File Offset: 0x000501FB
		public StatFloat(float originalValue, int code, string factionID)
		{
			this.Value = originalValue;
			this._code = code;
			Singleton<StatManager>.Instance.ApplyModifiersToStat(this, factionID);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0005201D File Offset: 0x0005021D
		public StatFloat(float originalValue, StatType code) : this(originalValue, (int)code, Singleton<FactionManager>.Instance.PlayerFactionID)
		{
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00052034 File Offset: 0x00050234
		private float Calculate()
		{
			float num = this._baseValue;
			num = this._gamemodeModifierSocket.Calculate(num);
			num = this._upgradeModifierSocket.Calculate(num);
			num = this._environmentModifierSocket.Calculate(num);
			num = this._buildingModifierSocket.Calculate(num);
			return (float)Math.Round((double)num, 2);
		}

		// Token: 0x04000F28 RID: 3880
		private float _baseValue;

		// Token: 0x04000F29 RID: 3881
		private float _modifiedValue;
	}
}
