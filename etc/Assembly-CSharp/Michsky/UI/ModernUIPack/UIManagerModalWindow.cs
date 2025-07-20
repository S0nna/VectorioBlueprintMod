using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200034A RID: 842
	[ExecuteInEditMode]
	public class UIManagerModalWindow : MonoBehaviour
	{
		// Token: 0x06001643 RID: 5699 RVA: 0x00067B34 File Offset: 0x00065D34
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
					this.UpdateModalWindow();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00067BA0 File Offset: 0x00065DA0
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateModalWindow();
			}
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00067BC4 File Offset: 0x00065DC4
		private void UpdateModalWindow()
		{
			try
			{
				this.background.color = this.UIManagerAsset.modalWindowBackgroundColor;
				this.contentBackground.color = this.UIManagerAsset.modalWindowContentPanelColor;
				this.icon.color = this.UIManagerAsset.modalWindowIconColor;
				this.title.color = this.UIManagerAsset.modalWindowTitleColor;
				this.description.color = this.UIManagerAsset.modalWindowDescriptionColor;
				this.title.font = this.UIManagerAsset.modalWindowTitleFont;
				this.description.font = this.UIManagerAsset.modalWindowContentFont;
			}
			catch
			{
			}
		}

		// Token: 0x04001529 RID: 5417
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400152A RID: 5418
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x0400152B RID: 5419
		[SerializeField]
		private Image contentBackground;

		// Token: 0x0400152C RID: 5420
		[SerializeField]
		private Image icon;

		// Token: 0x0400152D RID: 5421
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x0400152E RID: 5422
		[SerializeField]
		private TextMeshProUGUI description;
	}
}
