using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000353 RID: 851
	[ExecuteInEditMode]
	public class UIManagerTooltip : MonoBehaviour
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x00068708 File Offset: 0x00066908
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
					this.UpdateTooltip();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00068774 File Offset: 0x00066974
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateTooltip();
			}
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00068798 File Offset: 0x00066998
		private void UpdateTooltip()
		{
			try
			{
				this.background.color = this.UIManagerAsset.tooltipBackgroundColor;
				this.text.color = this.UIManagerAsset.tooltipTextColor;
				this.text.font = this.UIManagerAsset.tooltipFont;
				this.text.fontSize = this.UIManagerAsset.tooltipFontSize;
			}
			catch
			{
			}
		}

		// Token: 0x0400155C RID: 5468
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400155D RID: 5469
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x0400155E RID: 5470
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
