using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001E2 RID: 482
public class RecipeButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000EE6 RID: 3814 RVA: 0x00044CAC File Offset: 0x00042EAC
	public void SetRecipe(Crafter crafter, RecipeData newRecipe, bool useColor = true)
	{
		this._recipe = newRecipe;
		this._crafter = crafter;
		this.recipeName.text = newRecipe.outputResource.Name.ToUpper();
		this.recipeTier.text = "TIER " + newRecipe.outputResource.Tier.ToString();
		this.recipeTime.text = " " + ((int)newRecipe.time).ToString() + "s";
		this.background.color = (useColor ? this.normalColor : this.darkerColor);
		this.outputIcon.sprite = newRecipe.outputResource.IconSprite;
		this.tierBackground.color = newRecipe.outputResource.Accent.secondaryColor;
		this.recipeTier.color = newRecipe.outputResource.Accent.primaryColor;
		try
		{
			this.inputOneIcon.sprite = newRecipe.inputResources[0].IconSprite;
			this.inputTwoIcon.sprite = newRecipe.inputResources[1].IconSprite;
		}
		catch
		{
			Debug.Log("[RECIPE BUTTON] Invalid inputs setup for recipe " + newRecipe.Name);
		}
	}

	// Token: 0x04000BFF RID: 3071
	public Image background;

	// Token: 0x04000C00 RID: 3072
	public Image outputIcon;

	// Token: 0x04000C01 RID: 3073
	public Image inputOneIcon;

	// Token: 0x04000C02 RID: 3074
	public Image inputTwoIcon;

	// Token: 0x04000C03 RID: 3075
	public Image tierBackground;

	// Token: 0x04000C04 RID: 3076
	public TextMeshProUGUI recipeName;

	// Token: 0x04000C05 RID: 3077
	public TextMeshProUGUI recipeTier;

	// Token: 0x04000C06 RID: 3078
	public TextMeshProUGUI recipeTime;

	// Token: 0x04000C07 RID: 3079
	public Color normalColor;

	// Token: 0x04000C08 RID: 3080
	public Color darkerColor;

	// Token: 0x04000C09 RID: 3081
	protected RecipeData _recipe;

	// Token: 0x04000C0A RID: 3082
	protected Crafter _crafter;
}
