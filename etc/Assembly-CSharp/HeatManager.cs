using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Entities;
using Vectorio.PhasmaUI;
using Vectorio.Stats;

// Token: 0x020001BE RID: 446
[DefaultExecutionOrder(0)]
public class HeatManager : Singleton<HeatManager>
{
	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0003F6A1 File Offset: 0x0003D8A1
	public static StatFloat SpawnRate
	{
		get
		{
			return HeatManager._spawnRate;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000E31 RID: 3633 RVA: 0x0003F6A8 File Offset: 0x0003D8A8
	public FactionData FactionData
	{
		get
		{
			return this._factionData;
		}
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x0003F6B0 File Offset: 0x0003D8B0
	public void SetFactionData(FactionData data)
	{
		this._factionData = data;
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0003F6B9 File Offset: 0x0003D8B9
	public bool HasActiveEnemySpawns()
	{
		return this._activeEnemySpawns != null && this._activeEnemySpawns.Count > 0;
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0003F6D3 File Offset: 0x0003D8D3
	public List<HeatManager.Enemy> GetActiveEnemySpawns()
	{
		return this._activeEnemySpawns;
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0003F6DB File Offset: 0x0003D8DB
	public bool IsActive
	{
		get
		{
			return this._isActive;
		}
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0003F6E4 File Offset: 0x0003D8E4
	public void Setup(RegionData region)
	{
		this._activeEnemySpawns = new List<HeatManager.Enemy>();
		this._inactiveEnemySpawns = new List<HeatManager.Enemy>();
		Singleton<StatManager>.Instance.CreateStatFloat(ref HeatManager._spawnRate, StatType.UnitSpawnRate, 1f, null);
		this._regionID = region.ID;
		this._factionData = region.enemyFaction;
		if (!Singleton<Gamemode>.Instance.UseHeatSpawning)
		{
			this._isActive = false;
			return;
		}
		if (region.enemies != null && region.enemies.Count > 0)
		{
			this._isActive = true;
			foreach (EnemySpawn spawnData in region.enemies)
			{
				this._inactiveEnemySpawns.Add(new HeatManager.Enemy(spawnData));
			}
			this._inactiveEnemySpawns.Sort((HeatManager.Enemy x, HeatManager.Enemy y) => x.HeatLevel().CompareTo(y.HeatLevel()));
			for (int i = 0; i < this._inactiveEnemySpawns.Count; i++)
			{
				this._inactiveEnemySpawns[i].SetIndex(i);
			}
			this.UpdateHeat(Singleton<ResourceManager>.Instance.GetAmountByData(Singleton<ResourceManager>.Instance.heatData));
			Singleton<Events>.Instance.onHeatAmountUpdated.AddListener(new UnityAction<int>(this.UpdateHeat));
		}
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0003F844 File Offset: 0x0003DA44
	public void UpdateHeat(int heat)
	{
		int num = 0;
		bool flag = false;
		this._heatMinThreshold = int.MinValue;
		this._heatMaxThreshold = int.MaxValue;
		this._allEnemiesActive = true;
		for (int i = 0; i < this._inactiveEnemySpawns.Count; i++)
		{
			HeatManager.Enemy enemy = this._inactiveEnemySpawns[i];
			int num2 = enemy.HeatLevel();
			if (num2 <= heat)
			{
				this._activeEnemySpawns.Add(enemy);
				this._inactiveEnemySpawns.RemoveAt(i);
				enemy.ToggleActivity(true);
				i--;
				num++;
				if (num2 > this._heatMinThreshold)
				{
					this._heatMinThreshold = num2;
				}
				if (!flag)
				{
					flag = true;
				}
			}
			else if (num2 < this._heatMaxThreshold)
			{
				this._heatMaxThreshold = num2;
				this._allEnemiesActive = false;
			}
		}
		for (int j = 0; j < this._activeEnemySpawns.Count - num; j++)
		{
			HeatManager.Enemy enemy2 = this._activeEnemySpawns[j];
			int num2 = enemy2.HeatLevel();
			if (num2 > heat)
			{
				this._inactiveEnemySpawns.Add(enemy2);
				this._activeEnemySpawns.RemoveAt(j);
				enemy2.ToggleActivity(false);
				j--;
				if (num2 < this._heatMaxThreshold)
				{
					this._heatMaxThreshold = num2;
					this._allEnemiesActive = false;
				}
				if (!flag)
				{
					flag = true;
				}
			}
			else
			{
				if (num2 > this._heatMinThreshold)
				{
					this._heatMinThreshold = num2;
				}
				enemy2.UpdateSpawnRate(heat);
			}
		}
		if (flag)
		{
			if (this._allEnemiesActive)
			{
				Singleton<Events>.Instance.onHeatLimitReached.Invoke();
				return;
			}
			Singleton<Events>.Instance.onHeatThresholdUpdated.Invoke(this._heatMinThreshold, this._heatMaxThreshold);
		}
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0003F9CC File Offset: 0x0003DBCC
	public void Update()
	{
		if (!this.IsActive || Singleton<Gamemode>.Instance.IsPaused)
		{
			return;
		}
		if (Singleton<Gamemode>.Instance.UseHeatSpawning)
		{
			foreach (HeatManager.Enemy enemy in this._activeEnemySpawns)
			{
				if (enemy.Update(Time.deltaTime))
				{
					this.SpawnEnemy(enemy.spawnData.unit, this.GetRandomSpawnPosition(), 1, false);
				}
			}
		}
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x0003FA60 File Offset: 0x0003DC60
	public void SpawnEnemy(EntityData unit, Vector2 position, int amount, bool syncSpeed = false)
	{
		for (int i = 0; i < amount; i++)
		{
			Vector2 position2 = (i == 0) ? position : new Vector2(position.x + Random.Range(-5f, 5f), position.y + Random.Range(-5f, 5f));
			EntityCreationData creationData = EventBuilder.BuildCreationData(unit.ID, this.FactionData.ID, position2, SyncType.ServerInitiated);
			EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(this._factionData.accent));
			EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.ManagerCallback, 2U, 0);
			Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
		}
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0003FAFC File Offset: 0x0003DCFC
	public void OnEntityCreated(Entity entity)
	{
		if (entity.Has_EComponent<Unit>())
		{
			Unit unit = entity.Get_EComponent<Unit>(false);
			unit.SetTargetPosition(Singleton<WorldGenerator>.Instance.CenterWorldPos);
			unit.FaceTarget();
		}
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x0003FB24 File Offset: 0x0003DD24
	public Vector2 GetRandomSpawnPosition()
	{
		if (DevTools.ENEMY_TEST)
		{
			switch (Random.Range(0, 4))
			{
			case 0:
				return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.CenterWorldPos.x - DevTools.HEAT_SPAWN_OFFSET, Singleton<WorldGenerator>.Instance.CenterWorldPos.x + DevTools.HEAT_SPAWN_OFFSET), Singleton<WorldGenerator>.Instance.CenterWorldPos.y + DevTools.HEAT_SPAWN_OFFSET);
			case 1:
				return new Vector2(Singleton<WorldGenerator>.Instance.CenterWorldPos.x + DevTools.HEAT_SPAWN_OFFSET, Random.Range(Singleton<WorldGenerator>.Instance.CenterWorldPos.y - DevTools.HEAT_SPAWN_OFFSET, Singleton<WorldGenerator>.Instance.CenterWorldPos.y + DevTools.HEAT_SPAWN_OFFSET));
			case 2:
				return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.CenterWorldPos.x - DevTools.HEAT_SPAWN_OFFSET, Singleton<WorldGenerator>.Instance.CenterWorldPos.x + DevTools.HEAT_SPAWN_OFFSET), Singleton<WorldGenerator>.Instance.CenterWorldPos.y - DevTools.HEAT_SPAWN_OFFSET);
			case 3:
				return new Vector2(Singleton<WorldGenerator>.Instance.CenterWorldPos.x - DevTools.HEAT_SPAWN_OFFSET, Random.Range(Singleton<WorldGenerator>.Instance.CenterWorldPos.y - DevTools.HEAT_SPAWN_OFFSET, Singleton<WorldGenerator>.Instance.CenterWorldPos.y + DevTools.HEAT_SPAWN_OFFSET));
			default:
				return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.CenterWorldPos.x - DevTools.HEAT_SPAWN_OFFSET, Singleton<WorldGenerator>.Instance.CenterWorldPos.x + DevTools.HEAT_SPAWN_OFFSET), Singleton<WorldGenerator>.Instance.CenterWorldPos.y + DevTools.HEAT_SPAWN_OFFSET);
			}
		}
		else
		{
			switch (Random.Range(0, 4))
			{
			case 0:
				return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX), Singleton<WorldGenerator>.Instance.RegionMaxBoundaryY);
			case 1:
				return new Vector2(Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX, Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryY, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryY));
			case 2:
				return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX), Singleton<WorldGenerator>.Instance.RegionMinBoundaryY);
			case 3:
				return new Vector2(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX));
			default:
				return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX), Singleton<WorldGenerator>.Instance.RegionMaxBoundaryY);
			}
		}
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0003FDC0 File Offset: 0x0003DFC0
	public Vector2 GetRandomSpawnPosition(CardinalDirection direction)
	{
		switch (direction)
		{
		case CardinalDirection.North:
			return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX), Singleton<WorldGenerator>.Instance.RegionMaxBoundaryY);
		case CardinalDirection.East:
			return new Vector2(Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX, Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryY, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryY));
		case CardinalDirection.South:
			return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX), Singleton<WorldGenerator>.Instance.RegionMinBoundaryY);
		case CardinalDirection.West:
			return new Vector2(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX));
		default:
			return new Vector2(Random.Range(Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX), Singleton<WorldGenerator>.Instance.RegionMaxBoundaryY);
		}
	}

	// Token: 0x04000B0B RID: 2827
	protected string _regionID;

	// Token: 0x04000B0C RID: 2828
	protected FactionData _factionData;

	// Token: 0x04000B0D RID: 2829
	protected static StatFloat _spawnRate;

	// Token: 0x04000B0E RID: 2830
	protected List<HeatManager.Enemy> _activeEnemySpawns;

	// Token: 0x04000B0F RID: 2831
	protected List<HeatManager.Enemy> _inactiveEnemySpawns;

	// Token: 0x04000B10 RID: 2832
	protected bool _isActive;

	// Token: 0x04000B11 RID: 2833
	private int _heatMinThreshold;

	// Token: 0x04000B12 RID: 2834
	private int _heatMaxThreshold;

	// Token: 0x04000B13 RID: 2835
	private bool _allEnemiesActive;

	// Token: 0x020001BF RID: 447
	public class Enemy
	{
		// Token: 0x06000E3E RID: 3646 RVA: 0x0003FEBC File Offset: 0x0003E0BC
		public Enemy(EnemySpawn spawnData)
		{
			this.spawnData = spawnData;
			this.cooldown = spawnData.baseCooldown;
			this.timer = this.cooldown;
			this._heatSpawnUI = UI_Singleton<HeatManagerUI>.Instance.CreateHeatSpawnUI(this);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0003FEFC File Offset: 0x0003E0FC
		public bool Update(float time)
		{
			this.timer -= time;
			if (UI_Singleton<HeatManagerUI>.Instance.IsOpen)
			{
				this._heatSpawnUI.CustomUpdate();
			}
			if (this.timer <= 0f)
			{
				this.timer = this.cooldown;
				return true;
			}
			return false;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0003FF4C File Offset: 0x0003E14C
		public void UpdateSpawnRate(int heat)
		{
			int num = (heat - this.spawnData.heatLevel) / this.spawnData.reductionInterval;
			if (this._lastIntervalCalculation != num)
			{
				float num2 = (float)num * this.spawnData.reductionMultiplier;
				this.cooldown = Mathf.Max(this.spawnData.baseCooldown - num2, this.spawnData.maxCooldown);
				this.cooldown *= HeatManager._spawnRate.Value;
				this._lastIntervalCalculation = num;
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0003FFCC File Offset: 0x0003E1CC
		public void ToggleActivity(bool toggle)
		{
			this._isActive = toggle;
			this._heatSpawnUI.Toggle(toggle);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0003FFE1 File Offset: 0x0003E1E1
		public bool IsActive()
		{
			return this._isActive;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0003FFE9 File Offset: 0x0003E1E9
		public int HeatLevel()
		{
			return this.spawnData.heatLevel;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0003FFF6 File Offset: 0x0003E1F6
		public void SetIndex(int index)
		{
			this._heatSpawnUI.transform.SetSiblingIndex(index);
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x00040009 File Offset: 0x0003E209
		public EntityData UnitData
		{
			get
			{
				return this.spawnData.unit;
			}
		}

		// Token: 0x04000B14 RID: 2836
		public EnemySpawn spawnData;

		// Token: 0x04000B15 RID: 2837
		public float cooldown;

		// Token: 0x04000B16 RID: 2838
		public float timer;

		// Token: 0x04000B17 RID: 2839
		private bool _isActive;

		// Token: 0x04000B18 RID: 2840
		private int _lastIntervalCalculation = -1;

		// Token: 0x04000B19 RID: 2841
		private HeatSpawnUI _heatSpawnUI;
	}
}
