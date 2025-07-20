using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x0200004E RID: 78
[ModifierID("Only One Edge")]
public class OnlyOneEdgeModifier : ProceduralImageModifier
{
	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000435 RID: 1077 RVA: 0x000165A1 File Offset: 0x000147A1
	// (set) Token: 0x06000436 RID: 1078 RVA: 0x000165A9 File Offset: 0x000147A9
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

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000437 RID: 1079 RVA: 0x000165BD File Offset: 0x000147BD
	// (set) Token: 0x06000438 RID: 1080 RVA: 0x000165C5 File Offset: 0x000147C5
	public OnlyOneEdgeModifier.ProceduralImageEdge Side
	{
		get
		{
			return this.side;
		}
		set
		{
			this.side = value;
		}
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x000165D0 File Offset: 0x000147D0
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		switch (this.side)
		{
		case OnlyOneEdgeModifier.ProceduralImageEdge.Top:
			return new Vector4(this.radius, this.radius, 0f, 0f);
		case OnlyOneEdgeModifier.ProceduralImageEdge.Bottom:
			return new Vector4(0f, 0f, this.radius, this.radius);
		case OnlyOneEdgeModifier.ProceduralImageEdge.Left:
			return new Vector4(this.radius, 0f, 0f, this.radius);
		case OnlyOneEdgeModifier.ProceduralImageEdge.Right:
			return new Vector4(0f, this.radius, this.radius, 0f);
		default:
			return new Vector4(0f, 0f, 0f, 0f);
		}
	}

	// Token: 0x0400021C RID: 540
	[SerializeField]
	private float radius;

	// Token: 0x0400021D RID: 541
	[SerializeField]
	private OnlyOneEdgeModifier.ProceduralImageEdge side;

	// Token: 0x0200004F RID: 79
	public enum ProceduralImageEdge
	{
		// Token: 0x0400021F RID: 543
		Top,
		// Token: 0x04000220 RID: 544
		Bottom,
		// Token: 0x04000221 RID: 545
		Left,
		// Token: 0x04000222 RID: 546
		Right
	}
}
