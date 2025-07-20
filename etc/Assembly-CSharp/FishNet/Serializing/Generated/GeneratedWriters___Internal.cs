using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Utilities;

namespace FishNet.Serializing.Generated
{
	// Token: 0x02000395 RID: 917
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedWriters___Internal
	{
		// Token: 0x060017B0 RID: 6064 RVA: 0x00073544 File Offset: 0x00071744
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericWriter<Utilities.CompressedData>.Write = new Action<Writer, Utilities.CompressedData>(GeneratedWriters___Internal.Write___Vectorio.Utilities.Utilities/CompressedDataFishNet.Serializing.Generated);
			GenericWriter<NetworkEventPackage>.Write = new Action<Writer, NetworkEventPackage>(GeneratedWriters___Internal.Write___NetworkEventPackageFishNet.Serializing.Generated);
			GenericWriter<NetworkEventBase>.Write = new Action<Writer, NetworkEventBase>(GeneratedWriters___Internal.Write___NetworkEventBaseFishNet.Serializing.Generated);
			GenericWriter<List<NetworkEventBase>>.Write = new Action<Writer, List<NetworkEventBase>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<NetworkEventBase>FishNet.Serializing.Generated);
			GenericWriter<ResearchChangeEvent>.Write = new Action<Writer, ResearchChangeEvent>(GeneratedWriters___Internal.Write___ResearchChangeEventFishNet.Serializing.Generated);
			GenericWriter<RegionChangeEvent>.Write = new Action<Writer, RegionChangeEvent>(GeneratedWriters___Internal.Write___RegionChangeEventFishNet.Serializing.Generated);
			GenericWriter<EntityCreationData>.Write = new Action<Writer, EntityCreationData>(GeneratedWriters___Internal.Write___Vectorio.Entities.EntityCreationDataFishNet.Serializing.Generated);
			GenericWriter<SyncType>.Write = new Action<Writer, SyncType>(GeneratedWriters___Internal.Write___Vectorio.Entities.SyncTypeFishNet.Serializing.Generated);
			GenericWriter<EntityFlags>.Write = new Action<Writer, EntityFlags>(GeneratedWriters___Internal.Write___EntityFlagsFishNet.Serializing.Generated);
			GenericWriter<OnCreationCallback>.Write = new Action<Writer, OnCreationCallback>(GeneratedWriters___Internal.Write___Vectorio.Entities.OnCreationCallbackFishNet.Serializing.Generated);
			GenericWriter<CallbackType>.Write = new Action<Writer, CallbackType>(GeneratedWriters___Internal.Write___Vectorio.Entities.CallbackTypeFishNet.Serializing.Generated);
			GenericWriter<CreationFlags>.Write = new Action<Writer, CreationFlags>(GeneratedWriters___Internal.Write___Vectorio.Entities.CreationFlagsFishNet.Serializing.Generated);
			GenericWriter<CheckFlags>.Write = new Action<Writer, CheckFlags>(GeneratedWriters___Internal.Write___Vectorio.Entities.CheckFlagsFishNet.Serializing.Generated);
			GenericWriter<AccentData>.Write = new Action<Writer, AccentData>(GeneratedWriters___Internal.Write___AccentDataFishNet.Serializing.Generated);
			GenericWriter<VariableContainer>.Write = new Action<Writer, VariableContainer>(GeneratedWriters___Internal.Write___Vectorio.Entities.VariableContainerFishNet.Serializing.Generated);
			GenericWriter<byte?>.Write = new Action<Writer, byte?>(GeneratedWriters___Internal.Write___System.Nullable`1<System.Byte>FishNet.Serializing.Generated);
			GenericWriter<Dictionary<byte, string>>.Write = new Action<Writer, Dictionary<byte, string>>(GeneratedWriters___Internal.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.String>FishNet.Serializing.Generated);
			GenericWriter<Dictionary<byte, int>>.Write = new Action<Writer, Dictionary<byte, int>>(GeneratedWriters___Internal.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.Int32>FishNet.Serializing.Generated);
			GenericWriter<Dictionary<byte, float>>.Write = new Action<Writer, Dictionary<byte, float>>(GeneratedWriters___Internal.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.Single>FishNet.Serializing.Generated);
			GenericWriter<Dictionary<byte, bool>>.Write = new Action<Writer, Dictionary<byte, bool>>(GeneratedWriters___Internal.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.Boolean>FishNet.Serializing.Generated);
			GenericWriter<List<VariableContainer>>.Write = new Action<Writer, List<VariableContainer>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<Vectorio.Entities.VariableContainer>FishNet.Serializing.Generated);
			GenericWriter<EntityMetadata>.Write = new Action<Writer, EntityMetadata>(GeneratedWriters___Internal.Write___EntityMetadataFishNet.Serializing.Generated);
			GenericWriter<MetadataContext>.Write = new Action<Writer, MetadataContext>(GeneratedWriters___Internal.Write___MetadataContextFishNet.Serializing.Generated);
			GenericWriter<E_ID>.Write = new Action<Writer, E_ID>(GeneratedWriters___Internal.Write___E_IDFishNet.Serializing.Generated);
			GenericWriter<ComponentMetadataWrapper>.Write = new Action<Writer, ComponentMetadataWrapper>(GeneratedWriters___Internal.Write___ComponentMetadataWrapperFishNet.Serializing.Generated);
			GenericWriter<List<ComponentMetadataWrapper>>.Write = new Action<Writer, List<ComponentMetadataWrapper>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ComponentMetadataWrapper>FishNet.Serializing.Generated);
			GenericWriter<C_EntityCreationData>.Write = new Action<Writer, C_EntityCreationData>(GeneratedWriters___Internal.Write___C_EntityCreationDataFishNet.Serializing.Generated);
			GenericWriter<EntityDamageEvent>.Write = new Action<Writer, EntityDamageEvent>(GeneratedWriters___Internal.Write___EntityDamageEventFishNet.Serializing.Generated);
			GenericWriter<EntityDestroyEvent>.Write = new Action<Writer, EntityDestroyEvent>(GeneratedWriters___Internal.Write___EntityDestroyEventFishNet.Serializing.Generated);
			GenericWriter<C_EntityMetadataEvent>.Write = new Action<Writer, C_EntityMetadataEvent>(GeneratedWriters___Internal.Write___C_EntityMetadataEventFishNet.Serializing.Generated);
			GenericWriter<EntityCallbackEvent>.Write = new Action<Writer, EntityCallbackEvent>(GeneratedWriters___Internal.Write___EntityCallbackEventFishNet.Serializing.Generated);
			GenericWriter<SyncEvent>.Write = new Action<Writer, SyncEvent>(GeneratedWriters___Internal.Write___SyncEventFishNet.Serializing.Generated);
			GenericWriter<ClientSyncManager.ClientStateInformation>.Write = new Action<Writer, ClientSyncManager.ClientStateInformation>(GeneratedWriters___Internal.Write___ClientSyncManager/ClientStateInformationFishNet.Serializing.Generated);
			GenericWriter<int[]>.Write = new Action<Writer, int[]>(GeneratedWriters___Internal.Write___System.Int32[]FishNet.Serializing.Generated);
			GenericWriter<float[]>.Write = new Action<Writer, float[]>(GeneratedWriters___Internal.Write___System.Single[]FishNet.Serializing.Generated);
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x000737A4 File Offset: 0x000719A4
		public static void Generated(this Writer writer, Utilities.CompressedData value)
		{
			writer.WriteBytesAndSize(value.Data);
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000737C4 File Offset: 0x000719C4
		public static void Generated(this Writer writer, NetworkEventPackage value)
		{
			writer.Write___System.Collections.Generic.List`1<NetworkEventBase>FishNet.Serializing.Generated(value.events);
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000737E4 File Offset: 0x000719E4
		public static void Generated(this Writer writer, NetworkEventBase value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00073818 File Offset: 0x00071A18
		public static void List(this Writer writer, List<NetworkEventBase> value)
		{
			writer.WriteList<NetworkEventBase>(value);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00073834 File Offset: 0x00071A34
		public static void Generated(this Writer writer, ResearchChangeEvent value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ID);
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0007387C File Offset: 0x00071A7C
		public static void Generated(this Writer writer, RegionChangeEvent value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteUInt32(value.GatewayID, AutoPackType.Packed);
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x000738C8 File Offset: 0x00071AC8
		public static void Generated(this Writer writer, EntityCreationData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___Vectorio.Entities.SyncTypeFishNet.Serializing.Generated(value.SyncType);
			writer.WriteBoolean(value.IsBlueprint);
			writer.WriteString(value.EntityID);
			writer.Write___EntityFlagsFishNet.Serializing.Generated(value.EntityFlags);
			writer.WriteBoolean(value._hasRuntimeID);
			writer.WriteUInt32(value._runtimeID, AutoPackType.Packed);
			writer.Write___Vectorio.Entities.OnCreationCallbackFishNet.Serializing.Generated(value.Callback);
			writer.WriteString(value.FactionID);
			writer.WriteSingle(value.PosX, AutoPackType.Unpacked);
			writer.WriteSingle(value.PosY, AutoPackType.Unpacked);
			writer.Write___Vectorio.Entities.CreationFlagsFishNet.Serializing.Generated(value.Flags);
			writer.Write___Vectorio.Entities.CheckFlagsFishNet.Serializing.Generated(value.Checks);
			writer.Write___AccentDataFishNet.Serializing.Generated(value.Accent);
			writer.WriteString(value.ModelID);
			writer.Write___System.Collections.Generic.List`1<Vectorio.Entities.VariableContainer>FishNet.Serializing.Generated(value.Variables);
			writer.WriteBoolean(value.HasPipette);
			writer.Write___EntityMetadataFishNet.Serializing.Generated(value._pipetteData);
			writer.WriteBoolean(value.FromSave);
			writer.WriteBoolean(value.ApplyFlagsPostCreation);
			writer.Write___EntityMetadataFishNet.Serializing.Generated(value.PipetteData);
			writer.WriteUInt32(value.RuntimeID, AutoPackType.Packed);
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00073A8C File Offset: 0x00071C8C
		public static void Generated(this Writer writer, SyncType value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00073AAC File Offset: 0x00071CAC
		public static void Generated(this Writer writer, EntityFlags value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00073ACC File Offset: 0x00071CCC
		public static void Generated(this Writer writer, OnCreationCallback value)
		{
			writer.WriteUInt32(value.ID, AutoPackType.Packed);
			writer.Write___Vectorio.Entities.CallbackTypeFishNet.Serializing.Generated(value.Type);
			writer.WriteByte(value.Index);
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00073B14 File Offset: 0x00071D14
		public static void Generated(this Writer writer, CallbackType value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00073B34 File Offset: 0x00071D34
		public static void Generated(this Writer writer, CreationFlags value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00073B54 File Offset: 0x00071D54
		public static void Generated(this Writer writer, CheckFlags value)
		{
			writer.WriteInt32((int)value, AutoPackType.Packed);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00073B74 File Offset: 0x00071D74
		public static void Generated(this Writer writer, AccentData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBoolean(value.use);
			writer.WriteBoolean(value.pm);
			writer.WriteBoolean(value.sm);
			writer.WriteInt32(value.pmt, AutoPackType.Packed);
			writer.WriteInt32(value.pme, AutoPackType.Packed);
			writer.WriteInt32(value.smt, AutoPackType.Packed);
			writer.WriteInt32(value.sme, AutoPackType.Packed);
			writer.WriteInt32(value.pc, AutoPackType.Packed);
			writer.WriteInt32(value.sc, AutoPackType.Packed);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00073C68 File Offset: 0x00071E68
		public static void Generated(this Writer writer, VariableContainer value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBoolean(value.UseType);
			writer.WriteString(value.TypeKey);
			writer.Write___System.Nullable`1<System.Byte>FishNet.Serializing.Generated(value.C_Index);
			writer.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.String>FishNet.Serializing.Generated(value.StringVariables);
			writer.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.Int32>FishNet.Serializing.Generated(value.IntVariables);
			writer.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.Single>FishNet.Serializing.Generated(value.FloatVariables);
			writer.Write___System.Collections.Generic.Dictionary`2<System.Byte,System.Boolean>FishNet.Serializing.Generated(value.BoolVariables);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00073D1C File Offset: 0x00071F1C
		public static void Nullable(this Writer writer, byte? value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteByte(value.Value);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00073D64 File Offset: 0x00071F64
		public static void Dictionary(this Writer writer, Dictionary<byte, string> value)
		{
			writer.WriteDictionary<byte, string>(value);
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00073D80 File Offset: 0x00071F80
		public static void Dictionary(this Writer writer, Dictionary<byte, int> value)
		{
			writer.WriteDictionary<byte, int>(value);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00073D9C File Offset: 0x00071F9C
		public static void Dictionary(this Writer writer, Dictionary<byte, float> value)
		{
			writer.WriteDictionary<byte, float>(value);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00073DB8 File Offset: 0x00071FB8
		public static void Dictionary(this Writer writer, Dictionary<byte, bool> value)
		{
			writer.WriteDictionary<byte, bool>(value);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00073DD4 File Offset: 0x00071FD4
		public static void List(this Writer writer, List<VariableContainer> value)
		{
			writer.WriteList<VariableContainer>(value);
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00073DF0 File Offset: 0x00071FF0
		public static void Generated(this Writer writer, EntityMetadata value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___MetadataContextFishNet.Serializing.Generated(value.Context);
			writer.Write___E_IDFishNet.Serializing.Generated(value.RuntimeID);
			writer.WriteString(value.EntityID);
			writer.Write___EntityFlagsFishNet.Serializing.Generated(value.EntityFlags);
			writer.WriteString(value.ModelID);
			writer.WriteString(value.FactionID);
			writer.Write___AccentDataFishNet.Serializing.Generated(value.AccentData);
			writer.WriteSingle(value.PosX, AutoPackType.Unpacked);
			writer.WriteSingle(value.PosY, AutoPackType.Unpacked);
			writer.Write___E_IDFishNet.Serializing.Generated(value.LinkedEntityID);
			writer.Write___System.Collections.Generic.List`1<ComponentMetadataWrapper>FishNet.Serializing.Generated(value.Components);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00073EF4 File Offset: 0x000720F4
		public static void Generated(this Writer writer, MetadataContext value)
		{
			writer.WriteByte((byte)value);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00073F10 File Offset: 0x00072110
		public static void Generated(this Writer writer, E_ID value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteUInt32(value.ID, AutoPackType.Packed);
			writer.Write___MetadataContextFishNet.Serializing.Generated(value.ctx);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00073F70 File Offset: 0x00072170
		public static void Generated(this Writer writer, ComponentMetadataWrapper value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.Type);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00073FB8 File Offset: 0x000721B8
		public static void List(this Writer writer, List<ComponentMetadataWrapper> value)
		{
			writer.WriteList<ComponentMetadataWrapper>(value);
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00073FD4 File Offset: 0x000721D4
		public static void Generated(this Writer writer, C_EntityCreationData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBytesAndSize(value.CreationData);
			writer.WriteString(value.EntityID);
			writer.WriteUInt32(value.RuntimeID, AutoPackType.Packed);
			writer.WriteSingle(value.PosX, AutoPackType.Unpacked);
			writer.WriteSingle(value.PosY, AutoPackType.Unpacked);
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00074074 File Offset: 0x00072274
		public static void Generated(this Writer writer, EntityDamageEvent value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteUInt32(value.EntityID, AutoPackType.Packed);
			writer.WriteUInt32(value.DamagerID, AutoPackType.Packed);
			writer.WriteSingle(value.Damage, AutoPackType.Unpacked);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x000740F0 File Offset: 0x000722F0
		public static void Generated(this Writer writer, EntityDestroyEvent value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteUInt32(value.RuntimeID, AutoPackType.Packed);
			writer.WriteUInt32(value.DamagerID, AutoPackType.Packed);
			writer.WriteBoolean(value.Recycle);
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00074164 File Offset: 0x00072364
		public static void Generated(this Writer writer, C_EntityMetadataEvent value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteUInt32(value.RuntimeID, AutoPackType.Packed);
			writer.WriteBytesAndSize(value.Metadata);
			writer.WriteBoolean(value.AsPipette);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x000741D4 File Offset: 0x000723D4
		public static void Generated(this Writer writer, EntityCallbackEvent value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteUInt32(value.EntityID, AutoPackType.Packed);
			writer.WriteByte(value.ComponentID);
			writer.WriteSingle(value.Time, AutoPackType.Unpacked);
			writer.Write___Vectorio.Entities.VariableContainerFishNet.Serializing.Generated(value.Variable);
			writer.WriteBoolean(value.IsFinished);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0007426C File Offset: 0x0007246C
		public static void Generated(this Writer writer, SyncEvent value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBoolean(value.IsEntity);
			writer.WriteUInt32(value.RuntimeID, AutoPackType.Packed);
			writer.WriteByte(value.ComponentID);
			writer.WriteBytesAndSize(value.Data);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x000742F0 File Offset: 0x000724F0
		public static void Generated(this Writer writer, ClientSyncManager.ClientStateInformation value)
		{
			writer.Write___System.Int32[]FishNet.Serializing.Generated(value.ids);
			writer.Write___System.Single[]FishNet.Serializing.Generated(value.xCoords);
			writer.Write___System.Single[]FishNet.Serializing.Generated(value.yCoords);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00074334 File Offset: 0x00072534
		public static void Generated(this Writer writer, int[] value)
		{
			writer.WriteArray<int>(value);
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00074350 File Offset: 0x00072550
		public static void Generated(this Writer writer, float[] value)
		{
			writer.WriteArray<float>(value);
		}
	}
}
