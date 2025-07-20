using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002BC RID: 700
	[ExecuteInEditMode]
	[RequireComponent(typeof(Button))]
	public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x060013C7 RID: 5063 RVA: 0x0005A524 File Offset: 0x00058724
		private void OnEnable()
		{
			if (this.normalCG == null && this.highlightedCG == null)
			{
				return;
			}
			this.normalCG.alpha = 1f;
			this.highlightedCG.alpha = 0f;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0005A564 File Offset: 0x00058764
		private void Awake()
		{
			if (this.animationSolution == ButtonManager.AnimationSolution.Script)
			{
				this.normalCG = base.transform.Find("Normal").GetComponent<CanvasGroup>();
				this.highlightedCG = base.transform.Find("Highlighted").GetComponent<CanvasGroup>();
				Object.Destroy(base.GetComponent<Animator>());
			}
			if (this.buttonVar == null)
			{
				this.buttonVar = base.gameObject.GetComponent<Button>();
			}
			this.buttonVar.onClick.AddListener(delegate()
			{
				this.clickEvent.Invoke();
			});
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

		// Token: 0x060013C9 RID: 5065 RVA: 0x0005A674 File Offset: 0x00058874
		public void UpdateUI()
		{
			if (this.normalText != null)
			{
				this.normalText.text = this.buttonText;
			}
			if (this.normalText != null)
			{
				this.highlightedText.text = this.buttonText;
			}
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0005A6B4 File Offset: 0x000588B4
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
				if (this.rippleUpdateMode == ButtonManager.RippleUpdateMode.Normal)
				{
					component.unscaledTime = false;
					return;
				}
				component.unscaledTime = true;
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0005A7D6 File Offset: 0x000589D6
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.useRipple && this.isPointerOn)
			{
				this.CreateRipple(Input.mousePosition);
			}
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0005A7F8 File Offset: 0x000589F8
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableButtonSounds && this.useHoverSound && this.buttonVar.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
			this.hoverEvent.Invoke();
			this.isPointerOn = true;
			if (this.animationSolution == ButtonManager.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeIn");
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0005A867 File Offset: 0x00058A67
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerOn = false;
			if (this.animationSolution == ButtonManager.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeOut");
			}
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0005A892 File Offset: 0x00058A92
		public void OnSelect(BaseEventData eventData)
		{
			if (this.animationSolution == ButtonManager.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeIn");
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0005A8B6 File Offset: 0x00058AB6
		public void OnDeselect(BaseEventData eventData)
		{
			if (this.animationSolution == ButtonManager.AnimationSolution.Script && this.buttonVar.interactable)
			{
				base.StartCoroutine("FadeOut");
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0005A8DA File Offset: 0x00058ADA
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

		// Token: 0x060013D1 RID: 5073 RVA: 0x0005A8E9 File Offset: 0x00058AE9
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

		// Token: 0x04001117 RID: 4375
		public string buttonText = "Button";

		// Token: 0x04001118 RID: 4376
		public UnityEvent clickEvent;

		// Token: 0x04001119 RID: 4377
		public UnityEvent hoverEvent;

		// Token: 0x0400111A RID: 4378
		public AudioClip hoverSound;

		// Token: 0x0400111B RID: 4379
		public AudioClip clickSound;

		// Token: 0x0400111C RID: 4380
		public Button buttonVar;

		// Token: 0x0400111D RID: 4381
		public TextMeshProUGUI normalText;

		// Token: 0x0400111E RID: 4382
		public TextMeshProUGUI highlightedText;

		// Token: 0x0400111F RID: 4383
		public AudioSource soundSource;

		// Token: 0x04001120 RID: 4384
		public GameObject rippleParent;

		// Token: 0x04001121 RID: 4385
		public ButtonManager.AnimationSolution animationSolution = ButtonManager.AnimationSolution.Script;

		// Token: 0x04001122 RID: 4386
		[Range(0.25f, 15f)]
		public float fadingMultiplier = 8f;

		// Token: 0x04001123 RID: 4387
		public bool useCustomContent;

		// Token: 0x04001124 RID: 4388
		public bool enableButtonSounds;

		// Token: 0x04001125 RID: 4389
		public bool useHoverSound = true;

		// Token: 0x04001126 RID: 4390
		public bool useClickSound = true;

		// Token: 0x04001127 RID: 4391
		public bool useRipple = true;

		// Token: 0x04001128 RID: 4392
		public ButtonManager.RippleUpdateMode rippleUpdateMode = ButtonManager.RippleUpdateMode.UnscaledTime;

		// Token: 0x04001129 RID: 4393
		public Sprite rippleShape;

		// Token: 0x0400112A RID: 4394
		[Range(0.1f, 5f)]
		public float speed = 1f;

		// Token: 0x0400112B RID: 4395
		[Range(0.5f, 25f)]
		public float maxSize = 4f;

		// Token: 0x0400112C RID: 4396
		public Color startColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400112D RID: 4397
		public Color transitionColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400112E RID: 4398
		public bool renderOnTop;

		// Token: 0x0400112F RID: 4399
		public bool centered;

		// Token: 0x04001130 RID: 4400
		private bool isPointerOn;

		// Token: 0x04001131 RID: 4401
		public bool isPreset;

		// Token: 0x04001132 RID: 4402
		private float currentNormalValue;

		// Token: 0x04001133 RID: 4403
		private float currenthighlightedValue;

		// Token: 0x04001134 RID: 4404
		private CanvasGroup normalCG;

		// Token: 0x04001135 RID: 4405
		private CanvasGroup highlightedCG;

		// Token: 0x020002BD RID: 701
		public enum AnimationSolution
		{
			// Token: 0x04001137 RID: 4407
			Animator,
			// Token: 0x04001138 RID: 4408
			Script
		}

		// Token: 0x020002BE RID: 702
		public enum RippleUpdateMode
		{
			// Token: 0x0400113A RID: 4410
			Normal,
			// Token: 0x0400113B RID: 4411
			UnscaledTime
		}
	}
}
