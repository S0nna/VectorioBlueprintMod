using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000348 RID: 840
	[ExecuteInEditMode]
	public class UIManagerHSelector : MonoBehaviour
	{
		// Token: 0x0600163B RID: 5691 RVA: 0x000676C4 File Offset: 0x000658C4
		private void Awake()
		{
			try
			{
				if (this.hSelector == null)
				{
					this.hSelector = base.gameObject.GetComponent<HorizontalSelector>();
				}
				if (this.UIManagerAsset == null)
				{
					this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
				}
				base.enabled = true;
				if (!this.UIManagerAsset.enableDynamicUpdate)
				{
					this.UpdateSelector();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00067750 File Offset: 0x00065950
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSelector();
			}
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00067774 File Offset: 0x00065974
		private void UpdateSelector()
		{
			if (!this.overrideColors)
			{
				for (int i = 0; i < this.images.Count; i++)
				{
					Image component = this.images[i].GetComponent<Image>();
					component.color = new Color(this.UIManagerAsset.selectorColor.r, this.UIManagerAsset.selectorColor.g, this.UIManagerAsset.selectorColor.b, component.color.a);
				}
				for (int j = 0; j < this.imagesHighlighted.Count; j++)
				{
					Image component2 = this.imagesHighlighted[j].GetComponent<Image>();
					component2.color = new Color(this.UIManagerAsset.selectorHighlightedColor.r, this.UIManagerAsset.selectorHighlightedColor.g, this.UIManagerAsset.selectorHighlightedColor.b, component2.color.a);
				}
			}
			for (int k = 0; k < this.texts.Count; k++)
			{
				TextMeshProUGUI component3 = this.texts[k].GetComponent<TextMeshProUGUI>();
				if (!this.overrideColors)
				{
					component3.color = new Color(this.UIManagerAsset.selectorColor.r, this.UIManagerAsset.selectorColor.g, this.UIManagerAsset.selectorColor.b, component3.color.a);
				}
				if (!this.overrideFonts)
				{
					component3.font = this.UIManagerAsset.selectorFont;
					component3.fontSize = this.UIManagerAsset.hSelectorFontSize;
				}
			}
			if (this.hSelector != null && !this.overrideOptions)
			{
				this.hSelector.invertAnimation = this.UIManagerAsset.hSelectorInvertAnimation;
				this.hSelector.loopSelection = this.UIManagerAsset.hSelectorLoopSelection;
			}
		}

		// Token: 0x0400151C RID: 5404
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400151D RID: 5405
		public bool overrideOptions;

		// Token: 0x0400151E RID: 5406
		public bool overrideColors;

		// Token: 0x0400151F RID: 5407
		public bool overrideFonts;

		// Token: 0x04001520 RID: 5408
		[Header("Resources")]
		[SerializeField]
		private List<GameObject> images = new List<GameObject>();

		// Token: 0x04001521 RID: 5409
		[SerializeField]
		private List<GameObject> imagesHighlighted = new List<GameObject>();

		// Token: 0x04001522 RID: 5410
		[SerializeField]
		private List<GameObject> texts = new List<GameObject>();

		// Token: 0x04001523 RID: 5411
		private HorizontalSelector hSelector;
	}
}
