using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.Entities;
using Vectorio.PhasmaUI;

// Token: 0x020001E4 RID: 484
[DefaultExecutionOrder(9)]
public class RegionManager : Singleton<RegionManager>
{
	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00045020 File Offset: 0x00043220
	// (set) Token: 0x06000EEC RID: 3820 RVA: 0x00045028 File Offset: 0x00043228
	public RegionData Region
	{
		get
		{
			return this._region;
		}
		set
		{
			this._region = value;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000EED RID: 3821 RVA: 0x00045031 File Offset: 0x00043231
	public bool IsSequencePlaying
	{
		get
		{
			return this._isSequencePlaying;
		}
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0004503C File Offset: 0x0004323C
	private void ToggleSequence(bool toggle)
	{
		if (toggle)
		{
			this.OpenWarp();
			this._isSequencePlaying = true;
			this._sequenceStep = 0;
			this._timeToNextSequence = 1f;
			this._rotationSpeed = 0f;
			this.background.alpha = 1f;
			this.blackBarOne.sizeDelta = new Vector2(this.blackBarOne.sizeDelta.x, 0f);
			this.blackBarTwo.sizeDelta = new Vector2(this.blackBarTwo.sizeDelta.x, 0f);
			this.countdown.text = "";
			this.camera.orthographicSize = this.startCameraSize;
			Singleton<Events>.Instance.onJumpSequenceStarted.Invoke(this._gateway);
			Singleton<MusicPlayer>.Instance.Pause();
			Singleton<Gamemode>.Instance.ForceDisableMovement = true;
			return;
		}
		if (this._portal != null)
		{
			Object.Destroy(this._portal);
		}
		this._isSequencePlaying = false;
		this._sequenceStep = 0;
		this._timeToNextSequence = 0f;
		this._rotationSpeed = 0f;
		Singleton<Events>.Instance.onJumpSequenceFinished.Invoke();
		Singleton<MusicPlayer>.Instance.Play();
		Singleton<Gamemode>.Instance.ForceDisableMovement = false;
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0002A221 File Offset: 0x00028421
	private float EaseInOutCubic(float t)
	{
		if ((double)t >= 0.5)
		{
			return 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
		}
		return 4f * t * t * t;
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x00045180 File Offset: 0x00043380
	private void Update()
	{
		if (!this.IsSequencePlaying)
		{
			return;
		}
		this._elapsedTime += Time.deltaTime;
		bool flag = this._elapsedTime >= this._timeToNextSequence;
		if (this._sequenceStep > 1)
		{
			this._rotationSpeed += Time.deltaTime * this.rotationSpeedMultiplier;
			this._gateway.Animate(Time.deltaTime, this._rotationSpeed);
		}
		switch (this._sequenceStep)
		{
		case 0:
			if (flag)
			{
				this.NextStep(1, 3f);
				return;
			}
			break;
		case 1:
		{
			float num = Mathf.Clamp01(this._elapsedTime / this._timeToNextSequence);
			this.background.alpha = 1f - num;
			this._blackBarSize = num * this.targetBarSize;
			this.blackBarOne.sizeDelta = new Vector2(this.blackBarOne.sizeDelta.x, this._blackBarSize);
			this.blackBarTwo.sizeDelta = new Vector2(this.blackBarTwo.sizeDelta.x, this._blackBarSize);
			this.camera.orthographicSize = Mathf.Lerp(this.startCameraSize, this.jumpCameraSize, this.EaseInOutCubic(num));
			if (flag)
			{
				this._portal = Object.Instantiate<GameObject>(this.portal, this._gateway.transform.position, Quaternion.identity);
				this._portal.transform.localScale = new Vector2(0f, 0f);
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.portalSound);
				this.NextStep(2, 4f);
				return;
			}
			break;
		}
		case 2:
		{
			float num = Mathf.Clamp01(this._elapsedTime / this._timeToNextSequence);
			float num2 = Mathf.Lerp(0f, 1f, this.EaseInOutCubic(num));
			this._portal.transform.localScale = new Vector2(num2, num2);
			if (flag)
			{
				this.countdown.text = "3";
				this.NextStep(3, 1.5f);
				return;
			}
			break;
		}
		case 3:
			this.countdown.alpha = 1f - Mathf.Clamp01(this._elapsedTime / 0.8f);
			if (flag)
			{
				this.countdown.text = "2";
				this.NextStep(4, 1.5f);
				return;
			}
			break;
		case 4:
			this.countdown.alpha = 1f - Mathf.Clamp01(this._elapsedTime / 0.8f);
			if (flag)
			{
				this.countdown.text = "1";
				this.NextStep(5, 1.5f);
				return;
			}
			break;
		case 5:
			this.countdown.alpha = 1f - Mathf.Clamp01(this._elapsedTime / 0.8f);
			if (flag)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.jumpSound);
				this.NextStep(6, 0.5f);
				return;
			}
			break;
		case 6:
		{
			float num = Mathf.Clamp01(this._elapsedTime / this._timeToNextSequence);
			this.background.alpha = num;
			this.camera.orthographicSize = Mathf.Lerp(this.jumpCameraSize, this.finishCameraSize, this.EaseInOutCubic(num));
			if (flag)
			{
				this.InitiateJump();
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x000454BE File Offset: 0x000436BE
	private void NextStep(int step, float time)
	{
		this._sequenceStep = step;
		this._timeToNextSequence = time;
		this._elapsedTime = 0f;
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x000454DC File Offset: 0x000436DC
	private void OnGatewayClicked(Gateway gateway)
	{
		this._gateway = gateway;
		RegionData region = gateway.GetData().region;
		if (region == null)
		{
			Debug.Log("[REGION MANAGER] Cannot switch to region, because the region does not exist!");
			return;
		}
		this.title.text = "BEGIN WARP TO " + region.Name.ToUpper() + "?";
		Color minimapBorderColor = region.minimapBorderColor;
		this.warningBarOne.color = minimapBorderColor;
		this.warningBarTwo.color = minimapBorderColor;
		this.warningBackground.color = new Color(minimapBorderColor.r * 0.3f, minimapBorderColor.g * 0.3f, minimapBorderColor.b * 0.3f, 0.9f);
		this.title.color = minimapBorderColor;
		this.ToggleWarpUI(true);
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x000455A0 File Offset: 0x000437A0
	public void WarpToRegion()
	{
		if (this._gateway == null)
		{
			Debug.Log("[REGION MANAGER] Cannot begin warp to region because no gateway has been selected!");
			this.ToggleWarpUI(false);
			return;
		}
		if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
		{
			NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueRegionChange(new RegionChangeEvent
			{
				GatewayID = this._gateway.RuntimeID
			});
			return;
		}
		this.WarpToRegion(this._gateway);
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x00045604 File Offset: 0x00043804
	public void WarpToRegion(Gateway gateway)
	{
		if (this.IsSequencePlaying)
		{
			Debug.Log("[REGION MANAGER] Cannot begin warp to region because a jump sequence is already underway!");
			this.ToggleWarpUI(false);
			return;
		}
		if (gateway == null)
		{
			Debug.Log("[REGION MANAGER] Cannot begin warp to region because no gateway has been selected!");
			this.ToggleWarpUI(false);
			return;
		}
		this._gateway = gateway;
		if (this._gateway.GetData() == null)
		{
			Debug.Log("[REGION MANAGER] Cannot begin warp to region because the selected gateway does not have data assigned to it!");
			this.ToggleWarpUI(false);
			return;
		}
		if (this._gateway.GetData().region == null)
		{
			Debug.Log("[REGION MANAGER] Cannot begin warp to region because the selected gateways region is not specified!");
			this.ToggleWarpUI(false);
			return;
		}
		this._regionToJumpTo = Library.RequestData<RegionData>(this._gateway.GetData().region.ID);
		if (this._regionToJumpTo == null)
		{
			Debug.Log("[REGION MANAGER] Cannot begin warp to region because the region does not exist!");
			this.ToggleWarpUI(false);
			return;
		}
		SaveSystem.SaveData = Singleton<SaveSystem>.Instance.GenerateWorldSave();
		if (this.useJumpSequence)
		{
			this.ToggleSequence(true);
			return;
		}
		this.InitiateJump();
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x000456FC File Offset: 0x000438FC
	public void InitiateJump()
	{
		if (this.IsSequencePlaying)
		{
			this.ToggleSequence(false);
		}
		try
		{
			Debug.Log("[REGION MANAGER] Clearing all entities from region...");
			this._isSwitching = true;
			Singleton<EntityManager>.Instance.ClearAllEntities();
			Singleton<TileGrid>.Instance.ClearTileMap();
			Singleton<ResourceManager>.Instance.ResetFreeEntities();
			Debug.Log("[REGION MANAGER] Entities cleared, beginning warp...");
			this.SetupRegion(this._regionToJumpTo, true);
		}
		catch (Exception ex)
		{
			Debug.Log("[REGION MANAGER] Warping failed...\nMessage: " + ex.Message + "\nStack: " + ex.StackTrace);
			this._isSwitching = false;
			this.ToggleWarpUI(false);
			if (this.exitOnFailedSwitch)
			{
				Singleton<Events>.Instance.onDisableActionMap.Invoke();
				SaveSystem.SaveData = null;
				Singleton<Lobby>.Instance.LeaveLobby();
			}
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x000457CC File Offset: 0x000439CC
	public void SetupRegion(RegionData region, bool saveAfterLoad)
	{
		SaveSystem.SaveData.ActiveRegion = region.ID;
		this._region = region;
		RegionManager.FIRST_LOAD = (SaveSystem.SaveData.GetRegion(this._region.ID) == null);
		if (RegionManager.FIRST_LOAD)
		{
			Debug.Log("[REGION MANAGER] First time setup for region with ID " + this._region.ID);
			SaveSystem.SaveData.CreateRegion(this._region.ID);
		}
		Singleton<Gamemode>.Instance.IsGamePaused = true;
		Singleton<Gamemode>.Instance.ForceDisableSounds = true;
		Singleton<FactionManager>.Instance.SetPlayerFaction(this._region.playerFaction.ID);
		Singleton<TileGrid>.Instance.Setup(this._region);
		Singleton<Research>.Instance.EnsureRegion(this._region);
		Singleton<ResourceManager>.Instance.Setup(this._region);
		Singleton<MinimapUI>.Instance.Setup(this._region);
		Singleton<HeatManager>.Instance.Setup(this._region);
		Singleton<WorldGenerator>.Instance.GenerateWorld(this._region.ID);
		if (RegionManager.FIRST_LOAD && this._region.hub != null && Singleton<Gamemode>.Instance.UseHub && NetworkPlayerManager.IS_HOST)
		{
			EntityCreationData creationData = EventBuilder.BuildCreationData(this._region.hub.ID, Singleton<FactionManager>.Instance.PlayerFactionID, Singleton<WorldGenerator>.Instance.CenterWorldPos, SyncType.ServerInitiated);
			EventBuilder.ApplyCosmeticToCreationData(ref creationData, this._region.hub.model.id);
			Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
		}
		Singleton<SaveSystem>.Instance.LoadEntities(SaveSystem.SaveData);
		if (Singleton<Gamemode>.Instance.UseHub && !RegionManager.FIRST_LOAD && Singleton<FactionManager>.Instance.GetHubs(Singleton<FactionManager>.Instance.PlayerFactionID).Count == 0)
		{
			Debug.Log("[REGION MANAGER] Detected missing hub on world, placing new one...");
			if (this._region.hub != null && NetworkPlayerManager.IS_HOST)
			{
				EntityCreationData creationData2 = EventBuilder.BuildCreationData(this._region.hub.ID, Singleton<FactionManager>.Instance.PlayerFactionID, Singleton<WorldGenerator>.Instance.CenterWorldPos, SyncType.ServerInitiated);
				EventBuilder.ApplyCosmeticToCreationData(ref creationData2, this._region.hub.model.id);
				Singleton<EntityManager>.Instance.QueueCreationEvent(creationData2);
			}
			else
			{
				Debug.Log("[REGION MANAGER] Could not create hub, because either the regions hub is null, or the client is not the host!");
			}
		}
		Singleton<ResourceManager>.Instance.SortContainers();
		Singleton<DroneManager>.Instance.SortBlueprints();
		UI_Singleton<UI_ResearchWindow>.Instance.UpdateTechButtons();
		UI_Singleton<UI_ResearchWindow>.Instance.UpdateTreeButtons();
		if (saveAfterLoad)
		{
			SaveData save = Singleton<SaveSystem>.Instance.GenerateWorldSave();
			Singleton<SaveSystem>.Instance.WriteSaveToFile(save);
		}
		if (this._isSwitching)
		{
			this.ToggleWarpUI(false);
			this._isSwitching = false;
		}
		Singleton<Gamemode>.Instance.IsGamePaused = false;
		Singleton<Gamemode>.Instance.ForceDisableSounds = false;
		this.goalText.text = "<b>GOAL:</b> <color=white>" + region.goalText;
		if (!this._listening)
		{
			Singleton<Events>.Instance.onGatewayClicked.AddListener(new UnityAction<Gateway>(this.OnGatewayClicked));
			this._listening = true;
		}
		Singleton<MusicPlayer>.Instance.ForcePlayTrack(region.regionTrack);
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00045AE4 File Offset: 0x00043CE4
	public void ToggleWarpUI(bool toggle)
	{
		if (toggle)
		{
			this.mainInterfaceObject.alpha = 0f;
			this.mainInterfaceObject.interactable = false;
			this.mainInterfaceObject.blocksRaycasts = false;
			this.warpInterfaceObject.SetActive(true);
			this.OpenConfirmation();
			return;
		}
		this.warpInterfaceObject.SetActive(false);
		this.mainInterfaceObject.alpha = 1f;
		this.mainInterfaceObject.interactable = true;
		this.mainInterfaceObject.blocksRaycasts = true;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x00045B64 File Offset: 0x00043D64
	public void OpenConfirmation()
	{
		this.confirmationGroup.alpha = 1f;
		this.confirmationGroup.interactable = true;
		this.confirmationGroup.blocksRaycasts = true;
		this.warningGroup.alpha = 0f;
		this.warningGroup.interactable = false;
		this.warningGroup.blocksRaycasts = false;
		this.warpGroup.alpha = 0f;
		this.warpGroup.interactable = false;
		this.warpGroup.blocksRaycasts = false;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x00045BEC File Offset: 0x00043DEC
	public void OpenWarning()
	{
		this.confirmationGroup.alpha = 0f;
		this.confirmationGroup.interactable = false;
		this.confirmationGroup.blocksRaycasts = false;
		this.warningGroup.alpha = 1f;
		this.warningGroup.interactable = true;
		this.warningGroup.blocksRaycasts = true;
		this.warpGroup.alpha = 0f;
		this.warpGroup.interactable = false;
		this.warpGroup.blocksRaycasts = false;
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x00045C74 File Offset: 0x00043E74
	public void OpenWarp()
	{
		this.confirmationGroup.alpha = 0f;
		this.confirmationGroup.interactable = false;
		this.confirmationGroup.blocksRaycasts = false;
		this.warningGroup.alpha = 0f;
		this.warningGroup.interactable = false;
		this.warningGroup.blocksRaycasts = false;
		this.warpGroup.alpha = 1f;
		this.warpGroup.interactable = true;
		this.warpGroup.blocksRaycasts = true;
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x00045CF9 File Offset: 0x00043EF9
	public void Confirm()
	{
		if (this._gateway.GetData().region.isBeta)
		{
			this.OpenWarning();
			return;
		}
		this.WarpToRegion();
	}

	// Token: 0x04000C19 RID: 3097
	public static bool FIRST_LOAD;

	// Token: 0x04000C1A RID: 3098
	private RegionData _region;

	// Token: 0x04000C1B RID: 3099
	private Gateway _gateway;

	// Token: 0x04000C1C RID: 3100
	private bool _isSwitching;

	// Token: 0x04000C1D RID: 3101
	public TextMeshProUGUI goalText;

	// Token: 0x04000C1E RID: 3102
	public bool useJumpSequence = true;

	// Token: 0x04000C1F RID: 3103
	public bool exitOnFailedSwitch = true;

	// Token: 0x04000C20 RID: 3104
	public CanvasGroup mainInterfaceObject;

	// Token: 0x04000C21 RID: 3105
	public GameObject warpInterfaceObject;

	// Token: 0x04000C22 RID: 3106
	public CanvasGroup confirmationGroup;

	// Token: 0x04000C23 RID: 3107
	public Image warningBarOne;

	// Token: 0x04000C24 RID: 3108
	public Image warningBarTwo;

	// Token: 0x04000C25 RID: 3109
	public Image warningBackground;

	// Token: 0x04000C26 RID: 3110
	public TextMeshProUGUI title;

	// Token: 0x04000C27 RID: 3111
	public TextMeshProUGUI description;

	// Token: 0x04000C28 RID: 3112
	public CanvasGroup warpGroup;

	// Token: 0x04000C29 RID: 3113
	public CanvasGroup warningGroup;

	// Token: 0x04000C2A RID: 3114
	public CanvasGroup background;

	// Token: 0x04000C2B RID: 3115
	public RectTransform blackBarOne;

	// Token: 0x04000C2C RID: 3116
	public RectTransform blackBarTwo;

	// Token: 0x04000C2D RID: 3117
	public TextMeshProUGUI countdown;

	// Token: 0x04000C2E RID: 3118
	public Camera camera;

	// Token: 0x04000C2F RID: 3119
	public float targetBarSize;

	// Token: 0x04000C30 RID: 3120
	public float startCameraSize;

	// Token: 0x04000C31 RID: 3121
	public float jumpCameraSize;

	// Token: 0x04000C32 RID: 3122
	public float finishCameraSize;

	// Token: 0x04000C33 RID: 3123
	public float rotationSpeedMultiplier;

	// Token: 0x04000C34 RID: 3124
	public AudioClip portalSound;

	// Token: 0x04000C35 RID: 3125
	public AudioClip jumpSound;

	// Token: 0x04000C36 RID: 3126
	public GameObject portal;

	// Token: 0x04000C37 RID: 3127
	private bool _listening;

	// Token: 0x04000C38 RID: 3128
	[SerializeField]
	private RegionData _regionToJumpTo;

	// Token: 0x04000C39 RID: 3129
	private bool _isSequencePlaying;

	// Token: 0x04000C3A RID: 3130
	private int _sequenceStep;

	// Token: 0x04000C3B RID: 3131
	private float _timeToNextSequence;

	// Token: 0x04000C3C RID: 3132
	private float _elapsedTime;

	// Token: 0x04000C3D RID: 3133
	private float _blackBarSize;

	// Token: 0x04000C3E RID: 3134
	private float _rotationSpeed;

	// Token: 0x04000C3F RID: 3135
	private GameObject _portal;
}
