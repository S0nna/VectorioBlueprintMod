using System;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.ModernUIPack;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.Serialization;
using Vectorio.Stats;

// Token: 0x02000179 RID: 377
public class NewWorldPanel : MonoBehaviour
{
	// Token: 0x06000C76 RID: 3190 RVA: 0x00035964 File Offset: 0x00033B64
	public void Awake()
	{
		if (this.worldSeed != null)
		{
			this.worldSeed.inputText.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
		}
		bool flag = false;
		foreach (object obj in Enum.GetValues(typeof(StatType)))
		{
			StatType statType = (StatType)obj;
			if (!this._ignoredStats.Contains(statType))
			{
				StatModifierUI statModifierUI = Object.Instantiate<StatModifierUI>(this.modifierPrefab, this.modifierList);
				statModifierUI.Setup(statType, flag ? this.colorTwo : this.colorOne, new Action(this.OnModifierChanged));
				this._statModifiers.Add(statType, statModifierUI);
				flag = !flag;
			}
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00035A3C File Offset: 0x00033C3C
	public void Start()
	{
		foreach (PresetData presetData in (from p in Library.RequestAllDataOfType<PresetData>()
		orderby p.sortingOrder
		select p).ToList<PresetData>())
		{
			PresetButton presetButton = Object.Instantiate<PresetButton>(this.presetButtonPrefab, this.presetList);
			presetButton.Setup(presetData, this);
			this._presetButtons.Add(presetButton);
		}
		this.worldName.inputText.onValueChanged.AddListener(new UnityAction<string>(this.OnWorldNameUpdated));
		this.worldName.inputText.onValueChanged.AddListener(new UnityAction<string>(this.OnWorldSeedUpdated));
		this.version.text = "<b>Version:</b> " + Application.version;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x00035B34 File Offset: 0x00033D34
	public void OnWorldNameUpdated(string text)
	{
		this.world.text = "<b>World Name:</b> " + this.GetWorldName;
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x00035B54 File Offset: 0x00033D54
	public void OnWorldSeedUpdated(string text)
	{
		this.seed.text = "<b>Seed:</b> " + this.GetWorldSeed().ToString();
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00035B84 File Offset: 0x00033D84
	public void OnModifierChanged()
	{
		if (this._isLoading)
		{
			return;
		}
		this._presetData = null;
		this.preset.text = "<b>Preset:</b> Custom";
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x00035BA8 File Offset: 0x00033DA8
	public void OpenPanel()
	{
		this._isEditing = false;
		this._saveBeingEdited = null;
		this.title.text = "NEW SAVE";
		this.worldName.inputText.text = "New World";
		this.worldSeed.inputText.text = Random.Range(100000, 999999).ToString();
		this.world.text = "<b>World Name:</b> " + this.GetWorldName;
		this.seed.text = "<b>Seed:</b> " + this.GetWorldSeed().ToString();
		this.worldSeedObj.SetActive(true);
		this.noSeedChangeObj.SetActive(false);
		this._gamemodeData = SaveList.Gamemode;
		this.SetPreset(this.defaultModifiers);
		this.createText.text = "CREATE WORLD";
		this.multiplayer.text = "<b>Multiplayer Enabled:</b> " + (Menu.IsOnline() ? "Yes" : "No");
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x00035CB4 File Offset: 0x00033EB4
	private float CalculateValue(float value, StatType type)
	{
		StatInfo statInfo = StatLibrary.FetchInfo(type);
		if (statInfo == null)
		{
			return value;
		}
		if (!statInfo.lowerBetter)
		{
			return value;
		}
		return 1f / value;
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x00035CE0 File Offset: 0x00033EE0
	public void EditSave(SaveData saveData)
	{
		this._isEditing = true;
		this._saveBeingEdited = saveData;
		this.title.text = "EDITING SAVE";
		this.worldName.inputText.text = saveData.Name;
		this.worldSeed.inputText.text = saveData.Seed.ToString();
		this.worldSeedObj.SetActive(false);
		this.noSeedChangeObj.SetActive(true);
		this.SetPreset(this.defaultModifiers);
		this._isLoading = true;
		foreach (KeyValuePair<int, float> keyValuePair in saveData.GamemodeData.allyModifiers)
		{
			StatType key = (StatType)keyValuePair.Key;
			if (this._statModifiers.ContainsKey(key))
			{
				this._statModifiers[key].SetAllyValue(this.CalculateValue(keyValuePair.Value, key));
			}
		}
		foreach (KeyValuePair<int, float> keyValuePair2 in saveData.GamemodeData.enemyModifiers)
		{
			StatType key2 = (StatType)keyValuePair2.Key;
			if (this._statModifiers.ContainsKey(key2))
			{
				this._statModifiers[key2].SetEnemyValue(this.CalculateValue(keyValuePair2.Value, key2));
			}
		}
		this._gamemodeData = SaveList.Gamemode;
		this.multiplayer.text = "<b>Multiplayer Enabled:</b> " + (Menu.IsOnline() ? "Yes" : "No");
		this.createText.text = "SAVE WORLD";
		this._isLoading = false;
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00035EA8 File Offset: 0x000340A8
	public void SetPreset(PresetData presetData)
	{
		if (this._isEditing)
		{
			this.preset.text = "<b>Preset:</b> Custom";
		}
		else
		{
			this._presetData = presetData;
			this.preset.text = "<b>Preset:</b> " + presetData.Name;
		}
		this._isLoading = true;
		foreach (StatModifierUI statModifierUI in this._statModifiers.Values)
		{
			statModifierUI.SetAllyValue(1f);
			statModifierUI.SetEnemyValue(1f);
		}
		foreach (PresetData.Modifier modifier in presetData.allyModifiers)
		{
			if (this._statModifiers.ContainsKey(modifier.stat))
			{
				this._statModifiers[modifier.stat].SetAllyValue(modifier.value);
			}
		}
		foreach (PresetData.Modifier modifier2 in presetData.enemyModifiers)
		{
			if (this._statModifiers.ContainsKey(modifier2.stat))
			{
				this._statModifiers[modifier2.stat].SetAllyValue(modifier2.value);
			}
		}
		this._isLoading = false;
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000C7F RID: 3199 RVA: 0x0003602C File Offset: 0x0003422C
	public string GetWorldName
	{
		get
		{
			return this.worldName.inputText.text;
		}
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x00036040 File Offset: 0x00034240
	public int GetWorldSeed()
	{
		if (this.worldSeed != null)
		{
			int result;
			int.TryParse(this.worldSeed.inputText.text, out result);
			return result;
		}
		return Random.Range(100000, 999999);
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x00036084 File Offset: 0x00034284
	public void OnCreateWorld()
	{
		if (this._isEditing)
		{
			GamemodeSaveData gamemodeData = this._saveBeingEdited.GamemodeData;
			PresetData presetData = this._presetData;
			gamemodeData.PresetID = (((presetData != null) ? presetData.ID : null) ?? "");
			this._saveBeingEdited.GamemodeData.allyModifiers.Clear();
			this._saveBeingEdited.GamemodeData.enemyModifiers.Clear();
			foreach (KeyValuePair<StatType, StatModifierUI> keyValuePair in this._statModifiers)
			{
				int key = (int)keyValuePair.Key;
				Tuple<float?, float?> modifiers = keyValuePair.Value.GetModifiers();
				if (modifiers.Item1 != null)
				{
					this._saveBeingEdited.GamemodeData.AddAllyModifier(key, this.CalculateValue(modifiers.Item1.Value, keyValuePair.Key));
				}
				if (modifiers.Item2 != null)
				{
					this._saveBeingEdited.GamemodeData.AddEnemyModifier(key, this.CalculateValue(modifiers.Item2.Value, keyValuePair.Key));
				}
			}
			this._saveBeingEdited.Name = this.GetWorldName;
			this.menu.CloseNewWorldAndOpenSaves();
			return;
		}
		GamemodeSaveData gamemodeSaveData = new GamemodeSaveData(this._gamemodeData);
		PresetData presetData2 = this._presetData;
		gamemodeSaveData.PresetID = (((presetData2 != null) ? presetData2.ID : null) ?? "");
		GamemodeSaveData gamemodeSaveData2 = gamemodeSaveData;
		foreach (KeyValuePair<StatType, StatModifierUI> keyValuePair2 in this._statModifiers)
		{
			int key2 = (int)keyValuePair2.Key;
			Tuple<float?, float?> modifiers2 = keyValuePair2.Value.GetModifiers();
			if (modifiers2.Item1 != null)
			{
				gamemodeSaveData2.AddAllyModifier(key2, this.CalculateValue(modifiers2.Item1.Value, keyValuePair2.Key));
			}
			if (modifiers2.Item2 != null)
			{
				gamemodeSaveData2.AddEnemyModifier(key2, this.CalculateValue(modifiers2.Item2.Value, keyValuePair2.Key));
			}
		}
		SaveData saveData = new SaveData();
		saveData.ActiveRegion = this._gamemodeData.defaultRegion.ID;
		saveData.Seed = this.GetWorldSeed();
		saveData.FileName = FileOperations.GenerateSaveFileName(this._gamemodeData);
		saveData.GamemodeData = gamemodeSaveData2;
		saveData.Name = this.GetWorldName;
		saveData.ID = saveData.Name.Replace(' ', '_').ToLower();
		saveData.Version = Application.version;
		Singleton<Events>.Instance.onDisableActionMap.Invoke();
		Singleton<EntityManager>.Instance.ClearAllEntities();
		SaveSystem.SaveData = saveData;
		if (!Authenticator.UserAuthenticated)
		{
			Singleton<Lobby>.Instance.CreateOfflineLobby();
			return;
		}
		if (Menu.IsOnline())
		{
			Singleton<Lobby>.Instance.CreateSteamLobby(ELobbyType.k_ELobbyTypePublic, 20);
			return;
		}
		Singleton<Lobby>.Instance.CreateLocalLobby(2);
	}

	// Token: 0x04000848 RID: 2120
	public TextMeshProUGUI world;

	// Token: 0x04000849 RID: 2121
	public TextMeshProUGUI seed;

	// Token: 0x0400084A RID: 2122
	public TextMeshProUGUI preset;

	// Token: 0x0400084B RID: 2123
	public TextMeshProUGUI region;

	// Token: 0x0400084C RID: 2124
	public TextMeshProUGUI version;

	// Token: 0x0400084D RID: 2125
	public TextMeshProUGUI multiplayer;

	// Token: 0x0400084E RID: 2126
	public TextMeshProUGUI achievements;

	// Token: 0x0400084F RID: 2127
	public PresetData defaultModifiers;

	// Token: 0x04000850 RID: 2128
	public PresetButton presetButtonPrefab;

	// Token: 0x04000851 RID: 2129
	public StatModifierUI modifierPrefab;

	// Token: 0x04000852 RID: 2130
	public Transform presetList;

	// Token: 0x04000853 RID: 2131
	public Transform modifierList;

	// Token: 0x04000854 RID: 2132
	public TextMeshProUGUI title;

	// Token: 0x04000855 RID: 2133
	public TextMeshProUGUI createText;

	// Token: 0x04000856 RID: 2134
	public GameObject worldSeedObj;

	// Token: 0x04000857 RID: 2135
	public GameObject noSeedChangeObj;

	// Token: 0x04000858 RID: 2136
	public CustomInputField worldName;

	// Token: 0x04000859 RID: 2137
	public CustomInputField worldSeed;

	// Token: 0x0400085A RID: 2138
	public CustomInputField worldNote;

	// Token: 0x0400085B RID: 2139
	protected GamemodeData _gamemodeData;

	// Token: 0x0400085C RID: 2140
	protected PresetData _presetData;

	// Token: 0x0400085D RID: 2141
	public Color colorOne;

	// Token: 0x0400085E RID: 2142
	public Color colorTwo;

	// Token: 0x0400085F RID: 2143
	private Dictionary<StatType, StatModifierUI> _statModifiers = new Dictionary<StatType, StatModifierUI>();

	// Token: 0x04000860 RID: 2144
	private List<PresetButton> _presetButtons = new List<PresetButton>();

	// Token: 0x04000861 RID: 2145
	private List<StatType> _ignoredStats = new List<StatType>
	{
		StatType.Unassigned,
		StatType.ProjectileSize
	};

	// Token: 0x04000862 RID: 2146
	private bool _isEditing;

	// Token: 0x04000863 RID: 2147
	private bool _isLoading;

	// Token: 0x04000864 RID: 2148
	private SaveData _saveBeingEdited;

	// Token: 0x04000865 RID: 2149
	public Menu menu;

	// Token: 0x0200017A RID: 378
	[Serializable]
	public class ColorSetting
	{
		// Token: 0x04000866 RID: 2150
		public Image image;

		// Token: 0x04000867 RID: 2151
		public Color color;

		// Token: 0x04000868 RID: 2152
		public string iconID;
	}
}
