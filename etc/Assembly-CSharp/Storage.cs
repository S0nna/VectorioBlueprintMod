using System;
using Vectorio.Stats;

// Token: 0x02000134 RID: 308
public class Storage : ResourceComponent, IComponent<Storage, StorageData>
{
	// Token: 0x06000A24 RID: 2596 RVA: 0x0002A71D File Offset: 0x0002891D
	public StorageData GetData()
	{
		return this._storageData;
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0002A725 File Offset: 0x00028925
	public void OnInitialize(StorageData data)
	{
		this._storageData = data;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00026B03 File Offset: 0x00024D03
	public override void OnSpawn(bool fromSave)
	{
		this.CreateContainers();
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0002A730 File Offset: 0x00028930
	protected override void CreateContainers()
	{
		if (this._storageData.useIndicator)
		{
			base.CreateIndicator(ContainerType.Output, this._storageData.indicatorPosition.x, this._storageData.indicatorPosition.y);
		}
		ContainerFlags containerFlags = ContainerFlags.Xray | ContainerFlags.RouteInputToOutput;
		if (this._storageData.globalStorage && Singleton<FactionManager>.Instance.IsPlayerFaction(base.FactionID))
		{
			containerFlags |= ContainerFlags.GlobalStorage;
		}
		base.CreateContainer(ContainerType.Output, this._storageData.mode, this._storageData.storage, StatType.StorageCapacity, containerFlags);
	}

	// Token: 0x0400063E RID: 1598
	private StorageData _storageData;
}
