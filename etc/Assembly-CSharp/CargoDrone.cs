using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Serialization;
using Vectorio.Stats;

// Token: 0x020000FE RID: 254
public class CargoDrone : Drone, IComponent<CargoDrone, DroneData>, ISyncListener
{
	// Token: 0x0600080D RID: 2061 RVA: 0x000238CD File Offset: 0x00021ACD
	public DroneData GetData()
	{
		return this._droneData;
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x0600080E RID: 2062 RVA: 0x000238D5 File Offset: 0x00021AD5
	public DroneCoverage PickupCoverage
	{
		get
		{
			return this._pickupCoverage;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600080F RID: 2063 RVA: 0x000238DD File Offset: 0x00021ADD
	public DroneCoverage DropoffCoverage
	{
		get
		{
			return this._dropoffCoverage;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000810 RID: 2064 RVA: 0x000238E5 File Offset: 0x00021AE5
	public bool HasPickupCoverage
	{
		get
		{
			return this._pickupCoverage != null;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000811 RID: 2065 RVA: 0x000238F0 File Offset: 0x00021AF0
	public bool HasDropoffCoverage
	{
		get
		{
			return this._dropoffCoverage != null;
		}
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x000238FC File Offset: 0x00021AFC
	public void SyncFilter(ResourceData filter)
	{
		this.Filter = filter;
		EntityMetadataEvent data = new EntityMetadataEvent
		{
			RuntimeID = base.RuntimeID,
			Metadata = base.Entity.ExtractMetadata(false, MetadataContext.Global),
			AsPipette = true
		};
		Singleton<EntityManager>.Instance.QueueMetadataEvent(data, SyncType.ClientInitiated);
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x00023948 File Offset: 0x00021B48
	// (set) Token: 0x06000814 RID: 2068 RVA: 0x00023950 File Offset: 0x00021B50
	public bool RequireFilter
	{
		get
		{
			return this._requireFilter;
		}
		set
		{
			this._requireFilter = value;
			if (this._requireFilter)
			{
				EntityStatus entityStatus = this._entityStatus;
				if (entityStatus == null)
				{
					return;
				}
				entityStatus.Toggle(this.Filter == null, EntityStatus.Type.NoFilter);
				return;
			}
			else
			{
				EntityStatus entityStatus2 = this._entityStatus;
				if (entityStatus2 == null)
				{
					return;
				}
				entityStatus2.Toggle(false, EntityStatus.Type.NoFilter);
				return;
			}
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000815 RID: 2069 RVA: 0x0002399C File Offset: 0x00021B9C
	// (set) Token: 0x06000816 RID: 2070 RVA: 0x000239A4 File Offset: 0x00021BA4
	public ResourceData Filter
	{
		get
		{
			return this._filter;
		}
		set
		{
			this._filter = value;
			if (this._filter != null)
			{
				if (this.RequireFilter)
				{
					EntityStatus entityStatus = this._entityStatus;
					if (entityStatus != null)
					{
						entityStatus.Toggle(false, EntityStatus.Type.NoFilter);
					}
				}
				if (this._xray == null)
				{
					if (this._parent == null)
					{
						Debug.Log("[CARGO DRONE] Cannot set xray target because this drone does not have a parent!");
						return;
					}
					this._xray = Singleton<LogisticsManager>.Instance.RegisterDroneXray(this._parent.transform, this);
					return;
				}
			}
			else if (this._xray != null)
			{
				if (this.RequireFilter)
				{
					EntityStatus entityStatus2 = this._entityStatus;
					if (entityStatus2 != null)
					{
						entityStatus2.Toggle(true, EntityStatus.Type.NoFilter);
					}
				}
				Singleton<LogisticsManager>.Instance.RemoveXrayTarget(this._xray);
				this._xray = null;
			}
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000817 RID: 2071 RVA: 0x00023A59 File Offset: 0x00021C59
	// (set) Token: 0x06000818 RID: 2072 RVA: 0x00023A61 File Offset: 0x00021C61
	public ResourceData Resource
	{
		get
		{
			return this._resource;
		}
		set
		{
			this._resource = value;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000819 RID: 2073 RVA: 0x00023A6A File Offset: 0x00021C6A
	// (set) Token: 0x0600081A RID: 2074 RVA: 0x00023A72 File Offset: 0x00021C72
	public int Deliveries
	{
		get
		{
			return this._deliveries;
		}
		set
		{
			this._deliveries = value;
		}
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00023A7C File Offset: 0x00021C7C
	public void OnInitialize(DroneData droneData)
	{
		this._droneData = droneData;
		if (this._icon == null)
		{
			this._icon = new GameObject("Resource Icon").AddComponent<SpriteRenderer>();
			this._icon.transform.SetParent(base.transform);
			this._icon.transform.localPosition = Vector2.zero;
			this._icon.transform.localScale = new Vector2(0.6f, 0.6f);
			this._icon.sortingLayerName = Layers.DRONE_LAYER;
			this._icon.material = Library.GetDefaultMaterial();
			this._icon.gameObject.SetActive(false);
		}
		this._entityStatus = base.Entity.Get_EComponent<EntityStatus>(true);
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00023B50 File Offset: 0x00021D50
	public override void OnSpawn(bool fromSave)
	{
		this._entityStatus.OnSpawn(fromSave);
		if (!fromSave)
		{
			this.RequireFilter = base.IsPlayerFaction;
		}
		this._icon.sortingOrder = -(base.Entity.GetModel.transform.childCount + 1);
		base.SetLayers();
		Singleton<StatManager>.Instance.CreateStatInt(ref this._maxActions, StatType.DroneMaxActions, this._droneData.maxActions, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._maxStorage, StatType.DroneCapacity, this._droneData.storage, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._moveSpeed, StatType.DroneSpeed, this._droneData.speed, this);
		if (base.IsPlayerFaction && !this._isRegistered && Singleton<DroneManager>.Instance != null)
		{
			Singleton<DroneManager>.Instance.AddCargoDrone(this);
			this._isRegistered = true;
		}
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00023C2C File Offset: 0x00021E2C
	public override void CreateCoverage(CoverageArea pickupArea, CoverageArea dropoffArea)
	{
		this._settingUpCoverage = true;
		if (pickupArea != null)
		{
			if (this._pickupCoverage != null)
			{
				this._pickupCoverage.ClearCoverage();
			}
			else
			{
				this._pickupCoverage = new DroneCoverage(this, CoverageType.Pickup);
			}
			this._pickupCoverage.SetArea(pickupArea);
		}
		if (dropoffArea != null)
		{
			if (this._dropoffCoverage != null)
			{
				this._dropoffCoverage.ClearCoverage();
			}
			else
			{
				this._dropoffCoverage = new DroneCoverage(this, CoverageType.Dropoff);
			}
			this._dropoffCoverage.SetArea(dropoffArea);
		}
		this._settingUpCoverage = false;
		if (!base.IsBusy)
		{
			this.TryGenerateRoute();
		}
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00023CB8 File Offset: 0x00021EB8
	public override DroneCoverage GetDroneCoverage(CoverageType coverage)
	{
		DroneCoverage result;
		if (coverage != CoverageType.Pickup)
		{
			if (coverage != CoverageType.Dropoff)
			{
				result = null;
			}
			else
			{
				result = this._dropoffCoverage;
			}
		}
		else
		{
			result = this._pickupCoverage;
		}
		return result;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00023CE4 File Offset: 0x00021EE4
	public override bool CheckTarget(Entity entity, CoverageType coverage)
	{
		if (!entity.IsAlly(base.Entity.FactionID) || entity.Has_EComponent<Blueprint>() || !entity.Has_EComponent<ResourceModule>())
		{
			return false;
		}
		ResourceModule resourceModule = entity.Get_EComponent<ResourceModule>(false);
		if (coverage == CoverageType.Pickup)
		{
			return resourceModule.HasOutputContainer();
		}
		return coverage == CoverageType.Dropoff && resourceModule.HasInputContainer();
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00023D35 File Offset: 0x00021F35
	public override void OnAddTarget(Entity entity, CoverageType coverage)
	{
		if (!this._settingUpCoverage)
		{
			this.TryGenerateRoute();
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00023D45 File Offset: 0x00021F45
	public override void OnRemoveTarget(Entity entity, CoverageType coverage)
	{
		if (!this._settingUpCoverage && base.IsBusy && entity == this._target)
		{
			this.NextAction();
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00023D6C File Offset: 0x00021F6C
	public override void Tick(float time)
	{
		Drone.Status droneStatus = base.DroneStatus;
		if (droneStatus != Drone.Status.Inactive)
		{
			if (droneStatus != Drone.Status.TravellingToTarget)
			{
				base.Tick(time);
			}
			else
			{
				if (this._target == null || this._target.Has_EFlag_IsDead)
				{
					this.NextAction();
					return;
				}
				this.MoveTowardsTarget(this._target.transform, time);
				this._travelTime -= time;
				if (this._hasCurrentLine && base.Entity.IsOnScreen)
				{
					this.CurrentLine.UpdateLine(base.transform.position, this._target.transform.position);
				}
				if (this._travelTime <= 0f)
				{
					this.ProcessAction();
					this.NextAction();
					return;
				}
			}
		}
		else
		{
			this.TryGenerateRoute();
			if (base.DroneStatus == Drone.Status.Inactive)
			{
				Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
				return;
			}
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000823 RID: 2083 RVA: 0x00023E51 File Offset: 0x00022051
	public Queue<ResourceAction> Actions
	{
		get
		{
			return this._actions;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000824 RID: 2084 RVA: 0x00023E59 File Offset: 0x00022059
	// (set) Token: 0x06000825 RID: 2085 RVA: 0x00023E61 File Offset: 0x00022061
	public ResourceAction CurrentAction
	{
		get
		{
			return this._currentAction;
		}
		set
		{
			this._currentAction = value;
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000826 RID: 2086 RVA: 0x00023E6A File Offset: 0x0002206A
	// (set) Token: 0x06000827 RID: 2087 RVA: 0x00023E72 File Offset: 0x00022072
	public EntityUtilities.ConnectionLine CurrentLine
	{
		get
		{
			return this._currentLine;
		}
		set
		{
			this._currentLine = value;
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00023E7C File Offset: 0x0002207C
	public void TryGenerateRoute()
	{
		if (!NetworkPlayerManager.IS_HOST)
		{
			return;
		}
		if (base.IsBusy || this._pickupCoverage == null || this._dropoffCoverage == null)
		{
			return;
		}
		if (this.RequireFilter && this.Filter == null)
		{
			return;
		}
		DroneActionPackage droneActionPackage;
		if (Singleton<DroneRouteGenerator>.Instance.ScheduleRouteGeneration(this._maxActions.Value, this._maxStorage.Value, this._dropoffCoverage.targets, this._pickupCoverage.targets, MetadataContext.Global, this.Filter, out droneActionPackage))
		{
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
			{
				byte[] data = DataProcessor.SerializeAndCompressObject<DroneActionPackage>(droneActionPackage);
				Singleton<EntityManager>.Instance.QueueSyncEvent(EventBuilder.BuildSyncEvent(true, base.RuntimeID, base.ComponentIndex, data), SyncType.ServerInitiated);
				return;
			}
			this.LoadActionPackage(droneActionPackage);
		}
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x00023F38 File Offset: 0x00022138
	public void OnSyncDataReceived(SyncEvent syncEvent)
	{
		if (syncEvent.Data != null)
		{
			DroneActionPackage droneActionPackage = DataProcessor.DecompressAndDeserializeObject<DroneActionPackage>(syncEvent.Data);
			if (droneActionPackage != null)
			{
				this.LoadActionPackage(droneActionPackage);
			}
		}
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00023F64 File Offset: 0x00022164
	public void LoadActionPackage(DroneActionPackage actionPackage)
	{
		if (actionPackage.HasActionQueue() && actionPackage.HasResource())
		{
			this.ClearActionQueue();
			this._resource = Library.RequestData<ResourceData>(actionPackage.ResourceID);
			if (this._resource == null)
			{
				Debug.Log("[CARGO DRONE] The provided resource is not available!");
				if (base.DroneStatus == Drone.Status.TravellingToTarget)
				{
					this.ReturnToParent();
				}
				return;
			}
			foreach (DroneAction droneAction in actionPackage.savedActions)
			{
				Entity entity;
				if (droneAction.ID.TryGetEntity(out entity) && !(entity == null))
				{
					ResourceModule resourceModule = entity.Get_EComponent<ResourceModule>(false);
					if (!(resourceModule == null))
					{
						this._actions.Enqueue(new ResourceAction(resourceModule, this._resource, droneAction.Amount, droneAction.Pickup));
					}
				}
			}
			if (base.DroneStatus == Drone.Status.TravellingToTarget)
			{
				Vector2 lastActionPosition = base.transform.position;
				foreach (ResourceAction resourceAction in this._actions)
				{
					this.ApplyPendingAction(resourceAction, lastActionPosition);
					lastActionPosition = resourceAction.Target.transform.position;
				}
				this.NextAction();
				return;
			}
			if (!base.IsBusy)
			{
				this.Deploy();
			}
		}
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x000240E0 File Offset: 0x000222E0
	public override void Deploy()
	{
		Vector2 lastActionPosition = base.transform.position;
		foreach (ResourceAction resourceAction in this._actions)
		{
			this.ApplyPendingAction(resourceAction, lastActionPosition);
			lastActionPosition = resourceAction.Target.transform.position;
		}
		base.Deploy();
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00024164 File Offset: 0x00022364
	private void ApplyPendingAction(ResourceAction action, Vector2 lastActionPosition)
	{
		if (action.Pickup)
		{
			action.Target.AddPendingAction(ContainerType.Output, action);
		}
		else
		{
			action.Target.AddPendingAction(ContainerType.Input, action);
		}
		if (Singleton<LogisticsManager>.Instance.RoutePreview)
		{
			EntityUtilities.ConnectionLine item = Singleton<EntityUtilities>.Instance.CreateConnectionLine(EntityUtilities.ConnectionType.DroneRoute, LegacyLibrary.DRONE_PREVIEW_SPRITE, LegacyLibrary.DRONE_PREVIEW_MATERIAL, this._resource.Accent.primaryColor, 10, lastActionPosition, action.Target.transform.position);
			this._previewLines.Enqueue(item);
		}
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x000241EC File Offset: 0x000223EC
	private void NextAction()
	{
		if (this.CurrentAction != null)
		{
			this.ClearPendingAction(this.CurrentAction);
		}
		if (this._actions.Count == 0)
		{
			this.ReturnToParent();
			return;
		}
		this.CurrentAction = this._actions.Dequeue();
		if (this._previewLines.Count > 0)
		{
			this.CurrentLine = this._previewLines.Peek();
			this._hasCurrentLine = true;
		}
		else
		{
			this._hasCurrentLine = false;
		}
		if (this.CurrentAction.Target == null || this.CurrentAction.Target.Entity.Has_EFlag_IsDead)
		{
			this.NextAction();
			return;
		}
		this._target = this.CurrentAction.Target.Entity;
		this.CalculateTravelTime(base.transform, this.CurrentAction.Target.transform, false);
		this.RotateToTarget(this.CurrentAction.Target.transform);
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x000242DC File Offset: 0x000224DC
	private void ProcessAction()
	{
		if (this.CurrentAction != null)
		{
			if (this.CurrentAction.Pickup)
			{
				if (this.CurrentAction.Target.HasResource(ContainerType.Output, this._resource))
				{
					this.CurrentAction.Target.TakeResource(ContainerType.Output, this._resource, this.CurrentAction.Amount);
					if (!this._icon.gameObject.activeSelf)
					{
						this._icon.gameObject.SetActive(true);
						return;
					}
				}
			}
			else
			{
				this.CurrentAction.Target.AddResource(ContainerType.Input, this._resource, this.CurrentAction.Amount);
				this._deliveries++;
			}
		}
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00024394 File Offset: 0x00022594
	public override void OnReset()
	{
		this.ClearActionQueue();
		if (this._pickupCoverage != null)
		{
			this._pickupCoverage.ClearCoverage();
			this._pickupCoverage = null;
		}
		if (this._dropoffCoverage != null)
		{
			this._dropoffCoverage.ClearCoverage();
			this._dropoffCoverage = null;
		}
		if (base.IsUpdating)
		{
			Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
		}
		if (this._xray != null)
		{
			Singleton<LogisticsManager>.Instance.RemoveXrayTarget(this._xray);
			this._xray = null;
		}
		this._target = null;
		this._filter = null;
		this._icon.gameObject.SetActive(false);
		base.OnReset();
		if (this._isRegistered && Singleton<DroneManager>.Instance != null)
		{
			this._isRegistered = false;
			Singleton<DroneManager>.Instance.RemoveCargoDrone(this);
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00024459 File Offset: 0x00022659
	public override void ReturnToParent()
	{
		this.ClearActionQueue();
		this._icon.gameObject.SetActive(false);
		this._target = null;
		base.ReturnToParent();
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00024480 File Offset: 0x00022680
	private void ClearActionQueue()
	{
		if (this.CurrentAction != null)
		{
			this.ClearPendingAction(this.CurrentAction);
			this.CurrentAction = null;
		}
		if (this._actions != null && this._actions.Count > 0)
		{
			foreach (ResourceAction action in this._actions)
			{
				this.ClearPendingAction(action);
			}
			this._actions.Clear();
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00024510 File Offset: 0x00022710
	private void ClearPendingAction(ResourceAction action)
	{
		if (action.Pickup)
		{
			action.Target.RemovePendingAction(ContainerType.Output, action);
		}
		else
		{
			action.Target.RemovePendingAction(ContainerType.Input, action);
		}
		if (this._previewLines.Count > 0)
		{
			EntityUtilities.ConnectionLine line = this._previewLines.Dequeue();
			Singleton<EntityUtilities>.Instance.DestroyConnectionLine(line);
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00024566 File Offset: 0x00022766
	public override void OnMouseHover(bool toggle)
	{
		Singleton<Selector>.Instance.PreviewDroneCoverage(this, toggle);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00024574 File Offset: 0x00022774
	public override void OnQuickEdit()
	{
		Singleton<Selector>.Instance.Open(this);
	}

	// Token: 0x0400054B RID: 1355
	protected DroneData _droneData;

	// Token: 0x0400054C RID: 1356
	protected DroneCoverage _pickupCoverage;

	// Token: 0x0400054D RID: 1357
	protected DroneCoverage _dropoffCoverage;

	// Token: 0x0400054E RID: 1358
	protected StatInt _maxActions;

	// Token: 0x0400054F RID: 1359
	protected StatInt _maxStorage;

	// Token: 0x04000550 RID: 1360
	protected int _deliveries;

	// Token: 0x04000551 RID: 1361
	protected bool _isRegistered;

	// Token: 0x04000552 RID: 1362
	protected Queue<EntityUtilities.ConnectionLine> _previewLines = new Queue<EntityUtilities.ConnectionLine>();

	// Token: 0x04000553 RID: 1363
	private LogisticsManager.Xray _xray;

	// Token: 0x04000554 RID: 1364
	private EntityStatus _entityStatus;

	// Token: 0x04000555 RID: 1365
	private bool _requireFilter;

	// Token: 0x04000556 RID: 1366
	private ResourceData _filter;

	// Token: 0x04000557 RID: 1367
	private ResourceData _resource;

	// Token: 0x04000558 RID: 1368
	protected SpriteRenderer _icon;

	// Token: 0x04000559 RID: 1369
	private bool _settingUpCoverage;

	// Token: 0x0400055A RID: 1370
	private Queue<ResourceAction> _actions = new Queue<ResourceAction>();

	// Token: 0x0400055B RID: 1371
	private ResourceAction _currentAction;

	// Token: 0x0400055C RID: 1372
	private EntityUtilities.ConnectionLine _currentLine;

	// Token: 0x0400055D RID: 1373
	private bool _hasCurrentLine;
}
