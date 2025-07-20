using System;

// Token: 0x02000083 RID: 131
public class ResourceAction
{
	// Token: 0x060005D5 RID: 1493 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
	public ResourceAction(ResourceModule target, ResourceData resource, int amount, bool pickup)
	{
		this.Target = target;
		this.Resource = resource;
		this.Amount = amount;
		this.Pickup = pickup;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0001EFD9 File Offset: 0x0001D1D9
	public ResourceAction()
	{
		this.Target = null;
		this.Resource = null;
		this.Amount = 0;
		this.Pickup = false;
	}

	// Token: 0x04000351 RID: 849
	public ResourceModule Target;

	// Token: 0x04000352 RID: 850
	public ResourceData Resource;

	// Token: 0x04000353 RID: 851
	public int Amount;

	// Token: 0x04000354 RID: 852
	public bool Pickup;
}
