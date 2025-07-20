using System;
using System.IO;
using System.Linq;
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

// Token: 0x0200015E RID: 350
[DefaultExecutionOrder(0)]
public class ClientStateManager : NetworkSingleton<ClientStateManager>
{
	// Token: 0x06000B5E RID: 2910 RVA: 0x00031AD6 File Offset: 0x0002FCD6
	public void Cmd_QueueResearchChange(ResearchChangeEvent researchChangeEvent)
	{
		ServerSingleton<ServerStateManager>.Instance.Srv_QueueResearchChange(researchChangeEvent);
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00031AE3 File Offset: 0x0002FCE3
	public void Cmd_QueueRegionChange(RegionChangeEvent regionChangeEvent)
	{
		ServerSingleton<ServerStateManager>.Instance.Srv_QueueRegionChange(regionChangeEvent);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00031AF0 File Offset: 0x0002FCF0
	public void Cmd_QueueCreationEvent(EntityCreationData creationData)
	{
		Vector2 position = new Vector2(creationData.PosX, creationData.PosY);
		if (ClientStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasCreateEvent(creationData.EntityID, position))
		{
			return;
		}
		if (ClientStateManager.USE_ENTITY_COMPRESSION)
		{
			C_EntityCreationData compressedData = EventBuilder.BuildCompressedCreationData(creationData);
			ServerSingleton<ServerStateManager>.Instance.Srv_QueueCompressedCreationEvent(compressedData);
		}
		else
		{
			ServerSingleton<ServerStateManager>.Instance.Srv_QueueCreationEvent(creationData);
		}
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.AddCreationEventPosition(creationData.EntityID, position);
		}
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00031B6B File Offset: 0x0002FD6B
	public void Cmd_QueueDamageEvent(EntityDamageEvent damageData)
	{
		if (ClientStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasDamageEvent(damageData.EntityID))
		{
			return;
		}
		ServerSingleton<ServerStateManager>.Instance.Srv_QueueDamageEvent(damageData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.AddDamageEventEntity(damageData.EntityID);
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00031BAB File Offset: 0x0002FDAB
	public void Cmd_QueueDestroyEvent(EntityDestroyEvent destroyData)
	{
		if (ClientStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasDestroyEvent(destroyData.RuntimeID))
		{
			return;
		}
		ServerSingleton<ServerStateManager>.Instance.Srv_QueueDestroyEvent(destroyData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.AddDestroyEventEntity(destroyData.RuntimeID);
		}
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00031BEC File Offset: 0x0002FDEC
	public void Cmd_QueueMetadataEvent(EntityMetadataEvent metadataEvent)
	{
		if (ClientStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasMetadataEvent(metadataEvent.RuntimeID))
		{
			return;
		}
		C_EntityMetadataEvent metadataEvent2 = new C_EntityMetadataEvent
		{
			RuntimeID = metadataEvent.RuntimeID,
			Metadata = DataProcessor.SerializeAndCompressObject<EntityMetadata>(metadataEvent.Metadata),
			AsPipette = metadataEvent.AsPipette
		};
		ServerSingleton<ServerStateManager>.Instance.Srv_QueueMetadataEvent(metadataEvent2);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.AddMetadataEventEntity(metadataEvent.RuntimeID);
		}
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x00031C66 File Offset: 0x0002FE66
	public void Cmd_QueueCallbackEvent(EntityCallbackEvent callbackData)
	{
		if (ClientStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasStartedCallbackEvent(callbackData.EntityID))
		{
			return;
		}
		ServerSingleton<ServerStateManager>.Instance.Srv_QueueCallbackEvent(callbackData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.AddStartedCallbackEvent(callbackData.EntityID);
		}
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x00031CA6 File Offset: 0x0002FEA6
	public void Cmd_QueueSyncEvent(SyncEvent syncData)
	{
		if (ClientStateManager.USE_PACKAGE_VALIDATION && this._packageValidator.HasSyncEvent(syncData.RuntimeID))
		{
			return;
		}
		ServerSingleton<ServerStateManager>.Instance.Srv_QueueSyncEvent(syncData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.AddSyncEvent(syncData.RuntimeID);
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00031CE8 File Offset: 0x0002FEE8
	[TargetRpc(ExcludeServer = true)]
	public void Rpc_ReceiveWorldState(NetworkConnection connection, Utilities.CompressedData worldStateInformation)
	{
		this.RpcWriter___Target_Rpc_ReceiveWorldState_1584035033(connection, worldStateInformation);
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00031D04 File Offset: 0x0002FF04
	public static void WriteJsonToFile(string jsonString, string path, string fileName)
	{
		string path2 = Path.Combine(Application.persistentDataPath, path, fileName);
		string directoryName = Path.GetDirectoryName(path2);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		File.WriteAllText(path2, jsonString);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00031D39 File Offset: 0x0002FF39
	[ObserversRpc]
	public void Rpc_ReceiveNetworkEventPackage(NetworkEventPackage package)
	{
		this.RpcWriter___Observers_Rpc_ReceiveNetworkEventPackage_2430619785(package);
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00031D45 File Offset: 0x0002FF45
	[ObserversRpc]
	public void Rpc_ReceiveCompressedNetworkEventPackage(Utilities.CompressedData compressedPackage)
	{
		this.RpcWriter___Observers_Rpc_ReceiveCompressedNetworkEventPackage_4289215664(compressedPackage);
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00031D54 File Offset: 0x0002FF54
	private void ProcessNetworkEventPackage(NetworkEventPackage package)
	{
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.Reset();
		}
		foreach (NetworkEventBase networkEventBase in (from e in package.events
		orderby e.Timestamp
		select e).ToList<NetworkEventBase>())
		{
			EntityCreationData entityCreationData = networkEventBase as EntityCreationData;
			if (entityCreationData == null)
			{
				C_EntityCreationData c_EntityCreationData = networkEventBase as C_EntityCreationData;
				if (c_EntityCreationData == null)
				{
					EntityDamageEvent entityDamageEvent = networkEventBase as EntityDamageEvent;
					if (entityDamageEvent == null)
					{
						EntityDestroyEvent entityDestroyEvent = networkEventBase as EntityDestroyEvent;
						if (entityDestroyEvent == null)
						{
							C_EntityMetadataEvent c_EntityMetadataEvent = networkEventBase as C_EntityMetadataEvent;
							if (c_EntityMetadataEvent == null)
							{
								EntityCallbackEvent entityCallbackEvent = networkEventBase as EntityCallbackEvent;
								if (entityCallbackEvent == null)
								{
									SyncEvent syncEvent = networkEventBase as SyncEvent;
									if (syncEvent == null)
									{
										if (networkEventBase == null)
										{
											Debug.Log("[CSM] Null event type was caught!");
										}
										else
										{
											Debug.Log("[CSM] Unknown event type was received: " + networkEventBase.GetType().ToString());
										}
									}
									else
									{
										Singleton<EntityManager>.Instance.RunSyncEvent(syncEvent);
									}
								}
								else
								{
									Singleton<EntityManager>.Instance.RunCallbackEvent(entityCallbackEvent);
								}
							}
							else
							{
								Singleton<EntityManager>.Instance.C_RunMetadataEvent(c_EntityMetadataEvent);
							}
						}
						else
						{
							Singleton<EntityManager>.Instance.RunDestroyEvent(entityDestroyEvent);
						}
					}
					else
					{
						Singleton<EntityManager>.Instance.RunDamageEvent(entityDamageEvent);
					}
				}
				else
				{
					Singleton<EntityManager>.Instance.C_CreateEntity(c_EntityCreationData, false);
				}
			}
			else
			{
				Singleton<EntityManager>.Instance.CreateEntity(entityCreationData);
			}
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00031ED4 File Offset: 0x000300D4
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_ChangeResearch(ResearchChangeEvent researchChangeEvent)
	{
		this.RpcWriter___Observers_Rpc_ChangeResearch_676677770(researchChangeEvent);
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x00031EEC File Offset: 0x000300EC
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_ChangeRegion(RegionChangeEvent regionChangeEvent)
	{
		this.RpcWriter___Observers_Rpc_ChangeRegion_1815257947(regionChangeEvent);
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x00031F04 File Offset: 0x00030104
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_CreateEntity(EntityCreationData creationData)
	{
		this.RpcWriter___Observers_Rpc_CreateEntity_1973322133(creationData);
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00031F1C File Offset: 0x0003011C
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_C_CreateEntity(C_EntityCreationData creationData)
	{
		this.RpcWriter___Observers_Rpc_C_CreateEntity_68867397(creationData);
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00031F33 File Offset: 0x00030133
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_DamageEvent(EntityDamageEvent damageEvent)
	{
		this.RpcWriter___Observers_Rpc_DamageEvent_3491301233(damageEvent);
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00031F3F File Offset: 0x0003013F
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_DestroyEvent(EntityDestroyEvent destroyEvent)
	{
		this.RpcWriter___Observers_Rpc_DestroyEvent_3893339450(destroyEvent);
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x00031F4B File Offset: 0x0003014B
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_C_MetadataEvent(C_EntityMetadataEvent creationData)
	{
		this.RpcWriter___Observers_Rpc_C_MetadataEvent_2654222363(creationData);
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x00031F57 File Offset: 0x00030157
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_CallbackEntity(EntityCallbackEvent creationData)
	{
		this.RpcWriter___Observers_Rpc_CallbackEntity_2877855451(creationData);
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x00031F63 File Offset: 0x00030163
	[ObserversRpc(ExcludeServer = true)]
	public void Rpc_SyncEvent(SyncEvent creationData)
	{
		this.RpcWriter___Observers_Rpc_SyncEvent_671342912(creationData);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00031F84 File Offset: 0x00030184
	public override void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyClientStateManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyClientStateManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize___Early();
		base.RegisterTargetRpc(0U, new ClientRpcDelegate(this.RpcReader___Target_Rpc_ReceiveWorldState_1584035033));
		base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_ReceiveNetworkEventPackage_2430619785));
		base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_ReceiveCompressedNetworkEventPackage_4289215664));
		base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_ChangeResearch_676677770));
		base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_ChangeRegion_1815257947));
		base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_CreateEntity_1973322133));
		base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_C_CreateEntity_68867397));
		base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_DamageEvent_3491301233));
		base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_DestroyEvent_3893339450));
		base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_C_MetadataEvent_2654222363));
		base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_CallbackEntity_2877855451));
		base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_Rpc_SyncEvent_671342912));
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x000320BC File Offset: 0x000302BC
	public override void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LateClientStateManagerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LateClientStateManagerAssembly-CSharp.dll_Excuted = true;
		base.NetworkInitialize__Late();
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x000320D5 File Offset: 0x000302D5
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x000320E4 File Offset: 0x000302E4
	private void RpcWriter___Target_Rpc_ReceiveWorldState_1584035033(NetworkConnection connection, Utilities.CompressedData worldStateInformation)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___Vectorio.Utilities.Utilities/CompressedDataFishNet.Serializing.Generated(worldStateInformation);
		base.SendTargetRpc(0U, writer, channel, DataOrderType.Default, connection, true, true);
		writer.Store();
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0003219C File Offset: 0x0003039C
	public void RpcLogic___Rpc_ReceiveWorldState_1584035033(NetworkConnection connection, Utilities.CompressedData worldStateInformation)
	{
		Debug.Log("[NETWORK] Received world state data from server, decompressing...");
		SaveData saveData;
		try
		{
			saveData = DataProcessor.DecompressAndDeserializeObject<SaveData>(worldStateInformation.Data);
		}
		catch (Exception ex)
		{
			Debug.LogError("[NETWORK] Error decompressing or deserializing world state data: " + ex.Message);
			NetworkSingleton<ClientSyncManager>.Instance.Srv_SyncPlayerLoading(false);
			Singleton<Events>.Instance.onDisableActionMap.Invoke();
			Singleton<Lobby>.Instance.LeaveLobby();
			return;
		}
		if (saveData == null)
		{
			Debug.LogError("[NETWORK] Decompressed data is null or invalid. Disconnecting.");
			NetworkSingleton<ClientSyncManager>.Instance.Srv_SyncPlayerLoading(false);
			Singleton<Events>.Instance.onDisableActionMap.Invoke();
			Singleton<Lobby>.Instance.LeaveLobby();
			return;
		}
		GamemodeData gamemodeData = Library.RequestData<GamemodeData>(saveData.GamemodeData.ID);
		if (gamemodeData != null)
		{
			saveData.FileName = FileOperations.GenerateSaveFileName(gamemodeData);
		}
		else
		{
			saveData.FileName = FileOperations.GenerateSaveFileName();
		}
		SaveSystem.SaveData = saveData;
		Singleton<NetworkEvents>.Instance.onLocalClientReadyToLoad.Invoke(true);
		Debug.Log("[NETWORK] World state loaded! Ready to begin scene setup.");
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x00032298 File Offset: 0x00030498
	private void RpcReader___Target_Rpc_ReceiveWorldState_1584035033(PooledReader PooledReader0, Channel channel)
	{
		Utilities.CompressedData worldStateInformation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___Vectorio.Utilities.Utilities/CompressedDataFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_ReceiveWorldState_1584035033(base.LocalConnection, worldStateInformation);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x000322D0 File Offset: 0x000304D0
	private void RpcWriter___Observers_Rpc_ReceiveNetworkEventPackage_2430619785(NetworkEventPackage package)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___NetworkEventPackageFishNet.Serializing.Generated(package);
		base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
		writer.Store();
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x00032386 File Offset: 0x00030586
	public void RpcLogic___Rpc_ReceiveNetworkEventPackage_2430619785(NetworkEventPackage package)
	{
		if (base.IsHost)
		{
			if (ClientStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.Reset();
				return;
			}
		}
		else
		{
			this.ProcessNetworkEventPackage(package);
		}
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x000323AC File Offset: 0x000305AC
	private void RpcReader___Observers_Rpc_ReceiveNetworkEventPackage_2430619785(PooledReader PooledReader0, Channel channel)
	{
		NetworkEventPackage package = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___NetworkEventPackageFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_ReceiveNetworkEventPackage_2430619785(package);
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x000323E0 File Offset: 0x000305E0
	private void RpcWriter___Observers_Rpc_ReceiveCompressedNetworkEventPackage_4289215664(Utilities.CompressedData compressedPackage)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___Vectorio.Utilities.Utilities/CompressedDataFishNet.Serializing.Generated(compressedPackage);
		base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
		writer.Store();
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00032496 File Offset: 0x00030696
	public void RpcLogic___Rpc_ReceiveCompressedNetworkEventPackage_4289215664(Utilities.CompressedData compressedPackage)
	{
		if (base.IsHost)
		{
			if (ClientStateManager.USE_PACKAGE_VALIDATION)
			{
				this._packageValidator.Reset();
				return;
			}
		}
		else
		{
			this.ProcessNetworkEventPackage(DataProcessor.DecompressAndDeserializeObject<NetworkEventPackage>(compressedPackage.Data));
		}
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x000324C4 File Offset: 0x000306C4
	private void RpcReader___Observers_Rpc_ReceiveCompressedNetworkEventPackage_4289215664(PooledReader PooledReader0, Channel channel)
	{
		Utilities.CompressedData compressedPackage = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___Vectorio.Utilities.Utilities/CompressedDataFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_ReceiveCompressedNetworkEventPackage_4289215664(compressedPackage);
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x000324F8 File Offset: 0x000306F8
	private void RpcWriter___Observers_Rpc_ChangeResearch_676677770(ResearchChangeEvent researchChangeEvent)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___ResearchChangeEventFishNet.Serializing.Generated(researchChangeEvent);
		base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x000325B0 File Offset: 0x000307B0
	public void RpcLogic___Rpc_ChangeResearch_676677770(ResearchChangeEvent researchChangeEvent)
	{
		if (researchChangeEvent.ID == null || researchChangeEvent.ID == "")
		{
			Debug.Log("[CSM] Received bad research change event.");
			return;
		}
		ResearchTechData researchTechData = Library.RequestData<ResearchTechData>(researchChangeEvent.ID);
		if (researchTechData != null)
		{
			UI_Singleton<UI_ResearchWindow>.Instance.SetActiveTech(researchTechData, researchTechData.costs);
			return;
		}
		Debug.Log("[CSM] A tech with ID " + researchChangeEvent.ID + " does not exist!");
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x00032624 File Offset: 0x00030824
	private void RpcReader___Observers_Rpc_ChangeResearch_676677770(PooledReader PooledReader0, Channel channel)
	{
		ResearchChangeEvent researchChangeEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ResearchChangeEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_ChangeResearch_676677770(researchChangeEvent);
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x00032658 File Offset: 0x00030858
	private void RpcWriter___Observers_Rpc_ChangeRegion_1815257947(RegionChangeEvent regionChangeEvent)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___RegionChangeEventFishNet.Serializing.Generated(regionChangeEvent);
		base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00032710 File Offset: 0x00030910
	public void RpcLogic___Rpc_ChangeRegion_1815257947(RegionChangeEvent regionChangeEvent)
	{
		Entity entity;
		if (!Singleton<EntityManager>.Instance.TryGetEntity(regionChangeEvent.GatewayID, out entity))
		{
			Debug.Log("[CSM] No entity with ID " + regionChangeEvent.GatewayID.ToString() + " exists for region change event!");
		}
		if (entity != null && entity.Has_EComponent<Gateway>())
		{
			Gateway gateway = entity.Get_EComponent<Gateway>(false);
			Singleton<RegionManager>.Instance.WarpToRegion(gateway);
			return;
		}
		Debug.Log("[CSM] Gateway is null or entity does not have gateway component!");
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00032780 File Offset: 0x00030980
	private void RpcReader___Observers_Rpc_ChangeRegion_1815257947(PooledReader PooledReader0, Channel channel)
	{
		RegionChangeEvent regionChangeEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___RegionChangeEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_ChangeRegion_1815257947(regionChangeEvent);
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x000327B4 File Offset: 0x000309B4
	private void RpcWriter___Observers_Rpc_CreateEntity_1973322133(EntityCreationData creationData)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___Vectorio.Entities.EntityCreationDataFishNet.Serializing.Generated(creationData);
		base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x0003286C File Offset: 0x00030A6C
	public void RpcLogic___Rpc_CreateEntity_1973322133(EntityCreationData creationData)
	{
		Singleton<EntityManager>.Instance.CreateEntity(creationData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			Vector2 position = new Vector2(creationData.PosX, creationData.PosY);
			this._packageValidator.ResetCreationEvent(creationData.EntityID, position);
		}
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x000328B4 File Offset: 0x00030AB4
	private void RpcReader___Observers_Rpc_CreateEntity_1973322133(PooledReader PooledReader0, Channel channel)
	{
		EntityCreationData creationData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___Vectorio.Entities.EntityCreationDataFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_CreateEntity_1973322133(creationData);
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x000328E8 File Offset: 0x00030AE8
	private void RpcWriter___Observers_Rpc_C_CreateEntity_68867397(C_EntityCreationData creationData)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___C_EntityCreationDataFishNet.Serializing.Generated(creationData);
		base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x000329A0 File Offset: 0x00030BA0
	public void RpcLogic___Rpc_C_CreateEntity_68867397(C_EntityCreationData creationData)
	{
		Singleton<EntityManager>.Instance.C_CreateEntity(creationData, false);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			Vector2 position = new Vector2(creationData.PosX, creationData.PosY);
			this._packageValidator.ResetCreationEvent(creationData.EntityID, position);
		}
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x000329E8 File Offset: 0x00030BE8
	private void RpcReader___Observers_Rpc_C_CreateEntity_68867397(PooledReader PooledReader0, Channel channel)
	{
		C_EntityCreationData creationData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___C_EntityCreationDataFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_C_CreateEntity_68867397(creationData);
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x00032A1C File Offset: 0x00030C1C
	private void RpcWriter___Observers_Rpc_DamageEvent_3491301233(EntityDamageEvent damageEvent)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___EntityDamageEventFishNet.Serializing.Generated(damageEvent);
		base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00032AD2 File Offset: 0x00030CD2
	public void RpcLogic___Rpc_DamageEvent_3491301233(EntityDamageEvent damageEvent)
	{
		Singleton<EntityManager>.Instance.RunDamageEvent(damageEvent);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.ResetDamageEventEntity(damageEvent.EntityID);
		}
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00032AF8 File Offset: 0x00030CF8
	private void RpcReader___Observers_Rpc_DamageEvent_3491301233(PooledReader PooledReader0, Channel channel)
	{
		EntityDamageEvent damageEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___EntityDamageEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_DamageEvent_3491301233(damageEvent);
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x00032B2C File Offset: 0x00030D2C
	private void RpcWriter___Observers_Rpc_DestroyEvent_3893339450(EntityDestroyEvent destroyEvent)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___EntityDestroyEventFishNet.Serializing.Generated(destroyEvent);
		base.SendObserversRpc(8U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00032BE2 File Offset: 0x00030DE2
	public void RpcLogic___Rpc_DestroyEvent_3893339450(EntityDestroyEvent destroyEvent)
	{
		Singleton<EntityManager>.Instance.RunDestroyEvent(destroyEvent);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.ResetDestroyEventEntity(destroyEvent.RuntimeID);
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00032C08 File Offset: 0x00030E08
	private void RpcReader___Observers_Rpc_DestroyEvent_3893339450(PooledReader PooledReader0, Channel channel)
	{
		EntityDestroyEvent destroyEvent = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___EntityDestroyEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_DestroyEvent_3893339450(destroyEvent);
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00032C3C File Offset: 0x00030E3C
	private void RpcWriter___Observers_Rpc_C_MetadataEvent_2654222363(C_EntityMetadataEvent creationData)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___C_EntityMetadataEventFishNet.Serializing.Generated(creationData);
		base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00032CF2 File Offset: 0x00030EF2
	public void RpcLogic___Rpc_C_MetadataEvent_2654222363(C_EntityMetadataEvent creationData)
	{
		Singleton<EntityManager>.Instance.C_RunMetadataEvent(creationData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.ResetMetadataEventEntity(creationData.RuntimeID);
		}
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x00032D18 File Offset: 0x00030F18
	private void RpcReader___Observers_Rpc_C_MetadataEvent_2654222363(PooledReader PooledReader0, Channel channel)
	{
		C_EntityMetadataEvent creationData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___C_EntityMetadataEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_C_MetadataEvent_2654222363(creationData);
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x00032D4C File Offset: 0x00030F4C
	private void RpcWriter___Observers_Rpc_CallbackEntity_2877855451(EntityCallbackEvent creationData)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___EntityCallbackEventFishNet.Serializing.Generated(creationData);
		base.SendObserversRpc(10U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00032E02 File Offset: 0x00031002
	public void RpcLogic___Rpc_CallbackEntity_2877855451(EntityCallbackEvent creationData)
	{
		Singleton<EntityManager>.Instance.RunCallbackEvent(creationData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			if (creationData.IsFinished)
			{
				this._packageValidator.ResetFinishedCallbackEvent(creationData.EntityID);
				return;
			}
			this._packageValidator.ResetStartedCallbackEvent(creationData.EntityID);
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00032E44 File Offset: 0x00031044
	private void RpcReader___Observers_Rpc_CallbackEntity_2877855451(PooledReader PooledReader0, Channel channel)
	{
		EntityCallbackEvent creationData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___EntityCallbackEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_CallbackEntity_2877855451(creationData);
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00032E78 File Offset: 0x00031078
	private void RpcWriter___Observers_Rpc_SyncEvent_671342912(SyncEvent creationData)
	{
		if (!base.IsServerInitialized)
		{
			NetworkManager networkManager = base.NetworkManager;
			if (networkManager == null)
			{
				networkManager = InstanceFinder.NetworkManager;
			}
			if (networkManager != null)
			{
				networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			else
			{
				Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
			}
			return;
		}
		Channel channel = Channel.Reliable;
		PooledWriter writer = WriterPool.GetWriter();
		writer.Write___SyncEventFishNet.Serializing.Generated(creationData);
		base.SendObserversRpc(11U, writer, channel, DataOrderType.Default, false, true, false);
		writer.Store();
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00032F2E File Offset: 0x0003112E
	public void RpcLogic___Rpc_SyncEvent_671342912(SyncEvent creationData)
	{
		Singleton<EntityManager>.Instance.RunSyncEvent(creationData);
		if (ClientStateManager.USE_PACKAGE_VALIDATION)
		{
			this._packageValidator.ResetSyncEvent(creationData.RuntimeID);
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00032F54 File Offset: 0x00031154
	private void RpcReader___Observers_Rpc_SyncEvent_671342912(PooledReader PooledReader0, Channel channel)
	{
		SyncEvent creationData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___SyncEventFishNet.Serializing.Generateds(PooledReader0);
		if (!base.IsClientInitialized)
		{
			return;
		}
		this.RpcLogic___Rpc_SyncEvent_671342912(creationData);
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x00032F85 File Offset: 0x00031185
	public override void Awake()
	{
		this.NetworkInitialize___Early();
		base.Awake();
		this.NetworkInitialize__Late();
	}

	// Token: 0x040007E6 RID: 2022
	public static bool USE_PACKAGE_VALIDATION;

	// Token: 0x040007E7 RID: 2023
	public static bool USE_ENTITY_COMPRESSION;

	// Token: 0x040007E8 RID: 2024
	private NetworkPackageValidator _packageValidator = new NetworkPackageValidator();

	// Token: 0x040007E9 RID: 2025
	private bool dll_Excuted;

	// Token: 0x040007EA RID: 2026
	private bool dll_Excuted;
}
