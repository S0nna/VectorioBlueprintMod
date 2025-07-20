using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002C7 RID: 711
	[ExecuteInEditMode]
	[RequireComponent(typeof(Button))]
	public class ButtonManagerIcon : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x060013FC RID: 5116 RVA: 0x0005B58A File Offset: 0x0005978A
		private void OnEnable()
		{
			if (this.normalCG == null && this.highlightedCG == null)
			{
				return;
			}
			this.normalCG.alpha = 1f;
			this.highlightedCG.alpha = 0f;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0005B5CC File Offset: 0x000597CC
		private void Awake()
		{
			if (this.animationSolution == ButtonManagerIcon.AnimationSolution.Script)
			{
				this.normalCG = base.transform.Find("Normal").GetComponent<CanvasGroup>();
				this.highlightedCG = base.transform.Find("Highlighted").GetComponent<CanvasGroup>();
				Object.Destroy(base.GetComponent<Animator>());
			}
			if (this.buttonVar == null)
			{
				this.buttonVar = base.gameObject.GetComponent<Button>();
			}
			if (this.enableButtonSounds && this.useClickSound)
			{
				this.buttonVar.onClick.AddListener(delegate()
				{
					this.soundSource.PlayOneShot(this.clickSound);
				});
			}
			if (!this.useCustomContent)
			{
				this.UpdateUI();
			}
			this.buttonVar.onClick.AddListener(delegate()
			{
				this.clickEvent.Invoke();
			});
			if (this.useRipple && this.rippleParent != null)
			{
				this.rippleParent.SetActive(false);
				return;
			}
			if (!this.useRipple && this.rippleParent != null)
			{
				Object.Destroy(this.rippleParent);
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0005B6DC File Offset: 0x000598DC
		public void UpdateUI()
		{
			if (this.normalIcon != null)
			{
				this.normalIcon.sprite = this.buttonIcon;
			}
			if (this.highlightedIcon != null)
			{
				this.highlightedIcon.sprite = this.buttonIcon;
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0005B71C File Offset: 0x0005991C
		public void CreateRipple(Vector2 pos)
		{
			if (this.rippleParent != null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<Image>();
				gameObject.GetComponent<Image>().sprite = this.rippleShape;
				gameObject.name = "Ripple";
				this.rippleParent.SetActive(true);
				gameObject.transform.SetParent(this.rippleParent.transform);
				if (this.renderOnTop)
				{
					this.rippleParent.transform.SetAsLastSibling();
				}
				else
				{
					this.rippleParent.transform.SetAsFirstSibling();
				}
				if (this.centered)
				{
					gameObject.transform.localPosition = new Vector2(0f, 0f);
				}
				else
				{
					gameObject.transform.position = pos;
				}
				gameObject.AddComponent<Ripple>();
				Ripple component = gameObject.GetComponent<Ripple>();
				component.speed = this.speed;
				component.maxSize = this.maxSize;
				component.startColor = this.startColor;
				component.transitionColor = this.transitionColor;
				if (this.rippleUpdateMode == ButtonManagerIcon.RippleUpdateMode.Normal)
				{
					component.unscaledTime = false;
					return;
				}
				component.unscaledTime = true;
			}
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0005B83E File Offset: 0x00059A3E
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.useRipple && this.isPointerOn)
			{
				this.CreateRipple(Input.mousePosition);
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0005B860 File Offset: 0x00059A60
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableButtonSounds && this.useHoverSound && this.buttonVar.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
			this.hoverEvent.Invoke();
			this.isPointerOn = true;
			if (this.animationSolution == ButtonManagerIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeIn");
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0005B8CF File Offset: 0x00059ACF
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerOn = false;
			if (this.animationSolution == ButtonManagerIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeOut");
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0005B8FA File Offset: 0x00059AFA
		public void OnSelect(BaseEventData eventData)
		{
			if (this.animationSolution == ButtonManagerIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeIn");
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0005B91E File Offset: 0x00059B1E
		public void OnDeselect(BaseEventData eventData)
		{
			if (this.animationSolution == ButtonManagerIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeOut");
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0005B942 File Offset: 0x00059B42
		private IEnumerator FadeIn()
		{
			base.StopCoroutine("FadeOut");
			this.currentNormalValue = this.normalCG.alpha;
			this.currenthighlightedValue = this.highlightedCG.alpha;
			while (this.currenthighlightedValue <= 1f)
			{
				this.currentNormalValue -= Time.unscaledDeltaTime * this.fadingMultiplier;
				this.normalCG.alpha = this.currentNormalValue;
				this.currenthighlightedValue += Time.unscaledDeltaTime * this.fadingMultiplier;
				this.highlightedCG.alpha = this.currenthighlightedValue;
				if (this.normalCG.alpha >= 1f)
				{
					base.StopCoroutine("FadeIn");
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0005B951 File Offset: 0x00059B51
		private IEnumerator FadeOut()
		{
			base.StopCoroutine("FadeIn");
			this.currentNormalValue = this.normalCG.alpha;
			this.currenthighlightedValue = this.highlightedCG.alpha;
			while (this.currentNormalValue >= 0f)
			{
				this.currentNormalValue += Time.unscaledDeltaTime * this.fadingMultiplier;
				this.normalCG.alpha = this.currentNormalValue;
				this.currenthighlightedValue -= Time.unscaledDeltaTime * this.fadingMultiplier;
				this.highlightedCG.alpha = this.currenthighlightedValue;
				if (this.highlightedCG.alpha <= 0f)
				{
					base.StopCoroutine("FadeOut");
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04001195 RID: 4501
		public Sprite buttonIcon;

		// Token: 0x04001196 RID: 4502
		public UnityEvent clickEvent;

		// Token: 0x04001197 RID: 4503
		public UnityEvent hoverEvent;

		// Token: 0x04001198 RID: 4504
		public AudioClip hoverSound;

		// Token: 0x04001199 RID: 4505
		public AudioClip clickSound;

		// Token: 0x0400119A RID: 4506
		public Button buttonVar;

		// Token: 0x0400119B RID: 4507
		public Image normalIcon;

		// Token: 0x0400119C RID: 4508
		public Image highlightedIcon;

		// Token: 0x0400119D RID: 4509
		public AudioSource soundSource;

		// Token: 0x0400119E RID: 4510
		public GameObject rippleParent;

		// Token: 0x0400119F RID: 4511
		public ButtonManagerIcon.AnimationSolution animationSolution = ButtonManagerIcon.AnimationSolution.Script;

		// Token: 0x040011A0 RID: 4512
		[Range(0.25f, 15f)]
		public float fadingMultiplier = 8f;

		// Token: 0x040011A1 RID: 4513
		public bool useCustomContent;

		// Token: 0x040011A2 RID: 4514
		public bool enableButtonSounds;

		// Token: 0x040011A3 RID: 4515
		public bool useHoverSound = true;

		// Token: 0x040011A4 RID: 4516
		public bool useClickSound = true;

		// Token: 0x040011A5 RID: 4517
		public bool useRipple = true;

		// Token: 0x040011A6 RID: 4518
		public ButtonManagerIcon.RippleUpdateMode rippleUpdateMode = ButtonManagerIcon.RippleUpdateMode.UnscaledTime;

		// Token: 0x040011A7 RID: 4519
		public Sprite rippleShape;

		// Token: 0x040011A8 RID: 4520
		[Range(0.1f, 5f)]
		public float speed = 1f;

		// Token: 0x040011A9 RID: 4521
		[Range(0.5f, 25f)]
		public float maxSize = 4f;

		// Token: 0x040011AA RID: 4522
		public Color startColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x040011AB RID: 4523
		public Color transitionColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x040011AC RID: 4524
		public bool renderOnTop;

		// Token: 0x040011AD RID: 4525
		public bool centered;

		// Token: 0x040011AE RID: 4526
		private bool isPointerOn;

		// Token: 0x040011AF RID: 4527
		public bool isPreset;

		// Token: 0x040011B0 RID: 4528
		private float currentNormalValue;

		// Token: 0x040011B1 RID: 4529
		private float currenthighlightedValue;

		// Token: 0x040011B2 RID: 4530
		private CanvasGroup normalCG;

		// Token: 0x040011B3 RID: 4531
		private CanvasGroup highlightedCG;

		// Token: 0x020002C8 RID: 712
		public enum AnimationSolution
		{
			// Token: 0x040011B5 RID: 4533
			Animator,
			// Token: 0x040011B6 RID: 4534
			Script
		}

		// Token: 0x020002C9 RID: 713
		public enum RippleUpdateMode
		{
			// Token: 0x040011B8 RID: 4536
			Normal,
			// Token: 0x040011B9 RID: 4537
			UnscaledTime
		}
	}
}
