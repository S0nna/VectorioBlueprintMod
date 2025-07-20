using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class ResourceModule : EntityComponent
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060009D4 RID: 2516 RVA: 0x00029470 File Offset: 0x00027670
	// (remove) Token: 0x060009D5 RID: 2517 RVA: 0x000294A8 File Offset: 0x000276A8
	public event ResourceModule.ResourceEvent OnResourceAdded = delegate(ContainerType <p0>, ResourceData <p1>, int <p2>)
	{
	};

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060009D6 RID: 2518 RVA: 0x000294E0 File Offset: 0x000276E0
	// (remove) Token: 0x060009D7 RID: 2519 RVA: 0x00029518 File Offset: 0x00027718
	public event ResourceModule.ResourceEvent OnResourceRemoved = delegate(ContainerType <p0>, ResourceData <p1>, int <p2>)
	{
	};

	// Token: 0x060009D8 RID: 2520 RVA: 0x0002954D File Offset: 0x0002774D
	private void RegisterGlobalContainer(ResourceContainer container, ContainerType type)
	{
		if (!this._registeredAsGlobalStorage)
		{
			this._globalContainer = container;
			this._globalContainerType = type;
			Singleton<ResourceManager>.Instance.RegisterGlobalContainer(this);
			this._registeredAsGlobalStorage = true;
		}
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00029577 File Offset: 0x00027777
	private void UnregisterGlobalContainer()
	{
		if (this._registeredAsGlobalStorage)
		{
			Singleton<ResourceManager>.Instance.UnregisterGlobalContainer(this);
			this._registeredAsGlobalStorage = false;
			this._globalContainer = null;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x0002959A File Offset: 0x0002779A
	public bool IsRegisteredAsGlobalStorage
	{
		get
		{
			return this._registeredAsGlobalStorage;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x060009DB RID: 2523 RVA: 0x000295A2 File Offset: 0x000277A2
	public ResourceContainer GlobalContainer
	{
		get
		{
			return this._globalContainer;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x000295AA File Offset: 0x000277AA
	public ContainerType GlobalContainerType
	{
		get
		{
			return this._globalContainerType;
		}
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x000295B4 File Offset: 0x000277B4
	public void CreateContainer(ContainerCreationData container)
	{
		ContainerType type = container.Type;
		if (type != ContainerType.Input)
		{
			if (type == ContainerType.Output)
			{
				if (!this.HasOutputContainer())
				{
					this._outputContainer = new ResourceContainer(container, null);
					this._hasOutputContainer = true;
				}
				else
				{
					this._outputContainer.Setup(container, null);
				}
				if (container.GlobalStorage)
				{
					this.RegisterGlobalContainer(this._outputContainer, container.Type);
				}
				if (container.Xray && !this._hasXray)
				{
					this._xray = Singleton<LogisticsManager>.Instance.RegisterContainerXray(base.transform, this._outputContainer);
					this._hasXray = true;
				}
			}
		}
		else
		{
			if (!this.HasInputContainer())
			{
				this._inputContainer = new ResourceContainer(container, null);
				this._hasInputContainer = true;
			}
			else
			{
				this._inputContainer.Setup(container, null);
			}
			if (container.GlobalStorage)
			{
				this.RegisterGlobalContainer(this._inputContainer, container.Type);
			}
			if (container.Xray && !this._hasXray)
			{
				this._xray = Singleton<LogisticsManager>.Instance.RegisterContainerXray(base.transform, this._inputContainer);
				this._hasXray = true;
			}
		}
		if (container.RouteInputToOutput)
		{
			this.RouteInputToOutput = true;
		}
		if (this._hasBuildingComponent)
		{
			Singleton<TileGrid>.Instance.UpdateMultipleCells(this._building.Cells);
		}
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00029703 File Offset: 0x00027903
	public override void OnSpawn(bool fromSave)
	{
		if (base.Entity.Has_EComponent<Building>())
		{
			this._building = base.Entity.Get_EComponent<Building>(false);
			this._hasBuildingComponent = true;
			return;
		}
		this._hasBuildingComponent = false;
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x060009DF RID: 2527 RVA: 0x00029733 File Offset: 0x00027933
	// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0002973B File Offset: 0x0002793B
	public bool RouteInputToOutput
	{
		get
		{
			return this._routeInputToOutput;
		}
		set
		{
			this._routeInputToOutput = value;
		}
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00029744 File Offset: 0x00027944
	public bool HasInputContainer()
	{
		if (!this.RouteInputToOutput)
		{
			return this._hasInputContainer;
		}
		return this._hasOutputContainer;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0002975B File Offset: 0x0002795B
	public bool HasOutputContainer()
	{
		return this._hasOutputContainer;
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x00029763 File Offset: 0x00027963
	public ResourceContainer GetResourceContainer(ContainerType type)
	{
		if (type != ContainerType.Input)
		{
			return this.GetOutputContainer();
		}
		return this.GetInputContainer();
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x00029775 File Offset: 0x00027975
	public ResourceContainer GetResourceContainer(CoverageType type)
	{
		if (type != CoverageType.Pickup)
		{
			return this.GetInputContainer();
		}
		return this.GetOutputContainer();
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00029788 File Offset: 0x00027988
	public ResourceContainer GetInputContainer()
	{
		if (!this.RouteInputToOutput)
		{
			return this._inputContainer;
		}
		return this._outputContainer;
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x0002979F File Offset: 0x0002799F
	public ResourceContainer GetOutputContainer()
	{
		return this._outputContainer;
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x060009E7 RID: 2535 RVA: 0x000297A7 File Offset: 0x000279A7
	public Indicator Indicator
	{
		get
		{
			return this._indicator;
		}
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x000297B0 File Offset: 0x000279B0
	public void CreateIndicator(ContainerType container, float xPos, float yPos)
	{
		if (this._indicator == null)
		{
			this._indicator = Singleton<ModelConstructor>.Instance.ConstructIndicator(base.transform, new Vector2(xPos, yPos), Layers.SORTING_BUILDING_LAYER);
		}
		this._indicator.SetStatus(Indicator.Status.Waiting);
		this._indicatorContainer = container;
		this._useResourceIndicator = true;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00029808 File Offset: 0x00027A08
	public void UpdateIndicator(ResourceData resource)
	{
		if (this._useResourceIndicator)
		{
			ResourceContainer resourceContainer = this.GetResourceContainer(this._indicatorContainer);
			if (resourceContainer == null)
			{
				return;
			}
			if (resourceContainer.IsFull(resource))
			{
				this.Indicator.SetStatus(Indicator.Status.Inactive);
				return;
			}
			if (resourceContainer.IsEmpty(resource))
			{
				this.Indicator.SetStatus(Indicator.Status.Waiting);
				return;
			}
			this.Indicator.SetStatus(Indicator.Status.Active);
		}
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00029868 File Offset: 0x00027A68
	public bool HasResource(ContainerType type, ResourceData resource)
	{
		if (this._routeInputToOutput && type == ContainerType.Input)
		{
			type = ContainerType.Output;
		}
		ResourceContainer resourceContainer = this.GetResourceContainer(type);
		return resourceContainer != null && resourceContainer.HasResource(resource);
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00029898 File Offset: 0x00027A98
	public int AddResource(ContainerType type, ResourceData resource, int amount)
	{
		if (this._routeInputToOutput && type == ContainerType.Input)
		{
			type = ContainerType.Output;
		}
		ResourceContainer resourceContainer = this.GetResourceContainer(type);
		if (resourceContainer == null)
		{
			return amount;
		}
		if (DevTools.INSTANT_FILL)
		{
			amount = resourceContainer.GetSpace(resource);
		}
		int num = resourceContainer.AddAmount(resource, amount);
		bool flag = !resourceContainer.HasPendingAction(resource);
		if (this.IsRegisteredAsGlobalStorage && resource.CanBeStoredGlobally)
		{
			Singleton<ResourceManager>.Instance.Add(resource, amount - num);
		}
		ResourceModule.ResourceEvent onResourceAdded = this.OnResourceAdded;
		if (onResourceAdded != null)
		{
			onResourceAdded(type, resource, amount);
		}
		this.UpdateIndicator(resource);
		if (flag)
		{
			this.RequestDrone(type, resource);
		}
		return num;
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00029928 File Offset: 0x00027B28
	public int TakeResource(ContainerType type, ResourceData resource, int amount)
	{
		ContainerType container = this._routeInputToOutput ? ContainerType.Input : type;
		if (this._routeInputToOutput && type == ContainerType.Input)
		{
			type = ContainerType.Output;
		}
		ResourceContainer resourceContainer = this.GetResourceContainer(type);
		if (resourceContainer == null)
		{
			return 0;
		}
		int num = resourceContainer.RemoveAmount(resource, amount);
		bool flag = !resourceContainer.HasPendingAction(resource);
		if (this.IsRegisteredAsGlobalStorage && resource.CanBeStoredGlobally)
		{
			Singleton<ResourceManager>.Instance.Remove(resource, num);
		}
		ResourceModule.ResourceEvent onResourceRemoved = this.OnResourceRemoved;
		if (onResourceRemoved != null)
		{
			onResourceRemoved(type, resource, amount);
		}
		this.UpdateIndicator(resource);
		if (flag)
		{
			this.RequestDrone(container, resource);
		}
		return num;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x000299B4 File Offset: 0x00027BB4
	public void RequestDrone(ContainerType container, ResourceData resource)
	{
		if (!this._hasBuildingComponent)
		{
			return;
		}
		CoverageType type = (container == ContainerType.Input) ? CoverageType.Dropoff : CoverageType.Pickup;
		List<CargoDrone> list = new List<CargoDrone>();
		Singleton<TileGrid>.Instance.CheckTilesForAvailableDrone<CargoDrone>(this._building.Cells, type, ref list);
		CargoDrone cargoDrone = null;
		float num = float.PositiveInfinity;
		foreach (CargoDrone cargoDrone2 in list)
		{
			float num2 = Vector2.Distance(base.transform.position, cargoDrone2.GetParentTransform().position);
			if (num2 < num)
			{
				cargoDrone = cargoDrone2;
				num = num2;
			}
		}
		if (cargoDrone != null)
		{
			cargoDrone.TryGenerateRoute();
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00029A78 File Offset: 0x00027C78
	public void AddPendingAction(ContainerType type, ResourceAction action)
	{
		if (this._routeInputToOutput && type == ContainerType.Input)
		{
			type = ContainerType.Output;
		}
		ResourceContainer resourceContainer = this.GetResourceContainer(type);
		if (resourceContainer == null)
		{
			return;
		}
		resourceContainer.SetPendingAction(action);
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00029A9A File Offset: 0x00027C9A
	public void RemovePendingAction(ContainerType type, ResourceAction action)
	{
		if (this._routeInputToOutput && type == ContainerType.Input)
		{
			type = ContainerType.Output;
		}
		ResourceContainer resourceContainer = this.GetResourceContainer(type);
		if (resourceContainer == null)
		{
			return;
		}
		resourceContainer.ClearPendingAction(action);
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00029ABC File Offset: 0x00027CBC
	public override void OnReset()
	{
		if (this.IsRegisteredAsGlobalStorage)
		{
			this.UnregisterGlobalContainer();
		}
		if (this._hasXray)
		{
			Singleton<LogisticsManager>.Instance.RemoveXrayTarget(this._xray);
			this._hasXray = false;
			this._xray = null;
		}
		if (this._hasInputContainer)
		{
			this._inputContainer.Reset();
		}
		if (this._hasOutputContainer)
		{
			this._outputContainer.Reset();
		}
	}

	// Token: 0x0400060B RID: 1547
	protected Building _building;

	// Token: 0x0400060C RID: 1548
	protected bool _hasBuildingComponent;

	// Token: 0x0400060F RID: 1551
	[SerializeField]
	private ResourceContainer _inputContainer;

	// Token: 0x04000610 RID: 1552
	[SerializeField]
	private ResourceContainer _outputContainer;

	// Token: 0x04000611 RID: 1553
	private bool _hasInputContainer;

	// Token: 0x04000612 RID: 1554
	private bool _hasOutputContainer;

	// Token: 0x04000613 RID: 1555
	private bool _routeInputToOutput;

	// Token: 0x04000614 RID: 1556
	private bool _registeredAsGlobalStorage;

	// Token: 0x04000615 RID: 1557
	private ResourceContainer _globalContainer;

	// Token: 0x04000616 RID: 1558
	private ContainerType _globalContainerType;

	// Token: 0x04000617 RID: 1559
	private LogisticsManager.Xray _xray;

	// Token: 0x04000618 RID: 1560
	private bool _hasXray;

	// Token: 0x04000619 RID: 1561
	private Indicator _indicator;

	// Token: 0x0400061A RID: 1562
	private ContainerType _indicatorContainer;

	// Token: 0x0400061B RID: 1563
	private bool _useResourceIndicator;

	// Token: 0x0200012D RID: 301
	// (Invoke) Token: 0x060009F3 RID: 2547
	public delegate void ResourceEvent(ContainerType type, ResourceData resource, int amount);
}
