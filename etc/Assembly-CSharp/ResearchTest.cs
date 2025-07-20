using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000072 RID: 114
[DefaultExecutionOrder(0)]
public class ResearchTest : Singleton<ResearchTest>
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001B9AF File Offset: 0x00019BAF
	public bool IsResearching
	{
		get
		{
			return this._activeTech != null;
		}
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0001B9BD File Offset: 0x00019BBD
	public void StartResearch()
	{
		if (this._activeTech != null)
		{
			this.SetActiveTech(this._activeTech, this._activeTech.costs);
			Debug.Log("[RESEARCH TEST] Test tech activated.");
			return;
		}
		Debug.Log("[RESEARCH TEST] Could not activate research.");
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0001B9FC File Offset: 0x00019BFC
	public void FinishResearch()
	{
		if (this.IsResearching)
		{
			Debug.Log("[RESEARCH TEST] Finished researching " + this._activeTech.ID);
			Singleton<Events>.Instance.onResearchTechFinished.Invoke(this._activeTech);
			this._activeTech = null;
			return;
		}
		Debug.Log("[RESEARCH TEST] No research to finish.");
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001BA52 File Offset: 0x00019C52
	public ResearchTechData ActiveTech
	{
		get
		{
			return this._activeTech;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001BA5A File Offset: 0x00019C5A
	// (set) Token: 0x06000539 RID: 1337 RVA: 0x0001BA62 File Offset: 0x00019C62
	public Dictionary<ResourceData, int> TechRequirements
	{
		get
		{
			return this._techRequirements;
		}
		set
		{
			this._techRequirements = value;
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0001BA6C File Offset: 0x00019C6C
	public void SetActiveTech(ResearchTechData tech, List<Cost> costs)
	{
		this._activeTech = tech;
		this._techRequirements.Clear();
		foreach (Cost cost in costs)
		{
			this._techRequirements.Add(cost.resource, cost.amount);
		}
		Singleton<Events>.Instance.onResearchTechActivated.Invoke(tech, costs);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0001BAF0 File Offset: 0x00019CF0
	public bool HasTechResource(ResourceData resource)
	{
		return this._techRequirements.ContainsKey(resource);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0001BB00 File Offset: 0x00019D00
	public void AddTechResource(ResourceData resource, int amount)
	{
		if (this._techRequirements.ContainsKey(resource))
		{
			Dictionary<ResourceData, int> techRequirements = this._techRequirements;
			techRequirements[resource] -= amount;
			if (this._techRequirements[resource] <= 0)
			{
				Debug.Log("[RESEARCH TEST] Resource " + resource.ID + " has been finished.");
				this._techRequirements.Remove(resource);
			}
			if (this._techRequirements.Count == 0)
			{
				Debug.Log("[RESEARCH TEST] Finished researching " + this._activeTech.ID);
				Singleton<Events>.Instance.onResearchTechFinished.Invoke(this._activeTech);
				this._activeTech = null;
			}
		}
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x0001BBB0 File Offset: 0x00019DB0
	public List<CostData> GetTechResources()
	{
		List<CostData> list = new List<CostData>();
		if (this._activeTech != null)
		{
			foreach (Cost cost in this._activeTech.costs)
			{
				int amount = this._techRequirements.ContainsKey(cost.resource) ? this._techRequirements[cost.resource] : 0;
				list.Add(new CostData(cost.resource, amount));
			}
		}
		return list;
	}

	// Token: 0x040002FF RID: 767
	protected Dictionary<ResourceData, int> _techRequirements = new Dictionary<ResourceData, int>();

	// Token: 0x04000300 RID: 768
	[SerializeField]
	private ResearchTechData _activeTech;
}
