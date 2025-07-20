using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Serialization;

namespace Vectorio.Entities
{
	// Token: 0x02000297 RID: 663
	public class EventBuilder
	{
		// Token: 0x060012B8 RID: 4792 RVA: 0x000567EC File Offset: 0x000549EC
		public static EntityCreationData BuildCreationData(string entityID, string factionID, Vector2 position, SyncType syncType = SyncType.None)
		{
			return new EntityCreationData
			{
				SyncType = syncType,
				EntityID = entityID,
				EntityFlags = (EntityFlags.IsEditable | EntityFlags.IsTargetable),
				ApplyFlagsPostCreation = false,
				FactionID = factionID,
				PosX = position.x,
				PosY = position.y,
				Accent = null,
				Checks = (CheckFlags)0,
				Flags = (CreationFlags)0,
				ModelID = "default"
			};
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0005685C File Offset: 0x00054A5C
		public static C_EntityCreationData BuildCompressedCreationData(EntityCreationData creationData)
		{
			return new C_EntityCreationData
			{
				CreationData = DataProcessor.SerializeAndCompressObject<EntityCreationData>(creationData),
				EntityID = creationData.EntityID,
				RuntimeID = creationData.RuntimeID,
				PosX = creationData.PosX,
				PosY = creationData.PosY
			};
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000568AC File Offset: 0x00054AAC
		public static bool BuildCreationDataWithMetadata(EntityMetadata metadata, out EntityCreationData data)
		{
			if (metadata == null)
			{
				Debug.LogError("[EVENT BUILDER] Provided metadata was null, check save integrity.");
				data = null;
				return false;
			}
			data = new EntityCreationData
			{
				SyncType = SyncType.None,
				RuntimeID = metadata.RuntimeID.GetValue(),
				EntityID = metadata.EntityID,
				EntityFlags = metadata.EntityFlags,
				ApplyFlagsPostCreation = true,
				FactionID = metadata.FactionID,
				PosX = metadata.PosX,
				PosY = metadata.PosY,
				Checks = (CheckFlags)0,
				Flags = (CreationFlags)0,
				ModelID = "default"
			};
			if (metadata.Has_EFlag(EntityFlags.IsBlueprint))
			{
				EventBuilder.SetDataAsBlueprint(ref data);
			}
			if (metadata.ModelID != "")
			{
				EventBuilder.ApplyCosmeticToCreationData(ref data, metadata.ModelID);
			}
			if (metadata.AccentData != null)
			{
				EventBuilder.ApplyAccentToCreationData(ref data, metadata.AccentData);
			}
			return true;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0005698C File Offset: 0x00054B8C
		public static bool BuildCreationDataWithMetadata(EntityMetadata metadata, out EntityCreationData data, string factionID, float posX, float posY)
		{
			if (metadata == null)
			{
				Debug.LogError("[EVENT BUILDER] Provided metadata was null, check save integrity.");
				data = null;
				return false;
			}
			data = new EntityCreationData
			{
				SyncType = SyncType.None,
				RuntimeID = metadata.RuntimeID.GetValue(),
				EntityID = metadata.EntityID,
				EntityFlags = metadata.EntityFlags,
				ApplyFlagsPostCreation = true,
				FactionID = factionID,
				PosX = posX,
				PosY = posY,
				Checks = (CheckFlags)0,
				Flags = (CreationFlags)0,
				ModelID = "default"
			};
			if (metadata.Has_EFlag(EntityFlags.IsBlueprint))
			{
				EventBuilder.SetDataAsBlueprint(ref data);
			}
			if (metadata.ModelID != "")
			{
				EventBuilder.ApplyCosmeticToCreationData(ref data, metadata.ModelID);
			}
			if (metadata.AccentData != null)
			{
				EventBuilder.ApplyAccentToCreationData(ref data, metadata.AccentData);
			}
			return true;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00056A60 File Offset: 0x00054C60
		public static ContainerCreationData BuildContainer(uint id, ContainerType type, StorageMode mode, int storage, int statCode, ContainerFlags flags = ContainerFlags.None)
		{
			return new ContainerCreationData
			{
				Type = type,
				StorageMode = mode,
				Storage = storage,
				StatCode = statCode,
				Flags = flags
			};
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00056AA0 File Offset: 0x00054CA0
		public static EntityCallbackEvent BuildCallbackEvent(Entity entity, byte componentIndex, float time, VariableContainer variable = null)
		{
			return new EntityCallbackEvent
			{
				EntityID = entity.RuntimeID,
				ComponentID = componentIndex,
				Time = time,
				Variable = variable
			};
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00056AC8 File Offset: 0x00054CC8
		public static SyncEvent BuildSyncEvent(bool IsEntity, uint runtimeID, byte componentID, byte[] data)
		{
			return new SyncEvent
			{
				IsEntity = IsEntity,
				RuntimeID = runtimeID,
				ComponentID = componentID,
				Data = data
			};
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00056AEB File Offset: 0x00054CEB
		public static EntityDamageEvent BuildDamageEvent(Entity entity, float damage, Entity damager = null)
		{
			return new EntityDamageEvent
			{
				EntityID = entity.RuntimeID,
				DamagerID = ((damager != null) ? damager.RuntimeID : 0U),
				Damage = damage
			};
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00056B1D File Offset: 0x00054D1D
		public static EntityDestroyEvent BuildDestroyEvent(Entity entity, Entity damager = null)
		{
			return new EntityDestroyEvent
			{
				RuntimeID = entity.RuntimeID,
				DamagerID = ((damager != null) ? damager.RuntimeID : 0U),
				Recycle = true
			};
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00056B4F File Offset: 0x00054D4F
		public static void ToggleRecycleFlagOnDestroyEvent(ref EntityDestroyEvent destroyEvent, bool toggle)
		{
			destroyEvent.Recycle = toggle;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00056B59 File Offset: 0x00054D59
		public static EntityMetadataEvent BuildMetadataEvent(uint runtimeID, EntityMetadata metadata, bool asPipette)
		{
			return new EntityMetadataEvent
			{
				RuntimeID = runtimeID,
				Metadata = metadata,
				AsPipette = asPipette
			};
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00056B75 File Offset: 0x00054D75
		public static EntityMetadataEvent BuildMetadataEventFromEntity(Entity entity, bool asPipette)
		{
			return EventBuilder.BuildMetadataEvent(entity.RuntimeID, entity.ExtractMetadata(asPipette, MetadataContext.Global), asPipette);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00056B8B File Offset: 0x00054D8B
		public static void SetDataAsBlueprint(ref EntityCreationData data)
		{
			data.IsBlueprint = (Singleton<Gamemode>.Instance.UseBuilderDrones || !DevTools.INSTANT_BUILD);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00056BAB File Offset: 0x00054DAB
		public static void ApplyCostsToCreationData(ref EntityCreationData data, bool checkCosts)
		{
			data.Flags |= CreationFlags.UseCosts;
			if (checkCosts)
			{
				data.Checks |= CheckFlags.CheckCosts;
				return;
			}
			data.Checks &= ~CheckFlags.CheckCosts;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00056BDF File Offset: 0x00054DDF
		public static void ApplyCallbackToCreationData(ref EntityCreationData data, CallbackType callbackType, uint callbackID, byte componentIndex = 0)
		{
			data.Callback = new OnCreationCallback(callbackID, callbackType, componentIndex);
			data.Flags |= CreationFlags.UseCallback;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00056BFF File Offset: 0x00054DFF
		public static void ApplyVariablesToCreationData(ref EntityCreationData data, List<VariableContainer> variables)
		{
			data.Variables = variables;
			data.Flags |= CreationFlags.UseVariables;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00056C19 File Offset: 0x00054E19
		public static void ApplyChecksToCreationData(ref EntityCreationData data, CheckFlags checkFlags)
		{
			data.Checks |= checkFlags;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00056C2A File Offset: 0x00054E2A
		public static void ApplyCosmeticToCreationData(ref EntityCreationData data, string modelID)
		{
			data.ModelID = modelID;
			data.Flags |= CreationFlags.UseCosmetic;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00056C43 File Offset: 0x00054E43
		public static void ApplyAccentToCreationData(ref EntityCreationData data, AccentData accent)
		{
			data.Accent = accent;
			data.Flags |= CreationFlags.UseAccent;
		}
	}
}
