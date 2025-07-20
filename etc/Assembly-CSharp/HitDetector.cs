using System;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000214 RID: 532
public class HitDetector : MonoBehaviour
{
	// Token: 0x06000FBE RID: 4030 RVA: 0x0004A2D6 File Offset: 0x000484D6
	public void SetMode(int mode)
	{
		this.SetMode((HitDetector.Mode)mode);
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x0004A2DF File Offset: 0x000484DF
	public void SetMode(HitDetector.Mode mode)
	{
		this._mode = mode;
		this.HardCheckUnits(true);
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0004A2EF File Offset: 0x000484EF
	public HitDetector.Mode GetMode
	{
		get
		{
			return this._mode;
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0004A2F7 File Offset: 0x000484F7
	// (set) Token: 0x06000FC2 RID: 4034 RVA: 0x0004A2FF File Offset: 0x000484FF
	public bool WaitingForQuery
	{
		get
		{
			return this._waitingForQuery;
		}
		set
		{
			this._waitingForQuery = value;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0004A308 File Offset: 0x00048508
	public EntityDetector Parent
	{
		get
		{
			return this._parent;
		}
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x0004A310 File Offset: 0x00048510
	public HitDetector Setup(EntityDetector parent, ref StatInt range, string layer, bool staticDetector)
	{
		this._parent = parent;
		this._range = range;
		base.gameObject.isStatic = staticDetector;
		this._unitDetectionLayer = LayerMask.GetMask(new string[]
		{
			Layers.UNIT_LAYER(this._parent.FactionID, true)
		});
		this._buildingDetectionLayer = LayerMask.GetMask(new string[]
		{
			Layers.BUILDING_LAYER(this._parent.FactionID, true)
		});
		base.gameObject.layer = LayerMask.NameToLayer(layer);
		if (this._circleCollider2D == null)
		{
			this._circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
			this._circleCollider2D.isTrigger = true;
			this._circleCollider2D.radius = (float)this._range.Value * 5f;
		}
		this.HardCheckBuildings(true);
		return this;
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x0004A3EE File Offset: 0x000485EE
	public virtual void SoftCheck()
	{
		if (!this.WaitingForQuery)
		{
			Singleton<CallbackManager>.Instance.CreateDetectorQuery(this);
			this.WaitingForQuery = true;
		}
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x0004A40C File Offset: 0x0004860C
	public virtual void HardCheckUnits(bool hardCheckBuildings = false)
	{
		RaycastHit2D[] results = new RaycastHit2D[10];
		int num = Physics2D.CircleCastNonAlloc(base.transform.position, this._circleCollider2D.radius, -Vector3.forward, results, float.PositiveInfinity, this._unitDetectionLayer);
		if (num == 0)
		{
			if (hardCheckBuildings)
			{
				this.HardCheckBuildings(false);
				return;
			}
			this.EnterStandby();
			return;
		}
		else
		{
			Entity target = this.GetTarget(results, num);
			if (target != null)
			{
				this.TargetFound(target);
				return;
			}
			this.EnterStandby();
			return;
		}
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x0004A498 File Offset: 0x00048698
	public virtual void HardCheckBuildings(bool hardCheckUnits = false)
	{
		RaycastHit2D[] results = new RaycastHit2D[10];
		int num = Physics2D.CircleCastNonAlloc(base.transform.position, this._circleCollider2D.radius, -Vector3.forward, results, float.PositiveInfinity, this._buildingDetectionLayer);
		if (num == 0)
		{
			if (hardCheckUnits)
			{
				this.HardCheckUnits(false);
				return;
			}
			this.EnterStandby();
			return;
		}
		else
		{
			Entity target = this.GetTarget(results, num);
			if (target != null)
			{
				this.TargetFound(target);
				return;
			}
			this.EnterStandby();
			return;
		}
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x0004A523 File Offset: 0x00048723
	private void TargetFound(Entity entity)
	{
		this.ToggleCollisions(false);
		this._parent.EntityDetected(entity);
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x0004A538 File Offset: 0x00048738
	private void EnterStandby()
	{
		this.ToggleCollisions(true);
		this._parent.EntityDetected(null);
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x0004A54D File Offset: 0x0004874D
	private bool CheckTarget(Entity target)
	{
		return target != null && !target.Has_EFlag_IsBlueprint && target.Has_EFlag_IsTargetable && !target.Has_EFlag_IsDead && !target.IsAlly(this._parent.FactionID);
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x0004A588 File Offset: 0x00048788
	public virtual Entity GetTarget(RaycastHit2D[] results, int hits)
	{
		Entity result = null;
		switch (this._mode)
		{
		case HitDetector.Mode.Closest:
		{
			float num = float.PositiveInfinity;
			for (int i = 0; i < hits; i++)
			{
				Entity component = results[i].collider.GetComponent<Entity>();
				if (this.CheckTarget(component))
				{
					float sqrMagnitude = (component.transform.position - base.transform.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = component;
					}
				}
			}
			break;
		}
		case HitDetector.Mode.Furthest:
		{
			float num2 = float.NegativeInfinity;
			for (int j = 0; j < hits; j++)
			{
				Entity component2 = results[j].collider.GetComponent<Entity>();
				if (this.CheckTarget(component2))
				{
					float sqrMagnitude2 = (component2.transform.position - base.transform.position).sqrMagnitude;
					if (sqrMagnitude2 > num2)
					{
						num2 = sqrMagnitude2;
						result = component2;
					}
				}
			}
			break;
		}
		case HitDetector.Mode.Strongest:
		{
			float num3 = float.NegativeInfinity;
			for (int k = 0; k < hits; k++)
			{
				Entity component3 = results[k].collider.GetComponent<Entity>();
				if (this.CheckTarget(component3) && component3.Has_EComponent<HealthComponent>())
				{
					HealthComponent healthComponent = component3.Get_EComponent<HealthComponent>(false);
					if (healthComponent.Health > num3)
					{
						num3 = healthComponent.Health;
						result = component3;
					}
				}
			}
			break;
		}
		case HitDetector.Mode.Weakest:
		{
			float num4 = float.PositiveInfinity;
			for (int l = 0; l < hits; l++)
			{
				Entity component4 = results[l].collider.GetComponent<Entity>();
				if (this.CheckTarget(component4) && component4.Has_EComponent<HealthComponent>())
				{
					HealthComponent healthComponent2 = component4.Get_EComponent<HealthComponent>(false);
					if (healthComponent2.Health < num4)
					{
						num4 = healthComponent2.Health;
						result = component4;
					}
				}
			}
			break;
		}
		}
		return result;
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x0004A750 File Offset: 0x00048950
	public virtual void OnTriggerEnter2D(Collider2D collider)
	{
		Entity component = collider.GetComponent<Entity>();
		if (this.CheckTarget(component))
		{
			this.TargetFound(component);
		}
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x0004A774 File Offset: 0x00048974
	public void ToggleCollisions(bool toggle)
	{
		this._circleCollider2D.enabled = toggle;
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x0004A784 File Offset: 0x00048984
	public void ToggleVisibility(bool toggle)
	{
		if (toggle)
		{
			Singleton<EntityUtilities>.Instance.ShowRange(base.transform.position, (float)this._range.Value * 2f + 1f);
			return;
		}
		Singleton<EntityUtilities>.Instance.HideRange();
	}

	// Token: 0x04000DCF RID: 3535
	[SerializeField]
	protected HitDetector.Mode _mode;

	// Token: 0x04000DD0 RID: 3536
	protected EntityDetector _parent;

	// Token: 0x04000DD1 RID: 3537
	protected StatInt _range;

	// Token: 0x04000DD2 RID: 3538
	protected bool _waitingForQuery;

	// Token: 0x04000DD3 RID: 3539
	[SerializeField]
	protected LayerMask _unitDetectionLayer;

	// Token: 0x04000DD4 RID: 3540
	[SerializeField]
	protected LayerMask _buildingDetectionLayer;

	// Token: 0x04000DD5 RID: 3541
	protected CircleCollider2D _circleCollider2D;

	// Token: 0x04000DD6 RID: 3542
	protected Rigidbody2D _rigidBody2D;

	// Token: 0x02000215 RID: 533
	public enum Mode
	{
		// Token: 0x04000DD8 RID: 3544
		Closest,
		// Token: 0x04000DD9 RID: 3545
		Furthest,
		// Token: 0x04000DDA RID: 3546
		Strongest,
		// Token: 0x04000DDB RID: 3547
		Weakest
	}
}
