using System;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000113 RID: 275
public class Generator : ResourceComponent, ICallbackListener, IComponent<Generator, GeneratorData>
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000934 RID: 2356 RVA: 0x000270E4 File Offset: 0x000252E4
	// (remove) Token: 0x06000935 RID: 2357 RVA: 0x0002711C File Offset: 0x0002531C
	public event Generator.PowerEventHandler OnGeneratorStatusUpdated = delegate(Generator <p0>, bool <p1>)
	{
	};

	// Token: 0x06000936 RID: 2358 RVA: 0x00027151 File Offset: 0x00025351
	public GeneratorData GetData()
	{
		return this._generatorData;
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x00027159 File Offset: 0x00025359
	// (set) Token: 0x06000938 RID: 2360 RVA: 0x00027161 File Offset: 0x00025361
	public ResourceData ResourceBeingBurned
	{
		get
		{
			return this._resourceBeingBurned;
		}
		set
		{
			this._resourceBeingBurned = value;
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0002716C File Offset: 0x0002536C
	public void OnInitialize(GeneratorData data)
	{
		this._generatorData = data;
		if (data.effect != null)
		{
			this._effect = Object.Instantiate<GameObject>(data.effect.effect, base.transform.position, Quaternion.identity);
			this._effect.transform.SetParent(base.transform);
			this._effect.transform.SetSiblingIndex(data.effectIndex);
			this._effect.gameObject.SetActive(false);
			this._hasEffect = true;
			return;
		}
		this._hasEffect = false;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00027200 File Offset: 0x00025400
	public override void OnSpawn(bool fromSave)
	{
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._consumptionMultiplier, StatType.GeneratorSpeed, 1f, this);
		if (this._effect != null && base.Entity.Has_EComponent<FOW_Cloak>())
		{
			this._effect.gameObject.SetActive(false);
		}
		this.CreateContainers();
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00027258 File Offset: 0x00025458
	protected override void CreateContainers()
	{
		if (this._generatorData.useIndicator)
		{
			base.CreateIndicator(ContainerType.Input, this._generatorData.indicatorPosition.x, this._generatorData.indicatorPosition.y);
		}
		base.CreateContainer(ContainerType.Input, StorageMode.SharedStorage, this._generatorData.storage, StatType.GeneratorCapacity, ContainerFlags.Xray);
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x000272AF File Offset: 0x000254AF
	public override void OnAddResource(ContainerType type, ResourceData resource, int amount)
	{
		if (!base.IsUpdating)
		{
			this.FindResource();
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartCallback(EntityCallbackEvent callback)
	{
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x000272BF File Offset: 0x000254BF
	public void OnEndCallback(EntityCallbackEvent callback)
	{
		if (!base.Entity.Has_EFlag_IsDead)
		{
			this.FindResource();
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x000272D4 File Offset: 0x000254D4
	protected void FindResource()
	{
		StoredResource storedResource = null;
		int num = 0;
		ResourceContainer inputContainer = this._resourceModule.GetInputContainer();
		if (inputContainer == null)
		{
			Debug.Log("[GENERATOR] Missing input container, this will cause major problems!");
			if (this._effect.activeSelf)
			{
				this._effect.SetActive(false);
			}
			return;
		}
		foreach (StoredResource storedResource2 in inputContainer.GetStoredResources())
		{
			if (storedResource2.AmountStored > 0 && storedResource2.ResourceData.Power > num)
			{
				storedResource = storedResource2;
				num = storedResource2.ResourceData.Power;
			}
		}
		if (storedResource != null)
		{
			this.BurnResource(storedResource.ResourceData);
			return;
		}
		this.DisableBurn();
		if (this._effect.activeSelf)
		{
			this._effect.SetActive(false);
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x000273B0 File Offset: 0x000255B0
	public void BurnResource(ResourceData resource)
	{
		if (this._resourceBeingBurned != null)
		{
			if (this._resourceBeingBurned != resource)
			{
				if (Singleton<FactionManager>.Instance.IsPlayerFaction(base.FactionID))
				{
					Singleton<ResourceManager>.Instance.RemovePowerStorage(this._resourceBeingBurned.Power);
					Singleton<ResourceManager>.Instance.AddPowerStorage(resource.Power);
				}
				this._resourceBeingBurned = resource;
			}
		}
		else
		{
			if (Singleton<FactionManager>.Instance.IsPlayerFaction(base.FactionID))
			{
				Singleton<ResourceManager>.Instance.AddPowerStorage(resource.Power);
			}
			this._resourceBeingBurned = resource;
			this.OnGeneratorStatusUpdated(this, true);
		}
		this._resourceModule.TakeResource(ContainerType.Input, this._resourceBeingBurned, 1);
		Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, this._resourceBeingBurned.BurnTime * this._consumptionMultiplier.Value, null));
		if (!this._effect.activeSelf && this._hasEffect && !base.Entity.Has_EComponent<FOW_Cloak>())
		{
			this._effect.SetActive(true);
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x000274C4 File Offset: 0x000256C4
	private void DisableBurn()
	{
		if (this._resourceBeingBurned != null && Singleton<FactionManager>.Instance.IsPlayerFaction(base.FactionID))
		{
			Singleton<ResourceManager>.Instance.RemovePowerStorage(this._resourceBeingBurned.Power);
		}
		if (this._effect.activeSelf)
		{
			this._effect.SetActive(false);
		}
		this.OnGeneratorStatusUpdated(this, false);
		this._resourceBeingBurned = null;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00027533 File Offset: 0x00025733
	public override void OnReset()
	{
		this.DisableBurn();
	}

	// Token: 0x040005B1 RID: 1457
	private GeneratorData _generatorData;

	// Token: 0x040005B2 RID: 1458
	protected GameObject _effect;

	// Token: 0x040005B3 RID: 1459
	protected bool _hasEffect;

	// Token: 0x040005B4 RID: 1460
	protected StatFloat _consumptionMultiplier;

	// Token: 0x040005B5 RID: 1461
	private ResourceData _resourceBeingBurned;

	// Token: 0x02000114 RID: 276
	// (Invoke) Token: 0x06000945 RID: 2373
	public delegate void PowerEventHandler(Generator generator, bool toggle);
}
