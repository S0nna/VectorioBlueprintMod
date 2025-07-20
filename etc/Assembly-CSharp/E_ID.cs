using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
[Serializable]
public class E_ID
{
	// Token: 0x060005CB RID: 1483 RVA: 0x0001EEA2 File Offset: 0x0001D0A2
	public E_ID()
	{
		this.ID = 0U;
		this.ctx = MetadataContext.Global;
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0001EEB8 File Offset: 0x0001D0B8
	public E_ID(uint ID)
	{
		this.ID = ID;
		this.ctx = MetadataContext.Global;
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0001EECE File Offset: 0x0001D0CE
	public E_ID(uint ID, MetadataContext ctx)
	{
		this.ID = ID;
		this.ctx = ctx;
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001EEE4 File Offset: 0x0001D0E4
	// (set) Token: 0x060005CF RID: 1487 RVA: 0x0001EEEC File Offset: 0x0001D0EC
	public uint ID { get; set; }

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001EEF5 File Offset: 0x0001D0F5
	// (set) Token: 0x060005D1 RID: 1489 RVA: 0x0001EEFD File Offset: 0x0001D0FD
	public MetadataContext ctx { get; set; }

	// Token: 0x060005D2 RID: 1490 RVA: 0x0001EF06 File Offset: 0x0001D106
	public uint GetValue()
	{
		if (this.ctx == MetadataContext.Global)
		{
			return this.ID;
		}
		return ServerSingleton<ServerSyncManager>.Instance.MapID(this.ID);
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0001EF28 File Offset: 0x0001D128
	public bool TryGetEntity(out Entity entity)
	{
		if (Singleton<EntityManager>.Instance.TryGetEntity(this.GetValue(), out entity))
		{
			return !entity.Has_EFlag_IsDead;
		}
		Debug.Log("[E_ID] Failed to extract entity with ID " + this.GetValue().ToString() + " in context " + this.ctx.ToString());
		return false;
	}
}
