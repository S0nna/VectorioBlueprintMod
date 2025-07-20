using System;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class TempBackdrop : Singleton<TempBackdrop>
{
	// Token: 0x06000F65 RID: 3941 RVA: 0x00048C64 File Offset: 0x00046E64
	public void Set(Color color, Sprite backdropSprite, Material backdropMaterial)
	{
		this.layerOne.color = color;
		this.layerTwo.color = color;
		this.layerOne.sprite = backdropSprite;
		this.layerTwo.sprite = backdropSprite;
		this.layerOne.material = backdropMaterial;
		this.layerTwo.material = backdropMaterial;
	}

	// Token: 0x04000CFB RID: 3323
	public SpriteRenderer layerOne;

	// Token: 0x04000CFC RID: 3324
	public SpriteRenderer layerTwo;
}
