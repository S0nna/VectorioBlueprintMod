using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000343 RID: 835
	[ExecuteInEditMode]
	public class UIManagerButton : MonoBehaviour
	{
		// Token: 0x0600162B RID: 5675 RVA: 0x000665D8 File Offset: 0x000647D8
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
					this.UpdateButton();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00066644 File Offset: 0x00064844
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateButton();
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00066668 File Offset: 0x00064868
		private void UpdateButton()
		{
			try
			{
				if (this.UIManagerAsset.buttonThemeType == UIManager.ButtonThemeType.Basic)
				{
					if (this.buttonType == UIManagerButton.ButtonType.Basic)
					{
						if (!this.overrideColors)
						{
							this.basicFilled.color = this.UIManagerAsset.buttonBorderColor;
							this.basicText.color = this.UIManagerAsset.buttonFilledColor;
						}
						if (!this.overrideFonts)
						{
							this.basicText.font = this.UIManagerAsset.buttonFont;
							this.basicText.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOnlyIcon && !this.overrideColors)
					{
						this.basicOnlyIconFilled.color = this.UIManagerAsset.buttonBorderColor;
						this.basicOnlyIconIcon.color = this.UIManagerAsset.buttonFilledColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicWithIcon)
					{
						if (!this.overrideColors)
						{
							this.basicWithIconFilled.color = this.UIManagerAsset.buttonBorderColor;
							this.basicWithIconIcon.color = this.UIManagerAsset.buttonFilledColor;
							this.basicWithIconText.color = this.UIManagerAsset.buttonFilledColor;
						}
						if (!this.overrideFonts)
						{
							this.basicWithIconText.font = this.UIManagerAsset.buttonFont;
							this.basicWithIconText.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOutline)
					{
						if (!this.overrideColors)
						{
							this.basicOutlineBorder.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineFilled.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineText.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineTextHighligted.color = this.UIManagerAsset.buttonFilledColor;
						}
						if (!this.overrideFonts)
						{
							this.basicOutlineText.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineTextHighligted.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineText.fontSize = this.UIManagerAsset.buttonFontSize;
							this.basicOutlineTextHighligted.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOutlineOnlyIcon && !this.overrideColors)
					{
						this.basicOutlineOOBorder.color = this.UIManagerAsset.buttonBorderColor;
						this.basicOutlineOOFilled.color = this.UIManagerAsset.buttonBorderColor;
						this.basicOutlineOOIcon.color = this.UIManagerAsset.buttonBorderColor;
						this.basicOutlineOOIconHighlighted.color = this.UIManagerAsset.buttonFilledColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOutlineWithIcon)
					{
						if (!this.overrideColors)
						{
							this.basicOutlineWOBorder.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineWOFilled.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineWOIcon.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineWOIconHighlighted.color = this.UIManagerAsset.buttonFilledColor;
							this.basicOutlineWOText.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineWOTextHighligted.color = this.UIManagerAsset.buttonFilledColor;
						}
						if (!this.overrideFonts)
						{
							this.basicOutlineWOText.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineWOTextHighligted.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineWOText.fontSize = this.UIManagerAsset.buttonFontSize;
							this.basicOutlineWOTextHighligted.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.RadialOnlyIcon && !this.overrideColors)
					{
						this.radialOOBackground.color = this.UIManagerAsset.buttonBorderColor;
						this.radialOOIcon.color = this.UIManagerAsset.buttonFilledColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.RadialOutlineOnlyIcon && !this.overrideColors)
					{
						this.radialOutlineOOBorder.color = this.UIManagerAsset.buttonBorderColor;
						this.radialOutlineOOFilled.color = this.UIManagerAsset.buttonBorderColor;
						this.radialOutlineOOIcon.color = this.UIManagerAsset.buttonIconColor;
						this.radialOutlineOOIconHighlighted.color = this.UIManagerAsset.buttonFilledColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.Rounded)
					{
						if (!this.overrideColors)
						{
							this.roundedBackground.color = this.UIManagerAsset.buttonBorderColor;
							this.roundedText.color = this.UIManagerAsset.buttonFilledColor;
						}
						if (!this.overrideFonts)
						{
							this.roundedText.font = this.UIManagerAsset.buttonFont;
							this.roundedText.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.RoundedOutline)
					{
						if (!this.overrideColors)
						{
							this.roundedOutlineBorder.color = this.UIManagerAsset.buttonBorderColor;
							this.roundedOutlineFilled.color = this.UIManagerAsset.buttonBorderColor;
							this.roundedOutlineText.color = this.UIManagerAsset.buttonBorderColor;
							this.roundedOutlineTextHighligted.color = this.UIManagerAsset.buttonFilledColor;
						}
						if (!this.overrideFonts)
						{
							this.roundedOutlineText.font = this.UIManagerAsset.buttonFont;
							this.roundedOutlineTextHighligted.font = this.UIManagerAsset.buttonFont;
							this.roundedOutlineText.fontSize = this.UIManagerAsset.buttonFontSize;
							this.roundedOutlineTextHighligted.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
				}
				else if (this.UIManagerAsset.buttonThemeType == UIManager.ButtonThemeType.Custom)
				{
					if (this.buttonType == UIManagerButton.ButtonType.Basic)
					{
						if (!this.overrideColors)
						{
							this.basicFilled.color = this.UIManagerAsset.buttonFilledColor;
							this.basicText.color = this.UIManagerAsset.buttonTextBasicColor;
						}
						if (!this.overrideFonts)
						{
							this.basicText.font = this.UIManagerAsset.buttonFont;
							this.basicText.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOnlyIcon && !this.overrideColors)
					{
						this.basicOnlyIconFilled.color = this.UIManagerAsset.buttonFilledColor;
						this.basicOnlyIconIcon.color = this.UIManagerAsset.buttonIconBasicColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicWithIcon)
					{
						if (!this.overrideColors)
						{
							this.basicWithIconFilled.color = this.UIManagerAsset.buttonFilledColor;
							this.basicWithIconIcon.color = this.UIManagerAsset.buttonIconBasicColor;
							this.basicWithIconText.color = this.UIManagerAsset.buttonTextBasicColor;
						}
						if (!this.overrideFonts)
						{
							this.basicWithIconText.font = this.UIManagerAsset.buttonFont;
							this.basicWithIconText.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOutline)
					{
						if (!this.overrideColors)
						{
							this.basicOutlineBorder.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineFilled.color = this.UIManagerAsset.buttonFilledColor;
							this.basicOutlineText.color = this.UIManagerAsset.buttonTextColor;
							this.basicOutlineTextHighligted.color = this.UIManagerAsset.buttonTextHighlightedColor;
						}
						if (!this.overrideFonts)
						{
							this.basicOutlineText.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineTextHighligted.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineText.fontSize = this.UIManagerAsset.buttonFontSize;
							this.basicOutlineTextHighligted.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOutlineOnlyIcon && !this.overrideFonts)
					{
						this.basicOutlineOOBorder.color = this.UIManagerAsset.buttonBorderColor;
						this.basicOutlineOOFilled.color = this.UIManagerAsset.buttonFilledColor;
						this.basicOutlineOOIcon.color = this.UIManagerAsset.buttonBorderColor;
						this.basicOutlineOOIconHighlighted.color = this.UIManagerAsset.buttonFilledColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.BasicOutlineWithIcon)
					{
						if (!this.overrideColors)
						{
							this.basicOutlineWOBorder.color = this.UIManagerAsset.buttonBorderColor;
							this.basicOutlineWOFilled.color = this.UIManagerAsset.buttonFilledColor;
							this.basicOutlineWOIcon.color = this.UIManagerAsset.buttonIconColor;
							this.basicOutlineWOIconHighlighted.color = this.UIManagerAsset.buttonIconHighlightedColor;
							this.basicOutlineWOText.color = this.UIManagerAsset.buttonTextColor;
							this.basicOutlineWOTextHighligted.color = this.UIManagerAsset.buttonTextHighlightedColor;
						}
						if (!this.overrideFonts)
						{
							this.basicOutlineWOText.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineWOTextHighligted.font = this.UIManagerAsset.buttonFont;
							this.basicOutlineWOText.fontSize = this.UIManagerAsset.buttonFontSize;
							this.basicOutlineWOTextHighligted.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.RadialOnlyIcon && !this.overrideColors)
					{
						this.radialOOBackground.color = this.UIManagerAsset.buttonFilledColor;
						this.radialOOIcon.color = this.UIManagerAsset.buttonIconBasicColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.RadialOutlineOnlyIcon && !this.overrideColors)
					{
						this.radialOutlineOOBorder.color = this.UIManagerAsset.buttonBorderColor;
						this.radialOutlineOOFilled.color = this.UIManagerAsset.buttonFilledColor;
						this.radialOutlineOOIcon.color = this.UIManagerAsset.buttonIconColor;
						this.radialOutlineOOIconHighlighted.color = this.UIManagerAsset.buttonIconHighlightedColor;
					}
					else if (this.buttonType == UIManagerButton.ButtonType.Rounded)
					{
						if (!this.overrideColors)
						{
							this.roundedBackground.color = this.UIManagerAsset.buttonFilledColor;
							this.roundedText.color = this.UIManagerAsset.buttonTextBasicColor;
						}
						if (!this.overrideFonts)
						{
							this.roundedText.font = this.UIManagerAsset.buttonFont;
							this.roundedText.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
					else if (this.buttonType == UIManagerButton.ButtonType.RoundedOutline)
					{
						if (!this.overrideColors)
						{
							this.roundedOutlineBorder.color = this.UIManagerAsset.buttonBorderColor;
							this.roundedOutlineFilled.color = this.UIManagerAsset.buttonFilledColor;
							this.roundedOutlineText.color = this.UIManagerAsset.buttonTextColor;
							this.roundedOutlineTextHighligted.color = this.UIManagerAsset.buttonTextHighlightedColor;
						}
						if (!this.overrideFonts)
						{
							this.roundedOutlineText.font = this.UIManagerAsset.buttonFont;
							this.roundedOutlineTextHighligted.font = this.UIManagerAsset.buttonFont;
							this.roundedOutlineText.fontSize = this.UIManagerAsset.buttonFontSize;
							this.roundedOutlineTextHighligted.fontSize = this.UIManagerAsset.buttonFontSize;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x040014DA RID: 5338
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x040014DB RID: 5339
		public UIManagerButton.ButtonType buttonType;

		// Token: 0x040014DC RID: 5340
		public bool overrideColors;

		// Token: 0x040014DD RID: 5341
		public bool overrideFonts;

		// Token: 0x040014DE RID: 5342
		[HideInInspector]
		public Image basicFilled;

		// Token: 0x040014DF RID: 5343
		[HideInInspector]
		public TextMeshProUGUI basicText;

		// Token: 0x040014E0 RID: 5344
		[HideInInspector]
		public Image basicOnlyIconFilled;

		// Token: 0x040014E1 RID: 5345
		[HideInInspector]
		public Image basicOnlyIconIcon;

		// Token: 0x040014E2 RID: 5346
		[HideInInspector]
		public Image basicWithIconFilled;

		// Token: 0x040014E3 RID: 5347
		[HideInInspector]
		public Image basicWithIconIcon;

		// Token: 0x040014E4 RID: 5348
		[HideInInspector]
		public TextMeshProUGUI basicWithIconText;

		// Token: 0x040014E5 RID: 5349
		[HideInInspector]
		public Image basicOutlineBorder;

		// Token: 0x040014E6 RID: 5350
		[HideInInspector]
		public Image basicOutlineFilled;

		// Token: 0x040014E7 RID: 5351
		[HideInInspector]
		public TextMeshProUGUI basicOutlineText;

		// Token: 0x040014E8 RID: 5352
		[HideInInspector]
		public TextMeshProUGUI basicOutlineTextHighligted;

		// Token: 0x040014E9 RID: 5353
		[HideInInspector]
		public Image basicOutlineOOBorder;

		// Token: 0x040014EA RID: 5354
		[HideInInspector]
		public Image basicOutlineOOFilled;

		// Token: 0x040014EB RID: 5355
		[HideInInspector]
		public Image basicOutlineOOIcon;

		// Token: 0x040014EC RID: 5356
		[HideInInspector]
		public Image basicOutlineOOIconHighlighted;

		// Token: 0x040014ED RID: 5357
		[HideInInspector]
		public Image basicOutlineWOBorder;

		// Token: 0x040014EE RID: 5358
		[HideInInspector]
		public Image basicOutlineWOFilled;

		// Token: 0x040014EF RID: 5359
		[HideInInspector]
		public Image basicOutlineWOIcon;

		// Token: 0x040014F0 RID: 5360
		[HideInInspector]
		public Image basicOutlineWOIconHighlighted;

		// Token: 0x040014F1 RID: 5361
		[HideInInspector]
		public TextMeshProUGUI basicOutlineWOText;

		// Token: 0x040014F2 RID: 5362
		[HideInInspector]
		public TextMeshProUGUI basicOutlineWOTextHighligted;

		// Token: 0x040014F3 RID: 5363
		[HideInInspector]
		public Image radialOOBackground;

		// Token: 0x040014F4 RID: 5364
		[HideInInspector]
		public Image radialOOIcon;

		// Token: 0x040014F5 RID: 5365
		[HideInInspector]
		public Image radialOutlineOOBorder;

		// Token: 0x040014F6 RID: 5366
		[HideInInspector]
		public Image radialOutlineOOFilled;

		// Token: 0x040014F7 RID: 5367
		[HideInInspector]
		public Image radialOutlineOOIcon;

		// Token: 0x040014F8 RID: 5368
		[HideInInspector]
		public Image radialOutlineOOIconHighlighted;

		// Token: 0x040014F9 RID: 5369
		[HideInInspector]
		public Image roundedBackground;

		// Token: 0x040014FA RID: 5370
		[HideInInspector]
		public TextMeshProUGUI roundedText;

		// Token: 0x040014FB RID: 5371
		[HideInInspector]
		public Image roundedOutlineBorder;

		// Token: 0x040014FC RID: 5372
		[HideInInspector]
		public Image roundedOutlineFilled;

		// Token: 0x040014FD RID: 5373
		[HideInInspector]
		public TextMeshProUGUI roundedOutlineText;

		// Token: 0x040014FE RID: 5374
		[HideInInspector]
		public TextMeshProUGUI roundedOutlineTextHighligted;

		// Token: 0x02000344 RID: 836
		public enum ButtonType
		{
			// Token: 0x04001500 RID: 5376
			Basic,
			// Token: 0x04001501 RID: 5377
			BasicOnlyIcon,
			// Token: 0x04001502 RID: 5378
			BasicWithIcon,
			// Token: 0x04001503 RID: 5379
			BasicOutline,
			// Token: 0x04001504 RID: 5380
			BasicOutlineOnlyIcon,
			// Token: 0x04001505 RID: 5381
			BasicOutlineWithIcon,
			// Token: 0x04001506 RID: 5382
			RadialOnlyIcon,
			// Token: 0x04001507 RID: 5383
			RadialOutlineOnlyIcon,
			// Token: 0x04001508 RID: 5384
			Rounded,
			// Token: 0x04001509 RID: 5385
			RoundedOutline
		}
	}
}
