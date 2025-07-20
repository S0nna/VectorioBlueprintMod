using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Generation;

// Token: 0x0200007D RID: 125
public class CellData
{
	// Token: 0x0600059F RID: 1439 RVA: 0x0001E779 File Offset: 0x0001C979
	public void SetBiomeIndex(byte biomeIndex)
	{
		this._biomeIndex = biomeIndex;
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0001E782 File Offset: 0x0001C982
	public byte GetBiomeIndex()
	{
		return this._biomeIndex;
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0001E78A File Offset: 0x0001C98A
	public BiomeData GetBiome()
	{
		return Singleton<WorldGenerator>.Instance.GetBiome(this._biomeIndex);
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0001E79C File Offset: 0x0001C99C
	public void SetOccupier(Building building)
	{
		this._occupier = building;
		if (this._droneCoverage != null)
		{
			foreach (DroneCoverage droneCoverage in this._droneCoverage)
			{
				droneCoverage.AddTarget(this._occupier.Entity);
			}
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0001E808 File Offset: 0x0001CA08
	public void UpdateOccupier()
	{
		if (this._droneCoverage != null)
		{
			foreach (DroneCoverage droneCoverage in this._droneCoverage)
			{
				droneCoverage.AddTarget(this._occupier.Entity);
			}
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001E86C File Offset: 0x0001CA6C
	public bool IsOccupied
	{
		get
		{
			return this.GetOccupier != null;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001E87A File Offset: 0x0001CA7A
	public Building GetOccupier
	{
		get
		{
			return this._occupier;
		}
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0001E884 File Offset: 0x0001CA84
	public void ClearOccupier()
	{
		if (this._droneCoverage != null && this.IsOccupied)
		{
			foreach (DroneCoverage droneCoverage in this._droneCoverage)
			{
				droneCoverage.RemoveTarget(this._occupier.Entity);
			}
		}
		this._occupier = null;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x0001E8F8 File Offset: 0x0001CAF8
	public void AddClaimer(Claimer claimer)
	{
		if (this._claimers == null)
		{
			this._claimers = new List<Claimer>();
		}
		this._claimers.Add(claimer);
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0001E919 File Offset: 0x0001CB19
	public bool CheckForClaimer(Claimer claimer)
	{
		return this._claimers.Contains(claimer);
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001E927 File Offset: 0x0001CB27
	public int GetClaimerAmount
	{
		get
		{
			if (this._claimers == null)
			{
				return 0;
			}
			return this._claimers.Count;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060005AA RID: 1450 RVA: 0x0001E93E File Offset: 0x0001CB3E
	public bool IsCellClaimed
	{
		get
		{
			return this._claimers != null && this._claimers.Count > 0;
		}
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0001E958 File Offset: 0x0001CB58
	public void RemoveClaimer(Claimer claimer)
	{
		this._claimers.Remove(claimer);
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0001E968 File Offset: 0x0001CB68
	public void RegisterDroneCoverage(DroneCoverage coverage)
	{
		if (this._droneCoverage == null)
		{
			this._droneCoverage = new List<DroneCoverage>();
		}
		else if (this._droneCoverage.Contains(coverage))
		{
			return;
		}
		this._droneCoverage.Add(coverage);
		if (this._occupier == null)
		{
			return;
		}
		coverage.AddTarget(this._occupier.Entity);
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0001E9C5 File Offset: 0x0001CBC5
	public void ClearDroneCoverage(DroneCoverage coverage)
	{
		if (this._droneCoverage == null)
		{
			return;
		}
		if (!this._droneCoverage.Contains(coverage))
		{
			return;
		}
		if (this.IsOccupied)
		{
			coverage.RemoveTarget(this._occupier.Entity);
		}
		this._droneCoverage.Remove(coverage);
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0001EA08 File Offset: 0x0001CC08
	public void GetDronesOfType<T>(CoverageType type, ref List<T> drones) where T : Drone
	{
		if (this._droneCoverage == null)
		{
			return;
		}
		foreach (DroneCoverage droneCoverage in this._droneCoverage)
		{
			if (droneCoverage.type == type && !droneCoverage.drone.IsBusy)
			{
				T component = droneCoverage.drone.GetComponent<T>();
				if (component != null && !drones.Contains(component))
				{
					drones.Add(component);
				}
			}
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001EAA0 File Offset: 0x0001CCA0
	// (set) Token: 0x060005B0 RID: 1456 RVA: 0x0001EAA8 File Offset: 0x0001CCA8
	public ResourceData Resource
	{
		get
		{
			return this._resource;
		}
		set
		{
			this._resource = value;
			this._hasResource = (this._resource != null);
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060005B1 RID: 1457 RVA: 0x0001EAC3 File Offset: 0x0001CCC3
	public bool HasResource
	{
		get
		{
			return this._hasResource;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0001EACB File Offset: 0x0001CCCB
	public bool HasTileDesign
	{
		get
		{
			return this._hasTileDesign;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0001EAD3 File Offset: 0x0001CCD3
	public bool HasOriginalDesign
	{
		get
		{
			return this._hasOriginalDesign;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0001EAF3 File Offset: 0x0001CCF3
	// (set) Token: 0x060005B4 RID: 1460 RVA: 0x0001EADB File Offset: 0x0001CCDB
	public TileDesign TileDesign
	{
		get
		{
			return this._tileDesign;
		}
		set
		{
			this._tileDesign = value;
			this._hasTileDesign = (this._tileDesign != null);
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0001EB13 File Offset: 0x0001CD13
	// (set) Token: 0x060005B6 RID: 1462 RVA: 0x0001EAFB File Offset: 0x0001CCFB
	public TileDesign OriginalDesign
	{
		get
		{
			return this._originalDesign;
		}
		set
		{
			this._originalDesign = value;
			this._hasOriginalDesign = (this._originalDesign != null);
		}
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0001EB1B File Offset: 0x0001CD1B
	public void ClearTileDesign()
	{
		this.TileDesign = null;
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0001EB24 File Offset: 0x0001CD24
	public TileDesign GenerateDesign()
	{
		return new TileDesign(this.DetermineTile(), this.DetermineTileColor(), this.DetermineMapColor());
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0001EB3D File Offset: 0x0001CD3D
	private TileDesignData DetermineTile()
	{
		if (this.HasResource)
		{
			return this._resource.Tile;
		}
		if (this._hasTileDesign)
		{
			return this._tileDesign.data;
		}
		return this._originalDesign.data;
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0001EB74 File Offset: 0x0001CD74
	private Color DetermineTileColor()
	{
		if (this.HasResource)
		{
			return Color.white;
		}
		if (this._hasTileDesign)
		{
			return this._tileDesign.color;
		}
		if (this.IsCellClaimed)
		{
			return Singleton<TileGrid>.Instance.GetRandomClaimColor();
		}
		return this._originalDesign.color;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0001EBC4 File Offset: 0x0001CDC4
	private Color DetermineMapColor()
	{
		if (this.HasResource)
		{
			return this._resource.Accent.primaryColor;
		}
		if (this._hasTileDesign)
		{
			return this._tileDesign.color;
		}
		if (this.IsCellClaimed)
		{
			return Singleton<TileGrid>.Instance.GetRandomClaimColor();
		}
		return this._originalDesign.color;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0001EC1C File Offset: 0x0001CE1C
	public void Clear()
	{
		this._biomeIndex = 0;
		this._occupier = null;
		if (this._claimers != null)
		{
			this._claimers.Clear();
		}
		if (this._droneCoverage != null)
		{
			this._droneCoverage.Clear();
		}
		this._resource = null;
		this._hasResource = false;
		this._tileDesign = null;
		this._hasTileDesign = false;
	}

	// Token: 0x0400033C RID: 828
	private byte _biomeIndex;

	// Token: 0x0400033D RID: 829
	private Building _occupier;

	// Token: 0x0400033E RID: 830
	private List<Claimer> _claimers;

	// Token: 0x0400033F RID: 831
	private List<DroneCoverage> _droneCoverage;

	// Token: 0x04000340 RID: 832
	private ResourceData _resource;

	// Token: 0x04000341 RID: 833
	private bool _hasResource;

	// Token: 0x04000342 RID: 834
	private TileDesign _tileDesign;

	// Token: 0x04000343 RID: 835
	private TileDesign _originalDesign;

	// Token: 0x04000344 RID: 836
	private bool _hasTileDesign;

	// Token: 0x04000345 RID: 837
	private bool _hasOriginalDesign;

	// Token: 0x0200007E RID: 126
	public class NearbyBuilding
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x0001EC79 File Offset: 0x0001CE79
		public NearbyBuilding(Building building, Type constraint)
		{
			this.building = building;
			this.constraint = constraint;
		}

		// Token: 0x04000346 RID: 838
		public Building building;

		// Token: 0x04000347 RID: 839
		public Type constraint;
	}
}
