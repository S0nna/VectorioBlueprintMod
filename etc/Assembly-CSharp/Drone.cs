using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;
using Vectorio.Utilities;

// Token: 0x02000103 RID: 259
public class Drone : EntityComponent, IUpdateable
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x0600086B RID: 2155 RVA: 0x000252BB File Offset: 0x000234BB
	// (set) Token: 0x0600086C RID: 2156 RVA: 0x000252C3 File Offset: 0x000234C3
	public Drone.Status DroneStatus
	{
		get
		{
			return this._status;
		}
		set
		{
			this._status = value;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x000252CC File Offset: 0x000234CC
	public bool IsBusy
	{
		get
		{
			return this._status > Drone.Status.Inactive;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600086E RID: 2158 RVA: 0x000252D7 File Offset: 0x000234D7
	// (set) Token: 0x0600086F RID: 2159 RVA: 0x000252DF File Offset: 0x000234DF
	public bool IsRegistered
	{
		get
		{
			return this._isRegistered;
		}
		set
		{
			this._isRegistered = value;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000870 RID: 2160 RVA: 0x000252E8 File Offset: 0x000234E8
	public Port Parent
	{
		get
		{
			return this._parent;
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000871 RID: 2161 RVA: 0x000252F0 File Offset: 0x000234F0
	// (set) Token: 0x06000872 RID: 2162 RVA: 0x000252F8 File Offset: 0x000234F8
	public PortDoors PortDoors
	{
		get
		{
			return this._portDoors;
		}
		set
		{
			this._portDoors = value;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000873 RID: 2163 RVA: 0x00025301 File Offset: 0x00023501
	// (set) Token: 0x06000874 RID: 2164 RVA: 0x00025309 File Offset: 0x00023509
	public Entity Target
	{
		get
		{
			return this._target;
		}
		set
		{
			this._target = value;
		}
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00025309 File Offset: 0x00023509
	public virtual void SetMetadataTarget(Entity target)
	{
		this._target = target;
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00025312 File Offset: 0x00023512
	public Transform GetParentTransform()
	{
		if (!(this._parent != null))
		{
			return base.transform;
		}
		return this._parent.transform;
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void CreateCoverage(CoverageArea pickup, CoverageArea dropoff)
	{
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00025334 File Offset: 0x00023534
	public virtual DroneCoverage GetDroneCoverage(CoverageType coverage)
	{
		return null;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnAddTarget(Entity target, CoverageType coverage)
	{
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnRemoveTarget(Entity target, CoverageType coverage)
	{
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00025337 File Offset: 0x00023537
	public virtual bool CheckTarget(Entity target, CoverageType coverage)
	{
		return false;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x0002533A File Offset: 0x0002353A
	public virtual void LinkPort(Port port, PortDoors doors = null)
	{
		this._parent = port;
		this._portDoors = doors;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0002534C File Offset: 0x0002354C
	protected void SetLayers()
	{
		int childCount = base.Entity.GetModel.transform.childCount;
		foreach (object obj in base.Entity.GetModel.transform)
		{
			SpriteRenderer component = ((Transform)obj).GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.sortingOrder = 0 - childCount--;
				component.sortingLayerName = Layers.SORTING_BUILDING_LAYER;
				this._layers.Add(component);
			}
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x000253F0 File Offset: 0x000235F0
	public virtual void Deploy()
	{
		this.DroneStatus = Drone.Status.ExitingPort;
		this._parent.PlayCloseSound();
		if (!base.IsUpdating)
		{
			Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00025418 File Offset: 0x00023618
	public virtual void Tick(float time)
	{
		switch (this.DroneStatus)
		{
		case Drone.Status.ExitingPort:
			if (this._portDoors == null || this._portDoors.OpenDoors(time))
			{
				this.SwitchLayers(Layers.DRONE_LAYER);
				this.DroneStatus = Drone.Status.TravellingToTarget;
				return;
			}
			break;
		case Drone.Status.TravellingToTarget:
			break;
		case Drone.Status.ReturningToPort:
			this.MoveTowardsTarget(this._parent.transform, time);
			this._travelTime -= time;
			if (this._travelTime <= 0f)
			{
				base.transform.position = this._parent.transform.position;
				this.DroneStatus = Drone.Status.EnteringPort;
				this._parent.PlayOpenSound();
				this.SwitchLayers(Layers.SORTING_BUILDING_LAYER);
			}
			break;
		case Drone.Status.EnteringPort:
			if (this._portDoors == null || this._portDoors.CloseDoors(time))
			{
				this.DroneStatus = Drone.Status.Inactive;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00025500 File Offset: 0x00023700
	protected virtual void MoveTowardsTarget(Transform target, float timeSinceLastMovement)
	{
		this._step = this._moveSpeed.Value * timeSinceLastMovement;
		base.transform.position = Vector2.MoveTowards(base.transform.position, target.position, this._step);
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00025558 File Offset: 0x00023758
	public virtual void RotateToTarget(Transform target)
	{
		Vector2 vector = new Vector2(target.position.x, target.position.y) - new Vector2(base.transform.position.x, base.transform.position.y);
		base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f);
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x000255E2 File Offset: 0x000237E2
	protected virtual void CalculateTravelTime(Transform currentTarget, Transform newTarget, bool useCache = false)
	{
		this._travelTime = Singleton<TileGrid>.Instance.CalculateDistance(Utilities.ConvertWorldPositionToCell(currentTarget.position), Utilities.ConvertWorldPositionToCell(newTarget.position), useCache) / this._moveSpeed.Value;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00025624 File Offset: 0x00023824
	protected void SwitchLayers(string newSortingLayer)
	{
		foreach (SpriteRenderer spriteRenderer in this._layers)
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.sortingLayerName = newSortingLayer;
			}
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00025680 File Offset: 0x00023880
	public virtual void ReturnToParent()
	{
		if (this._parent != null)
		{
			Drone.Status droneStatus = this.DroneStatus;
			if (droneStatus != Drone.Status.ExitingPort)
			{
				if (droneStatus == Drone.Status.TravellingToTarget)
				{
					this.CalculateTravelTime(base.transform, this._parent.transform, false);
					this.DroneStatus = Drone.Status.ReturningToPort;
					this.RotateToTarget(this._parent.transform);
				}
			}
			else
			{
				base.transform.position = this._parent.transform.position;
				this.DroneStatus = Drone.Status.EnteringPort;
				this._parent.PlayOpenSound();
				this.SwitchLayers(Layers.SORTING_BUILDING_LAYER);
			}
		}
		else
		{
			this.DroneStatus = Drone.Status.Inactive;
		}
		this._target = null;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00025728 File Offset: 0x00023928
	public override void OnReset()
	{
		if (base.IsUpdating)
		{
			Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
		}
		if (this._parent != null)
		{
			base.transform.SetParent(this._parent.transform);
			base.transform.localPosition = Vector2.zero;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.SwitchLayers(Layers.SORTING_BUILDING_LAYER);
		this.DroneStatus = Drone.Status.Inactive;
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnMouseHover(bool toggle)
	{
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnQuickEdit()
	{
	}

	// Token: 0x0400056D RID: 1389
	[SerializeField]
	private Drone.Status _status;

	// Token: 0x0400056E RID: 1390
	protected StatFloat _moveSpeed;

	// Token: 0x0400056F RID: 1391
	protected Entity _target;

	// Token: 0x04000570 RID: 1392
	protected Port _parent;

	// Token: 0x04000571 RID: 1393
	protected PortDoors _portDoors;

	// Token: 0x04000572 RID: 1394
	private bool _isRegistered;

	// Token: 0x04000573 RID: 1395
	private List<SpriteRenderer> _layers = new List<SpriteRenderer>();

	// Token: 0x04000574 RID: 1396
	protected float _travelTime;

	// Token: 0x04000575 RID: 1397
	protected float _step;

	// Token: 0x02000104 RID: 260
	public enum Status
	{
		// Token: 0x04000577 RID: 1399
		Inactive,
		// Token: 0x04000578 RID: 1400
		ExitingPort,
		// Token: 0x04000579 RID: 1401
		TravellingToTarget,
		// Token: 0x0400057A RID: 1402
		ReturningToPort,
		// Token: 0x0400057B RID: 1403
		EnteringPort
	}
}
