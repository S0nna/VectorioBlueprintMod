using System;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000F8 RID: 248
public class BuilderDrone : Drone, IComponent<BuilderDrone, BuilderDroneData>
{
	// Token: 0x060007D1 RID: 2001 RVA: 0x00022AFB File Offset: 0x00020CFB
	public BuilderDroneData GetData()
	{
		return this._droneData;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00022B03 File Offset: 0x00020D03
	public void OnInitialize(BuilderDroneData droneData)
	{
		this._droneData = droneData;
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00022B0C File Offset: 0x00020D0C
	public override void OnSpawn(bool fromSave)
	{
		Singleton<DroneManager>.Instance.AddBuilderDrone(this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._moveSpeed, StatType.DroneSpeed, this._droneData.speed, this);
		base.SetLayers();
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00022B3D File Offset: 0x00020D3D
	public override void SetMetadataTarget(Entity target)
	{
		base.SetMetadataTarget(target);
		if (target.Has_EComponent<Blueprint>())
		{
			this.SetTarget(target.Get_EComponent<Blueprint>(false));
		}
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00022B5C File Offset: 0x00020D5C
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
				if (this._blueprint == null || this._blueprint.Entity.Has_EFlag_IsDead)
				{
					this.ReturnToParent();
					return;
				}
				this.MoveTowardsTarget(this._blueprint.transform, time);
				this._travelTime -= time;
				if (this._travelTime <= 0f)
				{
					this._blueprint.Construct();
					this.ReturnToParent();
					return;
				}
			}
			return;
		}
		Singleton<DroneManager>.Instance.AddBuilderDrone(this);
		Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00022C00 File Offset: 0x00020E00
	public void SetTarget(Blueprint blueprint)
	{
		if (this._blueprint != null)
		{
			this._blueprint.ClearBuilder();
		}
		if (blueprint == null)
		{
			Debug.Log("[BUILDER] The blueprint passed in was not valid!");
			if (base.IsBusy)
			{
				this.ReturnToParent();
			}
			return;
		}
		this._blueprint = blueprint;
		this._blueprint.SetBuilder(this);
		this._target = this._blueprint.Entity;
		if (base.DroneStatus == Drone.Status.Inactive)
		{
			base.DroneStatus = Drone.Status.ExitingPort;
		}
		this.RotateToTarget(this._target.transform);
		this.CalculateTravelTime(this._parent.transform, this._target.transform, false);
		Singleton<DroneManager>.Instance.RemoveBuilderDrone(this);
		if (!base.IsUpdating)
		{
			Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
		}
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00022CC7 File Offset: 0x00020EC7
	public void ResetBlueprintTarget()
	{
		if (this._blueprint != null)
		{
			Singleton<DroneManager>.Instance.AddBlueprint(this._blueprint);
			this._blueprint = null;
		}
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00022CEE File Offset: 0x00020EEE
	public void OnTargetDestroyed()
	{
		this.ReturnToParent();
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00022CF6 File Offset: 0x00020EF6
	public override void ReturnToParent()
	{
		this._target = null;
		base.ReturnToParent();
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00022D05 File Offset: 0x00020F05
	public override void OnReset()
	{
		this.ResetBlueprintTarget();
		Singleton<DroneManager>.Instance.RemoveBuilderDrone(this);
		base.OnReset();
	}

	// Token: 0x04000535 RID: 1333
	private BuilderDroneData _droneData;

	// Token: 0x04000536 RID: 1334
	protected Blueprint _blueprint;
}
