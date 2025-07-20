using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000305 RID: 773
	[RequireComponent(typeof(Animator))]
	public class AnimatedIconHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x0600152B RID: 5419 RVA: 0x000614CC File Offset: 0x0005F6CC
		private void Start()
		{
			if (this.iconAnimator == null)
			{
				this.iconAnimator = base.gameObject.GetComponent<Animator>();
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x000614ED File Offset: 0x0005F6ED
		public void PlayIn()
		{
			this.iconAnimator.Play("In");
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x000614FF File Offset: 0x0005F6FF
		public void PlayOut()
		{
			this.iconAnimator.Play("Out");
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00061511 File Offset: 0x0005F711
		public void ClickEvent()
		{
			if (this.isClicked)
			{
				this.PlayOut();
				this.isClicked = false;
				return;
			}
			this.PlayIn();
			this.isClicked = true;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x00061536 File Offset: 0x0005F736
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.playType == AnimatedIconHandler.PlayType.Click)
			{
				this.ClickEvent();
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00061546 File Offset: 0x0005F746
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.playType == AnimatedIconHandler.PlayType.Hover)
			{
				this.iconAnimator.Play("In");
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x00061561 File Offset: 0x0005F761
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.playType == AnimatedIconHandler.PlayType.Hover)
			{
				this.iconAnimator.Play("Out");
			}
		}

		// Token: 0x0400132A RID: 4906
		[Header("Settings")]
		public AnimatedIconHandler.PlayType playType;

		// Token: 0x0400132B RID: 4907
		public Animator iconAnimator;

		// Token: 0x0400132C RID: 4908
		private bool isClicked;

		// Token: 0x02000306 RID: 774
		public enum PlayType
		{
			// Token: 0x0400132E RID: 4910
			Click,
			// Token: 0x0400132F RID: 4911
			Hover,
			// Token: 0x04001330 RID: 4912
			None
		}
	}
}
