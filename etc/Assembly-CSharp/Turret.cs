using System;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000137 RID: 311
public class Turret : EntityComponent, IUpdateable, IAnimateable, IComponent<Turret, TurretData>
{
	// Token: 0x06000A3D RID: 2621 RVA: 0x0002ACB8 File Offset: 0x00028EB8
	public TurretData GetData()
	{
		return this._turretData;
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002ACC0 File Offset: 0x00028EC0
	public Entity Target
	{
		get
		{
			return this._target;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0002ACC8 File Offset: 0x00028EC8
	// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0002ACD0 File Offset: 0x00028ED0
	public bool IsAnimating
	{
		get
		{
			return this._isAnimating;
		}
		set
		{
			this._isAnimating = value;
		}
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x0002ACD9 File Offset: 0x00028ED9
	public void SetTargetMode(int mode)
	{
		this._entityDetector.SetTargetMode(mode);
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x0002ACE7 File Offset: 0x00028EE7
	public int GetTargetMode()
	{
		return this._entityDetector.GetTargetMode;
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0002ACF4 File Offset: 0x00028EF4
	// (set) Token: 0x06000A44 RID: 2628 RVA: 0x0002ACFC File Offset: 0x00028EFC
	public Rotator RotatorPivot
	{
		get
		{
			return this._rotatorPivot;
		}
		set
		{
			this._rotatorPivot = value;
		}
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x0002AD05 File Offset: 0x00028F05
	public float GetBarrelRotation()
	{
		return this._rotatorPivot.transform.eulerAngles.z;
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0002AD1C File Offset: 0x00028F1C
	// (set) Token: 0x06000A47 RID: 2631 RVA: 0x0002AD24 File Offset: 0x00028F24
	public float Timer
	{
		get
		{
			return this._timer;
		}
		set
		{
			this._timer = value;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0002AD2D File Offset: 0x00028F2D
	public float BulletDamage
	{
		get
		{
			return this._damage.Value;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0002AD3A File Offset: 0x00028F3A
	public float BulletSpeed
	{
		get
		{
			return this._bulletSpeed.Value;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0002AD47 File Offset: 0x00028F47
	public float BulletSpeedWithOffset
	{
		get
		{
			return this._bulletSpeed.Value + Random.Range(-this._bulletSpeedOffset, this._bulletSpeedOffset);
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0002AD67 File Offset: 0x00028F67
	public float BulletLifetime
	{
		get
		{
			return this._bulletLifetime.Value;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0002AD74 File Offset: 0x00028F74
	public float BulletLifetimeWithOffset
	{
		get
		{
			return this._bulletLifetime.Value + Random.Range(-this._bulletLifetimeOffset, this._bulletLifetimeOffset);
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0002AD94 File Offset: 0x00028F94
	public float BulletSpread
	{
		get
		{
			return this._bulletSpread.Value;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0002ADA1 File Offset: 0x00028FA1
	public int BulletPierces
	{
		get
		{
			return this._bulletPierces.Value;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0002ADAE File Offset: 0x00028FAE
	public float BulletSize
	{
		get
		{
			return this._bulletSize.Value;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0002ADBB File Offset: 0x00028FBB
	public LayerMask BulletLayer
	{
		get
		{
			return this._bulletLayer;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0002ADC3 File Offset: 0x00028FC3
	public float Range
	{
		get
		{
			return (float)this._range.Value;
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0002ADD4 File Offset: 0x00028FD4
	public void OnInitialize(TurretData data)
	{
		this._turretData = data;
		this._bulletData = data.bullet;
		this._rotatorPivot = new GameObject("Rotator").AddComponent<Rotator>();
		this._rotatorPivot.transform.SetParent(base.transform);
		this._rotatorPivot.ToggleAlignmentTracking(true);
		this._rotatorPivot.transform.localPosition = Vector2.zero;
		this._barrelPivot = new GameObject("Barrel").GetComponent<Transform>();
		this._barrelPivot.SetParent(this._rotatorPivot.transform);
		this._barrelPivot.transform.localPosition = Vector2.zero;
		this._bulletSpawnPoint = new GameObject("Bullet Spawn Point").GetComponent<Transform>();
		this._bulletSpawnPoint.SetParent(this._barrelPivot);
		this._bulletSpawnPoint.transform.localPosition = new Vector2(this._turretData.xFirePosition, this._turretData.yFirePosition);
		this._bulletSpawnPoint.transform.localRotation = Quaternion.identity;
		this._allowPredictiveAiming = data.allowPredictiveAiming;
		this._bulletSpeedOffset = data.bulletSpeedOffset;
		this._bulletLifetimeOffset = data.bulletLifetimeOffset;
		this._recoilAmplitude = data.recoilAmplitude;
		this._recoilFrequency = data.recoilFrequency;
		this._entityDetector = base.Entity.Get_EComponent<EntityDetector>(true);
		this._powerModule = base.Entity.Get_EComponent<PowerModule>(true);
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0002AF58 File Offset: 0x00029158
	public override void OnSpawn(bool fromSave)
	{
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._damage, StatType.TurretDamage, this._turretData.damage, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._cooldown, StatType.TurretCooldown, this._turretData.cooldown, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._range, StatType.TurretRange, (int)this._turretData.range, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._bulletSpread, StatType.TurretSpread, this._turretData.bulletSpread, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._bulletAmount, StatType.ProjectileAmount, this._turretData.bulletAmount, this);
		Singleton<StatManager>.Instance.CreateStatInt(ref this._bulletPierces, StatType.ProjectilePierces, this._turretData.bulletPierces, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._bulletSpeed, StatType.ProjectileSpeed, this._turretData.bulletSpeed, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._bulletLifetime, StatType.ProjectileLifetime, this._turretData.bulletLifetime, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._bulletSize, StatType.ProjectileSize, this._turretData.bulletSize, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._barrelSpeed, StatType.BarrelSpeed, this._turretData.rotationSpeed, this);
		this._rotatorPivot.SetSpeed(this._barrelSpeed.Value);
		this._modifiedRange = (float)this._range.Value * 5f + 2.5f;
		foreach (Transform transform in base.Entity.GetModel.GetAnimationGroup(AnimationGroup.Primary))
		{
			Vector2 v = transform.localPosition;
			transform.SetParent(this._barrelPivot.transform);
			transform.transform.localPosition = v;
			transform.transform.localRotation = Quaternion.identity;
		}
		this._timer = this._cooldown.Value;
		this._bulletLayer = LayerMask.NameToLayer(Layers.BULLET_LAYER(base.Entity.FactionID, false));
		this._powerModule.OnSpawn(fromSave);
		if (base.Entity.Has_EComponent<AmmoBin>())
		{
			this._requiresAmmo = true;
			this._ammoBin = base.Entity.Get_EComponent<AmmoBin>(false);
		}
		if (!this._isListening)
		{
			this._entityDetector.OnHitDetectorCheckFinished += this.OnHitDetectorCheckFinished;
			this._entityDetector.Setup(ref this._range, Layers.TURRET_DETECTOR_LAYER(base.FactionID, false), true);
			this._isListening = true;
			return;
		}
		Debug.Log("[TURRET] Multiple attempts to create a listener for the same detector!");
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00029F5E File Offset: 0x0002815E
	private bool CheckTarget(Entity target)
	{
		return target != null && !target.Has_EFlag_IsDead;
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x0002B1FC File Offset: 0x000293FC
	public void Tick(float time)
	{
		PowerModule powerModule = this._powerModule;
		if (powerModule != null && !powerModule.IsPowered)
		{
			return;
		}
		if (!this._entityDetector.WaitingForQuery)
		{
			if (this._timer > 0f)
			{
				this._timer -= time;
			}
			if (this.CheckTarget(this._target))
			{
				if (this._distanceCheckCooldown <= 0)
				{
					this._distanceCheckCooldown = 10;
					if (Vector2.Distance(base.transform.position, this._target.transform.position) > this._modifiedRange)
					{
						this._target = null;
						this._entityDetector.Check();
						return;
					}
				}
				else
				{
					this._distanceCheckCooldown--;
				}
				if (this._isTargetMoving && this._allowPredictiveAiming)
				{
					this.RotateToTargetPredicatively(time);
				}
				else
				{
					this.RotateToTarget(time);
				}
				if (this.CheckFireConditions())
				{
					this._timer = this._cooldown.Value;
					this.Shoot();
					return;
				}
			}
			else
			{
				this._entityDetector.Check();
			}
			return;
		}
		if (!this.CheckTarget(this._target))
		{
			Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
			return;
		}
		if (this._isTargetMoving && this._allowPredictiveAiming)
		{
			this.RotateToTargetPredicatively(time);
			return;
		}
		this.RotateToTarget(time);
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0002B344 File Offset: 0x00029544
	protected virtual void Shoot()
	{
		if (base.Entity.Has_EFlag_IsDead)
		{
			return;
		}
		for (int i = 0; i < this._bulletAmount.Value; i++)
		{
			EntityCreationData creationData = EventBuilder.BuildCreationData(this._bulletData.ID, base.FactionID, this._bulletSpawnPoint.position, SyncType.ServerInitiated);
			if (base.GetModel.HasAccent)
			{
				EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(base.GetModel.GetAccent()));
			}
			else
			{
				EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(this._turretData.bulletAccent));
			}
			EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.EntityCallback, base.RuntimeID, base.ComponentIndex);
			Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
		}
		if (this._requiresAmmo)
		{
			this._ammoBin.Consume(1);
		}
		this.ResetAnimation();
		if (base.Entity.IsOnScreen)
		{
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._turretData.shootSound, base.transform.name, base.transform.position, 1f, true, 0.9f, 1.1f, false);
			Singleton<AnimationManager>.Instance.RegisterAnimation(this);
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0002B472 File Offset: 0x00029672
	public override void OnCreationCallback(Entity entity)
	{
		if (entity.Has_EComponent<Bullet>())
		{
			entity.transform.rotation = this._rotatorPivot.transform.rotation;
			entity.Get_EComponent<Bullet>(false).LinkTurretValues(this, false);
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0002B4A8 File Offset: 0x000296A8
	public bool Animate(float timeSinceLastUpdate)
	{
		if (!base.Entity.IsOnScreen || base.Entity.Has_EFlag_IsCloaked)
		{
			return true;
		}
		if (this._barrelPivot != null)
		{
			this._animationTime += timeSinceLastUpdate;
			float y = Mathf.Sin(this._animationTime * this._recoilFrequency) * this._recoilAmplitude;
			this._barrelPivot.localPosition = Vector2.zero - new Vector2(0f, y);
			return this._barrelPivot.localPosition.y > 0f;
		}
		Debug.Log("[TURRET] The barrel transform has gone missing!");
		return true;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0002B54F File Offset: 0x0002974F
	public void ResetAnimation()
	{
		if (this._barrelPivot != null)
		{
			this._barrelPivot.localPosition = Vector2.zero;
		}
		this._animationTime = 0f;
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0002B57F File Offset: 0x0002977F
	protected virtual bool CheckFireConditions()
	{
		return (!this._requiresAmmo || this._ammoBin.HasResource) && this._timer <= 0f && this._rotatorPivot.IsAlignedToTarget();
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0002B5B0 File Offset: 0x000297B0
	public virtual void OnHitDetectorCheckFinished(Entity entity)
	{
		if (entity == null)
		{
			if (base.IsUpdating)
			{
				Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
			}
			return;
		}
		this._target = entity;
		if (this._target.Has_EComponent<Unit>())
		{
			Unit unit = this._target.Get_EComponent<Unit>(false);
			if (unit.GetSpeed() > 0f)
			{
				this._isTargetMoving = true;
				this._targetSpeed = unit.GetSpeed();
			}
			else
			{
				this._isTargetMoving = false;
			}
		}
		else
		{
			this._isTargetMoving = false;
		}
		if (!base.IsUpdating)
		{
			Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0002B640 File Offset: 0x00029840
	protected virtual void RotateToTarget(float time)
	{
		this._rotatorPivot.RotateTowards(this._target.transform, time);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0002B659 File Offset: 0x00029859
	protected virtual void RotateToTargetPredicatively(float time)
	{
		this._rotatorPivot.PredicativelyRotateTowards(this._target.transform, this._targetSpeed, this._bulletSpeed.Value, time);
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0002B683 File Offset: 0x00029883
	protected virtual void FaceTarget()
	{
		this._rotatorPivot.FaceTowards(this._target.transform);
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0002B69C File Offset: 0x0002989C
	public override void OnReset()
	{
		if (this._isListening)
		{
			this._entityDetector.OnHitDetectorCheckFinished -= this.OnHitDetectorCheckFinished;
			this._isListening = false;
		}
		this._rotatorPivot.transform.localPosition = Vector2.zero;
		this._rotatorPivot.transform.localRotation = Quaternion.identity;
		this._barrelPivot.transform.localPosition = Vector2.zero;
		this._barrelPivot.transform.localRotation = Quaternion.identity;
		foreach (Transform transform in base.Entity.GetModel.GetAnimationGroup(AnimationGroup.Primary))
		{
			transform.SetParent(base.Entity.GetModel.transform);
		}
		this._distanceCheckCooldown = 10;
	}

	// Token: 0x04000648 RID: 1608
	public TurretData _turretData;

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	protected Entity _target;

	// Token: 0x0400064A RID: 1610
	protected bool _isAnimating;

	// Token: 0x0400064B RID: 1611
	protected bool _allowPredictiveAiming;

	// Token: 0x0400064C RID: 1612
	protected bool _isTargetMoving;

	// Token: 0x0400064D RID: 1613
	protected float _targetSpeed;

	// Token: 0x0400064E RID: 1614
	protected Rotator _rotatorPivot;

	// Token: 0x0400064F RID: 1615
	protected Transform _barrelPivot;

	// Token: 0x04000650 RID: 1616
	protected Transform _bulletSpawnPoint;

	// Token: 0x04000651 RID: 1617
	protected EntityDetector _entityDetector;

	// Token: 0x04000652 RID: 1618
	protected float _timer;

	// Token: 0x04000653 RID: 1619
	protected float _animationTime;

	// Token: 0x04000654 RID: 1620
	protected StatInt _range;

	// Token: 0x04000655 RID: 1621
	protected StatFloat _damage;

	// Token: 0x04000656 RID: 1622
	protected StatFloat _cooldown;

	// Token: 0x04000657 RID: 1623
	protected StatFloat _bulletSpeed;

	// Token: 0x04000658 RID: 1624
	protected StatFloat _bulletLifetime;

	// Token: 0x04000659 RID: 1625
	protected StatFloat _bulletSize;

	// Token: 0x0400065A RID: 1626
	protected StatFloat _bulletSpread;

	// Token: 0x0400065B RID: 1627
	protected StatInt _bulletAmount;

	// Token: 0x0400065C RID: 1628
	protected StatInt _bulletPierces;

	// Token: 0x0400065D RID: 1629
	protected StatFloat _barrelSpeed;

	// Token: 0x0400065E RID: 1630
	protected float _modifiedRange;

	// Token: 0x0400065F RID: 1631
	protected float _recoilAmplitude;

	// Token: 0x04000660 RID: 1632
	protected float _recoilFrequency;

	// Token: 0x04000661 RID: 1633
	protected float _bulletSpeedOffset;

	// Token: 0x04000662 RID: 1634
	protected float _bulletLifetimeOffset;

	// Token: 0x04000663 RID: 1635
	protected EntityData _bulletData;

	// Token: 0x04000664 RID: 1636
	protected LayerMask _bulletLayer;

	// Token: 0x04000665 RID: 1637
	protected int _distanceCheckCooldown = 10;

	// Token: 0x04000666 RID: 1638
	protected bool _requiresAmmo;

	// Token: 0x04000667 RID: 1639
	protected AmmoBin _ammoBin;

	// Token: 0x04000668 RID: 1640
	protected PowerModule _powerModule;

	// Token: 0x04000669 RID: 1641
	private bool _isListening;
}
