using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Grid;

// Token: 0x020000C1 RID: 193
[Serializable]
public class OutpostData : SerializableData
{
	// Token: 0x04000428 RID: 1064
	public List<OutpostData.Entity> entities;

	// Token: 0x04000429 RID: 1065
	public Dictionary<string, List<DecorationData>> tiles;

	// Token: 0x0400042A RID: 1066
	public Dictionary<string, List<Vector2Int>> resources;

	// Token: 0x020000C2 RID: 194
	[Serializable]
	public class Entity
	{
		// Token: 0x0400042B RID: 1067
		public float posX;

		// Token: 0x0400042C RID: 1068
		public float posY;

		// Token: 0x0400042D RID: 1069
		public EntityMetadata metadata;
	}
}
