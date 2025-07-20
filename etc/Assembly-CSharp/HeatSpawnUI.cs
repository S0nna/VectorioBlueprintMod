using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.Formatting;

// Token: 0x020001C3 RID: 451
public class HeatSpawnUI : MonoBehaviour
{
	// Token: 0x06000E50 RID: 3664 RVA: 0x000402C0 File Offset: 0x0003E4C0
	public void Setup(HeatManager.Enemy enemy, Color color)
	{
		this._enemy = enemy;
		this.backgroundColor.color = color;
		HealthData component = enemy.UnitData.GetComponent<HealthData>();
		if (component != null)
		{
			this.health.text = Formatter.Round(component.health, 2);
		}
		else
		{
			this.health.text = "INVINCIBLE";
		}
		UnitData component2 = enemy.UnitData.GetComponent<UnitData>();
		if (component2 != null)
		{
			this.damage.text = Formatter.Round(component2.damage, 2);
			this.speed.text = Formatter.Round(component2.moveSpeed, 2);
		}
		this.heatAmount.text = enemy.HeatLevel().ToString();
		this.intervalProgress.minValue = enemy.spawnData.maxCooldown;
		this.intervalProgress.maxValue = this._enemy.spawnData.baseCooldown;
		this.intervalProgress.currentPercent = this.intervalProgress.maxValue;
		this.intervalProgress.UpdateUI();
		this.timeProgress.minValue = 0f;
		this.timeProgress.maxValue = enemy.cooldown;
		this.timeProgress.currentPercent = this.timeProgress.maxValue;
		this.timeProgress.UpdateUI();
		this.time.text = Formatter.Round(enemy.cooldown, 1) + " seconds";
		this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(enemy.spawnData.unit.model, this.iconSize, this.iconParent, Singleton<HeatManager>.Instance.FactionData.accent);
		this._model.transform.localScale = Vector2.one;
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x00040478 File Offset: 0x0003E678
	public void Toggle(bool toggle)
	{
		if (toggle)
		{
			this.title.text = this._enemy.UnitData.Name.ToUpper();
			this.description.text = this._enemy.UnitData.Description;
			this.unlockedObj.SetActive(true);
			this.lockedObj.SetActive(false);
			this.heatParent.position = this.heatUnlockedPosition;
			return;
		}
		this.title.text = "UNKNOWN";
		this.description.text = "Heat is not yet high enough to attract this unit.";
		this.intervalProgress.currentPercent = this.intervalProgress.maxValue;
		this.intervalProgress.UpdateUI();
		this.unlockedObj.SetActive(false);
		this.lockedObj.SetActive(true);
		this.heatParent.position = this.heatLockedPosition;
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x00040564 File Offset: 0x0003E764
	public void CustomUpdate()
	{
		this.timeProgress.maxValue = this._enemy.cooldown;
		this.timeProgress.currentPercent = this._enemy.timer;
		this.timeProgress.UpdateUI();
		this.intervalProgress.currentPercent = this._enemy.cooldown;
		this.intervalProgress.UpdateUI();
		this.time.text = Formatter.Round(this._enemy.timer, 1) + " seconds";
	}

	// Token: 0x04000B21 RID: 2849
	public TextMeshProUGUI title;

	// Token: 0x04000B22 RID: 2850
	public TextMeshProUGUI description;

	// Token: 0x04000B23 RID: 2851
	public TextMeshProUGUI heatAmount;

	// Token: 0x04000B24 RID: 2852
	public TextMeshProUGUI health;

	// Token: 0x04000B25 RID: 2853
	public TextMeshProUGUI damage;

	// Token: 0x04000B26 RID: 2854
	public TextMeshProUGUI speed;

	// Token: 0x04000B27 RID: 2855
	public TextMeshProUGUI time;

	// Token: 0x04000B28 RID: 2856
	public GameObject unlockedObj;

	// Token: 0x04000B29 RID: 2857
	public GameObject lockedObj;

	// Token: 0x04000B2A RID: 2858
	public Transform heatParent;

	// Token: 0x04000B2B RID: 2859
	public Transform iconParent;

	// Token: 0x04000B2C RID: 2860
	public ProgressBar timeProgress;

	// Token: 0x04000B2D RID: 2861
	public ProgressBar intervalProgress;

	// Token: 0x04000B2E RID: 2862
	public Vector2 iconSize;

	// Token: 0x04000B2F RID: 2863
	public Vector2 heatLockedPosition;

	// Token: 0x04000B30 RID: 2864
	public Vector2 heatUnlockedPosition;

	// Token: 0x04000B31 RID: 2865
	public Image backgroundColor;

	// Token: 0x04000B32 RID: 2866
	private HeatManager.Enemy _enemy;

	// Token: 0x04000B33 RID: 2867
	private GameObject _model;
}
