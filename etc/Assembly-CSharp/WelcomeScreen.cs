using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000208 RID: 520
[DefaultExecutionOrder(0)]
public class WelcomeScreen : MonoBehaviour
{
	// Token: 0x06000F9B RID: 3995 RVA: 0x00049CF5 File Offset: 0x00047EF5
	public void Start()
	{
		Singleton<Events>.Instance.onFinsihLoading.AddListener(new UnityAction(this.Open));
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00049D12 File Offset: 0x00047F12
	public void PlayTutorial()
	{
		if (this._isOpen)
		{
			Singleton<Events>.Instance.onStartTutorial.Invoke(this.tutorialID, false);
			this.Close();
		}
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x00049D38 File Offset: 0x00047F38
	public void SkipTutorial()
	{
		if (this._isOpen)
		{
			Singleton<Events>.Instance.onUnlockAllTutorials.Invoke();
			this.Close();
		}
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x00049D58 File Offset: 0x00047F58
	public void Open()
	{
		if (!Singleton<Gamemode>.Instance.UseWelcomeScreen)
		{
			return;
		}
		LeanTween.alphaCanvas(this.canvasGroup, 1f, 0.5f);
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this._isOpen = true;
		Singleton<Gamemode>.Instance.IsGamePaused = NetworkPlayerManager.ONLY_CLIENT_ON_SERVER;
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x00049DB8 File Offset: 0x00047FB8
	public void Close()
	{
		if (this._isOpen && !this._hasClosed)
		{
			this._hasClosed = true;
			LeanTween.alphaCanvas(this.canvasGroup, 0f, 0.5f);
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			this._isOpen = false;
			Singleton<Gamemode>.Instance.IsGamePaused = false;
		}
	}

	// Token: 0x04000D5C RID: 3420
	public string tutorialID;

	// Token: 0x04000D5D RID: 3421
	public CanvasGroup canvasGroup;

	// Token: 0x04000D5E RID: 3422
	protected bool _isOpen;

	// Token: 0x04000D5F RID: 3423
	protected bool _hasClosed;
}
