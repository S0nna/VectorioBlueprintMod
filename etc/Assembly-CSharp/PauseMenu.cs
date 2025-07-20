using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.PhasmaUI;

// Token: 0x020001DB RID: 475
public class PauseMenu : UI_Window
{
	// Token: 0x06000EC9 RID: 3785 RVA: 0x00044090 File Offset: 0x00042290
	public void Start()
	{
		Singleton<Events>.Instance.onOpenPause.AddListener(new UnityAction(this.Toggle));
		Singleton<Events>.Instance.onClosePause.AddListener(new UnityAction(this.ForceClose));
		this.experimentalObj.SetActive(SaveSystem.IS_EXPERIMENTAL);
		this.experimentalVersion.text = "VERSION: v" + Application.version;
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x00044100 File Offset: 0x00042300
	public override void Open()
	{
		if (Hologram.MODE_SELECTED)
		{
			Singleton<Events>.Instance.onChangeBuildMode.Invoke(Hologram.BuildMode.Default);
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		float num = 0.1f;
		LeanTween.alphaCanvas(this.canvasGroup, 1f, 0.2f);
		if (this._saved)
		{
			this._saved = false;
			this.saveButtonOverride.SetActive(false);
		}
		foreach (MenuButton menuButton in this.buttons)
		{
			menuButton.group.GetComponent<RectTransform>().localPosition = menuButton.inPos;
			if (menuButton.sound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayDelayedInterfaceSound(menuButton.sound, num);
			}
			menuButton.group.alpha = 0f;
			LeanTween.alphaCanvas(menuButton.group, 1f, 0.2f).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, 0.2f).setEase(LeanTweenType.easeOutExpo).setDelay(num += 0.1f);
		}
		base.SetIsOpen(true);
		Singleton<Interface>.Instance.SetCurrentlyOpen(this);
		Singleton<Gamemode>.Instance.IsGamePaused = NetworkPlayerManager.ONLY_CLIENT_ON_SERVER;
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00044290 File Offset: 0x00042490
	public override void Close()
	{
		if (this.settings.canvasGroup.alpha > 0f)
		{
			this.OnBackFromSettings();
			return;
		}
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		float num = 0.05f;
		foreach (MenuButton menuButton in this.buttons)
		{
			LeanTween.alphaCanvas(menuButton.group, 0f, 0.1f).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, 0.1f).setEase(LeanTweenType.easeOutExpo).setDelay(num += 0.05f);
		}
		LeanTween.alphaCanvas(this.canvasGroup, 0f, 0.1f).setDelay(num);
		base.SetIsOpen(false);
		Singleton<Interface>.Instance.SetCurrentlyOpen(null);
		Singleton<Gamemode>.Instance.IsGamePaused = false;
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x000443A4 File Offset: 0x000425A4
	public override void ForceClose()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		base.SetIsOpen(false);
		Singleton<Interface>.Instance.SetCurrentlyOpen(null);
		Singleton<Gamemode>.Instance.IsGamePaused = false;
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x000443F8 File Offset: 0x000425F8
	public void OnSave()
	{
		if (!this._saved)
		{
			SaveData save = Singleton<SaveSystem>.Instance.GenerateWorldSave();
			Singleton<SaveSystem>.Instance.WriteSaveToFile(save);
			this._saved = true;
			this.saveButtonOverride.SetActive(true);
		}
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x00003212 File Offset: 0x00001412
	public void OnArmory()
	{
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x00044438 File Offset: 0x00042638
	public void OnSettings()
	{
		if (this.settings != null)
		{
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			this.settings.Open();
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00044488 File Offset: 0x00042688
	public void OnBackFromSettings()
	{
		if (this.settings != null)
		{
			this.settings.Close();
			this.canvasGroup.alpha = 1f;
			this.canvasGroup.interactable = true;
			this.canvasGroup.blocksRaycasts = true;
		}
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x000444D6 File Offset: 0x000426D6
	public void OnQuit()
	{
		Singleton<Events>.Instance.onDisableActionMap.Invoke();
		SaveSystem.SaveData = null;
		Singleton<EntityManager>.Instance.ClearAllEntities();
		Singleton<Lobby>.Instance.LeaveLobby();
	}

	// Token: 0x04000BD6 RID: 3030
	public SettingsUI settings;

	// Token: 0x04000BD7 RID: 3031
	public List<MenuButton> buttons;

	// Token: 0x04000BD8 RID: 3032
	public GameObject saveButtonOverride;

	// Token: 0x04000BD9 RID: 3033
	public GameObject experimentalObj;

	// Token: 0x04000BDA RID: 3034
	public GameObject tutorialListObj;

	// Token: 0x04000BDB RID: 3035
	public TextMeshProUGUI experimentalVersion;

	// Token: 0x04000BDC RID: 3036
	protected bool _saved;
}
