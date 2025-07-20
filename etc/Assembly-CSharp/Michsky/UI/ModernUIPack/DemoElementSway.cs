using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002E3 RID: 739
	public class DemoElementSway : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x06001477 RID: 5239 RVA: 0x0005E040 File Offset: 0x0005C240
		private void Awake()
		{
			if (this.swayParent == null)
			{
				DemoElementSwayParent component = base.transform.parent.GetComponent<DemoElementSwayParent>();
				if (component == null)
				{
					base.transform.parent.gameObject.AddComponent<DemoElementSwayParent>();
				}
				this.swayParent = component;
			}
			this.defaultPos = this.swayObject.anchoredPosition;
			this.normalCG.alpha = 1f;
			this.highlightedCG.alpha = 0f;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0005E0C4 File Offset: 0x0005C2C4
		private void Update()
		{
			if (this.allowSway)
			{
				this.cursorPos = Input.mousePosition;
			}
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.ProcessOverlay();
				return;
			}
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || this.mainCanvas.renderMode == RenderMode.WorldSpace)
			{
				this.ProcessSSC();
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0005E11C File Offset: 0x0005C31C
		private void ProcessOverlay()
		{
			if (this.allowSway)
			{
				this.swayObject.position = Vector2.Lerp(this.swayObject.position, this.cursorPos, Time.deltaTime * this.smoothness);
				return;
			}
			this.swayObject.localPosition = Vector2.Lerp(this.swayObject.localPosition, this.defaultPos, Time.deltaTime * this.smoothness);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0005E1A8 File Offset: 0x0005C3A8
		private void ProcessSSC()
		{
			if (this.allowSway)
			{
				this.swayObject.position = Vector2.Lerp(this.swayObject.position, Camera.main.ScreenToWorldPoint(this.cursorPos), Time.deltaTime * this.smoothness);
				return;
			}
			this.swayObject.localPosition = Vector2.Lerp(this.swayObject.localPosition, this.defaultPos, Time.deltaTime * this.smoothness);
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0005E23B File Offset: 0x0005C43B
		public void Dissolve()
		{
			if (this.wmSelected)
			{
				return;
			}
			base.StopCoroutine("DissolveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StopCoroutine("ActiveHelper");
			base.StartCoroutine("DissolveHelper");
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0005E273 File Offset: 0x0005C473
		public void Highlight()
		{
			if (this.wmSelected)
			{
				return;
			}
			base.StopCoroutine("DissolveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StopCoroutine("ActiveHelper");
			base.StartCoroutine("HighlightHelper");
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0005E2AB File Offset: 0x0005C4AB
		public void Active()
		{
			if (this.wmSelected)
			{
				return;
			}
			base.StopCoroutine("DissolveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StopCoroutine("HighlightHelper");
			base.StartCoroutine("ActiveHelper");
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0005E2E3 File Offset: 0x0005C4E3
		public void WindowManagerSelect()
		{
			this.wmSelected = true;
			base.StopCoroutine("ActiveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StartCoroutine("WMSelectHelper");
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0005E30E File Offset: 0x0005C50E
		public void WindowManagerDeselect()
		{
			this.wmSelected = false;
			base.StartCoroutine("WMDeselectHelper");
			base.StartCoroutine("DissolveHelper");
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0005E32F File Offset: 0x0005C52F
		public void OnPointerEnter(PointerEventData data)
		{
			this.allowSway = true;
			this.swayParent.DissolveAll(this);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0005E344 File Offset: 0x0005C544
		public void OnPointerExit(PointerEventData data)
		{
			this.allowSway = false;
			this.swayParent.HighlightAll();
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0005E358 File Offset: 0x0005C558
		public void OnPointerClick(PointerEventData data)
		{
			this.onClick.Invoke();
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0005E365 File Offset: 0x0005C565
		private IEnumerator DissolveHelper()
		{
			while (this.normalCG.alpha > this.dissolveAlpha)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.highlightedCG.alpha = 0f;
			this.normalCG.alpha = this.dissolveAlpha;
			this.highlightedCG.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0005E374 File Offset: 0x0005C574
		private IEnumerator HighlightHelper()
		{
			while (this.normalCG.alpha < 1f)
			{
				this.normalCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.normalCG.alpha = 1f;
			this.highlightedCG.alpha = 0f;
			this.highlightedCG.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0005E383 File Offset: 0x0005C583
		private IEnumerator ActiveHelper()
		{
			this.highlightedCG.gameObject.SetActive(true);
			while (this.highlightedCG.alpha < 1f)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.highlightedCG.alpha = 1f;
			this.normalCG.alpha = 0f;
			yield break;
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0005E392 File Offset: 0x0005C592
		private IEnumerator WMSelectHelper()
		{
			this.selectedCG.gameObject.SetActive(true);
			while (this.selectedCG.alpha < 1f)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.selectedCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.highlightedCG.alpha = 0f;
			this.normalCG.alpha = 0f;
			this.selectedCG.alpha = 1f;
			yield break;
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0005E3A1 File Offset: 0x0005C5A1
		private IEnumerator WMDeselectHelper()
		{
			while (this.selectedCG.alpha > 0f)
			{
				this.selectedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.selectedCG.alpha = 0f;
			this.selectedCG.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04001261 RID: 4705
		[Header("Resources")]
		[SerializeField]
		private DemoElementSwayParent swayParent;

		// Token: 0x04001262 RID: 4706
		[SerializeField]
		private Canvas mainCanvas;

		// Token: 0x04001263 RID: 4707
		[SerializeField]
		private RectTransform swayObject;

		// Token: 0x04001264 RID: 4708
		[SerializeField]
		private CanvasGroup normalCG;

		// Token: 0x04001265 RID: 4709
		[SerializeField]
		private CanvasGroup highlightedCG;

		// Token: 0x04001266 RID: 4710
		[SerializeField]
		private CanvasGroup selectedCG;

		// Token: 0x04001267 RID: 4711
		[Header("Settings")]
		[SerializeField]
		private float smoothness = 10f;

		// Token: 0x04001268 RID: 4712
		[SerializeField]
		private float transitionSpeed = 8f;

		// Token: 0x04001269 RID: 4713
		[SerializeField]
		[Range(0f, 1f)]
		private float dissolveAlpha = 0.5f;

		// Token: 0x0400126A RID: 4714
		[Header("Events")]
		[SerializeField]
		private UnityEvent onClick;

		// Token: 0x0400126B RID: 4715
		private bool allowSway;

		// Token: 0x0400126C RID: 4716
		[HideInInspector]
		public bool wmSelected;

		// Token: 0x0400126D RID: 4717
		private Vector3 cursorPos;

		// Token: 0x0400126E RID: 4718
		private Vector2 defaultPos;
	}
}
