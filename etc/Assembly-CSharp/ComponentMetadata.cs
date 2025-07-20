using System;

// Token: 0x020000E4 RID: 228
[Serializable]
public abstract class ComponentMetadata<TComponent> : ComponentMetadataWrapper where TComponent : EntityComponent
{
	// Token: 0x0600073D RID: 1853 RVA: 0x00021380 File Offset: 0x0001F580
	public ComponentMetadata()
	{
		base.Type = typeof(TComponent).FullName;
	}

	// Token: 0x0600073E RID: 1854
	public abstract void GetValues(TComponent component, MetadataContext context);

	// Token: 0x0600073F RID: 1855
	public abstract void SetValues(TComponent component, bool asPipette, MetadataContext context);

	// Token: 0x06000740 RID: 1856 RVA: 0x000213A0 File Offset: 0x0001F5A0
	public override void GetValuesFromComponent(EntityComponent component, MetadataContext context)
	{
		TComponent tcomponent = component as TComponent;
		if (tcomponent != null)
		{
			this.GetValues(tcomponent, context);
			return;
		}
		throw new InvalidOperationException("Component type mismatch.");
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x000213D4 File Offset: 0x0001F5D4
	public override void SetValuesToComponent(EntityComponent component, bool asPipette, MetadataContext context)
	{
		TComponent tcomponent = component as TComponent;
		if (tcomponent != null)
		{
			this.SetValues(tcomponent, asPipette, context);
			return;
		}
		throw new InvalidOperationException("Component type mismatch.");
	}
}
