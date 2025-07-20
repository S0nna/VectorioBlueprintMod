using System;

// Token: 0x02000095 RID: 149
public interface IComponent<TComponent, TData> where TComponent : EntityComponent where TData : ComponentData<TComponent>
{
	// Token: 0x06000608 RID: 1544
	void OnInitialize(TData data);

	// Token: 0x06000609 RID: 1545
	void OnSpawn(bool fromSave);

	// Token: 0x0600060A RID: 1546
	TData GetData();
}
