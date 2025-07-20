using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001A1 RID: 417
public class BeaconSettings : EntitySettings
{
	// Token: 0x06000DBD RID: 3517 RVA: 0x0003CE7C File Offset: 0x0003B07C
	public override void Set(EntityComponent component)
	{
		Beacon beacon = component as Beacon;
		if (beacon != null)
		{
			this._beacon = beacon;
			List<IconData> list = Library.RequestAllDataOfType<IconData>();
			for (int i = 0; i < list.Count; i++)
			{
				MarkerButton markerButton;
				if (i < this._filterButtons.Count)
				{
					markerButton = this._filterButtons[i];
					markerButton.gameObject.SetActive(true);
				}
				else
				{
					markerButton = Object.Instantiate<MarkerButton>(this.marketButtonPrefab);
					markerButton.transform.SetParent(this.markerButtonList);
					markerButton.transform.localScale = Vector2.one;
				}
				Color color = (i % 2 == 0) ? this.colorOne : this.colorTwo;
				markerButton.Set(this, list[i], color);
			}
			this.inputField.text = beacon.GetTitle();
			this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.SetTitle));
			return;
		}
		Debug.LogWarning("The provided component does not match the setting type.");
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x0003CF74 File Offset: 0x0003B174
	public override void CustomUpdate()
	{
		if (this._beacon.GetIconSprite() != this.markerIcon)
		{
			this.markerIcon.sprite = this._beacon.GetIconSprite();
		}
		if (this._beacon.GetTitle() != this.markerName.text)
		{
			this.markerName.text = this._beacon.GetTitle();
		}
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0003CFE2 File Offset: 0x0003B1E2
	public void SetIcon(IconData icon)
	{
		this._beacon.SetIcon(icon);
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0003CFF0 File Offset: 0x0003B1F0
	public void SetTitle(string text)
	{
		this._beacon.SetTitle(text);
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0003D000 File Offset: 0x0003B200
	public override void Clear()
	{
		this.inputField.onValueChanged.RemoveListener(new UnityAction<string>(this.SetTitle));
		for (int i = 0; i < this._filterButtons.Count; i++)
		{
			this._filterButtons[i].gameObject.SetActive(false);
		}
		this._beacon = null;
	}

	// Token: 0x040009EF RID: 2543
	private Beacon _beacon;

	// Token: 0x040009F0 RID: 2544
	public MarkerButton marketButtonPrefab;

	// Token: 0x040009F1 RID: 2545
	public Transform markerButtonList;

	// Token: 0x040009F2 RID: 2546
	public TMP_InputField inputField;

	// Token: 0x040009F3 RID: 2547
	public TextMeshProUGUI markerName;

	// Token: 0x040009F4 RID: 2548
	public Image markerIcon;

	// Token: 0x040009F5 RID: 2549
	public Color colorOne;

	// Token: 0x040009F6 RID: 2550
	public Color colorTwo;

	// Token: 0x040009F7 RID: 2551
	private List<MarkerButton> _filterButtons = new List<MarkerButton>();
}
