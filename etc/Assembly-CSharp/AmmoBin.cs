using System;
using Vectorio.Stats;

// Token: 0x020000F1 RID: 241
public class AmmoBin : ResourceComponent, IComponent<AmmoBin, AmmoBinData>
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600079D RID: 1949 RVA: 0x0002241C File Offset: 0x0002061C
	// (remove) Token: 0x0600079E RID: 1950 RVA: 0x00022454 File Offset: 0x00020654
	public event AmmoBin.AmmoAddedEvent OnAmmoAdded = delegate()
	{
	};

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x00022489 File Offset: 0x00020689
	public bool HasResource
	{
		get
		{
			return this._hasResource;
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00022491 File Offset: 0x00020691
	public AmmoBinData GetData()
	{
		return this._data;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00022499 File Offset: 0x00020699
	public void OnInitialize(AmmoBinData data)
	{
		this._data = data;
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x000224A4 File Offset: 0x000206A4
	public override void OnSpawn(bool fromSave)
	{
		this._statusComponent = base.Entity.Get_EComponent<EntityStatus>(true);
		this._statusComponent.OnSpawn(fromSave);
		this.CreateContainers();
		this._storage = this._resourceModule.GetInputContainer().EnsureResource(this._data.ammoRequired);
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x000224F8 File Offset: 0x000206F8
	protected override void CreateContainers()
	{
		if (this._data.useIndicator)
		{
			base.CreateIndicator(ContainerType.Output, this._data.indicatorPosition.x, this._data.indicatorPosition.y);
		}
		base.CreateContainer(ContainerType.Input, StorageMode.LocalizedStorage, this._data.storage, StatType.AmmoBinCapacity, ContainerFlags.Xray);
		this._resourceModule.GetInputContainer().ApplyFilter(this._data.ammoRequired);
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0002256A File Offset: 0x0002076A
	public void Consume(int amount)
	{
		this._resourceModule.TakeResource(ContainerType.Input, this._storage.ResourceData, amount);
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00022585 File Offset: 0x00020785
	public override void OnAddResource(ContainerType type, ResourceData resource, int amount)
	{
		this._hasResource = (this._storage.AmountStored > 0);
		this._statusComponent.Toggle(!this._hasResource, EntityStatus.Type.Ammo);
		this.OnAmmoAdded();
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x000225BB File Offset: 0x000207BB
	public override void OnTakeResource(ContainerType type, ResourceData resource, int amount)
	{
		this._hasResource = (this._storage.AmountStored > 0);
		this._statusComponent.Toggle(!this._hasResource, EntityStatus.Type.Ammo);
	}

	// Token: 0x04000520 RID: 1312
	protected AmmoBinData _data;

	// Token: 0x04000521 RID: 1313
	protected StoredResource _storage;

	// Token: 0x04000522 RID: 1314
	private EntityStatus _statusComponent;

	// Token: 0x04000523 RID: 1315
	protected bool _hasResource;

	// Token: 0x020000F2 RID: 242
	// (Invoke) Token: 0x060007A9 RID: 1961
	public delegate void AmmoAddedEvent();
}
