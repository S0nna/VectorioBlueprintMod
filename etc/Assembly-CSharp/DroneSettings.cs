using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001B3 RID: 435
public class DroneSettings : EntitySettings
{
	// Token: 0x06000DFE RID: 3582 RVA: 0x0003E31C File Offset: 0x0003C51C
	public override void Set(EntityComponent component)
	{
		Port port = component as Port;
		if (port != null)
		{
			CargoDrone cargoDrone = port.GetDrone as CargoDrone;
			if (cargoDrone != null)
			{
				this._cargoDrone = cargoDrone;
				List<ResourceData> list = (from resource in Library.RequestAllDataOfType<ResourceData>()
				where !Singleton<ResourceManager>.Instance.IsResourceIgnored(resource) && Singleton<Research>.Instance.IsResourceUnlocked(resource)
				orderby resource.Order
				select resource).ToList<ResourceData>();
				for (int i = 0; i < list.Count; i++)
				{
					ResourceData resource2 = list[i];
					FilterButton filterButton;
					if (i < this._filterButtons.Count)
					{
						filterButton = this._filterButtons[i];
						filterButton.gameObject.SetActive(true);
					}
					else
					{
						filterButton = Object.Instantiate<FilterButton>(this.filterButtonPrefab);
						filterButton.transform.SetParent(this.filterButtonList);
						filterButton.transform.localScale = Vector2.one;
						this._filterButtons.Add(filterButton);
					}
					Color color = (i % 2 == 0) ? this.colorOne : this.colorTwo;
					filterButton.Set(cargoDrone, resource2, color);
				}
				return;
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x0003E460 File Offset: 0x0003C660
	public override void CustomUpdate()
	{
		this.pickupTargets.text = ((this._cargoDrone.PickupCoverage != null) ? this._cargoDrone.PickupCoverage.targets.Count.ToString() : "0");
		this.dropoffTargets.text = ((this._cargoDrone.DropoffCoverage != null) ? this._cargoDrone.DropoffCoverage.targets.Count.ToString() : "0");
		this.deliveries.text = this._cargoDrone.Deliveries.ToString() + " DELIVERIES MADE";
		if (this._cargoDrone.DroneStatus == Drone.Status.TravellingToTarget)
		{
			if (this._cargoDrone.CurrentAction != null && this._cargoDrone.CurrentAction.Pickup)
			{
				this.portIcon.color = this.inactiveColor;
				this.pickupIcon.color = this.activeColor;
				this.dropoffIcon.color = this.inactiveColor;
			}
			else
			{
				this.portIcon.color = this.inactiveColor;
				this.pickupIcon.color = this.inactiveColor;
				this.dropoffIcon.color = this.activeColor;
			}
		}
		else
		{
			this.portIcon.color = this.activeColor;
			this.pickupIcon.color = this.inactiveColor;
			this.dropoffIcon.color = this.inactiveColor;
		}
		if (this._cargoDrone.Filter != null)
		{
			if (!this.filterSelectedObject.activeSelf)
			{
				this.filterSelectedObject.SetActive(true);
				this.noFilterSelectedObject.SetActive(false);
			}
			if (this._lastFilter == null || this._lastFilter != this._cargoDrone.Filter)
			{
				this._lastFilter = this._cargoDrone.Filter;
				this.filterIcon.sprite = this._lastFilter.IconSprite;
				this.filterName.text = this._lastFilter.Name.ToUpper();
				return;
			}
		}
		else if (this.filterSelectedObject.activeSelf)
		{
			this.filterSelectedObject.SetActive(false);
			this.noFilterSelectedObject.SetActive(true);
		}
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x0003E6A3 File Offset: 0x0003C8A3
	public void EditRoute()
	{
		Singleton<Interface>.Instance.CloseCurrentlyOpen();
		this._cargoDrone.OnQuickEdit();
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x0003E6BA File Offset: 0x0003C8BA
	public void SetFilter(ResourceData resource)
	{
		this._cargoDrone.SyncFilter(resource);
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x0003E6C8 File Offset: 0x0003C8C8
	public void DisableFilter()
	{
		this._cargoDrone.SyncFilter(null);
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x0003E6D8 File Offset: 0x0003C8D8
	public override void Clear()
	{
		for (int i = 0; i < this._filterButtons.Count; i++)
		{
			this._filterButtons[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x04000A93 RID: 2707
	private CargoDrone _cargoDrone;

	// Token: 0x04000A94 RID: 2708
	private ResourceData _lastFilter;

	// Token: 0x04000A95 RID: 2709
	public TextMeshProUGUI pickupTargets;

	// Token: 0x04000A96 RID: 2710
	public TextMeshProUGUI dropoffTargets;

	// Token: 0x04000A97 RID: 2711
	public TextMeshProUGUI deliveries;

	// Token: 0x04000A98 RID: 2712
	public Image portIcon;

	// Token: 0x04000A99 RID: 2713
	public Image pickupIcon;

	// Token: 0x04000A9A RID: 2714
	public Image dropoffIcon;

	// Token: 0x04000A9B RID: 2715
	public Color activeColor;

	// Token: 0x04000A9C RID: 2716
	public Color inactiveColor;

	// Token: 0x04000A9D RID: 2717
	public FilterButton filterButtonPrefab;

	// Token: 0x04000A9E RID: 2718
	public Transform filterButtonList;

	// Token: 0x04000A9F RID: 2719
	public GameObject noFilterSelectedObject;

	// Token: 0x04000AA0 RID: 2720
	public GameObject filterSelectedObject;

	// Token: 0x04000AA1 RID: 2721
	public Image filterIcon;

	// Token: 0x04000AA2 RID: 2722
	public TextMeshProUGUI filterName;

	// Token: 0x04000AA3 RID: 2723
	public Color colorOne;

	// Token: 0x04000AA4 RID: 2724
	public Color colorTwo;

	// Token: 0x04000AA5 RID: 2725
	private List<FilterButton> _filterButtons = new List<FilterButton>();
}
