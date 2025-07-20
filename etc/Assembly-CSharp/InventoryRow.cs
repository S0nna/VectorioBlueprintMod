using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class InventoryRow : MonoBehaviour
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x0002F290 File Offset: 0x0002D490
	public bool HasAvailableButton()
	{
		for (int i = 0; i < this.buttons.Count; i++)
		{
			if (!this.buttons[i].IsButtonSet)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0002F2CC File Offset: 0x0002D4CC
	public void LinkEntity(EntityData data, Inventory inventory, bool unlocked)
	{
		InventoryButton inventoryButton = this.buttons[this._index];
		if (inventoryButton != null)
		{
			inventoryButton.gameObject.SetActive(true);
			inventoryButton.LinkInventory(inventory);
			inventoryButton.Set(data, "default", "faction_player", "", unlocked);
		}
		this._index++;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0002F32C File Offset: 0x0002D52C
	public void ResetButtons()
	{
		for (int i = 0; i < this.buttons.Count; i++)
		{
			this.buttons[i].ResetButtonState();
		}
		this._index = 0;
	}

	// Token: 0x04000750 RID: 1872
	public List<InventoryButton> buttons;

	// Token: 0x04000751 RID: 1873
	private int _index;
}
