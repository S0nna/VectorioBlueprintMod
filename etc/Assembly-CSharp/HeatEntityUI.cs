using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.Formatting;

// Token: 0x020001BC RID: 444
public class HeatEntityUI : MonoBehaviour
{
	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0003F1C2 File Offset: 0x0003D3C2
	public Color GetLineColor
	{
		get
		{
			if (!this.bottomLine.gameObject.activeSelf)
			{
				return Color.white;
			}
			return this.bottomLine.color;
		}
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0003F1E8 File Offset: 0x0003D3E8
	public void Setup(EnemySpawn spawn, bool shown, Color previousColor)
	{
		if (this._model != null)
		{
			Object.Destroy(this._model);
		}
		if (spawn.unit != null)
		{
			this.entityName = spawn.unit.Name.ToUpper();
			HealthData component = spawn.unit.GetComponent<HealthData>();
			if (component != null)
			{
				this.health = Formatter.Round(component.health, 2);
			}
			else
			{
				this.health = "INVINCIBLE";
			}
			UnitData component2 = spawn.unit.GetComponent<UnitData>();
			if (component2 != null)
			{
				this.damage = Formatter.Round(component2.damage, 2);
				this.speed = Formatter.Round(component2.moveSpeed, 2);
				this.weight = Formatter.Round(component2.physicalMass, 2);
			}
		}
		else
		{
			this.entityName = "INVALID ENTITY";
			this.health = "???";
			this.damage = "???";
			this.speed = "???";
			this.weight = "???";
		}
		this.spawnRate = "1x / " + Formatter.Round(spawn.baseCooldown, 1) + " SECONDS";
		this._isToggled = !shown;
		this.Toggle(shown, previousColor);
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x0003F314 File Offset: 0x0003D514
	public void Toggle(bool toggle, Color previousColor)
	{
		if (this._isToggled == toggle)
		{
			return;
		}
		this._isToggled = toggle;
		if (toggle)
		{
			if (this._model != null)
			{
				this._model.SetActive(true);
			}
			this.hiddenIcon.gameObject.SetActive(false);
			this.title.text = this.entityName;
			this.subtitle.text = this.spawnRate;
			this.healthText.text = this.health;
			this.damageText.text = this.damage;
			this.speedText.text = this.speed;
			this.weightText.text = this.weight;
			this.title.color = Color.white;
			this.subtitle.color = this.lightColor;
			this.healthText.color = Color.white;
			this.damageText.color = Color.white;
			this.speedText.color = Color.white;
			this.weightText.color = Color.white;
			this.upperLine.color = previousColor;
			this.bottomLine.color = this.normalColor;
			return;
		}
		if (this._model != null)
		{
			this._model.SetActive(false);
		}
		this.hiddenIcon.gameObject.SetActive(true);
		this.title.text = "UNKNOWN UNIT";
		this.subtitle.text = "UNKNOWN RATE";
		this.healthText.text = "???";
		this.damageText.text = "???";
		this.speedText.text = "???";
		this.weightText.text = "???";
		this.title.color = this.hiddenColor;
		this.subtitle.color = this.hiddenColor;
		this.healthText.color = this.hiddenColor;
		this.damageText.color = this.hiddenColor;
		this.speedText.color = this.hiddenColor;
		this.weightText.color = this.hiddenColor;
		this.upperLine.color = previousColor;
		this.bottomLine.color = this.hiddenColor;
	}

	// Token: 0x04000AEB RID: 2795
	public Transform modelParent;

	// Token: 0x04000AEC RID: 2796
	public Image hiddenIcon;

	// Token: 0x04000AED RID: 2797
	protected GameObject _model;

	// Token: 0x04000AEE RID: 2798
	public TextMeshProUGUI title;

	// Token: 0x04000AEF RID: 2799
	public TextMeshProUGUI subtitle;

	// Token: 0x04000AF0 RID: 2800
	public TextMeshProUGUI healthText;

	// Token: 0x04000AF1 RID: 2801
	public TextMeshProUGUI damageText;

	// Token: 0x04000AF2 RID: 2802
	public TextMeshProUGUI speedText;

	// Token: 0x04000AF3 RID: 2803
	public TextMeshProUGUI weightText;

	// Token: 0x04000AF4 RID: 2804
	protected string entityName;

	// Token: 0x04000AF5 RID: 2805
	protected string spawnRate;

	// Token: 0x04000AF6 RID: 2806
	protected string health;

	// Token: 0x04000AF7 RID: 2807
	protected string damage;

	// Token: 0x04000AF8 RID: 2808
	protected string speed;

	// Token: 0x04000AF9 RID: 2809
	protected string weight;

	// Token: 0x04000AFA RID: 2810
	public Image upperLine;

	// Token: 0x04000AFB RID: 2811
	public Image bottomLine;

	// Token: 0x04000AFC RID: 2812
	public Color normalColor;

	// Token: 0x04000AFD RID: 2813
	public Color lightColor;

	// Token: 0x04000AFE RID: 2814
	public Color hiddenColor;

	// Token: 0x04000AFF RID: 2815
	public Vector2 modelSize;

	// Token: 0x04000B00 RID: 2816
	protected bool _isToggled;
}
