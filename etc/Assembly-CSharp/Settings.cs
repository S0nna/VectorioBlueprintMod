using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Vectorio;
using Vectorio.Settings;
using Vectorio.Settings.Data;

// Token: 0x02000197 RID: 407
[DefaultExecutionOrder(-1)]
public class Settings : Singleton<Settings>
{
	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000D81 RID: 3457 RVA: 0x0003B3E3 File Offset: 0x000395E3
	public InputActions GetInputActions
	{
		get
		{
			return this._inputActions;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0003B3EB File Offset: 0x000395EB
	public SettingsData SettingsData
	{
		get
		{
			return this._settingsData;
		}
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x0003B3F4 File Offset: 0x000395F4
	public void Setup()
	{
		Debug.Log("[SETTINGS] Beginning initial setup...");
		this._settingsData = Singleton<SaveSystem>.Instance.LoadSettingsAsync();
		List<BaseSettingData> list = Library.RequestAllDataOfType<BaseSettingData>();
		foreach (BaseSettingData baseSettingData in list)
		{
			FlagSettingData flagSettingData = baseSettingData as FlagSettingData;
			FlagSetting setting4;
			if (flagSettingData == null)
			{
				FloatSettingData floatSettingData = baseSettingData as FloatSettingData;
				FloatSetting setting3;
				if (floatSettingData == null)
				{
					IntSettingData intSettingData = baseSettingData as IntSettingData;
					IntSetting setting2;
					if (intSettingData == null)
					{
						VectorSettingData vectorSettingData = baseSettingData as VectorSettingData;
						if (vectorSettingData != null)
						{
							VectorSetting setting;
							if (this._settingsData.TryGetSetting<VectorSetting>(vectorSettingData.ID, out setting))
							{
								this.CheckSetting<VectorSetting>(vectorSettingData.ID, setting);
							}
							else
							{
								this.AddSetting<VectorSetting>(vectorSettingData.ID, new VectorSetting
								{
									ValueX = vectorSettingData.GetDefaultX,
									ValueY = vectorSettingData.GetDefaultY
								});
							}
						}
					}
					else if (this._settingsData.TryGetSetting<IntSetting>(intSettingData.ID, out setting2))
					{
						this.CheckSetting<IntSetting>(intSettingData.ID, setting2);
					}
					else
					{
						this.AddSetting<IntSetting>(intSettingData.ID, new IntSetting
						{
							Value = intSettingData.GetDefault()
						});
					}
				}
				else if (this._settingsData.TryGetSetting<FloatSetting>(floatSettingData.ID, out setting3))
				{
					this.CheckSetting<FloatSetting>(floatSettingData.ID, setting3);
				}
				else
				{
					this.AddSetting<FloatSetting>(floatSettingData.ID, new FloatSetting
					{
						Value = floatSettingData.defaultValue
					});
				}
			}
			else if (this._settingsData.TryGetSetting<FlagSetting>(flagSettingData.ID, out setting4))
			{
				this.CheckSetting<FlagSetting>(flagSettingData.ID, setting4);
			}
			else
			{
				this.AddSetting<FlagSetting>(flagSettingData.ID, new FlagSetting
				{
					Value = flagSettingData.defaultValue
				});
			}
		}
		try
		{
			this._inputActions = new InputActions();
			this.LoadAllBindings();
		}
		catch (Exception ex)
		{
			Debug.Log("[SETTINGS] Ran into an error while parsing keybind data!\n\nMessage: " + ex.Message + "\nStack: " + ex.StackTrace);
			this._inputActions = new InputActions();
		}
		this._settingsUI.Setup(list);
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x0003B648 File Offset: 0x00039848
	public void AddSetting<T>(string id, T setting) where T : BaseSetting
	{
		this._settingsData.AddSetting<T>(id, setting);
		this.CheckSetting<T>(id, setting);
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x0003B65F File Offset: 0x0003985F
	public bool TryGetSetting<T>(string id, out T typedSetting) where T : BaseSetting
	{
		return this._settingsData.TryGetSetting<T>(id, out typedSetting);
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0003B670 File Offset: 0x00039870
	private void CheckSetting<T>(string id, T setting) where T : BaseSetting
	{
		FlagSetting flagSetting = setting as FlagSetting;
		if (flagSetting != null)
		{
			this.CheckFlagSetting(id, flagSetting);
			return;
		}
		FloatSetting floatSetting = setting as FloatSetting;
		if (floatSetting != null)
		{
			this.CheckFloatSetting(id, floatSetting);
			return;
		}
		IntSetting intSetting = setting as IntSetting;
		if (intSetting != null)
		{
			this.CheckIntSetting(id, intSetting);
			return;
		}
		VectorSetting vectorSetting = setting as VectorSetting;
		if (vectorSetting == null)
		{
			return;
		}
		this.CheckVectorSetting(id, vectorSetting);
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0003B6E0 File Offset: 0x000398E0
	private void CheckFlagSetting(string id, FlagSetting flag)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
		if (num > 1842723226U)
		{
			if (num <= 2688061118U)
			{
				if (num != 2395556736U)
				{
					if (num != 2688061118U)
					{
						return;
					}
					if (!(id == "setting_film_grain"))
					{
						return;
					}
					if (Singleton<VolumeManager>.Instance != null)
					{
						Singleton<VolumeManager>.Instance.ToggleFilmGrain(flag.Value);
						return;
					}
					Debug.Log("[SETTINGS] No volume manager active in this scene!");
				}
				else
				{
					if (!(id == "setting_damage_numbers"))
					{
						return;
					}
					this._useDamageNumbers = flag;
					return;
				}
			}
			else if (num != 3342672886U)
			{
				if (num == 3756455580U)
				{
					if (!(id == "setting_vertical_sync"))
					{
						return;
					}
					QualitySettings.vSyncCount = (flag.Value ? 1 : 0);
					return;
				}
			}
			else
			{
				if (!(id == "setting_vignette"))
				{
					return;
				}
				if (Singleton<VolumeManager>.Instance != null)
				{
					Singleton<VolumeManager>.Instance.ToggleVignette(flag.Value);
					return;
				}
				Debug.Log("[SETTINGS] No volume manager active in this scene!");
				return;
			}
			return;
		}
		if (num <= 717455571U)
		{
			if (num != 498019544U)
			{
				if (num != 717455571U)
				{
					return;
				}
				if (!(id == "setting_use_particles"))
				{
					return;
				}
				this._useParticles = flag;
				return;
			}
			else
			{
				if (!(id == "setting_show_hints"))
				{
					return;
				}
				this._useHints = flag;
				return;
			}
		}
		else if (num != 1709267658U)
		{
			if (num != 1842723226U)
			{
				return;
			}
			if (!(id == "setting_drag_movement"))
			{
				return;
			}
			this._useDragMovement = flag;
			return;
		}
		else
		{
			if (!(id == "setting_screen_panning"))
			{
				return;
			}
			this._useScreenPanning = flag;
			return;
		}
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x0003B850 File Offset: 0x00039A50
	private void CheckFloatSetting(string id, FloatSetting flag)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
		if (num <= 994319426U)
		{
			if (num <= 126387193U)
			{
				if (num != 12890705U)
				{
					if (num != 126387193U)
					{
						return;
					}
					if (!(id == "setting_master_volume"))
					{
						return;
					}
					this._masterVolume = flag;
					Singleton<Events>.Instance.onMusicVolumeChanged.Invoke();
					return;
				}
				else
				{
					if (!(id == "setting_effects_volume"))
					{
						return;
					}
					this._effectsVolume = flag;
					return;
				}
			}
			else if (num != 685465311U)
			{
				if (num != 994319426U)
				{
					return;
				}
				if (!(id == "setting_interface_scale"))
				{
					return;
				}
				this._interfaceScale = flag;
				Singleton<Events>.Instance.onInterfaceScaleChanged.Invoke(this._interfaceScale.Value);
				return;
			}
			else
			{
				if (!(id == "setting_bloom"))
				{
					return;
				}
				if (Singleton<VolumeManager>.Instance != null)
				{
					Singleton<VolumeManager>.Instance.SetBloom(flag.Value);
					return;
				}
				Debug.Log("[SETTINGS] No volume manager active in this scene!");
				return;
			}
		}
		else if (num <= 3187953930U)
		{
			if (num != 1769241490U)
			{
				if (num != 3187953930U)
				{
					return;
				}
				if (!(id == "setting_music_volume"))
				{
					return;
				}
				this._musicVolume = flag;
				Singleton<Events>.Instance.onMusicVolumeChanged.Invoke();
				return;
			}
			else
			{
				if (!(id == "setting_interface_volume"))
				{
					return;
				}
				this._interfaceVolume = flag;
				return;
			}
		}
		else if (num != 3568523949U)
		{
			if (num != 4142025383U)
			{
				return;
			}
			if (!(id == "setting_zoom_multiplier"))
			{
				return;
			}
			this._scrollMultiplier = flag;
			return;
		}
		else
		{
			if (!(id == "setting_movement_multiplier"))
			{
				return;
			}
			this._movementMultiplier = flag;
			return;
		}
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x0003B9CC File Offset: 0x00039BCC
	private void CheckIntSetting(string id, IntSetting flag)
	{
		if (id == "setting_framerate")
		{
			Application.targetFrameRate = flag.Value;
			return;
		}
		if (!(id == "setting_screen_mode"))
		{
			return;
		}
		FullScreenMode value = (FullScreenMode)flag.Value;
		Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, value);
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x0003BA27 File Offset: 0x00039C27
	private void CheckVectorSetting(string id, VectorSetting flag)
	{
		if (id == "setting_resolution")
		{
			Screen.SetResolution(flag.ValueX, flag.ValueY, Screen.fullScreenMode);
		}
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x0003BA4C File Offset: 0x00039C4C
	public float GetMusicVolume()
	{
		return this._masterVolume.Value * this._musicVolume.Value;
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0003BA65 File Offset: 0x00039C65
	public float GetEffectsVolume()
	{
		return this._masterVolume.Value * this._effectsVolume.Value;
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0003BA7E File Offset: 0x00039C7E
	public float GetInterfaceVolume()
	{
		return this._masterVolume.Value * this._interfaceVolume.Value;
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0003BA97 File Offset: 0x00039C97
	public float InterfaceScale
	{
		get
		{
			return this._interfaceScale.Value;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0003BAA4 File Offset: 0x00039CA4
	public float MovementMultiplier
	{
		get
		{
			return this._movementMultiplier.Value;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0003BAB1 File Offset: 0x00039CB1
	public float ScrollMultiplier
	{
		get
		{
			return this._scrollMultiplier.Value;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0003BABE File Offset: 0x00039CBE
	public bool UseDamageNumbers
	{
		get
		{
			return this._useDamageNumbers.Value;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0003BACB File Offset: 0x00039CCB
	public bool UseHints
	{
		get
		{
			return this._useHints.Value;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0003BAD8 File Offset: 0x00039CD8
	public bool UseParticles
	{
		get
		{
			return this._useParticles.Value;
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0003BAE5 File Offset: 0x00039CE5
	public bool UseDragMovement
	{
		get
		{
			return this._useDragMovement.Value;
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000D95 RID: 3477 RVA: 0x0003BAF2 File Offset: 0x00039CF2
	public bool UseScreenPanning
	{
		get
		{
			return this._useScreenPanning.Value;
		}
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0003BB00 File Offset: 0x00039D00
	public void StartRebinding(string actionName, int bindingIndex, TextMeshProUGUI status, Action onRebindComplete, Action onRebindCanceled)
	{
		if (string.IsNullOrEmpty(actionName) || this._inputActions == null)
		{
			Debug.LogError("[SETTINGS] Invalid action name or input actions asset is null.");
			return;
		}
		InputAction inputAction = this._inputActions.FindAction(actionName, false);
		if (inputAction == null || bindingIndex < 0 || bindingIndex >= inputAction.bindings.Count)
		{
			Debug.LogError(string.Format("[SETTINGS] Invalid action '{0}' or binding index {1}.", actionName, bindingIndex));
			return;
		}
		if (inputAction.bindings[bindingIndex].isComposite)
		{
			int num = bindingIndex + 1;
			if (num < inputAction.bindings.Count && !inputAction.bindings[num].isComposite)
			{
				this.StartRebinding(inputAction, num, status, true, onRebindComplete, onRebindCanceled);
				return;
			}
		}
		else
		{
			this.StartRebinding(inputAction, bindingIndex, status, false, onRebindComplete, onRebindCanceled);
		}
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0003BBCC File Offset: 0x00039DCC
	private void StartRebinding(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI status, bool isComposite, Action onRebindComplete, Action onRebindCanceled)
	{
		if (actionToRebind == null || bindingIndex < 0)
		{
			Debug.LogError("[SETTINGS] Invalid parameters passed, ignoring rebind request.");
			return;
		}
		TextMeshProUGUI status2 = status;
		if (status2 != null)
		{
			status2.SetText("PRESS A " + actionToRebind.expectedControlType, true);
		}
		actionToRebind.Disable();
		actionToRebind.PerformInteractiveRebinding(bindingIndex).WithTimeout(10f).OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation operation)
		{
			actionToRebind.Enable();
			this.UpdateBinding(actionToRebind.name, bindingIndex, operation.selectedControl.path);
			operation.Dispose();
			if (isComposite)
			{
				int num = bindingIndex + 1;
				if (num < actionToRebind.bindings.Count && !actionToRebind.bindings[num].isComposite)
				{
					this.StartRebinding(actionToRebind, num, status, isComposite, onRebindComplete, onRebindCanceled);
					return;
				}
			}
			Action onRebindComplete2 = onRebindComplete;
			if (onRebindComplete2 == null)
			{
				return;
			}
			onRebindComplete2();
		}).OnCancel(delegate(InputActionRebindingExtensions.RebindingOperation operation)
		{
			actionToRebind.Enable();
			operation.Dispose();
			Action onRebindCanceled2 = onRebindCanceled;
			if (onRebindCanceled2 == null)
			{
				return;
			}
			onRebindCanceled2();
		}).Start();
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x0003BCA4 File Offset: 0x00039EA4
	public string GetBindingName(string actionName, int bindingIndex)
	{
		InputActions inputActions = this._inputActions;
		InputAction inputAction = (inputActions != null) ? inputActions.FindAction(actionName, false) : null;
		if (inputAction == null)
		{
			Debug.LogError("[SETTINGS] Action '" + actionName + "' not found.");
			return string.Empty;
		}
		return inputAction.GetBindingDisplayString(bindingIndex, (InputBinding.DisplayStringOptions)0);
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x0003BCEC File Offset: 0x00039EEC
	public void UpdateBinding(string actionName, int bindingIndex, string bindingOverride)
	{
		InputActions inputActions = this._inputActions;
		InputAction inputAction = (inputActions != null) ? inputActions.FindAction(actionName, false) : null;
		if (inputAction == null)
		{
			Debug.LogError("[SETTINGS] Action '" + actionName + "' not found.");
			return;
		}
		inputAction.ApplyBindingOverride(bindingIndex, bindingOverride);
		this.SaveBindingOverride(inputAction);
		Singleton<Events>.Instance.onUpdatedKeybinds.Invoke(this._inputActions);
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0003BD4C File Offset: 0x00039F4C
	private void SaveBindingOverride(InputAction action)
	{
		if (this._settingsData == null)
		{
			Debug.LogError("[SETTINGS] Settings data is null. Cannot save binding override.");
			return;
		}
		for (int i = 0; i < action.bindings.Count; i++)
		{
			string text = string.Format("{0}{1}{2}", action.actionMap.name, action.name, i);
			string effectivePath = action.bindings[i].effectivePath;
			if (!string.IsNullOrEmpty(effectivePath))
			{
				string a;
				if (!this._settingsData.keybinds.TryGetValue(text, out a) || a != effectivePath)
				{
					this._settingsData.keybinds[text] = effectivePath;
					Debug.Log("[SETTINGS] Updated binding override for " + text + ": " + effectivePath);
				}
			}
			else if (this._settingsData.keybinds.ContainsKey(text))
			{
				this._settingsData.keybinds.Remove(text);
				Debug.Log("[SETTINGS] Removed binding override for " + text);
			}
		}
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x0003BE4C File Offset: 0x0003A04C
	private void LoadAllBindings()
	{
		if (this._settingsData == null || this._inputActions == null)
		{
			Debug.LogError("[SETTINGS] Settings data or input actions are null. Cannot load bindings.");
			return;
		}
		foreach (InputActionMap inputActionMap in this._inputActions.asset.actionMaps)
		{
			foreach (InputAction inputAction in inputActionMap.actions)
			{
				for (int i = 0; i < inputAction.bindings.Count; i++)
				{
					string text = string.Format("{0}{1}{2}", inputActionMap.name, inputAction.name, i);
					string text2;
					if (this._settingsData.keybinds.TryGetValue(text, out text2))
					{
						if (string.IsNullOrEmpty(text2))
						{
							Debug.Log("[SETTINGS] Skipping null or empty override path for " + text);
						}
						else
						{
							try
							{
								inputAction.ApplyBindingOverride(i, text2);
								Debug.Log("[SETTINGS] Applied binding override for " + text + ": " + text2);
							}
							catch (Exception ex)
							{
								Debug.LogError("[SETTINGS] Error applying binding override for " + text + ": " + ex.Message);
							}
						}
					}
				}
			}
		}
		Singleton<Events>.Instance.onUpdatedKeybinds.Invoke(this._inputActions);
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x0003C010 File Offset: 0x0003A210
	public void UpdateSettings()
	{
		Singleton<SaveSystem>.Instance.WriteSettingsToFile(this._settingsData);
	}

	// Token: 0x04000997 RID: 2455
	[SerializeField]
	private SettingsUI _settingsUI;

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	private InputActions _inputActions;

	// Token: 0x04000999 RID: 2457
	private SettingsData _settingsData = new SettingsData();

	// Token: 0x0400099A RID: 2458
	private FloatSetting _masterVolume = new FloatSetting
	{
		Value = 0.5f
	};

	// Token: 0x0400099B RID: 2459
	private FloatSetting _effectsVolume = new FloatSetting
	{
		Value = 0.5f
	};

	// Token: 0x0400099C RID: 2460
	private FloatSetting _musicVolume = new FloatSetting
	{
		Value = 0.5f
	};

	// Token: 0x0400099D RID: 2461
	private FloatSetting _interfaceVolume = new FloatSetting
	{
		Value = 0.5f
	};

	// Token: 0x0400099E RID: 2462
	private FloatSetting _interfaceScale = new FloatSetting
	{
		Value = 1f
	};

	// Token: 0x0400099F RID: 2463
	private FloatSetting _movementMultiplier = new FloatSetting
	{
		Value = 1f
	};

	// Token: 0x040009A0 RID: 2464
	private FloatSetting _scrollMultiplier = new FloatSetting
	{
		Value = 1f
	};

	// Token: 0x040009A1 RID: 2465
	private FlagSetting _useDamageNumbers = new FlagSetting
	{
		Value = true
	};

	// Token: 0x040009A2 RID: 2466
	private FlagSetting _useHints = new FlagSetting
	{
		Value = true
	};

	// Token: 0x040009A3 RID: 2467
	private FlagSetting _useParticles = new FlagSetting
	{
		Value = true
	};

	// Token: 0x040009A4 RID: 2468
	private FlagSetting _useDragMovement = new FlagSetting
	{
		Value = true
	};

	// Token: 0x040009A5 RID: 2469
	private FlagSetting _useScreenPanning = new FlagSetting
	{
		Value = true
	};
}
