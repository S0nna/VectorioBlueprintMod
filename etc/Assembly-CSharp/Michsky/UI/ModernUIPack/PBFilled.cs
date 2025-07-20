using System;
using TMPro;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000321 RID: 801
	public class PBFilled : MonoBehaviour
	{
		// Token: 0x0600159A RID: 5530 RVA: 0x00062E68 File Offset: 0x00061068
		private void Start()
		{
			this.progressBar = base.gameObject.GetComponent<ProgressBar>();
			this.barAnimatior = base.gameObject.GetComponent<Animator>();
			this.minLabel.color = this.minColor;
			this.maxLabel.color = this.maxColor;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00062EBC File Offset: 0x000610BC
		private void Update()
		{
			if (this.progressBar.currentPercent >= (float)this.transitionAfter)
			{
				this.barAnimatior.Play("Radial PB Filled");
			}
			if (this.progressBar.currentPercent <= (float)this.transitionAfter)
			{
				this.barAnimatior.Play("Radial PB Empty");
			}
			this.maxLabel.text = this.minLabel.text;
		}

		// Token: 0x040013C3 RID: 5059
		[Header("Resources")]
		public TextMeshProUGUI minLabel;

		// Token: 0x040013C4 RID: 5060
		public TextMeshProUGUI maxLabel;

		// Token: 0x040013C5 RID: 5061
		[Header("Settings")]
		[Range(0f, 100f)]
		public int transitionAfter = 50;

		// Token: 0x040013C6 RID: 5062
		public Color minColor = new Color(0f, 0f, 0f, 255f);

		// Token: 0x040013C7 RID: 5063
		public Color maxColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040013C8 RID: 5064
		private ProgressBar progressBar;

		// Token: 0x040013C9 RID: 5065
		private Animator barAnimatior;
	}
}
