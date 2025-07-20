using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000106 RID: 262
public class Entity : BaseComponent
{
	// Token: 0x0600088E RID: 2190 RVA: 0x000257D8 File Offset: 0x000239D8
	public EntityData GetData()
	{
		return this._entityData;
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x000257E0 File Offset: 0x000239E0
	public string ID
	{
		get
		{
			return this.GetData().ID;
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x000257ED File Offset: 0x000239ED
	private bool Has_EFlag(EntityFlags flag)
	{
		return (this._flags & flag) == flag;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x000257FA File Offset: 0x000239FA
	private void Set_EFlag(EntityFlags flag, bool value)
	{
		if (value)
		{
			this._flags |= flag;
			return;
		}
		this._flags &= ~flag;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0002581D File Offset: 0x00023A1D
	public void Parse_EFlags(EntityFlags flags)
	{
		this.Reset_EFlags();
		this._flags = flags;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0002582C File Offset: 0x00023A2C
	public void Reset_EFlags()
	{
		this._flags = (EntityFlags)0;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00025835 File Offset: 0x00023A35
	public EntityFlags Extract_EFlags()
	{
		return this._flags;
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000895 RID: 2197 RVA: 0x0002583D File Offset: 0x00023A3D
	public bool Has_EFlag_IsDead
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsDead);
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00025846 File Offset: 0x00023A46
	public void Set_EFlag_IsDead(bool value)
	{
		this.Set_EFlag(EntityFlags.IsDead, value);
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000897 RID: 2199 RVA: 0x00025850 File Offset: 0x00023A50
	public bool Has_EFlag_IsEditable
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsEditable);
		}
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00025859 File Offset: 0x00023A59
	public void Set_EFlag_IsEditable(bool value)
	{
		this.Set_EFlag(EntityFlags.IsEditable, value);
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000899 RID: 2201 RVA: 0x00025863 File Offset: 0x00023A63
	public bool Has_EFlag_IsTargetable
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsTargetable);
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0002586C File Offset: 0x00023A6C
	public void Set_EFlag_IsTargetable(bool value)
	{
		this.Set_EFlag(EntityFlags.IsTargetable, value);
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600089B RID: 2203 RVA: 0x00025876 File Offset: 0x00023A76
	public bool Has_EFlag_IsWorldFeature
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsWorldFeature);
		}
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0002587F File Offset: 0x00023A7F
	public void Set_EFlag_IsWorldFeature(bool value)
	{
		this.Set_EFlag(EntityFlags.IsWorldFeature, value);
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x0600089D RID: 2205 RVA: 0x00025889 File Offset: 0x00023A89
	public bool Has_EFlag_IsCostsApplied
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsCostsApplied);
		}
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00025893 File Offset: 0x00023A93
	public void Set_EFlag_IsCostsApplied(bool value)
	{
		this.Set_EFlag(EntityFlags.IsCostsApplied, value);
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x0600089F RID: 2207 RVA: 0x0002589E File Offset: 0x00023A9E
	public bool Has_EFlag_IsFreeEntity
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsFreeEntity);
		}
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x000258A8 File Offset: 0x00023AA8
	public void Set_EFlag_IsFreeEntity(bool value)
	{
		this.Set_EFlag(EntityFlags.IsFreeEntity, value);
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x000258B3 File Offset: 0x00023AB3
	public bool Has_EFlag_IsInvincible
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsInvincible);
		}
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x000258BD File Offset: 0x00023ABD
	public void Set_EFlag_IsInvincible(bool value)
	{
		this.Set_EFlag(EntityFlags.IsInvincible, value);
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x000258C8 File Offset: 0x00023AC8
	public bool Has_EFlag_IsBlueprint
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsBlueprint);
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x000258D5 File Offset: 0x00023AD5
	public void Set_EFlag_IsBlueprint(bool value)
	{
		this.Set_EFlag(EntityFlags.IsBlueprint, value);
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060008A5 RID: 2213 RVA: 0x000258E3 File Offset: 0x00023AE3
	public bool Has_EFlag_IsCloaked
	{
		get
		{
			return this.Has_EFlag(EntityFlags.IsCloaked);
		}
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x000258F0 File Offset: 0x00023AF0
	public void Set_EFlag_IsCloaked(bool value)
	{
		this.Set_EFlag(EntityFlags.IsCloaked, value);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x000258FE File Offset: 0x00023AFE
	public void RegisterStat(Stat stat)
	{
		this._stats.Add(stat);
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0002590C File Offset: 0x00023B0C
	public List<Stat> Stats
	{
		get
		{
			if (this._hasBridge)
			{
				List<Stat> list = new List<Stat>();
				list.AddRange(this._pipetteBridge.Stats);
				list.AddRange(this._stats);
				return list;
			}
			return this._stats;
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0002593F File Offset: 0x00023B3F
	// (set) Token: 0x060008AA RID: 2218 RVA: 0x00025947 File Offset: 0x00023B47
	public bool IsSaveable
	{
		get
		{
			return this._isSaveable;
		}
		set
		{
			this._isSaveable = value;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060008AB RID: 2219 RVA: 0x00025950 File Offset: 0x00023B50
	// (set) Token: 0x060008AC RID: 2220 RVA: 0x00025961 File Offset: 0x00023B61
	public bool IsOnScreen
	{
		get
		{
			return !MapView.IsEnabled && this._isOnScreen;
		}
		set
		{
			this._isOnScreen = value;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060008AD RID: 2221 RVA: 0x0002596A File Offset: 0x00023B6A
	// (set) Token: 0x060008AE RID: 2222 RVA: 0x00025972 File Offset: 0x00023B72
	public bool AreComponentsCreated
	{
		get
		{
			return this._componentsInitialized;
		}
		set
		{
			this._componentsInitialized = value;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x0002597B File Offset: 0x00023B7B
	public uint RuntimeID
	{
		get
		{
			return this._runtimeID;
		}
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00025983 File Offset: 0x00023B83
	public void SetRuntimeID(uint runtimeID)
	{
		this._runtimeID = runtimeID;
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060008B1 RID: 2225 RVA: 0x0002598C File Offset: 0x00023B8C
	// (remove) Token: 0x060008B2 RID: 2226 RVA: 0x000259C4 File Offset: 0x00023BC4
	public event Entity.LinkEvent OnLink = delegate(Entity <p0>)
	{
	};

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060008B3 RID: 2227 RVA: 0x000259FC File Offset: 0x00023BFC
	// (remove) Token: 0x060008B4 RID: 2228 RVA: 0x00025A34 File Offset: 0x00023C34
	public event Entity.LinkEvent OnUnlink = delegate(Entity <p0>)
	{
	};

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060008B5 RID: 2229 RVA: 0x00025A6C File Offset: 0x00023C6C
	// (remove) Token: 0x060008B6 RID: 2230 RVA: 0x00025AA4 File Offset: 0x00023CA4
	public event Entity.LinkEvent OnEntityLinked = delegate(Entity <p0>)
	{
	};

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060008B7 RID: 2231 RVA: 0x00025ADC File Offset: 0x00023CDC
	// (remove) Token: 0x060008B8 RID: 2232 RVA: 0x00025B14 File Offset: 0x00023D14
	public event Entity.LinkEvent OnEntityUnlinked = delegate(Entity <p0>)
	{
	};

	// Token: 0x060008B9 RID: 2233 RVA: 0x00025B4C File Offset: 0x00023D4C
	public void Link(Entity entity)
	{
		if (entity == null)
		{
			Debug.Log("[ENTITY] Cannot link to null entity.");
			return;
		}
		if (entity == this)
		{
			Debug.Log("[ENTITY] Cannot link entity to itself.");
			return;
		}
		entity.OnLinked(this);
		Entity.LinkEvent onEntityLinked = this.OnEntityLinked;
		if (onEntityLinked == null)
		{
			return;
		}
		onEntityLinked(entity);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00025B99 File Offset: 0x00023D99
	public void Unlink(Entity entity)
	{
		if (entity == null)
		{
			Debug.Log("[ENTITY] Cannot unlink null entity.");
			return;
		}
		entity.OnUnlinked(this);
		Entity.LinkEvent onEntityUnlinked = this.OnEntityUnlinked;
		if (onEntityUnlinked == null)
		{
			return;
		}
		onEntityUnlinked(entity);
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00025BC8 File Offset: 0x00023DC8
	public void OnLinked(Entity entity)
	{
		if (entity == null)
		{
			Debug.Log("[ENTITY] Cannot link to null entity.");
			return;
		}
		if (entity == this)
		{
			Debug.Log("[ENTITY] Cannot link entity to itself.");
			return;
		}
		if (this._hasLink)
		{
			Debug.Log("[ENTITY] Already linked to an entity. Unlink first.");
			return;
		}
		this._linkedEntity = entity;
		this._hasLink = true;
		Entity.LinkEvent onLink = this.OnLink;
		if (onLink == null)
		{
			return;
		}
		onLink(entity);
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00025C30 File Offset: 0x00023E30
	public void OnUnlinked(Entity entity)
	{
		if (entity == null)
		{
			Debug.Log("[ENTITY] Cannot unlink null entity.");
			return;
		}
		if (entity != this._linkedEntity)
		{
			Debug.Log(string.Concat(new string[]
			{
				"[ENTITY] Could not unlink entity ",
				entity.name,
				" from ",
				base.name,
				". It's not the currently linked entity."
			}));
			return;
		}
		this._linkedEntity = null;
		this._hasLink = false;
		Entity.LinkEvent onUnlink = this.OnUnlink;
		if (onUnlink == null)
		{
			return;
		}
		onUnlink(entity);
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060008BD RID: 2237 RVA: 0x00025CB9 File Offset: 0x00023EB9
	public Entity LinkedEntity
	{
		get
		{
			return this._linkedEntity;
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060008BE RID: 2238 RVA: 0x00025CC1 File Offset: 0x00023EC1
	public bool HasLinkedEntity
	{
		get
		{
			return this._hasLink;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060008BF RID: 2239 RVA: 0x00025CC9 File Offset: 0x00023EC9
	public FOW_Cloak Cloak
	{
		get
		{
			return this._cloak;
		}
	}

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060008C0 RID: 2240 RVA: 0x00025CD4 File Offset: 0x00023ED4
	// (remove) Token: 0x060008C1 RID: 2241 RVA: 0x00025D0C File Offset: 0x00023F0C
	public event Entity.OnMetadataApplied onMetadataApplied;

	// Token: 0x060008C2 RID: 2242 RVA: 0x00025D41 File Offset: 0x00023F41
	public List<EntityComponent> GetComponentValues()
	{
		return this._components.Values.ToList<EntityComponent>();
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00025D54 File Offset: 0x00023F54
	public T Add_EComponent<T>() where T : EntityComponent
	{
		if (this.Has_EComponent<T>())
		{
			return this.Get_EComponent<T>(false);
		}
		if (this._componentCounter >= 100)
		{
			Debug.Log("[ENTITY] Max components reached for " + base.name);
			return default(T);
		}
		T t = base.gameObject.AddComponent<T>();
		this._components.Add(typeof(T), t);
		this._byteCache.Add(this._componentCounter, t);
		EntityComponent entityComponent = t;
		byte componentCounter = this._componentCounter;
		this._componentCounter = componentCounter + 1;
		entityComponent.Register(this, componentCounter);
		return t;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00025DF8 File Offset: 0x00023FF8
	public bool Has_EComponent<T>() where T : EntityComponent
	{
		Type typeFromHandle = typeof(T);
		if (this._components.ContainsKey(typeFromHandle))
		{
			return true;
		}
		foreach (Type c in this._components.Keys)
		{
			if (typeFromHandle.IsAssignableFrom(c))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00025E74 File Offset: 0x00024074
	public T Get_EComponent<T>(bool createIfNotFound) where T : EntityComponent
	{
		Type typeFromHandle = typeof(T);
		EntityComponent entityComponent;
		if (this._components.TryGetValue(typeFromHandle, out entityComponent))
		{
			return entityComponent as T;
		}
		foreach (KeyValuePair<Type, EntityComponent> keyValuePair in this._components)
		{
			if (typeFromHandle.IsAssignableFrom(keyValuePair.Key))
			{
				return keyValuePair.Value as T;
			}
		}
		if (createIfNotFound)
		{
			return this.Add_EComponent<T>();
		}
		return default(T);
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00025F24 File Offset: 0x00024124
	public bool TryGet_EComponent(Type componentType, out EntityComponent component)
	{
		if (this._components.TryGetValue(componentType, out component))
		{
			return true;
		}
		component = null;
		return false;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00025F3B File Offset: 0x0002413B
	public bool TryGet_EComponent(byte index, out EntityComponent component)
	{
		if (this._byteCache.TryGetValue(index, out component))
		{
			return true;
		}
		component = null;
		return false;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00025F54 File Offset: 0x00024154
	public TInterface Get_EComponentInterface<TInterface>(byte index) where TInterface : class
	{
		if (this._byteCache.ContainsKey(index))
		{
			TInterface tinterface = this._byteCache[index] as TInterface;
			if (tinterface != null)
			{
				return tinterface;
			}
		}
		return default(TInterface);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00025F9C File Offset: 0x0002419C
	public void Remove_EComponent<T>() where T : EntityComponent
	{
		if (!this.Has_EComponent<T>())
		{
			Debug.Log("[ENTITY] Could not remove entity component because it does not exist!");
			return;
		}
		T component = base.gameObject.GetComponent<T>();
		component.OnReset();
		this._components.Remove(typeof(T));
		this._byteCache.Remove(component.ComponentIndex);
		Object.Destroy(component);
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0002600C File Offset: 0x0002420C
	public void Apply_ECVariableContainers(List<VariableContainer> variableContainers)
	{
		foreach (VariableContainer variableContainer in variableContainers)
		{
			variableContainer.ApplyToEntity(this);
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060008CB RID: 2251 RVA: 0x00026058 File Offset: 0x00024258
	// (set) Token: 0x060008CC RID: 2252 RVA: 0x00026060 File Offset: 0x00024260
	public Entity PipetteBridge
	{
		get
		{
			return this._pipetteBridge;
		}
		set
		{
			this._pipetteBridge = value;
			this._hasBridge = true;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060008CD RID: 2253 RVA: 0x00026070 File Offset: 0x00024270
	public bool HasBridge
	{
		get
		{
			return this._hasBridge;
		}
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00026078 File Offset: 0x00024278
	public EntityMetadata ExtractMetadata(bool asPipette, MetadataContext context = MetadataContext.Global)
	{
		if (this._hasBridge && asPipette)
		{
			return this._pipetteBridge.ExtractMetadata(asPipette, context);
		}
		List<ComponentMetadataWrapper> list = new List<ComponentMetadataWrapper>();
		foreach (EntityComponent entityComponent in this._components.Values)
		{
			Type metadataType = MetadataTypeCache.GetMetadataType(entityComponent.GetType());
			if (metadataType != null)
			{
				ComponentMetadataWrapper componentMetadataWrapper = (ComponentMetadataWrapper)Activator.CreateInstance(metadataType);
				componentMetadataWrapper.GetValuesFromComponent(entityComponent, context);
				list.Add(componentMetadataWrapper);
			}
		}
		return new EntityMetadata(this, list, context);
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00026124 File Offset: 0x00024324
	public void ApplyMetadata(EntityMetadata metadata, bool asPipette)
	{
		Entity.OnMetadataApplied onMetadataApplied = this.onMetadataApplied;
		if (onMetadataApplied != null)
		{
			onMetadataApplied(metadata, asPipette);
		}
		if (this._hasBridge && asPipette)
		{
			this._pipetteBridge.ApplyMetadata(metadata, asPipette);
			return;
		}
		Entity entity;
		if (metadata.LinkedEntityID != null && metadata.LinkedEntityID.TryGetEntity(out entity))
		{
			entity.Link(this);
		}
		foreach (ComponentMetadataWrapper componentMetadataWrapper in metadata.Components)
		{
			Type type = Type.GetType(componentMetadataWrapper.Type);
			if (type == null)
			{
				Debug.Log("[METADATA] Unable to find type for " + componentMetadataWrapper.Type);
			}
			else
			{
				foreach (EntityComponent entityComponent in this._components.Values)
				{
					if (type.IsAssignableFrom(entityComponent.GetType()))
					{
						componentMetadataWrapper.SetValuesToComponent(entityComponent, asPipette, metadata.Context);
						break;
					}
				}
			}
		}
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00026250 File Offset: 0x00024450
	public void QuickEdit()
	{
		foreach (EntityComponent entityComponent in this.GetComponentValues())
		{
			IMouseListener mouseListener = entityComponent as IMouseListener;
			if (mouseListener != null)
			{
				mouseListener.OnQuickEdit();
			}
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060008D1 RID: 2257 RVA: 0x000262AC File Offset: 0x000244AC
	// (set) Token: 0x060008D2 RID: 2258 RVA: 0x000262B4 File Offset: 0x000244B4
	public ModelObject GetModel
	{
		get
		{
			return this._model;
		}
		set
		{
			this._model = value;
		}
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x000262BD File Offset: 0x000244BD
	public void SetModel(ModelObject model)
	{
		if (model != null)
		{
			if (this._model != null)
			{
				Object.Destroy(this._model.gameObject);
			}
			this._model = model;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x000262ED File Offset: 0x000244ED
	// (set) Token: 0x060008D5 RID: 2261 RVA: 0x000262F5 File Offset: 0x000244F5
	public string FactionID
	{
		get
		{
			return this._factionID;
		}
		set
		{
			this._factionID = value;
			this._isPlayerFaction = Singleton<FactionManager>.Instance.IsPlayerFaction(this._factionID);
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00026314 File Offset: 0x00024514
	public bool IsPlayerFaction
	{
		get
		{
			return this._isPlayerFaction;
		}
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0002631C File Offset: 0x0002451C
	public bool IsAlly(string faction)
	{
		return this._factionID == faction;
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0002632A File Offset: 0x0002452A
	public void OnInitialize(EntityData data)
	{
		this._entityData = data;
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00026334 File Offset: 0x00024534
	public void OnSpawn()
	{
		if (this._entityData.broadcastCreation)
		{
			Singleton<Events>.Instance.onEntityCreated.Invoke(this);
		}
		if (this._entityData.usePlacementAnimation && this.GetModel != null)
		{
			Singleton<AnimationManager>.Instance.RegisterPlacementAnimation(this.GetModel.transform, false);
		}
		if (this._entityData.usePlacementSound && this.IsOnScreen && !this.Has_EFlag_IsBlueprint)
		{
			if (this._entityData.useCustomSound)
			{
				Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._entityData.placeSound, "sp", base.transform.position, 1f, true, 0.9f, 1.1f, false);
			}
			else
			{
				Singleton<AudioPlayer>.Instance.PlayPlacementSound(base.transform.position);
			}
		}
		if (Singleton<FogOfWarHandler>.Instance.IsEnabled() && !Singleton<FactionManager>.Instance.IsPlayerFaction(this.FactionID))
		{
			if (this.Has_EComponent<Illuminator>())
			{
				if (this.Has_EComponent<FOW_Cloak>())
				{
					this._cloak = null;
					this.Remove_EComponent<FOW_Cloak>();
					return;
				}
			}
			else if (!this.Has_EComponent<FOW_Cloak>())
			{
				this._cloak = this.Add_EComponent<FOW_Cloak>();
				this._cloak.OnSpawn(false);
				if (this.GetModel != null)
				{
					foreach (ModelObject.Layer layer in this.GetModel.Layers)
					{
						this._cloak.AddLayer(layer.spriteRenderer);
					}
				}
			}
		}
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x000264D8 File Offset: 0x000246D8
	public void DestroyEntity(bool recycle)
	{
		Singleton<EntityManager>.Instance.Destroy(this.RuntimeID, recycle);
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x000264EC File Offset: 0x000246EC
	public override void OnReset()
	{
		if (this._hasLink)
		{
			this._linkedEntity.Unlink(this);
		}
		this.Set_EFlag_IsDead(true);
		foreach (EntityComponent entityComponent in this._components.Values)
		{
			entityComponent.OnReset();
			entityComponent.CancelUpdates();
			if (entityComponent is IUpdateable && entityComponent.IsUpdating)
			{
				Singleton<EntityManager>.Instance.UnregisterUpdatingComponent((IUpdateable)entityComponent);
			}
		}
		if (this._cloak != null)
		{
			this.Remove_EComponent<FOW_Cloak>();
			this._cloak = null;
		}
		ModelObject getModel = this.GetModel;
		if (getModel == null)
		{
			return;
		}
		getModel.Reset();
	}

	// Token: 0x0400057E RID: 1406
	private EntityData _entityData;

	// Token: 0x0400057F RID: 1407
	private EntityFlags _flags;

	// Token: 0x04000580 RID: 1408
	private List<Stat> _stats = new List<Stat>();

	// Token: 0x04000581 RID: 1409
	private bool _isSaveable = true;

	// Token: 0x04000582 RID: 1410
	[SerializeField]
	private bool _isOnScreen;

	// Token: 0x04000583 RID: 1411
	private uint _runtimeID;

	// Token: 0x04000584 RID: 1412
	private byte _componentCounter;

	// Token: 0x04000585 RID: 1413
	private bool _componentsInitialized;

	// Token: 0x04000586 RID: 1414
	private Entity _linkedEntity;

	// Token: 0x04000587 RID: 1415
	private bool _hasLink;

	// Token: 0x0400058C RID: 1420
	private FOW_Cloak _cloak;

	// Token: 0x0400058D RID: 1421
	private Dictionary<Type, EntityComponent> _components = new Dictionary<Type, EntityComponent>();

	// Token: 0x0400058E RID: 1422
	private Dictionary<byte, EntityComponent> _byteCache = new Dictionary<byte, EntityComponent>();

	// Token: 0x04000590 RID: 1424
	private Entity _pipetteBridge;

	// Token: 0x04000591 RID: 1425
	private bool _hasBridge;

	// Token: 0x04000592 RID: 1426
	private ModelObject _model;

	// Token: 0x04000593 RID: 1427
	private string _factionID = "default";

	// Token: 0x04000594 RID: 1428
	private bool _isPlayerFaction;

	// Token: 0x02000107 RID: 263
	// (Invoke) Token: 0x060008DE RID: 2270
	public delegate void LinkEvent(Entity entity);

	// Token: 0x02000108 RID: 264
	// (Invoke) Token: 0x060008E2 RID: 2274
	public delegate void OnMetadataApplied(EntityMetadata metadata, bool asPipette);
}
