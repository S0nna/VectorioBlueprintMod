using System;
using System.Collections.Generic;
using Vectorio.Stats;

// Token: 0x020000A9 RID: 169
[Serializable]
public abstract class ComponentData<T> : IComponentData where T : EntityComponent
{
	// Token: 0x0600066D RID: 1645
	public abstract void ApplyData(T component);

	// Token: 0x0600066E RID: 1646 RVA: 0x0001FA18 File Offset: 0x0001DC18
	public void CreateAndAddComponent(Entity entity)
	{
		T component = entity.Add_EComponent<T>();
		this.ApplyData(component);
	}

	// Token: 0x0600066F RID: 1647
	public abstract List<Stat> RetrieveStats();
}
