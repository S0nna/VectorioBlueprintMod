using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x0200013C RID: 316
[DefaultExecutionOrder(0)]
public class Gamemode : Singleton<Gamemode>
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0002C654 File Offset: 0x0002A854
	// (set) Token: 0x06000A80 RID: 2688 RVA: 0x0002C64B File Offset: 0x0002A84B
	public GamemodeData GamemodeData
	{
		get
		{
			return this._gamemodeData;
		}
		set
		{
			this._gamemodeData = value;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000A82 RID: 2690 RVA: 0x0002C65C File Offset: 0x0002A85C
	public bool IsOfflineScene
	{
		get
		{
			return this._isOfflineScene;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0002C696 File Offset: 0x0002A896
	// (set) Token: 0x06000A83 RID: 2691 RVA: 0x0002C664 File Offset: 0x0002A864
	public bool IsGamePaused
	{
		get
		{
			return this.IsPaused;
		}
		set
		{
			this._gamePaused = value;
			Singleton<Events>.Instance.onPauseStateUpdated.Invoke();
			Debug.Log("[GAMEMODE] Game pause state toggled to: " + this._gamePaused.ToString());
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0002C69E File Offset: 0x0002A89E
	public bool IsPaused
	{
		get
		{
			return this._gamePaused || Singleton<NetworkPlayerManager>.Instance.IsPlayerLoading;
		}
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0002C6B4 File Offset: 0x0002A8B4
	public void Update()
	{
		this._gameTime += Time.deltaTime;
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
	// (set) Token: 0x06000A88 RID: 2696 RVA: 0x0002C6D0 File Offset: 0x0002A8D0
	public float GameTime
	{
		get
		{
			return this._gameTime;
		}
		set
		{
			this._gameTime = value;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0002C6D9 File Offset: 0x0002A8D9
	// (set) Token: 0x06000A8A RID: 2698 RVA: 0x0002C6E1 File Offset: 0x0002A8E1
	public bool ForceDisableSounds
	{
		get
		{
			return this._forceDisableSounds;
		}
		set
		{
			this._forceDisableSounds = value;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0002C6EA File Offset: 0x0002A8EA
	// (set) Token: 0x06000A8C RID: 2700 RVA: 0x0002C6F2 File Offset: 0x0002A8F2
	public bool ForceDisableMovement
	{
		get
		{
			return this._forceDisableMovement;
		}
		set
		{
			this._forceDisableMovement = value;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0002C6FB File Offset: 0x0002A8FB
	public bool EnforceTileRestrictions
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseTileRestrictions);
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002C70A File Offset: 0x0002A90A
	public bool AllowFactionSwitching
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.AllowFactionSwitching);
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0002C718 File Offset: 0x0002A918
	public bool AllowDeveloperTools
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.AllowDeveloperTools);
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0002C726 File Offset: 0x0002A926
	public bool UseHub
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseHub);
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0002C735 File Offset: 0x0002A935
	public bool UseBorder
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseBorder);
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0002C747 File Offset: 0x0002A947
	public bool UseResearch
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseResearch);
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000A93 RID: 2707 RVA: 0x0002C759 File Offset: 0x0002A959
	public bool UseResources
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseResources);
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002C76B File Offset: 0x0002A96B
	public bool UseHeatSpawning
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseHeatSpawning);
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0002C77D File Offset: 0x0002A97D
	public bool UseEntityAnimations
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseEntityAnimations);
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002C78F File Offset: 0x0002A98F
	public bool UseWelcomeScreen
	{
		get
		{
			return this.GamemodeData.HasRule(GamemodeRules.UseWelcomeScreen);
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0002C7A1 File Offset: 0x0002A9A1
	public bool UseEntitySounds
	{
		get
		{
			return !this.ForceDisableSounds && this.GamemodeData.HasRule(GamemodeRules.UseEntitySounds);
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002C7BD File Offset: 0x0002A9BD
	public bool AllowClientMovement
	{
		get
		{
			return !(this.GamemodeData != null) || (!this.ForceDisableMovement && this.GamemodeData.HasRule(GamemodeRules.AllowClientMovement));
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0002C7E5 File Offset: 0x0002A9E5
	public bool HasOption(GamemodeOptions optionToCheck)
	{
		return (this._gamemodeOptions & optionToCheck) == optionToCheck;
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0002C7F2 File Offset: 0x0002A9F2
	public bool UseBuilderDrones
	{
		get
		{
			return this.HasOption(GamemodeOptions.UseBuilderDrones);
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0002C7FB File Offset: 0x0002A9FB
	public bool UseReclaimers
	{
		get
		{
			return this.HasOption(GamemodeOptions.UseReclaimers);
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0002C804 File Offset: 0x0002AA04
	public bool UsePower
	{
		get
		{
			return this.HasOption(GamemodeOptions.UsePower);
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x0002C80D File Offset: 0x0002AA0D
	public override void Awake()
	{
		if (this._isOfflineScene)
		{
			this.Setup(this._gamemodeData);
		}
		base.Awake();
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0002C64B File Offset: 0x0002A84B
	public void Setup(GamemodeData gamemode)
	{
		this._gamemodeData = gamemode;
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0002C82C File Offset: 0x0002AA2C
	public void SetGamemodeSaveData(GamemodeSaveData saveData)
	{
		GamemodeData gamemodeData = Library.RequestData<GamemodeData>(saveData.ID);
		if (gamemodeData != null)
		{
			this.GamemodeData = gamemodeData;
		}
		this._gamemodeOptions = saveData.options;
		foreach (KeyValuePair<int, float> keyValuePair in saveData.allyModifiers)
		{
			Singleton<StatManager>.Instance.AddAllyModifier(keyValuePair.Key, new StatModifier(keyValuePair.Value, true));
		}
		foreach (KeyValuePair<int, float> keyValuePair2 in saveData.enemyModifiers)
		{
			Singleton<StatManager>.Instance.AddEnemyModifier(keyValuePair2.Key, new StatModifier(keyValuePair2.Value, true));
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0002C918 File Offset: 0x0002AB18
	public GamemodeSaveData GetGamemodeSaveData()
	{
		GamemodeSaveData gamemodeSaveData = new GamemodeSaveData
		{
			ID = this.GamemodeData.ID,
			options = this._gamemodeOptions
		};
		foreach (KeyValuePair<int, StatModifier> keyValuePair in Singleton<StatManager>.Instance.AllyModifiers)
		{
			gamemodeSaveData.AddAllyModifier(keyValuePair.Key, keyValuePair.Value.Value);
		}
		foreach (KeyValuePair<int, StatModifier> keyValuePair2 in Singleton<StatManager>.Instance.EnemyModifiers)
		{
			gamemodeSaveData.AddEnemyModifier(keyValuePair2.Key, keyValuePair2.Value.Value);
		}
		return gamemodeSaveData;
	}

	// Token: 0x04000688 RID: 1672
	[SerializeField]
	private GamemodeData _gamemodeData;

	// Token: 0x04000689 RID: 1673
	[SerializeField]
	protected bool _isOfflineScene;

	// Token: 0x0400068A RID: 1674
	protected bool _gamePaused;

	// Token: 0x0400068B RID: 1675
	protected float _gameTime;

	// Token: 0x0400068C RID: 1676
	private bool _forceDisableSounds;

	// Token: 0x0400068D RID: 1677
	private bool _forceDisableMovement;

	// Token: 0x0400068E RID: 1678
	private GamemodeOptions _gamemodeOptions;
}
