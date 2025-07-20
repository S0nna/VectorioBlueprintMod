using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.PhasmaUI;
using XNode;

// Token: 0x02000071 RID: 113
[DefaultExecutionOrder(0)]
public class Research : Singleton<Research>
{
	// Token: 0x06000516 RID: 1302 RVA: 0x0001AEB9 File Offset: 0x000190B9
	public ResearchTreeData GetEntityTree(EntityData entity)
	{
		if (this._treeDictionary.ContainsKey(entity))
		{
			return this._treeDictionary[entity];
		}
		return null;
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001AED7 File Offset: 0x000190D7
	// (set) Token: 0x06000518 RID: 1304 RVA: 0x0001AEDF File Offset: 0x000190DF
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

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001AEE8 File Offset: 0x000190E8
	public List<ResearchTreeData> UnlockedTrees
	{
		get
		{
			return this._unlockedTrees;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001AEF0 File Offset: 0x000190F0
	public List<ResearchTechData> UnlockedTechs
	{
		get
		{
			return this._unlockedTechs;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001AEF8 File Offset: 0x000190F8
	public bool IsResearching
	{
		get
		{
			return this._activeTech != null;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001AF06 File Offset: 0x00019106
	public ResearchTechData ActiveTech
	{
		get
		{
			return this._activeTech;
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0001AF10 File Offset: 0x00019110
	public void Setup(SaveData saveData)
	{
		if (this._isSetup)
		{
			return;
		}
		UI_Singleton<UI_ResearchWindow>.Instance.Setup();
		if (!Singleton<Gamemode>.Instance.UseResearch)
		{
			foreach (EntityData arg in Library.RequestAllDataOfType<EntityData>())
			{
				Singleton<Events>.Instance.onAddEntityToInventory.Invoke(arg);
			}
			Singleton<Events>.Instance.onSortAllCategories.Invoke();
			if (DevTools.USE_RESEARCH_TESTING)
			{
				ResearchTest researchTest = base.gameObject.AddComponent<ResearchTest>();
				if (DevTools.START_RESEARCH_TECH != null)
				{
					researchTest.SetActiveTech(DevTools.START_RESEARCH_TECH, DevTools.START_RESEARCH_TECH.costs);
				}
			}
			return;
		}
		bool flag = false;
		foreach (ResearchTreeData researchTreeData in Singleton<Gamemode>.Instance.GamemodeData.researchTrees)
		{
			UI_Singleton<UI_ResearchWindow>.Instance.CreateTreeButton(researchTreeData);
			foreach (Node node in researchTreeData.nodes)
			{
				ResearchTechData researchTechData = (ResearchTechData)node;
				if (researchTechData != null && researchTechData.rewardType == ResearchTechData.RewardType.Entity)
				{
					EntityData reward = researchTechData.GetReward<EntityData>();
					Singleton<Events>.Instance.onAddEntityToInventory.Invoke(reward);
				}
			}
			if (!flag)
			{
				UI_Singleton<UI_ResearchWindow>.Instance.SetTree(researchTreeData);
				flag = true;
			}
		}
		if (DevTools.UNLOCK_ALL_TECHS)
		{
			using (List<ResearchTreeData>.Enumerator enumerator2 = Singleton<Gamemode>.Instance.GamemodeData.researchTrees.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ResearchTreeData researchTreeData2 = enumerator2.Current;
					foreach (Node node2 in researchTreeData2.nodes)
					{
						ResearchTechData researchTechData2 = (ResearchTechData)node2;
						if (researchTechData2 != null)
						{
							this.UnlockTech(researchTechData2, false);
						}
					}
				}
				goto IL_23C;
			}
		}
		foreach (string id in saveData.completedResearchTechs)
		{
			ResearchTechData researchTechData3 = Library.RequestData<ResearchTechData>(id);
			if (researchTechData3 != null)
			{
				this.UnlockTech(researchTechData3, false);
			}
		}
		IL_23C:
		foreach (string id2 in saveData.completedResearchTechs)
		{
			ResearchTechData researchTechData4 = Library.RequestData<ResearchTechData>(id2);
			if (researchTechData4 != null)
			{
				this.UnlockTech(researchTechData4, false);
			}
		}
		UI_Singleton<UI_ResearchWindow>.Instance.UpdateTechButtons();
		if (saveData.ActiveResearchTech != "")
		{
			ResearchTechData researchTechData5 = Library.RequestData<ResearchTechData>(saveData.ActiveResearchTech);
			if (researchTechData5 != null)
			{
				List<Cost> list = new List<Cost>();
				foreach (CostData costData in saveData.researchTechResources)
				{
					list.Add(new Cost(costData));
				}
				UI_Singleton<UI_ResearchWindow>.Instance.SetActiveTech(researchTechData5, list);
			}
		}
		this._isSetup = true;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001B294 File Offset: 0x00019494
	public void EnsureRegion(RegionData region)
	{
		foreach (ResearchTechData tech in region.defaultTechs)
		{
			if (!this.IsTechUnlocked(tech))
			{
				this.UnlockTech(tech, false);
			}
		}
		if (region.useFreeEntities)
		{
			foreach (EntityData data in region.freeEntities)
			{
				Singleton<ResourceManager>.Instance.AddFreeEntity(data);
			}
		}
		foreach (RegionData regionData in region.ensureRegionTechs)
		{
			foreach (ResearchTechData tech2 in regionData.defaultTechs)
			{
				if (!this.IsTechUnlocked(tech2))
				{
					this.UnlockTech(tech2, false);
				}
			}
		}
		Singleton<Events>.Instance.onSortAllCategories.Invoke();
		UI_Singleton<UI_ResearchWindow>.Instance.UpdateTechButtons();
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001B3E4 File Offset: 0x000195E4
	public void SetActiveTech(ResearchTechData tech, List<Cost> costs, bool playSound = true)
	{
		this._activeTech = tech;
		this._techRequirements.Clear();
		foreach (Cost cost in costs)
		{
			this._techRequirements.Add(cost.resource, cost.amount);
		}
		if (playSound && this.techActivatedSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.techActivatedSound);
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001B478 File Offset: 0x00019678
	public bool HasTechResource(ResourceData resource)
	{
		return this._techRequirements.ContainsKey(resource);
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001B488 File Offset: 0x00019688
	public void AddTechResource(ResourceData resource, int amount)
	{
		if (this._techRequirements.ContainsKey(resource))
		{
			Singleton<Events>.Instance.onResearchResourceTechAdded.Invoke(resource.ID, amount);
			int num = this._techRequirements[resource];
			int num2 = 0;
			if (this._techRequirements.ContainsKey(resource))
			{
				Dictionary<ResourceData, int> techRequirements = this._techRequirements;
				techRequirements[resource] -= amount;
				if (this._techRequirements[resource] <= 0)
				{
					this._techRequirements.Remove(resource);
					num2 = 0;
				}
				else
				{
					num2 = this._techRequirements[resource];
				}
			}
			int changed = num - num2;
			UI_Singleton<UI_ResearchWindow>.Instance.UpdateActiveTech(resource, num2, changed);
			if (this._techRequirements.Count == 0)
			{
				this.UnlockTech(this._activeTech, true);
				UI_Singleton<UI_ResearchWindow>.Instance.UpdateTechButtons();
				UI_Singleton<UI_ResearchWindow>.Instance.ToggleActiveTech(false);
			}
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001B560 File Offset: 0x00019760
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

	// Token: 0x06000523 RID: 1315 RVA: 0x0001B600 File Offset: 0x00019800
	public void UnlockTech(ResearchTechData tech, bool broadcast)
	{
		if (this.IsTechUnlocked(tech))
		{
			return;
		}
		this._unlockedTechs.Add(tech);
		if (tech == this._activeTech)
		{
			this._activeTech = null;
		}
		switch (tech.rewardType)
		{
		case ResearchTechData.RewardType.Entity:
			this.UnlockEntity(tech.GetReward<EntityData>());
			break;
		case ResearchTechData.RewardType.Resource:
			this.UnlockResource(tech.GetReward<ResourceData>());
			break;
		case ResearchTechData.RewardType.Recipe:
		{
			RecipeData reward = tech.GetReward<RecipeData>();
			this.UnlockRecipe(reward);
			Singleton<Events>.Instance.onRecipeUnlocked.Invoke(reward);
			break;
		}
		case ResearchTechData.RewardType.Tree:
			this.UnlockTree(tech.GetReward<ResearchTreeData>());
			break;
		}
		if (broadcast)
		{
			Singleton<Events>.Instance.onResearchTechFinished.Invoke(tech);
		}
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0001B6B4 File Offset: 0x000198B4
	public bool IsTreeUnlocked(ResearchTreeData tree)
	{
		return !Singleton<Gamemode>.Instance.UseResearch || this._unlockedTrees.Contains(tree);
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001B6D0 File Offset: 0x000198D0
	public bool IsTechUnlocked(ResearchTechData tech)
	{
		return !Singleton<Gamemode>.Instance.UseResearch || this._unlockedTechs.Contains(tech);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001B6EC File Offset: 0x000198EC
	public bool IsEntityUnlocked(EntityData entity)
	{
		return !Singleton<Gamemode>.Instance.UseResearch || this._unlockedEntities.Contains(entity);
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001B708 File Offset: 0x00019908
	public bool IsRecipeUnlocked(RecipeData recipe)
	{
		return !Singleton<Gamemode>.Instance.UseResearch || this._unlockedRecipes.Contains(recipe);
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001B724 File Offset: 0x00019924
	public bool IsResourceUnlocked(ResourceData resource)
	{
		return !Singleton<Gamemode>.Instance.UseResearch || this._unlockedResources.Contains(resource);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001B740 File Offset: 0x00019940
	public void UnlockEntity(EntityData data)
	{
		if (!this._unlockedEntities.Contains(data))
		{
			this._unlockedEntities.Add(data);
			Singleton<Events>.Instance.onEntityUnlocked.Invoke(data);
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001B76C File Offset: 0x0001996C
	public void UnlockRecipe(RecipeData recipe)
	{
		if (!this._unlockedRecipes.Contains(recipe))
		{
			this._unlockedRecipes.Add(recipe);
			this.UnlockResource(recipe.outputResource);
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0001B794 File Offset: 0x00019994
	public void UnlockResource(ResourceData resource)
	{
		if (!this._unlockedResources.Contains(resource))
		{
			this._unlockedResources.Add(resource);
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0001B7B0 File Offset: 0x000199B0
	public void UnlockTree(ResearchTreeData tree)
	{
		if (!this._unlockedTrees.Contains(tree))
		{
			this._unlockedTrees.Add(tree);
			UI_Singleton<UI_ResearchWindow>.Instance.UpdateTreeButtons();
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0001B7D6 File Offset: 0x000199D6
	public bool IsDecryptorUnlocked(ResearchTechData tech)
	{
		return this.IsTechUnlocked(tech.GetInputNode());
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0001B7E4 File Offset: 0x000199E4
	public TechStatus GetTechStatus(ResearchTechData tech)
	{
		if (tech == this._activeTech)
		{
			return TechStatus.Active;
		}
		ResearchTechData inputNode = tech.GetInputNode();
		if (inputNode != null && !this.IsTechUnlocked(inputNode))
		{
			return TechStatus.Locked;
		}
		if (!tech.encrypted || !this._decryptors.ContainsKey(tech))
		{
			return TechStatus.Available;
		}
		return TechStatus.Encrypted;
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0001B838 File Offset: 0x00019A38
	public void AddDecryptor(Decryptor decryptor)
	{
		if (decryptor == null || decryptor.Tech == null)
		{
			Debug.Log("[RESEARCH] Could not add decryptor as it or its tech is null!");
			return;
		}
		if (!this._decryptors.ContainsKey(decryptor.Tech))
		{
			this._decryptors.Add(decryptor.Tech, decryptor);
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001B88C File Offset: 0x00019A8C
	public void RemoveDecryptor(Decryptor decryptor)
	{
		if (decryptor == null || decryptor.Tech == null)
		{
			Debug.Log("[RESEARCH] Could not remove decryptor as it or its tech is null!");
			return;
		}
		if (this._decryptors.ContainsKey(decryptor.Tech))
		{
			this._decryptors.Remove(decryptor.Tech);
			UI_Singleton<UI_ResearchWindow>.Instance.UpdateTechButtons();
		}
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0001B8EA File Offset: 0x00019AEA
	public Decryptor FindDecryptor(ResearchTechData tech)
	{
		Debug.Log("[RESEARCH] Looking for decryptor with ID " + tech.ID);
		if (this._decryptors.ContainsKey(tech))
		{
			return this._decryptors[tech];
		}
		return null;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0001B91D File Offset: 0x00019B1D
	public void FinishTech()
	{
		if (this._activeTech != null)
		{
			this.UnlockTech(this._activeTech, true);
			UI_Singleton<UI_ResearchWindow>.Instance.UpdateTechButtons();
		}
	}

	// Token: 0x040002F4 RID: 756
	public AudioClip techActivatedSound;

	// Token: 0x040002F5 RID: 757
	protected Dictionary<EntityData, ResearchTreeData> _treeDictionary = new Dictionary<EntityData, ResearchTreeData>();

	// Token: 0x040002F6 RID: 758
	protected Dictionary<ResourceData, int> _techRequirements = new Dictionary<ResourceData, int>();

	// Token: 0x040002F7 RID: 759
	protected List<ResearchTreeData> _unlockedTrees = new List<ResearchTreeData>();

	// Token: 0x040002F8 RID: 760
	protected List<ResearchTechData> _unlockedTechs = new List<ResearchTechData>();

	// Token: 0x040002F9 RID: 761
	protected List<EntityData> _unlockedEntities = new List<EntityData>();

	// Token: 0x040002FA RID: 762
	protected List<ResourceData> _unlockedResources = new List<ResourceData>();

	// Token: 0x040002FB RID: 763
	protected List<RecipeData> _unlockedRecipes = new List<RecipeData>();

	// Token: 0x040002FC RID: 764
	protected Dictionary<ResearchTechData, Decryptor> _decryptors = new Dictionary<ResearchTechData, Decryptor>();

	// Token: 0x040002FD RID: 765
	private ResearchTechData _activeTech;

	// Token: 0x040002FE RID: 766
	private bool _isSetup;
}
