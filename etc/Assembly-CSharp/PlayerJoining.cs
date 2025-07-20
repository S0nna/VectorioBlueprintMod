using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001DD RID: 477
public class PlayerJoining : MonoBehaviour
{
	// Token: 0x06000ED5 RID: 3797 RVA: 0x00044534 File Offset: 0x00042734
	public void Start()
	{
		Singleton<NetworkEvents>.Instance.onPlayerStartedLoading.AddListener(new UnityAction(this.OnPlayerStartJoining));
		Singleton<NetworkEvents>.Instance.onPlayerStoppedLoading.AddListener(new UnityAction(this.OnPlayerDoneJoining));
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0004456C File Offset: 0x0004276C
	public void OnPlayerStartJoining()
	{
		LeanTween.alphaCanvas(this.button.group, 1f, 0.5f);
		LeanTween.moveLocal(this.button.group.gameObject, this.button.normalPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
		if (this.button.sound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.button.sound);
		}
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x000445F0 File Offset: 0x000427F0
	public void OnPlayerDoneJoining()
	{
		LeanTween.alphaCanvas(this.button.group, 0f, 0.5f);
		LeanTween.moveLocal(this.button.group.gameObject, this.button.outPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
	}

	// Token: 0x04000BDD RID: 3037
	public MenuButton button;
}
