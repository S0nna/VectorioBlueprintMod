using System;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x020000FD RID: 253
public class Bullet : EntityComponent, IUpdateable, IComponent<Bullet, BulletData>
{
	// Token: 0x060007FA RID: 2042 RVA: 0x00023352 File Offset: 0x00021552
	public BulletData GetData()
	{
		return this._bulletData;
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060007FB RID: 2043 RVA: 0x0002335A File Offset: 0x0002155A
	// (set) Token: 0x060007FC RID: 2044 RVA: 0x00023362 File Offset: 0x00021562
	public Turret Turret
	{
		get
		{
			return this._turret;
		}
		set
		{
			this._turret = value;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060007FD RID: 2045 RVA: 0x0002336B File Offset: 0x0002156B
	// (set) Token: 0x060007FE RID: 2046 RVA: 0x00023373 File Offset: 0x00021573
	public float Lifetime
	{
		get
		{
			return this._lifetime;
		}
		set
		{
			this._lifetime = value;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060007FF RID: 2047 RVA: 0x0002337C File Offset: 0x0002157C
	// (set) Token: 0x06000800 RID: 2048 RVA: 0x00023384 File Offset: 0x00021584
	public int Pierces
	{
		get
		{
			return this._pierces;
		}
		set
		{
			this._pierces = value;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000801 RID: 2049 RVA: 0x0002338D File Offset: 0x0002158D
	public float GetDamage
	{
		get
		{
			return this._damage;
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00023398 File Offset: 0x00021598
	public void OnInitialize(BulletData data)
	{
		this._bulletData = data;
		this._circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		this._circleCollider2D.isTrigger = true;
		this._rigidbody2D = base.gameObject.AddComponent<Rigidbody2D>();
		this._rigidbody2D.gravityScale = 0f;
		this._hitSound = data.sound;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x000233F8 File Offset: 0x000215F8
	public override void OnSpawn(bool fromSave)
	{
		if (this._bulletData.useParticle)
		{
			this._particleInfo.Setup(this._bulletData.particle, this._bulletData.coloringType, Library.GetDefaultMaterial());
		}
		this._rigidbody2D.simulated = true;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00023444 File Offset: 0x00021644
	public virtual void LinkTurretValues(Turret turret, bool fromSave)
	{
		base.gameObject.layer = turret.BulletLayer;
		this._turret = turret;
		this._damage = this._turret.BulletDamage;
		this._speed = this._turret.BulletSpeedWithOffset;
		this._lifetime = this._turret.BulletLifetimeWithOffset;
		this._pierces = this._turret.BulletPierces;
		this._explosive = this._turret.GetData().explosive;
		this._particleInfo.SetColoring(this._turret.Entity.GetModel.GetMaterial(AccentType.PrimaryMaterial));
		base.transform.localScale = new Vector2(this._turret.BulletSize, this._turret.BulletSize);
		this._circleCollider2D.radius = this._turret.BulletSize;
		if (!fromSave)
		{
			base.transform.Rotate(0f, 0f, Random.Range(-this._turret.BulletSpread, this._turret.BulletSpread));
		}
		Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0002356C File Offset: 0x0002176C
	public void Tick(float time)
	{
		this._lifetime -= time;
		if (this._lifetime <= 0f)
		{
			Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(base.Entity, null), SyncType.ServerInitiated);
		}
		base.transform.position += base.transform.up * this._speed * time;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000235E0 File Offset: 0x000217E0
	public virtual void OnTriggerEnter2D(Collider2D collider)
	{
		Entity component = collider.GetComponent<Entity>();
		if (component != null)
		{
			if (component.IsAlly(base.Entity.FactionID) || !component.Has_EComponent<HealthComponent>())
			{
				Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(base.Entity, null), SyncType.ServerInitiated);
				return;
			}
			int pierces = this._pierces;
			this._pierces = pierces - 1;
			bool flag = pierces <= 0;
			Singleton<EntityManager>.Instance.QueueDamageEvent(EventBuilder.BuildDamageEvent(component, this._damage, this._turret.Entity), SyncType.ServerInitiated);
			if (flag && !base.Entity.Has_EFlag_IsDead)
			{
				Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(base.Entity, null), SyncType.ServerInitiated);
				if (base.Entity.IsOnScreen)
				{
					this._particleInfo.SetParticlePosition(component.transform.position);
					this._particleInfo.SetColoring(component.GetModel.GetMaterial(AccentType.PrimaryMaterial));
					Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._hitSound, base.transform.name, base.transform.position, 1f, true, 0.9f, 1.1f, false);
					return;
				}
			}
		}
		else
		{
			BulletDetector component2 = collider.GetComponent<BulletDetector>();
			if (component2 != null)
			{
				component2.OnBulletDetected(this);
			}
		}
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00023734 File Offset: 0x00021934
	public void ReverseBullet()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, base.transform.eulerAngles.z - 180f);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00023768 File Offset: 0x00021968
	public override void OnReset()
	{
		if (this._explosive)
		{
			Singleton<EntityUtilities>.Instance.CreateExplosion(base.Entity, base.transform.position, 15f, this.GetDamage, 1000f, base.Entity.FactionID);
		}
		this._rigidbody2D.simulated = false;
		this.CreateParticle();
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x000237CC File Offset: 0x000219CC
	public virtual void CreateParticle()
	{
		if (!Singleton<Settings>.Instance.UseParticles)
		{
			return;
		}
		if (base.Entity.IsOnScreen && this._particleInfo.UseParticle)
		{
			switch (this._particleInfo.GetColoringMode)
			{
			case ParticleInfo.ColoringMode.None:
				Singleton<EntityUtilities>.Instance.CreateBulletParticle(this._particleInfo.Particle, base.transform.position, base.transform.rotation);
				return;
			case ParticleInfo.ColoringMode.Material:
				Singleton<EntityUtilities>.Instance.CreateBulletParticle(this._particleInfo.Particle, this._particleInfo.Material, base.transform.position, base.transform.rotation);
				return;
			case ParticleInfo.ColoringMode.Color:
				Singleton<EntityUtilities>.Instance.CreateBulletParticle(this._particleInfo.Particle, this._particleInfo.Color, base.transform.position, base.transform.rotation);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0400053F RID: 1343
	private BulletData _bulletData;

	// Token: 0x04000540 RID: 1344
	protected Turret _turret;

	// Token: 0x04000541 RID: 1345
	protected ParticleInfo _particleInfo;

	// Token: 0x04000542 RID: 1346
	protected CircleCollider2D _circleCollider2D;

	// Token: 0x04000543 RID: 1347
	protected Rigidbody2D _rigidbody2D;

	// Token: 0x04000544 RID: 1348
	protected float _damage;

	// Token: 0x04000545 RID: 1349
	protected float _speed;

	// Token: 0x04000546 RID: 1350
	protected float _lifetime;

	// Token: 0x04000547 RID: 1351
	protected int _pierces;

	// Token: 0x04000548 RID: 1352
	protected bool _explosive;

	// Token: 0x04000549 RID: 1353
	protected bool _isSplit;

	// Token: 0x0400054A RID: 1354
	protected AudioClip _hitSound;
}
