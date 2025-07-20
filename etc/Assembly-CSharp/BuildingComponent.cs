using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
public abstract class BuildingComponent : EntityComponent
{
	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060007F7 RID: 2039 RVA: 0x000232D8 File Offset: 0x000214D8
	// (set) Token: 0x060007F8 RID: 2040 RVA: 0x0002333A File Offset: 0x0002153A
	public Building Building
	{
		get
		{
			if (this._building == null)
			{
				if (!this._checkBuilding)
				{
					return null;
				}
				this._checkBuilding = false;
				if (base.Entity.Has_EComponent<Building>())
				{
					this._building = base.Entity.Get_EComponent<Building>(false);
					return this._building;
				}
				Debug.Log("[B-Comp] Missing building component, this will cause errors!");
			}
			return this._building;
		}
		set
		{
			this._building = value;
		}
	}

	// Token: 0x0400053D RID: 1341
	private Building _building;

	// Token: 0x0400053E RID: 1342
	private bool _checkBuilding = true;
}
