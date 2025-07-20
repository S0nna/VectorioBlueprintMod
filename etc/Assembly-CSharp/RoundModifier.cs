using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000050 RID: 80
[ModifierID("Round")]
public class RoundModifier : ProceduralImageModifier
{
	// Token: 0x0600043B RID: 1083 RVA: 0x00016685 File Offset: 0x00014885
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		float num = Mathf.Min(imageRect.width, imageRect.height) * 0.5f;
		return new Vector4(num, num, num, num);
	}
}
