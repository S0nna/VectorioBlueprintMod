using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x0200011E RID: 286
public class Lab : ResourceComponent, IComponent<Lab, LabData>, ICallbackListener
{
	// Token: 0x06000976 RID: 2422 RVA: 0x00027F8F File Offset: 0x0002618F
	public LabData GetData()
	{
		return this._labData;
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x00027F97 File Offset: 0x00026197
	public int GetTotalResearched
	{
		get
		{
			return this._totalResearched;
		}
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00027FA0 File Offset: 0x000261A0
	public void OnInitialize(LabData data)
	{
		this._labData = data;
		if (data.effectData != null)
		{
			GameObject effect = data.effectData.effect;
			this._effect = Object.Instantiate<GameObject>(effect, base.transform.position, Quaternion.identity);
			this._effect.transform.SetParent(base.transform);
			this._hasEffect = true;
		}
		else
		{
			this._hasEffect = false;
		}
		this._researchingSound = data.researchingSound;
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0002801C File Offset: 0x0002621C
	public override void OnSpawn(bool fromSave)
	{
		if (this._hasEffect)
		{
			this._effect.SetActive(false);
		}
		if (Singleton<Gamemode>.Instance.UseResearch && !this._isListening)
		{
			Singleton<Events>.Instance.onResearchTechActivated.AddListener(new UnityAction<ResearchTechData, List<Cost>>(this.OnResearchActivated));
			Singleton<Events>.Instance.onResearchTechFinished.AddListener(new UnityAction<ResearchTechData>(this.OnResearchCompleted));
			this._isListening = true;
		}
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._cooldown, StatType.LabSpeed, this._labData.cooldown, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._value, StatType.LabValue, this._labData.value, this);
		this.CreateContainers();
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x000280D4 File Offset: 0x000262D4
	protected override void CreateContainers()
	{
		if (this._labData.useIndicator)
		{
			base.CreateIndicator(ContainerType.Input, this._labData.indicatorPosition.x, this._labData.indicatorPosition.y);
		}
		base.CreateContainer(ContainerType.Input, StorageMode.LocalizedStorage, this._labData.storage, StatType.LabCapacity, ContainerFlags.Xray);
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0002812B File Offset: 0x0002632B
	private void ConvertResource(ResourceData resource)
	{
		Singleton<Research>.Instance.AddTechResource(resource, this._value.Value);
		this._resourceModule.TakeResource(ContainerType.Input, resource, 1);
		this._totalResearched++;
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00028160 File Offset: 0x00026360
	public override void OnAddResource(ContainerType type, ResourceData resource, int amount)
	{
		if (Singleton<Research>.Instance.IsResearching && !base.IsUpdating)
		{
			this.GetNextResource();
		}
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0002817C File Offset: 0x0002637C
	public void OnStartCallback(EntityCallbackEvent callback)
	{
		if (this._hasEffect && Singleton<Research>.Instance.IsResearching)
		{
			this._effect.SetActive(true);
		}
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x000281A0 File Offset: 0x000263A0
	public void OnEndCallback(EntityCallbackEvent callback)
	{
		if (Singleton<Research>.Instance.IsResearching)
		{
			string text;
			if (callback.Variable != null && callback.Variable.TryGetString(0, out text))
			{
				ResourceData resourceData = Library.RequestData<ResourceData>(text);
				if (resourceData != null)
				{
					this.ConvertResource(resourceData);
				}
				else
				{
					Debug.Log("[LAB] Could not convert resource with ID " + text);
				}
			}
			else
			{
				Debug.Log("[LAB] Does not have variable container!");
			}
			this.GetNextResource();
			return;
		}
		this.DisableResearch();
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00028214 File Offset: 0x00026414
	public void GetNextResource()
	{
		if (!NetworkPlayerManager.IS_HOST)
		{
			return;
		}
		ResourceContainer inputContainer = this._resourceModule.GetInputContainer();
		if (inputContainer == null)
		{
			return;
		}
		if (!Singleton<Research>.Instance.IsResearching)
		{
			this.DisableResearch();
			return;
		}
		StoredResource storedResource = null;
		int num = 0;
		foreach (KeyValuePair<ResourceData, int> keyValuePair in Singleton<Research>.Instance.TechRequirements)
		{
			StoredResource storedResource2;
			if (inputContainer.TryGetStoredResource(keyValuePair.Key, out storedResource2) && storedResource2.AmountStored > num)
			{
				storedResource = storedResource2;
				num = storedResource2.AmountStored;
			}
		}
		if (storedResource != null)
		{
			Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, this._cooldown.Value, new VariableContainer(0, storedResource.ResourceData.ID)));
			return;
		}
		this.DisableResearch();
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00028300 File Offset: 0x00026500
	public virtual void OnResearchActivated(ResearchTechData tech, List<Cost> costs)
	{
		this.GetNextResource();
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00028308 File Offset: 0x00026508
	public virtual void OnResearchCompleted(ResearchTechData data)
	{
		this.DisableResearch();
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00028310 File Offset: 0x00026510
	protected void DisableResearch()
	{
		if (this._hasEffect)
		{
			this._effect.SetActive(false);
		}
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00028328 File Offset: 0x00026528
	public override void OnReset()
	{
		this.DisableResearch();
		if (Singleton<Gamemode>.Instance.UseResearch && this._isListening)
		{
			Singleton<Events>.Instance.onResearchTechActivated.RemoveListener(new UnityAction<ResearchTechData, List<Cost>>(this.OnResearchActivated));
			Singleton<Events>.Instance.onResearchTechFinished.RemoveListener(new UnityAction<ResearchTechData>(this.OnResearchCompleted));
			this._isListening = false;
		}
		this._totalResearched = 0;
	}

	// Token: 0x040005D4 RID: 1492
	protected LabData _labData;

	// Token: 0x040005D5 RID: 1493
	protected AudioClip _researchingSound;

	// Token: 0x040005D6 RID: 1494
	protected StatFloat _cooldown;

	// Token: 0x040005D7 RID: 1495
	protected StatInt _value;

	// Token: 0x040005D8 RID: 1496
	protected int _totalResearched;

	// Token: 0x040005D9 RID: 1497
	protected GameObject _effect;

	// Token: 0x040005DA RID: 1498
	private bool _hasEffect;

	// Token: 0x040005DB RID: 1499
	private bool _isListening;
}
