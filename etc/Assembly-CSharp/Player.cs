using System;
using FishNet.Object;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000187 RID: 391
public class Player : NetworkBehaviour
{
	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000D30 RID: 3376 RVA: 0x000397CF File Offset: 0x000379CF
	public int SyncID
	{
		get
		{
			return this._syncID;
		}
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x000397D7 File Offset: 0x000379D7
	public FactionData GetFactionData()
	{
		return this._factionData;
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000D32 RID: 3378 RVA: 0x000397DF File Offset: 0x000379DF
	public Vector2 GetHologramPosition
	{
		get
		{
			return this.hologram.transform.position;
		}
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x000397F6 File Offset: 0x000379F6
	public void SetHologramPosition(Vector2 position)
	{
		this.hologram.transform.position = position;
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00039810 File Offset: 0x00037A10
	public override void OnStartClient()
	{
		base.OnStartClient();
		if (base.IsOwner)
		{
			Singleton<Events>.Instance.onFinsihLoading.AddListener(new UnityAction(this.OnFinsihLoading));
			Singleton<Events>.Instance.onChangeFaction.AddListener(new UnityAction<FactionData>(this.OnChangeFaction));
			Singleton<NetworkPlayerManager>.Instance.OnPlayerLoaded(base.Owner);
			base.gameObject.AddComponent<InputController>();
			this._syncCooldown = GameServer.TickInterval;
			this._syncTimer = 0f;
			this._isStarted = true;
			this.hologram.Setup();
			this.playerInfo.SetActive(false);
		}
		else
		{
			this.playerInfo.SetActive(true);
		}
		if (NetworkSingleton<ClientSyncManager>.Instance != null)
		{
			this._syncID = base.OwnerId;
			NetworkSingleton<ClientSyncManager>.Instance.RegisterPlayer(this);
		}
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x000398E6 File Offset: 0x00037AE6
	public void OnChangeFaction(FactionData faction)
	{
		this._factionData = faction;
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x000398EF File Offset: 0x00037AEF
	public void OnFinsihLoading()
	{
		base.transform.position = Singleton<WorldGenerator>.Instance.CenterWorldPos;
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0003990B File Offset: 0x00037B0B
	public override void OnStopClient()
	{
		if (NetworkSingleton<ClientSyncManager>.Instance != null)
		{
			NetworkSingleton<ClientSyncManager>.Instance.UnregisterPlayer(this);
		}
		base.OnStopClient();
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x0003992C File Offset: 0x00037B2C
	public void Update()
	{
		if (base.IsOwner && this._isStarted && Singleton<Gamemode>.Instance.AllowClientMovement)
		{
			this.hologram.Tick();
			if (this._syncTimer <= 0f)
			{
				if (NetworkSingleton<ClientSyncManager>.Instance != null)
				{
					NetworkSingleton<ClientSyncManager>.Instance.Srv_SendClientState(base.OwnerId, this.GetHologramPosition);
				}
				this._syncTimer = this._syncCooldown;
				return;
			}
			this._syncTimer -= Time.deltaTime;
		}
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x000399AF File Offset: 0x00037BAF
	public virtual void NetworkInitialize___Early()
	{
		if (this.NetworkInitialize___EarlyPlayerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize___EarlyPlayerAssembly-CSharp.dll_Excuted = true;
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x000399C2 File Offset: 0x00037BC2
	public virtual void NetworkInitialize__Late()
	{
		if (this.NetworkInitialize__LatePlayerAssembly-CSharp.dll_Excuted)
		{
			return;
		}
		this.NetworkInitialize__LatePlayerAssembly-CSharp.dll_Excuted = true;
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x000399D5 File Offset: 0x00037BD5
	public override void NetworkInitializeIfDisabled()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x000399D5 File Offset: 0x00037BD5
	public virtual void Awake()
	{
		this.NetworkInitialize___Early();
		this.NetworkInitialize__Late();
	}

	// Token: 0x0400092E RID: 2350
	private int _syncID;

	// Token: 0x0400092F RID: 2351
	[SerializeField]
	private Hologram hologram;

	// Token: 0x04000930 RID: 2352
	[SerializeField]
	private FactionData _factionData;

	// Token: 0x04000931 RID: 2353
	public GameObject playerInfo;

	// Token: 0x04000932 RID: 2354
	public TextMeshPro title;

	// Token: 0x04000933 RID: 2355
	private bool _isStarted;

	// Token: 0x04000934 RID: 2356
	private float _syncTimer;

	// Token: 0x04000935 RID: 2357
	private float _syncCooldown;

	// Token: 0x04000936 RID: 2358
	private bool dll_Excuted;

	// Token: 0x04000937 RID: 2359
	private bool dll_Excuted;
}
