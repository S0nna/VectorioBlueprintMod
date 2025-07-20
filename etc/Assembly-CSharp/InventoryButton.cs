using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x02000150 RID: 336
public class InventoryButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000AFB RID: 2811 RVA: 0x0002EC17 File Offset: 0x0002CE17
	public void LinkInventory(Inventory inventory)
	{
		this._inventory = inventory;
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0002EC20 File Offset: 0x0002CE20
	public bool IsButtonSet
	{
		get
		{
			return this._entityData != null;
		}
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0002EC2E File Offset: 0x0002CE2E
	public void Set(EntityData data, string modelID, string factionID, string metadata, bool unlocked)
	{
		this._entityData = data;
		this._modelID = modelID;
		this._factionID = factionID;
		this._metadata = metadata;
		this.UpdateStatus(unlocked);
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0002EC58 File Offset: 0x0002CE58
	public void UpdateStatus(bool unlocked)
	{
		if (unlocked)
		{
			this.title.text = this._entityData.Name.ToUpper();
			this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(this._entityData.model, new Vector2(40f, 40f), this.iconParent, null);
			this._model.transform.localScale = Vector2.one;
			this.lightColor = this._entityData.model.GetAccentColor(AccentType.PrimaryColor);
			this.darkColor = new Color(this.lightColor.r * this.darkColorMultiplier, this.lightColor.g * this.darkColorMultiplier, this.lightColor.b * this.darkColorMultiplier, 1f);
			this.background.color = this.lightColor;
			this.digitalBackground.color = this.darkColor;
			this.border.color = this.lightColor;
			this.typeIcon.color = this.darkColor;
			this.title.color = this.darkColor;
			if (this._entityData.UseSpecialCost && this._entityData.SpecialCost.resource != null)
			{
				this.typeIcon.sprite = this._entityData.SpecialCost.resource.IconSprite;
				this.typeBackground.color = this.lightColor;
			}
			else
			{
				this.typeIcon.gameObject.SetActive(false);
				this.typeBackground.gameObject.SetActive(false);
			}
			if (this._entityData.UseNormalCost && this._entityData.NormalCost.resource != null)
			{
				this.resourceIcon.sprite = this._entityData.NormalCost.resource.IconSprite;
				this.resourceAmount.text = this._entityData.NormalCost.amount.ToString();
			}
			else
			{
				this.resourceIcon.gameObject.SetActive(false);
				this.resourceAmount.gameObject.SetActive(false);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.resourceLayoutGroup.GetComponent<RectTransform>());
			this.unlockedObj.SetActive(true);
			this.lockedObj.SetActive(false);
			this._isActive = true;
			return;
		}
		this.unlockedObj.SetActive(false);
		this.lockedObj.SetActive(true);
		ResearchTreeData entityTree = Singleton<Research>.Instance.GetEntityTree(this._entityData);
		if (entityTree != null)
		{
			this.treeIcon.sprite = entityTree.icon;
			this.treeIcon.gameObject.SetActive(true);
			return;
		}
		this.treeIcon.sprite = this.defaultLockIcon;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0002EF19 File Offset: 0x0002D119
	public void ResetButtonState()
	{
		if (this._model != null)
		{
			Object.Destroy(this._model);
		}
		base.gameObject.SetActive(false);
		this._entityData = null;
		this._isActive = false;
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0002EF50 File Offset: 0x0002D150
	public void Equip()
	{
		if (!this._isActive)
		{
			return;
		}
		if (this._inventory != null)
		{
			this._inventory.panel.Set(this._entityData, this._modelID);
		}
		Singleton<Events>.Instance.onInventoryButtonClicked.Invoke(this._entityData);
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0002EFA8 File Offset: 0x0002D1A8
	public override void ToggleHover(bool toggle)
	{
		if (!this._isActive)
		{
			return;
		}
		if (toggle)
		{
			this._model.transform.localScale = new Vector2(1.05f, 1.05f);
		}
		else
		{
			this._model.transform.localScale = Vector2.one;
		}
		base.ToggleHover(toggle);
	}

	// Token: 0x0400072B RID: 1835
	protected EntityData _entityData;

	// Token: 0x0400072C RID: 1836
	protected string _modelID = "default";

	// Token: 0x0400072D RID: 1837
	protected string _factionID = "faction_player";

	// Token: 0x0400072E RID: 1838
	protected string _metadata = "";

	// Token: 0x0400072F RID: 1839
	protected GameObject _model;

	// Token: 0x04000730 RID: 1840
	public GameObject unlockedObj;

	// Token: 0x04000731 RID: 1841
	public GameObject lockedObj;

	// Token: 0x04000732 RID: 1842
	public Transform iconParent;

	// Token: 0x04000733 RID: 1843
	public Image border;

	// Token: 0x04000734 RID: 1844
	public Image background;

	// Token: 0x04000735 RID: 1845
	public Image digitalBackground;

	// Token: 0x04000736 RID: 1846
	public Image resourceIcon;

	// Token: 0x04000737 RID: 1847
	public Image typeIcon;

	// Token: 0x04000738 RID: 1848
	public Image typeBackground;

	// Token: 0x04000739 RID: 1849
	public Image treeIcon;

	// Token: 0x0400073A RID: 1850
	public TextMeshProUGUI title;

	// Token: 0x0400073B RID: 1851
	public TextMeshProUGUI resourceAmount;

	// Token: 0x0400073C RID: 1852
	public float darkColorMultiplier = 0.2f;

	// Token: 0x0400073D RID: 1853
	public HorizontalLayoutGroup resourceLayoutGroup;

	// Token: 0x0400073E RID: 1854
	public Sprite defaultLockIcon;

	// Token: 0x0400073F RID: 1855
	private Color lightColor;

	// Token: 0x04000740 RID: 1856
	private Color darkColor;

	// Token: 0x04000741 RID: 1857
	protected Inventory _inventory;

	// Token: 0x04000742 RID: 1858
	private bool _isActive;
}
