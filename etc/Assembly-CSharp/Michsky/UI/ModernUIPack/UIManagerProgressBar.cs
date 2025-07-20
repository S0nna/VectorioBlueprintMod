using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200034D RID: 845
	[ExecuteInEditMode]
	public class UIManagerProgressBar : MonoBehaviour
	{
		// Token: 0x0600164C RID: 5708 RVA: 0x00067DF4 File Offset: 0x00065FF4
		private void Awake()
		{
			try
			{
				if (this.UIManagerAsset == null)
				{
					this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
				}
				base.enabled = true;
				if (!this.UIManagerAsset.enableDynamicUpdate)
				{
					this.UpdateProgressBar();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00067E60 File Offset: 0x00066060
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateProgressBar();
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00067E84 File Offset: 0x00066084
		private void UpdateProgressBar()
		{
			try
			{
				if (!this.overrideColors)
				{
					this.bar.color = this.UIManagerAsset.progressBarColor;
					this.background.color = this.UIManagerAsset.progressBarBackgroundColor;
					this.label.color = this.UIManagerAsset.progressBarLabelColor;
				}
				if (!this.overrideFonts)
				{
					this.label.font = this.UIManagerAsset.progressBarLabelFont;
					this.label.fontSize = this.UIManagerAsset.progressBarLabelFontSize;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04001536 RID: 5430
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001537 RID: 5431
		public bool overrideColors;

		// Token: 0x04001538 RID: 5432
		public bool overrideFonts;

		// Token: 0x04001539 RID: 5433
		[Header("Resources")]
		[SerializeField]
		private Image bar;

		// Token: 0x0400153A RID: 5434
		[SerializeField]
		private Image background;

		// Token: 0x0400153B RID: 5435
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x0400153C RID: 5436
		private bool dynamicUpdateEnabled;
	}
}
