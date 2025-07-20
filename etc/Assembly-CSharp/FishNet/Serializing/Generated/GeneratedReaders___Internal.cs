using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Utilities;

namespace FishNet.Serializing.Generated
{
	// Token: 0x02000396 RID: 918
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedReaders___Internal
	{
		// Token: 0x060017D4 RID: 6100 RVA: 0x0007436C File Offset: 0x0007256C
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericReader<Utilities.CompressedData>.Read = new Func<Reader, Utilities.CompressedData>(GeneratedReaders___Internal.Read___Vectorio.Utilities.Utilities/CompressedDataFishNet.Serializing.Generateds);
			GenericReader<NetworkEventPackage>.Read = new Func<Reader, NetworkEventPackage>(GeneratedReaders___Internal.Read___NetworkEventPackageFishNet.Serializing.Generateds);
			GenericReader<NetworkEventBase>.Read = new Func<Reader, NetworkEventBase>(GeneratedReaders___Internal.Read___NetworkEventBaseFishNet.Serializing.Generateds);
			GenericReader<List<NetworkEventBase>>.Read = new Func<Reader, List<NetworkEventBase>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<NetworkEventBase>FishNet.Serializing.Generateds);
			GenericReader<ResearchChangeEvent>.Read = new Func<Reader, ResearchChangeEvent>(GeneratedReaders___Internal.Read___ResearchChangeEventFishNet.Serializing.Generateds);
			GenericReader<RegionChangeEvent>.Read = new Func<Reader, RegionChangeEvent>(GeneratedReaders___Internal.Read___RegionChangeEventFishNet.Serializing.Generateds);
			GenericReader<EntityCreationData>.Read = new Func<Reader, EntityCreationData>(GeneratedReaders___Internal.Read___Vectorio.Entities.EntityCreationDataFishNet.Serializing.Generateds);
			GenericReader<SyncType>.Read = new Func<Reader, SyncType>(GeneratedReaders___Internal.Read___Vectorio.Entities.SyncTypeFishNet.Serializing.Generateds);
			GenericReader<EntityFlags>.Read = new Func<Reader, EntityFlags>(GeneratedReaders___Internal.Read___EntityFlagsFishNet.Serializing.Generateds);
			GenericReader<OnCreationCallback>.Read = new Func<Reader, OnCreationCallback>(GeneratedReaders___Internal.Read___Vectorio.Entities.OnCreationCallbackFishNet.Serializing.Generateds);
			GenericReader<CallbackType>.Read = new Func<Reader, CallbackType>(GeneratedReaders___Internal.Read___Vectorio.Entities.CallbackTypeFishNet.Serializing.Generateds);
			GenericReader<CreationFlags>.Read = new Func<Reader, CreationFlags>(GeneratedReaders___Internal.Read___Vectorio.Entities.CreationFlagsFishNet.Serializing.Generateds);
			GenericReader<CheckFlags>.Read = new Func<Reader, CheckFlags>(GeneratedReaders___Internal.Read___Vectorio.Entities.CheckFlagsFishNet.Serializing.Generateds);
			GenericReader<AccentData>.Read = new Func<Reader, AccentData>(GeneratedReaders___Internal.Read___AccentDataFishNet.Serializing.Generateds);
			GenericReader<VariableContainer>.Read = new Func<Reader, VariableContainer>(GeneratedReaders___Internal.Read___Vectorio.Entities.VariableContainerFishNet.Serializing.Generateds);
			GenericReader<byte?>.Read = new Func<Reader, byte?>(GeneratedReaders___Internal.Read___System.Nullable`1<System.Byte>FishNet.Serializing.Generateds);
			GenericReader<Dictionary<byte, string>>.Read = new Func<Reader, Dictionary<byte, string>>(GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.String>FishNet.Serializing.Generateds);
			GenericReader<Dictionary<byte, int>>.Read = new Func<Reader, Dictionary<byte, int>>(GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.Int32>FishNet.Serializing.Generateds);
			GenericReader<Dictionary<byte, float>>.Read = new Func<Reader, Dictionary<byte, float>>(GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.Single>FishNet.Serializing.Generateds);
			GenericReader<Dictionary<byte, bool>>.Read = new Func<Reader, Dictionary<byte, bool>>(GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.Boolean>FishNet.Serializing.Generateds);
			GenericReader<List<VariableContainer>>.Read = new Func<Reader, List<VariableContainer>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<Vectorio.Entities.VariableContainer>FishNet.Serializing.Generateds);
			GenericReader<EntityMetadata>.Read = new Func<Reader, EntityMetadata>(GeneratedReaders___Internal.Read___EntityMetadataFishNet.Serializing.Generateds);
			GenericReader<MetadataContext>.Read = new Func<Reader, MetadataContext>(GeneratedReaders___Internal.Read___MetadataContextFishNet.Serializing.Generateds);
			GenericReader<E_ID>.Read = new Func<Reader, E_ID>(GeneratedReaders___Internal.Read___E_IDFishNet.Serializing.Generateds);
			GenericReader<ComponentMetadataWrapper>.Read = new Func<Reader, ComponentMetadataWrapper>(GeneratedReaders___Internal.Read___ComponentMetadataWrapperFishNet.Serializing.Generateds);
			GenericReader<List<ComponentMetadataWrapper>>.Read = new Func<Reader, List<ComponentMetadataWrapper>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ComponentMetadataWrapper>FishNet.Serializing.Generateds);
			GenericReader<C_EntityCreationData>.Read = new Func<Reader, C_EntityCreationData>(GeneratedReaders___Internal.Read___C_EntityCreationDataFishNet.Serializing.Generateds);
			GenericReader<EntityDamageEvent>.Read = new Func<Reader, EntityDamageEvent>(GeneratedReaders___Internal.Read___EntityDamageEventFishNet.Serializing.Generateds);
			GenericReader<EntityDestroyEvent>.Read = new Func<Reader, EntityDestroyEvent>(GeneratedReaders___Internal.Read___EntityDestroyEventFishNet.Serializing.Generateds);
			GenericReader<C_EntityMetadataEvent>.Read = new Func<Reader, C_EntityMetadataEvent>(GeneratedReaders___Internal.Read___C_EntityMetadataEventFishNet.Serializing.Generateds);
			GenericReader<EntityCallbackEvent>.Read = new Func<Reader, EntityCallbackEvent>(GeneratedReaders___Internal.Read___EntityCallbackEventFishNet.Serializing.Generateds);
			GenericReader<SyncEvent>.Read = new Func<Reader, SyncEvent>(GeneratedReaders___Internal.Read___SyncEventFishNet.Serializing.Generateds);
			GenericReader<ClientSyncManager.ClientStateInformation>.Read = new Func<Reader, ClientSyncManager.ClientStateInformation>(GeneratedReaders___Internal.Read___ClientSyncManager/ClientStateInformationFishNet.Serializing.Generateds);
			GenericReader<int[]>.Read = new Func<Reader, int[]>(GeneratedReaders___Internal.Read___System.Int32[]FishNet.Serializing.Generateds);
			GenericReader<float[]>.Read = new Func<Reader, float[]>(GeneratedReaders___Internal.Read___System.Single[]FishNet.Serializing.Generateds);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000745CC File Offset: 0x000727CC
		public static Utilities.CompressedData Generateds(Reader reader)
		{
			return new Utilities.CompressedData
			{
				Data = reader.ReadBytesAndSizeAllocated()
			};
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x000745FC File Offset: 0x000727FC
		public static NetworkEventPackage Generateds(Reader reader)
		{
			return new NetworkEventPackage
			{
				events = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<NetworkEventBase>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0007462C File Offset: 0x0007282C
		public static NetworkEventBase Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new NetworkEventBase();
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00074660 File Offset: 0x00072860
		public static List<NetworkEventBase> List(Reader reader)
		{
			return reader.ReadListAllocated<NetworkEventBase>();
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00074678 File Offset: 0x00072878
		public static ResearchChangeEvent Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ResearchChangeEvent
			{
				ID = reader.ReadString()
			};
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000746C0 File Offset: 0x000728C0
		public static RegionChangeEvent Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new RegionChangeEvent
			{
				GatewayID = reader.ReadUInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0007470C File Offset: 0x0007290C
		public static EntityCreationData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new EntityCreationData
			{
				SyncType = GeneratedReaders___Internal.Read___Vectorio.Entities.SyncTypeFishNet.Serializing.Generateds(reader),
				IsBlueprint = reader.ReadBoolean(),
				EntityID = reader.ReadString(),
				EntityFlags = GeneratedReaders___Internal.Read___EntityFlagsFishNet.Serializing.Generateds(reader),
				_hasRuntimeID = reader.ReadBoolean(),
				_runtimeID = reader.ReadUInt32(AutoPackType.Packed),
				Callback = GeneratedReaders___Internal.Read___Vectorio.Entities.OnCreationCallbackFishNet.Serializing.Generateds(reader),
				FactionID = reader.ReadString(),
				PosX = reader.ReadSingle(AutoPackType.Unpacked),
				PosY = reader.ReadSingle(AutoPackType.Unpacked),
				Flags = GeneratedReaders___Internal.Read___Vectorio.Entities.CreationFlagsFishNet.Serializing.Generateds(reader),
				Checks = GeneratedReaders___Internal.Read___Vectorio.Entities.CheckFlagsFishNet.Serializing.Generateds(reader),
				Accent = GeneratedReaders___Internal.Read___AccentDataFishNet.Serializing.Generateds(reader),
				ModelID = reader.ReadString(),
				Variables = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<Vectorio.Entities.VariableContainer>FishNet.Serializing.Generateds(reader),
				HasPipette = reader.ReadBoolean(),
				_pipetteData = GeneratedReaders___Internal.Read___EntityMetadataFishNet.Serializing.Generateds(reader),
				FromSave = reader.ReadBoolean(),
				ApplyFlagsPostCreation = reader.ReadBoolean(),
				PipetteData = GeneratedReaders___Internal.Read___EntityMetadataFishNet.Serializing.Generateds(reader),
				RuntimeID = reader.ReadUInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000748D0 File Offset: 0x00072AD0
		public static SyncType Generateds(Reader reader)
		{
			return (SyncType)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000748EC File Offset: 0x00072AEC
		public static EntityFlags Generateds(Reader reader)
		{
			return (EntityFlags)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00074908 File Offset: 0x00072B08
		public static OnCreationCallback Generateds(Reader reader)
		{
			return new OnCreationCallback
			{
				ID = reader.ReadUInt32(AutoPackType.Packed),
				Type = GeneratedReaders___Internal.Read___Vectorio.Entities.CallbackTypeFishNet.Serializing.Generateds(reader),
				Index = reader.ReadByte()
			};
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00074960 File Offset: 0x00072B60
		public static CallbackType Generateds(Reader reader)
		{
			return (CallbackType)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0007497C File Offset: 0x00072B7C
		public static CreationFlags Generateds(Reader reader)
		{
			return (CreationFlags)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00074998 File Offset: 0x00072B98
		public static CheckFlags Generateds(Reader reader)
		{
			return (CheckFlags)reader.ReadInt32(AutoPackType.Packed);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000749B4 File Offset: 0x00072BB4
		public static AccentData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new AccentData
			{
				use = reader.ReadBoolean(),
				pm = reader.ReadBoolean(),
				sm = reader.ReadBoolean(),
				pmt = reader.ReadInt32(AutoPackType.Packed),
				pme = reader.ReadInt32(AutoPackType.Packed),
				smt = reader.ReadInt32(AutoPackType.Packed),
				sme = reader.ReadInt32(AutoPackType.Packed),
				pc = reader.ReadInt32(AutoPackType.Packed),
				sc = reader.ReadInt32(AutoPackType.Packed)
			};
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00074AA8 File Offset: 0x00072CA8
		public static VariableContainer Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new VariableContainer
			{
				UseType = reader.ReadBoolean(),
				TypeKey = reader.ReadString(),
				C_Index = GeneratedReaders___Internal.Read___System.Nullable`1<System.Byte>FishNet.Serializing.Generateds(reader),
				StringVariables = GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.String>FishNet.Serializing.Generateds(reader),
				IntVariables = GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.Int32>FishNet.Serializing.Generateds(reader),
				FloatVariables = GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.Single>FishNet.Serializing.Generateds(reader),
				BoolVariables = GeneratedReaders___Internal.Read___System.Collections.Generic.Dictionary`2<System.Byte,System.Boolean>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00074B5C File Offset: 0x00072D5C
		public static byte? Nullable(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new byte?(reader.ReadByte());
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00074BA0 File Offset: 0x00072DA0
		public static Dictionary<byte, string> Dictionary(Reader reader)
		{
			return reader.ReadDictionary<byte, string>();
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00074BB8 File Offset: 0x00072DB8
		public static Dictionary<byte, int> Dictionary(Reader reader)
		{
			return reader.ReadDictionary<byte, int>();
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00074BD0 File Offset: 0x00072DD0
		public static Dictionary<byte, float> Dictionary(Reader reader)
		{
			return reader.ReadDictionary<byte, float>();
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00074BE8 File Offset: 0x00072DE8
		public static Dictionary<byte, bool> Dictionary(Reader reader)
		{
			return reader.ReadDictionary<byte, bool>();
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00074C00 File Offset: 0x00072E00
		public static List<VariableContainer> List(Reader reader)
		{
			return reader.ReadListAllocated<VariableContainer>();
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00074C18 File Offset: 0x00072E18
		public static EntityMetadata Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new EntityMetadata
			{
				Context = GeneratedReaders___Internal.Read___MetadataContextFishNet.Serializing.Generateds(reader),
				RuntimeID = GeneratedReaders___Internal.Read___E_IDFishNet.Serializing.Generateds(reader),
				EntityID = reader.ReadString(),
				EntityFlags = GeneratedReaders___Internal.Read___EntityFlagsFishNet.Serializing.Generateds(reader),
				ModelID = reader.ReadString(),
				FactionID = reader.ReadString(),
				AccentData = GeneratedReaders___Internal.Read___AccentDataFishNet.Serializing.Generateds(reader),
				PosX = reader.ReadSingle(AutoPackType.Unpacked),
				PosY = reader.ReadSingle(AutoPackType.Unpacked),
				LinkedEntityID = GeneratedReaders___Internal.Read___E_IDFishNet.Serializing.Generateds(reader),
				Components = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ComponentMetadataWrapper>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00074D1C File Offset: 0x00072F1C
		public static MetadataContext Generateds(Reader reader)
		{
			return (MetadataContext)reader.ReadByte();
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00074D34 File Offset: 0x00072F34
		public static E_ID Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new E_ID
			{
				ID = reader.ReadUInt32(AutoPackType.Packed),
				ctx = GeneratedReaders___Internal.Read___MetadataContextFishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00074D90 File Offset: 0x00072F90
		public static ComponentMetadataWrapper Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ComponentMetadataWrapper
			{
				Type = reader.ReadString()
			};
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00074DD8 File Offset: 0x00072FD8
		public static List<ComponentMetadataWrapper> List(Reader reader)
		{
			return reader.ReadListAllocated<ComponentMetadataWrapper>();
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00074DF0 File Offset: 0x00072FF0
		public static C_EntityCreationData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new C_EntityCreationData
			{
				CreationData = reader.ReadBytesAndSizeAllocated(),
				EntityID = reader.ReadString(),
				RuntimeID = reader.ReadUInt32(AutoPackType.Packed),
				PosX = reader.ReadSingle(AutoPackType.Unpacked),
				PosY = reader.ReadSingle(AutoPackType.Unpacked)
			};
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00074E8C File Offset: 0x0007308C
		public static EntityDamageEvent Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new EntityDamageEvent
			{
				EntityID = reader.ReadUInt32(AutoPackType.Packed),
				DamagerID = reader.ReadUInt32(AutoPackType.Packed),
				Damage = reader.ReadSingle(AutoPackType.Unpacked)
			};
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00074F04 File Offset: 0x00073104
		public static EntityDestroyEvent Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new EntityDestroyEvent
			{
				RuntimeID = reader.ReadUInt32(AutoPackType.Packed),
				DamagerID = reader.ReadUInt32(AutoPackType.Packed),
				Recycle = reader.ReadBoolean()
			};
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00074F78 File Offset: 0x00073178
		public static C_EntityMetadataEvent Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new C_EntityMetadataEvent
			{
				RuntimeID = reader.ReadUInt32(AutoPackType.Packed),
				Metadata = reader.ReadBytesAndSizeAllocated(),
				AsPipette = reader.ReadBoolean()
			};
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00074FE8 File Offset: 0x000731E8
		public static EntityCallbackEvent Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new EntityCallbackEvent
			{
				EntityID = reader.ReadUInt32(AutoPackType.Packed),
				ComponentID = reader.ReadByte(),
				Time = reader.ReadSingle(AutoPackType.Unpacked),
				Variable = GeneratedReaders___Internal.Read___Vectorio.Entities.VariableContainerFishNet.Serializing.Generateds(reader),
				IsFinished = reader.ReadBoolean()
			};
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00075080 File Offset: 0x00073280
		public static SyncEvent Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new SyncEvent
			{
				IsEntity = reader.ReadBoolean(),
				RuntimeID = reader.ReadUInt32(AutoPackType.Packed),
				ComponentID = reader.ReadByte(),
				Data = reader.ReadBytesAndSizeAllocated()
			};
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00075100 File Offset: 0x00073300
		public static ClientSyncManager.ClientStateInformation Generateds(Reader reader)
		{
			return new ClientSyncManager.ClientStateInformation
			{
				ids = GeneratedReaders___Internal.Read___System.Int32[]FishNet.Serializing.Generateds(reader),
				xCoords = GeneratedReaders___Internal.Read___System.Single[]FishNet.Serializing.Generateds(reader),
				yCoords = GeneratedReaders___Internal.Read___System.Single[]FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00075154 File Offset: 0x00073354
		public static int[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<int>();
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0007516C File Offset: 0x0007336C
		public static float[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<float>();
		}
	}
}
