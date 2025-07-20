using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005D RID: 93
[DefaultExecutionOrder(0)]
public class DroneManager : Singleton<DroneManager>
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x0600047C RID: 1148 RVA: 0x00017B9D File Offset: 0x00015D9D
	public int TotalDronesAvailable
	{
		get
		{
			return this._builderDrones.Count;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x0600047D RID: 1149 RVA: 0x00017BAA File Offset: 0x00015DAA
	public int TotalBlueprintsQueued
	{
		get
		{
			return this._blueprints.Count;
		}
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00017BB7 File Offset: 0x00015DB7
	public void AddBlueprint(Blueprint blueprint)
	{
		if (!blueprint.IsRegistered)
		{
			this._blueprints.Add(blueprint);
			blueprint.IsRegistered = true;
			this.SortBlueprints();
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00017BDA File Offset: 0x00015DDA
	public void RemoveBlueprint(Blueprint blueprint)
	{
		if (blueprint.IsRegistered)
		{
			this._blueprints.Remove(blueprint);
			blueprint.IsRegistered = false;
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00017BF8 File Offset: 0x00015DF8
	public void SortBlueprints()
	{
		if (SaveSystem.IS_LOADING)
		{
			return;
		}
		this._blueprints.Sort((Blueprint blueprint1, Blueprint blueprint2) => blueprint1.RuntimeID.CompareTo(blueprint2.RuntimeID));
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00017C2C File Offset: 0x00015E2C
	public void AddBuilderDrone(BuilderDrone drone)
	{
		if (!drone.IsRegistered)
		{
			this._builderDrones.Add(drone);
			drone.IsRegistered = true;
			return;
		}
		Debug.Log("[DRONE MANAGER] Could not add a builder drone as the key " + drone.RuntimeID.ToString() + " is already in use!");
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00017C78 File Offset: 0x00015E78
	public void RemoveBuilderDrone(BuilderDrone drone)
	{
		if (drone.IsRegistered)
		{
			this._builderDrones.Remove(drone);
			drone.IsRegistered = false;
			return;
		}
		Debug.Log("[DRONE MANAGER] Could not remove a builder drone with key " + drone.RuntimeID.ToString() + " as it was not registered!");
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00017CC4 File Offset: 0x00015EC4
	public void AddCargoDrone(CargoDrone drone)
	{
		if (!drone.IsRegistered)
		{
			this._cargoDrones.Add(drone);
			drone.IsRegistered = true;
			return;
		}
		Debug.Log("[DRONE MANAGER] Could not add a builder drone as the key " + drone.RuntimeID.ToString() + " is already in use!");
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00017D10 File Offset: 0x00015F10
	public void RemoveCargoDrone(CargoDrone drone)
	{
		if (drone.IsRegistered)
		{
			this._cargoDrones.Remove(drone);
			drone.IsRegistered = false;
			return;
		}
		Debug.Log("[DRONE MANAGER] Could not remove a builder drone with key " + drone.RuntimeID.ToString() + " as it was not registered!");
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00017D5C File Offset: 0x00015F5C
	public override void Awake()
	{
		if (base.gameObject.GetComponent<DroneRouteGenerator>() == null)
		{
			base.gameObject.AddComponent<DroneRouteGenerator>();
		}
		base.Awake();
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00017D83 File Offset: 0x00015F83
	public void Update()
	{
		this.UpdateCargoDrones();
		this.UpdateBuilderDrones();
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00017D94 File Offset: 0x00015F94
	private void UpdateCargoDrones()
	{
		try
		{
			if (NetworkPlayerManager.IS_HOST && this._cargoDrones.Count > 0)
			{
				int num = 0;
				int lastCargoDroneIndex = this._lastCargoDroneIndex;
				do
				{
					if (this._lastCargoDroneIndex < 0 || this._lastCargoDroneIndex >= this._cargoDrones.Count)
					{
						Debug.Log(string.Format("[DRONE MANAGER] Invalid index: {0}. Resetting to 0.", this._lastCargoDroneIndex));
						this._lastCargoDroneIndex = 0;
					}
					CargoDrone cargoDrone = this._cargoDrones[this._lastCargoDroneIndex];
					if (cargoDrone != null && !cargoDrone.Entity.Has_EFlag_IsDead)
					{
						cargoDrone.TryGenerateRoute();
					}
					this._lastCargoDroneIndex = (this._lastCargoDroneIndex + 1) % this._cargoDrones.Count;
					num++;
				}
				while (num < 0 && this._lastCargoDroneIndex != lastCargoDroneIndex);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("[DRONE MANAGER] Ran into an error while updating cargo drones: " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00017E94 File Offset: 0x00016094
	private void UpdateBuilderDrones()
	{
		if (this._builderDrones.Count == 0)
		{
			return;
		}
		List<Blueprint> list = new List<Blueprint>();
		for (int i = 0; i < this._blueprints.Count; i++)
		{
			Blueprint blueprint = this._blueprints[i];
			if (blueprint == null || blueprint.Entity.Has_EFlag_IsDead)
			{
				list.Add(blueprint);
			}
			else if (Singleton<ResourceManager>.Instance.CheckEntityCosts(blueprint.Entity.GetData(), true))
			{
				BuilderDrone builderDrone = null;
				float num = float.PositiveInfinity;
				for (int j = 0; j < this._builderDrones.Count; j++)
				{
					BuilderDrone builderDrone2 = this._builderDrones[j];
					if (!(builderDrone2 == null) && !builderDrone2.Entity.Has_EFlag_IsDead)
					{
						float sqrMagnitude = (builderDrone2.transform.position - blueprint.transform.position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							builderDrone = builderDrone2;
						}
					}
				}
				if (builderDrone != null)
				{
					builderDrone.SetTarget(blueprint);
					break;
				}
			}
		}
		foreach (Blueprint blueprint2 in list)
		{
			this._blueprints.Remove(blueprint2);
			if (blueprint2 != null)
			{
				blueprint2.IsRegistered = false;
			}
		}
	}

	// Token: 0x04000262 RID: 610
	private const int MAX_CARGO_DRONE_UPDATES = 0;

	// Token: 0x04000263 RID: 611
	protected List<BuilderDrone> _builderDrones = new List<BuilderDrone>();

	// Token: 0x04000264 RID: 612
	protected List<CargoDrone> _cargoDrones = new List<CargoDrone>();

	// Token: 0x04000265 RID: 613
	protected List<Blueprint> _blueprints = new List<Blueprint>();

	// Token: 0x04000266 RID: 614
	private int _lastCargoDroneIndex;
}
