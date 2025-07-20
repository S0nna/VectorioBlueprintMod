using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000342 RID: 834
	[ExecuteInEditMode]
	public class UIManagerAnimatedIcon : MonoBehaviour
	{
		// Token: 0x06001627 RID: 5671 RVA: 0x00066478 File Offset: 0x00064678
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
					this.UpdateAnimatedIcon();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000664E4 File Offset: 0x000646E4
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateAnimatedIcon();
			}
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00066508 File Offset: 0x00064708
		private void UpdateAnimatedIcon()
		{
			for (int i = 0; i < this.images.Count; i++)
			{
				this.images[i].GetComponent<Image>().color = this.UIManagerAsset.animatedIconColor;
			}
			for (int j = 0; j < this.imagesWithAlpha.Count; j++)
			{
				Image component = this.imagesWithAlpha[j].GetComponent<Image>();
				component.color = new Color(this.UIManagerAsset.animatedIconColor.r, this.UIManagerAsset.animatedIconColor.g, this.UIManagerAsset.animatedIconColor.b, component.color.a);
			}
		}

		// Token: 0x040014D7 RID: 5335
		[Header("Settings")]
		public UIManager UIManagerAsset;

		// Token: 0x040014D8 RID: 5336
		[Header("Resources")]
		public List<GameObject> images = new List<GameObject>();

		// Token: 0x040014D9 RID: 5337
		public List<GameObject> imagesWithAlpha = new List<GameObject>();
	}
}
