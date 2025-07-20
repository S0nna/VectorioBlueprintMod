using System;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.Formatting;

// Token: 0x020001E3 RID: 483
public class RecipeInfo : MonoBehaviour
{
	// Token: 0x06000EE8 RID: 3816 RVA: 0x00044E00 File Offset: 0x00043000
	public void Set(Crafter target, RecipeData recipe)
	{
		this._target = target;
		this._recipe = recipe;
		if (!Singleton<Research>.Instance.IsRecipeUnlocked(this._recipe))
		{
			this.unlockedObj.SetActive(false);
			this.lockedObj.SetActive(true);
			return;
		}
		this.unlockedObj.SetActive(true);
		this.lockedObj.SetActive(false);
		this.recipeIcon.sprite = recipe.outputResource.IconSprite;
		this.title.text = recipe.outputResource.Name.ToUpper();
		this.tier.text = "TIER " + recipe.outputResource.Tier.ToString() + " RECIPE";
		bool flag = true;
		int num = 0;
		while (num < recipe.inputResources.Count && num != this.recipeInputs.Count)
		{
			this.recipeInputs[num].sprite = recipe.inputResources[num].IconSprite;
			this.recipeInputs[num].gameObject.SetActive(true);
			if (!flag && num - 1 < this.plusSigns.Count)
			{
				this.plusSigns[num - 1].gameObject.SetActive(true);
			}
			flag = false;
			num++;
		}
		this.time.text = Formatter.Round(recipe.time, 1) + "s";
		this.tier.color = recipe.outputResource.Accent.secondaryColor;
		this.tierBackground.color = recipe.outputResource.Accent.primaryColor;
		this.barImage.color = recipe.outputResource.Accent.primaryColor;
		this.barBackground.color = recipe.outputResource.Accent.secondaryColor;
		this.progressBar.minValue = 0f;
		this.progressBar.currentPercent = this._recipe.time;
		this.progressBar.maxValue = this._recipe.time;
		this.progressBar.UpdateUI();
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x00003212 File Offset: 0x00001412
	public void CustomUpdate()
	{
	}

	// Token: 0x04000C0B RID: 3083
	public Image recipeIcon;

	// Token: 0x04000C0C RID: 3084
	public Image barImage;

	// Token: 0x04000C0D RID: 3085
	public Image barBackground;

	// Token: 0x04000C0E RID: 3086
	public Image tierBackground;

	// Token: 0x04000C0F RID: 3087
	public List<Image> recipeInputs;

	// Token: 0x04000C10 RID: 3088
	public List<Image> plusSigns;

	// Token: 0x04000C11 RID: 3089
	public ProgressBar progressBar;

	// Token: 0x04000C12 RID: 3090
	public TextMeshProUGUI title;

	// Token: 0x04000C13 RID: 3091
	public TextMeshProUGUI tier;

	// Token: 0x04000C14 RID: 3092
	public TextMeshProUGUI time;

	// Token: 0x04000C15 RID: 3093
	public GameObject unlockedObj;

	// Token: 0x04000C16 RID: 3094
	public GameObject lockedObj;

	// Token: 0x04000C17 RID: 3095
	private Crafter _target;

	// Token: 0x04000C18 RID: 3096
	private RecipeData _recipe;
}
