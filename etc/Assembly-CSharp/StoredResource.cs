using System;

// Token: 0x02000233 RID: 563
[Serializable]
public class StoredResource
{
	// Token: 0x06001065 RID: 4197 RVA: 0x0004CE39 File Offset: 0x0004B039
	public StoredResource(ResourceData resource)
	{
		this._resource = resource;
		this._amount = 0;
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x0004CE4F File Offset: 0x0004B04F
	public StoredResource()
	{
		this._resource = null;
		this._amount = 0;
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x0004CE65 File Offset: 0x0004B065
	// (set) Token: 0x06001068 RID: 4200 RVA: 0x0004CE6D File Offset: 0x0004B06D
	public ResourceData ResourceData
	{
		get
		{
			return this._resource;
		}
		set
		{
			this._resource = value;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x0004CE76 File Offset: 0x0004B076
	// (set) Token: 0x0600106A RID: 4202 RVA: 0x0004CE7E File Offset: 0x0004B07E
	public int AmountStored
	{
		get
		{
			return this._amount;
		}
		set
		{
			this._amount = value;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x0600106B RID: 4203 RVA: 0x0004CE87 File Offset: 0x0004B087
	// (set) Token: 0x0600106C RID: 4204 RVA: 0x0004CE8F File Offset: 0x0004B08F
	public ResourceAction PendingAction
	{
		get
		{
			return this._pendingAction;
		}
		set
		{
			this._pendingAction = value;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x0600106D RID: 4205 RVA: 0x0004CE98 File Offset: 0x0004B098
	public bool HasPendingAction
	{
		get
		{
			return this._pendingAction != null;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x0600106E RID: 4206 RVA: 0x0004CEA3 File Offset: 0x0004B0A3
	public int AmountPending
	{
		get
		{
			ResourceAction pendingAction = this._pendingAction;
			if (pendingAction == null)
			{
				return 0;
			}
			return pendingAction.Amount;
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x0004CEB6 File Offset: 0x0004B0B6
	public int AmountAfterPending
	{
		get
		{
			return this.AmountStored + this.AmountPending;
		}
	}

	// Token: 0x04000E6A RID: 3690
	private ResourceData _resource;

	// Token: 0x04000E6B RID: 3691
	private int _amount;

	// Token: 0x04000E6C RID: 3692
	private ResourceAction _pendingAction;
}
