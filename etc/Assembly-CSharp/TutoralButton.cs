using System;
using TMPro;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001FF RID: 511
public class TutoralButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000F72 RID: 3954 RVA: 0x00048F38 File Offset: 0x00047138
	public void Unlock(TutorialManager.Segment segment)
	{
		this._tutorialID = segment.id;
		base.enabled = true;
		this.tutorialType.text = segment.tutorialName.ToUpper();
		this.tutorialName.text = segment.title.ToUpper();
		this.tutorialType.color = segment.buttonTitleColor;
		this.bar.color = segment.buttonBorderColor;
		this.background.color = segment.buttonBackgroundColor;
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x00048FB7 File Offset: 0x000471B7
	public override void OnClick()
	{
		Singleton<Events>.Instance.onClosePause.Invoke();
		Singleton<Events>.Instance.onStartTutorial.Invoke(this._tutorialID, true);
	}

	// Token: 0x04000D0E RID: 3342
	public TextMeshProUGUI tutorialType;

	// Token: 0x04000D0F RID: 3343
	public TextMeshProUGUI tutorialName;

	// Token: 0x04000D10 RID: 3344
	public Image bar;

	// Token: 0x04000D11 RID: 3345
	public Image background;

	// Token: 0x04000D12 RID: 3346
	private string _tutorialID = "";
}
