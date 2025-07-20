using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000351 RID: 849
	[ExecuteInEditMode]
	public class UIManagerSwitch : MonoBehaviour
	{
		// Token: 0x0600165C RID: 5724 RVA: 0x0006846C File Offset: 0x0006666C
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
					this.UpdateSwitch();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x000684D8 File Offset: 0x000666D8
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSwitch();
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x000684FC File Offset: 0x000666FC
		private void UpdateSwitch()
		{
			if (!this.overrideColors)
			{
				try
				{
					this.border.color = new Color(this.UIManagerAsset.switchBorderColor.r, this.UIManagerAsset.switchBorderColor.g, this.UIManagerAsset.switchBorderColor.b, this.border.color.a);
					this.background.color = new Color(this.UIManagerAsset.switchBackgroundColor.r, this.UIManagerAsset.switchBackgroundColor.g, this.UIManagerAsset.switchBackgroundColor.b, this.background.color.a);
					this.handleOn.color = new Color(this.UIManagerAsset.switchHandleOnColor.r, this.UIManagerAsset.switchHandleOnColor.g, this.UIManagerAsset.switchHandleOnColor.b, this.handleOn.color.a);
					this.handleOff.color = new Color(this.UIManagerAsset.switchHandleOffColor.r, this.UIManagerAsset.switchHandleOffColor.g, this.UIManagerAsset.switchHandleOffColor.b, this.handleOff.color.a);
				}
				catch
				{
				}
			}
		}

		// Token: 0x04001550 RID: 5456
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001551 RID: 5457
		public bool overrideColors;

		// Token: 0x04001552 RID: 5458
		[Header("Resources")]
		[SerializeField]
		private Image border;

		// Token: 0x04001553 RID: 5459
		[SerializeField]
		private Image background;

		// Token: 0x04001554 RID: 5460
		[SerializeField]
		private Image handleOn;

		// Token: 0x04001555 RID: 5461
		[SerializeField]
		private Image handleOff;
	}
}
