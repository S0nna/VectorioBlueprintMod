using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x02000130 RID: 304
public class Scythe : EntityComponent, IComponent<Scythe, ScytheData>, IUpdateable
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x00029BF8 File Offset: 0x00027DF8
	public int Damage
	{
		get
		{
			return this._damage.Value;
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00029C05 File Offset: 0x00027E05
	public float TravelTime
	{
		get
		{
			return this._travelTime.Value;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x00029C12 File Offset: 0x00027E12
	public float Cooldown
	{
		get
		{
			return this._cooldown.Value;
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00029C1F File Offset: 0x00027E1F
	public ScytheData GetData()
	{
		return this._scytheData;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00029C27 File Offset: 0x00027E27
	public void OnInitialize(ScytheData data)
	{
		this._scytheData = data;
		this._entityDetector = base.Entity.Get_EComponent<EntityDetector>(true);
		this._powerModule = base.Entity.Get_EComponent<PowerModule>(true);
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00029C54 File Offset: 0x00027E54
	public override void OnSpawn(bool fromSave)
	{
		if (this._scytheProjectile == null)
		{
			this._scytheProjectile = new GameObject("Scythe Projectile").AddComponent<ScytheProjectile>();
			this._scytheProjectile.transform.position = base.transform.position;
			using (List<Transform>.Enumerator enumerator = base.Entity.GetModel.GetAnimationGroup(AnimationGroup.Primary).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = enumerator.Current;
					transform.SetParent(this._scytheProjectile.transform);
					transform.transform.localPosition = Vector2.zero;
					transform.transform.localRotation = Quaternion.identity;
				}
				goto IL_DB;
			}
		}
		this._scytheProjectile.transform.SetParent(null);
		this._scytheProjectile.transform.position = base.transform.position;
		IL_DB:
		this._powerModule.OnSpawn(fromSave);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._damage, StatType.TurretDamage, this._scytheData.damage, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._range, StatType.TurretRange, this._scytheData.range, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._travelTime, StatType.ProjectileSpeed, this._scytheData.timeToCrest, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._cooldown, StatType.TurretCooldown, this._scytheData.cooldown, this);
		this._modifiedRange = (float)this._range.Value * 5f + 2.5f;
		this._fastRotationSpeed = this._scytheData.rotationSpeed * 3f;
		this._entityDetector.Setup(ref this._range, Layers.TURRET_DETECTOR_LAYER(base.FactionID, false), true);
		this._entityDetector.OnHitDetectorCheckFinished += this.OnHitDetectorCheckFinished;
		this._scytheProjectile.Setup(this, LayerMask.NameToLayer(Layers.BULLET_LAYER(base.Entity.FactionID, false)));
		Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00029E68 File Offset: 0x00028068
	public void Tick(float time)
	{
		PowerModule powerModule = this._powerModule;
		if (powerModule != null && !powerModule.IsPowered)
		{
			return;
		}
		if (base.Entity.IsOnScreen)
		{
			if (this._projectileActive)
			{
				this._scytheProjectile.transform.Rotate(Vector3.forward, this._fastRotationSpeed * time);
			}
			else
			{
				this._scytheProjectile.transform.Rotate(Vector3.forward, this._scytheData.rotationSpeed * time);
			}
		}
		if (this._projectileActive)
		{
			this._scytheProjectile.Tick(time);
			return;
		}
		if (!this._onCooldown)
		{
			return;
		}
		if (this._target != null && !this.CheckTarget(this._target))
		{
			this._target = null;
		}
		this._timer -= time;
		if (this._timer <= 0f)
		{
			this._onCooldown = false;
			this._timer = this._cooldown.Value;
			this.OnCooldownFinished();
			return;
		}
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00029F5E File Offset: 0x0002815E
	private bool CheckTarget(Entity target)
	{
		return target != null && !target.Has_EFlag_IsDead;
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x00029F74 File Offset: 0x00028174
	private void OnCooldownFinished()
	{
		if (this._target != null && this.CheckTarget(this._target))
		{
			this.LaunchProjectileAtPosition(this._target.transform.position);
			return;
		}
		this._target = null;
		this._entityDetector.Check();
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x00029FCB File Offset: 0x000281CB
	public void OnHitDetectorCheckFinished(Entity entity)
	{
		if (entity == null)
		{
			return;
		}
		if (!this._projectileActive)
		{
			this._target = entity;
			this.LaunchProjectileAtPosition(this._target.transform.position);
		}
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0002A004 File Offset: 0x00028204
	public void LaunchProjectileAtPosition(Vector2 enemyPosition)
	{
		Vector2 normalized = (enemyPosition - base.transform.position).normalized;
		Vector2 position = base.transform.position + normalized * this._modifiedRange;
		this._projectileActive = true;
		this._scytheProjectile.Launch(position);
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0002A065 File Offset: 0x00028265
	public void OnScytheReturned()
	{
		this._projectileActive = false;
		this._onCooldown = true;
		this._timer = this.Cooldown;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0002A081 File Offset: 0x00028281
	public override void OnReset()
	{
		this._entityDetector.OnHitDetectorCheckFinished -= this.OnHitDetectorCheckFinished;
		this._scytheProjectile.transform.SetParent(base.transform);
	}

	// Token: 0x04000620 RID: 1568
	private ScytheData _scytheData;

	// Token: 0x04000621 RID: 1569
	private ScytheProjectile _scytheProjectile;

	// Token: 0x04000622 RID: 1570
	private EntityDetector _entityDetector;

	// Token: 0x04000623 RID: 1571
	private bool _projectileActive;

	// Token: 0x04000624 RID: 1572
	private StatInt _damage;

	// Token: 0x04000625 RID: 1573
	private StatInt _range;

	// Token: 0x04000626 RID: 1574
	private StatFloat _travelTime;

	// Token: 0x04000627 RID: 1575
	private StatFloat _cooldown;

	// Token: 0x04000628 RID: 1576
	protected float _modifiedRange;

	// Token: 0x04000629 RID: 1577
	private float _fastRotationSpeed;

	// Token: 0x0400062A RID: 1578
	private bool _onCooldown;

	// Token: 0x0400062B RID: 1579
	private float _timer;

	// Token: 0x0400062C RID: 1580
	private PowerModule _powerModule;

	// Token: 0x0400062D RID: 1581
	[SerializeField]
	protected Entity _target;
}
