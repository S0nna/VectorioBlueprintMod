using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class Beacon : EntityComponent, IComponent<Beacon, BeaconData>
{
	// Token: 0x060007B3 RID: 1971 RVA: 0x00022630 File Offset: 0x00020830
	public BeaconData GetData()
	{
		return this._beaconData;
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00022638 File Offset: 0x00020838
	public void OnInitialize(BeaconData data)
	{
		this._beaconData = data;
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00022644 File Offset: 0x00020844
	public override void OnSpawn(bool fromSave)
	{
		if (this._marker == null)
		{
			this._marker = Singleton<MarkerHandler>.Instance.CreateAndReturnMarker(base.transform.position, this._beaconData.defaultIcon.sprite, this._beaconData.defaultText, this._beaconData.defaultSize, "");
		}
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000226AA File Offset: 0x000208AA
	public IconData GetIcon()
	{
		return this._icon;
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x000226B2 File Offset: 0x000208B2
	public Sprite GetIconSprite()
	{
		return this._marker.icon.sprite;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x000226C4 File Offset: 0x000208C4
	public string GetTitle()
	{
		return this._marker.title.text;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x000226D6 File Offset: 0x000208D6
	public void SetIcon(IconData icon)
	{
		if (this._marker != null)
		{
			this._icon = icon;
			this._marker.icon.sprite = icon.sprite;
			return;
		}
		Debug.Log("[MARKER] Cannot set marker icon because the marker itself is null!");
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0002270E File Offset: 0x0002090E
	public void SetTitle(string title)
	{
		if (this._marker != null)
		{
			this._marker.title.text = title;
			return;
		}
		Debug.Log("[MARKER] Cannot set marker title because the marker itself is null!");
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0002273A File Offset: 0x0002093A
	public override void OnReset()
	{
		if (this._marker != null)
		{
			Singleton<MarkerHandler>.Instance.DestroyMarker(this._marker);
		}
	}

	// Token: 0x04000527 RID: 1319
	private BeaconData _beaconData;

	// Token: 0x04000528 RID: 1320
	private Marker _marker;

	// Token: 0x04000529 RID: 1321
	private IconData _icon;
}
