using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200035B RID: 859
	public class WindowManagerButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600168A RID: 5770 RVA: 0x00069856 File Offset: 0x00067A56
		private void OnEnable()
		{
			if (this.buttonAnimator == null)
			{
				this.buttonAnimator = base.gameObject.GetComponent<Animator>();
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00003212 File Offset: 0x00001412
		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00003212 File Offset: 0x00001412
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x04001580 RID: 5504
		public bool enableMobileMode;

		// Token: 0x04001581 RID: 5505
		[HideInInspector]
		public Animator buttonAnimator;
	}
}
