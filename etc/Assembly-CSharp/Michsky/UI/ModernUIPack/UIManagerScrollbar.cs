using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200034F RID: 847
	[ExecuteInEditMode]
	public class UIManagerScrollbar : MonoBehaviour
	{
		// Token: 0x06001654 RID: 5716 RVA: 0x00068034 File Offset: 0x00066234
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
					this.UpdateScrollbar();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000680A0 File Offset: 0x000662A0
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateScrollbar();
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00003212 File Offset: 0x00001412
		private void UpdateScrollbar()
		{
		}

		// Token: 0x04001543 RID: 5443
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001544 RID: 5444
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x04001545 RID: 5445
		[SerializeField]
		private Image bar;
	}
}
