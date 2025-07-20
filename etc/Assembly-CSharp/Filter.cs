using System;
using Vectorio.Stats;

// Token: 0x02000110 RID: 272
public class Filter : ResourceComponent, IComponent<Filter, FilterData>
{
	// Token: 0x06000915 RID: 2325 RVA: 0x00026AF2 File Offset: 0x00024CF2
	public FilterData GetData()
	{
		return this._filterData;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00026AFA File Offset: 0x00024CFA
	public void OnInitialize(FilterData data)
	{
		this._filterData = data;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00026B03 File Offset: 0x00024D03
	public override void OnSpawn(bool fromeSave)
	{
		this.CreateContainers();
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x00026B0B File Offset: 0x00024D0B
	public bool IsFiltering
	{
		get
		{
			return this._resourceModule.GetInputContainer().HasFilter();
		}
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00026B1D File Offset: 0x00024D1D
	public ResourceData GetFilter()
	{
		return this._resourceModule.GetInputContainer().GetFilter();
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00026B2F File Offset: 0x00024D2F
	public void SetFilter(ResourceData filter)
	{
		if (this.IsFiltering)
		{
			this.ClearFilter();
		}
		this._resourceModule.GetInputContainer().ApplyFilter(filter);
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00026B50 File Offset: 0x00024D50
	public void ClearFilter()
	{
		this._resourceModule.GetInputContainer().Clear();
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00026B64 File Offset: 0x00024D64
	protected override void CreateContainers()
	{
		if (this._filterData.useIndicator)
		{
			base.CreateIndicator(ContainerType.Output, this._filterData.indicatorPosition.x, this._filterData.indicatorPosition.y);
		}
		base.CreateContainer(ContainerType.Output, this._filterData.mode, this._filterData.storage, StatType.StorageCapacity, ContainerFlags.Xray | ContainerFlags.RouteInputToOutput);
	}

	// Token: 0x040005A8 RID: 1448
	private FilterData _filterData;
}
