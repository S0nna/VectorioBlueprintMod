using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000350 RID: 848
	[ExecuteInEditMode]
	public class UIManagerSlider : MonoBehaviour
	{
		// Token: 0x06001658 RID: 5720 RVA: 0x000680C4 File Offset: 0x000662C4
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
					this.UpdateSlider();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00068130 File Offset: 0x00066330
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSlider();
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00068154 File Offset: 0x00066354
		private void UpdateSlider()
		{
			try
			{
				if (this.UIManagerAsset.sliderThemeType == UIManager.SliderThemeType.Basic)
				{
					if (!this.overrideColors)
					{
						this.background.color = this.UIManagerAsset.sliderBackgroundColor;
						this.bar.color = this.UIManagerAsset.sliderColor;
						this.handle.color = this.UIManagerAsset.sliderColor;
					}
					if (this.hasLabel)
					{
						if (!this.overrideColors)
						{
							this.label.color = new Color(this.UIManagerAsset.sliderColor.r, this.UIManagerAsset.sliderColor.g, this.UIManagerAsset.sliderColor.b, this.label.color.a);
						}
						if (!this.overrideFonts)
						{
							this.label.font = this.UIManagerAsset.sliderLabelFont;
							this.label.fontSize = this.UIManagerAsset.sliderLabelFontSize;
						}
					}
					if (this.hasPopupLabel)
					{
						if (!this.overrideColors)
						{
							this.popupLabel.color = new Color(this.UIManagerAsset.sliderPopupLabelColor.r, this.UIManagerAsset.sliderPopupLabelColor.g, this.UIManagerAsset.sliderPopupLabelColor.b, this.popupLabel.color.a);
						}
						if (!this.overrideFonts)
						{
							this.popupLabel.font = this.UIManagerAsset.sliderLabelFont;
						}
					}
				}
				else if (this.UIManagerAsset.sliderThemeType == UIManager.SliderThemeType.Custom)
				{
					if (!this.overrideColors)
					{
						this.background.color = this.UIManagerAsset.sliderBackgroundColor;
						this.bar.color = this.UIManagerAsset.sliderColor;
						this.handle.color = this.UIManagerAsset.sliderHandleColor;
					}
					if (this.hasLabel)
					{
						if (!this.overrideColors)
						{
							this.label.color = new Color(this.UIManagerAsset.sliderLabelColor.r, this.UIManagerAsset.sliderLabelColor.g, this.UIManagerAsset.sliderLabelColor.b, this.label.color.a);
						}
						if (!this.overrideFonts)
						{
							this.label.font = this.UIManagerAsset.sliderLabelFont;
							this.label.font = this.UIManagerAsset.sliderLabelFont;
						}
					}
					if (this.hasPopupLabel)
					{
						if (!this.overrideColors)
						{
							this.popupLabel.color = new Color(this.UIManagerAsset.sliderPopupLabelColor.r, this.UIManagerAsset.sliderPopupLabelColor.g, this.UIManagerAsset.sliderPopupLabelColor.b, this.popupLabel.color.a);
						}
						if (!this.overrideFonts)
						{
							this.popupLabel.font = this.UIManagerAsset.sliderLabelFont;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04001546 RID: 5446
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001547 RID: 5447
		public bool hasLabel;

		// Token: 0x04001548 RID: 5448
		public bool hasPopupLabel;

		// Token: 0x04001549 RID: 5449
		public bool overrideColors;

		// Token: 0x0400154A RID: 5450
		public bool overrideFonts;

		// Token: 0x0400154B RID: 5451
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x0400154C RID: 5452
		[SerializeField]
		private Image bar;

		// Token: 0x0400154D RID: 5453
		[SerializeField]
		private Image handle;

		// Token: 0x0400154E RID: 5454
		[HideInInspector]
		public TextMeshProUGUI label;

		// Token: 0x0400154F RID: 5455
		[HideInInspector]
		public TextMeshProUGUI popupLabel;
	}
}
