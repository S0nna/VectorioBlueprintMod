using System;
using FOW;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class FogOfWarHandler : Singleton<FogOfWarHandler>
{
	// Token: 0x06000E22 RID: 3618 RVA: 0x0003F0E3 File Offset: 0x0003D2E3
	public bool IsEnabled()
	{
		return this._fogOfWar.enabled;
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x0003F0F0 File Offset: 0x0003D2F0
	public void OnEnable()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		this._fogOfWar = base.GetComponent<FogOfWarWorld>();
		this._fogOfWar.enabled = false;
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x0003F118 File Offset: 0x0003D318
	public void Enable(RegionData region, Vector2 position, Vector2 size)
	{
		base.transform.position = position;
		this._fogOfWar.enabled = true;
		this._fogOfWar.WorldBounds.center = size;
		this._fogOfWar.WorldBounds.extents = size;
		this._fogOfWar.UnknownColor = region.fogColor;
		this._fogOfWar.FogTexture = region.fogTexture;
		this._fogOfWar.FogTextureTiling = region.textureTiling;
		this._fogOfWar.UpdateAllMaterialProperties();
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x0003F1AC File Offset: 0x0003D3AC
	public void Hide()
	{
		this._fogOfWar.enabled = false;
	}

	// Token: 0x04000AEA RID: 2794
	private FogOfWarWorld _fogOfWar;
}
