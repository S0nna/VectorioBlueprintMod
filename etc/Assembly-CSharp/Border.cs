using System;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class Border : Singleton<Border>
{
	// Token: 0x06000DC3 RID: 3523 RVA: 0x0003D070 File Offset: 0x0003B270
	public void SetBorder(Vector2Int size, Color fill, Material borderMaterial, Color backdropColor, Sprite backdropSprite, Material backdropMaterial)
	{
		this.xSize = (float)(size.x * 5) / 2f;
		this.ySize = (float)(size.y * 5) / 2f;
		float x = (float)(size.x + 1);
		float x2 = (float)(size.y + 1);
		base.transform.position = new Vector2(this.xSize, this.ySize);
		Singleton<TempBackdrop>.Instance.Set(backdropColor, backdropSprite, backdropMaterial);
		this.north.localPosition = new Vector2(0f, this.ySize);
		this.north.localScale = new Vector2(x, 1f);
		this.east.localPosition = new Vector2(this.xSize, 0f);
		this.east.localScale = new Vector2(x2, 1f);
		this.south.localPosition = new Vector2(0f, -this.ySize);
		this.south.localScale = new Vector2(x, 1f);
		this.west.localPosition = new Vector2(-this.xSize, 0f);
		this.west.localScale = new Vector2(x2, 1f);
		this.northFill.color = fill;
		this.eastFill.color = fill;
		this.southFill.color = fill;
		this.westFill.color = fill;
		this.northLight.material = borderMaterial;
		this.eastLight.material = borderMaterial;
		this.southLight.material = borderMaterial;
		this.westLight.material = borderMaterial;
		Camera.main.backgroundColor = fill;
		int x3 = size.x;
		int y = size.y;
		for (int i = 0; i < x3; i++)
		{
			Singleton<MapView>.Instance.SetCellColor(new Vector3Int(i, 0, 0), borderMaterial.color);
			Singleton<MapView>.Instance.SetCellColor(new Vector3Int(i, y, 0), borderMaterial.color);
		}
		for (int j = 0; j < y; j++)
		{
			Singleton<MapView>.Instance.SetCellColor(new Vector3Int(0, j, 0), borderMaterial.color);
			Singleton<MapView>.Instance.SetCellColor(new Vector3Int(x3, j, 0), borderMaterial.color);
		}
	}

	// Token: 0x040009F8 RID: 2552
	public float xSize;

	// Token: 0x040009F9 RID: 2553
	public float ySize;

	// Token: 0x040009FA RID: 2554
	public Transform north;

	// Token: 0x040009FB RID: 2555
	public Transform east;

	// Token: 0x040009FC RID: 2556
	public Transform south;

	// Token: 0x040009FD RID: 2557
	public Transform west;

	// Token: 0x040009FE RID: 2558
	public SpriteRenderer northFill;

	// Token: 0x040009FF RID: 2559
	public SpriteRenderer northLight;

	// Token: 0x04000A00 RID: 2560
	public SpriteRenderer eastFill;

	// Token: 0x04000A01 RID: 2561
	public SpriteRenderer eastLight;

	// Token: 0x04000A02 RID: 2562
	public SpriteRenderer southFill;

	// Token: 0x04000A03 RID: 2563
	public SpriteRenderer southLight;

	// Token: 0x04000A04 RID: 2564
	public SpriteRenderer westFill;

	// Token: 0x04000A05 RID: 2565
	public SpriteRenderer westLight;

	// Token: 0x04000A06 RID: 2566
	public SpriteRenderer backdrop;
}
