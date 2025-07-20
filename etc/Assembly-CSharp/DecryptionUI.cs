using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001AF RID: 431
public class DecryptionUI : UI_Window
{
	// Token: 0x06000DEE RID: 3566 RVA: 0x0003DE6F File Offset: 0x0003C06F
	public void Start()
	{
		Singleton<Events>.Instance.onOpenDecryption.AddListener(new UnityAction<Decryptor>(this.Open));
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x0003DE8C File Offset: 0x0003C08C
	public void Open(Decryptor decryptor)
	{
		base.SetIsOpen(true);
		if (decryptor.Tech == null)
		{
			Debug.Log("[DECRYPTOR] Interface cannot open decryptor with missing tech data!");
			return;
		}
		this._decryptor = decryptor;
		if (Singleton<Research>.Instance.IsDecryptorUnlocked(this._decryptor.Tech))
		{
			this.validObj.SetActive(true);
			this.invalidObj.SetActive(false);
			this.title.text = "BEGIN DECRYPTION?";
			this.subtitle.text = "Prepare to defend the decryption site.";
			this.techTitle.text = "DECRYPTS TECH";
			ResearchTechData tech = decryptor.Tech;
			if (tech != null)
			{
				this.techName.text = tech.Name.ToUpper();
			}
			this.title.color = this.normalTitleColor;
			this.background.color = this.normalBackgroundColor;
			this.panelBackground.color = this.normalPanelColor;
		}
		else
		{
			this.validObj.SetActive(false);
			this.invalidObj.SetActive(true);
			this.title.text = "UNKNOWN ENCRYPTION";
			this.subtitle.text = "This encryption has not been unlocked yet.";
			this.title.color = this.normalTitleColor;
			this.background.color = this.normalBackgroundColor;
		}
		LeanTween.alphaCanvas(this.canvasGroup, 1f, 0.15f);
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		Singleton<Interface>.Instance.SetCurrentlyOpen(this);
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0003E010 File Offset: 0x0003C210
	public override void Close()
	{
		base.SetIsOpen(false);
		LeanTween.alphaCanvas(this.canvasGroup, 0f, 0.15f);
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		Singleton<Interface>.Instance.SetCurrentlyOpen(null);
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0003E05D File Offset: 0x0003C25D
	public void StartDecryption()
	{
		this._decryptor.StartProcess();
		this.Close();
	}

	// Token: 0x04000A4D RID: 2637
	protected Decryptor _decryptor;

	// Token: 0x04000A4E RID: 2638
	public TextMeshProUGUI title;

	// Token: 0x04000A4F RID: 2639
	public TextMeshProUGUI subtitle;

	// Token: 0x04000A50 RID: 2640
	public TextMeshProUGUI techTitle;

	// Token: 0x04000A51 RID: 2641
	public TextMeshProUGUI techName;

	// Token: 0x04000A52 RID: 2642
	public TextMeshProUGUI techDesc;

	// Token: 0x04000A53 RID: 2643
	public Image background;

	// Token: 0x04000A54 RID: 2644
	public Image panelBackground;

	// Token: 0x04000A55 RID: 2645
	public GameObject validObj;

	// Token: 0x04000A56 RID: 2646
	public GameObject invalidObj;

	// Token: 0x04000A57 RID: 2647
	public Color normalTitleColor;

	// Token: 0x04000A58 RID: 2648
	public Color quantumTitleColor;

	// Token: 0x04000A59 RID: 2649
	public Color normalBackgroundColor;

	// Token: 0x04000A5A RID: 2650
	public Color quantumBackgroundColor;

	// Token: 0x04000A5B RID: 2651
	public Color normalPanelColor;

	// Token: 0x04000A5C RID: 2652
	public Color quantumPanelColor;
}
