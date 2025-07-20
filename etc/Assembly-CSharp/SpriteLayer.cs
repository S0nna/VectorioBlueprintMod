using System;
using UnityEngine;

// Token: 0x02000231 RID: 561
[Serializable]
public class SpriteLayer
{
	// Token: 0x06001063 RID: 4195 RVA: 0x00003212 File Offset: 0x00001412
	private void OnEnable()
	{
	}

	// Token: 0x04000E57 RID: 3671
	public Sprite sprite;

	// Token: 0x04000E58 RID: 3672
	public Material material;

	// Token: 0x04000E59 RID: 3673
	public Color color = Color.white;

	// Token: 0x04000E5A RID: 3674
	public float posX;

	// Token: 0x04000E5B RID: 3675
	public float posY;

	// Token: 0x04000E5C RID: 3676
	public float posZ;

	// Token: 0x04000E5D RID: 3677
	public int index;

	// Token: 0x04000E5E RID: 3678
	[Range(0.25f, 4f)]
	public float size = 1f;

	// Token: 0x04000E5F RID: 3679
	public bool useRotation;

	// Token: 0x04000E60 RID: 3680
	public float rotation;

	// Token: 0x04000E61 RID: 3681
	public AccentType accent;

	// Token: 0x04000E62 RID: 3682
	public AnimationGroup animationGroup;

	// Token: 0x04000E63 RID: 3683
	public bool overrideUI;

	// Token: 0x04000E64 RID: 3684
	public Sprite overrideSprite;

	// Token: 0x04000E65 RID: 3685
	public bool hideOnUI;

	// Token: 0x04000E66 RID: 3686
	public bool hideOnBlueprint;
}
