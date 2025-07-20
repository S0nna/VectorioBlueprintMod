using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015D RID: 349
public class UI_ResourceTechRequirement : MonoBehaviour
{
	// Token: 0x06000B58 RID: 2904 RVA: 0x000318E4 File Offset: 0x0002FAE4
	public void Set(string name, Sprite sprite, int amount, int maxValue = -1)
	{
		this.icon.sprite = sprite;
		this.UpdateAmount(amount);
		if (this.nameText != null)
		{
			this.nameText.text = name;
		}
		if (maxValue != -1 && this.progressBar != null)
		{
			this.progressBar.maxValue = (float)maxValue;
			this.UpdateUI(amount);
		}
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00031946 File Offset: 0x0002FB46
	public void UpdateAmount(int amount)
	{
		this.amount += amount;
		if (this.amountText != null)
		{
			this.amountText.text = this.amount.ToString() + this.amountSuffix;
		}
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x00031985 File Offset: 0x0002FB85
	public void SetAmount(int amount)
	{
		this.amount = amount;
		if (this.amountText != null)
		{
			this.amountText.text = this.amount.ToString() + this.amountSuffix;
		}
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x000319BD File Offset: 0x0002FBBD
	public void SetSuffix(string newSuffix)
	{
		this.amountSuffix = newSuffix;
		this.UpdateUI(this.amount);
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x000319D4 File Offset: 0x0002FBD4
	public void UpdateUI(int amount)
	{
		if ((float)amount <= 0f)
		{
			if (this.completeText != "")
			{
				this.amountText.text = this.completeText;
			}
			else
			{
				this.amountText.text = amount.ToString() + this.amountSuffix;
			}
			this.amountText.color = this.completeColor;
			if (this.progressBar != null)
			{
				this.progressBar.currentPercent = (float)amount;
				this.progressBar.UpdateUI();
				return;
			}
		}
		else
		{
			this.amountText.text = amount.ToString() + this.amountSuffix;
			this.amountText.color = this.incompleteColor;
			if (this.progressBar != null)
			{
				this.progressBar.currentPercent = (float)amount;
				this.progressBar.UpdateUI();
			}
		}
	}

	// Token: 0x040007DD RID: 2013
	public Image icon;

	// Token: 0x040007DE RID: 2014
	public TextMeshProUGUI nameText;

	// Token: 0x040007DF RID: 2015
	public TextMeshProUGUI amountText;

	// Token: 0x040007E0 RID: 2016
	public ProgressBar progressBar;

	// Token: 0x040007E1 RID: 2017
	public int amount;

	// Token: 0x040007E2 RID: 2018
	public Color completeColor;

	// Token: 0x040007E3 RID: 2019
	public Color incompleteColor;

	// Token: 0x040007E4 RID: 2020
	public string completeText = "COMPLETE";

	// Token: 0x040007E5 RID: 2021
	public string amountSuffix = " REMAINING";
}
