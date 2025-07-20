using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002EB RID: 747
	public class DemoListShadow : MonoBehaviour, IBeginDragHandler, IEventSystemHandler
	{
		// Token: 0x060014B3 RID: 5299 RVA: 0x0005EAB8 File Offset: 0x0005CCB8
		private void Awake()
		{
			this.CheckForValue(0f);
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0005EAC8 File Offset: 0x0005CCC8
		public void CheckForValue(float value)
		{
			if ((double)value > 0.05)
			{
				base.StopCoroutine("LeftCGFadeOut");
				base.StartCoroutine("LeftCGFadeIn");
			}
			else
			{
				base.StopCoroutine("LeftCGFadeIn");
				base.StartCoroutine("LeftCGFadeOut");
			}
			if ((double)value < 0.95)
			{
				base.StopCoroutine("RightCGFadeOut");
				base.StartCoroutine("RightCGFadeIn");
				return;
			}
			base.StopCoroutine("RightCGFadeIn");
			base.StartCoroutine("RightCGFadeOut");
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0005EB4E File Offset: 0x0005CD4E
		public void ScrollUp()
		{
			base.StopCoroutine("ScrollDownHelper");
			base.StartCoroutine("ScrollUpHelper");
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0005EB67 File Offset: 0x0005CD67
		public void ScrollDown()
		{
			base.StopCoroutine("ScrollUpHelper");
			base.StartCoroutine("ScrollDownHelper");
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0005EB80 File Offset: 0x0005CD80
		public void OnBeginDrag(PointerEventData data)
		{
			base.StopCoroutine("ScrollUpHelper");
			base.StopCoroutine("ScrollDownHelper");
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0005EB98 File Offset: 0x0005CD98
		private IEnumerator ScrollUpHelper()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.scrollTime)
			{
				this.listScrollbar.value = Mathf.Lerp(this.listScrollbar.value, 0f, elapsedTime / this.scrollTime);
				elapsedTime += Time.unscaledDeltaTime;
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0005EBA7 File Offset: 0x0005CDA7
		private IEnumerator ScrollDownHelper()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.scrollTime)
			{
				this.listScrollbar.value = Mathf.Lerp(this.listScrollbar.value, 1f, elapsedTime / this.scrollTime);
				elapsedTime += Time.unscaledDeltaTime;
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0005EBB6 File Offset: 0x0005CDB6
		private IEnumerator LeftCGFadeIn()
		{
			this.leftCG.interactable = true;
			this.leftCG.blocksRaycasts = true;
			while (this.leftCG.alpha < 0.99f)
			{
				this.leftCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.leftCG.alpha = 1f;
			yield break;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0005EBC5 File Offset: 0x0005CDC5
		private IEnumerator LeftCGFadeOut()
		{
			this.leftCG.interactable = false;
			this.leftCG.blocksRaycasts = false;
			while (this.leftCG.alpha > 0.01f)
			{
				this.leftCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.leftCG.alpha = 0f;
			yield break;
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0005EBD4 File Offset: 0x0005CDD4
		private IEnumerator RightCGFadeIn()
		{
			this.rightCG.interactable = true;
			this.rightCG.blocksRaycasts = true;
			while (this.rightCG.alpha < 0.99f)
			{
				this.rightCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.rightCG.alpha = 1f;
			yield break;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0005EBE3 File Offset: 0x0005CDE3
		private IEnumerator RightCGFadeOut()
		{
			this.rightCG.interactable = false;
			this.rightCG.blocksRaycasts = false;
			while (this.rightCG.alpha > 0.01f)
			{
				this.rightCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.rightCG.alpha = 0f;
			yield break;
		}

		// Token: 0x04001287 RID: 4743
		[Header("Resources")]
		[SerializeField]
		private Scrollbar listScrollbar;

		// Token: 0x04001288 RID: 4744
		[SerializeField]
		private CanvasGroup leftCG;

		// Token: 0x04001289 RID: 4745
		[SerializeField]
		private CanvasGroup rightCG;

		// Token: 0x0400128A RID: 4746
		[Header("Settings")]
		[SerializeField]
		private float scrollTime = 5f;

		// Token: 0x0400128B RID: 4747
		[SerializeField]
		private float transitionSpeed = 4f;
	}
}
