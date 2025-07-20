using System;
using System.Collections.Generic;
using FOW;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class FOW_Cloak : EntityComponent, IUpdateable
{
	// Token: 0x0600091E RID: 2334 RVA: 0x00026BC8 File Offset: 0x00024DC8
	public void AddLayer(SpriteRenderer layer)
	{
		if (!this._layers.Contains(layer))
		{
			this._layers.Add(layer);
			layer.color = new Color(layer.color.r, layer.color.g, layer.color.b, this._isActive ? 1f : 0f);
			return;
		}
		Debug.Log("[FOW_Cloak] This layer has already been added!");
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00026C3C File Offset: 0x00024E3C
	public void RemoveLayer(SpriteRenderer layer)
	{
		if (this._layers.Contains(layer))
		{
			this._layers.Remove(layer);
			layer.color = new Color(layer.color.r, layer.color.g, layer.color.b, 1f);
			return;
		}
		Debug.Log("[FOW_Cloak] This layer does not exist!");
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000920 RID: 2336 RVA: 0x00026CA0 File Offset: 0x00024EA0
	public FogOfWarHider HiderComponent
	{
		get
		{
			return this._fogOfWarHider;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000921 RID: 2337 RVA: 0x00026CA8 File Offset: 0x00024EA8
	public bool IsCloakActive
	{
		get
		{
			return this._isActive;
		}
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00026CB0 File Offset: 0x00024EB0
	public override void OnSpawn(bool fromSave)
	{
		if (this._fogOfWarHider == null)
		{
			this._fogOfWarHider = base.gameObject.AddComponent<FogOfWarHider>();
			this._fogOfWarHider.OnActiveChanged += this.OnVisibilityUpdated;
			this.OnVisibilityUpdated(this._fogOfWarHider.NumObservers > 0);
		}
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00026D08 File Offset: 0x00024F08
	public void Tick(float time)
	{
		if (!base.Entity.IsOnScreen)
		{
			foreach (SpriteRenderer spriteRenderer in this._layers)
			{
				if (spriteRenderer != null)
				{
					spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, this._isActive ? 1f : 0f);
				}
			}
			Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
			return;
		}
		if (this._isActive)
		{
			this._alpha += time * 5f;
			if (this._alpha >= 1f)
			{
				this._alpha = 1f;
				Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
			}
		}
		else
		{
			this._alpha -= time * 5f;
			if (this._alpha <= 0f)
			{
				this._alpha = 0f;
				Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
			}
		}
		foreach (SpriteRenderer spriteRenderer2 in this._layers)
		{
			if (spriteRenderer2 != null)
			{
				spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, this._alpha);
			}
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00026EA8 File Offset: 0x000250A8
	public void OnVisibilityUpdated(bool active)
	{
		this._isActive = active;
		base.Entity.Set_EFlag_IsCloaked(!active);
		if (base.Entity.IsOnScreen && !base.IsUpdating)
		{
			if (Singleton<EntityManager>.Instance != null)
			{
				Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
				return;
			}
		}
		else
		{
			foreach (SpriteRenderer spriteRenderer in this._layers)
			{
				if (spriteRenderer != null)
				{
					spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, this._isActive ? 1f : 0f);
				}
			}
		}
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00026F84 File Offset: 0x00025184
	public override void OnReset()
	{
		if (this._fogOfWarHider != null)
		{
			this.OnVisibilityUpdated(true);
			Object.Destroy(this._fogOfWarHider);
		}
	}

	// Token: 0x040005A9 RID: 1449
	private const float TRANSITION_SPEED = 5f;

	// Token: 0x040005AA RID: 1450
	private List<SpriteRenderer> _layers = new List<SpriteRenderer>();

	// Token: 0x040005AB RID: 1451
	private FogOfWarHider _fogOfWarHider;

	// Token: 0x040005AC RID: 1452
	private bool _isActive;

	// Token: 0x040005AD RID: 1453
	private float _alpha;
}
