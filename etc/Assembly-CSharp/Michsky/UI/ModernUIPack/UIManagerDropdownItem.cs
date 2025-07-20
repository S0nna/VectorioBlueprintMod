using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000347 RID: 839
	[ExecuteInEditMode]
	public class UIManagerDropdownItem : MonoBehaviour
	{
		// Token: 0x06001637 RID: 5687 RVA: 0x000674E4 File Offset: 0x000656E4
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
					this.UpdateDropdown();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00067550 File Offset: 0x00065750
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

		// Token: 0x06001639 RID: 5689 RVA: 0x00067574 File Offset: 0x00065774
		private void UpdateDropdown()
		{
			try
			{
				if (this.UIManagerAsset.buttonThemeType == UIManager.ButtonThemeType.Basic)
				{
					if (!this.overrideColors)
					{
						this.itemBackground.color = this.UIManagerAsset.dropdownItemColor;
						this.itemIcon.color = this.UIManagerAsset.dropdownTextColor;
						this.itemText.color = this.UIManagerAsset.dropdownTextColor;
					}
					if (!this.overrideFonts)
					{
						this.itemText.font = this.UIManagerAsset.dropdownFont;
						this.itemText.fontSize = this.UIManagerAsset.dropdownFontSize;
					}
				}
				else if (this.UIManagerAsset.buttonThemeType == UIManager.ButtonThemeType.Custom)
				{
					if (!this.overrideColors)
					{
						this.itemBackground.color = this.UIManagerAsset.dropdownItemColor;
						this.itemIcon.color = this.UIManagerAsset.dropdownItemIconColor;
						this.itemText.color = this.UIManagerAsset.dropdownItemTextColor;
					}
					if (!this.overrideFonts)
					{
						this.itemText.font = this.UIManagerAsset.dropdownItemFont;
						this.itemText.fontSize = this.UIManagerAsset.dropdownItemFontSize;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04001516 RID: 5398
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001517 RID: 5399
		public bool overrideColors;

		// Token: 0x04001518 RID: 5400
		public bool overrideFonts;

		// Token: 0x04001519 RID: 5401
		[Header("Resources")]
		[SerializeField]
		private Image itemBackground;

		// Token: 0x0400151A RID: 5402
		[SerializeField]
		private Image itemIcon;

		// Token: 0x0400151B RID: 5403
		[SerializeField]
		private TextMeshProUGUI itemText;
	}
}
