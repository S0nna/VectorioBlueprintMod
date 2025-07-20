using System;

namespace Vectorio.Stats
{
	// Token: 0x02000261 RID: 609
	public class ModifierSocket
	{
		// Token: 0x060011CB RID: 4555 RVA: 0x00051D12 File Offset: 0x0004FF12
		public void SetModifier(StatModifier newModifier)
		{
			this._modifier = newModifier;
			this._isModifierAttached = true;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00051D22 File Offset: 0x0004FF22
		public float Calculate(float value)
		{
			if (!this._isModifierAttached)
			{
				return value;
			}
			if (!this._modifier.IsMultiplier)
			{
				return value + this._modifier.Value;
			}
			return value * this._modifier.Value;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00051D56 File Offset: 0x0004FF56
		public void Clear()
		{
			this._modifier = null;
			this._isModifierAttached = false;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00051D66 File Offset: 0x0004FF66
		public StatModifier Modifier
		{
			get
			{
				return this._modifier;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x00051D6E File Offset: 0x0004FF6E
		public bool IsModifierAttached
		{
			get
			{
				return this._isModifierAttached;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00051D76 File Offset: 0x0004FF76
		public bool DoesModifierHaveEffect
		{
			get
			{
				if (!this._modifier.IsMultiplier)
				{
					return this._modifier.Value != 0f;
				}
				return this._modifier.Value != 1f;
			}
		}

		// Token: 0x04000F1F RID: 3871
		private StatModifier _modifier;

		// Token: 0x04000F20 RID: 3872
		private bool _isModifierAttached;
	}
}
