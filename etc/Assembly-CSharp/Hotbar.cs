using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000182 RID: 386
[DefaultExecutionOrder(1)]
public class Hotbar : Singleton<Hotbar>
{
	// Token: 0x06000CB9 RID: 3257 RVA: 0x00037B69 File Offset: 0x00035D69
	public List<List<HotbarData>> RequestHotbar()
	{
		return this._hotbars;
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00037B74 File Offset: 0x00035D74
	public override void Awake()
	{
		base.Awake();
		this._hotbars = new List<List<HotbarData>>();
		for (int i = 0; i <= 11; i++)
		{
			this._hotbars.Add(new List<HotbarData>());
			for (int j = 0; j < this._elements.Count; j++)
			{
				this._hotbars[i].Add(new HotbarData());
			}
		}
		this.SetHotbar(this._selectedHotbar);
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00037BE8 File Offset: 0x00035DE8
	public void Setup()
	{
		InputManager.OnHotbarNumberChanged.AddListener(new UnityAction<int>(this.OnHotbarInput));
		InputManager.OnFunctionNumberChanged.AddListener(new UnityAction<int>(this.OnFunctionKeyPressed));
		if (!Singleton<Gamemode>.Instance.UseResources)
		{
			for (int i = 0; i < this._elements.Count; i++)
			{
				this._elements[i].ToggleCost(false);
			}
		}
		this.researchButton.SetActive(Singleton<Gamemode>.Instance.UseResearch);
		this.factionButton.SetActive(Singleton<Gamemode>.Instance.AllowFactionSwitching);
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x00037C80 File Offset: 0x00035E80
	public void SetupDefaultEntities()
	{
		int num = 0;
		foreach (string text in this.defaultEntities)
		{
			if (Library.RequestData<EntityData>(text) != null)
			{
				this._elements[num].SetEntity(text);
				num++;
			}
		}
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x00037CF4 File Offset: 0x00035EF4
	public void SetHotbar(SaveData.Hotbar hotbar)
	{
		if (hotbar.hotbars != null)
		{
			this._hotbars = hotbar.hotbars;
			this.SetHotbar(Math.Clamp(hotbar.SelectedHotbar, 0, 11));
		}
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x00037D20 File Offset: 0x00035F20
	private void OnHotbarInput(int number)
	{
		int num = number - 1;
		if (num == -1)
		{
			num = 9;
		}
		if (num >= 0 && num < this._elements.Count)
		{
			this._elements[num].Use();
			return;
		}
		Debug.Log("[HOTBAR] Invalid number input " + number.ToString());
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x00037D72 File Offset: 0x00035F72
	public void OnSetHotbarElement(int index, string entityID)
	{
		index = Math.Clamp(index, 0, 11);
		this._hotbars[this._selectedHotbar][index].entityID = entityID;
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x00037D9C File Offset: 0x00035F9C
	public void OnAltAndScrollPressed(float value)
	{
		this.ChangeHotbarNumber((value > 0f) ? 1 : -1);
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00037DB0 File Offset: 0x00035FB0
	public void ChangeHotbarNumber(int value)
	{
		this.SetHotbar(Math.Clamp(this._selectedHotbar + value, 0, 11));
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x00037DC8 File Offset: 0x00035FC8
	public void OnFunctionKeyPressed(int value)
	{
		this.SetHotbar(value - 1);
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x00037DD4 File Offset: 0x00035FD4
	private void SetHotbar(int number)
	{
		this._selectedHotbar = number;
		int num = 0;
		while (num < this._hotbars[this._selectedHotbar].Count && num < this._elements.Count)
		{
			HotbarData hotbarData = this._hotbars[this._selectedHotbar][num];
			this._elements[num].SetEntity(hotbarData.entityID);
			num++;
		}
		this.hotbarNumber.text = this._selectedHotbar.ToString();
		this.upArrow.color = ((this._selectedHotbar == 11) ? this.invalidColor : this.validColor);
		this.downArrow.color = ((this._selectedHotbar == 0) ? this.invalidColor : this.validColor);
	}

	// Token: 0x0400089C RID: 2204
	public GameObject researchButton;

	// Token: 0x0400089D RID: 2205
	public GameObject factionButton;

	// Token: 0x0400089E RID: 2206
	public TextMeshProUGUI hotbarNumber;

	// Token: 0x0400089F RID: 2207
	public Image upArrow;

	// Token: 0x040008A0 RID: 2208
	public Image downArrow;

	// Token: 0x040008A1 RID: 2209
	public Color validColor;

	// Token: 0x040008A2 RID: 2210
	public Color invalidColor;

	// Token: 0x040008A3 RID: 2211
	public List<string> defaultEntities;

	// Token: 0x040008A4 RID: 2212
	private const int MAX_HOTBARS = 11;

	// Token: 0x040008A5 RID: 2213
	private List<List<HotbarData>> _hotbars = new List<List<HotbarData>>();

	// Token: 0x040008A6 RID: 2214
	private int _selectedHotbar;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	protected List<HotbarElement> _elements;
}
