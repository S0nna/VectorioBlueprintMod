using System;
using FOW;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x0200011D RID: 285
public class Illuminator : EntityComponent, IComponent<Illuminator, IlluminatorData>, IUpdateable
{
	// Token: 0x0600096E RID: 2414 RVA: 0x00027D6C File Offset: 0x00025F6C
	public IlluminatorData GetData()
	{
		return this._illuminatorData;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00027D74 File Offset: 0x00025F74
	public void OnInitialize(IlluminatorData data)
	{
		this._illuminatorData = data;
		if (data.rotates)
		{
			if (this._pivot == null)
			{
				this._pivot = new GameObject("Pivot").GetComponent<Transform>();
				this._pivot.SetParent(base.transform);
				this._pivot.transform.localPosition = Vector2.zero;
			}
			foreach (Transform transform in base.Entity.GetModel.GetAnimationGroup(AnimationGroup.Primary))
			{
				Vector2 v = transform.localPosition;
				transform.SetParent(this._pivot.transform);
				transform.transform.localPosition = v;
				transform.transform.localRotation = Quaternion.identity;
			}
			this._pivot.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
			this._revealer = this._pivot.gameObject.AddComponent<FogOfWarRevealer2D>();
			return;
		}
		this._revealer = base.gameObject.AddComponent<FogOfWarRevealer2D>();
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00027EC0 File Offset: 0x000260C0
	public override void OnSpawn(bool fromSave)
	{
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._viewRadius, StatType.LightRadius, this._illuminatorData.viewRadius, this);
		this._revealer.ViewRadius = this._viewRadius.Value;
		this._revealer.SoftenDistance = this._illuminatorData.softenDistance;
		this._revealer.ViewAngle = this._illuminatorData.viewAngle;
		this._revealer.Opacity = this._illuminatorData.opacity;
		if (this._illuminatorData.rotates)
		{
			Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
		}
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x00027F5B File Offset: 0x0002615B
	public void Tick(float time)
	{
		this._pivot.Rotate(Vector3.forward, this._illuminatorData.rotateSpeed * time);
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00027F7A File Offset: 0x0002617A
	public override void OnReset()
	{
		if (base.IsUpdating)
		{
			Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
		}
	}

	// Token: 0x040005D0 RID: 1488
	private IlluminatorData _illuminatorData;

	// Token: 0x040005D1 RID: 1489
	private FogOfWarRevealer2D _revealer;

	// Token: 0x040005D2 RID: 1490
	protected Transform _pivot;

	// Token: 0x040005D3 RID: 1491
	private StatFloat _viewRadius;
}
