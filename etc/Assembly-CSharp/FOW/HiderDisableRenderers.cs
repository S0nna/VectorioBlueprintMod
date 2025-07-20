using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200037A RID: 890
	public class HiderDisableRenderers : HiderBehavior
	{
		// Token: 0x0600172E RID: 5934 RVA: 0x0006ED04 File Offset: 0x0006CF04
		protected override void OnHide()
		{
			Renderer[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].enabled = false;
			}
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0006ED30 File Offset: 0x0006CF30
		protected override void OnReveal()
		{
			Renderer[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].enabled = true;
			}
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0006ED5B File Offset: 0x0006CF5B
		public void ModifyHiddenRenderers(Renderer[] newObjectsToHide)
		{
			this.OnReveal();
			this.ObjectsToHide = newObjectsToHide;
			if (!base.enabled)
			{
				return;
			}
			if (!this.IsEnabled)
			{
				this.OnHide();
				return;
			}
			this.OnReveal();
		}

		// Token: 0x04001693 RID: 5779
		[SerializeField]
		private Renderer[] ObjectsToHide;
	}
}
