using System;
using UnityEngine;

// Token: 0x02000221 RID: 545
[Serializable]
public class ModelSprite
{
	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06001006 RID: 4102 RVA: 0x0004B59F File Offset: 0x0004979F
	public SpriteRenderer GetSprite
	{
		get
		{
			return this.sprite;
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0004B5A7 File Offset: 0x000497A7
	public ModelSprite(SpriteRenderer sprite, float colorMultiplier)
	{
		this.sprite = sprite;
		this.colorMultiplier = colorMultiplier;
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0004B5C8 File Offset: 0x000497C8
	public void SetColor(Color color, bool multiply)
	{
		if (multiply)
		{
			this.sprite.color = new Color(color.r * this.colorMultiplier, color.g * this.colorMultiplier, color.b * this.colorMultiplier, 1f);
			return;
		}
		this.sprite.color = color;
	}

	// Token: 0x04000E20 RID: 3616
	[SerializeField]
	protected SpriteRenderer sprite;

	// Token: 0x04000E21 RID: 3617
	protected float colorMultiplier = 1f;
}
