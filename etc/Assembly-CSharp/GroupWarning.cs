using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.PhasmaUI;

// Token: 0x0200014B RID: 331
public class GroupWarning : Button
{
	// Token: 0x06000AD8 RID: 2776 RVA: 0x0002DCC4 File Offset: 0x0002BEC4
	public override void Start()
	{
		Singleton<Events>.Instance.onEnemyGroupSpawned.AddListener(new UnityAction<Vector2>(this.OpenWarning));
		base.Start();
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0002DCE7 File Offset: 0x0002BEE7
	public void Update()
	{
		if (this._isOpen)
		{
			this._time -= Time.deltaTime;
			if (this._time <= 0f)
			{
				this.CloseWarning();
			}
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x0002DD18 File Offset: 0x0002BF18
	public void OpenWarning(Vector2 position)
	{
		if (this.activatedSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.activatedSound);
		}
		LeanTween.alphaCanvas(this.button.group, 1f, 0.5f);
		LeanTween.moveLocal(this.button.group.gameObject, this.button.normalPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
		this.button.group.interactable = true;
		this.button.group.blocksRaycasts = true;
		this._position = position;
		this._isOpen = true;
		this._time = 7f;
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0002DDCB File Offset: 0x0002BFCB
	public void MoveToGroup()
	{
		Singleton<Events>.Instance.onMoveCameraToPosition.Invoke(this._position);
		this.CloseWarning();
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0002DDE8 File Offset: 0x0002BFE8
	public void CloseWarning()
	{
		LeanTween.alphaCanvas(this.button.group, 0f, 0.5f);
		LeanTween.moveLocal(this.button.group.gameObject, this.button.outPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
		this.button.group.interactable = false;
		this.button.group.blocksRaycasts = false;
		this._isOpen = false;
		this._time = 0f;
	}

	// Token: 0x04000707 RID: 1799
	public MenuButton button;

	// Token: 0x04000708 RID: 1800
	public AudioClip activatedSound;

	// Token: 0x04000709 RID: 1801
	public TextMeshProUGUI description;

	// Token: 0x0400070A RID: 1802
	protected float _time = 7f;

	// Token: 0x0400070B RID: 1803
	protected bool _isOpen;

	// Token: 0x0400070C RID: 1804
	protected Vector2 _position;
}
