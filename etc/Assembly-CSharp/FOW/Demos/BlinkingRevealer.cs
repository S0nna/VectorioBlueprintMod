using System;
using UnityEngine;

namespace FOW.Demos
{
	// Token: 0x0200038E RID: 910
	public class BlinkingRevealer : MonoBehaviour
	{
		// Token: 0x060017A2 RID: 6050 RVA: 0x00072DCB File Offset: 0x00070FCB
		private void Awake()
		{
			if (this.RandomOffset)
			{
				this.BlinkCycleTime += Random.Range(0f, this.BlinkCycleTime * 0.5f);
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00072DF8 File Offset: 0x00070FF8
		private void Update()
		{
			if (Time.time % this.BlinkCycleTime < this.BlinkCycleTime / 2f)
			{
				if (!base.transform.GetChild(0).gameObject.activeInHierarchy)
				{
					base.transform.GetChild(0).gameObject.SetActive(true);
					return;
				}
			}
			else if (base.transform.GetChild(0).gameObject.activeInHierarchy)
			{
				base.transform.GetChild(0).gameObject.SetActive(false);
			}
		}

		// Token: 0x04001746 RID: 5958
		public float BlinkCycleTime = 5f;

		// Token: 0x04001747 RID: 5959
		public bool RandomOffset = true;
	}
}
