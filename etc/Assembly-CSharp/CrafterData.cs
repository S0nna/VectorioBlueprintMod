using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000AD RID: 173
public class CrafterData : ResourceComponentData<Crafter>
{
	// Token: 0x06000679 RID: 1657 RVA: 0x0001FA9E File Offset: 0x0001DC9E
	public override void ApplyData(Crafter component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0001FAA7 File Offset: 0x0001DCA7
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatInt(this.storage, StatType.CraftingCapacity)
		};
	}

	// Token: 0x040003EA RID: 1002
	public List<RecipeData> recipes;
}
