using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x0200004D RID: 77
[ModifierID("Free")]
public class FreeModifier : ProceduralImageModifier
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x000164EF File Offset: 0x000146EF
	// (set) Token: 0x06000431 RID: 1073 RVA: 0x000164F7 File Offset: 0x000146F7
	public Vector4 Radius
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

	// Token: 0x06000432 RID: 1074 RVA: 0x000164EF File Offset: 0x000146EF
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		return this.radius;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0001650C File Offset: 0x0001470C
	protected void OnValidate()
	{
		this.radius.x = Mathf.Max(0f, this.radius.x);
		this.radius.y = Mathf.Max(0f, this.radius.y);
		this.radius.z = Mathf.Max(0f, this.radius.z);
		this.radius.w = Mathf.Max(0f, this.radius.w);
	}

	// Token: 0x0400021B RID: 539
	[SerializeField]
	private Vector4 radius;
}
