using System;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x020000F6 RID: 246
public class Blueprint : BuildingComponent
{
	// Token: 0x060007BD RID: 1981 RVA: 0x00022762 File Offset: 0x00020962
	public EntityCreationData GetCreationData()
	{
		return this._creationData;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0002276A File Offset: 0x0002096A
	public void SetPipetteData(EntityMetadata pipetteData)
	{
		this._creationData.PipetteData = pipetteData;
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060007BF RID: 1983 RVA: 0x00022778 File Offset: 0x00020978
	// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00022780 File Offset: 0x00020980
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

	// Token: 0x060007C1 RID: 1985 RVA: 0x00022789 File Offset: 0x00020989
	public void SetBuilder(BuilderDrone drone)
	{
		this._builderDrone = drone;
		Singleton<ResourceManager>.Instance.ApplyPendingCosts(base.Entity);
		Singleton<DroneManager>.Instance.RemoveBlueprint(this);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000227AD File Offset: 0x000209AD
	public void ClearBuilder()
	{
		this._builderDrone = null;
		Singleton<ResourceManager>.Instance.RevertPendingCosts(base.Entity);
		Singleton<DroneManager>.Instance.AddBlueprint(this);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000227D4 File Offset: 0x000209D4
	public override void OnSpawn(bool fromSave)
	{
		if (!fromSave)
		{
			Singleton<AudioPlayer>.Instance.PlayBlueprintCreateSound(base.transform.position);
		}
		base.Entity.Set_EFlag_IsBlueprint(true);
		base.Building.OnClaimedEvent += this.OnClaimed;
		base.Building.OnUnclaimedEvent += this.OnUnclaimed;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00022838 File Offset: 0x00020A38
	public void Construct()
	{
		this.ClearBuilder();
		base.Building.DestroyCells();
		EntityDestroyEvent data = EventBuilder.BuildDestroyEvent(base.Entity, null);
		EventBuilder.ToggleRecycleFlagOnDestroyEvent(ref data, false);
		if (this._creationData.FromSave)
		{
			this._creationData.FromSave = false;
		}
		Singleton<EntityManager>.Instance.QueueDestroyEvent(data, SyncType.ServerInitiated);
		Singleton<EntityManager>.Instance.QueueCreationEvent(this._creationData);
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000228A0 File Offset: 0x00020AA0
	public void SetCreationData(EntityCreationData creationData)
	{
		this._creationData = new EntityCreationData(creationData)
		{
			SyncType = SyncType.ServerInitiated,
			FromSave = false
		};
		if (creationData.HasPipette)
		{
			this.SetPipetteData(creationData.PipetteData);
		}
		base.Entity.Set_EFlag_IsEditable(true);
		this._entityData = Library.RequestData<EntityData>(creationData.EntityID);
		if (!this._creationData.IsCreationFlagSet(CreationFlags.UseCosts))
		{
			EventBuilder.ApplyCostsToCreationData(ref this._creationData, false);
		}
		this._isReclaimer = this._entityData.HasComponent<ClaimerData>();
		base.gameObject.layer = LayerMask.NameToLayer(Layers.HOLOGRAM_LAYER);
		if (this._isReclaimer)
		{
			this._range = this._entityData.GetComponent<ClaimerData>().range;
			this._rangePreview = Object.Instantiate<GameObject>(LegacyLibrary.RECLAIMER_PREVIEW_PREFAB).GetComponent<SpriteRenderer>();
			this._rangePreview.transform.position = base.transform.position;
			this._rangePreview.transform.localScale = new Vector3((float)this._range * 2f + 1f, (float)this._range * 2f + 1f);
		}
		this.UpdateValidity();
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000229CA File Offset: 0x00020BCA
	private bool IsValid()
	{
		if (this._isReclaimer)
		{
			return Singleton<TileGrid>.Instance.IsClaimerValid(base.transform.position, this._range);
		}
		return Singleton<TileGrid>.Instance.AreCellsClaimed(base.Building.Cells);
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00022A0A File Offset: 0x00020C0A
	private void UpdateValidity()
	{
		if (this.IsValid())
		{
			Singleton<DroneManager>.Instance.AddBlueprint(this);
			return;
		}
		Singleton<DroneManager>.Instance.RemoveBlueprint(this);
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00022A2B File Offset: 0x00020C2B
	public void OnClaimed()
	{
		this.UpdateValidity();
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00022A2B File Offset: 0x00020C2B
	public void OnUnclaimed(Entity damager = null)
	{
		this.UpdateValidity();
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00022A34 File Offset: 0x00020C34
	public override void OnReset()
	{
		base.Building.OnClaimedEvent -= this.OnClaimed;
		base.Building.OnUnclaimedEvent -= this.OnUnclaimed;
		if (this._rangePreview != null)
		{
			Object.Destroy(this._rangePreview.gameObject);
		}
		if (this._builderDrone != null)
		{
			this._builderDrone.OnTargetDestroyed();
			this.ClearBuilder();
		}
		Singleton<DroneManager>.Instance.RemoveBlueprint(this);
		Singleton<AudioPlayer>.Instance.PlayBlueprintRemoveSound(base.transform.position);
	}

	// Token: 0x0400052A RID: 1322
	protected EntityData _entityData;

	// Token: 0x0400052B RID: 1323
	protected EntityCreationData _creationData;

	// Token: 0x0400052C RID: 1324
	protected SpriteRenderer _rangePreview;

	// Token: 0x0400052D RID: 1325
	protected BuilderDrone _builderDrone;

	// Token: 0x0400052E RID: 1326
	protected bool _isReclaimer;

	// Token: 0x0400052F RID: 1327
	protected int _range;

	// Token: 0x04000530 RID: 1328
	protected bool _isRegistered;
}
