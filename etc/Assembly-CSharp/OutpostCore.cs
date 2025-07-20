using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x02000120 RID: 288
public class OutpostCore : EntityComponent, IComponent<OutpostCore, CoreData>
{
	// Token: 0x06000986 RID: 2438 RVA: 0x0002839D File Offset: 0x0002659D
	public CoreData GetData()
	{
		return this._coreData;
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x000283A5 File Offset: 0x000265A5
	public void OnInitialize(CoreData data)
	{
		this._coreData = data;
		this._powerModule = base.Entity.Get_EComponent<PowerModule>(true);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x000283C0 File Offset: 0x000265C0
	public override void OnSpawn(bool fromSave)
	{
		if (!this._isListening)
		{
			base.Entity.OnEntityLinked += this.OnEntityLinked;
			base.Entity.OnEntityUnlinked += this.OnEntityUnlinked;
			this._isListening = true;
		}
		base.Entity.IsSaveable = true;
		this._powerModule.OnSpawn(fromSave);
		this._powerModule.SetPowerStatus(true);
		if (base.Entity.Has_EComponent<FOW_Cloak>())
		{
			this._usingMarker = false;
			return;
		}
		if (Singleton<MarkerHandler>.Instance != null)
		{
			Singleton<MarkerHandler>.Instance.CreateMarker(base.Entity.RuntimeID.ToString(), base.transform.position, new Vector2(6f, 6f), new Vector2(1.2f, 1.2f), this._coreData.markerIcon, this._coreData.markerText, "");
			this._usingMarker = true;
			return;
		}
		this._usingMarker = false;
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x000284C8 File Offset: 0x000266C8
	private void OnEntityLinked(Entity entity)
	{
		if (entity.Has_EComponent<Generator>())
		{
			Generator generator = entity.Get_EComponent<Generator>(false);
			generator.OnGeneratorStatusUpdated += this.OnGeneratorStatusUpdated;
			if (generator.IsUpdating && !this._generators.Contains(generator))
			{
				this._generators.Add(generator);
			}
		}
		if (!this._links.Contains(entity))
		{
			this._links.Add(entity);
		}
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00028533 File Offset: 0x00026733
	private void OnEntityUnlinked(Entity entity)
	{
		if (this._links.Contains(entity))
		{
			this._links.Remove(entity);
		}
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00028550 File Offset: 0x00026750
	private void OnGeneratorStatusUpdated(Generator generator, bool status)
	{
		if (status)
		{
			if (!this._generators.Contains(generator))
			{
				this._generators.Add(generator);
			}
		}
		else if (this._generators.Contains(generator))
		{
			this._generators.Remove(generator);
			if (generator.Entity.Has_EFlag_IsDead)
			{
				generator.OnGeneratorStatusUpdated -= this.OnGeneratorStatusUpdated;
			}
		}
		this._powerModule.SetPowerStatus(this._generators.Count > 0);
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x000285D0 File Offset: 0x000267D0
	public override void OnReset()
	{
		if (this._isListening)
		{
			base.Entity.OnEntityLinked -= this.OnEntityLinked;
			this._isListening = false;
		}
		if (this._usingMarker)
		{
			Singleton<MarkerHandler>.Instance.DestroyMarker(base.Entity.RuntimeID.ToString());
			this._usingMarker = false;
		}
		if (!Singleton<EntityManager>.Instance.IsClearingEntities())
		{
			foreach (Entity entity in this._links)
			{
				if (entity != null && !entity.Has_EFlag_IsDead)
				{
					Singleton<EntityUtilities>.Instance.AddDelayedEntity(entity, 0.05f);
				}
			}
		}
		this._generators.Clear();
		this._links.Clear();
		this._decorations.Clear();
	}

	// Token: 0x040005DC RID: 1500
	private CoreData _coreData;

	// Token: 0x040005DD RID: 1501
	protected List<Entity> _links = new List<Entity>();

	// Token: 0x040005DE RID: 1502
	protected List<Generator> _generators = new List<Generator>();

	// Token: 0x040005DF RID: 1503
	protected List<Vector2Int> _decorations = new List<Vector2Int>();

	// Token: 0x040005E0 RID: 1504
	private bool _usingMarker;

	// Token: 0x040005E1 RID: 1505
	private bool _isListening;

	// Token: 0x040005E2 RID: 1506
	private PowerModule _powerModule;
}
