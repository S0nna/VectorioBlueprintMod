using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class Shield : BulletDetector
{
	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002A338 File Offset: 0x00028538
	public bool IsActive
	{
		get
		{
			return this._isActive;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0002A340 File Offset: 0x00028540
	public float GetRange
	{
		get
		{
			return this._range;
		}
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0002A348 File Offset: 0x00028548
	public void Setup(Shielder shielder)
	{
		this._shielder = shielder;
		ShielderData data = shielder.GetData();
		this._range = (float)data.range;
		if (this._shieldIcon == null)
		{
			this._shieldIcon = base.gameObject.AddComponent<SpriteRenderer>();
			this._shieldIcon.sortingLayerName = Layers.SORTING_BUILDING_LAYER;
			this._shieldIcon.sortingOrder = 100;
			this._circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
			this._circleCollider2D.isTrigger = true;
			this._circleCollider2D.radius = 1.2f;
			if (this._isActive)
			{
				this.EnableShield();
			}
			else
			{
				this.DisableShield();
			}
		}
		this._shieldIcon.sprite = data.sprite;
		this._shieldIcon.material = data.material;
		if (shielder.GetAccent != null)
		{
			this._shieldIcon.color = shielder.GetAccent.primaryColor;
		}
		else
		{
			this._shieldIcon.color = data.color;
		}
		base.transform.localScale = new Vector2((float)data.range, (float)data.range);
		base.gameObject.layer = LayerMask.NameToLayer(Layers.BULLET_DETECTOR_LAYER(shielder.FactionID, false));
		this.DisableShield();
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0002A488 File Offset: 0x00028688
	public override void OnBulletDetected(Bullet bullet)
	{
		this._health -= bullet.GetDamage;
		bullet.ReverseBullet();
		if (this._shielder.Entity.IsOnScreen)
		{
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._shielder.GetData().hitSound, "sound_shield_hit", base.transform.position, 1f, true, 0.9f, 1.1f, false);
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0002A500 File Offset: 0x00028700
	public void DisableShield()
	{
		this._shieldIcon.transform.localScale = Vector2.zero;
		this._circleCollider2D.enabled = false;
		this._isActive = false;
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0002A52F File Offset: 0x0002872F
	public void EnableShield()
	{
		this._shieldIcon.transform.localScale = new Vector2(this._range, this._range);
		this._circleCollider2D.enabled = true;
		this._isActive = true;
	}

	// Token: 0x04000634 RID: 1588
	private bool _isActive;

	// Token: 0x04000635 RID: 1589
	protected float _health;

	// Token: 0x04000636 RID: 1590
	protected Shielder _shielder;

	// Token: 0x04000637 RID: 1591
	protected SpriteRenderer _shieldIcon;

	// Token: 0x04000638 RID: 1592
	protected CircleCollider2D _circleCollider2D;

	// Token: 0x04000639 RID: 1593
	protected float _range;
}
