using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000B9 RID: 185
public class HubData : ComponentData<Hub>
{
	// Token: 0x060006A2 RID: 1698 RVA: 0x0001FF71 File Offset: 0x0001E171
	public override void ApplyData(Hub component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0001FF7A File Offset: 0x0001E17A
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatInt(this.startingPower, StatType.HubPower)
		};
	}

	// Token: 0x04000413 RID: 1043
	[SerializeField]
	public List<HubData.Spawnable> spawnables;

	// Token: 0x04000414 RID: 1044
	public int startingPower;

	// Token: 0x04000415 RID: 1045
	public GameObject effect;

	// Token: 0x020000BA RID: 186
	[Serializable]
	public class Spawnable
	{
		// Token: 0x04000416 RID: 1046
		public EntityData data;

		// Token: 0x04000417 RID: 1047
		public string modelID;

		// Token: 0x04000418 RID: 1048
		public Vector2 position;
	}
}
