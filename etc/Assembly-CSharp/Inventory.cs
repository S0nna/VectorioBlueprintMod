using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.PhasmaUI;

// Token: 0x0200014D RID: 333
[DefaultExecutionOrder(0)]
public class Inventory : UI_Window
{
	// Token: 0x06000AE4 RID: 2788 RVA: 0x0002DFA8 File Offset: 0x0002C1A8
	public void Start()
	{
		Inventory._entities.Clear();
		this._categories = new Dictionary<Category, CategoryButton>();
		this._categoryLibrary = new Dictionary<Category, Inventory.CategoryButtonData>();
		foreach (Inventory.CategoryButtonData categoryButtonData in this.categoryButtons)
		{
			this._categoryLibrary.Add(categoryButtonData.category, categoryButtonData);
		}
		Singleton<Events>.Instance.onAddEntityToInventory.AddListener(new UnityAction<EntityData>(this.Add));
		Singleton<Events>.Instance.onEntityUnlocked.AddListener(new UnityAction<EntityData>(this.UpdateEntityStatus));
		Singleton<Events>.Instance.openInventoryToEntity.AddListener(new UnityAction<EntityData>(this.OpenToEntity));
		Singleton<Events>.Instance.onCategorySelected.AddListener(new UnityAction<int>(this.SelectCategory));
		Singleton<Events>.Instance.onSortAllCategories.AddListener(new UnityAction(this.SortAllCategories));
		Singleton<Events>.Instance.onSortCategory.AddListener(new UnityAction<Category>(this.SortCategory));
		InputManager.OnInventoryActionPressed.AddListener(new UnityAction(this.Toggle));
		InventoryRow item = Object.Instantiate<InventoryRow>(this.inventoryRowPrefab, this.inventoryRowList);
		this._inventoryRows.Add(item);
		Inventory.IsOpen = false;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0002E104 File Offset: 0x0002C304
	public void OpenToEntity(EntityData entity)
	{
		this.SelectCategory((int)entity.category);
		this.panel.Set(entity, "default");
		if (!Inventory.IsOpen)
		{
			this.Open();
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0002E130 File Offset: 0x0002C330
	public void Add(EntityData entity)
	{
		if (entity.category == Category.Unlisted)
		{
			return;
		}
		if (!Inventory._entities.ContainsKey(entity.category))
		{
			Inventory._entities.Add(entity.category, new List<EntityData>());
		}
		if (!Inventory._entities[entity.category].Contains(entity))
		{
			Inventory._entities[entity.category].Add(entity);
			this.UpdateCategory(entity.category);
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0002E1A8 File Offset: 0x0002C3A8
	public void SortCategory(Category category)
	{
		List<EntityData> list;
		if (Inventory._entities.TryGetValue(category, out list))
		{
			list.Sort((EntityData a, EntityData b) => a.inventoryIndex.CompareTo(b.inventoryIndex));
		}
		else
		{
			Debug.LogWarning(string.Format("No entity list found for category: {0}", category));
		}
		if (this._selectedCategory == category)
		{
			this.SelectCategory((int)category);
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0002E210 File Offset: 0x0002C410
	public void SortAllCategories()
	{
		foreach (Category category in Inventory._entities.Keys)
		{
			this.SortCategory(category);
		}
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0002E268 File Offset: 0x0002C468
	public void UpdateEntityStatus(EntityData entity)
	{
		if (entity.category == Category.Unlisted)
		{
			return;
		}
		if (!Inventory._entities.ContainsKey(entity.category))
		{
			Inventory._entities.Add(entity.category, new List<EntityData>());
			Inventory._entities[entity.category].Add(entity);
		}
		else if (!Inventory._entities[entity.category].Contains(entity))
		{
			Inventory._entities[entity.category].Add(entity);
		}
		this.UpdateCategory(entity.category);
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0002E2F8 File Offset: 0x0002C4F8
	public void SelectCategory(int categoryNumber)
	{
		if (!Inventory._entities.ContainsKey((Category)categoryNumber))
		{
			return;
		}
		this.ResetCategory();
		this._selectedCategory = (Category)categoryNumber;
		int num = 0;
		int num2 = 0;
		foreach (EntityData entityData in Inventory._entities[(Category)categoryNumber])
		{
			if (!this._inventoryRows[this._activeRowIndex].HasAvailableButton())
			{
				this._activeRowIndex++;
				if (this._inventoryRows.Count <= this._activeRowIndex)
				{
					InventoryRow item = Object.Instantiate<InventoryRow>(this.inventoryRowPrefab, this.inventoryRowList);
					this._inventoryRows.Add(item);
				}
			}
			if (!this._inventoryRows[this._activeRowIndex].gameObject.activeSelf)
			{
				this._inventoryRows[this._activeRowIndex].gameObject.SetActive(true);
			}
			bool flag = Singleton<Research>.Instance.IsEntityUnlocked(entityData);
			this._inventoryRows[this._activeRowIndex].LinkEntity(entityData, this, flag);
			if (flag)
			{
				num++;
			}
			num2++;
		}
		if (this.CheckCategory((Category)categoryNumber))
		{
			CategoryButton categoryButton = this._categories[(Category)categoryNumber];
			categoryButton.unlocked.text = num.ToString() + " / " + num2.ToString() + " UNLOCKED";
			categoryButton.background.color = this.selectedColor;
		}
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0002E488 File Offset: 0x0002C688
	private bool CheckCategory(Category category)
	{
		if (category == Category.Unlisted)
		{
			return false;
		}
		if (this._categories.ContainsKey(category))
		{
			return true;
		}
		if (!this._categoryLibrary.ContainsKey(category))
		{
			return false;
		}
		Inventory.CategoryButtonData categoryButtonData = this._categoryLibrary[category];
		CategoryButton categoryButton = Object.Instantiate<CategoryButton>(this.categoryButtonPrefab, this.categoryButtonList);
		categoryButton.GetComponent<RectTransform>().localScale = Vector2.one;
		categoryButton.Setup(categoryButtonData, this._alternate);
		this._alternate = !this._alternate;
		bool flag = false;
		this._categories.Add(categoryButtonData.category, categoryButton);
		this._categories = (from x in this._categories
		orderby (int)x.Key
		select x).ToDictionary((KeyValuePair<Category, CategoryButton> x) => x.Key, (KeyValuePair<Category, CategoryButton> x) => x.Value);
		int num = 0;
		foreach (KeyValuePair<Category, CategoryButton> keyValuePair in this._categories)
		{
			keyValuePair.Value.GetComponent<RectTransform>().SetSiblingIndex(num);
			keyValuePair.Value.SetOriginalColor(flag ? this.normalColorOne : this.normalColorTwo);
			flag = !flag;
			num++;
		}
		return true;
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0002E610 File Offset: 0x0002C810
	private void UpdateCategory(Category category)
	{
		if (!this.CheckCategory(category))
		{
			return;
		}
		if (this._selectedCategory != category)
		{
			int num = 0;
			int num2 = 0;
			foreach (EntityData entity in Inventory._entities[category])
			{
				if (Singleton<Research>.Instance.IsEntityUnlocked(entity))
				{
					num++;
				}
				num2++;
			}
			this._categories[category].unlocked.text = num.ToString() + " / " + num2.ToString() + " UNLOCKED";
			return;
		}
		this.SelectCategory((int)category);
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0002E6CC File Offset: 0x0002C8CC
	private void ResetCategory()
	{
		if (this._categories.ContainsKey(this._selectedCategory))
		{
			this._categories[this._selectedCategory].ResetImageColor();
		}
		if (this._inventoryRows.Count > 0)
		{
			for (int i = this._activeRowIndex; i >= 0; i--)
			{
				if (i < this._inventoryRows.Count)
				{
					this._inventoryRows[i].ResetButtons();
					this._inventoryRows[i].gameObject.SetActive(false);
				}
				else
				{
					Debug.Log("[INVENTORY] Mismatch with active row index " + this._activeRowIndex.ToString());
					i = this._inventoryRows.Count - 1;
				}
			}
		}
		this._activeRowIndex = 0;
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0002E788 File Offset: 0x0002C988
	public override void Open()
	{
		if (!Singleton<Interface>.Instance.CanOpenUI)
		{
			return;
		}
		if (this._firstOpen)
		{
			this._firstOpen = false;
			this.SelectCategory(1);
		}
		base.SetIsOpen(true);
		Singleton<Interface>.Instance.SetCurrentlyOpen(this);
		if (this.canvasGroup.interactable)
		{
			return;
		}
		float num = 0f;
		foreach (MenuButton menuButton in this.menuButtons)
		{
			menuButton.group.GetComponent<RectTransform>().localPosition = menuButton.inPos;
			menuButton.group.alpha = 0f;
			LeanTween.alphaCanvas(menuButton.group, 1f, this.buttonAnimationSpeed).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, this.buttonAnimationSpeed).setEase(LeanTweenType.easeOutExpo).setDelay(num += this.buttonAlphaCooldown);
		}
		Inventory.IsOpen = true;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		Singleton<Events>.Instance.onInventoryOpen.Invoke();
		if (this.openSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.openSound);
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0002E8F0 File Offset: 0x0002CAF0
	public override void ForceOpen()
	{
		base.SetIsOpen(true);
		foreach (MenuButton menuButton in this.menuButtons)
		{
			menuButton.group.alpha = 1f;
			menuButton.group.transform.localPosition = menuButton.normalPos;
		}
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		Inventory.IsOpen = true;
		Singleton<Events>.Instance.onInventoryOpen.Invoke();
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0002E99C File Offset: 0x0002CB9C
	public override void Close()
	{
		base.SetIsOpen(false);
		this.panel.entityPreview.TogglePreviewCamera(false);
		Singleton<Interface>.Instance.SetCurrentlyOpen(null);
		if (!this.canvasGroup.interactable)
		{
			return;
		}
		float num = 0f;
		foreach (MenuButton menuButton in this.menuButtons)
		{
			LeanTween.alphaCanvas(menuButton.group, 0f, this.buttonAnimationSpeed).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.outPos, this.buttonAnimationSpeed).setEase(LeanTweenType.easeOutExpo).setDelay(num += this.buttonAlphaCooldown);
		}
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		Inventory.IsOpen = false;
		Singleton<Events>.Instance.onInventoryClose.Invoke();
		if (this.closeSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.closeSound);
		}
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0002EAC0 File Offset: 0x0002CCC0
	public override void ForceClose()
	{
		base.SetIsOpen(false);
		this.panel.entityPreview.TogglePreviewCamera(false);
		foreach (MenuButton menuButton in this.menuButtons)
		{
			if (LeanTween.isTweening(menuButton.group.gameObject))
			{
				LeanTween.cancel(menuButton.group.gameObject);
			}
			menuButton.group.alpha = 0f;
			menuButton.group.transform.localPosition = menuButton.outPos;
		}
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		Inventory.IsOpen = false;
		Singleton<Events>.Instance.onInventoryClose.Invoke();
	}

	// Token: 0x0400070D RID: 1805
	public new static bool IsOpen = false;

	// Token: 0x0400070E RID: 1806
	public InventoryPanel panel;

	// Token: 0x0400070F RID: 1807
	protected static Dictionary<Category, List<EntityData>> _entities = new Dictionary<Category, List<EntityData>>();

	// Token: 0x04000710 RID: 1808
	public InventoryRow inventoryRowPrefab;

	// Token: 0x04000711 RID: 1809
	public Transform inventoryRowList;

	// Token: 0x04000712 RID: 1810
	protected List<InventoryRow> _inventoryRows = new List<InventoryRow>();

	// Token: 0x04000713 RID: 1811
	[SerializeField]
	private int _activeRowIndex;

	// Token: 0x04000714 RID: 1812
	public CategoryButton categoryButtonPrefab;

	// Token: 0x04000715 RID: 1813
	public Transform categoryButtonList;

	// Token: 0x04000716 RID: 1814
	public List<Inventory.CategoryButtonData> categoryButtons;

	// Token: 0x04000717 RID: 1815
	private Dictionary<Category, Inventory.CategoryButtonData> _categoryLibrary;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private Dictionary<Category, CategoryButton> _categories;

	// Token: 0x04000719 RID: 1817
	public Color selectedColor;

	// Token: 0x0400071A RID: 1818
	public Color normalColorOne;

	// Token: 0x0400071B RID: 1819
	public Color normalColorTwo;

	// Token: 0x0400071C RID: 1820
	protected Category _selectedCategory;

	// Token: 0x0400071D RID: 1821
	public List<MenuButton> menuButtons;

	// Token: 0x0400071E RID: 1822
	public float buttonAnimationSpeed;

	// Token: 0x0400071F RID: 1823
	public float buttonAlphaCooldown;

	// Token: 0x04000720 RID: 1824
	private bool _firstOpen = true;

	// Token: 0x04000721 RID: 1825
	private bool _alternate = true;

	// Token: 0x0200014E RID: 334
	[Serializable]
	public class CategoryButtonData
	{
		// Token: 0x04000722 RID: 1826
		public Category category;

		// Token: 0x04000723 RID: 1827
		public Sprite icon;

		// Token: 0x04000724 RID: 1828
		public string title;

		// Token: 0x04000725 RID: 1829
		public Vector2 size = Vector2.one;
	}
}
