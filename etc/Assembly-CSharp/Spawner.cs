using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x020001F5 RID: 501
public class Spawner : MonoBehaviour
{
	// Token: 0x06000F53 RID: 3923 RVA: 0x00047FF0 File Offset: 0x000461F0
	public void Update()
	{
		if (this.trackCooldown > 0f)
		{
			this.trackCooldown -= Time.deltaTime;
			return;
		}
		this.trackCooldown = this.cooldown;
		Spawner.Spawnable spawnable = this.enemyIDs[Random.Range(0, this.enemyIDs.Count)];
		Vector2 position = new Vector2(base.transform.position.x + Random.Range(-this.xSpawnRange, this.xSpawnRange), base.transform.position.y);
		EntityCreationData creationData = EventBuilder.BuildCreationData(spawnable.entityID, spawnable.factionID, position, SyncType.None);
		EventBuilder.ApplyCosmeticToCreationData(ref creationData, spawnable.modelID);
		Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
	}

	// Token: 0x04000CAC RID: 3244
	[SerializeField]
	public List<Spawner.Spawnable> enemyIDs;

	// Token: 0x04000CAD RID: 3245
	public float cooldown;

	// Token: 0x04000CAE RID: 3246
	public float xSpawnRange;

	// Token: 0x04000CAF RID: 3247
	private float trackCooldown;

	// Token: 0x020001F6 RID: 502
	[Serializable]
	public class Spawnable
	{
		// Token: 0x04000CB0 RID: 3248
		public string entityID;

		// Token: 0x04000CB1 RID: 3249
		public string modelID;

		// Token: 0x04000CB2 RID: 3250
		public string factionID;
	}
}
