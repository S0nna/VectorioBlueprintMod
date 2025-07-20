using System;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x020002B3 RID: 691
	public struct ProceduralImageInfo
	{
		// Token: 0x06001356 RID: 4950 RVA: 0x00058A6C File Offset: 0x00056C6C
		public ProceduralImageInfo(float width, float height, float fallOffDistance, float pixelSize, Vector4 radius, float borderWidth)
		{
			this.width = Mathf.Abs(width);
			this.height = Mathf.Abs(height);
			this.fallOffDistance = Mathf.Max(0f, fallOffDistance);
			this.radius = radius;
			this.borderWidth = Mathf.Max(borderWidth, 0f);
			this.pixelSize = Mathf.Max(0f, pixelSize);
		}

		// Token: 0x040010E2 RID: 4322
		public float width;

		// Token: 0x040010E3 RID: 4323
		public float height;

		// Token: 0x040010E4 RID: 4324
		public float fallOffDistance;

		// Token: 0x040010E5 RID: 4325
		public Vector4 radius;

		// Token: 0x040010E6 RID: 4326
		public float borderWidth;

		// Token: 0x040010E7 RID: 4327
		public float pixelSize;
	}
}
