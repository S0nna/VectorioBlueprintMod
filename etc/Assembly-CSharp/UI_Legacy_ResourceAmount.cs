using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x0200015C RID: 348
public class UI_Legacy_ResourceAmount : Vectorio.PhasmaUI.Button
{
	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x000316FE File Offset: 0x0002F8FE
	public ResourceData GetResourceData
	{
		get
		{
			return this._resource;
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00031708 File Offset: 0x0002F908
	public virtual UI_Legacy_ResourceAmount Setup(ResourceData resource, int amount, int storage)
	{
		this._resource = resource;
		if (this.useManualScaling)
		{
			this._rectTransform = base.GetComponent<RectTransform>();
		}
		if (this.isGenerated)
		{
			this.icon.sprite = this._resource.IconSprite;
			this.UpdateAmount(amount);
			this.UpdateStorage(storage);
		}
		return this;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00031760 File Offset: 0x0002F960
	public virtual void UpdateAmount(int newAmount)
	{
		this._amount = newAmount.ToString();
		if (this._showStorage)
		{
			this.amount.text = this._amount + " <size=4>/" + this._storage;
			if (this.useProgressBar)
			{
				this.progressBar.currentPercent = (float)newAmount;
				this.progressBar.UpdateUI();
			}
		}
		else
		{
			this.amount.text = this._amount;
		}
		if (this.useManualScaling && this._scaleValue != this.amount.text.Length)
		{
			this._scaleValue = this.amount.text.Length;
			float x = this._baseScale + this._scaleMultiplier * (float)this._scaleValue;
			this._rectTransform.sizeDelta = new Vector2(x, this._rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00031844 File Offset: 0x0002FA44
	public virtual void UpdateStorage(int newAmount)
	{
		this._storage = newAmount.ToString();
		if (this._showStorage)
		{
			this.amount.text = this._amount + " <size=4>/" + this._storage;
			if (this.useProgressBar)
			{
				this.progressBar.maxValue = (float)newAmount;
				this.progressBar.UpdateUI();
			}
		}
	}

	// Token: 0x040007CE RID: 1998
	protected ResourceData _resource;

	// Token: 0x040007CF RID: 1999
	public TextMeshProUGUI title;

	// Token: 0x040007D0 RID: 2000
	public TextMeshProUGUI amount;

	// Token: 0x040007D1 RID: 2001
	public Image icon;

	// Token: 0x040007D2 RID: 2002
	public bool useProgressBar;

	// Token: 0x040007D3 RID: 2003
	public ProgressBar progressBar;

	// Token: 0x040007D4 RID: 2004
	public bool useManualScaling;

	// Token: 0x040007D5 RID: 2005
	private float _scaleMultiplier = 3.72f;

	// Token: 0x040007D6 RID: 2006
	private float _baseScale = 15f;

	// Token: 0x040007D7 RID: 2007
	private RectTransform _rectTransform;

	// Token: 0x040007D8 RID: 2008
	private int _scaleValue;

	// Token: 0x040007D9 RID: 2009
	public bool isGenerated = true;

	// Token: 0x040007DA RID: 2010
	protected string _amount = "0";

	// Token: 0x040007DB RID: 2011
	protected string _storage = "0";

	// Token: 0x040007DC RID: 2012
	protected bool _showStorage;
}
