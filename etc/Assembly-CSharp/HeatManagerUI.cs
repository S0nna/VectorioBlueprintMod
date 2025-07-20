using System;
using UnityEngine;
using Vectorio.PhasmaUI;

// Token: 0x020001C1 RID: 449
public class HeatManagerUI : UI_Singleton<HeatManagerUI>
{
	// Token: 0x06000E49 RID: 3657 RVA: 0x00040048 File Offset: 0x0003E248
	public HeatSpawnUI CreateHeatSpawnUI(HeatManager.Enemy spawn)
	{
		HeatSpawnUI heatSpawnUI = Object.Instantiate<HeatSpawnUI>(this.heatSpawnPrefab, this.spawnList);
		heatSpawnUI.GetComponent<RectTransform>().localScale = Vector2.one;
		heatSpawnUI.Setup(spawn, (this.counter % 2 == 0) ? this.colorOne : this.colorTwo);
		this.counter++;
		return heatSpawnUI;
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x000400A8 File Offset: 0x0003E2A8
	public override void Open()
	{
		if (!Singleton<Interface>.Instance.CanOpenUI)
		{
			return;
		}
		base.SetIsOpen(true);
		Singleton<Interface>.Instance.SetCurrentlyOpen(this);
		if (this.canvasGroup.interactable)
		{
			return;
		}
		if (LeanTween.isTweening(this.canvasGroup.gameObject))
		{
			LeanTween.cancel(this.canvasGroup.gameObject);
		}
		LeanTween.alphaCanvas(this.canvasGroup, 1f, 0.25f);
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		if (this.openSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.openSound);
		}
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x00040150 File Offset: 0x0003E350
	public override void ForceOpen()
	{
		base.SetIsOpen(true);
		Singleton<Interface>.Instance.SetCurrentlyOpen(this);
		if (LeanTween.isTweening(this.canvasGroup.gameObject))
		{
			LeanTween.cancel(this.canvasGroup.gameObject);
		}
		this.canvasGroup.alpha = 1f;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x000401BC File Offset: 0x0003E3BC
	public override void Close()
	{
		base.SetIsOpen(false);
		Singleton<Interface>.Instance.SetCurrentlyOpen(null);
		if (LeanTween.isTweening(this.canvasGroup.gameObject))
		{
			LeanTween.cancel(this.canvasGroup.gameObject);
		}
		LeanTween.alphaCanvas(this.canvasGroup, 0f, 0.25f);
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		if (this.openSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.openSound);
		}
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x0004024C File Offset: 0x0003E44C
	public override void ForceClose()
	{
		base.SetIsOpen(false);
		Singleton<Interface>.Instance.SetCurrentlyOpen(null);
		if (LeanTween.isTweening(this.canvasGroup.gameObject))
		{
			LeanTween.cancel(this.canvasGroup.gameObject);
		}
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
	}

	// Token: 0x04000B1C RID: 2844
	public HeatSpawnUI heatSpawnPrefab;

	// Token: 0x04000B1D RID: 2845
	public Transform spawnList;

	// Token: 0x04000B1E RID: 2846
	public Color colorOne;

	// Token: 0x04000B1F RID: 2847
	public Color colorTwo;

	// Token: 0x04000B20 RID: 2848
	private int counter;
}
