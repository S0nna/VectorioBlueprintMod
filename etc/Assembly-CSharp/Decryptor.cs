using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;

// Token: 0x02000102 RID: 258
public class Decryptor : EntityComponent, IUpdateable, IComponent<Decryptor, DecryptorData>, ICallbackListener, IMouseListener
{
	// Token: 0x06000853 RID: 2131 RVA: 0x00024CC4 File Offset: 0x00022EC4
	public DecryptorData GetData()
	{
		return this._decryptorData;
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000854 RID: 2132 RVA: 0x00024CCC File Offset: 0x00022ECC
	public ResearchTechData Tech
	{
		get
		{
			return this._tech;
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00024CD4 File Offset: 0x00022ED4
	public void SetTech(ResearchTechData tech)
	{
		this._tech = tech;
		Singleton<Research>.Instance.AddDecryptor(this);
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x00024CE8 File Offset: 0x00022EE8
	public float DecryptionTimer
	{
		get
		{
			return this._decryptionTimer;
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00024CF0 File Offset: 0x00022EF0
	public void OnInitialize(DecryptorData data)
	{
		this._decryptorData = data;
		base.Entity.Set_EFlag_IsTargetable(false);
		base.Entity.Set_EFlag_IsWorldFeature(true);
		base.Entity.Set_EFlag_IsEditable(false);
		base.Entity.Set_EFlag_IsInvincible(true);
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00024D2C File Offset: 0x00022F2C
	public override void OnSpawn(bool fromSave)
	{
		if (base.Entity.Has_EComponent<HealthComponent>())
		{
			base.Entity.Get_EComponent<HealthComponent>(false).OnDamage += this.OnDamage;
		}
		Singleton<MarkerHandler>.Instance.CreateMarker(base.Entity.RuntimeID.ToString(), base.transform.position, new Vector2(6f, 6f), new Vector2(0.4f, 0.4f), this._decryptorData.markerIcon, "DECRYPTOR", "");
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00024DC4 File Offset: 0x00022FC4
	public override void ApplyVariableContainer(VariableContainer variableContainer)
	{
		string text;
		if (!variableContainer.TryGetString(0, out text))
		{
			Debug.Log("[DECRYPTOR] No string variable with ID 0 exists!");
			return;
		}
		ResearchTechData researchTechData = Library.RequestData<ResearchTechData>(text);
		if (researchTechData != null)
		{
			this.SetTech(researchTechData);
			return;
		}
		Debug.Log("[DECRYPTOR] The provided tech ID " + text + " is not valid!");
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00024E14 File Offset: 0x00023014
	public void Tick(float time)
	{
		if (this._decryptionTimer <= 0f)
		{
			this.FinishProcess();
			return;
		}
		this._decryptionTimer -= Time.deltaTime;
		this._spawnTimer -= Time.deltaTime;
		if (this._spawnTimer <= 0f)
		{
			List<HeatManager.Enemy> activeEnemySpawns = Singleton<HeatManager>.Instance.GetActiveEnemySpawns();
			this.SpawnEnemy(activeEnemySpawns[Random.Range(0, activeEnemySpawns.Count)]);
			this._spawnTimer = this._decryptorData.spawnCooldown;
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00024E9C File Offset: 0x0002309C
	public void SpawnEnemy(HeatManager.Enemy enemy)
	{
		Vector2 spawnPosition = this.GetSpawnPosition();
		EntityCreationData creationData = EventBuilder.BuildCreationData(enemy.UnitData.ID, Singleton<HeatManager>.Instance.FactionData.ID, spawnPosition, SyncType.ServerInitiated);
		EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(Singleton<HeatManager>.Instance.FactionData.accent));
		EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.EntityCallback, base.RuntimeID, base.ComponentIndex);
		Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00024F0C File Offset: 0x0002310C
	public override void OnCreationCallback(Entity entity)
	{
		if (entity.Has_EComponent<Unit>())
		{
			Unit unit = entity.Get_EComponent<Unit>(false);
			unit.OnHitDetectorCheckFinished(base.Entity);
			unit.FaceTarget();
		}
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00024F30 File Offset: 0x00023130
	public Vector2 GetSpawnPosition()
	{
		switch (Random.Range(0, 4))
		{
		case 0:
			return new Vector2(Random.Range(base.transform.position.x - this._decryptorData.spawnRange, base.transform.position.x + this._decryptorData.spawnRange), base.transform.position.y + this._decryptorData.spawnRange);
		case 1:
			return new Vector2(base.transform.position.x + this._decryptorData.spawnRange, Random.Range(base.transform.position.y - this._decryptorData.spawnRange, base.transform.position.y + this._decryptorData.spawnRange));
		case 2:
			return new Vector2(Random.Range(base.transform.position.x - this._decryptorData.spawnRange, base.transform.position.x + this._decryptorData.spawnRange), base.transform.position.y - this._decryptorData.spawnRange);
		case 3:
			return new Vector2(base.transform.position.x - this._decryptorData.spawnRange, Random.Range(base.transform.position.y - this._decryptorData.spawnRange, base.transform.position.y + this._decryptorData.spawnRange));
		default:
			return new Vector2(Random.Range(base.transform.position.x - this._decryptorData.spawnRange, base.transform.position.x + this._decryptorData.spawnRange), base.transform.position.y + this._decryptorData.spawnRange);
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0002513A File Offset: 0x0002333A
	public void StartProcess()
	{
		Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, 0f, null));
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartCallback(EntityCallbackEvent callback)
	{
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0002515D File Offset: 0x0002335D
	public void OnEndCallback(EntityCallbackEvent callback)
	{
		this._decryptionTimer = 50f;
		this._spawnTimer = 0f;
		Singleton<EntityManager>.Instance.RegisterUpdatingComponent(this);
		Singleton<Events>.Instance.onDecryptionStarted.Invoke(this);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00025190 File Offset: 0x00023390
	public void CancelProcess()
	{
		Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
		Singleton<Events>.Instance.onDecryptionFailed.Invoke(this);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x000251AD File Offset: 0x000233AD
	public void FinishProcess()
	{
		Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(this);
		Singleton<Events>.Instance.onDecryptionFinished.Invoke(this);
		Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(base.Entity, null), SyncType.ServerInitiated);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x000251E4 File Offset: 0x000233E4
	public void OnDamage(float amount, Entity damager = null)
	{
		if (base.IsUpdating)
		{
			this.CancelProcess();
		}
		if (damager != null && !damager.Has_EFlag_IsDead && damager.Get_EComponent<Guardian>(false) == null)
		{
			Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(damager, null), SyncType.ServerInitiated);
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00025234 File Offset: 0x00023434
	public override void OnReset()
	{
		if (base.IsUpdating)
		{
			this.CancelProcess();
		}
		if (base.Entity.Has_EComponent<HealthComponent>())
		{
			base.Entity.Get_EComponent<HealthComponent>(false).OnDamage -= this.OnDamage;
		}
		Singleton<MarkerHandler>.Instance.DestroyMarker(base.Entity.RuntimeID.ToString());
		Singleton<Research>.Instance.RemoveDecryptor(this);
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x000252A1 File Offset: 0x000234A1
	public void OnMouseClick()
	{
		if (!base.IsUpdating)
		{
			Singleton<Events>.Instance.onOpenDecryption.Invoke(this);
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00003212 File Offset: 0x00001412
	public void OnMouseHover(bool toggle)
	{
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x000252A1 File Offset: 0x000234A1
	public void OnQuickEdit()
	{
		if (!base.IsUpdating)
		{
			Singleton<Events>.Instance.onOpenDecryption.Invoke(this);
		}
	}

	// Token: 0x04000569 RID: 1385
	private DecryptorData _decryptorData;

	// Token: 0x0400056A RID: 1386
	private ResearchTechData _tech;

	// Token: 0x0400056B RID: 1387
	protected float _spawnTimer;

	// Token: 0x0400056C RID: 1388
	protected float _decryptionTimer;
}
