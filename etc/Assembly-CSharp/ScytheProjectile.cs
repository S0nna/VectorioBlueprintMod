using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x02000131 RID: 305
public class ScytheProjectile : MonoBehaviour
{
	// Token: 0x06000A0F RID: 2575 RVA: 0x0002A0B0 File Offset: 0x000282B0
	public void Setup(Scythe parent, LayerMask layer)
	{
		this._parent = parent;
		if (this._circleCollider2D == null)
		{
			this._circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
			this._circleCollider2D.isTrigger = true;
			this._circleCollider2D.radius = this._parent.GetData().scytheSize;
		}
		base.gameObject.layer = layer;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0002A11B File Offset: 0x0002831B
	public void Launch(Vector2 position)
	{
		this._elapsedTime = 0f;
		this._targetPosition = position;
		this._headingToTarget = true;
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0002A138 File Offset: 0x00028338
	public void Tick(float time)
	{
		this._elapsedTime += time;
		float num = Mathf.Clamp01(this._elapsedTime / this._parent.TravelTime);
		float t = this.EaseInOutCubic(num);
		if (this._headingToTarget)
		{
			base.transform.position = Vector2.Lerp(this._parent.transform.position, this._targetPosition, t);
			if (num >= 1f)
			{
				this._headingToTarget = false;
				this._elapsedTime = 0f;
				this._hitList.Clear();
				return;
			}
		}
		else
		{
			base.transform.position = Vector2.Lerp(this._targetPosition, this._parent.transform.position, t);
			if (num >= 1f)
			{
				this._parent.OnScytheReturned();
				this._hitList.Clear();
			}
		}
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0002A221 File Offset: 0x00028421
	private float EaseInOutCubic(float t)
	{
		if ((double)t >= 0.5)
		{
			return 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
		}
		return 4f * t * t * t;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0002A260 File Offset: 0x00028460
	public virtual void OnTriggerEnter2D(Collider2D collider)
	{
		Entity component = collider.GetComponent<Entity>();
		if (component != null && !component.IsAlly(this._parent.FactionID) && component.Has_EComponent<HealthComponent>())
		{
			Singleton<EntityManager>.Instance.QueueDamageEvent(EventBuilder.BuildDamageEvent(component, (float)this._parent.Damage, this._parent.Entity), SyncType.ServerInitiated);
			this._hitList.Add(component.RuntimeID);
			AudioClip hitSound = this._parent.GetData().hitSound;
			if (hitSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayClipAtPoint(hitSound, "scythe_hit", base.transform.position, 1f, true, 0.9f, 1.1f, false);
			}
		}
	}

	// Token: 0x0400062E RID: 1582
	private Scythe _parent;

	// Token: 0x0400062F RID: 1583
	private List<uint> _hitList = new List<uint>();

	// Token: 0x04000630 RID: 1584
	private float _elapsedTime;

	// Token: 0x04000631 RID: 1585
	private Vector2 _targetPosition;

	// Token: 0x04000632 RID: 1586
	private bool _headingToTarget;

	// Token: 0x04000633 RID: 1587
	private CircleCollider2D _circleCollider2D;
}
