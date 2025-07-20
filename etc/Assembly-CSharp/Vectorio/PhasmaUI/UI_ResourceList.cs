using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.PhasmaUI
{
	// Token: 0x02000291 RID: 657
	public class UI_ResourceList : MonoBehaviour
	{
		// Token: 0x06001294 RID: 4756 RVA: 0x00055F98 File Offset: 0x00054198
		public void AddResource(ResourceData data, int amount)
		{
			if (this._resources.ContainsKey(data))
			{
				Debug.Log("[RESOURCE LIST] Duplicate ID " + data.ID + " attempting to register in resource list!");
				return;
			}
			UI_GlobalResourceValue ui_GlobalResourceValue = Object.Instantiate<UI_GlobalResourceValue>(this.resourceUI, this.resourceList);
			ui_GlobalResourceValue.Setup(this, data, amount);
			this._resources.Add(data, ui_GlobalResourceValue);
			this.UpdateListOrganization();
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00055FFC File Offset: 0x000541FC
		private void UpdateListOrganization()
		{
			foreach (KeyValuePair<ResourceData, UI_GlobalResourceValue> keyValuePair in this._resources)
			{
				keyValuePair.Value.transform.SetSiblingIndex(keyValuePair.Value.Resource.Order);
			}
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0005606C File Offset: 0x0005426C
		public void UpdateResourceAmount(ResourceData data, int amount)
		{
			if (this._resources.ContainsKey(data))
			{
				this._resources[data].UpdateAmount(amount);
				return;
			}
			this.AddResource(data, amount);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00056097 File Offset: 0x00054297
		public void UpdatePower(int amount)
		{
			if (this._powerUI != null)
			{
				this._powerUI.UpdateAmount(amount);
				return;
			}
			Debug.Log("[UIRL] Cant update power UI because the instance has not been set!");
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000560BE File Offset: 0x000542BE
		public void UpdateHeat(int amount)
		{
			if (this._heatUI != null)
			{
				this._heatUI.UpdateAmount(amount);
				return;
			}
			Debug.Log("[UIRL] Cant update heat UI because the instance has not been set!");
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x000560E5 File Offset: 0x000542E5
		public void AddPowerResource(UI_Legacy_ResourceAmount ui)
		{
			this._powerUI = ui;
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x000560EE File Offset: 0x000542EE
		public void AddHeatResource(UI_Legacy_ResourceAmount ui)
		{
			this._heatUI = ui;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x000560F7 File Offset: 0x000542F7
		public void OnHover(ResourceData resource, float xPos, bool toggle)
		{
			if (toggle)
			{
				this.resourceInfo.Set(resource, xPos);
				return;
			}
			this.resourceInfo.Disable();
		}

		// Token: 0x0400104E RID: 4174
		protected Dictionary<ResourceData, UI_GlobalResourceValue> _resources = new Dictionary<ResourceData, UI_GlobalResourceValue>();

		// Token: 0x0400104F RID: 4175
		public UI_GlobalResourceValue resourceUI;

		// Token: 0x04001050 RID: 4176
		public UI_ResourceInfo resourceInfo;

		// Token: 0x04001051 RID: 4177
		public Transform resourceList;

		// Token: 0x04001052 RID: 4178
		private UI_Legacy_ResourceAmount _powerUI;

		// Token: 0x04001053 RID: 4179
		private UI_Legacy_ResourceAmount _heatUI;
	}
}
