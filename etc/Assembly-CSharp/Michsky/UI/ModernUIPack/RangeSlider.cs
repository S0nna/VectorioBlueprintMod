using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200032E RID: 814
	public class RangeSlider : MonoBehaviour
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x00064845 File Offset: 0x00062A45
		public float CurrentLowerValue
		{
			get
			{
				return this.minSlider.value;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x00064852 File Offset: 0x00062A52
		public float CurrentUpperValue
		{
			get
			{
				return this.maxSlider.realValue;
			}
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x00064860 File Offset: 0x00062A60
		private void Awake()
		{
			if (this.minSlider == null || this.maxSlider == null)
			{
				return;
			}
			if (this.showLabels)
			{
				this.minSlider.label = this.minSliderLabel;
				this.minSlider.numberFormat = "n" + this.decimalPlaces.ToString();
				this.maxSlider.label = this.maxSliderLabel;
				this.maxSlider.numberFormat = "n" + this.decimalPlaces.ToString();
			}
			else
			{
				this.minSliderLabel.gameObject.SetActive(false);
				this.maxSliderLabel.gameObject.SetActive(false);
			}
			if (this.useWholeNumbers)
			{
				this.minSlider.wholeNumbers = true;
				this.maxSlider.wholeNumbers = true;
			}
			this.minSlider.minValue = this.minValue;
			this.minSlider.maxValue = this.maxValue;
			this.minSlider.onValueChanged.AddListener(new UnityAction<float>(this.CheckForMinState));
			this.maxSlider.minValue = this.minValue;
			this.maxSlider.maxValue = this.maxValue;
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x00064998 File Offset: 0x00062B98
		public void CheckForMinState(float value)
		{
			if (this.minSlider.value >= this.maxSlider.realValue)
			{
				this.maxSlider.realValue = this.minSlider.value;
				this.minSlider.value = this.maxSlider.realValue - 1f;
			}
		}

		// Token: 0x04001415 RID: 5141
		[Header("Settings")]
		[Range(0f, 2f)]
		public int decimalPlaces;

		// Token: 0x04001416 RID: 5142
		public float minValue;

		// Token: 0x04001417 RID: 5143
		public float maxValue = 1f;

		// Token: 0x04001418 RID: 5144
		public bool showLabels = true;

		// Token: 0x04001419 RID: 5145
		public bool useWholeNumbers = true;

		// Token: 0x0400141A RID: 5146
		[Header("Min Slider")]
		public RangeMinSlider minSlider;

		// Token: 0x0400141B RID: 5147
		public TextMeshProUGUI minSliderLabel;

		// Token: 0x0400141C RID: 5148
		[Header("Max Slider")]
		public RangeMaxSlider maxSlider;

		// Token: 0x0400141D RID: 5149
		public TextMeshProUGUI maxSliderLabel;
	}
}
