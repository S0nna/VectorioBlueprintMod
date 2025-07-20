using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002E9 RID: 745
	public class DemoElementSwayParent : MonoBehaviour
	{
		// Token: 0x060014A7 RID: 5287 RVA: 0x0005E830 File Offset: 0x0005CA30
		private void Awake()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				this.elements.Add(transform.GetComponent<DemoElementSway>());
			}
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0005E894 File Offset: 0x0005CA94
		public void DissolveAll(DemoElementSway currentSway)
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (this.elements[i] == currentSway)
				{
					this.elements[i].Active();
				}
				else
				{
					this.elements[i].Dissolve();
				}
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0005E8F0 File Offset: 0x0005CAF0
		public void HighlightAll()
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				this.elements[i].Highlight();
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0005E924 File Offset: 0x0005CB24
		public void SetWindowManagerButton(int index)
		{
			if (this.elements.Count == 0)
			{
				base.StartCoroutine("SWMHelper", index);
				return;
			}
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (i == index)
				{
					this.elements[i].WindowManagerSelect();
				}
				else if (this.elements[i].wmSelected)
				{
					this.elements[i].WindowManagerDeselect();
				}
			}
			if (this.titleAnimator == null)
			{
				return;
			}
			this.elementTitleHelper.text = this.elements[this.prevIndex].gameObject.name;
			this.elementTitle.text = this.elements[index].gameObject.name;
			this.titleAnimator.Play("Idle");
			this.titleAnimator.Play("Transition");
			this.prevIndex = index;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0005EA1F File Offset: 0x0005CC1F
		private IEnumerator SWMHelper(int index)
		{
			yield return new WaitForSeconds(0.1f);
			this.SetWindowManagerButton(index);
			yield break;
		}

		// Token: 0x0400127E RID: 4734
		[SerializeField]
		private Animator titleAnimator;

		// Token: 0x0400127F RID: 4735
		[SerializeField]
		private TextMeshProUGUI elementTitle;

		// Token: 0x04001280 RID: 4736
		[SerializeField]
		private TextMeshProUGUI elementTitleHelper;

		// Token: 0x04001281 RID: 4737
		private List<DemoElementSway> elements = new List<DemoElementSway>();

		// Token: 0x04001282 RID: 4738
		private int prevIndex;
	}
}
