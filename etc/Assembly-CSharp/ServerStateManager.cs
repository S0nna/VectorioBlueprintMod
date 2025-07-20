using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.PhasmaUI;
using Vectorio.Serialization;
using Vectorio.Utilities;

// Token: 0x02000177 RID: 375
public class ServerStateManager : ServerSingleton<ServerStateManager>
{
	// Token: 0x06000C33 RID: 3123 RVA: 0x00034276 File Offset: 0x00032476
	public override void Setup()
	{
		this._clientStateManager = base.GetComponent<ClientStateManager>();
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			this._eventPackage.Reset();
		}
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x00034298 File Offset: 0x00032498
	public override void OnServerTick()
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING && this._sendPackage)
		{
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER && ServerStateManager.USE_PACKAGE_COMPRESSION)
			{
				byte[] data = DataProcessor.SerializeAndCompressObject<NetworkEventPackage>(this._eventPackage);
				NetworkSingleton<ClientStateManager>.Instance.Rpc_ReceiveCompressedNetworkEventPackage(Utilities.PackData(data));
			}
			else
			{
				NetworkSingleton<ClientStateManager>.Instance.Rpc_ReceiveNetworkEventPackage(this._eventPackage);
			}
			this._eventPackage.Reset();
			this._sendPackage = false;
			if (ServerStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.Reset();
			}
		}
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00034314 File Offset: 0x00032514
	private void QueueNetworkEvent<T>(T networkEvent) where T : NetworkEventBase
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			networkEvent.Stamp();
			this._eventPackage.events.Add(networkEvent);
		}
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00034340 File Offset: 0x00032540
	public override void OnServerUpdate(float time)
	{
		LinkedListNode<EntityCallbackEvent> next;
		for (LinkedListNode<EntityCallbackEvent> linkedListNode = this._callbackList.First; linkedListNode != null; linkedListNode = next)
		{
			next = linkedListNode.Next;
			EntityCallbackEvent value = linkedListNode.Value;
			value.Time -= time;
			if (value.Time <= 0f)
			{
				value.IsFinished = true;
				this._callbackList.Remove(linkedListNode);
				this._callbackDict.Remove(value.EntityID);
				this.Srv_QueueCallbackEvent(value);
			}
			else
			{
				linkedListNode.Value = value;
			}
		}
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x000343BC File Offset: 0x000325BC
	[ServerRpc(RequireOwnership = false)]
	public void Srv_RequestWorldState(NetworkConnection connection)
	{
		this.RpcWriter___Server_Srv_RequestWorldState_328543758(connection);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x000343D4 File Offset: 0x000325D4
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueResearchChange(ResearchChangeEvent researchChangeEvent)
	{
		this.RpcWriter___Server_Srv_QueueResearchChange_676677770(researchChangeEvent);
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x000343EC File Offset: 0x000325EC
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueRegionChange(RegionChangeEvent regionChangeEvent)
	{
		this.RpcWriter___Server_Srv_QueueRegionChange_1815257947(regionChangeEvent);
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x00034404 File Offset: 0x00032604
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueCompressedCreationEvent(C_EntityCreationData compressedData)
	{
		this.RpcWriter___Server_Srv_QueueCompressedCreationEvent_68867397(compressedData);
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0003441C File Offset: 0x0003261C
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueCreationEvent(EntityCreationData creationData)
	{
		this.RpcWriter___Server_Srv_QueueCreationEvent_1973322133(creationData);
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00034434 File Offset: 0x00032634
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueMetadataEvent(C_EntityMetadataEvent metadataEvent)
	{
		this.RpcWriter___Server_Srv_QueueMetadataEvent_2654222363(metadataEvent);
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0003444C File Offset: 0x0003264C
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueDamageEvent(EntityDamageEvent damageEvent)
	{
		this.RpcWriter___Server_Srv_QueueDamageEvent_3491301233(damageEvent);
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x00034464 File Offset: 0x00032664
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueDestroyEvent(EntityDestroyEvent destroyEvent)
	{
		this.RpcWriter___Server_Srv_QueueDestroyEvent_3893339450(destroyEvent);
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0003447C File Offset: 0x0003267C
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueCallbackEvent(EntityCallbackEvent callbackEvent)
	{
		this.RpcWriter___Server_Srv_QueueCallbackEvent_2877855451(callbackEvent);
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00034494 File Offset: 0x00032694
	[ServerRpc(RequireOwnership = false)]
	public void Srv_CancelCallbackEvent(uint id)
	{
		this.RpcWriter___Server_Srv_CancelCallbackEvent_1489494155(id);
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x000344AC File Offset: 0x000326AC
	[ServerRpc(RequireOwnership = false)]
	public void Srv_QueueSyncEvent(SyncEvent syncEvent)
	{
		this.RpcWriter___Server_Srv_QueueSyncEvent_671342912(syncEvent);
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x000344EC File Offset: 0x000326EC
	public override void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyServerStateManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyServerStateManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize___Early();
		base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Srv_RequestWorldState_328543758));
		base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueResearchChange_676677770));
		base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueRegionChange_1815257947));
		base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueCompressedCreationEvent_68867397));
		base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueCreationEvent_1973322133));
		base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueMetadataEvent_2654222363));
		base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueDamageEvent_3491301233));
		base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueDestroyEvent_3893339450));
		base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueCallbackEvent_2877855451));
		base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_Srv_CancelCallbackEvent_1489494155));
		base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_Srv_QueueSyncEvent_671342912));
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0003460D File Offset: 0x0003280D
	public override void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LateServerStateManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LateServerStateManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize__Late();
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00034626 File Offset: 0x00032826
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00034634 File Offset: 0x00032834
	private void RpcWriter___Server_Srv_RequestWorldState_328543758(NetworkConnection connection)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.WriteNetworkConnection(connection);
		base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x000346DC File Offset: 0x000328DC
	public void RpcLogic___Srv_RequestWorldState_328543758(NetworkConnection connection)
	{
		Debug.Log("[SERVER] Processing world state for client " + connection.ClientId.ToString());
		byte[] data = DataProcessor.SerializeAndCompressObject<SaveData>(Singleton<SaveSystem>.Instance.GenerateWorldSave());
		this._clientStateManager.Rpc_ReceiveWorldState(connection, Utilities.PackData(data));
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00034728 File Offset: 0x00032928
	private void RpcReader___Server_Srv_RequestWorldState_328543758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		NetworkConnection connection = PooledReader0.ReadNetworkConnection();
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_RequestWorldState_328543758(connection);
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0003475C File Offset: 0x0003295C
	private void RpcWriter___Server_Srv_QueueResearchChange_676677770(ResearchChangeEvent researchChangeEvent)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___ResearchChangeEventFishNet.Serializing.Generated(researchChangeEvent);
		base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00034804 File Offset: 0x00032A04
	public void RpcLogic___Srv_QueueResearchChange_676677770(ResearchChangeEvent researchChangeEvent)
	{
		if (researchChangeEvent.ID == null || researchChangeEvent.ID == "")
		{
			Debug.Log("[SERVER] Received a bad ID for research change event.");
			return;
		}
		ResearchTechData researchTechData = Library.RequestData<ResearchTechData>(researchChangeEvent.ID);
		if (!(researchTechData != null))
		{
			Debug.Log("[SERVER] Invalid tech ID passed to server!");
			return;
		}
		UI_Singleton<UI_ResearchWindow>.Instance.SetActiveTech(researchTechData, researchTechData.costs);
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			this.QueueNetworkEvent<ResearchChangeEvent>(researchChangeEvent);
			this._sendPackage = true;
			return;
		}
		NetworkSingleton<ClientStateManager>.Instance.Rpc_ChangeResearch(researchChangeEvent);
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00034888 File Offset: 0x00032A88
	private void RpcReader___Server_Srv_QueueResearchChange_676677770(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		ResearchChangeEvent researchChangeEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ResearchChangeEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueResearchChange_676677770(researchChangeEvent);
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x000348BC File Offset: 0x00032ABC
	private void RpcWriter___Server_Srv_QueueRegionChange_1815257947(RegionChangeEvent regionChangeEvent)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___RegionChangeEventFishNet.Serializing.Generated(regionChangeEvent);
		base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00034964 File Offset: 0x00032B64
	public void RpcLogic___Srv_QueueRegionChange_1815257947(RegionChangeEvent regionChangeEvent)
	{
		Entity entity;
		if (!Singleton<EntityManager>.Instance.TryGetEntity(regionChangeEvent.GatewayID, out entity))
		{
			Debug.Log("[CSM] No entity with ID " + regionChangeEvent.GatewayID.ToString() + " exists for region change event!");
		}
		if (!(entity != null) || !entity.Has_EComponent<Gateway>())
		{
			Debug.Log("[CSM] Gateway is null or entity does not have gateway component!");
			return;
		}
		Gateway gateway = entity.Get_EComponent<Gateway>(false);
		Singleton<RegionManager>.Instance.WarpToRegion(gateway);
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			this.QueueNetworkEvent<RegionChangeEvent>(regionChangeEvent);
			this._sendPackage = true;
			return;
		}
		NetworkSingleton<ClientStateManager>.Instance.Rpc_ChangeRegion(regionChangeEvent);
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x000349F4 File Offset: 0x00032BF4
	private void RpcReader___Server_Srv_QueueRegionChange_1815257947(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		RegionChangeEvent regionChangeEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___RegionChangeEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueRegionChange_1815257947(regionChangeEvent);
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00034A28 File Offset: 0x00032C28
	private void RpcWriter___Server_Srv_QueueCompressedCreationEvent_68867397(C_EntityCreationData compressedData)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___C_EntityCreationDataFishNet.Serializing.Generated(compressedData);
		base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00034AD0 File Offset: 0x00032CD0
	public void RpcLogic___Srv_QueueCompressedCreationEvent_68867397(C_EntityCreationData compressedData)
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			Vector2 position = new Vector2(compressedData.PosX, compressedData.PosY);
			if (ServerStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasCreateEvent(compressedData.EntityID, position))
			{
				return;
			}
			uint num = ServerSingleton<ServerSyncManager>.Instance.RequestNewRuntimeID();
			compressedData.RuntimeID = num;
			if (!Singleton<EntityManager>.Instance.C_CreateEntity(compressedData, true))
			{
				ServerSingleton<ServerSyncManager>.Instance.ReleaseRuntimeID(num);
				return;
			}
			this.QueueNetworkEvent<C_EntityCreationData>(compressedData);
			this._sendPackage = true;
			if (ServerStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.AddCreationEventPosition(compressedData.EntityID, position);
				return;
			}
		}
		else
		{
			uint num2 = ServerSingleton<ServerSyncManager>.Instance.RequestNewRuntimeID();
			compressedData.RuntimeID = num2;
			if (!Singleton<EntityManager>.Instance.C_CreateEntity(compressedData, true))
			{
				ServerSingleton<ServerSyncManager>.Instance.ReleaseRuntimeID(num2);
				return;
			}
			NetworkSingleton<ClientStateManager>.Instance.Rpc_C_CreateEntity(compressedData);
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00034BA4 File Offset: 0x00032DA4
	private void RpcReader___Server_Srv_QueueCompressedCreationEvent_68867397(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		C_EntityCreationData compressedData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___C_EntityCreationDataFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueCompressedCreationEvent_68867397(compressedData);
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00034BD8 File Offset: 0x00032DD8
	private void RpcWriter___Server_Srv_QueueCreationEvent_1973322133(EntityCreationData creationData)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___Vectorio.Entities.EntityCreationDataFishNet.Serializing.Generated(creationData);
		base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00034C80 File Offset: 0x00032E80
	public void RpcLogic___Srv_QueueCreationEvent_1973322133(EntityCreationData creationData)
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			Vector2 position = new Vector2(creationData.PosX, creationData.PosY);
			if (ServerStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasCreateEvent(creationData.EntityID, position))
			{
				return;
			}
			uint num = ServerSingleton<ServerSyncManager>.Instance.RequestNewRuntimeID();
			creationData.RuntimeID = num;
			if (!Singleton<EntityManager>.Instance.CreateEntity(creationData))
			{
				ServerSingleton<ServerSyncManager>.Instance.ReleaseRuntimeID(num);
				return;
			}
			this.QueueNetworkEvent<EntityCreationData>(creationData);
			this._sendPackage = true;
			if (ServerStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.AddCreationEventPosition(creationData.EntityID, position);
				return;
			}
		}
		else
		{
			uint num2 = ServerSingleton<ServerSyncManager>.Instance.RequestNewRuntimeID();
			creationData.RuntimeID = num2;
			if (!Singleton<EntityManager>.Instance.CreateEntity(creationData))
			{
				ServerSingleton<ServerSyncManager>.Instance.ReleaseRuntimeID(num2);
				return;
			}
			NetworkSingleton<ClientStateManager>.Instance.Rpc_CreateEntity(creationData);
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00034D50 File Offset: 0x00032F50
	private void RpcReader___Server_Srv_QueueCreationEvent_1973322133(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		EntityCreationData creationData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___Vectorio.Entities.EntityCreationDataFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueCreationEvent_1973322133(creationData);
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00034D84 File Offset: 0x00032F84
	private void RpcWriter___Server_Srv_QueueMetadataEvent_2654222363(C_EntityMetadataEvent metadataEvent)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___C_EntityMetadataEventFishNet.Serializing.Generated(metadataEvent);
		base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00034E2C File Offset: 0x0003302C
	public void RpcLogic___Srv_QueueMetadataEvent_2654222363(C_EntityMetadataEvent metadataEvent)
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			if (ServerStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasMetadataEvent(metadataEvent.RuntimeID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.HasEntity(metadataEvent.RuntimeID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.C_RunMetadataEvent(metadataEvent))
			{
				return;
			}
			this.QueueNetworkEvent<C_EntityMetadataEvent>(metadataEvent);
			this._sendPackage = true;
			if (ServerStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.AddMetadataEventEntity(metadataEvent.RuntimeID);
				return;
			}
		}
		else
		{
			if (!Singleton<EntityManager>.Instance.HasEntity(metadataEvent.RuntimeID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.C_RunMetadataEvent(metadataEvent))
			{
				return;
			}
			NetworkSingleton<ClientStateManager>.Instance.Rpc_C_MetadataEvent(metadataEvent);
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00034ED0 File Offset: 0x000330D0
	private void RpcReader___Server_Srv_QueueMetadataEvent_2654222363(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		C_EntityMetadataEvent metadataEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___C_EntityMetadataEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueMetadataEvent_2654222363(metadataEvent);
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00034F04 File Offset: 0x00033104
	private void RpcWriter___Server_Srv_QueueDamageEvent_3491301233(EntityDamageEvent damageEvent)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___EntityDamageEventFishNet.Serializing.Generated(damageEvent);
		base.SendServerRpc(6U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00034FAC File Offset: 0x000331AC
	public void RpcLogic___Srv_QueueDamageEvent_3491301233(EntityDamageEvent damageEvent)
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			if (ServerStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasDamageEvent(damageEvent.EntityID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.HasEntity(damageEvent.EntityID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.RunDamageEvent(damageEvent))
			{
				return;
			}
			this.QueueNetworkEvent<EntityDamageEvent>(damageEvent);
			this._sendPackage = true;
			if (ServerStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.AddDamageEventEntity(damageEvent.EntityID);
				return;
			}
		}
		else
		{
			if (!Singleton<EntityManager>.Instance.HasEntity(damageEvent.EntityID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.RunDamageEvent(damageEvent))
			{
				return;
			}
			NetworkSingleton<ClientStateManager>.Instance.Rpc_DamageEvent(damageEvent);
		}
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00035050 File Offset: 0x00033250
	private void RpcReader___Server_Srv_QueueDamageEvent_3491301233(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		EntityDamageEvent damageEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___EntityDamageEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueDamageEvent_3491301233(damageEvent);
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00035084 File Offset: 0x00033284
	private void RpcWriter___Server_Srv_QueueDestroyEvent_3893339450(EntityDestroyEvent destroyEvent)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___EntityDestroyEventFishNet.Serializing.Generated(destroyEvent);
		base.SendServerRpc(7U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0003512C File Offset: 0x0003332C
	public void RpcLogic___Srv_QueueDestroyEvent_3893339450(EntityDestroyEvent destroyEvent)
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			if (ServerStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasDestroyEvent(destroyEvent.RuntimeID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.HasEntity(destroyEvent.RuntimeID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.RunDestroyEvent(destroyEvent))
			{
				return;
			}
			ServerSingleton<ServerSyncManager>.Instance.ReleaseRuntimeID(destroyEvent.RuntimeID);
			this.QueueNetworkEvent<EntityDestroyEvent>(destroyEvent);
			this._sendPackage = true;
			if (ServerStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.AddDestroyEventEntity(destroyEvent.RuntimeID);
				return;
			}
		}
		else
		{
			if (!Singleton<EntityManager>.Instance.HasEntity(destroyEvent.RuntimeID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.RunDestroyEvent(destroyEvent))
			{
				return;
			}
			NetworkSingleton<ClientStateManager>.Instance.Rpc_DestroyEvent(destroyEvent);
			ServerSingleton<ServerSyncManager>.Instance.ReleaseRuntimeID(destroyEvent.RuntimeID);
		}
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x000351F0 File Offset: 0x000333F0
	private void RpcReader___Server_Srv_QueueDestroyEvent_3893339450(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		EntityDestroyEvent destroyEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___EntityDestroyEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueDestroyEvent_3893339450(destroyEvent);
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00035224 File Offset: 0x00033424
	private void RpcWriter___Server_Srv_QueueCallbackEvent_2877855451(EntityCallbackEvent callbackEvent)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___EntityCallbackEventFishNet.Serializing.Generated(callbackEvent);
		base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x000352CC File Offset: 0x000334CC
	public void RpcLogic___Srv_QueueCallbackEvent_2877855451(EntityCallbackEvent callbackEvent)
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			if (ServerStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasStartedCallbackEvent(callbackEvent.EntityID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.HasEntity(callbackEvent.EntityID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.RunCallbackEvent(callbackEvent))
			{
				return;
			}
			if (callbackEvent.IsFinished)
			{
				this._eventPackage.events.Add(callbackEvent);
				this._sendPackage = true;
				if (ServerStateManager.USE_PACKAGE_VALIDATION)
				{
					this._packageValidator.AddFinishedCallbackEvent(callbackEvent.EntityID);
					return;
				}
			}
			else
			{
				LinkedListNode<EntityCallbackEvent> linkedListNode = new LinkedListNode<EntityCallbackEvent>(callbackEvent);
				this._callbackList.AddLast(linkedListNode);
				this._callbackDict[callbackEvent.EntityID] = linkedListNode;
				this._eventPackage.events.Add(callbackEvent);
				this._sendPackage = true;
				if (ServerStateManager.USE_PACKAGE_VALIDATION)
				{
					this._packageValidator.AddStartedCallbackEvent(callbackEvent.EntityID);
					return;
				}
			}
		}
		else
		{
			if (!Singleton<EntityManager>.Instance.HasEntity(callbackEvent.EntityID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.RunCallbackEvent(callbackEvent))
			{
				return;
			}
			if (callbackEvent.IsFinished)
			{
				NetworkSingleton<ClientStateManager>.Instance.Rpc_CallbackEntity(callbackEvent);
				return;
			}
			LinkedListNode<EntityCallbackEvent> linkedListNode2 = new LinkedListNode<EntityCallbackEvent>(callbackEvent);
			this._callbackList.AddLast(linkedListNode2);
			this._callbackDict[callbackEvent.EntityID] = linkedListNode2;
			NetworkSingleton<ClientStateManager>.Instance.Rpc_CallbackEntity(callbackEvent);
		}
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x00035418 File Offset: 0x00033618
	private void RpcReader___Server_Srv_QueueCallbackEvent_2877855451(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		EntityCallbackEvent callbackEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___EntityCallbackEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueCallbackEvent_2877855451(callbackEvent);
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x0003544C File Offset: 0x0003364C
	private void RpcWriter___Server_Srv_CancelCallbackEvent_1489494155(uint id)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.WriteUInt32(id, AutoPackType.Packed);
		base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x000354F8 File Offset: 0x000336F8
	public void RpcLogic___Srv_CancelCallbackEvent_1489494155(uint id)
	{
		LinkedListNode<EntityCallbackEvent> node;
		if (this._callbackDict.TryGetValue(id, out node))
		{
			this._callbackList.Remove(node);
			this._callbackDict.Remove(id);
		}
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00035530 File Offset: 0x00033730
	private void RpcReader___Server_Srv_CancelCallbackEvent_1489494155(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		uint id = PooledReader0.ReadUInt32(AutoPackType.Packed);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_CancelCallbackEvent_1489494155(id);
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00035568 File Offset: 0x00033768
	private void RpcWriter___Server_Srv_QueueSyncEvent_671342912(SyncEvent syncEvent)
	{
		if (!base.IsClientInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___SyncEventFishNet.Serializing.Generated(syncEvent);
		base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
		writer.Store();
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00035610 File Offset: 0x00033810
	public void RpcLogic___Srv_QueueSyncEvent_671342912(SyncEvent syncEvent)
	{
		if (ServerStateManager.USE_NETWORK_PACKAGING)
		{
			if (ServerStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasSyncEvent(syncEvent.RuntimeID))
			{
				return;
			}
			if (!Singleton<EntityManager>.Instance.RunSyncEvent(syncEvent))
			{
				return;
			}
			this.QueueNetworkEvent<SyncEvent>(syncEvent);
			this._sendPackage = true;
			if (ServerStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.AddSyncEvent(syncEvent.RuntimeID);
				return;
			}
		}
		else
		{
			if (!Singleton<EntityManager>.Instance.RunSyncEvent(syncEvent))
			{
				return;
			}
			NetworkSingleton<ClientStateManager>.Instance.Rpc_SyncEvent(syncEvent);
		}
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00035690 File Offset: 0x00033890
	private void RpcReader___Server_Srv_QueueSyncEvent_671342912(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
	{
		SyncEvent syncEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___SyncEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsServerInitialized)
		{
			return;
		}
		this.RpcLogic___Srv_QueueSyncEvent_671342912(syncEvent);
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x000356C1 File Offset: 0x000338C1
	public override void Awake()
	{
		this.NetworkInitialize___Early();
		base.Awake();
		this.NetworkInitialize__Late();
	}

	// Token: 0x04000835 RID: 2101
	public static bool USE_NETWORK_PACKAGING;

	// Token: 0x04000836 RID: 2102
	public static bool USE_PACKAGE_VALIDATION;

	// Token: 0x04000837 RID: 2103
	public static bool USE_PACKAGE_COMPRESSION;

	// Token: 0x04000838 RID: 2104
	private ClientStateManager _clientStateManager;

	// Token: 0x04000839 RID: 2105
	private NetworkEventPackage _eventPackage;

	// Token: 0x0400083A RID: 2106
	private NetworkPackageValidator _packageValidator = new NetworkPackageValidator();

	// Token: 0x0400083B RID: 2107
	private bool _sendPackage;

	// Token: 0x0400083C RID: 2108
	private Dictionary<uint, LinkedListNode<EntityCallbackEvent>> _callbackDict = new Dictionary<uint, LinkedListNode<EntityCallbackEvent>>();

	// Token: 0x0400083D RID: 2109
	private LinkedList<EntityCallbackEvent> _callbackList = new LinkedList<EntityCallbackEvent>();

	// Token: 0x0400083E RID: 2110
	private bool dll_Excuted;

	// Token: 0x0400083F RID: 2111
	private bool dll_Excuted;
}
