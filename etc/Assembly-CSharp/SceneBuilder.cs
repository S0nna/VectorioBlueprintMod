using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001F1 RID: 497
[DefaultExecutionOrder(10)]
public class SceneBuilder : MonoBehaviour
{
	// Token: 0x06000F41 RID: 3905 RVA: 0x000478E1 File Offset: 0x00045AE1
	public void Awake()
	{
		if (!this.autoStart)
		{
			Singleton<NetworkEvents>.Instance.onLocalClientReadyToLoad.AddListener(new UnityAction<bool>(this.Build));
		}
		NetworkPlayerManager.LOCAL_CLIENT_READY = false;
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x0004790C File Offset: 0x00045B0C
	public void Start()
	{
		if (this.autoStart)
		{
			this.Build(false);
		}
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x00047920 File Offset: 0x00045B20
	private void Build(bool broadcastFinish)
	{
		if (this._isSetup)
		{
			Debug.Log("[SCENE BUILDING] Duplicate setup detected, aborting...");
			return;
		}
		Debug.Log("[SCENE BUILDING] Starting scene build...");
		try
		{
			Singleton<Settings>.Instance.Setup();
			Singleton<EntityManager>.Instance.Setup();
			Singleton<SaveSystem>.Instance.LoadPlayerDataAsync();
			if (Singleton<Gamemode>.Instance.IsOfflineScene)
			{
				Singleton<FactionManager>.Instance.SetPlayerFaction("faction_player");
				if (broadcastFinish)
				{
					NetworkSingleton<ClientSyncManager>.Instance.Srv_SyncPlayerLoading(false);
				}
				NetworkPlayerManager.LOCAL_CLIENT_READY = true;
				this._isSetup = true;
			}
			else
			{
				RegionData regionData = Library.RequestData<RegionData>(SaveSystem.SaveData.ActiveRegion);
				if (regionData == null)
				{
					Debug.Log("[SCENE BUILDER] Invalid region with ID " + SaveSystem.SaveData.ActiveRegion);
					regionData = Library.RequestData<RegionData>("region_the_abyss");
				}
				Singleton<Gamemode>.Instance.SetGamemodeSaveData(SaveSystem.SaveData.GamemodeData);
				Singleton<Gamemode>.Instance.GameTime = SaveSystem.SaveData.WorldTime;
				Singleton<Research>.Instance.Setup(SaveSystem.SaveData);
				Singleton<RegionManager>.Instance.SetupRegion(regionData, false);
				Singleton<TutorialManager>.Instance.Setup();
				Singleton<Hotbar>.Instance.Setup();
				if (!RegionManager.FIRST_LOAD)
				{
					if (SaveSystem.SaveData.hotbars != null && SaveSystem.SaveData.hotbars.ContainsKey(0U))
					{
						Singleton<Hotbar>.Instance.SetHotbar(SaveSystem.SaveData.hotbars[0U]);
					}
				}
				else
				{
					Singleton<Hotbar>.Instance.SetupDefaultEntities();
				}
				Singleton<Selector>.Instance.Setup();
			}
		}
		catch (Exception ex)
		{
			Debug.Log("[SCENE BUILDER] Ran into an issue while building...\n\nError Message: " + ex.Message + "\n\nStack Trace: " + ex.StackTrace);
		}
		finally
		{
			Singleton<Events>.Instance.onFinsihLoading.Invoke();
			if (broadcastFinish)
			{
				NetworkSingleton<ClientSyncManager>.Instance.Srv_SyncPlayerLoading(false);
			}
			NetworkPlayerManager.LOCAL_CLIENT_READY = true;
			this._isSetup = true;
		}
	}

	// Token: 0x04000C92 RID: 3218
	public bool autoStart;

	// Token: 0x04000C93 RID: 3219
	private bool _isSetup;
}
