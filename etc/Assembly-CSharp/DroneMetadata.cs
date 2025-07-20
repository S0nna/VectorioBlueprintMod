using System;
using UnityEngine;

// Token: 0x020000E7 RID: 231
[MetadataFor(typeof(Drone))]
[Serializable]
public class DroneMetadata : ComponentMetadata<Drone>
{
	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x000214A4 File Offset: 0x0001F6A4
	// (set) Token: 0x0600074D RID: 1869 RVA: 0x000214AC File Offset: 0x0001F6AC
	public E_ID ParentID { get; set; }

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x0600074E RID: 1870 RVA: 0x000214B5 File Offset: 0x0001F6B5
	// (set) Token: 0x0600074F RID: 1871 RVA: 0x000214BD File Offset: 0x0001F6BD
	public byte Status { get; set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x000214C6 File Offset: 0x0001F6C6
	// (set) Token: 0x06000751 RID: 1873 RVA: 0x000214CE File Offset: 0x0001F6CE
	public float DoorProgress { get; set; }

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000752 RID: 1874 RVA: 0x000214D7 File Offset: 0x0001F6D7
	// (set) Token: 0x06000753 RID: 1875 RVA: 0x000214DF File Offset: 0x0001F6DF
	public bool HasTarget { get; set; }

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000754 RID: 1876 RVA: 0x000214E8 File Offset: 0x0001F6E8
	// (set) Token: 0x06000755 RID: 1877 RVA: 0x000214F0 File Offset: 0x0001F6F0
	public E_ID TargetID { get; set; }

	// Token: 0x06000756 RID: 1878 RVA: 0x000214FC File Offset: 0x0001F6FC
	public override void GetValues(Drone component, MetadataContext context)
	{
		if (component.Parent != null)
		{
			this.ParentID = new E_ID(component.Parent.RuntimeID, context);
		}
		this.Status = (byte)component.DroneStatus;
		if (component.DroneStatus != Drone.Status.Inactive)
		{
			if (component.DroneStatus == Drone.Status.EnteringPort || component.DroneStatus == Drone.Status.ExitingPort)
			{
				this.DoorProgress = component.PortDoors.ElapsedTime;
			}
			else
			{
				this.DoorProgress = 0f;
			}
			if (component.Target != null)
			{
				this.HasTarget = true;
				this.TargetID = new E_ID(component.Target.RuntimeID, context);
			}
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x000215A0 File Offset: 0x0001F7A0
	public override void SetValues(Drone component, bool asPipette, MetadataContext context)
	{
		if (!asPipette)
		{
			Entity entity;
			if (this.ParentID != null && this.ParentID.TryGetEntity(out entity))
			{
				Port port = entity.Get_EComponent<Port>(false);
				if (port != null)
				{
					port.LinkDrone(component);
				}
				else
				{
					Debug.Log("[DRONE] Entity " + entity.GetData().Name + " does not have a port component!");
				}
			}
			else
			{
				Debug.Log("[DRONE] No parent ID was provided for drone " + component.Entity.GetData().Name);
			}
			Drone.Status status = (Drone.Status)this.Status;
			if (status == Drone.Status.Inactive)
			{
				return;
			}
			switch (status)
			{
			case Drone.Status.ExitingPort:
			{
				component.transform.position = component.GetParentTransform().position;
				component.PortDoors.ElapsedTime = this.DoorProgress;
				component.DroneStatus = Drone.Status.ExitingPort;
				Entity metadataTarget;
				if (this.HasTarget && this.TargetID.TryGetEntity(out metadataTarget))
				{
					component.SetMetadataTarget(metadataTarget);
				}
				break;
			}
			case Drone.Status.TravellingToTarget:
			{
				Entity metadataTarget;
				if (!this.HasTarget)
				{
					if (component.Parent != null)
					{
						component.ReturnToParent();
						return;
					}
					component.DroneStatus = Drone.Status.Inactive;
					return;
				}
				else if (this.TargetID.TryGetEntity(out metadataTarget))
				{
					component.SetMetadataTarget(metadataTarget);
					component.DroneStatus = Drone.Status.TravellingToTarget;
				}
				else
				{
					Debug.Log("[DRONE] Was travelling to target but that target is no longer available.");
					component.ReturnToParent();
				}
				break;
			}
			case Drone.Status.ReturningToPort:
				if (component.Parent != null)
				{
					component.ReturnToParent();
				}
				else
				{
					Debug.Log("[DRONE] Was returning to parent, but the parent is now null!");
				}
				break;
			case Drone.Status.EnteringPort:
				component.transform.position = component.GetParentTransform().position;
				component.PortDoors.ElapsedTime = this.DoorProgress;
				component.DroneStatus = Drone.Status.EnteringPort;
				break;
			}
			if (!component.IsUpdating)
			{
				Singleton<EntityManager>.Instance.RegisterUpdatingComponent(component);
			}
		}
	}
}
