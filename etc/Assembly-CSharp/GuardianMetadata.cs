using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EA RID: 234
[MetadataFor(typeof(Guardian))]
[Serializable]
public class GuardianMetadata : UnitMetadata
{
	// Token: 0x06000778 RID: 1912 RVA: 0x00003212 File Offset: 0x00001412
	public void SetMetadata(Guardian guardian)
	{
	}

	// Token: 0x040004F7 RID: 1271
	public int mode;

	// Token: 0x040004F8 RID: 1272
	[SerializeField]
	public List<GuardianMetadata.PatrolPoint> patrolPoints;

	// Token: 0x040004F9 RID: 1273
	public int patrolPointIndex;

	// Token: 0x020000EB RID: 235
	[Serializable]
	public class PatrolPoint
	{
		// Token: 0x0600077A RID: 1914 RVA: 0x00021B89 File Offset: 0x0001FD89
		public PatrolPoint(Vector2 point)
		{
			this.posX = point.x;
			this.posY = point.y;
		}

		// Token: 0x040004FA RID: 1274
		public float posX;

		// Token: 0x040004FB RID: 1275
		public float posY;
	}
}
