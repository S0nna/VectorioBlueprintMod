using System;

// Token: 0x0200007F RID: 127
[Serializable]
public class DroneAction
{
	// Token: 0x060005C0 RID: 1472 RVA: 0x0001EC8F File Offset: 0x0001CE8F
	public DroneAction(ResourceAction action, MetadataContext ctx)
	{
		this.ID = new E_ID(action.Target.RuntimeID, ctx);
		this.Amount = action.Amount;
		this.Pickup = action.Pickup;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0001ECC6 File Offset: 0x0001CEC6
	public DroneAction()
	{
		this.ID = new E_ID();
		this.Amount = 0;
		this.Pickup = false;
	}

	// Token: 0x04000348 RID: 840
	public E_ID ID;

	// Token: 0x04000349 RID: 841
	public int Amount;

	// Token: 0x0400034A RID: 842
	public bool Pickup;
}
