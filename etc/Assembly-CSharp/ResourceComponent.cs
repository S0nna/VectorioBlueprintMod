using System;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x0200018A RID: 394
public abstract class ResourceComponent : BuildingComponent
{
	// Token: 0x06000D43 RID: 3395 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnAddResource(ContainerType type, ResourceData resource, int amount)
	{
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnTakeResource(ContainerType type, ResourceData resource, int amount)
	{
	}

	// Token: 0x06000D45 RID: 3397
	protected abstract void CreateContainers();

	// Token: 0x06000D46 RID: 3398 RVA: 0x00039A45 File Offset: 0x00037C45
	protected void CreateContainer(ContainerType type, StorageMode mode, int storage, StatType statType, ContainerFlags flags = ContainerFlags.None)
	{
		this.CreateContainer(type, mode, storage, (int)statType, flags);
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00039A54 File Offset: 0x00037C54
	protected void CreateContainer(ContainerType type, StorageMode mode, int storage, int statCode, ContainerFlags flags = ContainerFlags.None)
	{
		if (this._resourceModule == null)
		{
			this._resourceModule = base.Entity.Get_EComponent<ResourceModule>(true);
			this._resourceModule.OnSpawn(false);
		}
		if (!this._listening)
		{
			this._resourceModule.OnResourceAdded += this.OnAddResource;
			this._resourceModule.OnResourceRemoved += this.OnTakeResource;
			this._listening = true;
		}
		ContainerCreationData container = EventBuilder.BuildContainer(base.RuntimeID, type, mode, storage, statCode, flags);
		this._resourceModule.CreateContainer(container);
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00039AEC File Offset: 0x00037CEC
	protected void CreateIndicator(ContainerType type, float posX, float posY)
	{
		if (!this._hasIndicator)
		{
			if (this._resourceModule == null)
			{
				this._resourceModule = base.Entity.Get_EComponent<ResourceModule>(true);
				this._resourceModule.OnSpawn(false);
			}
			this._resourceModule.CreateIndicator(type, posX, posY);
			this._hasIndicator = true;
		}
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x00039B44 File Offset: 0x00037D44
	public override void OnReset()
	{
		if (this._listening)
		{
			this._resourceModule.OnResourceAdded -= this.OnAddResource;
			this._resourceModule.OnResourceRemoved -= this.OnTakeResource;
			this._listening = false;
		}
	}

	// Token: 0x0400093C RID: 2364
	protected ResourceModule _resourceModule;

	// Token: 0x0400093D RID: 2365
	private bool _listening;

	// Token: 0x0400093E RID: 2366
	private bool _hasIndicator;
}
