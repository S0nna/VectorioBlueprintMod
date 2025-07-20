using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Vectorio.Settings.Data;

// Token: 0x0200019B RID: 411
[DefaultExecutionOrder(11)]
public class SettingsUI : MonoBehaviour
{
	// Token: 0x06000DA5 RID: 3493 RVA: 0x0003C2F4 File Offset: 0x0003A4F4
	public void Setup(List<BaseSettingData> settings)
	{
		if (this._isSetup)
		{
			Debug.Log("[SETTINGS UI] The menu is already setup!");
			return;
		}
		if (settings != null && settings.Count > 0)
		{
			using (List<BaseSettingData>.Enumerator enumerator = (from p in settings
			orderby p.sortingOrder
			select p).ToList<BaseSettingData>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BaseSettingData baseSettingData = enumerator.Current;
					if (!this._settingsDictionary.ContainsKey(baseSettingData.category))
					{
						this._settingsDictionary.Add(baseSettingData.category, new List<BaseSettingData>());
					}
					this._settingsDictionary[baseSettingData.category].Add(baseSettingData);
				}
				goto IL_C2;
			}
		}
		Debug.Log("[SETTINGS UI] Could not load any settings! Is this a bug?");
		IL_C2:
		this.SetCategory(0);
		this._isSetup = true;
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x0003C3E4 File Offset: 0x0003A5E4
	public void SetCategory(int categoryIndex)
	{
		foreach (GameObject gameObject in this._activeSettings)
		{
			Object.Destroy(gameObject.gameObject);
		}
		this._alternate = false;
		SettingCategory settingCategory = (SettingCategory)categoryIndex;
		this.settingsTitle.text = "SETTINGS | <size=8>" + settingCategory.ToString();
		if (settingCategory == SettingCategory.Keybinds)
		{
			this.LoadKeybinds();
			return;
		}
		if (!this._settingsDictionary.ContainsKey(settingCategory))
		{
			return;
		}
		foreach (BaseSettingData baseSettingData in this._settingsDictionary[settingCategory])
		{
			if (baseSettingData.useHeader)
			{
				SettingOption_Header settingOption_Header = Object.Instantiate<SettingOption_Header>(this.headerPrefab, this.settingList);
				settingOption_Header.Setup(baseSettingData.headerText, this.headerColor);
				this._activeSettings.Add(settingOption_Header.gameObject);
			}
			FlagSettingData flagSettingData = baseSettingData as FlagSettingData;
			if (flagSettingData == null)
			{
				FloatSettingData floatSettingData = baseSettingData as FloatSettingData;
				if (floatSettingData == null)
				{
					IntSettingData intSettingData = baseSettingData as IntSettingData;
					if (intSettingData == null)
					{
						VectorSettingData vectorSettingData = baseSettingData as VectorSettingData;
						if (vectorSettingData != null)
						{
							SettingOption_Vector settingOption_Vector = Object.Instantiate<SettingOption_Vector>(this.vectorSettingPrefab, this.settingList);
							settingOption_Vector.Setup(vectorSettingData, this._alternate ? this.colorOne : this.colorTwo);
							this._activeSettings.Add(settingOption_Vector.gameObject);
						}
					}
					else
					{
						SettingOption_Int settingOption_Int = Object.Instantiate<SettingOption_Int>(this.intSettingPrefab, this.settingList);
						settingOption_Int.Setup(intSettingData, this._alternate ? this.colorOne : this.colorTwo);
						this._activeSettings.Add(settingOption_Int.gameObject);
					}
				}
				else
				{
					SettingOption_Float settingOption_Float = Object.Instantiate<SettingOption_Float>(this.floatSettingPrefab, this.settingList);
					settingOption_Float.Setup(floatSettingData, this._alternate ? this.colorOne : this.colorTwo);
					this._activeSettings.Add(settingOption_Float.gameObject);
				}
			}
			else
			{
				SettingOption_Flag settingOption_Flag = Object.Instantiate<SettingOption_Flag>(this.flagSettingPrefab, this.settingList);
				settingOption_Flag.Setup(flagSettingData, this._alternate ? this.colorOne : this.colorTwo);
				this._activeSettings.Add(settingOption_Flag.gameObject);
			}
			this._alternate = !this._alternate;
		}
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x0003C684 File Offset: 0x0003A884
	private void LoadKeybinds()
	{
		InputActionMap inputActionMap = Singleton<Settings>.Instance.GetInputActions.asset.FindActionMap(this._desiredActionMap, false);
		string value = "Keyboard&Mouse";
		if (inputActionMap == null)
		{
			Debug.LogError("Action map '" + this._desiredActionMap + "' not found!");
			return;
		}
		Dictionary<string, string> keybinds = Singleton<Settings>.Instance.SettingsData.keybinds;
		foreach (InputAction inputAction in inputActionMap.actions)
		{
			if (!this._ignoredActions.Contains(inputAction.name))
			{
				bool flag = false;
				int num = -1;
				for (int i = 0; i < inputAction.bindings.Count; i++)
				{
					InputBinding inputBinding = inputAction.bindings[i];
					if (string.IsNullOrEmpty(inputBinding.groups) || inputBinding.groups.Contains(value))
					{
						if (inputBinding.isComposite)
						{
							flag = true;
							num = i;
						}
						else
						{
							if (inputBinding.isPartOfComposite)
							{
								if (!flag)
								{
									goto IL_1AA;
								}
							}
							else
							{
								flag = false;
							}
							string key = string.Format("{0}{1}{2}", inputAction.actionMap.name, inputAction.name, i);
							string path;
							if (keybinds.TryGetValue(key, out path))
							{
								inputAction.ApplyBindingOverride(i, path);
							}
							if (!string.IsNullOrEmpty(inputBinding.effectivePath) && (!inputBinding.isPartOfComposite || num == i - 1))
							{
								SettingOption_Keybind settingOption_Keybind = Object.Instantiate<SettingOption_Keybind>(this.keybindSettingPrefab, this.settingList);
								settingOption_Keybind.Setup(inputAction, flag ? num : i, this._alternate ? this.colorOne : this.colorTwo);
								this._activeSettings.Add(settingOption_Keybind.gameObject);
								this._alternate = !this._alternate;
							}
						}
					}
					IL_1AA:;
				}
			}
		}
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x0003C890 File Offset: 0x0003AA90
	public void Open()
	{
		this.canvasGroup.alpha = 1f;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x0003C8BA File Offset: 0x0003AABA
	public void Close()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		Singleton<Settings>.Instance.UpdateSettings();
	}

	// Token: 0x040009C5 RID: 2501
	public CanvasGroup canvasGroup;

	// Token: 0x040009C6 RID: 2502
	public Transform settingList;

	// Token: 0x040009C7 RID: 2503
	public TextMeshProUGUI settingsTitle;

	// Token: 0x040009C8 RID: 2504
	public Color headerColor;

	// Token: 0x040009C9 RID: 2505
	public Color colorOne;

	// Token: 0x040009CA RID: 2506
	public Color colorTwo;

	// Token: 0x040009CB RID: 2507
	private bool _alternate;

	// Token: 0x040009CC RID: 2508
	public SettingOption_Header headerPrefab;

	// Token: 0x040009CD RID: 2509
	public SettingOption_Flag flagSettingPrefab;

	// Token: 0x040009CE RID: 2510
	public SettingOption_Float floatSettingPrefab;

	// Token: 0x040009CF RID: 2511
	public SettingOption_Int intSettingPrefab;

	// Token: 0x040009D0 RID: 2512
	public SettingOption_Vector vectorSettingPrefab;

	// Token: 0x040009D1 RID: 2513
	public SettingOption_Keybind keybindSettingPrefab;

	// Token: 0x040009D2 RID: 2514
	private Dictionary<SettingCategory, List<BaseSettingData>> _settingsDictionary = new Dictionary<SettingCategory, List<BaseSettingData>>();

	// Token: 0x040009D3 RID: 2515
	private List<GameObject> _activeSettings = new List<GameObject>();

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	private string _desiredActionMap = "Player";

	// Token: 0x040009D5 RID: 2517
	[SerializeField]
	private List<string> _ignoredActions = new List<string>();

	// Token: 0x040009D6 RID: 2518
	private bool _isSetup;
}
