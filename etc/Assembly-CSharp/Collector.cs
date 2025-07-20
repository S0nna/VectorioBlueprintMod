using System;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;
using Vectorio.Utilities;

// Token: 0x02000100 RID: 256
public class Collector : ResourceComponent, ICallbackListener, IComponent<Collector, CollectorData>
{
	// Token: 0x0600083B RID: 2107 RVA: 0x00024618 File Offset: 0x00022818
	public CollectorData GetData()
	{
		return this._collectorData;
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00024620 File Offset: 0x00022820
	public void OnInitialize(CollectorData data)
	{
		this._collectorData = data;
		if (base.Entity.GetModel.GetAnimationGroup(AnimationGroup.Primary).Count > 0)
		{
			Transform transform = base.Entity.GetModel.GetAnimationGroup(AnimationGroup.Primary)[0];
			this._resourceIcon = transform.GetComponent<SpriteRenderer>();
			this._resourceIcon.enabled = false;
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00024680 File Offset: 0x00022880
	public override void OnSpawn(bool fromSave)
	{
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._cooldown, StatType.CollectorSpeed, this._collectorData.cooldown, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._value, StatType.CollectorValue, this._collectorData.value, this);
		this.CheckForResource();
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x000246D0 File Offset: 0x000228D0
	protected override void CreateContainers()
	{
		if (this._collectorData.useIndicator)
		{
			base.CreateIndicator(ContainerType.Output, this._collectorData.indicatorPosition.x, this._collectorData.indicatorPosition.y);
		}
		base.CreateContainer(ContainerType.Output, this._collectorData.mode, this._collectorData.storage, StatType.CollectorCapacity, ContainerFlags.GeneratesResources | ContainerFlags.RouteInputToOutput);
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00024734 File Offset: 0x00022934
	public void CheckForResource()
	{
		this._resourceNode = Singleton<TileGrid>.Instance.GetResource(Utilities.ConvertWorldPositionToCell(base.transform.position));
		if (this._resourceNode != null)
		{
			this.CreateContainers();
			Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, this._cooldown.Value, null));
			if (this._resourceIcon != null)
			{
				this._resourceIcon.sprite = this._resourceNode.IconSprite;
				this._resourceIcon.enabled = true;
			}
			else
			{
				Debug.Log("[COLLECTOR] Could not set resource icon for " + base.transform.name + " because no primary animation group was set!");
			}
			if (base.Entity.IsPlayerFaction)
			{
				base.Entity.GetModel.ApplyAccent(this._resourceNode.Accent);
			}
		}
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00024820 File Offset: 0x00022A20
	public override void OnAddResource(ContainerType type, ResourceData resource, int amount)
	{
		if (this._collectorData.useCollectSound && base.Entity.IsOnScreen)
		{
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._collectorData.collectSound, base.transform.name, base.transform.position, 1f, true, 0.9f, 1.1f, false);
		}
		if (!base.IsUpdating && !this._resourceModule.GetOutputContainer().IsFull(null))
		{
			Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, this._cooldown.Value, null));
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x000248CC File Offset: 0x00022ACC
	public override void OnTakeResource(ContainerType type, ResourceData resource, int amount)
	{
		if (!base.IsUpdating && !this._resourceModule.GetOutputContainer().IsFull(null))
		{
			Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, this._cooldown.Value, null));
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartCallback(EntityCallbackEvent callback)
	{
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0002491B File Offset: 0x00022B1B
	public void OnEndCallback(EntityCallbackEvent callback)
	{
		this._resourceModule.AddResource(ContainerType.Output, this._resourceNode, this._value.Value);
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0002493B File Offset: 0x00022B3B
	public override void OnReset()
	{
		base.OnReset();
		if (this._resourceIcon != null)
		{
			this._resourceIcon.enabled = false;
		}
	}

	// Token: 0x04000561 RID: 1377
	private CollectorData _collectorData;

	// Token: 0x04000562 RID: 1378
	protected ResourceData _resourceNode;

	// Token: 0x04000563 RID: 1379
	protected SpriteRenderer _resourceIcon;

	// Token: 0x04000564 RID: 1380
	protected StatFloat _cooldown;

	// Token: 0x04000565 RID: 1381
	protected StatInt _value;
}
