using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Stats
{
	// Token: 0x02000262 RID: 610
	public abstract class Stat
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00051DB0 File Offset: 0x0004FFB0
		public int Code
		{
			get
			{
				return this._code;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00051DB8 File Offset: 0x0004FFB8
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x00051DC5 File Offset: 0x0004FFC5
		public StatModifier GamemodeModifier
		{
			get
			{
				return this._gamemodeModifierSocket.Modifier;
			}
			set
			{
				this._gamemodeModifierSocket.SetModifier(value);
				this._isDirty = true;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x00051DDA File Offset: 0x0004FFDA
		// (set) Token: 0x060011D6 RID: 4566 RVA: 0x00051DE7 File Offset: 0x0004FFE7
		public StatModifier UpgradeModifier
		{
			get
			{
				return this._upgradeModifierSocket.Modifier;
			}
			set
			{
				this._upgradeModifierSocket.SetModifier(value);
				this._isDirty = true;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00051DFC File Offset: 0x0004FFFC
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x00051E09 File Offset: 0x00050009
		public StatModifier EnvironmentModifier
		{
			get
			{
				return this._environmentModifierSocket.Modifier;
			}
			set
			{
				this._environmentModifierSocket.SetModifier(value);
				this._isDirty = true;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x00051E1E File Offset: 0x0005001E
		public StatModifier BuildingModifier
		{
			get
			{
				return this._buildingModifierSocket.Modifier;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x00051E2B File Offset: 0x0005002B
		public bool HasGamemodeModifier
		{
			get
			{
				return this._gamemodeModifierSocket.IsModifierAttached && this._gamemodeModifierSocket.DoesModifierHaveEffect;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00051E47 File Offset: 0x00050047
		public bool HasUpgradeModifier
		{
			get
			{
				return this._upgradeModifierSocket.IsModifierAttached && this._gamemodeModifierSocket.DoesModifierHaveEffect;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x00051E63 File Offset: 0x00050063
		public bool HasEnvironmentModifier
		{
			get
			{
				return this._environmentModifierSocket.IsModifierAttached && this._gamemodeModifierSocket.DoesModifierHaveEffect;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x00051E7F File Offset: 0x0005007F
		public bool HasBuildingModifier
		{
			get
			{
				return this._buildingModifierSocket.IsModifierAttached && this._gamemodeModifierSocket.DoesModifierHaveEffect;
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00051E9C File Offset: 0x0005009C
		public void AddModifier(StatModifier modifier)
		{
			if (modifier.SourceID == null)
			{
				Debug.Log("[E_Stat] Cannot apply modifier with no source ID!");
				return;
			}
			if (this._buildingModifiers.ContainsKey(modifier.SourceID.Value))
			{
				Debug.Log("[E_Stat] This source has already applied a modifier to this stat! (" + this._code.ToString() + ")");
				return;
			}
			this._buildingModifiers.Add(modifier.SourceID.Value, modifier);
			this.CalculateBuildingModifier();
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00003212 File Offset: 0x00001412
		private void CalculateBuildingModifier()
		{
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00051F16 File Offset: 0x00050116
		public void RemoveModifier(ulong sourceID)
		{
			if (!this._buildingModifiers.ContainsKey(sourceID))
			{
				Debug.Log("[E_Stat] The stat does not have a modifier from source ID " + sourceID.ToString());
				return;
			}
			this._buildingModifiers.Remove(sourceID);
			this.CalculateBuildingModifier();
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00051F50 File Offset: 0x00050150
		public void ClearModifiers()
		{
			this._buildingModifiers.Clear();
			this._buildingModifierSocket.Clear();
			this._isDirty = true;
		}

		// Token: 0x04000F21 RID: 3873
		protected int _code;

		// Token: 0x04000F22 RID: 3874
		protected bool _isDirty = true;

		// Token: 0x04000F23 RID: 3875
		protected ModifierSocket _gamemodeModifierSocket = new ModifierSocket();

		// Token: 0x04000F24 RID: 3876
		protected ModifierSocket _upgradeModifierSocket = new ModifierSocket();

		// Token: 0x04000F25 RID: 3877
		protected ModifierSocket _environmentModifierSocket = new ModifierSocket();

		// Token: 0x04000F26 RID: 3878
		protected ModifierSocket _buildingModifierSocket = new ModifierSocket();

		// Token: 0x04000F27 RID: 3879
		private Dictionary<ulong, StatModifier> _buildingModifiers = new Dictionary<ulong, StatModifier>();
	}
}
