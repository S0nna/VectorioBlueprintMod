using System;

// Token: 0x02000079 RID: 121
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MetadataForAttribute : Attribute
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001E601 File Offset: 0x0001C801
	public Type ComponentType { get; }

	// Token: 0x06000599 RID: 1433 RVA: 0x0001E609 File Offset: 0x0001C809
	public MetadataForAttribute(Type componentType)
	{
		this.ComponentType = componentType;
	}
}
