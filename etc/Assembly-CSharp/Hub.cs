using System;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x0200011C RID: 284
public class Hub : EntityComponent, IComponent<Hub, HubData>
{
	// Token: 0x06000968 RID: 2408 RVA: 0x00027BBA File Offset: 0x00025DBA
	public HubData GetData()
	{
		return this._hubData;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00027BC4 File Offset: 0x00025DC4
	public void OnInitialize(HubData data)
	{
		this._hubData = data;
		base.Entity.Set_EFlag_IsEditable(false);
		if (RegionManager.FIRST_LOAD)
		{
			foreach (HubData.Spawnable spawnable in this._hubData.spawnables)
			{
				Vector2 position = new Vector2(base.transform.position.x + spawnable.position.x, base.transform.position.y + spawnable.position.y);
				EntityCreationData creationData = EventBuilder.BuildCreationData(spawnable.data.ID, base.Entity.FactionID, position, SyncType.None);
				EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.EntityCallback, base.RuntimeID, base.ComponentIndex);
				Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
			}
		}
		if (data.effect != null)
		{
			this._effect = Object.Instantiate<GameObject>(data.effect, base.transform.position, Quaternion.identity);
		}
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00027CE4 File Offset: 0x00025EE4
	public override void OnCreationCallback(Entity entity)
	{
		if (entity != null)
		{
			entity.Set_EFlag_IsEditable(false);
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00027CF6 File Offset: 0x00025EF6
	public override void OnSpawn(bool fromSave)
	{
		Singleton<ResourceManager>.Instance.AddPowerStorage(this._hubData.startingPower);
		Singleton<FactionManager>.Instance.AddHub(base.FactionID, this);
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x00027D20 File Offset: 0x00025F20
	public override void OnReset()
	{
		if (this._effect != null)
		{
			Object.Destroy(this._effect);
		}
		Singleton<ResourceManager>.Instance.RemovePowerStorage(this._hubData.startingPower);
		Singleton<FactionManager>.Instance.RemoveHub(base.FactionID, this);
	}

	// Token: 0x040005CE RID: 1486
	private HubData _hubData;

	// Token: 0x040005CF RID: 1487
	private GameObject _effect;
}
