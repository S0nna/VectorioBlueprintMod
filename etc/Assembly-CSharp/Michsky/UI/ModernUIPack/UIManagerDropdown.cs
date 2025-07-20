using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000346 RID: 838
	[ExecuteInEditMode]
	public class UIManagerDropdown : MonoBehaviour
	{
		// Token: 0x06001633 RID: 5683 RVA: 0x00067278 File Offset: 0x00065478
		private void Awake()
		{
			try
			{
				this.dropdownMain = base.gameObject.GetComponent<CustomDropdown>();
				if (this.dropdownMain == null)
				{
					this.dropdownMulti = base.gameObject.GetComponent<DropdownMultiSelect>();
				}
				if (this.UIManagerAsset == null)
				{
					this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
				}
				base.enabled = true;
				if (!this.UIManagerAsset.enableDynamicUpdate)
				{
					this.UpdateDropdown();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00067314 File Offset: 0x00065514
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateDropdown();
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00067338 File Offset: 0x00065538
		private void UpdateDropdown()
		{
			try
			{
				if (this.UIManagerAsset.buttonThemeType == UIManager.ButtonThemeType.Basic)
				{
					if (!this.overrideColors)
					{
						this.background.color = this.UIManagerAsset.dropdownColor;
						this.contentBackground.color = this.UIManagerAsset.dropdownColor;
						this.mainIcon.color = this.UIManagerAsset.dropdownTextColor;
						this.mainText.color = this.UIManagerAsset.dropdownTextColor;
						this.expandIcon.color = this.UIManagerAsset.dropdownTextColor;
					}
					if (!this.overrideFonts)
					{
						this.mainText.font = this.UIManagerAsset.dropdownFont;
						this.mainText.fontSize = this.UIManagerAsset.dropdownFontSize;
					}
				}
				else if (this.UIManagerAsset.buttonThemeType == UIManager.ButtonThemeType.Custom)
				{
					if (!this.overrideColors)
					{
						this.background.color = this.UIManagerAsset.dropdownColor;
						this.contentBackground.color = this.UIManagerAsset.dropdownColor;
						this.mainIcon.color = this.UIManagerAsset.dropdownIconColor;
						this.mainText.color = this.UIManagerAsset.dropdownTextColor;
						this.expandIcon.color = this.UIManagerAsset.dropdownIconColor;
					}
					if (!this.overrideFonts)
					{
						this.mainText.font = this.UIManagerAsset.dropdownFont;
						this.mainText.fontSize = this.UIManagerAsset.dropdownFontSize;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400150C RID: 5388
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400150D RID: 5389
		public bool overrideColors;

		// Token: 0x0400150E RID: 5390
		public bool overrideFonts;

		// Token: 0x0400150F RID: 5391
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x04001510 RID: 5392
		[SerializeField]
		private Image contentBackground;

		// Token: 0x04001511 RID: 5393
		[SerializeField]
		private Image mainIcon;

		// Token: 0x04001512 RID: 5394
		[SerializeField]
		private TextMeshProUGUI mainText;

		// Token: 0x04001513 RID: 5395
		[SerializeField]
		private Image expandIcon;

		// Token: 0x04001514 RID: 5396
		private CustomDropdown dropdownMain;

		// Token: 0x04001515 RID: 5397
		private DropdownMultiSelect dropdownMulti;
	}
}
