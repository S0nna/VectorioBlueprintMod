using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000379 RID: 889
	public class HiderDisableObjects : HiderBehavior
	{
		// Token: 0x0600172A RID: 5930 RVA: 0x0006EC78 File Offset: 0x0006CE78
		protected override void OnHide()
		{
			GameObject[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].SetActive(false);
			}
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0006ECA4 File Offset: 0x0006CEA4
		protected override void OnReveal()
		{
			GameObject[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].SetActive(true);
			}
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0006ECCF File Offset: 0x0006CECF
		public void ModifyHiddenObjects(GameObject[] newObjectsToHide)
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

		// Token: 0x04001692 RID: 5778
		[SerializeField]
		private GameObject[] ObjectsToHide;
	}
}
