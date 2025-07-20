using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000329 RID: 809
	public class RadialSlider : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x00064337 File Offset: 0x00062537
		// (set) Token: 0x060015C1 RID: 5569 RVA: 0x0006433F File Offset: 0x0006253F
		public float SliderAngle
		{
			get
			{
				return this.currentAngle;
			}
			set
			{
				this.currentAngle = Mathf.Clamp(value, 0f, 360f);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x00064357 File Offset: 0x00062557
		// (set) Token: 0x060015C3 RID: 5571 RVA: 0x0006436F File Offset: 0x0006256F
		public float SliderValue
		{
			get
			{
				return (float)((long)(this.SliderValueRaw * this.valueDisplayPrecision)) / this.valueDisplayPrecision;
			}
			set
			{
				this.SliderValueRaw = value;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00064378 File Offset: 0x00062578
		// (set) Token: 0x060015C5 RID: 5573 RVA: 0x0006438D File Offset: 0x0006258D
		public float SliderValueRaw
		{
			get
			{
				return this.SliderAngle / 360f * this.maxValue;
			}
			set
			{
				this.SliderAngle = value * 360f / this.maxValue;
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x000643A3 File Offset: 0x000625A3
		private void Awake()
		{
			this.graphicRaycaster = base.GetComponentInParent<GraphicRaycaster>();
			if (this.graphicRaycaster == null)
			{
				Debug.LogWarning("<b>[Radial Slider]</b> Could not find GraphicRaycaster component in parent.", this);
			}
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x000643CC File Offset: 0x000625CC
		private void Start()
		{
			this.valueDisplayPrecision = Mathf.Pow(10f, (float)this.decimals);
			if (this.rememberValue)
			{
				this.LoadState();
			}
			else
			{
				this.SliderAngle = this.currentValue * 3.6f;
			}
			this.SliderValue = this.currentValue;
			this.onValueChanged.Invoke(this.SliderValueRaw);
			this.UpdateUI();
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00064438 File Offset: 0x00062638
		public void OnPointerDown(PointerEventData eventData)
		{
			this.hitRectTransform = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();
			this.isPointerDown = true;
			this.currentAngleOnPointerDown = this.SliderAngle;
			this.HandleSliderMouseInput(eventData, true);
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00064479 File Offset: 0x00062679
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.HasValueChanged())
			{
				this.SaveState();
			}
			this.hitRectTransform = null;
			this.isPointerDown = false;
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00064497 File Offset: 0x00062697
		public void OnDrag(PointerEventData eventData)
		{
			if (this.currentValue >= this.minValue)
			{
				this.HandleSliderMouseInput(eventData, false);
				return;
			}
			if (this.currentValue <= this.minValue)
			{
				this.SliderValueRaw = this.minValue;
			}
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x000644CA File Offset: 0x000626CA
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.onPointerEnter.Invoke();
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x000644D7 File Offset: 0x000626D7
		public void OnPointerExit(PointerEventData eventData)
		{
			this.onPointerExit.Invoke();
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x000644E4 File Offset: 0x000626E4
		public void LoadState()
		{
			this.currentAngle = PlayerPrefs.GetFloat(this.sliderTag + "Radial");
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00064501 File Offset: 0x00062701
		public void SaveState()
		{
			if (!this.rememberValue)
			{
				return;
			}
			PlayerPrefs.SetFloat(this.sliderTag + "Radial", this.currentAngle);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00064528 File Offset: 0x00062728
		public void UpdateUI()
		{
			if (this.SliderValueRaw >= this.minValue)
			{
				float fillAmount = this.SliderAngle / 360f;
				this.indicatorPivot.transform.localEulerAngles = new Vector3(180f, 0f, this.SliderAngle);
				this.sliderImage.fillAmount = fillAmount;
				this.valueText.text = string.Format("{0}{1}", this.SliderValue, this.isPercent ? "%" : "");
				this.currentValue = this.SliderValue;
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x000645C1 File Offset: 0x000627C1
		private bool HasValueChanged()
		{
			return this.SliderAngle != this.currentAngleOnPointerDown;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x000645D4 File Offset: 0x000627D4
		private void HandleSliderMouseInput(PointerEventData eventData, bool allowValueWrap)
		{
			if (!this.isPointerDown)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.hitRectTransform, eventData.position, eventData.pressEventCamera, out vector);
			float num = Mathf.Atan2(-vector.y, vector.x) * 57.29578f + 180f;
			if (!allowValueWrap)
			{
				this.currentAngle = this.SliderAngle;
				if (Mathf.Abs(num - this.currentAngle) >= 180f)
				{
					num = ((this.currentAngle < num) ? 0f : 360f);
				}
			}
			this.SliderAngle = num;
			this.UpdateUI();
			if (this.HasValueChanged())
			{
				this.onValueChanged.Invoke(this.SliderValueRaw);
			}
		}

		// Token: 0x040013F3 RID: 5107
		private const string PREFS_UI_SAVE_NAME = "Radial";

		// Token: 0x040013F4 RID: 5108
		public float currentValue = 50f;

		// Token: 0x040013F5 RID: 5109
		public Image sliderImage;

		// Token: 0x040013F6 RID: 5110
		public Transform indicatorPivot;

		// Token: 0x040013F7 RID: 5111
		public TextMeshProUGUI valueText;

		// Token: 0x040013F8 RID: 5112
		public float minValue;

		// Token: 0x040013F9 RID: 5113
		public float maxValue = 100f;

		// Token: 0x040013FA RID: 5114
		[Range(0f, 8f)]
		public int decimals;

		// Token: 0x040013FB RID: 5115
		public bool isPercent;

		// Token: 0x040013FC RID: 5116
		public RadialSlider.StartPoint startPoint;

		// Token: 0x040013FD RID: 5117
		public bool rememberValue;

		// Token: 0x040013FE RID: 5118
		public string sliderTag;

		// Token: 0x040013FF RID: 5119
		[SerializeField]
		private RadialSlider.SliderEvent onValueChanged = new RadialSlider.SliderEvent();

		// Token: 0x04001400 RID: 5120
		public UnityEvent onPointerEnter;

		// Token: 0x04001401 RID: 5121
		public UnityEvent onPointerExit;

		// Token: 0x04001402 RID: 5122
		private GraphicRaycaster graphicRaycaster;

		// Token: 0x04001403 RID: 5123
		private RectTransform hitRectTransform;

		// Token: 0x04001404 RID: 5124
		private bool isPointerDown;

		// Token: 0x04001405 RID: 5125
		private float currentAngle;

		// Token: 0x04001406 RID: 5126
		private float currentAngleOnPointerDown;

		// Token: 0x04001407 RID: 5127
		private float valueDisplayPrecision;

		// Token: 0x0200032A RID: 810
		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}

		// Token: 0x0200032B RID: 811
		public enum StartPoint
		{
			// Token: 0x04001409 RID: 5129
			Left,
			// Token: 0x0400140A RID: 5130
			Right,
			// Token: 0x0400140B RID: 5131
			Top,
			// Token: 0x0400140C RID: 5132
			Down
		}
	}
}
