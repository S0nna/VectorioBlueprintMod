using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000AE RID: 174
public class DecryptorData : ComponentData<Decryptor>
{
	// Token: 0x0600067C RID: 1660 RVA: 0x0001FAC9 File Offset: 0x0001DCC9
	public override void ApplyData(Decryptor component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0001FAD2 File Offset: 0x0001DCD2
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.spawnRange, StatType.DecryptorRange),
			new StatFloat(this.spawnCooldown, StatType.DecryptorSpawnRate)
		};
	}

	// Token: 0x040003EB RID: 1003
	public Sprite markerIcon;

	// Token: 0x040003EC RID: 1004
	public float spawnCooldown;

	// Token: 0x040003ED RID: 1005
	public float spawnRange;
}
