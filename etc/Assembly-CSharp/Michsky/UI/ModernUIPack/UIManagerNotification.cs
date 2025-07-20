using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200034B RID: 843
	[ExecuteInEditMode]
	public class UIManagerNotification : MonoBehaviour
	{
		// Token: 0x06001647 RID: 5703 RVA: 0x00067C80 File Offset: 0x00065E80
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
					this.UpdateNotification();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x00067CEC File Offset: 0x00065EEC
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateNotification();
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00067D10 File Offset: 0x00065F10
		private void UpdateNotification()
		{
			try
			{
				if (!this.overrideColors)
				{
					this.background.color = this.UIManagerAsset.notificationBackgroundColor;
					this.icon.color = this.UIManagerAsset.notificationIconColor;
					this.title.color = this.UIManagerAsset.notificationTitleColor;
					this.description.color = this.UIManagerAsset.notificationDescriptionColor;
				}
				if (!this.overrideFonts)
				{
					this.title.font = this.UIManagerAsset.notificationTitleFont;
					this.title.fontSize = this.UIManagerAsset.notificationTitleFontSize;
					this.description.font = this.UIManagerAsset.notificationDescriptionFont;
					this.description.fontSize = this.UIManagerAsset.notificationDescriptionFontSize;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400152F RID: 5423
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001530 RID: 5424
		public bool overrideColors;

		// Token: 0x04001531 RID: 5425
		public bool overrideFonts;

		// Token: 0x04001532 RID: 5426
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x04001533 RID: 5427
		[SerializeField]
		private Image icon;

		// Token: 0x04001534 RID: 5428
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04001535 RID: 5429
		[SerializeField]
		private TextMeshProUGUI description;
	}
}
