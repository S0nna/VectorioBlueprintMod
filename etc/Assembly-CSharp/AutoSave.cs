using System;
using TMPro;
using UnityEngine;
using Vectorio.PhasmaUI;
using Vectorio.Settings;

// Token: 0x02000144 RID: 324
public class AutoSave : Button
{
	// Token: 0x06000ABA RID: 2746 RVA: 0x0002D12C File Offset: 0x0002B32C
	public void Awake()
	{
		this.stage = AutoSave.Stage.Waiting;
		this.time = this.cooldown;
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0002D144 File Offset: 0x0002B344
	public void Update()
	{
		this.time -= Time.deltaTime;
		switch (this.stage)
		{
		case AutoSave.Stage.Waiting:
			if (this.time <= 0f)
			{
				FlagSetting flagSetting;
				if (Singleton<Settings>.Instance.TryGetSetting<FlagSetting>("setting_auto_save", out flagSetting) && !flagSetting.Value)
				{
					Debug.Log("[AUTO SAVE] Attempted to save, but auto save setting is disabled.");
					this.ResetTimer();
					return;
				}
				this.stage = AutoSave.Stage.Countdown;
				this.time = 10f;
				LeanTween.alphaCanvas(this.button.group, 1f, 0.5f);
				LeanTween.moveLocal(this.button.group.gameObject, this.button.normalPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
				this.button.group.interactable = true;
				this.button.group.blocksRaycasts = true;
				if (this.activatedSound != null)
				{
					Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.activatedSound);
				}
				this.normal.SetActive(true);
				this.saved.SetActive(false);
				this.cancelled.SetActive(false);
				return;
			}
			break;
		case AutoSave.Stage.Countdown:
			this.countdown.text = "AUTO SAVING IN... " + Mathf.RoundToInt(this.time).ToString() + "s";
			if (this.time <= 0f)
			{
				this.Save();
				return;
			}
			break;
		case AutoSave.Stage.Saved:
			if (this.time <= 0f)
			{
				this.ResetTimer();
				return;
			}
			break;
		case AutoSave.Stage.Cancelled:
			if (this.time <= 0f)
			{
				this.ResetTimer();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0002D2EC File Offset: 0x0002B4EC
	public void Save()
	{
		this.stage = AutoSave.Stage.Saved;
		this.time = 2f;
		SaveData save = Singleton<SaveSystem>.Instance.GenerateWorldSave();
		Singleton<SaveSystem>.Instance.WriteSaveToFile(save);
		if (this.savedSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.savedSound);
		}
		this.normal.SetActive(false);
		this.saved.SetActive(true);
		this.cancelled.SetActive(false);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0002D363 File Offset: 0x0002B563
	public void Cancel()
	{
		this.stage = AutoSave.Stage.Cancelled;
		this.time = 3f;
		this.normal.SetActive(false);
		this.saved.SetActive(false);
		this.cancelled.SetActive(true);
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0002D39C File Offset: 0x0002B59C
	public void ResetTimer()
	{
		this.stage = AutoSave.Stage.Waiting;
		this.time = this.cooldown;
		LeanTween.alphaCanvas(this.button.group, 0f, 0.5f);
		LeanTween.moveLocal(this.button.group.gameObject, this.button.outPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
		this.button.group.interactable = false;
		this.button.group.blocksRaycasts = false;
	}

	// Token: 0x040006CD RID: 1741
	public MenuButton button;

	// Token: 0x040006CE RID: 1742
	public AudioClip activatedSound;

	// Token: 0x040006CF RID: 1743
	public AudioClip savedSound;

	// Token: 0x040006D0 RID: 1744
	public TextMeshProUGUI countdown;

	// Token: 0x040006D1 RID: 1745
	public GameObject normal;

	// Token: 0x040006D2 RID: 1746
	public GameObject saved;

	// Token: 0x040006D3 RID: 1747
	public GameObject cancelled;

	// Token: 0x040006D4 RID: 1748
	public float cooldown = 30f;

	// Token: 0x040006D5 RID: 1749
	protected float time;

	// Token: 0x040006D6 RID: 1750
	protected AutoSave.Stage stage;

	// Token: 0x02000145 RID: 325
	public enum Stage
	{
		// Token: 0x040006D8 RID: 1752
		Waiting,
		// Token: 0x040006D9 RID: 1753
		Countdown,
		// Token: 0x040006DA RID: 1754
		Saved,
		// Token: 0x040006DB RID: 1755
		Cancelled
	}
}
