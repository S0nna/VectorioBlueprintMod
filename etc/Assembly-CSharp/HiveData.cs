using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x0200011A RID: 282
public class HiveData : ComponentData<Hive>
{
	// Token: 0x06000964 RID: 2404 RVA: 0x00027B52 File Offset: 0x00025D52
	public override void ApplyData(Hive component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00027B5B File Offset: 0x00025D5B
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>
		{
			new StatFloat(this.spawnRange, StatType.HiveRange),
			new StatFloat((float)this.damageThreshold, StatType.HiveThreshold)
		};
	}

	// Token: 0x040005C5 RID: 1477
	public bool isCoreCell;

	// Token: 0x040005C6 RID: 1478
	public Sprite markerIcon;

	// Token: 0x040005C7 RID: 1479
	[SerializeField]
	public List<HiveData.HiveCell> cells;

	// Token: 0x040005C8 RID: 1480
	public int damageThreshold = 10;

	// Token: 0x040005C9 RID: 1481
	public float spawnRange = 50f;

	// Token: 0x040005CA RID: 1482
	public float colliderSize = 1.5f;

	// Token: 0x040005CB RID: 1483
	public List<EntityData> spawns = new List<EntityData>();

	// Token: 0x0200011B RID: 283
	[Serializable]
	public class HiveCell
	{
		// Token: 0x040005CC RID: 1484
		public EntityData hiveData;

		// Token: 0x040005CD RID: 1485
		public Vector2 position;
	}
}
