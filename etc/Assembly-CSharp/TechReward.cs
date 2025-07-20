using System;

// Token: 0x02000189 RID: 393
[Serializable]
public class TechReward
{
	// Token: 0x06000D3F RID: 3391 RVA: 0x000399EB File Offset: 0x00037BEB
	public bool CheckEntityData()
	{
		return this.recipe != null || this.resource != null;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00039A09 File Offset: 0x00037C09
	public bool CheckRecipeData()
	{
		return this.entity != null || this.resource != null;
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00039A27 File Offset: 0x00037C27
	public bool CheckResourceData()
	{
		return this.entity != null || this.recipe != null;
	}

	// Token: 0x04000939 RID: 2361
	public EntityData entity;

	// Token: 0x0400093A RID: 2362
	public RecipeData recipe;

	// Token: 0x0400093B RID: 2363
	public ResourceData resource;
}
