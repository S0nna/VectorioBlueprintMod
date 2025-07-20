using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x02000096 RID: 150
public interface IComponentData
{
	// Token: 0x0600060B RID: 1547
	void CreateAndAddComponent(Entity entity);

	// Token: 0x0600060C RID: 1548
	List<Stat> RetrieveStats();
}
