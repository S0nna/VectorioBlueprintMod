using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000122 RID: 290
public class Port : EntityComponent, IMouseListener, IComponent<Port, PortData>
{
	// Token: 0x0600098F RID: 2447 RVA: 0x000286ED File Offset: 0x000268ED
	public PortData GetData()
	{
		return this._portData;
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x000286F5 File Offset: 0x000268F5
	public StatFloat DoorOpenTime
	{
		get
		{
			return this._doorOpenTime;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x000286FD File Offset: 0x000268FD
	public Drone GetDrone
	{
		get
		{
			return this._drone;
		}
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x00028705 File Offset: 0x00026905
	public void DisableSounds()
	{
		this._useSounds = false;
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0002870E File Offset: 0x0002690E
	public void OnInitialize(PortData portData)
	{
		this._portData = portData;
		if (portData.useDoorSounds)
		{
			this._openSound = portData.openSound;
			this._closeSound = portData.closeSound;
			this._useSounds = true;
		}
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00028740 File Offset: 0x00026940
	public override void OnSpawn(bool fromSave)
	{
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._doorOpenTime, StatType.DroneDoorSpeed, this._portData.doorTimer, this);
		if (!fromSave)
		{
			BaseData droneData = this._portData.droneData;
			base.Entity.onMetadataApplied += this.OnMetadataApplied;
			this._metadataListener = default(Port.DroneMetadataListener);
			this._isListeningForMetadata = true;
			EntityCreationData creationData = EventBuilder.BuildCreationData(droneData.ID, base.FactionID, base.transform.position, SyncType.ServerInitiated);
			EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.EntityCallback, base.RuntimeID, base.ComponentIndex);
			EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(base.GetAccent));
			Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
			PortDoors doors = this._doors;
			if (doors == null)
			{
				return;
			}
			doors.ResetDoors();
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00028809 File Offset: 0x00026A09
	public override void OnCreationCallback(Entity entity)
	{
		if (!entity.Has_EComponent<Drone>())
		{
			Debug.Log("[PORT] No drone component attached!");
			return;
		}
		this.LinkDrone(entity.Get_EComponent<Drone>(false));
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0002882B File Offset: 0x00026A2B
	public void OnMetadataApplied(EntityMetadata metadata, bool asPipette)
	{
		this._metadataListener.HasMetadataForDrone = true;
		this._metadataListener.IsDroneMetadataPipette = asPipette;
		this._metadataListener.DroneMetadata = metadata;
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00028854 File Offset: 0x00026A54
	public void LinkDrone(Drone drone)
	{
		this._drone = drone;
		List<Transform> animationGroup = base.GetModel.GetAnimationGroup(AnimationGroup.Primary);
		if (animationGroup.Count > 1)
		{
			Transform transform = animationGroup[0];
			Transform transform2 = animationGroup[1];
			if (transform != null && transform2 != null)
			{
				this._doors = transform.gameObject.AddComponent<PortDoors>();
				this._doors.Setup(this, this._drone, transform, transform2);
			}
			else
			{
				this._doors = null;
			}
		}
		this._drone.LinkPort(this, this._doors);
		PortDoors doors = this._doors;
		if (doors != null)
		{
			doors.ResetDoors();
		}
		base.Entity.PipetteBridge = this._drone.Entity;
		if (this._metadataListener.HasMetadataForDrone)
		{
			this._drone.Entity.ApplyMetadata(this._metadataListener.DroneMetadata, this._metadataListener.IsDroneMetadataPipette);
			this._metadataListener = default(Port.DroneMetadataListener);
			if (this._isListeningForMetadata)
			{
				base.Entity.onMetadataApplied -= this.OnMetadataApplied;
				this._isListeningForMetadata = false;
			}
		}
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0002896C File Offset: 0x00026B6C
	public void OnMouseHover(bool toggle)
	{
		if (this._drone != null)
		{
			this._drone.OnMouseHover(toggle);
		}
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00028988 File Offset: 0x00026B88
	public void OnQuickEdit()
	{
		if (this._drone != null)
		{
			this._drone.OnQuickEdit();
		}
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x000289A4 File Offset: 0x00026BA4
	public void PlayOpenSound()
	{
		if (this._useSounds && base.Entity.IsOnScreen)
		{
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._openSound, "sound_doors_open", base.transform.position, 0.2f, true, 0.9f, 1.1f, false);
		}
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x000289FC File Offset: 0x00026BFC
	public void PlayCloseSound()
	{
		if (this._useSounds && base.Entity.IsOnScreen)
		{
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._closeSound, "sound_doors_close", base.transform.position, 0.2f, true, 0.9f, 1.1f, false);
		}
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00028A54 File Offset: 0x00026C54
	public override void OnReset()
	{
		if (this._drone != null)
		{
			Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(this._drone.Entity, null), SyncType.ServerInitiated);
		}
		Object.Destroy(this._doors);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00028A8B File Offset: 0x00026C8B
	public void OnMouseClick()
	{
		throw new NotImplementedException();
	}

	// Token: 0x040005E3 RID: 1507
	private PortData _portData;

	// Token: 0x040005E4 RID: 1508
	private StatFloat _doorOpenTime;

	// Token: 0x040005E5 RID: 1509
	protected PortDoors _doors;

	// Token: 0x040005E6 RID: 1510
	[SerializeField]
	protected Drone _drone;

	// Token: 0x040005E7 RID: 1511
	private bool _isListeningForMetadata;

	// Token: 0x040005E8 RID: 1512
	private Port.DroneMetadataListener _metadataListener;

	// Token: 0x040005E9 RID: 1513
	protected AudioClip _openSound;

	// Token: 0x040005EA RID: 1514
	protected AudioClip _closeSound;

	// Token: 0x040005EB RID: 1515
	protected bool _useSounds;

	// Token: 0x02000123 RID: 291
	private struct DroneMetadataListener
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x00028A92 File Offset: 0x00026C92
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x00028A9A File Offset: 0x00026C9A
		public bool HasMetadataForDrone { readonly get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x00028AA3 File Offset: 0x00026CA3
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x00028AAB File Offset: 0x00026CAB
		public bool IsDroneMetadataPipette { readonly get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x00028AB4 File Offset: 0x00026CB4
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x00028ABC File Offset: 0x00026CBC
		public EntityMetadata DroneMetadata { readonly get; set; }
	}
}
