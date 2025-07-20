using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;

namespace Vectorio.Generation
{
	// Token: 0x0200027B RID: 635
	[Serializable]
	public class WorldFeature
	{
		// Token: 0x04000FAE RID: 4014
		public string featureName = "New Spawnable";

		// Token: 0x04000FAF RID: 4015
		public int cellsToPick;

		// Token: 0x04000FB0 RID: 4016
		public bool useMinDistance;

		// Token: 0x04000FB1 RID: 4017
		public int minCellDistance;

		// Token: 0x04000FB2 RID: 4018
		public bool useMaxDistance;

		// Token: 0x04000FB3 RID: 4019
		public int maxCellDistance;

		// Token: 0x04000FB4 RID: 4020
		public bool hasEntityFeature;

		// Token: 0x04000FB5 RID: 4021
		public WorldFeature.EntityFeature entityFeature;

		// Token: 0x04000FB6 RID: 4022
		public bool hasResourceFeature;

		// Token: 0x04000FB7 RID: 4023
		public WorldFeature.ResourceFeature resourceFeature;

		// Token: 0x04000FB8 RID: 4024
		public bool hasOutpostFeature;

		// Token: 0x04000FB9 RID: 4025
		public WorldFeature.OutpostFeature outpostFeature;

		// Token: 0x04000FBA RID: 4026
		public bool enableAdvancedFeatureSettings;

		// Token: 0x04000FBB RID: 4027
		public bool stripNearbyCells;

		// Token: 0x04000FBC RID: 4028
		public int stripRange;

		// Token: 0x04000FBD RID: 4029
		public bool linkEntityToResource;

		// Token: 0x04000FBE RID: 4030
		public bool isEnabled = true;

		// Token: 0x0200027C RID: 636
		[Serializable]
		public class BaseFeature
		{
			// Token: 0x04000FBF RID: 4031
			public int amountToSpawn;

			// Token: 0x04000FC0 RID: 4032
			public int minDistance;

			// Token: 0x04000FC1 RID: 4033
			public int maxDistance = 8;
		}

		// Token: 0x0200027D RID: 637
		[Serializable]
		public class EntityFeature : WorldFeature.BaseFeature
		{
			// Token: 0x04000FC2 RID: 4034
			public EntityData data;

			// Token: 0x04000FC3 RID: 4035
			public FactionData faction;

			// Token: 0x04000FC4 RID: 4036
			public List<VariableObject> variables;
		}

		// Token: 0x0200027E RID: 638
		[Serializable]
		public class ResourceFeature : WorldFeature.BaseFeature
		{
			// Token: 0x04000FC5 RID: 4037
			public ResourceData data;

			// Token: 0x04000FC6 RID: 4038
			public bool useUniqueNodes;

			// Token: 0x04000FC7 RID: 4039
			public NodeType nodeType;

			// Token: 0x04000FC8 RID: 4040
			public List<GameObject> nodes;
		}

		// Token: 0x0200027F RID: 639
		[Serializable]
		public class OutpostFeature : WorldFeature.BaseFeature
		{
			// Token: 0x04000FC9 RID: 4041
			public List<TextAsset> outposts;
		}
	}
}
