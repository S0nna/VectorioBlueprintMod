using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class CosmeticPanel : MonoBehaviour
{
	// Token: 0x06000ACA RID: 2762 RVA: 0x0002D5B4 File Offset: 0x0002B7B4
	public void Awake()
	{
		this._canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00003212 File Offset: 0x00001412
	public void Open(EntityData entity)
	{
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0002D5C2 File Offset: 0x0002B7C2
	public void Toggle(bool toggle)
	{
		this._canvasGroup.alpha = (toggle ? 1f : 0f);
		this._canvasGroup.interactable = toggle;
		this._canvasGroup.blocksRaycasts = toggle;
	}

	// Token: 0x040006EF RID: 1775
	public CosmeticButton cosmeticButton;

	// Token: 0x040006F0 RID: 1776
	public Transform cosmeticListParent;

	// Token: 0x040006F1 RID: 1777
	protected List<CosmeticButton> _buttons = new List<CosmeticButton>();

	// Token: 0x040006F2 RID: 1778
	protected CanvasGroup _canvasGroup;
}
