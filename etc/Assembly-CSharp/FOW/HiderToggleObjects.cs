using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200037B RID: 891
	public class HiderToggleObjects : HiderBehavior
	{
		// Token: 0x06001732 RID: 5938 RVA: 0x0006ED88 File Offset: 0x0006CF88
		protected override void OnHide()
		{
			GameObject[] array = this.RevealedObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			array = this.HiddenObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0006EDD4 File Offset: 0x0006CFD4
		protected override void OnReveal()
		{
			GameObject[] array = this.RevealedObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			array = this.HiddenObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}

		// Token: 0x04001694 RID: 5780
		[Tooltip("Objects that will be visible when in Line Of Sight")]
		public GameObject[] RevealedObjects;

		// Token: 0x04001695 RID: 5781
		[Tooltip("Objects that will be visible when out of Line Of Sight")]
		public GameObject[] HiddenObjects;
	}
}
