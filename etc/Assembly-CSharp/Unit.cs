using System;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000138 RID: 312
public class Unit : EntityComponent, IUpdateable, IComponent<Unit, UnitData>
{
	// Token: 0x06000A63 RID: 2659 RVA: 0x0002B7A4 File Offset: 0x000299A4
	public UnitData GetData()
	{
		return this._unitData;
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0002B7AC File Offset: 0x000299AC
	public Rigidbody2D Rigidbody2D
	{
		get
		{
			return this._rigidbody2D;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0002B7B4 File Offset: 0x000299B4
	public Entity Target
	{
		get
		{
			return this._target;
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0002B7BC File Offset: 0x000299BC
	public void SetBehaviour(Unit.Behaviour behaviour)
	{
		this._behaviour = behaviour;
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0002B7C5 File Offset: 0x000299C5
	public float GetSpeed()
	{
		return this._moveSpeed.Value;
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0002B7D4 File Offset: 0x000299D4
	public void OnInitialize(UnitData data)
	{
		this._unitData = data;
		GameObject unit_ICON_MARKER = LegacyLibrary.UNIT_ICON_MARKER;
		this._minimapIcon = Object.Instantiate<GameObject>(unit_ICON_MARKER).GetComponent<SpriteRenderer>();
		this._minimapIcon.transform.SetParent(base.transform);
		this._minimapIcon.transform.localPosition = new Vector3(0f, 0f, 1f);
		this._minimapIcon.transform.localScale = new Vector3(data.minimapIconSize, data.minimapIconSize, 1f);
		if (data.minimapIcon != null)
		{
			this._minimapIcon.sprite = data.minimapIcon;
		}
		this._circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		this._circleCollider2D.radius = data.colliderSize;
		this._rigidbody2D = base.gameObject.AddComponent<Rigidbody2D>();
		this._rigidbody2D.gravityScale = 0f;
		this._rigidbody2D.mass = data.physicalMass;
		this._rigidbody2D.drag = data.physicalDrag;
		this._rigidbody2D.angularDrag = data.physicalDrag;
		this._entityDetector = base.Entity.Get_EComponent<EntityDetector>(true);
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0002B908 File Offset: 0x00029B08
	public override void OnSpawn(bool fromSave)
	{
		Singleton<StatManager>.Instance.CreateStatInt(ref this._range, StatType.UnitRange, this._unitData.range, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._damage, StatType.UnitDamage, this._unitData.damage, this);
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._moveSpeed, StatType.UnitSpeed, this._unitData.moveSpeed, this);
		if (base.GetAccent != null)
		{
			this._minimapIcon.color = base.GetAccent.primaryColor;
		}
		base.gameObject.layer = LayerMask.NameToLayer(Layers.UNIT_LAYER(base.Entity.FactionID, false));
		if (!this._rigidbody2D.simulated)
		{
			this._rigidbody2D.simulated = true;
		}
		this._entityDetector.OnHitDetectorCheckFinished += this.OnHitDetectorCheckFinished;
		this._entityDetector.Setup(ref this._range, Layers.UNIT_DETECTOR_LAYER(base.FactionID, false), false);
		Singleton<Events>.Instance.onPauseStateUpdated.AddListener(new UnityAction(this.OnPauseStateUpdated));
		if (base.Entity.Has_EFlag_IsCloaked)
		{
			base.Entity.Cloak.HiderComponent.OnActiveChanged += this.OnCloakActivityChanged;
			this.OnCloakActivityChanged(base.Entity.Cloak.IsCloakActive);
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0002BA5C File Offset: 0x00029C5C
	public void OnCloakActivityChanged(bool active)
	{
		this._minimapIcon.gameObject.SetActive(active);
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0002BA6F File Offset: 0x00029C6F
	public void OnPauseStateUpdated()
	{
		this._rigidbody2D.simulated = !Singleton<Gamemode>.Instance.IsPaused;
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0002BA8C File Offset: 0x00029C8C
	public void Tick(float time)
	{
		if (!this._movingToPosition && (this._target == null || this._target.Has_EFlag_IsDead))
		{
			this.SetTargetPosition(Singleton<WorldGenerator>.Instance.CenterWorldPos);
		}
		switch (this._behaviour)
		{
		case Unit.Behaviour.Normal:
			this.MoveToTarget(time);
			return;
		case Unit.Behaviour.Leading:
		case Unit.Behaviour.Following:
		case Unit.Behaviour.Inactive:
			break;
		case Unit.Behaviour.Guarding:
			this.RotateAroundTarget(time);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0002BB00 File Offset: 0x00029D00
	protected virtual void MoveToTarget(float time)
	{
		this._step = this._moveSpeed.Value * time;
		this._rotateStep = this._unitData.rotateSpeed * time;
		this._angle = Mathf.Atan2(this._targetPosition.y - base.transform.position.y, this._targetPosition.x - base.transform.position.x) * 57.29578f;
		Quaternion to = Quaternion.Euler(new Vector3(0f, 0f, this._angle - 90f));
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this._rotateStep);
		base.transform.position += base.transform.up * this._step;
		this._entityDetector.SyncPosition();
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0002BBF8 File Offset: 0x00029DF8
	protected virtual void RotateAroundTarget(float time)
	{
		this._step = this._moveSpeed.Value * time;
		base.transform.RotateAround(this._targetPosition, base.transform.up, this._step);
		this._entityDetector.transform.position = base.transform.position;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0002BC5C File Offset: 0x00029E5C
	public void Knockback(float amount, Vector3 origin)
	{
		this._rigidbody2D.AddForce(Vector3.Normalize(origin - base.transform.position) * -amount);
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0002BC8C File Offset: 0x00029E8C
	public virtual void OnHitDetectorCheckFinished(Entity target)
	{
		if (target != null && this._target == null)
		{
			this._target = target;
			this._targetPosition = this._target.transform.position;
			this._movingToPosition = false;
			if (!base.IsUpdating)
			{
				Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
			}
		}
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0002BCEC File Offset: 0x00029EEC
	public virtual void SetTargetPosition(Vector2 position)
	{
		this._targetPosition = position;
		this._movingToPosition = true;
		if (!base.IsUpdating)
		{
			Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0002BD10 File Offset: 0x00029F10
	public void FaceTarget()
	{
		float num = Mathf.Atan2(this._targetPosition.y - base.transform.position.y, this._targetPosition.x - base.transform.position.x) * 57.29578f;
		Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, num - 90f));
		base.transform.rotation = rotation;
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0002BD8C File Offset: 0x00029F8C
	public virtual void OnTargetHit(Entity entity)
	{
		if (!entity.Has_EComponent<HealthComponent>())
		{
			Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(base.Entity, null), SyncType.ServerInitiated);
			return;
		}
		Singleton<EntityManager>.Instance.QueueDamageEvent(EventBuilder.BuildDamageEvent(entity, this._damage.Value, base.Entity), SyncType.ServerInitiated);
		this.Knockback(this._unitData.knockbackValue, entity.transform.position);
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0002BDF8 File Offset: 0x00029FF8
	public override void OnReset()
	{
		this._rigidbody2D.simulated = false;
		this._entityDetector.OnHitDetectorCheckFinished -= this.OnHitDetectorCheckFinished;
		this._target = null;
		Singleton<Events>.Instance.onPauseStateUpdated.RemoveListener(new UnityAction(this.OnPauseStateUpdated));
		if (!Singleton<EntityManager>.Instance.IsClearingEntities() && base.Entity.IsOnScreen)
		{
			ParticleSystem particle;
			AudioClip clip;
			if (this._unitData.useUniqueAnimations)
			{
				particle = this._unitData.deathParticle;
				clip = this._unitData.deathSound;
			}
			else
			{
				particle = LegacyLibrary.UNIT_DEATH_PARTICLE;
				clip = LegacyLibrary.UNIT_DEATH_SOUND;
			}
			if (Singleton<Settings>.Instance.UseParticles)
			{
				Singleton<EntityUtilities>.Instance.CreateBulletParticle(particle, base.Entity.GetModel.GetMaterial(AccentType.PrimaryMaterial).color, base.transform.position, base.transform.rotation);
			}
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(clip, base.transform.name, base.transform.position, 1f, true, 0.9f, 1.1f, false);
		}
		if (base.Entity.Has_EFlag_IsCloaked)
		{
			base.Entity.Cloak.HiderComponent.OnActiveChanged -= this.OnCloakActivityChanged;
		}
	}

	// Token: 0x0400066A RID: 1642
	private UnitData _unitData;

	// Token: 0x0400066B RID: 1643
	[SerializeField]
	protected Entity _target;

	// Token: 0x0400066C RID: 1644
	[SerializeField]
	protected Vector2 _targetPosition;

	// Token: 0x0400066D RID: 1645
	[SerializeField]
	protected Unit.Behaviour _behaviour;

	// Token: 0x0400066E RID: 1646
	protected Rigidbody2D _rigidbody2D;

	// Token: 0x0400066F RID: 1647
	protected CircleCollider2D _circleCollider2D;

	// Token: 0x04000670 RID: 1648
	protected EntityDetector _entityDetector;

	// Token: 0x04000671 RID: 1649
	protected StatInt _range;

	// Token: 0x04000672 RID: 1650
	protected StatFloat _damage;

	// Token: 0x04000673 RID: 1651
	protected StatFloat _moveSpeed;

	// Token: 0x04000674 RID: 1652
	protected bool _movingToPosition = true;

	// Token: 0x04000675 RID: 1653
	private float _angle;

	// Token: 0x04000676 RID: 1654
	private float _rotateStep;

	// Token: 0x04000677 RID: 1655
	private float _step;

	// Token: 0x04000678 RID: 1656
	private SpriteRenderer _minimapIcon;

	// Token: 0x02000139 RID: 313
	public enum Behaviour
	{
		// Token: 0x0400067A RID: 1658
		Normal,
		// Token: 0x0400067B RID: 1659
		Leading,
		// Token: 0x0400067C RID: 1660
		Following,
		// Token: 0x0400067D RID: 1661
		Guarding,
		// Token: 0x0400067E RID: 1662
		Inactive
	}
}
