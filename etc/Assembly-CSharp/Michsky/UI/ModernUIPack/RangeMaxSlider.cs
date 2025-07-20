using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200032C RID: 812
	public class RangeMaxSlider : Slider
	{
		// Token: 0x060015D4 RID: 5588 RVA: 0x000646B0 File Offset: 0x000628B0
		protected override void Start()
		{
			this.realValue = base.maxValue;
			base.Start();
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x000646C4 File Offset: 0x000628C4
		protected override void Set(float input, bool sendCallback)
		{
			if (this.minSlider == null)
			{
				this.minSlider = base.transform.parent.Find("Min Slider").GetComponent<RangeMinSlider>();
			}
			if (!this.assignedRealValue)
			{
				this.realValue = base.maxValue;
				this.assignedRealValue = true;
			}
			else
			{
				this.realValue = base.maxValue - input + base.minValue;
			}
			if (base.wholeNumbers)
			{
				this.realValue = Mathf.Round(this.realValue);
			}
			if (this.realValue <= this.minSlider.value)
			{
				return;
			}
			if (this.label != null)
			{
				this.label.text = this.realValue.ToString(this.numberFormat);
			}
			base.Set(input, sendCallback);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00064790 File Offset: 0x00062990
		public void Refresh(float input)
		{
			this.Set(input, false);
		}

		// Token: 0x0400140D RID: 5133
		public RangeMinSlider minSlider;

		// Token: 0x0400140E RID: 5134
		public TextMeshProUGUI label;

		// Token: 0x0400140F RID: 5135
		public string numberFormat;

		// Token: 0x04001410 RID: 5136
		public float realValue;

		// Token: 0x04001411 RID: 5137
		private bool assignedRealValue;
	}
}
