using System;
using Vectorio.Entities;

// Token: 0x0200010A RID: 266
public abstract class EntityComponent : BaseComponent
{
	// Token: 0x060008EB RID: 2283 RVA: 0x00026696 File Offset: 0x00024896
	public void Register(Entity entity, byte componentIndex)
	{
		this._entity = entity;
		this._componentIndex = componentIndex;
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x000266A6 File Offset: 0x000248A6
	public Entity Entity
	{
		get
		{
			return this._entity;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060008ED RID: 2285 RVA: 0x000266AE File Offset: 0x000248AE
	public byte ComponentIndex
	{
		get
		{
			return this._componentIndex;
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnSpawn(bool fromSave)
	{
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void OnCreationCallback(Entity entity)
	{
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x000266B6 File Offset: 0x000248B6
	public string EntityID
	{
		get
		{
			return this._entity.GetData().ID;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060008F1 RID: 2289 RVA: 0x000266C8 File Offset: 0x000248C8
	public string FactionID
	{
		get
		{
			return this._entity.FactionID;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x000266D5 File Offset: 0x000248D5
	public bool IsPlayerFaction
	{
		get
		{
			return this._entity.IsPlayerFaction;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x060008F3 RID: 2291 RVA: 0x000266E2 File Offset: 0x000248E2
	public uint RuntimeID
	{
		get
		{
			return this._entity.RuntimeID;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060008F4 RID: 2292 RVA: 0x000266EF File Offset: 0x000248EF
	public ModelObject GetModel
	{
		get
		{
			return this._entity.GetModel;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x000266FC File Offset: 0x000248FC
	public Accent GetAccent
	{
		get
		{
			return this.GetModel.GetAccent();
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00026709 File Offset: 0x00024909
	public bool IsAlly(string factionID)
	{
		return this._entity.IsAlly(factionID);
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x00003212 File Offset: 0x00001412
	public virtual void ApplyVariableContainer(VariableContainer variableContainer)
	{
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x00026718 File Offset: 0x00024918
	public void CancelUpdates()
	{
		if (base.IsUpdating)
		{
			IUpdateable updateable = this.Entity.Get_EComponentInterface<IUpdateable>(this.ComponentIndex);
			if (updateable != null)
			{
				Singleton<EntityManager>.Instance.UnregisterUpdatingComponent(updateable);
				base.IsUpdating = false;
			}
			if (this.Entity.Get_EComponentInterface<ICallbackListener>(this.ComponentIndex) != null)
			{
				Singleton<EntityManager>.Instance.CancelEntityCallback(this.RuntimeID);
				base.IsUpdating = false;
			}
		}
	}

	// Token: 0x0400059A RID: 1434
	private Entity _entity;

	// Token: 0x0400059B RID: 1435
	private byte _componentIndex;
}
