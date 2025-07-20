using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vectorio.Serialization;

// Token: 0x02000069 RID: 105
[DefaultExecutionOrder(-1)]
public class Library : MonoBehaviour
{
	// Token: 0x060004F9 RID: 1273 RVA: 0x0001A351 File Offset: 0x00018551
	public static Material GetDefaultMaterial()
	{
		return Library.defaultMaterial;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0001A358 File Offset: 0x00018558
	public static Material GetMainMaterial()
	{
		return Library.mainMaterial;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001A360 File Offset: 0x00018560
	public void ProcessDataList()
	{
		Library.defaultMaterial = this._defaultMaterial;
		Library.mainMaterial = this._mainMaterial;
		Library._library = new Dictionary<string, BaseData>();
		for (int i = 0; i < this.dataList.Count; i++)
		{
			if (!Library._library.ContainsKey(this.dataList[i].ID))
			{
				Library._library.Add(this.dataList[i].ID, this.dataList[i]);
			}
			else
			{
				Debug.Log(string.Concat(new string[]
				{
					"[LIBRARY] Could not add ",
					this.dataList[i].FormattedName,
					" because the ID ",
					this.dataList[i].ID,
					" is already registered in the library!\n\nID Owner: ",
					Library.RequestData<BaseData>(this.dataList[i].ID).Name
				}));
			}
		}
		MetadataTypeCache.Initialize();
		if (DevTools.QUICK_START)
		{
			SaveData saveData = new SaveData();
			GamemodeData gamemodeData = Library.RequestData<GamemodeData>(DevTools.GAMEMODE_ID);
			RegionData regionData = Library.RequestData<RegionData>(DevTools.REGION_ID);
			saveData.Seed = Random.Range(100000, 999999);
			saveData.ActiveRegion = regionData.ID;
			saveData.FileName = FileOperations.GenerateSaveFileName(gamemodeData);
			saveData.Name = "Quick Start World";
			saveData.ID = saveData.Name.Replace(' ', '_').ToLower();
			saveData.Version = Application.version;
			saveData.GamemodeData = new GamemodeSaveData(gamemodeData);
			Singleton<Events>.Instance.onDisableActionMap.Invoke();
			SaveSystem.SaveData = saveData;
			Singleton<Lobby>.Instance.CreateLocalLobby(2);
			return;
		}
		SceneManager.LoadScene("Menu");
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0001A51C File Offset: 0x0001871C
	public static T RequestData<T>(string id) where T : BaseData
	{
		BaseData baseData = Library._cache.Get(id);
		if (baseData != null)
		{
			return (T)((object)baseData);
		}
		if (Library._library.ContainsKey(id))
		{
			baseData = Library._library[id];
			Library._cache.Put(id, baseData);
			return (T)((object)baseData);
		}
		Debug.Log("[LIBRARY] No data object with ID " + id + " exists!");
		return default(T);
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0001A590 File Offset: 0x00018790
	public static List<T> RequestAllDataOfType<T>() where T : BaseData
	{
		List<T> list = new List<T>();
		foreach (KeyValuePair<string, BaseData> keyValuePair in Library._library)
		{
			T t = keyValuePair.Value as T;
			if (t != null)
			{
				list.Add(t);
			}
		}
		return list;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00003212 File Offset: 0x00001412
	public void UpdateDataList()
	{
	}

	// Token: 0x040002DE RID: 734
	public const int CACHE_CAPACITY = 100;

	// Token: 0x040002DF RID: 735
	public static string DATA_FILES = "Assets/Vectorio/Data Files/";

	// Token: 0x040002E0 RID: 736
	public List<BaseData> dataList;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private Material _defaultMaterial;

	// Token: 0x040002E2 RID: 738
	private static Material defaultMaterial;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	private Material _mainMaterial;

	// Token: 0x040002E4 RID: 740
	private static Material mainMaterial;

	// Token: 0x040002E5 RID: 741
	private static Dictionary<string, BaseData> _library = new Dictionary<string, BaseData>();

	// Token: 0x040002E6 RID: 742
	private static Library.Cache<string, BaseData> _cache = new Library.Cache<string, BaseData>();

	// Token: 0x0200006A RID: 106
	public class Cache<K, V>
	{
		// Token: 0x06000501 RID: 1281 RVA: 0x0001A624 File Offset: 0x00018824
		public Cache()
		{
			this._capacity = 100;
			this._map = new Dictionary<K, V>(100);
			this._lruList = new LinkedList<K>();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001A64C File Offset: 0x0001884C
		public V Get(K key)
		{
			if (!this._map.ContainsKey(key))
			{
				return default(V);
			}
			V result = this._map[key];
			this._lruList.Remove(key);
			this._lruList.AddLast(key);
			return result;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001A698 File Offset: 0x00018898
		public void Put(K key, V value)
		{
			if (this._map.ContainsKey(key))
			{
				this._map[key] = value;
				this._lruList.Remove(key);
			}
			else
			{
				if (this._map.Count >= this._capacity)
				{
					K value2 = this._lruList.First.Value;
					this._map.Remove(value2);
					this._lruList.RemoveFirst();
				}
				this._map.Add(key, value);
			}
			this._lruList.AddLast(key);
		}

		// Token: 0x040002E7 RID: 743
		private readonly int _capacity;

		// Token: 0x040002E8 RID: 744
		private Dictionary<K, V> _map;

		// Token: 0x040002E9 RID: 745
		private LinkedList<K> _lruList;
	}
}
