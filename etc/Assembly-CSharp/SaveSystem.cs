using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Grid;
using Vectorio.Serialization;
using Vectorio.Utilities;

// Token: 0x02000074 RID: 116
[DefaultExecutionOrder(1)]
public class SaveSystem : global::Singleton<SaveSystem>
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001BC6B File Offset: 0x00019E6B
	// (set) Token: 0x06000541 RID: 1345 RVA: 0x0001BC72 File Offset: 0x00019E72
	public static SaveData SaveData
	{
		get
		{
			return SaveSystem._saveData;
		}
		set
		{
			SaveSystem._saveData = value;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001BC7A File Offset: 0x00019E7A
	public static PlayerData PlayerData
	{
		get
		{
			return SaveSystem._playerData;
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0001BC84 File Offset: 0x00019E84
	public void WritePlayerDataFile(PlayerData playerData)
	{
		string fileName = "player" + FileOperations.SAVE_FILE_EXTENSION;
		FileOperations.WriteUserFile<PlayerData>(playerData, "Settings", fileName);
		SaveSystem._playerData = playerData;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0001BCB4 File Offset: 0x00019EB4
	public void LoadPlayerDataAsync()
	{
		string fileName = "player" + FileOperations.SAVE_FILE_EXTENSION;
		if (FileOperations.UserFileExists("Settings", fileName))
		{
			SaveSystem._playerData = FileOperations.GetUserFile<PlayerData>("Settings", fileName);
			return;
		}
		SaveSystem._playerData = new PlayerData();
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0001BCFC File Offset: 0x00019EFC
	public void WriteSettingsToFile(SettingsData settings)
	{
		settings.Version = Application.version;
		settings.ID = "user_settings";
		settings.Name = "User Settings";
		settings.Description = "A file containing the users preferred game settings.";
		string fileName = "settings" + FileOperations.SETTINGS_FILE_EXTENSION;
		FileOperations.WriteUserFile<SettingsData>(settings, "Settings", fileName);
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0001BD54 File Offset: 0x00019F54
	public SettingsData LoadSettingsAsync()
	{
		if (SaveSystem._checkForOldSettings)
		{
			string fileName = "settings" + FileOperations.SAVE_FILE_EXTENSION;
			if (FileOperations.UserFileExists("Settings", fileName))
			{
				FileOperations.DeleteUserFile("Settings", fileName);
			}
			else
			{
				Debug.Log("[SAVE] Old settings file already cleared.");
			}
		}
		string fileName2 = "settings" + FileOperations.SETTINGS_FILE_EXTENSION;
		if (FileOperations.UserFileExists("Settings", fileName2))
		{
			return FileOperations.GetUserFile<SettingsData>("Settings", fileName2);
		}
		return new SettingsData();
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0001BDCC File Offset: 0x00019FCC
	public void WriteSaveToFile(SaveData save)
	{
		GamemodeData gamemodeData = Library.RequestData<GamemodeData>(save.GamemodeData.ID);
		if (gamemodeData != null)
		{
			FileOperations.WriteSaveToFile(save, gamemodeData);
			return;
		}
		Debug.Log("[SAVE SYSTEM] Invalid gamemode ID, placing save in unmarked folder.");
		FileOperations.WriteSaveToFile(save, "Unknown");
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0001BE10 File Offset: 0x0001A010
	public SaveData GenerateWorldSave()
	{
		SaveSystem.IS_SAVING = true;
		SaveData saveData = SaveSystem._saveData;
		string id = global::Singleton<RegionManager>.Instance.Region.ID;
		if (!SaveSystem._saveData.HasRegion(id))
		{
			Debug.Log("[SAVE SYSTEM] The region ID " + id + " does not exist in the save data! This should not happen.");
			SaveSystem._saveData.CreateRegion(id);
		}
		SaveData.Region region = SaveSystem._saveData.GetRegion(id);
		Dictionary<string, List<EntityMetadata>> dictionary = new Dictionary<string, List<EntityMetadata>>();
		Dictionary<string, List<EntityMetadata>> dictionary2 = new Dictionary<string, List<EntityMetadata>>();
		Dictionary<string, List<TileData>> dictionary3 = new Dictionary<string, List<TileData>>();
		Dictionary<string, List<DecorationData>> dictionary4 = new Dictionary<string, List<DecorationData>>();
		foreach (KeyValuePair<uint, Entity> keyValuePair in global::Singleton<EntityManager>.Instance.Entities)
		{
			if (!keyValuePair.Value.Has_EFlag_IsDead)
			{
				if (keyValuePair.Value.Has_EFlag_IsWorldFeature)
				{
					if (!dictionary2.ContainsKey(keyValuePair.Value.ID))
					{
						dictionary2.Add(keyValuePair.Value.ID, new List<EntityMetadata>());
					}
					dictionary2[keyValuePair.Value.ID].Add(keyValuePair.Value.ExtractMetadata(false, MetadataContext.Global));
				}
				else if (keyValuePair.Value.IsSaveable)
				{
					if (!dictionary.ContainsKey(keyValuePair.Value.ID))
					{
						dictionary.Add(keyValuePair.Value.ID, new List<EntityMetadata>());
					}
					dictionary[keyValuePair.Value.ID].Add(keyValuePair.Value.ExtractMetadata(false, MetadataContext.Global));
				}
			}
		}
		foreach (Vector3Int v in global::Singleton<TileGrid>.Instance.GetResourceTilePositions)
		{
			Vector2Int coords = (Vector2Int)v;
			CellData cell = global::Singleton<TileGrid>.Instance.GetCell(coords, false);
			if (cell.HasResource)
			{
				ResourceData resource = cell.Resource;
				if (!dictionary3.ContainsKey(resource.ID))
				{
					dictionary3.Add(resource.ID, new List<TileData>());
				}
				dictionary3[resource.ID].Add(new TileData(coords.x, coords.y));
			}
		}
		foreach (Vector3Int v2 in global::Singleton<TileGrid>.Instance.GetDecoratedTilePositions)
		{
			Vector2Int coords2 = (Vector2Int)v2;
			CellData cell2 = global::Singleton<TileGrid>.Instance.GetCell(coords2, false);
			if (cell2.HasTileDesign)
			{
				TileDesign tileDesign = cell2.TileDesign;
				if (!(tileDesign.data == null))
				{
					if (!dictionary4.ContainsKey(tileDesign.data.ID))
					{
						dictionary4.Add(tileDesign.data.ID, new List<DecorationData>());
					}
					DecorationData item = new DecorationData(coords2.x, coords2.y, Utilities.ColorToInt(tileDesign.color), Utilities.ColorToInt(tileDesign.map));
					dictionary4[tileDesign.data.ID].Add(item);
				}
			}
		}
		string[,] array = new string[SaveSystem.PREVIEW_SIZE, SaveSystem.PREVIEW_SIZE];
		int num = global::Singleton<WorldGenerator>.Instance.CenterTilePosition.x - SaveSystem.PREVIEW_SIZE / 2;
		int num2 = global::Singleton<WorldGenerator>.Instance.CenterTilePosition.y - SaveSystem.PREVIEW_SIZE / 2;
		for (int i = 0; i < array.GetLength(0); i++)
		{
			for (int j = 0; j < array.GetLength(1); j++)
			{
				Vector3Int tilePos = new Vector3Int(i + num, j + num2, 0);
				Color cellColor = global::Singleton<MapView>.Instance.GetCellColor(tilePos);
				array[i, j] = "#" + cellColor.ToHexString();
			}
		}
		region.entities = dictionary;
		region.worldFeatures = dictionary2;
		region.resources = dictionary3;
		region.decorations = dictionary4;
		region.preview = array;
		saveData.Seed = global::Singleton<WorldGenerator>.Instance.seed;
		saveData.ActiveRegion = global::Singleton<RegionManager>.Instance.Region.ID;
		saveData.ActiveResearchTech = ((global::Singleton<Research>.Instance.ActiveTech != null) ? global::Singleton<Research>.Instance.ActiveTech.ID : "");
		saveData.researchTechResources = global::Singleton<Research>.Instance.GetTechResources();
		List<string> list = new List<string>();
		foreach (ResearchTechData researchTechData in global::Singleton<Research>.Instance.UnlockedTechs)
		{
			list.Add(researchTechData.ID);
		}
		saveData.completedResearchTechs = list;
		SaveData.Hotbar hotbar;
		if (saveData.hotbars.ContainsKey(0U))
		{
			hotbar = saveData.hotbars[0U];
		}
		else
		{
			hotbar = new SaveData.Hotbar();
			saveData.hotbars.Add(0U, hotbar);
		}
		hotbar.hotbars = global::Singleton<Hotbar>.Instance.RequestHotbar();
		saveData.GamemodeData = global::Singleton<Gamemode>.Instance.GetGamemodeSaveData();
		saveData.WorldTime = global::Singleton<Gamemode>.Instance.GameTime;
		SaveSystem.IS_SAVING = false;
		return saveData;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0001C358 File Offset: 0x0001A558
	public void LoadEntities(SaveData saveData)
	{
		Debug.Log("[SAVE SYSTEM] Loading entities!");
		string activeRegion = saveData.ActiveRegion;
		if (!saveData.HasRegion(activeRegion))
		{
			return;
		}
		SaveData.Region region = saveData.GetRegion(activeRegion);
		SaveSystem.IS_LOADING = true;
		try
		{
			if (NetworkPlayerManager.IS_HOST)
			{
				this.PreloadAndSetRuntimeIDTracker(region);
			}
			foreach (KeyValuePair<string, List<TileData>> keyValuePair in region.resources)
			{
				ResourceData resourceData = Library.RequestData<ResourceData>(keyValuePair.Key);
				if (!(resourceData == null))
				{
					foreach (TileData tileData in keyValuePair.Value)
					{
						global::Singleton<TileGrid>.Instance.SetResourceTile(new Vector3Int(tileData.X, tileData.Y, 0), resourceData);
					}
				}
			}
			foreach (KeyValuePair<string, List<DecorationData>> keyValuePair2 in region.decorations)
			{
				TileDesignData tileDesignData = Library.RequestData<TileDesignData>(keyValuePair2.Key);
				if (!(tileDesignData == null))
				{
					foreach (DecorationData decorationData in keyValuePair2.Value)
					{
						TileDesign design = new TileDesign(tileDesignData, Utilities.IntToColor(decorationData.TileColor), Utilities.IntToColor(decorationData.MapColor));
						global::Singleton<TileGrid>.Instance.SetTileDesign(design, new Vector3Int(decorationData.X, decorationData.Y, 0), false);
					}
				}
			}
			foreach (KeyValuePair<string, List<EntityMetadata>> keyValuePair3 in region.worldFeatures)
			{
				this.CreateEntities(keyValuePair3.Value);
			}
			foreach (KeyValuePair<string, List<EntityMetadata>> keyValuePair4 in region.entities)
			{
				this.CreateEntities(keyValuePair4.Value);
			}
			foreach (KeyValuePair<string, List<EntityMetadata>> keyValuePair5 in region.worldFeatures)
			{
				this.ApplyMetadata(keyValuePair5.Value);
			}
			foreach (KeyValuePair<string, List<EntityMetadata>> keyValuePair6 in region.entities)
			{
				this.ApplyMetadata(keyValuePair6.Value);
			}
			global::Singleton<ResourceManager>.Instance.ForceUpdatePowerStatus();
			Debug.Log("[SAVE SYSTEM] Entities and metadata applied successfully.");
		}
		finally
		{
			SaveSystem.IS_LOADING = false;
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0001C6EC File Offset: 0x0001A8EC
	public void PreloadAndSetRuntimeIDTracker(SaveData.Region region)
	{
		uint num = ServerSingleton<ServerSyncManager>.Instance.PeekRuntimeIDTracker();
		foreach (KeyValuePair<string, List<EntityMetadata>> keyValuePair in region.worldFeatures)
		{
			foreach (EntityMetadata entityMetadata in keyValuePair.Value)
			{
				if (entityMetadata.RuntimeID.GetValue() > num)
				{
					num = entityMetadata.RuntimeID.GetValue();
				}
			}
		}
		foreach (KeyValuePair<string, List<EntityMetadata>> keyValuePair2 in region.entities)
		{
			foreach (EntityMetadata entityMetadata2 in keyValuePair2.Value)
			{
				if (entityMetadata2.RuntimeID.GetValue() > num)
				{
					num = entityMetadata2.RuntimeID.GetValue();
				}
			}
		}
		ServerSingleton<ServerSyncManager>.Instance.SetRuntimeID(num + 1U);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0001C840 File Offset: 0x0001AA40
	private void CreateEntities(IEnumerable<EntityMetadata> metadataList)
	{
		using (IEnumerator<EntityMetadata> enumerator = metadataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EntityCreationData entityCreationData;
				if (EventBuilder.BuildCreationDataWithMetadata(enumerator.Current, out entityCreationData))
				{
					entityCreationData.FromSave = true;
					EventBuilder.ApplyCostsToCreationData(ref entityCreationData, false);
					global::Singleton<EntityManager>.Instance.QueueCreationEvent(entityCreationData);
				}
			}
		}
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0001C8A4 File Offset: 0x0001AAA4
	private void ApplyMetadata(IEnumerable<EntityMetadata> metadataList)
	{
		foreach (EntityMetadata entityMetadata in metadataList)
		{
			EntityMetadataEvent data = EventBuilder.BuildMetadataEvent(entityMetadata.RuntimeID.GetValue(), entityMetadata, false);
			global::Singleton<EntityManager>.Instance.QueueMetadataEvent(data, SyncType.None);
		}
	}

	// Token: 0x04000301 RID: 769
	public static bool IS_EXPERIMENTAL = false;

	// Token: 0x04000302 RID: 770
	public static bool IS_LOADING = false;

	// Token: 0x04000303 RID: 771
	public static bool IS_SAVING = false;

	// Token: 0x04000304 RID: 772
	private const string PLAYER_PATH = "Settings";

	// Token: 0x04000305 RID: 773
	private const string PLAYER_FILE = "player";

	// Token: 0x04000306 RID: 774
	private const string SETTINGS_PATH = "Settings";

	// Token: 0x04000307 RID: 775
	private const string SETTINGS_FILE = "settings";

	// Token: 0x04000308 RID: 776
	public static int PREVIEW_SIZE = 100;

	// Token: 0x04000309 RID: 777
	private static SaveData _saveData;

	// Token: 0x0400030A RID: 778
	private static PlayerData _playerData;

	// Token: 0x0400030B RID: 779
	private static bool _checkForOldSettings = true;
}
