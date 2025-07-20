using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
[DefaultExecutionOrder(10)]
public class Bootstrap : MonoBehaviour
{
	// Token: 0x06000477 RID: 1143 RVA: 0x000179B8 File Offset: 0x00015BB8
	public void Start()
	{
		LeanTween.alphaCanvas(this.iconContent.group, 1f, 0.5f).setDelay(0.5f);
		LeanTween.moveLocal(this.iconContent.group.gameObject, this.iconContent.normalPos, 0.5f).setEase(LeanTweenType.easeOutExpo).setDelay(0.5f);
		LeanTween.alphaCanvas(this.mainContent.group, 1f, 0.5f).setDelay(1.5f);
		LeanTween.alphaCanvas(this.riskContent.group, 1f, 0.5f).setDelay(2.5f);
		LeanTween.alphaCanvas(this.iconContent.group, 0f, 1f).setDelay(6.5f);
		LeanTween.alphaCanvas(this.mainContent.group, 0f, 1f).setDelay(6.5f);
		LeanTween.alphaCanvas(this.riskContent.group, 0f, 1f).setDelay(6.5f);
		this.library.ProcessDataList();
	}

	// Token: 0x04000259 RID: 601
	public Library library;

	// Token: 0x0400025A RID: 602
	public MenuButton iconContent;

	// Token: 0x0400025B RID: 603
	public MenuButton mainContent;

	// Token: 0x0400025C RID: 604
	public MenuButton riskContent;

	// Token: 0x0400025D RID: 605
	public MenuButton spaceSkip;
}
