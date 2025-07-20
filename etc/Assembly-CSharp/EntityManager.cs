using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Serialization;
using Vectorio.Stats;
using Vectorio.Utilities;

// Token: 0x02000066 RID: 102
[DefaultExecutionOrder(0)]
public class EntityManager : Singleton<EntityManager>
{
	// Token: 0x060004BD RID: 1213 RVA: 0x00018AF3 File Offset: 0x00016CF3
	public bool HasEntity(uint runtimeID)
	{
		return this._entities.ContainsKey(runtimeID);
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060004BE RID: 1214 RVA: 0x00018B01 File Offset: 0x00016D01
	public Dictionary<uint, Entity> Entities
	{
		get
		{
			return this._entities;
		}
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00018B09 File Offset: 0x00016D09
	public bool TryGetEntity(uint runtimeID, out Entity entity)
	{
		return this._entities.TryGetValue(runtimeID, out entity);
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00018B18 File Offset: 0x00016D18
	public bool IsClearingEntities()
	{
		return this._clearingEntities;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00018B20 File Offset: 0x00016D20
	public void ClearAllEntities()
	{
		try
		{
			Singleton<Gamemode>.Instance.ForceDisableSounds = true;
			this._clearingEntities = true;
			foreach (KeyValuePair<uint, Entity> keyValuePair in this._entities)
			{
				if (!keyValuePair.Value.Has_EFlag_IsDead)
				{
					this.Destroy(keyValuePair.Value, true);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(string.Concat(new string[]
			{
				"[ENTITY MANAGER] Clearing failed...\nMessage: ",
				ex.Message,
				"\n\nStack Trace: ",
				ex.StackTrace,
				"\n\n"
			}));
		}
		finally
		{
			Singleton<Gamemode>.Instance.ForceDisableSounds = false;
			this._clearingEntities = false;
			this._entities.Clear();
		}
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00018C10 File Offset: 0x00016E10
	public void Setup()
	{
		if (base.gameObject.GetComponent<StatManager>() == null)
		{
			base.gameObject.AddComponent<StatManager>();
		}
		if (base.gameObject.GetComponent<EntityUtilities>() == null)
		{
			base.gameObject.AddComponent<EntityUtilities>();
		}
		if (base.gameObject.GetComponent<OutpostCreator>() == null)
		{
			base.gameObject.AddComponent<OutpostCreator>();
		}
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00018C7C File Offset: 0x00016E7C
	public void Update()
	{
		if (Singleton<Gamemode>.Instance.IsPaused)
		{
			return;
		}
		this._errorList.Clear();
		for (int i = 0; i < this._updatingComponents.Count; i++)
		{
			IUpdateable updateable = this._updatingComponents[i];
			try
			{
				updateable.Tick(Time.deltaTime);
			}
			catch (Exception item)
			{
				this._errorList.Add(new ValueTuple<IUpdateable, Exception>(updateable, item));
			}
		}
		this.ProcessErrors();
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			this.UpdateLocalCallbacks();
		}
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00018D10 File Offset: 0x00016F10
	private void ProcessErrors()
	{
		foreach (ValueTuple<IUpdateable, Exception> valueTuple in this._errorList)
		{
			IUpdateable item = valueTuple.Item1;
			Exception item2 = valueTuple.Item2;
			if (((item != null) ? item.Entity : null) != null)
			{
				if (item.Entity.Has_EComponent<Drone>())
				{
					this.ProcessDroneError(item.Entity.Get_EComponent<Drone>(false), item2);
				}
				else
				{
					this.ProcessGenericEntityError(item.Entity, item2);
				}
			}
			else
			{
				Debug.Log("[ENTITY MANAGER] Ran into an unknown error during update cycle.");
			}
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00018DB8 File Offset: 0x00016FB8
	private void ProcessDroneError(Drone drone, Exception ex)
	{
		if (drone.Target != null)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"[ENTITY MANAGER] A drone encountered an error when interacting with ",
				drone.Target.name,
				". The entity has been terminated and the drone has been sent back to its port.\n\nMessage: ",
				ex.Message,
				"\n\nStack Trace: ",
				ex.StackTrace
			}));
			this.Destroy(drone.Target.RuntimeID, true);
		}
		else
		{
			Debug.LogWarning("[ENTITY MANAGER] A drone encountered an error with an unknown entity.\n\nMessage: " + ex.Message + "\n\nStack Trace: " + ex.StackTrace);
		}
		drone.ReturnToParent();
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00018E58 File Offset: 0x00017058
	private void ProcessGenericEntityError(Entity entity, Exception ex)
	{
		Debug.LogWarning(string.Concat(new string[]
		{
			"[ENTITY MANAGER] Entity with ID ",
			entity.ID,
			" encountered an error in one of its components, and has been terminated.\n\nMessage: ",
			ex.Message,
			"\n\nStack Trace: ",
			ex.StackTrace
		}));
		this.Destroy(entity.RuntimeID, true);
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00018EB8 File Offset: 0x000170B8
	public bool C_CreateEntity(C_EntityCreationData compressedData, bool asHost)
	{
		EntityCreationData entityCreationData = DataProcessor.DecompressAndDeserializeObject<EntityCreationData>(compressedData.CreationData);
		if (entityCreationData != null)
		{
			entityCreationData.RuntimeID = compressedData.RuntimeID;
			if (!asHost)
			{
				entityCreationData.Checks = (CheckFlags)0;
			}
			return this.CreateEntity(entityCreationData);
		}
		Debug.Log("[ENTITY MANAGER] Ran into an issue while decompressing creation data!");
		return false;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00018F00 File Offset: 0x00017100
	public bool CreateEntity(EntityCreationData creationData)
	{
		if (!creationData.HasRuntimeID || creationData.RuntimeID == 0U)
		{
			Debug.Log("[ENTITY MANAGER] Creation data was passed in without a runtime ID, aborting!");
			return false;
		}
		EntityData entityData = Library.RequestData<EntityData>(creationData.EntityID);
		Vector2 vector = new Vector2(creationData.PosX, creationData.PosY);
		if (!this.IsValid(entityData, creationData.Checks, vector, creationData.IsBlueprint))
		{
			return false;
		}
		if (this._entities.ContainsKey(creationData.RuntimeID))
		{
			Entity entity;
			if (this.TryGetEntity(creationData.RuntimeID, out entity))
			{
				Debug.Log(string.Concat(new string[]
				{
					"[ENTITY MANAGER] An entity with ID ",
					entityData.ID,
					" attempted to request ID ",
					creationData.RuntimeID.ToString(),
					", which is already assigned to entity ",
					entity.name,
					"!"
				}));
			}
			else
			{
				Debug.Log(string.Concat(new string[]
				{
					"[ENTITY MANAGER] An entity with ID ",
					entityData.ID,
					" attempted to request ID ",
					creationData.RuntimeID.ToString(),
					", which is still registered in the entity manager. However, the existing entity could not be found!"
				}));
			}
			return false;
		}
		bool flag = true;
		Entity entity2;
		if (creationData.IsBlueprint)
		{
			if (this._blueprintPool.ContainsKey(entityData) && this._blueprintPool[entityData].Count > 0)
			{
				entity2 = this._blueprintPool[entityData].Pop();
				entity2.gameObject.SetActive(true);
				entity2.transform.SetParent(null);
				flag = (entity2.GetModel.ID != creationData.ModelID);
			}
			else
			{
				entity2 = new GameObject(entityData.ID).AddComponent<Entity>();
			}
		}
		else if (this._entityPool.ContainsKey(entityData) && this._entityPool[entityData].Count > 0)
		{
			entity2 = this._entityPool[entityData].Pop();
			entity2.gameObject.SetActive(true);
			entity2.transform.SetParent(null);
			flag = (entity2.GetModel.ID != creationData.ModelID);
		}
		else
		{
			entity2 = new GameObject(entityData.ID).AddComponent<Entity>();
		}
		if (!creationData.ApplyFlagsPostCreation)
		{
			entity2.Parse_EFlags(creationData.EntityFlags);
		}
		entity2.SetRuntimeID(creationData.RuntimeID);
		entity2.FactionID = creationData.FactionID;
		this._entities.Add(entity2.RuntimeID, entity2);
		entity2.transform.position = vector;
		if (creationData.IsBlueprint)
		{
			entity2.SetModel(Singleton<ModelConstructor>.Instance.BuildBlueprintModel(entityData.GetModel(creationData.ModelID), entity2.transform));
			entityData.SetupAsBlueprint(ref entity2, creationData);
			if ((creationData.Flags & CreationFlags.UseCallback) != (CreationFlags)0)
			{
				this.ReturnEntity(entity2, creationData.Callback);
			}
		}
		else
		{
			if (flag)
			{
				if (creationData.ModelID == null)
				{
					creationData.ModelID = "";
				}
				Singleton<ModelConstructor>.Instance.GetModel(entityData.GetModel(creationData.ModelID), entity2, entityData.sortingLayer, false);
			}
			if ((creationData.Flags & CreationFlags.UseAccent) != (CreationFlags)0)
			{
				entity2.GetModel.ApplyAccent(new Accent(creationData.Accent));
			}
			entityData.Setup(ref entity2, creationData.FromSave);
			if ((creationData.Flags & CreationFlags.UseVariables) != (CreationFlags)0)
			{
				entity2.Apply_ECVariableContainers(creationData.Variables);
			}
			if ((creationData.Flags & CreationFlags.UseCosts) != (CreationFlags)0)
			{
				Singleton<ResourceManager>.Instance.ApplyCosts(entity2);
			}
			if ((creationData.Flags & CreationFlags.UseCallback) != (CreationFlags)0)
			{
				this.ReturnEntity(entity2, creationData.Callback);
			}
			if (creationData.HasPipette)
			{
				entity2.ApplyMetadata(creationData.PipetteData, true);
			}
		}
		if (creationData.ApplyFlagsPostCreation)
		{
			entity2.Parse_EFlags(creationData.EntityFlags);
		}
		return true;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00019290 File Offset: 0x00017490
	public bool RunDamageEvent(EntityDamageEvent damageEvent)
	{
		Entity entity;
		if (!this.TryGetEntity(damageEvent.EntityID, out entity))
		{
			return false;
		}
		HealthComponent healthComponent = entity.Get_EComponent<HealthComponent>(false);
		if (!(healthComponent != null))
		{
			return false;
		}
		if (damageEvent.DamagerID == 0U)
		{
			healthComponent.Damage(damageEvent.Damage, null);
			return true;
		}
		Entity damager;
		if (!this.TryGetEntity(damageEvent.DamagerID, out damager))
		{
			Debug.Log("[ENTITY MANAGER] No entity with ID " + damageEvent.EntityID.ToString() + " exists for damage event!");
			return false;
		}
		healthComponent.Damage(damageEvent.Damage, damager);
		return true;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00019318 File Offset: 0x00017518
	public bool RunDestroyEvent(EntityDestroyEvent destroyEvent)
	{
		Entity entity;
		if (!this.TryGetEntity(destroyEvent.RuntimeID, out entity))
		{
			Debug.Log("[ENTITY MANAGER] No entity with ID " + destroyEvent.RuntimeID.ToString() + " exists for destroy event!");
			return false;
		}
		entity.DestroyEntity(destroyEvent.Recycle);
		return true;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00019363 File Offset: 0x00017563
	public void RegisterUpdatingComponent(IUpdateable component)
	{
		if (!component.IsUpdating)
		{
			this._updatingComponents.Add(component);
			component.IsUpdating = true;
			component.OnStartUpdating();
		}
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00019386 File Offset: 0x00017586
	public void UnregisterUpdatingComponent(IUpdateable component)
	{
		if (component.IsUpdating)
		{
			this._updatingComponents.Remove(component);
			component.IsUpdating = false;
			component.OnStopUpdating();
		}
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000193AC File Offset: 0x000175AC
	public void QueueCreationEvent(EntityCreationData creationData)
	{
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			uint runtimeID = this.runtimeIDTracker + 1U;
			this.runtimeIDTracker = runtimeID;
			creationData.RuntimeID = runtimeID;
			this.CreateEntity(creationData);
			return;
		}
		switch (creationData.SyncType)
		{
		case SyncType.None:
			if (creationData.HasRuntimeID)
			{
				this.CreateEntity(creationData);
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				this._CreateEntityAsHost(creationData, false, null);
				return;
			}
			break;
		case SyncType.ClientInitiated:
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
			{
				NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueCreationEvent(creationData);
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				this._CreateEntityAsHost(creationData, false, null);
				return;
			}
			break;
		case SyncType.ServerInitiated:
			if (NetworkPlayerManager.IS_HOST)
			{
				if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
				{
					C_EntityCreationData compressedData = EventBuilder.BuildCompressedCreationData(creationData);
					ServerSingleton<ServerStateManager>.Instance.Srv_QueueCompressedCreationEvent(compressedData);
					return;
				}
				this._CreateEntityAsHost(creationData, false, null);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00019470 File Offset: 0x00017670
	private void _CreateEntityAsHost(EntityCreationData creationData, bool playSound = false, AudioClip sound = null)
	{
		uint num = ServerSingleton<ServerSyncManager>.Instance.RequestNewRuntimeID();
		creationData.RuntimeID = num;
		if (!this.CreateEntity(creationData))
		{
			ServerSingleton<ServerSyncManager>.Instance.ReleaseRuntimeID(num);
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x000194A4 File Offset: 0x000176A4
	public void QueueDamageEvent(EntityDamageEvent data, SyncType syncType)
	{
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			this.RunDamageEvent(data);
			return;
		}
		if (syncType != SyncType.ClientInitiated)
		{
			if (syncType != SyncType.ServerInitiated)
			{
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
				{
					ServerSingleton<ServerStateManager>.Instance.Srv_QueueDamageEvent(data);
					return;
				}
				this.RunDamageEvent(data);
			}
		}
		else
		{
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
			{
				NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueDamageEvent(data);
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				this.RunDamageEvent(data);
				return;
			}
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00019514 File Offset: 0x00017714
	public void QueueDestroyEvent(EntityDestroyEvent data, SyncType syncType)
	{
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			this.RunDestroyEvent(data);
			return;
		}
		if (syncType != SyncType.ClientInitiated)
		{
			if (syncType != SyncType.ServerInitiated)
			{
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
				{
					ServerSingleton<ServerStateManager>.Instance.Srv_QueueDestroyEvent(data);
					return;
				}
				this.RunDestroyEvent(data);
			}
		}
		else
		{
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
			{
				NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueDestroyEvent(data);
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				this.RunDestroyEvent(data);
				return;
			}
		}
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00019584 File Offset: 0x00017784
	public void QueueEntityCallback(EntityCallbackEvent data)
	{
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			this.LocalQueueCallback(data);
			return;
		}
		if (NetworkPlayerManager.IS_HOST)
		{
			ServerSingleton<ServerStateManager>.Instance.Srv_QueueCallbackEvent(data);
		}
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x000195AC File Offset: 0x000177AC
	public void CancelEntityCallback(uint id)
	{
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			this.LocalCancelCallback(id);
			return;
		}
		if (NetworkPlayerManager.IS_HOST)
		{
			ServerSingleton<ServerStateManager>.Instance.Srv_CancelCallbackEvent(id);
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x000195D4 File Offset: 0x000177D4
	public void QueueSyncEvent(SyncEvent data, SyncType syncType)
	{
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			this.RunSyncEvent(data);
			return;
		}
		if (syncType != SyncType.ClientInitiated)
		{
			if (syncType != SyncType.ServerInitiated)
			{
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
				{
					ServerSingleton<ServerStateManager>.Instance.Srv_QueueSyncEvent(data);
					return;
				}
				this.RunSyncEvent(data);
			}
		}
		else
		{
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
			{
				NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueSyncEvent(data);
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				this.RunSyncEvent(data);
				return;
			}
		}
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00019644 File Offset: 0x00017844
	private void LocalQueueCallback(EntityCallbackEvent data)
	{
		if (!data.IsFinished)
		{
			LinkedListNode<EntityCallbackEvent> linkedListNode = new LinkedListNode<EntityCallbackEvent>(data);
			this._callbackList.AddLast(linkedListNode);
			this._callbackDict[data.EntityID] = linkedListNode;
		}
		this.RunCallbackEvent(data);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00019688 File Offset: 0x00017888
	private void UpdateLocalCallbacks()
	{
		LinkedListNode<EntityCallbackEvent> next;
		for (LinkedListNode<EntityCallbackEvent> linkedListNode = this._callbackList.First; linkedListNode != null; linkedListNode = next)
		{
			next = linkedListNode.Next;
			EntityCallbackEvent value = linkedListNode.Value;
			value.Time -= Time.deltaTime;
			if (value.Time <= 0f)
			{
				value.IsFinished = true;
				this._callbackList.Remove(linkedListNode);
				this._callbackDict.Remove(value.EntityID);
				this.RunCallbackEvent(value);
			}
			else
			{
				linkedListNode.Value = value;
			}
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0001970C File Offset: 0x0001790C
	private void LocalCancelCallback(uint id)
	{
		LinkedListNode<EntityCallbackEvent> node;
		if (this._callbackDict.TryGetValue(id, out node))
		{
			this._callbackList.Remove(node);
			this._callbackDict.Remove(id);
		}
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00019744 File Offset: 0x00017944
	public void QueueMetadataEvent(EntityMetadataEvent data, SyncType syncType)
	{
		if (Singleton<Gamemode>.Instance.IsOfflineScene)
		{
			this.RunMetadataEvent(data);
			return;
		}
		switch (syncType)
		{
		case SyncType.None:
			this.RunMetadataEvent(data);
			return;
		case SyncType.ClientInitiated:
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
			{
				NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueMetadataEvent(data);
				return;
			}
			if (NetworkPlayerManager.IS_HOST)
			{
				this.RunMetadataEvent(data);
				return;
			}
			break;
		case SyncType.ServerInitiated:
			if (NetworkPlayerManager.IS_HOST)
			{
				if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
				{
					NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueMetadataEvent(data);
					return;
				}
				this.RunMetadataEvent(data);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x000197C8 File Offset: 0x000179C8
	public bool C_RunMetadataEvent(C_EntityMetadataEvent c_metadataEvent)
	{
		Entity entity;
		if (!this.TryGetEntity(c_metadataEvent.RuntimeID, out entity))
		{
			Debug.Log("[ENTITY MANAGER] No entity with ID " + c_metadataEvent.RuntimeID.ToString() + " exists for metadata event!");
			return false;
		}
		EntityMetadataEvent entityMetadataEvent = new EntityMetadataEvent
		{
			RuntimeID = c_metadataEvent.RuntimeID,
			Metadata = DataProcessor.DecompressAndDeserializeObject<EntityMetadata>(c_metadataEvent.Metadata),
			AsPipette = c_metadataEvent.AsPipette
		};
		entity.ApplyMetadata(entityMetadataEvent.Metadata, entityMetadataEvent.AsPipette);
		return true;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0001984C File Offset: 0x00017A4C
	public bool RunMetadataEvent(EntityMetadataEvent metadataEvent)
	{
		Entity entity;
		if (!this.TryGetEntity(metadataEvent.RuntimeID, out entity))
		{
			Debug.Log("[ENTITY MANAGER] No entity with ID " + metadataEvent.RuntimeID.ToString() + " exists for metadata event!");
			return false;
		}
		entity.ApplyMetadata(metadataEvent.Metadata, metadataEvent.AsPipette);
		return true;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x000198A0 File Offset: 0x00017AA0
	public bool RunCallbackEvent(EntityCallbackEvent callbackEvent)
	{
		Entity entity;
		if (!this.TryGetEntity(callbackEvent.EntityID, out entity))
		{
			Debug.Log("[ENTITY MANAGER] No entity with ID " + callbackEvent.EntityID.ToString() + " exists for end callback event!");
			return false;
		}
		ICallbackListener callbackListener = entity.Get_EComponentInterface<ICallbackListener>(callbackEvent.ComponentID);
		if (callbackListener == null)
		{
			Debug.Log("[ENTITY MANAGER] Provided entity does not have a component that inherits from ICallbackListener!");
			return false;
		}
		if (!callbackEvent.IsFinished)
		{
			callbackListener.IsUpdating = true;
			callbackListener.OnStartCallback(callbackEvent);
		}
		else
		{
			callbackListener.IsUpdating = false;
			callbackListener.OnEndCallback(callbackEvent);
		}
		return true;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00019924 File Offset: 0x00017B24
	public bool RunSyncEvent(SyncEvent syncEvent)
	{
		if (!syncEvent.IsEntity)
		{
			return true;
		}
		Entity entity;
		if (!this.TryGetEntity(syncEvent.RuntimeID, out entity))
		{
			Debug.Log("[ENTITY MANAGER] No entity with ID " + syncEvent.RuntimeID.ToString() + " exists for end callback event!");
			return false;
		}
		ISyncListener syncListener = entity.Get_EComponentInterface<ISyncListener>(syncEvent.ComponentID);
		if (syncListener == null)
		{
			Debug.Log("[ENTITY MANAGER] Provided entity does not have a component that inherits from ICallbackListener!");
			return false;
		}
		syncListener.OnSyncDataReceived(syncEvent);
		return true;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00019994 File Offset: 0x00017B94
	private void ReturnEntity(Entity entity, OnCreationCallback callback)
	{
		if (callback.Type != CallbackType.EntityCallback)
		{
			switch (callback.ID)
			{
			case 2U:
				Singleton<HeatManager>.Instance.OnEntityCreated(entity);
				return;
			case 4U:
				Singleton<WorldGenerator>.Instance.WorldSpawner.OnEntityCreated(entity);
				return;
			case 5U:
				Singleton<OutpostGenerator>.Instance.OnEntityCreated(entity);
				return;
			case 7U:
				Singleton<MenuScene>.Instance.OnEntityCreated(entity, callback.Index);
				return;
			}
			Debug.Log("[ENTITY MANAGER] No ID with " + callback.ID.ToString() + " exists in callback registry");
			return;
		}
		Entity entity2;
		if (!this.TryGetEntity(callback.ID, out entity2))
		{
			Debug.Log("[ENTITY MANAGER] No entity with ID " + callback.ID.ToString() + " exists for end callback event!");
			return;
		}
		EntityComponent entityComponent;
		if (entity2.TryGet_EComponent(callback.Index, out entityComponent))
		{
			entityComponent.OnCreationCallback(entity);
			return;
		}
		Debug.Log("[ENTITY MANAGER] Component discrepancy across clients, recommend restarting!");
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00019A84 File Offset: 0x00017C84
	public void Destroy(uint id, bool recycle)
	{
		if (this._entities.ContainsKey(id))
		{
			if (this._entities[id] != null)
			{
				this.Destroy(this._entities[id], recycle);
			}
			if (!this._clearingEntities)
			{
				this._entities.Remove(id);
			}
		}
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00019ADC File Offset: 0x00017CDC
	private void Destroy(Entity entity, bool recycle)
	{
		Singleton<Events>.Instance.onEntityDestroyed.Invoke(entity);
		Singleton<ResourceManager>.Instance.RevertCosts(entity);
		entity.OnReset();
		if (recycle)
		{
			EntityData data = entity.GetData();
			if (entity.Has_EFlag_IsBlueprint)
			{
				if (!this._blueprintPool.ContainsKey(data))
				{
					this._blueprintPool.Add(data, new Stack<Entity>());
				}
				this._blueprintPool[data].Push(entity);
			}
			else
			{
				if (!this._entityPool.ContainsKey(data))
				{
					this._entityPool.Add(data, new Stack<Entity>());
				}
				this._entityPool[data].Push(entity);
			}
			entity.gameObject.SetActive(false);
			entity.transform.SetParent(base.transform);
			entity.gameObject.transform.localPosition = Vector2.zero;
			return;
		}
		Object.Destroy(entity.gameObject);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00019BC8 File Offset: 0x00017DC8
	public bool IsValid(EntityData data, CheckFlags flags, Vector2 position, bool isBlueprint)
	{
		if (!isBlueprint && this.IsFlagSet(flags, CheckFlags.CheckCosts) && !Singleton<ResourceManager>.Instance.CheckEntityCosts(data, false))
		{
			return false;
		}
		CheckType checkType = data.checkType;
		if (checkType != CheckType.All)
		{
			if (checkType != CheckType.Decoration)
			{
				return true;
			}
		}
		else
		{
			if (this.IsFlagSet(flags, CheckFlags.CheckForEntity))
			{
				RaycastHit2D raycastHit2D = Physics2D.Raycast(position, -Vector3.forward, float.PositiveInfinity, this._entityLayers);
				if (raycastHit2D.collider != null && raycastHit2D.collider.GetComponent<Entity>() != null)
				{
					return false;
				}
			}
			if (!this.IsFlagSet(flags, CheckFlags.CheckBuildingComponents))
			{
				return true;
			}
			bool flag = false;
			if (!isBlueprint && data.HasComponent<ClaimerData>())
			{
				int range = data.GetComponent<ClaimerData>().range;
				if (!Singleton<TileGrid>.Instance.IsClaimerValid(position, range))
				{
					return false;
				}
				flag = true;
			}
			if (!data.HasComponent<BuildingData>())
			{
				return true;
			}
			BuildingData component = data.GetComponent<BuildingData>();
			List<Vector2Int> list = new List<Vector2Int>();
			Utilities.CalculateBuildingCells(ref list, position, component.Width, component.Height);
			if (!Singleton<TileGrid>.Instance.AreCellsValid(list))
			{
				return false;
			}
			if (!isBlueprint && this.IsFlagSet(flags, CheckFlags.CheckForClaim) && !flag && !Singleton<TileGrid>.Instance.AreCellsClaimed(list))
			{
				return false;
			}
			if (!Singleton<Gamemode>.Instance.EnforceTileRestrictions)
			{
				return true;
			}
			if (component.RequireAnyResource)
			{
				if (!Singleton<TileGrid>.Instance.HasResources(list, true))
				{
					return false;
				}
				return true;
			}
			else
			{
				if (component.RequiredResources == null || component.RequiredResources.Count <= 0)
				{
					return true;
				}
				using (List<string>.Enumerator enumerator = component.RequiredResources.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string resourceID = enumerator.Current;
						if (!Singleton<TileGrid>.Instance.HasResources(list, resourceID, true))
						{
							return false;
						}
					}
					return true;
				}
			}
		}
		Vector2Int coords = Utilities.ConvertWorldPositionToCell(position);
		if (this.IsFlagSet(flags, CheckFlags.CheckForClaim) && !Singleton<TileGrid>.Instance.IsCellClaimed(coords))
		{
			return false;
		}
		if (data.HasComponent<TilePlacerData>())
		{
			TilePlacerData component2 = data.GetComponent<TilePlacerData>();
			CellData cell = Singleton<TileGrid>.Instance.GetCell(coords, true);
			if (cell.HasTileDesign && cell.TileDesign.data == component2.tileData)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00019E04 File Offset: 0x00018004
	private bool IsFlagSet(CheckFlags flags, CheckFlags flagToCheck)
	{
		return (flags & flagToCheck) == flagToCheck;
	}

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	protected LayerMask _entityLayers;

	// Token: 0x040002A2 RID: 674
	public GameObject debugObject;

	// Token: 0x040002A3 RID: 675
	public bool useCargoDroneBandaid = true;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private Dictionary<EntityData, Stack<Entity>> _entityPool = new Dictionary<EntityData, Stack<Entity>>();

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private Dictionary<EntityData, Stack<Entity>> _blueprintPool = new Dictionary<EntityData, Stack<Entity>>();

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private Dictionary<uint, Entity> _entities = new Dictionary<uint, Entity>();

	// Token: 0x040002A7 RID: 679
	private Dictionary<uint, LinkedListNode<EntityCallbackEvent>> _callbackDict = new Dictionary<uint, LinkedListNode<EntityCallbackEvent>>();

	// Token: 0x040002A8 RID: 680
	private LinkedList<EntityCallbackEvent> _callbackList = new LinkedList<EntityCallbackEvent>();

	// Token: 0x040002A9 RID: 681
	private bool _clearingEntities;

	// Token: 0x040002AA RID: 682
	private List<IUpdateable> _updatingComponents = new List<IUpdateable>();

	// Token: 0x040002AB RID: 683
	[TupleElementNames(new string[]
	{
		"component",
		"exception"
	})]
	private List<ValueTuple<IUpdateable, Exception>> _errorList = new List<ValueTuple<IUpdateable, Exception>>();

	// Token: 0x040002AC RID: 684
	private uint runtimeIDTracker;
}
