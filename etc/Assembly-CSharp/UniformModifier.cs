using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000051 RID: 81
[ModifierID("Uniform")]
public class UniformModifier : ProceduralImageModifier
{
	// Token: 0x17000036 RID: 54
	// (get) Token: 0x0600043D RID: 1085 RVA: 0x000166A8 File Offset: 0x000148A8
	// (set) Token: 0x0600043E RID: 1086 RVA: 0x000166B0 File Offset: 0x000148B0
	public float Radius
	{
		get
		{
			return this.radius;
		}
		set
		{
			this.radius = value;
			base._Graphic.SetVerticesDirty();
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x000166C4 File Offset: 0x000148C4
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		float num = this.radius;
		return new Vector4(num, num, num, num);
	}

	// Token: 0x04000223 RID: 547
	[SerializeField]
	private float radius;
}
