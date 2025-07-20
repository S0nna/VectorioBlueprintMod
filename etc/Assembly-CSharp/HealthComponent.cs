using System;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000117 RID: 279
public class HealthComponent : EntityComponent, IComponent<HealthComponent, HealthData>
{
	// Token: 0x0600094C RID: 2380 RVA: 0x0002757C File Offset: 0x0002577C
	public HealthData GetData()
	{
		return this._healthData;
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x0600094D RID: 2381 RVA: 0x00027584 File Offset: 0x00025784
	// (remove) Token: 0x0600094E RID: 2382 RVA: 0x000275BC File Offset: 0x000257BC
	public event HealthComponent.DamageEventHandler OnDamage;

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x0002761E File Offset: 0x0002581E
	// (set) Token: 0x0600094F RID: 2383 RVA: 0x000275F1 File Offset: 0x000257F1
	public float Health
	{
		get
		{
			return this._health;
		}
		set
		{
			this._health = value;
			if (this._health > this._maxHealth.Value)
			{
				this._health = this._maxHealth.Value;
			}
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000951 RID: 2385 RVA: 0x00027626 File Offset: 0x00025826
	// (set) Token: 0x06000952 RID: 2386 RVA: 0x0002762E File Offset: 0x0002582E
	public StatFloat MaxHealth
	{
		get
		{
			return this._maxHealth;
		}
		set
		{
			this._maxHealth = value;
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00027637 File Offset: 0x00025837
	public void OnInitialize(HealthData data)
	{
		this._healthData = data;
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x00027640 File Offset: 0x00025840
	public override void OnSpawn(bool fromSave)
	{
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._maxHealth, this._healthData.type, this._healthData.health, this);
		this._health = this._maxHealth.Value;
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0002767C File Offset: 0x0002587C
	public void Damage(float amount, Entity damager = null)
	{
		if (base.Entity.IsOnScreen)
		{
			Singleton<AnimationManager>.Instance.CreateDamageNumber(base.transform.position, amount);
		}
		if (base.Entity.Has_EFlag_IsInvincible)
		{
			HealthComponent.DamageEventHandler onDamage = this.OnDamage;
			if (onDamage == null)
			{
				return;
			}
			onDamage(amount, damager);
			return;
		}
		else
		{
			if ((this._health -= amount) <= 0f)
			{
				Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(base.Entity, damager), SyncType.ServerInitiated);
				return;
			}
			HealthComponent.DamageEventHandler onDamage2 = this.OnDamage;
			if (onDamage2 == null)
			{
				return;
			}
			onDamage2(amount, damager);
			return;
		}
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x00027713 File Offset: 0x00025913
	public override void OnReset()
	{
		this._health = 0f;
	}

	// Token: 0x040005B8 RID: 1464
	private HealthData _healthData;

	// Token: 0x040005BA RID: 1466
	private float _health;

	// Token: 0x040005BB RID: 1467
	private StatFloat _maxHealth;

	// Token: 0x02000118 RID: 280
	// (Invoke) Token: 0x06000959 RID: 2393
	public delegate void DamageEventHandler(float amount, Entity damager);
}
