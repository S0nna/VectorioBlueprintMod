using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200034E RID: 846
	[ExecuteInEditMode]
	public class UIManagerProgressBarLoop : MonoBehaviour
	{
		// Token: 0x06001650 RID: 5712 RVA: 0x00067F24 File Offset: 0x00066124
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

		// Token: 0x06001651 RID: 5713 RVA: 0x00067F90 File Offset: 0x00066190
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

		// Token: 0x06001652 RID: 5714 RVA: 0x00067FB4 File Offset: 0x000661B4
		private void UpdateProgressBar()
		{
			if (!this.overrideColors)
			{
				try
				{
					this.bar.color = this.UIManagerAsset.progressBarColor;
					if (this.hasBackground)
					{
						if (this.useRegularBackground)
						{
							this.background.color = this.UIManagerAsset.progressBarBackgroundColor;
						}
						else
						{
							this.background.color = this.UIManagerAsset.progressBarLoopBackgroundColor;
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0400153D RID: 5437
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400153E RID: 5438
		public bool hasBackground;

		// Token: 0x0400153F RID: 5439
		public bool useRegularBackground;

		// Token: 0x04001540 RID: 5440
		public bool overrideColors;

		// Token: 0x04001541 RID: 5441
		[Header("Resources")]
		public Image bar;

		// Token: 0x04001542 RID: 5442
		[HideInInspector]
		public Image background;
	}
}
