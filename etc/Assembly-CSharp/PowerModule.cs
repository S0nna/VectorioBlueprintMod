using System;
using UnityEngine.Events;

// Token: 0x02000125 RID: 293
public class PowerModule : EntityComponent
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060009AC RID: 2476 RVA: 0x00028DC0 File Offset: 0x00026FC0
	// (remove) Token: 0x060009AD RID: 2477 RVA: 0x00028DF8 File Offset: 0x00026FF8
	public event PowerModule.PowerEvent OnPowerStatusUpdated = delegate(bool <p0>)
	{
	};

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x060009AE RID: 2478 RVA: 0x00028E2D File Offset: 0x0002702D
	public bool IsPowered
	{
		get
		{
			return this._isPowered;
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00028E38 File Offset: 0x00027038
	public override void OnSpawn(bool fromSave)
	{
		this._statusComponent = base.Entity.Get_EComponent<EntityStatus>(true);
		this._statusComponent.OnSpawn(fromSave);
		this.CheckConnection();
		if (!this._listeningToLinkEvents)
		{
			base.Entity.OnLink += this.OnConnectionAdded;
			base.Entity.OnUnlink += this.OnConnectionRemoved;
			this._listeningToLinkEvents = true;
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00028EA6 File Offset: 0x000270A6
	public void SetPowerStatus(bool status)
	{
		this._isPowered = status;
		this._statusComponent.Toggle(!this._isPowered, EntityStatus.Type.Power);
		this.OnPowerStatusUpdated(status);
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00028ED0 File Offset: 0x000270D0
	private void OnPowerOutage()
	{
		this.SetPowerStatus(false);
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00028ED9 File Offset: 0x000270D9
	private void OnPowerRecovered()
	{
		this.SetPowerStatus(true);
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x00028EE2 File Offset: 0x000270E2
	private void OnConnectionAdded(Entity entity)
	{
		this.CheckConnection();
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x00028EE2 File Offset: 0x000270E2
	private void OnConnectionRemoved(Entity entity)
	{
		this.CheckConnection();
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00028EEC File Offset: 0x000270EC
	private void CheckConnection()
	{
		if (base.Entity.HasLinkedEntity && base.Entity.LinkedEntity.Has_EComponent<PowerModule>())
		{
			this._connection = base.Entity.LinkedEntity.Get_EComponent<PowerModule>(false);
			this._connection.OnPowerStatusUpdated += this.SetPowerStatus;
			if (this._listeningToPowerEvents)
			{
				Singleton<Events>.Instance.onPowerExceeded.RemoveListener(new UnityAction(this.OnPowerOutage));
				Singleton<Events>.Instance.onPowerRecovered.RemoveListener(new UnityAction(this.OnPowerRecovered));
				this._listeningToPowerEvents = false;
			}
			this.SetPowerStatus(this._connection.IsPowered);
			return;
		}
		if (this._connection != null)
		{
			this._connection.OnPowerStatusUpdated -= this.SetPowerStatus;
			this._connection = null;
		}
		if (base.Entity.IsPlayerFaction)
		{
			if (!this._listeningToPowerEvents)
			{
				Singleton<Events>.Instance.onPowerExceeded.AddListener(new UnityAction(this.OnPowerOutage));
				Singleton<Events>.Instance.onPowerRecovered.AddListener(new UnityAction(this.OnPowerRecovered));
				this._listeningToPowerEvents = true;
			}
			this.SetPowerStatus(!Singleton<ResourceManager>.Instance.PowerOutage);
		}
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00029034 File Offset: 0x00027234
	public override void OnReset()
	{
		if (this._listeningToLinkEvents)
		{
			base.Entity.OnLink -= this.OnConnectionAdded;
			base.Entity.OnUnlink -= this.OnConnectionRemoved;
			this._listeningToLinkEvents = false;
		}
		if (this._listeningToPowerEvents)
		{
			Singleton<Events>.Instance.onPowerExceeded.RemoveListener(new UnityAction(this.OnPowerOutage));
			Singleton<Events>.Instance.onPowerRecovered.RemoveListener(new UnityAction(this.OnPowerRecovered));
			this._listeningToPowerEvents = false;
		}
	}

	// Token: 0x040005F7 RID: 1527
	private bool _isPowered = true;

	// Token: 0x040005F8 RID: 1528
	private PowerModule _connection;

	// Token: 0x040005F9 RID: 1529
	private bool _listeningToLinkEvents;

	// Token: 0x040005FA RID: 1530
	private bool _listeningToPowerEvents;

	// Token: 0x040005FB RID: 1531
	private EntityStatus _statusComponent;

	// Token: 0x02000126 RID: 294
	// (Invoke) Token: 0x060009B9 RID: 2489
	public delegate void PowerEvent(bool toggle);
}
