using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002CC RID: 716
	[ExecuteInEditMode]
	[RequireComponent(typeof(Button))]
	public class ButtonManagerWithIcon : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x06001416 RID: 5142 RVA: 0x0005BC35 File Offset: 0x00059E35
		private void OnEnable()
		{
			if (this.normalCG == null && this.highlightedCG == null)
			{
				return;
			}
			this.normalCG.alpha = 1f;
			this.highlightedCG.alpha = 0f;
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0005BC74 File Offset: 0x00059E74
		private void Awake()
		{
			if (this.animationSolution == ButtonManagerWithIcon.AnimationSolution.Script)
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

		// Token: 0x06001418 RID: 5144 RVA: 0x0005BD84 File Offset: 0x00059F84
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
			if (this.normalText != null)
			{
				this.normalText.text = this.buttonText;
			}
			if (this.highlightedText != null)
			{
				this.highlightedText.text = this.buttonText;
			}
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0005BE10 File Offset: 0x0005A010
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
				if (this.rippleUpdateMode == ButtonManagerWithIcon.RippleUpdateMode.Normal)
				{
					component.unscaledTime = false;
					return;
				}
				component.unscaledTime = true;
			}
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0005BF32 File Offset: 0x0005A132
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.useRipple && this.isPointerOn)
			{
				this.CreateRipple(Input.mousePosition);
			}
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0005BF54 File Offset: 0x0005A154
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableButtonSounds && this.useHoverSound && this.buttonVar.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
			this.hoverEvent.Invoke();
			this.isPointerOn = true;
			if (this.animationSolution == ButtonManagerWithIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeIn");
			}
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0005BFC3 File Offset: 0x0005A1C3
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerOn = false;
			if (this.animationSolution == ButtonManagerWithIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeOut");
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0005BFEE File Offset: 0x0005A1EE
		public void OnSelect(BaseEventData eventData)
		{
			if (this.animationSolution == ButtonManagerWithIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeIn");
			}
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0005C012 File Offset: 0x0005A212
		public void OnDeselect(BaseEventData eventData)
		{
			if (this.animationSolution == ButtonManagerWithIcon.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeOut");
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0005C036 File Offset: 0x0005A236
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

		// Token: 0x06001420 RID: 5152 RVA: 0x0005C045 File Offset: 0x0005A245
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

		// Token: 0x040011C0 RID: 4544
		public Sprite buttonIcon;

		// Token: 0x040011C1 RID: 4545
		public string buttonText = "Button";

		// Token: 0x040011C2 RID: 4546
		public UnityEvent clickEvent;

		// Token: 0x040011C3 RID: 4547
		public UnityEvent hoverEvent;

		// Token: 0x040011C4 RID: 4548
		public AudioClip hoverSound;

		// Token: 0x040011C5 RID: 4549
		public AudioClip clickSound;

		// Token: 0x040011C6 RID: 4550
		public Button buttonVar;

		// Token: 0x040011C7 RID: 4551
		public Image normalIcon;

		// Token: 0x040011C8 RID: 4552
		public Image highlightedIcon;

		// Token: 0x040011C9 RID: 4553
		public TextMeshProUGUI normalText;

		// Token: 0x040011CA RID: 4554
		public TextMeshProUGUI highlightedText;

		// Token: 0x040011CB RID: 4555
		public AudioSource soundSource;

		// Token: 0x040011CC RID: 4556
		public GameObject rippleParent;

		// Token: 0x040011CD RID: 4557
		public ButtonManagerWithIcon.AnimationSolution animationSolution = ButtonManagerWithIcon.AnimationSolution.Script;

		// Token: 0x040011CE RID: 4558
		[Range(0.25f, 15f)]
		public float fadingMultiplier = 8f;

		// Token: 0x040011CF RID: 4559
		public bool useCustomContent;

		// Token: 0x040011D0 RID: 4560
		public bool enableButtonSounds;

		// Token: 0x040011D1 RID: 4561
		public bool useHoverSound = true;

		// Token: 0x040011D2 RID: 4562
		public bool useClickSound = true;

		// Token: 0x040011D3 RID: 4563
		public bool useRipple = true;

		// Token: 0x040011D4 RID: 4564
		public ButtonManagerWithIcon.RippleUpdateMode rippleUpdateMode = ButtonManagerWithIcon.RippleUpdateMode.UnscaledTime;

		// Token: 0x040011D5 RID: 4565
		public Sprite rippleShape;

		// Token: 0x040011D6 RID: 4566
		[Range(0.1f, 5f)]
		public float speed = 1f;

		// Token: 0x040011D7 RID: 4567
		[Range(0.5f, 25f)]
		public float maxSize = 4f;

		// Token: 0x040011D8 RID: 4568
		public Color startColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x040011D9 RID: 4569
		public Color transitionColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x040011DA RID: 4570
		public bool renderOnTop;

		// Token: 0x040011DB RID: 4571
		public bool centered;

		// Token: 0x040011DC RID: 4572
		private bool isPointerOn;

		// Token: 0x040011DD RID: 4573
		public bool isPreset;

		// Token: 0x040011DE RID: 4574
		private float currentNormalValue;

		// Token: 0x040011DF RID: 4575
		private float currenthighlightedValue;

		// Token: 0x040011E0 RID: 4576
		private CanvasGroup normalCG;

		// Token: 0x040011E1 RID: 4577
		private CanvasGroup highlightedCG;

		// Token: 0x020002CD RID: 717
		public enum AnimationSolution
		{
			// Token: 0x040011E3 RID: 4579
			Animator,
			// Token: 0x040011E4 RID: 4580
			Script
		}

		// Token: 0x020002CE RID: 718
		public enum RippleUpdateMode
		{
			// Token: 0x040011E6 RID: 4582
			Normal,
			// Token: 0x040011E7 RID: 4583
			UnscaledTime
		}
	}
}
