using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002C3 RID: 707
	[ExecuteInEditMode]
	[RequireComponent(typeof(Button))]
	public class ButtonManagerBasicIcon : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler
	{
		// Token: 0x060013EA RID: 5098 RVA: 0x0005AF10 File Offset: 0x00059110
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

		// Token: 0x060013EB RID: 5099 RVA: 0x0005AFD6 File Offset: 0x000591D6
		public void UpdateUI()
		{
			if (this.normalIcon != null)
			{
				this.normalIcon.sprite = this.buttonIcon;
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0005AFF8 File Offset: 0x000591F8
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
				if (this.rippleUpdateMode == ButtonManagerBasicIcon.RippleUpdateMode.Normal)
				{
					component.unscaledTime = false;
					return;
				}
				component.unscaledTime = true;
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0005B11A File Offset: 0x0005931A
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.useRipple && this.isPointerOn)
			{
				this.CreateRipple(Input.mousePosition);
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0005B13C File Offset: 0x0005933C
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableButtonSounds && this.useHoverSound && this.buttonVar.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
			this.hoverEvent.Invoke();
			this.isPointerOn = true;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0005B189 File Offset: 0x00059389
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerOn = false;
		}

		// Token: 0x0400115D RID: 4445
		public Sprite buttonIcon;

		// Token: 0x0400115E RID: 4446
		public UnityEvent clickEvent;

		// Token: 0x0400115F RID: 4447
		public UnityEvent hoverEvent;

		// Token: 0x04001160 RID: 4448
		public AudioClip hoverSound;

		// Token: 0x04001161 RID: 4449
		public AudioClip clickSound;

		// Token: 0x04001162 RID: 4450
		public Button buttonVar;

		// Token: 0x04001163 RID: 4451
		public Image normalIcon;

		// Token: 0x04001164 RID: 4452
		public AudioSource soundSource;

		// Token: 0x04001165 RID: 4453
		public GameObject rippleParent;

		// Token: 0x04001166 RID: 4454
		public bool useCustomContent;

		// Token: 0x04001167 RID: 4455
		public bool enableButtonSounds;

		// Token: 0x04001168 RID: 4456
		public bool useHoverSound = true;

		// Token: 0x04001169 RID: 4457
		public bool useClickSound = true;

		// Token: 0x0400116A RID: 4458
		public bool useRipple = true;

		// Token: 0x0400116B RID: 4459
		public ButtonManagerBasicIcon.RippleUpdateMode rippleUpdateMode = ButtonManagerBasicIcon.RippleUpdateMode.UnscaledTime;

		// Token: 0x0400116C RID: 4460
		public Sprite rippleShape;

		// Token: 0x0400116D RID: 4461
		[Range(0.1f, 5f)]
		public float speed = 1f;

		// Token: 0x0400116E RID: 4462
		[Range(0.5f, 25f)]
		public float maxSize = 4f;

		// Token: 0x0400116F RID: 4463
		public Color startColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04001170 RID: 4464
		public Color transitionColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04001171 RID: 4465
		public bool renderOnTop;

		// Token: 0x04001172 RID: 4466
		public bool centered;

		// Token: 0x04001173 RID: 4467
		private bool isPointerOn;

		// Token: 0x04001174 RID: 4468
		public bool isPreset;

		// Token: 0x020002C4 RID: 708
		public enum RippleUpdateMode
		{
			// Token: 0x04001176 RID: 4470
			Normal,
			// Token: 0x04001177 RID: 4471
			UnscaledTime
		}
	}
}
