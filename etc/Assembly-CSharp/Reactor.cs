using System;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x02000129 RID: 297
public class Reactor : EntityComponent, IComponent<Reactor, ReactorData>
{
	// Token: 0x060009C8 RID: 2504 RVA: 0x0002916C File Offset: 0x0002736C
	public ReactorData GetData()
	{
		return this._data;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00029174 File Offset: 0x00027374
	public void OnInitialize(ReactorData data)
	{
		this._data = data;
		this._resourceIcon = new GameObject("Resource Icon").AddComponent<SpriteRenderer>();
		this._resourceIcon.transform.SetParent(base.transform);
		this._resourceIcon.transform.localPosition = Vector2.zero;
		this._resourceIcon.transform.localScale = new Vector2(data.iconSize, data.iconSize);
		this._resourceIcon.sortingLayerName = Layers.SORTING_BUILDING_LAYER;
		this._resourceIcon.sortingOrder = 100;
		this._resourceIcon.material = Library.GetDefaultMaterial();
		this._resourceIcon.enabled = true;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0002922C File Offset: 0x0002742C
	public override void OnSpawn(bool fromSave)
	{
		ResourceData resource = Singleton<TileGrid>.Instance.GetResource(Utilities.ConvertWorldPositionToCell(base.transform.position));
		if (resource == null)
		{
			return;
		}
		this._enabled = true;
		this._powerAmount = resource.Power;
		this._resourceIcon.sprite = resource.IconSprite;
		base.Entity.GetModel.ApplyAccent(resource.Accent);
		if (base.IsPlayerFaction)
		{
			Singleton<ResourceManager>.Instance.AddPowerStorage(this._powerAmount);
		}
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x000292B5 File Offset: 0x000274B5
	public override void OnReset()
	{
		if (base.IsPlayerFaction && this._enabled)
		{
			Singleton<ResourceManager>.Instance.RemovePowerStorage(this._powerAmount);
		}
	}

	// Token: 0x04000600 RID: 1536
	private ReactorData _data;

	// Token: 0x04000601 RID: 1537
	protected SpriteRenderer _resourceIcon;

	// Token: 0x04000602 RID: 1538
	protected int _powerAmount;

	// Token: 0x04000603 RID: 1539
	protected bool _enabled;
}
