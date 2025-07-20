using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000307 RID: 775
	[CreateAssetMenu(fileName = "New Icon Library", menuName = "Modern UI Pack/New Icon Library")]
	public class IconLibrary : ScriptableObject
	{
		// Token: 0x04001331 RID: 4913
		public bool alwaysUpdate;

		// Token: 0x04001332 RID: 4914
		public bool optimizeUpdates = true;

		// Token: 0x04001333 RID: 4915
		public Texture2D searchIcon;

		// Token: 0x04001334 RID: 4916
		public List<IconLibrary.IconItem> icons = new List<IconLibrary.IconItem>();

		// Token: 0x02000308 RID: 776
		[Serializable]
		public class IconItem
		{
			// Token: 0x04001335 RID: 4917
			public string iconTitle = "Icon";

			// Token: 0x04001336 RID: 4918
			public Texture2D iconPreview;

			// Token: 0x04001337 RID: 4919
			public Sprite iconSprite32;

			// Token: 0x04001338 RID: 4920
			public Sprite iconSprite64;

			// Token: 0x04001339 RID: 4921
			public Sprite iconSprite128;

			// Token: 0x0400133A RID: 4922
			public Sprite iconSprite256;
		}
	}
}
