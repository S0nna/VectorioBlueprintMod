using System;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;
using Vectorio.Utilities;

// Token: 0x0200012A RID: 298
public class RegionBooster : BuildingComponent, IComponent<RegionBooster, RegionBoosterData>
{
	// Token: 0x060009CD RID: 2509 RVA: 0x000292D7 File Offset: 0x000274D7
	public RegionBoosterData GetData()
	{
		return this._regionBoosterData;
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x000292E0 File Offset: 0x000274E0
	public override void ApplyVariableContainer(VariableContainer container)
	{
		if (this._isBoosted)
		{
			return;
		}
		float num;
		if (!container.TryGetFloat(0, out num))
		{
			return;
		}
		string variableID;
		if (!container.TryGetString(0, out variableID))
		{
			return;
		}
		this._variableID = variableID;
		Utilities.CalculateTileRange(base.transform, this._regionBoosterData.range, base.Building.Width, base.Building.Height);
		Object.Instantiate<GameObject>(this._boostAreaPrefab, base.transform.position, Quaternion.identity).transform.localScale = new Vector2((float)this._regionBoosterData.range + 1.5f, (float)this._regionBoosterData.range + 1.5f);
		this._isBoosted = true;
		string desc;
		float num2;
		if (container.TryGetString(1, out desc) && container.TryGetFloat(1, out num2))
		{
			Singleton<MarkerHandler>.Instance.CreateMarker(base.Entity.RuntimeID.ToString(), base.transform.position, new Vector2(8f, 8f), new Vector2(num2, num2), this._markerIcon, "REGION BOOSTER", desc);
			return;
		}
		Debug.Log("[BOOSTER] No marker data provided, skipping marker creation...");
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0002940D File Offset: 0x0002760D
	public void OnInitialize(RegionBoosterData data)
	{
		this._regionBoosterData = data;
		this._isMultiplier = data.isMultiplier;
		this._markerIcon = data.markerIcon;
		this._boostAreaPrefab = data.boostAreaPrefab;
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00003212 File Offset: 0x00001412
	public override void OnSpawn(bool fromSave)
	{
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0002943C File Offset: 0x0002763C
	public override void OnReset()
	{
		Singleton<MarkerHandler>.Instance.DestroyMarker(base.Entity.RuntimeID.ToString());
	}

	// Token: 0x04000604 RID: 1540
	private RegionBoosterData _regionBoosterData;

	// Token: 0x04000605 RID: 1541
	private StatModifier _modifier;

	// Token: 0x04000606 RID: 1542
	private string _variableID;

	// Token: 0x04000607 RID: 1543
	private Sprite _markerIcon;

	// Token: 0x04000608 RID: 1544
	private bool _isMultiplier;

	// Token: 0x04000609 RID: 1545
	private GameObject _boostAreaPrefab;

	// Token: 0x0400060A RID: 1546
	private bool _isBoosted;
}
