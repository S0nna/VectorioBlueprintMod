using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001B0 RID: 432
public class DemoMenu : MonoBehaviour
{
	// Token: 0x06000DF3 RID: 3571 RVA: 0x0003E070 File Offset: 0x0003C270
	public void Start()
	{
		if (this.demoEnabled)
		{
			this.buttonOne.enabled = false;
			this.buttonTwo.enabled = false;
			this.buttonThree.enabled = false;
			this.buttonFour.enabled = false;
			this.freeplayTitle.text = this.freeplayTitleText;
			this.freeplayDesc.text = this.freeplayDescText;
			this.buttonOneDesc.text = this.gamemodeLocked;
			this.buttonTwoDesc.text = this.gamemodeLocked;
			this.buttonThreeDesc.text = this.gamemodeLocked;
			this.buttonFourDesc.text = this.gamemodeLocked;
			this.creativeBackground.color = this.backgroundColor;
			this.creativeBorder.color = this.borderColor;
			this.creativeBanner.color = this.borderColor;
			this.creativeIcon.sprite = this.greyPicture;
		}
	}

	// Token: 0x04000A5D RID: 2653
	public bool demoEnabled;

	// Token: 0x04000A5E RID: 2654
	public string freeplayTitleText;

	// Token: 0x04000A5F RID: 2655
	public string freeplayDescText;

	// Token: 0x04000A60 RID: 2656
	public string gamemodeLocked;

	// Token: 0x04000A61 RID: 2657
	public Color borderColor;

	// Token: 0x04000A62 RID: 2658
	public Color backgroundColor;

	// Token: 0x04000A63 RID: 2659
	public Vectorio.PhasmaUI.Button buttonOne;

	// Token: 0x04000A64 RID: 2660
	public Vectorio.PhasmaUI.Button buttonTwo;

	// Token: 0x04000A65 RID: 2661
	public Vectorio.PhasmaUI.Button buttonThree;

	// Token: 0x04000A66 RID: 2662
	public Vectorio.PhasmaUI.Button buttonFour;

	// Token: 0x04000A67 RID: 2663
	public TextMeshProUGUI freeplayTitle;

	// Token: 0x04000A68 RID: 2664
	public TextMeshProUGUI freeplayDesc;

	// Token: 0x04000A69 RID: 2665
	public TextMeshProUGUI buttonOneDesc;

	// Token: 0x04000A6A RID: 2666
	public TextMeshProUGUI buttonTwoDesc;

	// Token: 0x04000A6B RID: 2667
	public TextMeshProUGUI buttonThreeDesc;

	// Token: 0x04000A6C RID: 2668
	public TextMeshProUGUI buttonFourDesc;

	// Token: 0x04000A6D RID: 2669
	public Image creativeBackground;

	// Token: 0x04000A6E RID: 2670
	public Image creativeBorder;

	// Token: 0x04000A6F RID: 2671
	public Image creativeBanner;

	// Token: 0x04000A70 RID: 2672
	public Image creativeIcon;

	// Token: 0x04000A71 RID: 2673
	public Sprite greyPicture;
}
