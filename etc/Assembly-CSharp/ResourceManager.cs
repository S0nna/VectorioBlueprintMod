using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vectorio.Formatting;
using Vectorio.PhasmaUI;

// Token: 0x020001E8 RID: 488
[DefaultExecutionOrder(0)]
public class ResourceManager : Singleton<ResourceManager>
{
	// Token: 0x06000F06 RID: 3846 RVA: 0x00045E76 File Offset: 0x00044076
	public ResourceData HeatData()
	{
		return this.heatData;
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x00045E7E File Offset: 0x0004407E
	public ResourceData PowerData()
	{
		return this.powerData;
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x00045E86 File Offset: 0x00044086
	public bool IsResourceIgnored(ResourceData resource)
	{
		return this._ignoredResources.Contains(resource);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x00045E94 File Offset: 0x00044094
	public void ResetFreeEntities()
	{
		this._freeEntities.Clear();
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x00045EA1 File Offset: 0x000440A1
	public void AddFreeEntity(EntityData data)
	{
		if (!this._freeEntities.ContainsKey(data))
		{
			this._freeEntities.Add(data, 0);
		}
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x00045EBE File Offset: 0x000440BE
	public bool CheckFreeEntity(EntityData entity)
	{
		return this._freeEntities.ContainsKey(entity) && this._freeEntities[entity] <= 0;
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x00045EE4 File Offset: 0x000440E4
	public void UpdateGlobalStorage(int amount)
	{
		if (!this.allowResourcesInScene)
		{
			return;
		}
		this._globalStorage = amount;
		if (this._globalStorage > 0)
		{
			this.storageText.text = Formatter.Number((float)this._globalAmount) + " / " + Formatter.Number((float)this._globalStorage);
			return;
		}
		this.storageText.text = "NO AVAILABLE STORAGE";
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x00045F48 File Offset: 0x00044148
	public void UpdateGlobalAmount(int amount)
	{
		if (!this.allowResourcesInScene)
		{
			return;
		}
		this._globalAmount = amount;
		this.storageText.text = Formatter.Number((float)this._globalAmount) + " / " + Formatter.Number((float)this._globalStorage);
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000F0E RID: 3854 RVA: 0x00045F87 File Offset: 0x00044187
	public int PowerStorage
	{
		get
		{
			return this._powerStorage;
		}
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x00045F8F File Offset: 0x0004418F
	public void SortContainers()
	{
		if (SaveSystem.IS_LOADING)
		{
			return;
		}
		this._containers.Sort((ResourceModule container1, ResourceModule container2) => container1.RuntimeID.CompareTo(container2.RuntimeID));
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000F10 RID: 3856 RVA: 0x00045FC3 File Offset: 0x000441C3
	public bool PowerOutage
	{
		get
		{
			return this._powerOutage;
		}
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x00045FCB File Offset: 0x000441CB
	public int GetAmountByData(ResourceData resource)
	{
		if (!this._resources.ContainsKey(resource))
		{
			return -1;
		}
		return this._resources[resource].GetAmount(false);
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x00045FF0 File Offset: 0x000441F0
	public void Setup(RegionData regionData)
	{
		if (!this.allowResourcesInScene)
		{
			return;
		}
		if (regionData.useFreeEntities)
		{
			foreach (EntityData data in regionData.freeEntities)
			{
				this.AddFreeEntity(data);
			}
		}
		if (!this._resources.ContainsKey(this.powerData))
		{
			this._resources.Add(this.powerData, new ResourceManager.Resource(this.powerData, 0));
			this._powerStorage = 0;
			this.resourceList.AddPowerResource(this.powerParent);
			this.powerParent.Setup(this.heatData, 0, 0);
			this._ignoredResources.Add(this.powerData);
		}
		if (!this._resources.ContainsKey(this.heatData))
		{
			if (DevTools.ENEMY_TEST)
			{
				this._resources.Add(this.heatData, new ResourceManager.Resource(this.heatData, DevTools.HEAT_LEVEL));
				this._heatStorage = DevTools.HEAT_LEVEL + 1000;
				this.resourceList.AddHeatResource(this.heatParent);
				this.heatParent.Setup(this.powerData, DevTools.HEAT_LEVEL, this._heatStorage);
			}
			else
			{
				this._resources.Add(this.heatData, new ResourceManager.Resource(this.heatData, 0));
				this._heatStorage = 100;
				this.resourceList.AddHeatResource(this.heatParent);
				this.heatParent.Setup(this.powerData, 0, this._heatStorage);
			}
			this._ignoredResources.Add(this.heatData);
		}
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x000461A0 File Offset: 0x000443A0
	public void Add(ResourceData resource, int amount)
	{
		if (!this.allowResourcesInScene)
		{
			return;
		}
		if (amount == 0)
		{
			return;
		}
		if (resource == this.heatData)
		{
			int num = this._resources[resource].Amount += amount;
			this.resourceList.UpdateHeat(num);
			Singleton<Events>.Instance.onHeatAmountUpdated.Invoke(num);
		}
		else if (resource == this.powerData)
		{
			int num = this._resources[this.powerData].Amount += amount;
			this.resourceList.UpdatePower(num);
			if (!SaveSystem.IS_LOADING && !this.PowerOutage && this._resources[this.powerData].GetAmount(false) > this.PowerStorage)
			{
				this._powerOutage = true;
				Singleton<Events>.Instance.onPowerExceeded.Invoke();
			}
		}
		else if (this._resources.ContainsKey(resource))
		{
			int num = this._resources[resource].Amount += amount;
			this._globalAmount += amount;
			this.UpdateGlobalAmount(this._globalAmount);
			this.resourceList.UpdateResourceAmount(resource, num);
		}
		else
		{
			this._resources.Add(resource, new ResourceManager.Resource(resource, amount));
			this.resourceList.AddResource(resource, amount);
			this._globalAmount += amount;
			this.UpdateGlobalAmount(this._globalAmount);
		}
		if (Singleton<Gamemode>.Instance.UseResearch && Singleton<Research>.Instance != null)
		{
			Singleton<Events>.Instance.onResourceUpdated.Invoke(resource, this._resources[resource].GetAmount(false));
		}
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x00046360 File Offset: 0x00044560
	public void Remove(ResourceData resource, int amount)
	{
		if (!this.allowResourcesInScene)
		{
			return;
		}
		if (amount == 0)
		{
			return;
		}
		if (Singleton<Gamemode>.Instance.UseResearch && Singleton<Research>.Instance != null)
		{
			Singleton<Events>.Instance.onResourceUpdated.Invoke(resource, -amount);
		}
		if (resource == this.heatData)
		{
			int num = this._resources[this.heatData].Amount -= amount;
			this.resourceList.UpdateHeat(num);
			Singleton<Events>.Instance.onHeatAmountUpdated.Invoke(num);
			return;
		}
		if (resource == this.powerData)
		{
			int num = this._resources[this.powerData].Amount -= amount;
			this.resourceList.UpdatePower(num);
			if (this.PowerOutage && this._resources[this.powerData].GetAmount(false) <= this.PowerStorage)
			{
				this._powerOutage = false;
				Singleton<Events>.Instance.onPowerRecovered.Invoke();
				return;
			}
		}
		else if (this._resources.ContainsKey(resource))
		{
			this._resources[resource].Amount -= amount;
			this._globalAmount -= amount;
			this.UpdateGlobalAmount(this._globalAmount);
			this.resourceList.UpdateResourceAmount(resource, this._resources[resource].Amount);
		}
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x000464D0 File Offset: 0x000446D0
	public unsafe void RegisterGlobalContainer(ResourceModule container)
	{
		if (!this.allowResourcesInScene)
		{
			return;
		}
		foreach (StoredResource storedResource in container.GlobalContainer.GetStoredResources())
		{
			this.Add(storedResource.ResourceData, storedResource.AmountStored);
		}
		this._globalStorage += container.GlobalContainer.Storage->Value;
		this.UpdateGlobalStorage(this._globalStorage);
		this._containers.Add(container);
		this.SortContainers();
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x00046578 File Offset: 0x00044778
	public unsafe void UnregisterGlobalContainer(ResourceModule container)
	{
		if (!this.allowResourcesInScene)
		{
			return;
		}
		foreach (StoredResource storedResource in container.GlobalContainer.GetStoredResources())
		{
			this.Remove(storedResource.ResourceData, storedResource.AmountStored);
		}
		this._globalStorage -= container.GlobalContainer.Storage->Value;
		this.UpdateGlobalStorage(this._globalStorage);
		this._containers.Remove(container);
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0004661C File Offset: 0x0004481C
	public void AddAmountToStorages(ResourceData resource, int amount)
	{
		int num = amount;
		foreach (ResourceModule resourceModule in this._containers)
		{
			num = resourceModule.AddResource(resourceModule.GlobalContainerType, resource, num);
			if (num <= 0)
			{
				break;
			}
		}
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x00046680 File Offset: 0x00044880
	public void RemoveAmountFromStorages(ResourceData resource, int amount)
	{
		int num = amount;
		foreach (ResourceModule resourceModule in this._containers)
		{
			num -= resourceModule.TakeResource(resourceModule.GlobalContainerType, resource, num);
			if (num <= 0)
			{
				break;
			}
		}
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x000466E8 File Offset: 0x000448E8
	public void AddPowerStorage(int amount)
	{
		if (!Singleton<Gamemode>.Instance.UseResources)
		{
			return;
		}
		int num = this._powerStorage += amount;
		this.powerParent.UpdateStorage(num);
		if (!SaveSystem.IS_LOADING && this.PowerOutage && this._resources[this.powerData].GetAmount(false) <= num)
		{
			this._powerOutage = false;
			Singleton<Events>.Instance.onPowerRecovered.Invoke();
		}
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x00046760 File Offset: 0x00044960
	public void RemovePowerStorage(int amount)
	{
		if (!Singleton<Gamemode>.Instance.UseResources)
		{
			return;
		}
		int num = this._powerStorage -= amount;
		this.powerParent.UpdateStorage(num);
		if (!SaveSystem.IS_LOADING && !this.PowerOutage && this._resources[this.powerData].GetAmount(false) > num)
		{
			this._powerOutage = true;
			Singleton<Events>.Instance.onPowerExceeded.Invoke();
		}
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x000467D8 File Offset: 0x000449D8
	public void ForceUpdatePowerStatus()
	{
		if (!Singleton<Gamemode>.Instance.UseResources)
		{
			return;
		}
		if (this._resources[this.powerData].GetAmount(false) > this.PowerStorage)
		{
			this._powerOutage = true;
			Singleton<Events>.Instance.onPowerExceeded.Invoke();
			return;
		}
		this._powerOutage = false;
		Singleton<Events>.Instance.onPowerRecovered.Invoke();
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0004683E File Offset: 0x00044A3E
	public bool CheckEntityCosts(EntityData data, bool pending)
	{
		return !Singleton<Gamemode>.Instance.UseResources || !data.UseNormalCost || this.CheckCost(data, data.NormalCost, pending);
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x00046866 File Offset: 0x00044A66
	public bool CheckCost(EntityData data, Cost cost, bool pending)
	{
		return DevTools.SKIP_RESOURCE_CHECKS || this.CheckFreeEntity(data) || this.CheckAmount(cost.resource, cost.amount, pending);
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x00046890 File Offset: 0x00044A90
	public void ApplyCosts(Entity entity)
	{
		if (!Singleton<Gamemode>.Instance.UseResources || !entity.IsPlayerFaction)
		{
			return;
		}
		EntityData data = entity.GetData();
		if (entity.Has_EFlag_IsCostsApplied)
		{
			return;
		}
		if (!DevTools.SKIP_RESOURCE_CHECKS && data.UseNormalCost)
		{
			if (this._freeEntities.ContainsKey(data))
			{
				if (this._freeEntities[data] != 0 && !SaveSystem.IS_LOADING)
				{
					this.RemoveAmountFromStorages(data.NormalCost.resource, data.NormalCost.amount);
				}
				Dictionary<EntityData, int> freeEntities = this._freeEntities;
				EntityData key = data;
				freeEntities[key]++;
			}
			else if (!SaveSystem.IS_LOADING)
			{
				this.RemoveAmountFromStorages(data.NormalCost.resource, data.NormalCost.amount);
			}
		}
		if (data.UseSpecialCost)
		{
			this.Add(data.SpecialCost.resource, data.SpecialCost.amount);
		}
		entity.Set_EFlag_IsCostsApplied(true);
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0004697C File Offset: 0x00044B7C
	public void ApplyPendingCosts(Entity entity)
	{
		if (!Singleton<Gamemode>.Instance.UseResources)
		{
			return;
		}
		EntityData data = entity.GetData();
		if (entity.Has_EFlag_IsCostsApplied)
		{
			return;
		}
		if (!DevTools.SKIP_RESOURCE_CHECKS && data.UseNormalCost)
		{
			if (this._freeEntities.ContainsKey(data))
			{
				if (this._freeEntities[data] > 0)
				{
					if (!this._resources.ContainsKey(data.NormalCost.resource))
					{
						this._resources.Add(data.NormalCost.resource, new ResourceManager.Resource(data.NormalCost.resource, 0));
					}
					this._resources[data.NormalCost.resource].Pending -= data.NormalCost.amount;
				}
				else
				{
					entity.Set_EFlag_IsFreeEntity(true);
				}
				Dictionary<EntityData, int> freeEntities = this._freeEntities;
				EntityData key = data;
				freeEntities[key]++;
			}
			else
			{
				if (!this._resources.ContainsKey(data.NormalCost.resource))
				{
					this._resources.Add(data.NormalCost.resource, new ResourceManager.Resource(data.NormalCost.resource, 0));
				}
				this._resources[data.NormalCost.resource].Pending -= data.NormalCost.amount;
			}
		}
		if (data.UseSpecialCost)
		{
			if (!this._resources.ContainsKey(data.SpecialCost.resource))
			{
				this._resources.Add(data.SpecialCost.resource, new ResourceManager.Resource(data.NormalCost.resource, 0));
			}
			this._resources[data.SpecialCost.resource].Pending += data.SpecialCost.amount;
		}
		entity.Set_EFlag_IsCostsApplied(true);
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x00046B54 File Offset: 0x00044D54
	public void RevertCosts(Entity entity)
	{
		if (!Singleton<Gamemode>.Instance.UseResources)
		{
			return;
		}
		EntityData data = entity.GetData();
		if (entity.Has_EFlag_IsBlueprint || !entity.Has_EFlag_IsCostsApplied)
		{
			return;
		}
		if (!DevTools.SKIP_RESOURCE_CHECKS && data.UseNormalCost && this._freeEntities.ContainsKey(data))
		{
			Dictionary<EntityData, int> freeEntities = this._freeEntities;
			EntityData key = data;
			freeEntities[key]--;
		}
		if (data.UseSpecialCost)
		{
			this.Remove(data.SpecialCost.resource, data.SpecialCost.amount);
		}
		entity.Set_EFlag_IsCostsApplied(false);
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x00046BE8 File Offset: 0x00044DE8
	public void RevertPendingCosts(Entity entity)
	{
		if (!Singleton<Gamemode>.Instance.UseResources)
		{
			return;
		}
		EntityData data = entity.GetData();
		if (!entity.Has_EFlag_IsCostsApplied)
		{
			return;
		}
		if (!DevTools.SKIP_RESOURCE_CHECKS && data.UseNormalCost)
		{
			if (this._freeEntities.ContainsKey(data))
			{
				Dictionary<EntityData, int> freeEntities = this._freeEntities;
				EntityData key = data;
				freeEntities[key]--;
				if (!entity.Has_EFlag_IsFreeEntity)
				{
					this._resources[data.NormalCost.resource].Pending += data.NormalCost.amount;
				}
				else
				{
					entity.Set_EFlag_IsFreeEntity(false);
				}
			}
			else if (this._resources.ContainsKey(data.NormalCost.resource))
			{
				this._resources[data.NormalCost.resource].Pending += data.NormalCost.amount;
			}
		}
		if (data.UseSpecialCost && this._resources.ContainsKey(data.SpecialCost.resource))
		{
			this._resources[data.SpecialCost.resource].Pending -= data.SpecialCost.amount;
		}
		entity.Set_EFlag_IsCostsApplied(false);
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x00046D27 File Offset: 0x00044F27
	public bool CheckAmount(ResourceData resource, int amount, bool pending)
	{
		return !Singleton<Gamemode>.Instance.UseResources || DevTools.SKIP_RESOURCE_CHECKS || (this._resources.ContainsKey(resource) && this._resources[resource].GetAmount(pending) >= amount);
	}

	// Token: 0x04000C44 RID: 3140
	public bool allowResourcesInScene = true;

	// Token: 0x04000C45 RID: 3141
	public UI_ResourceList resourceList;

	// Token: 0x04000C46 RID: 3142
	public ResourceData heatData;

	// Token: 0x04000C47 RID: 3143
	public ResourceData powerData;

	// Token: 0x04000C48 RID: 3144
	private Dictionary<ResourceData, ResourceManager.Resource> _resources = new Dictionary<ResourceData, ResourceManager.Resource>();

	// Token: 0x04000C49 RID: 3145
	private Dictionary<EntityData, int> _freeEntities = new Dictionary<EntityData, int>();

	// Token: 0x04000C4A RID: 3146
	private int _globalAmount;

	// Token: 0x04000C4B RID: 3147
	private int _globalStorage;

	// Token: 0x04000C4C RID: 3148
	private int _heatStorage;

	// Token: 0x04000C4D RID: 3149
	private int _powerStorage;

	// Token: 0x04000C4E RID: 3150
	private List<ResourceData> _ignoredResources = new List<ResourceData>();

	// Token: 0x04000C4F RID: 3151
	protected List<ResourceModule> _containers = new List<ResourceModule>();

	// Token: 0x04000C50 RID: 3152
	public TextMeshProUGUI storageText;

	// Token: 0x04000C51 RID: 3153
	public HeatUI heatParent;

	// Token: 0x04000C52 RID: 3154
	public PowerUI powerParent;

	// Token: 0x04000C53 RID: 3155
	protected bool _powerOutage;

	// Token: 0x020001E9 RID: 489
	public class Resource
	{
		// Token: 0x06000F24 RID: 3876 RVA: 0x00046DA1 File Offset: 0x00044FA1
		public Resource(ResourceData data, int amount)
		{
			this._data = data;
			this._amount = amount;
			this._pending = 0;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00046DBE File Offset: 0x00044FBE
		public int GetAmount(bool pending)
		{
			if (!pending)
			{
				return this._amount;
			}
			return this._amount + this._pending;
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00046DD7 File Offset: 0x00044FD7
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x00046DDF File Offset: 0x00044FDF
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				this._amount = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x00046DE8 File Offset: 0x00044FE8
		// (set) Token: 0x06000F29 RID: 3881 RVA: 0x00046DF0 File Offset: 0x00044FF0
		public int Pending
		{
			get
			{
				return this._pending;
			}
			set
			{
				this._pending = value;
			}
		}

		// Token: 0x04000C54 RID: 3156
		private ResourceData _data;

		// Token: 0x04000C55 RID: 3157
		private int _amount;

		// Token: 0x04000C56 RID: 3158
		private int _pending;
	}
}
