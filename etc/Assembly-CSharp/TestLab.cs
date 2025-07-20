using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000135 RID: 309
public class TestLab : ResourceComponent, IComponent<TestLab, TestLabData>, ICallbackListener
{
	// Token: 0x06000A29 RID: 2601 RVA: 0x0002A7B7 File Offset: 0x000289B7
	public TestLabData GetData()
	{
		return this._labData;
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0002A7BF File Offset: 0x000289BF
	public int GetTotalResearched
	{
		get
		{
			return this._totalResearched;
		}
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0002A7C8 File Offset: 0x000289C8
	public void OnInitialize(TestLabData data)
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
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._cooldown, StatType.LabSpeed, data.cooldown, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._value, StatType.LabValue, data.value, this);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0002A878 File Offset: 0x00028A78
	public override void OnSpawn(bool fromSave)
	{
		if (this._hasEffect)
		{
			this._effect.SetActive(false);
		}
		if (!this._isListening)
		{
			Singleton<Events>.Instance.onResearchTechActivated.AddListener(new UnityAction<ResearchTechData, List<Cost>>(this.OnResearchActivated));
			Singleton<Events>.Instance.onResearchTechFinished.AddListener(new UnityAction<ResearchTechData>(this.OnResearchCompleted));
			this._isListening = true;
		}
		this.CreateContainers();
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0002A8E8 File Offset: 0x00028AE8
	protected override void CreateContainers()
	{
		if (this._labData.useIndicator)
		{
			base.CreateIndicator(ContainerType.Input, this._labData.indicatorPosition.x, this._labData.indicatorPosition.y);
		}
		base.CreateContainer(ContainerType.Input, StorageMode.LocalizedStorage, this._labData.storage, StatType.LabCapacity, ContainerFlags.Xray);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0002A940 File Offset: 0x00028B40
	private void ConvertResource(ResourceData resource)
	{
		if (Singleton<ResearchTest>.Instance != null)
		{
			Singleton<ResearchTest>.Instance.AddTechResource(resource, this._value.Value);
			this._resourceModule.TakeResource(ContainerType.Input, resource, 1);
			this._totalResearched++;
		}
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0002A98D File Offset: 0x00028B8D
	public override void OnAddResource(ContainerType type, ResourceData resource, int amount)
	{
		if (Singleton<ResearchTest>.Instance != null && Singleton<ResearchTest>.Instance.IsResearching && !base.IsUpdating)
		{
			this.GetNextResource();
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0002A9B6 File Offset: 0x00028BB6
	public void OnStartCallback(EntityCallbackEvent callback)
	{
		if (Singleton<ResearchTest>.Instance != null && this._hasEffect && Singleton<ResearchTest>.Instance.IsResearching)
		{
			this._effect.SetActive(true);
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0002A9E8 File Offset: 0x00028BE8
	public void OnEndCallback(EntityCallbackEvent callback)
	{
		if (Singleton<ResearchTest>.Instance != null)
		{
			if (Singleton<ResearchTest>.Instance.IsResearching)
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
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0002AA68 File Offset: 0x00028C68
	public void GetNextResource()
	{
		if (!NetworkPlayerManager.IS_HOST || Singleton<ResearchTest>.Instance == null)
		{
			return;
		}
		ResourceContainer inputContainer = this._resourceModule.GetInputContainer();
		if (inputContainer == null)
		{
			return;
		}
		if (!Singleton<ResearchTest>.Instance.IsResearching)
		{
			this.DisableResearch();
			return;
		}
		StoredResource storedResource = null;
		int num = 0;
		foreach (KeyValuePair<ResourceData, int> keyValuePair in Singleton<ResearchTest>.Instance.TechRequirements)
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

	// Token: 0x06000A33 RID: 2611 RVA: 0x0002AB60 File Offset: 0x00028D60
	public virtual void OnResearchActivated(ResearchTechData tech, List<Cost> costs)
	{
		this.GetNextResource();
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0002AB68 File Offset: 0x00028D68
	public virtual void OnResearchCompleted(ResearchTechData data)
	{
		this.DisableResearch();
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0002AB70 File Offset: 0x00028D70
	protected void DisableResearch()
	{
		if (this._hasEffect)
		{
			this._effect.SetActive(false);
		}
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x0002AB88 File Offset: 0x00028D88
	public override void OnReset()
	{
		this.DisableResearch();
		if (this._isListening)
		{
			Singleton<Events>.Instance.onResearchTechActivated.RemoveListener(new UnityAction<ResearchTechData, List<Cost>>(this.OnResearchActivated));
			Singleton<Events>.Instance.onResearchTechFinished.RemoveListener(new UnityAction<ResearchTechData>(this.OnResearchCompleted));
			this._isListening = false;
		}
		this._totalResearched = 0;
	}

	// Token: 0x0400063F RID: 1599
	protected TestLabData _labData;

	// Token: 0x04000640 RID: 1600
	protected AudioClip _researchingSound;

	// Token: 0x04000641 RID: 1601
	protected StatFloat _cooldown;

	// Token: 0x04000642 RID: 1602
	protected StatInt _value;

	// Token: 0x04000643 RID: 1603
	protected int _totalResearched;

	// Token: 0x04000644 RID: 1604
	protected GameObject _effect;

	// Token: 0x04000645 RID: 1605
	private bool _hasEffect;

	// Token: 0x04000646 RID: 1606
	private bool _isListening;
}
