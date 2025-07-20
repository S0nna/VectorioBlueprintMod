using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002C5 RID: 709
	[ExecuteInEditMode]
	[RequireComponent(typeof(Button))]
	public class ButtonManagerBasicWithIcon : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler
	{
		// Token: 0x060013F3 RID: 5107 RVA: 0x0005B238 File Offset: 0x00059438
		private void Awake()
		{
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

		// Token: 0x060013F4 RID: 5108 RVA: 0x0005B2FE File Offset: 0x000594FE
		public void UpdateUI()
		{
			if (this.normalImage != null)
			{
				this.normalImage.sprite = this.buttonIcon;
			}
			if (this.normalText != null)
			{
				this.normalText.text = this.buttonText;
			}
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0005B340 File Offset: 0x00059540
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
				if (this.rippleUpdateMode == ButtonManagerBasicWithIcon.RippleUpdateMode.Normal)
				{
					component.unscaledTime = false;
					return;
				}
				component.unscaledTime = true;
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0005B462 File Offset: 0x00059662
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.useRipple && this.isPointerOn)
			{
				this.CreateRipple(Input.mousePosition);
			}
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0005B484 File Offset: 0x00059684
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableButtonSounds && this.useHoverSound && this.buttonVar.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
			this.hoverEvent.Invoke();
			this.isPointerOn = true;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0005B4D1 File Offset: 0x000596D1
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerOn = false;
		}

		// Token: 0x04001178 RID: 4472
		public Sprite buttonIcon;

		// Token: 0x04001179 RID: 4473
		public string buttonText = "Button";

		// Token: 0x0400117A RID: 4474
		public UnityEvent clickEvent;

		// Token: 0x0400117B RID: 4475
		public UnityEvent hoverEvent;

		// Token: 0x0400117C RID: 4476
		public AudioClip hoverSound;

		// Token: 0x0400117D RID: 4477
		public AudioClip clickSound;

		// Token: 0x0400117E RID: 4478
		public Button buttonVar;

		// Token: 0x0400117F RID: 4479
		public Image normalImage;

		// Token: 0x04001180 RID: 4480
		public TextMeshProUGUI normalText;

		// Token: 0x04001181 RID: 4481
		public AudioSource soundSource;

		// Token: 0x04001182 RID: 4482
		public GameObject rippleParent;

		// Token: 0x04001183 RID: 4483
		public bool useCustomContent;

		// Token: 0x04001184 RID: 4484
		public bool enableButtonSounds;

		// Token: 0x04001185 RID: 4485
		public bool useHoverSound = true;

		// Token: 0x04001186 RID: 4486
		public bool useClickSound = true;

		// Token: 0x04001187 RID: 4487
		public bool useRipple = true;

		// Token: 0x04001188 RID: 4488
		public ButtonManagerBasicWithIcon.RippleUpdateMode rippleUpdateMode = ButtonManagerBasicWithIcon.RippleUpdateMode.UnscaledTime;

		// Token: 0x04001189 RID: 4489
		public Sprite rippleShape;

		// Token: 0x0400118A RID: 4490
		[Range(0.1f, 5f)]
		public float speed = 1f;

		// Token: 0x0400118B RID: 4491
		[Range(0.5f, 25f)]
		public float maxSize = 4f;

		// Token: 0x0400118C RID: 4492
		public Color startColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400118D RID: 4493
		public Color transitionColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400118E RID: 4494
		public bool renderOnTop;

		// Token: 0x0400118F RID: 4495
		public bool centered;

		// Token: 0x04001190 RID: 4496
		private bool isPointerOn;

		// Token: 0x04001191 RID: 4497
		public bool isPreset;

		// Token: 0x020002C6 RID: 710
		public enum RippleUpdateMode
		{
			// Token: 0x04001193 RID: 4499
			Normal,
			// Token: 0x04001194 RID: 4500
			UnscaledTime
		}
	}
}
