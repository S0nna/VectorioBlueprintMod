using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002C1 RID: 705
	[ExecuteInEditMode]
	[RequireComponent(typeof(Button))]
	public class ButtonManagerBasic : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler
	{
		// Token: 0x060013E1 RID: 5089 RVA: 0x0005ABDC File Offset: 0x00058DDC
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

		// Token: 0x060013E2 RID: 5090 RVA: 0x0005ACA2 File Offset: 0x00058EA2
		public void UpdateUI()
		{
			if (this.normalText != null)
			{
				this.normalText.text = this.buttonText;
			}
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0005ACC4 File Offset: 0x00058EC4
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
				if (this.rippleUpdateMode == ButtonManagerBasic.RippleUpdateMode.Normal)
				{
					component.unscaledTime = false;
					return;
				}
				component.unscaledTime = true;
			}
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0005ADE6 File Offset: 0x00058FE6
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.useRipple && this.isPointerOn)
			{
				this.CreateRipple(Input.mousePosition);
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0005AE08 File Offset: 0x00059008
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableButtonSounds && this.useHoverSound && this.buttonVar.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
			this.hoverEvent.Invoke();
			this.isPointerOn = true;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0005AE55 File Offset: 0x00059055
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerOn = false;
		}

		// Token: 0x04001142 RID: 4418
		public string buttonText = "Button";

		// Token: 0x04001143 RID: 4419
		public UnityEvent clickEvent;

		// Token: 0x04001144 RID: 4420
		public UnityEvent hoverEvent;

		// Token: 0x04001145 RID: 4421
		public AudioClip hoverSound;

		// Token: 0x04001146 RID: 4422
		public AudioClip clickSound;

		// Token: 0x04001147 RID: 4423
		public Button buttonVar;

		// Token: 0x04001148 RID: 4424
		public TextMeshProUGUI normalText;

		// Token: 0x04001149 RID: 4425
		public AudioSource soundSource;

		// Token: 0x0400114A RID: 4426
		public GameObject rippleParent;

		// Token: 0x0400114B RID: 4427
		public bool useCustomContent;

		// Token: 0x0400114C RID: 4428
		public bool enableButtonSounds;

		// Token: 0x0400114D RID: 4429
		public bool useHoverSound = true;

		// Token: 0x0400114E RID: 4430
		public bool useClickSound = true;

		// Token: 0x0400114F RID: 4431
		public bool useRipple = true;

		// Token: 0x04001150 RID: 4432
		public ButtonManagerBasic.RippleUpdateMode rippleUpdateMode = ButtonManagerBasic.RippleUpdateMode.UnscaledTime;

		// Token: 0x04001151 RID: 4433
		public Sprite rippleShape;

		// Token: 0x04001152 RID: 4434
		[Range(0.1f, 5f)]
		public float speed = 1f;

		// Token: 0x04001153 RID: 4435
		[Range(0.5f, 25f)]
		public float maxSize = 4f;

		// Token: 0x04001154 RID: 4436
		public Color startColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04001155 RID: 4437
		public Color transitionColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04001156 RID: 4438
		public bool renderOnTop;

		// Token: 0x04001157 RID: 4439
		public bool centered;

		// Token: 0x04001158 RID: 4440
		private bool isPointerOn;

		// Token: 0x04001159 RID: 4441
		public bool isPreset;

		// Token: 0x020002C2 RID: 706
		public enum RippleUpdateMode
		{
			// Token: 0x0400115B RID: 4443
			Normal,
			// Token: 0x0400115C RID: 4444
			UnscaledTime
		}
	}
}
