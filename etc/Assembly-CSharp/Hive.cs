using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x02000119 RID: 281
public class Hive : EntityComponent, IComponent<Hive, HiveData>
{
	// Token: 0x0600095C RID: 2396 RVA: 0x00027720 File Offset: 0x00025920
	public HiveData GetData()
	{
		return this._hiveData;
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00027728 File Offset: 0x00025928
	public void OnInitialize(HiveData data)
	{
		this._hiveData = data;
		this._spawnRange = data.spawnRange;
		this._damageThreshold = (float)data.damageThreshold;
		this._circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		this._circleCollider2D.radius = data.colliderSize;
		this._rigidbody2D = base.gameObject.AddComponent<Rigidbody2D>();
		this._rigidbody2D.gravityScale = 0f;
		base.gameObject.layer = LayerMask.NameToLayer(Layers.UNIT_LAYER(base.Entity.FactionID, false));
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x000277BC File Offset: 0x000259BC
	public override void OnSpawn(bool fromSave)
	{
		if (this._hiveData.isCoreCell)
		{
			foreach (HiveData.HiveCell hiveCell in this._hiveData.cells)
			{
				Vector2 position = new Vector2(base.transform.position.x + hiveCell.position.x, base.transform.position.y + hiveCell.position.y);
				EntityCreationData creationData = EventBuilder.BuildCreationData(hiveCell.hiveData.ID, base.FactionID, position, SyncType.ServerInitiated);
				Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
			}
			Singleton<MarkerHandler>.Instance.CreateMarker(base.Entity.RuntimeID.ToString(), base.transform.position, new Vector2(6f, 6f), new Vector2(0.4f, 0.4f), this._hiveData.markerIcon, "ENEMY HIVE", "");
		}
		if (base.Entity.Has_EComponent<HealthComponent>())
		{
			this._damageReceiver = base.Entity.Get_EComponent<HealthComponent>(false);
			this._damageReceiver.OnDamage += this.OnDamage;
		}
		else
		{
			Debug.Log("[HIVE] No health component attached to this hive!");
		}
		base.OnSpawn(fromSave);
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0002792C File Offset: 0x00025B2C
	public void OnDamage(float amount, Entity damager = null)
	{
		if (!NetworkPlayerManager.IS_HOST)
		{
			return;
		}
		this._damageTracked += amount;
		int num = 0;
		while (this._damageTracked > 0f && num < this._maxSpawnsPerCall)
		{
			this._damageTracked -= this._damageThreshold;
			if (!Singleton<HeatManager>.Instance.HasActiveEnemySpawns())
			{
				if (this._hiveData.spawns.Count <= 0)
				{
					Debug.Log("[HIVE] No valid spawns in heat manager or hive spawn list!");
					return;
				}
				this.SpawnEnemy(this._hiveData.spawns[Random.Range(0, this._hiveData.spawns.Count)].ID);
				num++;
			}
			List<HeatManager.Enemy> activeEnemySpawns = Singleton<HeatManager>.Instance.GetActiveEnemySpawns();
			this.SpawnEnemy(activeEnemySpawns[Random.Range(0, activeEnemySpawns.Count)].UnitData.ID);
			num++;
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00027A14 File Offset: 0x00025C14
	private void SpawnEnemy(string id)
	{
		Vector2 position = new Vector2(base.transform.position.x + Random.Range(-this._spawnRange, this._spawnRange), base.transform.position.y + Random.Range(-this._spawnRange, this._spawnRange));
		FactionData factionData = Singleton<HeatManager>.Instance.FactionData;
		EntityCreationData creationData = EventBuilder.BuildCreationData(id, factionData.ID, position, SyncType.ServerInitiated);
		EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(factionData.accent));
		EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.EntityCallback, base.RuntimeID, base.ComponentIndex);
		Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00027AB9 File Offset: 0x00025CB9
	public override void OnCreationCallback(Entity entity)
	{
		if (entity.Has_EComponent<Unit>())
		{
			Unit unit = entity.Get_EComponent<Unit>(false);
			unit.SetBehaviour(Unit.Behaviour.Normal);
			unit.SetTargetPosition(Singleton<WorldGenerator>.Instance.CenterWorldPos);
			unit.FaceTarget();
		}
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00027AE8 File Offset: 0x00025CE8
	public override void OnReset()
	{
		if (this._hiveData.isCoreCell)
		{
			Singleton<MarkerHandler>.Instance.DestroyMarker(base.RuntimeID.ToString());
		}
		this._damageReceiver.OnDamage -= this.OnDamage;
	}

	// Token: 0x040005BC RID: 1468
	protected HiveData _hiveData;

	// Token: 0x040005BD RID: 1469
	protected bool _spawnEnemiesOnDeath = true;

	// Token: 0x040005BE RID: 1470
	protected float _damageThreshold = 10f;

	// Token: 0x040005BF RID: 1471
	protected float _damageTracked;

	// Token: 0x040005C0 RID: 1472
	protected float _spawnRange;

	// Token: 0x040005C1 RID: 1473
	protected int _maxSpawnsPerCall = 5;

	// Token: 0x040005C2 RID: 1474
	protected CircleCollider2D _circleCollider2D;

	// Token: 0x040005C3 RID: 1475
	protected Rigidbody2D _rigidbody2D;

	// Token: 0x040005C4 RID: 1476
	protected HealthComponent _damageReceiver;
}
