using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000378 RID: 888
	[RequireComponent(typeof(FogOfWarHider))]
	public abstract class HiderBehavior : MonoBehaviour
	{
		// Token: 0x06001725 RID: 5925 RVA: 0x0006EC3F File Offset: 0x0006CE3F
		protected virtual void Awake()
		{
			this.OnHide();
			base.GetComponent<FogOfWarHider>().OnActiveChanged += this.OnStatusChanged;
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0006EC5E File Offset: 0x0006CE5E
		private void OnStatusChanged(bool isEnabled)
		{
			this.IsEnabled = isEnabled;
			if (isEnabled)
			{
				this.OnReveal();
				return;
			}
			this.OnHide();
		}

		// Token: 0x06001727 RID: 5927
		protected abstract void OnReveal();

		// Token: 0x06001728 RID: 5928
		protected abstract void OnHide();

		// Token: 0x04001691 RID: 5777
		protected bool IsEnabled;
	}
}
