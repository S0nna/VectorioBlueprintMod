using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class EntityRenderer : MonoBehaviour
{
	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0004A1A2 File Offset: 0x000483A2
	public Entity Parent
	{
		get
		{
			return this._parent;
		}
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x0004A1AA File Offset: 0x000483AA
	public EntityRenderer Setup(Entity entity)
	{
		this._parent = entity;
		this.ForceUpdate();
		return this;
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x0004A1BA File Offset: 0x000483BA
	public void ForceUpdate()
	{
		if (this._parent == null)
		{
			return;
		}
		if (this._parent.Has_EFlag_IsDead)
		{
			return;
		}
		if (Singleton<MainCamera>.Instance.IsOnScreen(base.transform))
		{
			this._parent.IsOnScreen = true;
		}
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x0004A1F7 File Offset: 0x000483F7
	public void OnBecameVisible()
	{
		if (this._parent == null)
		{
			return;
		}
		if (!this._parent.Has_EFlag_IsDead)
		{
			this._parent.IsOnScreen = true;
		}
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x0004A221 File Offset: 0x00048421
	public void OnBecameInvisible()
	{
		if (this._parent == null)
		{
			return;
		}
		if (!this._parent.Has_EFlag_IsDead)
		{
			this._parent.IsOnScreen = false;
		}
	}

	// Token: 0x04000D82 RID: 3458
	private Entity _parent;
}
