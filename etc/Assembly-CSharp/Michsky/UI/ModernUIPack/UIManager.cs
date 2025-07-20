using System;
using TMPro;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200033B RID: 827
	[CreateAssetMenu(fileName = "New UI Manager", menuName = "Modern UI Pack/New UI Manager")]
	public class UIManager : ScriptableObject
	{
		// Token: 0x04001471 RID: 5233
		[HideInInspector]
		public bool enableDynamicUpdate = true;

		// Token: 0x04001472 RID: 5234
		[HideInInspector]
		public bool enableExtendedColorPicker = true;

		// Token: 0x04001473 RID: 5235
		[HideInInspector]
		public bool editorHints = true;

		// Token: 0x04001474 RID: 5236
		public Color animatedIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001475 RID: 5237
		public Color contextBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001476 RID: 5238
		public UIManager.ButtonThemeType buttonThemeType;

		// Token: 0x04001477 RID: 5239
		public TMP_FontAsset buttonFont;

		// Token: 0x04001478 RID: 5240
		public float buttonFontSize = 22.5f;

		// Token: 0x04001479 RID: 5241
		public Color buttonBorderColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400147A RID: 5242
		public Color buttonFilledColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400147B RID: 5243
		public Color buttonTextBasicColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400147C RID: 5244
		public Color buttonTextColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400147D RID: 5245
		public Color buttonTextHighlightedColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400147E RID: 5246
		public Color buttonIconBasicColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400147F RID: 5247
		public Color buttonIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001480 RID: 5248
		public Color buttonIconHighlightedColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001481 RID: 5249
		public TMP_FontAsset dropdownItemFont;

		// Token: 0x04001482 RID: 5250
		public float dropdownItemFontSize = 22.5f;

		// Token: 0x04001483 RID: 5251
		public UIManager.DropdownThemeType dropdownThemeType;

		// Token: 0x04001484 RID: 5252
		public TMP_FontAsset dropdownFont;

		// Token: 0x04001485 RID: 5253
		public float dropdownFontSize = 22.5f;

		// Token: 0x04001486 RID: 5254
		public Color dropdownColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001487 RID: 5255
		public Color dropdownTextColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001488 RID: 5256
		public Color dropdownIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001489 RID: 5257
		public Color dropdownItemColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400148A RID: 5258
		public Color dropdownItemTextColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400148B RID: 5259
		public Color dropdownItemIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400148C RID: 5260
		public TMP_FontAsset selectorFont;

		// Token: 0x0400148D RID: 5261
		public float hSelectorFontSize = 28f;

		// Token: 0x0400148E RID: 5262
		public Color selectorColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400148F RID: 5263
		public Color selectorHighlightedColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001490 RID: 5264
		public bool hSelectorInvertAnimation;

		// Token: 0x04001491 RID: 5265
		public bool hSelectorLoopSelection;

		// Token: 0x04001492 RID: 5266
		public TMP_FontAsset inputFieldFont;

		// Token: 0x04001493 RID: 5267
		public float inputFieldFontSize = 28f;

		// Token: 0x04001494 RID: 5268
		public Color inputFieldColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001495 RID: 5269
		public TMP_FontAsset modalWindowTitleFont;

		// Token: 0x04001496 RID: 5270
		public TMP_FontAsset modalWindowContentFont;

		// Token: 0x04001497 RID: 5271
		public UIManager.DropdownThemeType modalThemeType;

		// Token: 0x04001498 RID: 5272
		public Color modalWindowTitleColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001499 RID: 5273
		public Color modalWindowDescriptionColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400149A RID: 5274
		public Color modalWindowIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400149B RID: 5275
		public Color modalWindowBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400149C RID: 5276
		public Color modalWindowContentPanelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400149D RID: 5277
		public TMP_FontAsset notificationTitleFont;

		// Token: 0x0400149E RID: 5278
		public float notificationTitleFontSize = 22.5f;

		// Token: 0x0400149F RID: 5279
		public TMP_FontAsset notificationDescriptionFont;

		// Token: 0x040014A0 RID: 5280
		public float notificationDescriptionFontSize = 18f;

		// Token: 0x040014A1 RID: 5281
		public UIManager.NotificationThemeType notificationThemeType;

		// Token: 0x040014A2 RID: 5282
		public Color notificationBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014A3 RID: 5283
		public Color notificationTitleColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014A4 RID: 5284
		public Color notificationDescriptionColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014A5 RID: 5285
		public Color notificationIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014A6 RID: 5286
		public TMP_FontAsset progressBarLabelFont;

		// Token: 0x040014A7 RID: 5287
		public float progressBarLabelFontSize = 25f;

		// Token: 0x040014A8 RID: 5288
		public Color progressBarColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014A9 RID: 5289
		public Color progressBarBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014AA RID: 5290
		public Color progressBarLoopBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014AB RID: 5291
		public Color progressBarLabelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014AC RID: 5292
		public Color scrollbarColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014AD RID: 5293
		public Color scrollbarBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014AE RID: 5294
		public TMP_FontAsset sliderLabelFont;

		// Token: 0x040014AF RID: 5295
		public float sliderLabelFontSize = 24f;

		// Token: 0x040014B0 RID: 5296
		public UIManager.SliderThemeType sliderThemeType;

		// Token: 0x040014B1 RID: 5297
		public Color sliderColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B2 RID: 5298
		public Color sliderBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B3 RID: 5299
		public Color sliderLabelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B4 RID: 5300
		public Color sliderPopupLabelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B5 RID: 5301
		public Color sliderHandleColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B6 RID: 5302
		public Color switchBorderColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B7 RID: 5303
		public Color switchBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B8 RID: 5304
		public Color switchHandleOnColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014B9 RID: 5305
		public Color switchHandleOffColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014BA RID: 5306
		public TMP_FontAsset toggleFont;

		// Token: 0x040014BB RID: 5307
		public float toggleFontSize = 35f;

		// Token: 0x040014BC RID: 5308
		public UIManager.ToggleThemeType toggleThemeType;

		// Token: 0x040014BD RID: 5309
		public Color toggleTextColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014BE RID: 5310
		public Color toggleBorderColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014BF RID: 5311
		public Color toggleBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014C0 RID: 5312
		public Color toggleCheckColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014C1 RID: 5313
		public TMP_FontAsset tooltipFont;

		// Token: 0x040014C2 RID: 5314
		public float tooltipFontSize = 22f;

		// Token: 0x040014C3 RID: 5315
		public Color tooltipTextColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040014C4 RID: 5316
		public Color tooltipBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0200033C RID: 828
		public enum ButtonThemeType
		{
			// Token: 0x040014C6 RID: 5318
			Basic,
			// Token: 0x040014C7 RID: 5319
			Custom
		}

		// Token: 0x0200033D RID: 829
		public enum DropdownThemeType
		{
			// Token: 0x040014C9 RID: 5321
			Basic,
			// Token: 0x040014CA RID: 5322
			Custom
		}

		// Token: 0x0200033E RID: 830
		public enum ModalWindowThemeType
		{
			// Token: 0x040014CC RID: 5324
			Basic,
			// Token: 0x040014CD RID: 5325
			Custom
		}

		// Token: 0x0200033F RID: 831
		public enum NotificationThemeType
		{
			// Token: 0x040014CF RID: 5327
			Basic,
			// Token: 0x040014D0 RID: 5328
			Custom
		}

		// Token: 0x02000340 RID: 832
		public enum SliderThemeType
		{
			// Token: 0x040014D2 RID: 5330
			Basic,
			// Token: 0x040014D3 RID: 5331
			Custom
		}

		// Token: 0x02000341 RID: 833
		public enum ToggleThemeType
		{
			// Token: 0x040014D5 RID: 5333
			Basic,
			// Token: 0x040014D6 RID: 5334
			Custom
		}
	}
}
