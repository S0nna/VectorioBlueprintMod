using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200032D RID: 813
	public class RangeMinSlider : Slider
	{
		// Token: 0x060015D8 RID: 5592 RVA: 0x000647A4 File Offset: 0x000629A4
		protected override void Set(float input, bool sendCallback)
		{
			if (this.maxSlider == null)
			{
				this.maxSlider = base.transform.parent.Find("Max Slider").GetComponent<RangeMaxSlider>();
			}
			float num = input;
			if (base.wholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (num >= this.maxSlider.realValue && this.maxSlider.realValue != this.maxSlider.minValue)
			{
				return;
			}
			if (this.label != null)
			{
				this.label.text = num.ToString(this.numberFormat);
			}
			base.Set(input, sendCallback);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00064790 File Offset: 0x00062990
		public void Refresh(float input)
		{
			this.Set(input, false);
		}

		// Token: 0x04001412 RID: 5138
		[Header("RESOURCES")]
		public RangeMaxSlider maxSlider;

		// Token: 0x04001413 RID: 5139
		public TextMeshProUGUI label;

		// Token: 0x04001414 RID: 5140
		public string numberFormat;
	}
}
