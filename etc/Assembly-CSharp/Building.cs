using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Utilities;

// Token: 0x020000F9 RID: 249
public class Building : EntityComponent, IComponent<Building, BuildingData>
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060007DC RID: 2012 RVA: 0x00022D28 File Offset: 0x00020F28
	// (remove) Token: 0x060007DD RID: 2013 RVA: 0x00022D60 File Offset: 0x00020F60
	public event Building.ClaimedEventHandler OnClaimedEvent;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060007DE RID: 2014 RVA: 0x00022D98 File Offset: 0x00020F98
	// (remove) Token: 0x060007DF RID: 2015 RVA: 0x00022DD0 File Offset: 0x00020FD0
	public event Building.UnclaimedEventHandler OnUnclaimedEvent;

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00022E05 File Offset: 0x00021005
	public List<Vector2Int> Cells
	{
		get
		{
			return this._cells;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00022E0D File Offset: 0x0002100D
	public int Width
	{
		get
		{
			return this._buildingData.Width;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00022E1A File Offset: 0x0002101A
	public int Height
	{
		get
		{
			return this._buildingData.Height;
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00022E28 File Offset: 0x00021028
	public void OnInitialize(BuildingData data)
	{
		this._buildingData = data;
		this._boxCollider2D = base.gameObject.AddComponent<BoxCollider2D>();
		this._boxCollider2D.size = new Vector2((float)data.Width * 5f, (float)data.Height * 5f);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00022E78 File Offset: 0x00021078
	public override void OnSpawn(bool fromSave)
	{
		if (base.Entity.Has_EComponent<HealthComponent>())
		{
			base.Entity.Get_EComponent<HealthComponent>(false).OnDamage += this.BroadcastDamage;
			this._listening = true;
		}
		base.gameObject.isStatic = true;
		base.gameObject.layer = LayerMask.NameToLayer(Layers.BUILDING_LAYER(base.Entity.FactionID, false));
		Utilities.CalculateBuildingCells(ref this._cells, base.transform.position, this.Width, this.Height);
		Singleton<TileGrid>.Instance.SetMultipleCells(this._cells, this, !base.Entity.Has_EComponent<FOW_Cloak>());
		if (!base.Entity.Has_EFlag_IsBlueprint)
		{
			this.UpdateNearbyDetectors();
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00022F3C File Offset: 0x0002113C
	private void UpdateNearbyDetectors()
	{
		Collider2D[] array = new Collider2D[10];
		int num = Physics2D.OverlapPointNonAlloc(base.transform.position, array, LayerMask.GetMask(new string[]
		{
			Layers.TURRET_DETECTOR_LAYER(base.FactionID, true)
		}));
		for (int i = 0; i < num; i++)
		{
			HitDetector component = array[i].GetComponent<HitDetector>();
			if (component != null)
			{
				component.OnTriggerEnter2D(this._boxCollider2D);
			}
		}
		num = Physics2D.OverlapPointNonAlloc(base.transform.position, array, LayerMask.GetMask(new string[]
		{
			Layers.UNIT_DETECTOR_LAYER(base.FactionID, true)
		}));
		for (int j = 0; j < num; j++)
		{
			HitDetector component2 = array[j].GetComponent<HitDetector>();
			if (component2 != null)
			{
				component2.OnTriggerEnter2D(this._boxCollider2D);
			}
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0002300F File Offset: 0x0002120F
	public BuildingData GetData()
	{
		return this._buildingData;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00023018 File Offset: 0x00021218
	private void ApplyAccentToMap(Accent accent)
	{
		if (this._cells != null)
		{
			foreach (Vector2Int vector2Int in this._cells)
			{
				Singleton<MapView>.Instance.SetBuildingTile(new Vector3Int(vector2Int.x, vector2Int.y, 0), accent.primaryColor);
			}
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00023090 File Offset: 0x00021290
	public virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (base.Entity.Has_EFlag_IsDead)
		{
			return;
		}
		if (collision.gameObject.layer == base.gameObject.layer)
		{
			return;
		}
		Unit component = collision.collider.GetComponent<Unit>();
		if (component != null && !component.Entity.Has_EFlag_IsDead)
		{
			component.OnTargetHit(base.Entity);
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x000230F2 File Offset: 0x000212F2
	public void BroadcastDamage(float amount, Entity damager = null)
	{
		if (damager != null)
		{
			Singleton<Events>.Instance.onBuildingDamaged.Invoke(this, amount);
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00023110 File Offset: 0x00021310
	public override void OnReset()
	{
		if (base.Entity.Has_EComponent<HealthComponent>() && this._listening)
		{
			base.Entity.Get_EComponent<HealthComponent>(false).OnDamage -= this.BroadcastDamage;
			this._listening = false;
		}
		if (!Singleton<EntityManager>.Instance.IsClearingEntities() && !base.Entity.Has_EFlag_IsBlueprint && base.Entity.IsOnScreen && this._buildingData.useDeathAnimation)
		{
			ParticleSystem particle;
			AudioClip clip;
			if (this._buildingData.useUniqueAnimations)
			{
				particle = this._buildingData.deathParticle;
				clip = this._buildingData.deathSound;
			}
			else
			{
				particle = LegacyLibrary.BUILDING_DEATH_PARTICLE;
				clip = LegacyLibrary.BUILDING_DEATH_SOUND;
			}
			if (Singleton<Settings>.Instance.UseParticles)
			{
				Singleton<EntityUtilities>.Instance.CreateBuildingExplosion(particle, base.transform.position);
			}
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(clip, base.transform.name, base.transform.position, 1f, true, 0.9f, 1.1f, false);
		}
		base.gameObject.isStatic = false;
		if (base.IsPlayerFaction)
		{
			Singleton<Events>.Instance.onBuildingDestroyed.Invoke(this);
		}
		this.DestroyCells();
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00023250 File Offset: 0x00021450
	public void DestroyCells()
	{
		if (this._cells.Count > 0)
		{
			Singleton<TileGrid>.Instance.DestroyMultipleCells(this._cells);
			this._cells.Clear();
		}
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0002327B File Offset: 0x0002147B
	public virtual void OnClaimed()
	{
		Building.ClaimedEventHandler onClaimedEvent = this.OnClaimedEvent;
		if (onClaimedEvent == null)
		{
			return;
		}
		onClaimedEvent();
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0002328D File Offset: 0x0002148D
	public virtual void OnUnclaimed(Entity damager)
	{
		Building.UnclaimedEventHandler onUnclaimedEvent = this.OnUnclaimedEvent;
		if (onUnclaimedEvent != null)
		{
			onUnclaimedEvent(damager);
		}
		if (!base.Entity.Has_EFlag_IsWorldFeature)
		{
			Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(base.Entity, damager), SyncType.ServerInitiated);
		}
	}

	// Token: 0x04000537 RID: 1335
	private BuildingData _buildingData;

	// Token: 0x04000538 RID: 1336
	private List<Vector2Int> _cells = new List<Vector2Int>();

	// Token: 0x04000539 RID: 1337
	private BoxCollider2D _boxCollider2D;

	// Token: 0x0400053C RID: 1340
	private bool _listening;

	// Token: 0x020000FA RID: 250
	// (Invoke) Token: 0x060007F0 RID: 2032
	public delegate void ClaimedEventHandler();

	// Token: 0x020000FB RID: 251
	// (Invoke) Token: 0x060007F4 RID: 2036
	public delegate void UnclaimedEventHandler(Entity damager);
}
