using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public static class EmptySprite
{
	// Token: 0x0600042E RID: 1070 RVA: 0x000164BA File Offset: 0x000146BA
	public static Sprite Get()
	{
		if (EmptySprite.instance == null)
		{
			EmptySprite.instance = Resources.Load<Sprite>("procedural_ui_image_default_sprite");
		}
		return EmptySprite.instance;
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x000164DD File Offset: 0x000146DD
	public static bool IsEmptySprite(Sprite s)
	{
		return EmptySprite.Get() == s;
	}

	// Token: 0x0400021A RID: 538
	private static Sprite instance;
}
