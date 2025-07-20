using System;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x020002B4 RID: 692
	[DisallowMultipleComponent]
	public abstract class ProceduralImageModifier : MonoBehaviour
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x00058ACE File Offset: 0x00056CCE
		protected Graphic _Graphic
		{
			get
			{
				if (this.graphic == null)
				{
					this.graphic = base.GetComponent<Graphic>();
				}
				return this.graphic;
			}
		}

		// Token: 0x06001358 RID: 4952
		public abstract Vector4 CalculateRadius(Rect imageRect);

		// Token: 0x040010E8 RID: 4328
		protected Graphic graphic;
	}
}
