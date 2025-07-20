using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000349 RID: 841
	[ExecuteInEditMode]
	public class UIManagerInputField : MonoBehaviour
	{
		// Token: 0x0600163F RID: 5695 RVA: 0x00067984 File Offset: 0x00065B84
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
					this.UpdateInputField();
					base.enabled = false;
				}
			}
			catch
			{
				Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this);
			}
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000679F0 File Offset: 0x00065BF0
		private void LateUpdate()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateInputField();
			}
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00067A14 File Offset: 0x00065C14
		private void UpdateInputField()
		{
			if (!this.overrideColors)
			{
				for (int i = 0; i < this.images.Count; i++)
				{
					Image component = this.images[i].GetComponent<Image>();
					component.color = new Color(this.UIManagerAsset.inputFieldColor.r, this.UIManagerAsset.inputFieldColor.g, this.UIManagerAsset.inputFieldColor.b, component.color.a);
				}
			}
			for (int j = 0; j < this.texts.Count; j++)
			{
				TextMeshProUGUI component2 = this.texts[j].GetComponent<TextMeshProUGUI>();
				if (!this.overrideColors)
				{
					component2.color = new Color(this.UIManagerAsset.inputFieldColor.r, this.UIManagerAsset.inputFieldColor.g, this.UIManagerAsset.inputFieldColor.b, component2.color.a);
				}
				bool flag = this.overrideFonts;
			}
		}

		// Token: 0x04001524 RID: 5412
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001525 RID: 5413
		public bool overrideColors;

		// Token: 0x04001526 RID: 5414
		public bool overrideFonts;

		// Token: 0x04001527 RID: 5415
		[Header("Resources")]
		[SerializeField]
		private List<GameObject> images = new List<GameObject>();

		// Token: 0x04001528 RID: 5416
		[SerializeField]
		private List<GameObject> texts = new List<GameObject>();
	}
}
