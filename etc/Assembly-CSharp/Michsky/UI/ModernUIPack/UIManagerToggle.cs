using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000352 RID: 850
	[ExecuteInEditMode]
	public class UIManagerToggle : MonoBehaviour
	{
		// Token: 0x06001660 RID: 5728 RVA: 0x00068678 File Offset: 0x00066878
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
					this.UpdateToggle();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x000686E4 File Offset: 0x000668E4
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateToggle();
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00003212 File Offset: 0x00001412
		private void UpdateToggle()
		{
		}

		// Token: 0x04001556 RID: 5462
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001557 RID: 5463
		[Header("Resources")]
		[SerializeField]
		private Image border;

		// Token: 0x04001558 RID: 5464
		[SerializeField]
		private Image background;

		// Token: 0x04001559 RID: 5465
		[SerializeField]
		private Image check;

		// Token: 0x0400155A RID: 5466
		[SerializeField]
		private TextMeshProUGUI onLabel;

		// Token: 0x0400155B RID: 5467
		[SerializeField]
		private TextMeshProUGUI offLabel;
	}
}
