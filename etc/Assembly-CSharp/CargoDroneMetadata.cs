using System;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x020000E3 RID: 227
[MetadataFor(typeof(CargoDrone))]
[Serializable]
public class CargoDroneMetadata : DroneMetadata
{
	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000729 RID: 1833 RVA: 0x00020E62 File Offset: 0x0001F062
	// (set) Token: 0x0600072A RID: 1834 RVA: 0x00020E6A File Offset: 0x0001F06A
	public CoverageArea PickupCoverage { get; set; }

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600072B RID: 1835 RVA: 0x00020E73 File Offset: 0x0001F073
	// (set) Token: 0x0600072C RID: 1836 RVA: 0x00020E7B File Offset: 0x0001F07B
	public CoverageArea DropoffCoverage { get; set; }

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600072D RID: 1837 RVA: 0x00020E84 File Offset: 0x0001F084
	// (set) Token: 0x0600072E RID: 1838 RVA: 0x00020E8C File Offset: 0x0001F08C
	public bool RF { get; set; }

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x0600072F RID: 1839 RVA: 0x00020E95 File Offset: 0x0001F095
	// (set) Token: 0x06000730 RID: 1840 RVA: 0x00020E9D File Offset: 0x0001F09D
	public string FilterID { get; set; } = "";

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x00020EA6 File Offset: 0x0001F0A6
	// (set) Token: 0x06000732 RID: 1842 RVA: 0x00020EAE File Offset: 0x0001F0AE
	public string ResourceID { get; set; } = "";

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000733 RID: 1843 RVA: 0x00020EB7 File Offset: 0x0001F0B7
	// (set) Token: 0x06000734 RID: 1844 RVA: 0x00020EBF File Offset: 0x0001F0BF
	public DroneActionPackage ActionPackage { get; set; }

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000735 RID: 1845 RVA: 0x00020EC8 File Offset: 0x0001F0C8
	// (set) Token: 0x06000736 RID: 1846 RVA: 0x00020ED0 File Offset: 0x0001F0D0
	public int StoredAmount { get; set; }

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x00020ED9 File Offset: 0x0001F0D9
	// (set) Token: 0x06000738 RID: 1848 RVA: 0x00020EE1 File Offset: 0x0001F0E1
	public int DeliveriesMade { get; set; }

	// Token: 0x06000739 RID: 1849 RVA: 0x00020EEA File Offset: 0x0001F0EA
	public override string ToString()
	{
		return string.Format("PickupCoverage: {0}\nDropoffCoverage: {1}", this.PickupCoverage, this.DropoffCoverage);
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00020F04 File Offset: 0x0001F104
	public override void GetValues(Drone drone, MetadataContext context)
	{
		base.GetValues(drone, context);
		CargoDrone cargoDrone = drone as CargoDrone;
		if (cargoDrone != null)
		{
			this.RF = cargoDrone.RequireFilter;
			ResourceData filter = cargoDrone.Filter;
			this.FilterID = (((filter != null) ? filter.ID : null) ?? "");
			ResourceData resource = cargoDrone.Resource;
			this.ResourceID = (((resource != null) ? resource.ID : null) ?? "");
			this.ActionPackage = new DroneActionPackage(cargoDrone.CurrentAction, cargoDrone.Actions, cargoDrone.Resource, context);
			if (cargoDrone.PickupCoverage != null)
			{
				if (context == MetadataContext.Local)
				{
					Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(drone.Parent.transform.position);
					CoverageArea pickupCoverage = new CoverageArea
					{
						startX = cargoDrone.PickupCoverage.area.startX - vector2Int.x,
						endX = cargoDrone.PickupCoverage.area.endX - vector2Int.x,
						startY = cargoDrone.PickupCoverage.area.startY - vector2Int.y,
						endY = cargoDrone.PickupCoverage.area.endY - vector2Int.y
					};
					this.PickupCoverage = pickupCoverage;
				}
				else
				{
					this.PickupCoverage = cargoDrone.PickupCoverage.area;
				}
			}
			if (cargoDrone.DropoffCoverage != null)
			{
				if (context == MetadataContext.Local)
				{
					Vector2Int vector2Int2 = Utilities.ConvertWorldPositionToCell(drone.Parent.transform.position);
					CoverageArea dropoffCoverage = new CoverageArea
					{
						startX = cargoDrone.DropoffCoverage.area.startX - vector2Int2.x,
						endX = cargoDrone.DropoffCoverage.area.endX - vector2Int2.x,
						startY = cargoDrone.DropoffCoverage.area.startY - vector2Int2.y,
						endY = cargoDrone.DropoffCoverage.area.endY - vector2Int2.y
					};
					this.DropoffCoverage = dropoffCoverage;
				}
				else
				{
					this.DropoffCoverage = cargoDrone.DropoffCoverage.area;
				}
			}
			this.DeliveriesMade = cargoDrone.Deliveries;
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002112C File Offset: 0x0001F32C
	public override void SetValues(Drone drone, bool asPipette, MetadataContext context)
	{
		base.SetValues(drone, asPipette, context);
		CargoDrone cargoDrone = drone as CargoDrone;
		if (cargoDrone != null)
		{
			cargoDrone.RequireFilter = this.RF;
			CoverageArea pickup = null;
			CoverageArea dropoff = null;
			if (this.FilterID != null && this.FilterID != "")
			{
				ResourceData resourceData = Library.RequestData<ResourceData>(this.FilterID);
				if (resourceData != null)
				{
					cargoDrone.Filter = resourceData;
				}
			}
			else if (cargoDrone.IsBusy)
			{
				cargoDrone.ReturnToParent();
			}
			if (this.PickupCoverage != null)
			{
				if (context == MetadataContext.Local)
				{
					if (drone.Parent == null)
					{
						Debug.Log("[CARGO DRONE] No parent port was provided!");
						return;
					}
					Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(drone.Parent.transform.position);
					pickup = new CoverageArea
					{
						startX = vector2Int.x + this.PickupCoverage.startX,
						endX = vector2Int.x + this.PickupCoverage.endX,
						startY = vector2Int.y + this.PickupCoverage.startY,
						endY = vector2Int.y + this.PickupCoverage.endY
					};
				}
				else
				{
					pickup = this.PickupCoverage;
				}
			}
			if (this.DropoffCoverage != null)
			{
				if (context == MetadataContext.Local)
				{
					Vector2Int vector2Int2 = Utilities.ConvertWorldPositionToCell(drone.Parent.transform.position);
					dropoff = new CoverageArea
					{
						startX = vector2Int2.x + this.DropoffCoverage.startX,
						endX = vector2Int2.x + this.DropoffCoverage.endX,
						startY = vector2Int2.y + this.DropoffCoverage.startY,
						endY = vector2Int2.y + this.DropoffCoverage.endY
					};
				}
				else
				{
					dropoff = this.DropoffCoverage;
				}
			}
			cargoDrone.CreateCoverage(pickup, dropoff);
			if (this.ResourceID != null && this.ResourceID != "")
			{
				ResourceData resourceData2 = Library.RequestData<ResourceData>(this.ResourceID);
				if (resourceData2 != null)
				{
					cargoDrone.Resource = resourceData2;
				}
			}
			if (!asPipette && this.ActionPackage != null)
			{
				cargoDrone.LoadActionPackage(this.ActionPackage);
				return;
			}
		}
		else
		{
			base.SetValues(drone, asPipette, context);
		}
	}
}
