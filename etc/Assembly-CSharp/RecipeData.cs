using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C6 RID: 198
[CreateAssetMenu(fileName = "New Recipe", menuName = "Vectorio/Recipe")]
[Serializable]
public class RecipeData : BaseData
{
	// Token: 0x0400043A RID: 1082
	public ResourceData outputResource;

	// Token: 0x0400043B RID: 1083
	public List<ResourceData> inputResources;

	// Token: 0x0400043C RID: 1084
	public float time;
}
