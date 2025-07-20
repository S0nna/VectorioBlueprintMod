using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x02000228 RID: 552
[Serializable]
public class ResourceContainer
{
	// Token: 0x0600101E RID: 4126 RVA: 0x0004BFA5 File Offset: 0x0004A1A5
	public ResourceContainer(ContainerCreationData container, EntityComponent component = null)
	{
		this.Setup(container, component);
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0004BFC0 File Offset: 0x0004A1C0
	public void Setup(ContainerCreationData container, EntityComponent component = null)
	{
		this._storageMode = container.StorageMode;
		Singleton<StatManager>.Instance.CreateStatInt(ref this._storage, container.StatCode, container.Storage, component);
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06001020 RID: 4128 RVA: 0x0004BFEE File Offset: 0x0004A1EE
	public ref StatInt Storage
	{
		get
		{
			return ref this._storage;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06001021 RID: 4129 RVA: 0x0004BFF6 File Offset: 0x0004A1F6
	public StorageMode StorageMode
	{
		get
		{
			return this._storageMode;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06001022 RID: 4130 RVA: 0x0004BFFE File Offset: 0x0004A1FE
	public int AmountStored
	{
		get
		{
			return this._amountStored;
		}
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0004C006 File Offset: 0x0004A206
	public bool HasFilter()
	{
		return this._hasFilter;
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0004C00E File Offset: 0x0004A20E
	public ResourceData GetFilter()
	{
		return this._filter;
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0004C016 File Offset: 0x0004A216
	public void ApplyFilter(ResourceData filter)
	{
		this._filter = filter;
		this._hasFilter = true;
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x0004C026 File Offset: 0x0004A226
	private bool CheckFilter(ResourceData resource)
	{
		return resource == null || !this._hasFilter || resource == this._filter;
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0004C04C File Offset: 0x0004A24C
	public bool HasPendingAction(ResourceData resource)
	{
		StorageMode storageMode = this._storageMode;
		if (storageMode != StorageMode.SharedStorage)
		{
			return storageMode == StorageMode.LocalizedStorage && this.HasResource(resource) && this._storedResources[resource].HasPendingAction;
		}
		return this._pendingAction != null;
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0004C094 File Offset: 0x0004A294
	public void SetPendingAction(ResourceAction action)
	{
		StorageMode storageMode = this._storageMode;
		if (storageMode == StorageMode.SharedStorage)
		{
			this._pendingAction = action;
			return;
		}
		if (storageMode != StorageMode.LocalizedStorage)
		{
			return;
		}
		this.EnsureResource(action.Resource).PendingAction = action;
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0004C0CC File Offset: 0x0004A2CC
	public void ClearPendingAction(ResourceAction action)
	{
		StorageMode storageMode = this._storageMode;
		if (storageMode == StorageMode.SharedStorage)
		{
			this._pendingAction = null;
			return;
		}
		if (storageMode != StorageMode.LocalizedStorage)
		{
			return;
		}
		StoredResource storedResource;
		if (this.TryGetStoredResource(action.Resource, out storedResource))
		{
			storedResource.PendingAction = null;
			return;
		}
		string str = "[CONTAINER] Couldn't remove action as the resource ";
		ResourceData resource = action.Resource;
		Debug.Log(str + (((resource != null) ? resource.ID : null) ?? "UNKNOWN") + " does not exist in the container!");
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0004C137 File Offset: 0x0004A337
	public bool HasResource(ResourceData resource)
	{
		return this._storedResources.ContainsKey(resource);
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0004C145 File Offset: 0x0004A345
	public bool HasResourceAvailable(ResourceData resource)
	{
		return this.HasResource(resource) && this._storedResources[resource].AmountStored > 0;
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0004C166 File Offset: 0x0004A366
	public bool TryGetStoredResource(ResourceData resource, out StoredResource storedResource)
	{
		if (this.HasResource(resource))
		{
			storedResource = this._storedResources[resource];
			return true;
		}
		storedResource = null;
		return false;
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x0004C188 File Offset: 0x0004A388
	public int GetAmount(ResourceData resource)
	{
		StoredResource storedResource;
		if (this.TryGetStoredResource(resource, out storedResource))
		{
			return storedResource.AmountStored;
		}
		return 0;
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x0004C1A8 File Offset: 0x0004A3A8
	public List<StoredResource> GetStoredResources()
	{
		List<StoredResource> list = new List<StoredResource>();
		foreach (StoredResource item in this._storedResources.Values)
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0004C208 File Offset: 0x0004A408
	public List<ResourceData> GetMostStored()
	{
		List<StoredResource> list = new List<StoredResource>();
		foreach (StoredResource storedResource in this._storedResources.Values)
		{
			if (storedResource.AmountStored > 0)
			{
				list.Add(storedResource);
			}
		}
		list.Sort((StoredResource a, StoredResource b) => b.AmountStored.CompareTo(a.AmountStored));
		List<ResourceData> list2 = new List<ResourceData>();
		int num = Math.Min(4, list.Count);
		for (int i = 0; i < num; i++)
		{
			list2.Add(list[i].ResourceData);
		}
		return list2;
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0004C2D0 File Offset: 0x0004A4D0
	public StoredResource GetBiggestResource(ResourceData filter = null)
	{
		int num = 0;
		bool flag = filter != null;
		StoredResource result = null;
		foreach (StoredResource storedResource in this.GetStoredResources())
		{
			if ((!flag || !(storedResource.ResourceData != filter)) && storedResource.AmountStored > num)
			{
				num = storedResource.AmountStored;
				result = storedResource;
			}
		}
		return result;
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0004C354 File Offset: 0x0004A554
	public int GetSpace(ResourceData resource = null)
	{
		if (resource != null && !this.CheckFilter(resource))
		{
			return 0;
		}
		StorageMode storageMode = this._storageMode;
		if (storageMode == StorageMode.SharedStorage)
		{
			return this._storage.Value - this._amountStored;
		}
		if (storageMode != StorageMode.LocalizedStorage)
		{
			return 0;
		}
		if (!this.HasResource(resource))
		{
			return this._storage.Value;
		}
		return this._storage.Value - this._storedResources[resource].AmountStored;
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0004C3CC File Offset: 0x0004A5CC
	public bool IsFull(ResourceData resource = null)
	{
		StorageMode storageMode = this._storageMode;
		if (storageMode != StorageMode.SharedStorage)
		{
			return storageMode == StorageMode.LocalizedStorage && resource != null && this.HasResource(resource) && this._storedResources[resource].AmountStored >= this._storage.Value;
		}
		return this._amountStored >= this._storage.Value;
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0004C43C File Offset: 0x0004A63C
	public bool IsEmpty(ResourceData resource = null)
	{
		StorageMode storageMode = this._storageMode;
		if (storageMode != StorageMode.SharedStorage)
		{
			return storageMode == StorageMode.LocalizedStorage && resource != null && (!this.HasResource(resource) || this._storedResources[resource].AmountStored == 0);
		}
		return this._amountStored == 0;
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0004C490 File Offset: 0x0004A690
	public StoredResource EnsureResource(ResourceData resource)
	{
		StoredResource storedResource;
		if (!this.HasResource(resource))
		{
			storedResource = new StoredResource(resource);
			this._storedResources.Add(resource, storedResource);
		}
		else
		{
			storedResource = this._storedResources[resource];
		}
		return storedResource;
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0004C4CC File Offset: 0x0004A6CC
	public int AddAmount(ResourceData resource, int amountToAdd)
	{
		if (!this.CheckFilter(resource))
		{
			return amountToAdd;
		}
		StoredResource storedResource = this.EnsureResource(resource);
		int num = 0;
		StorageMode storageMode = this._storageMode;
		int num2;
		if (storageMode == StorageMode.SharedStorage)
		{
			num2 = this._amountStored + amountToAdd;
			if (num2 > this._storage.Value)
			{
				num = num2 - this._storage.Value;
				num2 = this._storage.Value;
			}
			storedResource.AmountStored = storedResource.AmountStored + amountToAdd - num;
			this._amountStored = num2;
			return num;
		}
		if (storageMode != StorageMode.LocalizedStorage)
		{
			return 0;
		}
		num2 = storedResource.AmountStored + amountToAdd;
		if (num2 > this._storage.Value)
		{
			num = num2 - this._storage.Value;
			num2 = this._storage.Value;
		}
		this._amountStored += num2 - storedResource.AmountStored;
		storedResource.AmountStored = num2;
		return num;
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0004C59C File Offset: 0x0004A79C
	public int RemoveAmount(ResourceData resource, int amountToRemove)
	{
		StoredResource storedResource;
		if (!this.TryGetStoredResource(resource, out storedResource))
		{
			return 0;
		}
		StorageMode storageMode = this._storageMode;
		int num;
		if (storageMode == StorageMode.SharedStorage)
		{
			num = Math.Min(storedResource.AmountStored, amountToRemove);
			storedResource.AmountStored -= num;
			this._amountStored -= num;
			return num;
		}
		if (storageMode != StorageMode.LocalizedStorage)
		{
			return 0;
		}
		num = Math.Min(storedResource.AmountStored, amountToRemove);
		storedResource.AmountStored -= num;
		this._amountStored -= num;
		return num;
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0004C61D File Offset: 0x0004A81D
	public void Clear()
	{
		this._storedResources.Clear();
		this._amountStored = 0;
		this._pendingAction = null;
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0004C61D File Offset: 0x0004A81D
	public void Reset()
	{
		this._storedResources.Clear();
		this._amountStored = 0;
		this._pendingAction = null;
	}

	// Token: 0x04000E3B RID: 3643
	[SerializeField]
	private Dictionary<ResourceData, StoredResource> _storedResources = new Dictionary<ResourceData, StoredResource>();

	// Token: 0x04000E3C RID: 3644
	private StatInt _storage;

	// Token: 0x04000E3D RID: 3645
	private StorageMode _storageMode;

	// Token: 0x04000E3E RID: 3646
	private int _amountStored;

	// Token: 0x04000E3F RID: 3647
	private ResourceData _filter;

	// Token: 0x04000E40 RID: 3648
	private bool _hasFilter;

	// Token: 0x04000E41 RID: 3649
	private ResourceAction _pendingAction;
}
