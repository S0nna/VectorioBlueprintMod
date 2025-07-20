using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x0200015B RID: 347
public class UI_GlobalResourceValue : Vectorio.PhasmaUI.Button
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00031672 File Offset: 0x0002F872
	public ResourceData Resource
	{
		get
		{
			return this._resource;
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0003167A File Offset: 0x0002F87A
	public virtual void Setup(UI_ResourceList parent, ResourceData resource, int amount)
	{
		this._parent = parent;
		this._resource = resource;
		this._icon.sprite = this._resource.IconSprite;
		this.UpdateAmount(amount);
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x000316A7 File Offset: 0x0002F8A7
	public void UpdateAmount()
	{
		this.UpdateAmount(Singleton<ResourceManager>.Instance.GetAmountByData(this._resource));
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x000316BF File Offset: 0x0002F8BF
	public virtual void UpdateAmount(int newAmount)
	{
		this._amount.text = newAmount.ToString();
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x000316D3 File Offset: 0x0002F8D3
	public override void ToggleHover(bool toggle)
	{
		base.ToggleHover(toggle);
		this._parent.OnHover(this._resource, base.transform.localPosition.x, toggle);
	}

	// Token: 0x040007CA RID: 1994
	private ResourceData _resource;

	// Token: 0x040007CB RID: 1995
	private UI_ResourceList _parent;

	// Token: 0x040007CC RID: 1996
	[SerializeField]
	private TextMeshProUGUI _amount;

	// Token: 0x040007CD RID: 1997
	[SerializeField]
	private Image _icon;
}
