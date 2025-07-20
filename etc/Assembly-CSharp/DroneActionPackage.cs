using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000080 RID: 128
[Serializable]
public class DroneActionPackage
{
	// Token: 0x060005C2 RID: 1474 RVA: 0x0001ECE8 File Offset: 0x0001CEE8
	public DroneActionPackage(ResourceAction currentAction, Queue<ResourceAction> actions, ResourceData resource, MetadataContext context)
	{
		if (actions != null)
		{
			this.savedActions = new List<DroneAction>();
			if (currentAction != null)
			{
				this.savedActions.Add(new DroneAction(currentAction, context));
			}
			using (Queue<ResourceAction>.Enumerator enumerator = actions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResourceAction action = enumerator.Current;
					this.savedActions.Add(new DroneAction(action, context));
				}
				goto IL_8C;
			}
		}
		if (currentAction != null)
		{
			this.savedActions = new List<DroneAction>
			{
				new DroneAction(currentAction, context)
			};
		}
		else
		{
			this.savedActions = null;
		}
		IL_8C:
		this.ResourceID = (((resource != null) ? resource.ID : null) ?? null);
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0001EDA8 File Offset: 0x0001CFA8
	public DroneActionPackage(Queue<ResourceAction> actions, ResourceData resource, MetadataContext context)
	{
		if (actions != null)
		{
			this.savedActions = new List<DroneAction>();
			using (Queue<ResourceAction>.Enumerator enumerator = actions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResourceAction action = enumerator.Current;
					this.savedActions.Add(new DroneAction(action, context));
				}
				goto IL_57;
			}
		}
		this.savedActions = null;
		IL_57:
		this.ResourceID = (((resource != null) ? resource.ID : null) ?? null);
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0001EE34 File Offset: 0x0001D034
	public DroneActionPackage()
	{
		this.ResourceID = null;
		this.savedActions = null;
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001EE4A File Offset: 0x0001D04A
	// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001EE52 File Offset: 0x0001D052
	public string ResourceID { get; set; }

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001EE5B File Offset: 0x0001D05B
	// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0001EE63 File Offset: 0x0001D063
	[SerializeField]
	public List<DroneAction> savedActions { get; set; }

	// Token: 0x060005C9 RID: 1481 RVA: 0x0001EE6C File Offset: 0x0001D06C
	public bool HasResource()
	{
		return this.ResourceID != null && this.ResourceID != "";
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0001EE88 File Offset: 0x0001D088
	public bool HasActionQueue()
	{
		return this.savedActions != null && this.savedActions.Count > 0;
	}
}
