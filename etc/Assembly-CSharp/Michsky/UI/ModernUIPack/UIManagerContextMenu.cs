using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000345 RID: 837
	[ExecuteInEditMode]
	public class UIManagerContextMenu : MonoBehaviour
	{
		// Token: 0x0600162F RID: 5679 RVA: 0x000671D0 File Offset: 0x000653D0
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
					this.UpdateContextMenu();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0006723C File Offset: 0x0006543C
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateContextMenu();
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00067260 File Offset: 0x00065460
		private void UpdateContextMenu()
		{
			this.backgroundImage.color = this.UIManagerAsset.contextBackgroundColor;
		}

		// Token: 0x0400150A RID: 5386
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400150B RID: 5387
		[Header("Resources")]
		[SerializeField]
		private Image backgroundImage;
	}
}
