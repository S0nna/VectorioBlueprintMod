using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021E RID: 542
[Serializable]
public class Model
{
	// Token: 0x06000FEC RID: 4076 RVA: 0x0004AD51 File Offset: 0x00048F51
	public Model(string name, List<SpriteLayer> layers)
	{
		this.name = name;
		this.layers = layers.ToArray();
		this.UpdateID();
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x0004AD80 File Offset: 0x00048F80
	public Color GetAccentColor(AccentType accentType)
	{
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.layers[i].accent == accentType)
			{
				return this.layers[i].color;
			}
		}
		return Color.white;
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0004ADC4 File Offset: 0x00048FC4
	public Material GetAccentMaterial(AccentType accentType)
	{
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.layers[i].accent == accentType)
			{
				return this.layers[i].material;
			}
		}
		return Library.GetDefaultMaterial();
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0004AE07 File Offset: 0x00049007
	private void UpdateID()
	{
		this.id = this.name.Replace(' ', '_').ToLower();
	}

	// Token: 0x04000E10 RID: 3600
	public string id;

	// Token: 0x04000E11 RID: 3601
	public string name;

	// Token: 0x04000E12 RID: 3602
	[SerializeField]
	public SpriteLayer[] layers = new SpriteLayer[0];
}
