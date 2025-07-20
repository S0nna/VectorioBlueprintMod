using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000C3 RID: 195
public class PortData : ComponentData<Port>
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x000200DF File Offset: 0x0001E2DF
	public override void ApplyData(Port component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x000200E8 File Offset: 0x0001E2E8
	public override List<Stat> RetrieveStats()
	{
		List<Stat> list = new List<Stat>();
		list.AddRange(this.droneData.RetrieveStats());
		list.Add(new StatFloat(this.doorTimer, StatType.DroneDoorSpeed));
		return list;
	}

	// Token: 0x0400042E RID: 1070
	public EntityData droneData;

	// Token: 0x0400042F RID: 1071
	public Vector2 doorOpenPosition = new Vector2(2f, 0f);

	// Token: 0x04000430 RID: 1072
	public Vector2 doorClosedPosition = new Vector2(0f, 0f);

	// Token: 0x04000431 RID: 1073
	public Vector2 doorOpenScale = new Vector3(0f, 1f, 1f);

	// Token: 0x04000432 RID: 1074
	public Vector2 doorClosedScale = new Vector3(1f, 1f, 1f);

	// Token: 0x04000433 RID: 1075
	public float doorTimer = 2f;

	// Token: 0x04000434 RID: 1076
	public bool useDoorSounds;

	// Token: 0x04000435 RID: 1077
	public AudioClip openSound;

	// Token: 0x04000436 RID: 1078
	public AudioClip closeSound;
}
