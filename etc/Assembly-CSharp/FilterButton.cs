using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001BA RID: 442
public class FilterButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000E1D RID: 3613 RVA: 0x0003EF78 File Offset: 0x0003D178
	private void SetDesign(ResourceData resource, Color color)
	{
		this._resource = resource;
		this.icon.sprite = resource.IconSprite;
		this.title.text = resource.Name.ToUpper();
		this.background.color = color;
		if (this.useTier)
		{
			this.tier.text = "TIER " + resource.Tier.ToString() + " RESOURCE";
			this.tierBackground.color = resource.Accent.primaryColor;
			this.tier.color = new Color(resource.Accent.primaryColor.r * 0.2f, resource.Accent.primaryColor.g * 0.2f, resource.Accent.primaryColor.b * 0.2f);
		}
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0003F05A File Offset: 0x0003D25A
	public void Set(CargoDrone parent, ResourceData resource, Color color)
	{
		this._cargoDrone = parent;
		this._useDrone = true;
		this._useSettings = false;
		this.SetDesign(resource, color);
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0003F079 File Offset: 0x0003D279
	public void Set(DroneSettings parent, ResourceData resource, Color color)
	{
		this._droneSettings = parent;
		this._useSettings = true;
		this._useDrone = false;
		this.SetDesign(resource, color);
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0003F098 File Offset: 0x0003D298
	public override void OnClick()
	{
		if (this._useSettings)
		{
			this._droneSettings.SetFilter(this._resource);
		}
		else if (this._useDrone)
		{
			this._cargoDrone.SyncFilter(this._resource);
		}
		base.OnClick();
	}

	// Token: 0x04000ADF RID: 2783
	private DroneSettings _droneSettings;

	// Token: 0x04000AE0 RID: 2784
	private bool _useSettings;

	// Token: 0x04000AE1 RID: 2785
	private CargoDrone _cargoDrone;

	// Token: 0x04000AE2 RID: 2786
	private bool _useDrone;

	// Token: 0x04000AE3 RID: 2787
	private ResourceData _resource;

	// Token: 0x04000AE4 RID: 2788
	public Image icon;

	// Token: 0x04000AE5 RID: 2789
	public Image background;

	// Token: 0x04000AE6 RID: 2790
	public TextMeshProUGUI title;

	// Token: 0x04000AE7 RID: 2791
	public bool useTier = true;

	// Token: 0x04000AE8 RID: 2792
	public TextMeshProUGUI tier;

	// Token: 0x04000AE9 RID: 2793
	public Image tierBackground;
}
