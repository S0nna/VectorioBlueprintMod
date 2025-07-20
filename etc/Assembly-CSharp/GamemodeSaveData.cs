using System;
using System.Collections.Generic;

// Token: 0x02000140 RID: 320
[Serializable]
public class GamemodeSaveData
{
	// Token: 0x06000AA5 RID: 2725 RVA: 0x0002CA78 File Offset: 0x0002AC78
	public GamemodeSaveData(GamemodeData gamemodeData)
	{
		this.ID = gamemodeData.ID;
		this.options = gamemodeData.defaultOptions;
		this.allyModifiers = new Dictionary<int, float>();
		this.enemyModifiers = new Dictionary<int, float>();
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0002CAC4 File Offset: 0x0002ACC4
	public GamemodeSaveData()
	{
		this.ID = "";
		this.options = (GamemodeOptions.UseBuilderDrones | GamemodeOptions.UseReclaimers);
		this.allyModifiers = new Dictionary<int, float>();
		this.enemyModifiers = new Dictionary<int, float>();
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0002CAFF File Offset: 0x0002ACFF
	// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x0002CB07 File Offset: 0x0002AD07
	public string PresetID { get; set; } = "";

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0002CB10 File Offset: 0x0002AD10
	public void AddAllyModifier(int key, float modifier)
	{
		if (!this.allyModifiers.ContainsKey(key))
		{
			this.allyModifiers.Add(key, modifier);
			return;
		}
		this.allyModifiers[key] = modifier;
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0002CB3B File Offset: 0x0002AD3B
	public void AddEnemyModifier(int key, float modifier)
	{
		if (!this.enemyModifiers.ContainsKey(key))
		{
			this.enemyModifiers.Add(key, modifier);
			return;
		}
		this.enemyModifiers[key] = modifier;
	}

	// Token: 0x040006A5 RID: 1701
	public string ID;

	// Token: 0x040006A6 RID: 1702
	public GamemodeOptions options;

	// Token: 0x040006A8 RID: 1704
	public Dictionary<int, float> allyModifiers;

	// Token: 0x040006A9 RID: 1705
	public Dictionary<int, float> enemyModifiers;
}
