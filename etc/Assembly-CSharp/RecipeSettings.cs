using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class RecipeSettings : EntitySettings
{
	// Token: 0x06000795 RID: 1941 RVA: 0x00021E2C File Offset: 0x0002002C
	public override void Set(EntityComponent component)
	{
		Crafter crafter = component as Crafter;
		if (crafter != null)
		{
			this._target = crafter;
			using (List<RecipeData>.Enumerator enumerator = this._target.GetAvailableRecipes().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RecipeData recipe = enumerator.Current;
					RecipeInfo recipeInfo = Object.Instantiate<RecipeInfo>(this.infoPrefab);
					recipeInfo.transform.SetParent(this.infoList);
					recipeInfo.transform.localScale = Vector2.one;
					recipeInfo.Set(this._target, recipe);
					this._recipes.Add(recipeInfo);
				}
				return;
			}
		}
		Debug.LogWarning("The provided component does not match the setting type.");
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00021EE8 File Offset: 0x000200E8
	public override void CustomUpdate()
	{
		foreach (RecipeInfo recipeInfo in this._recipes)
		{
			recipeInfo.CustomUpdate();
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00003212 File Offset: 0x00001412
	public override void Clear()
	{
	}

	// Token: 0x04000504 RID: 1284
	private Crafter _target;

	// Token: 0x04000505 RID: 1285
	public RecipeInfo infoPrefab;

	// Token: 0x04000506 RID: 1286
	public Transform infoList;

	// Token: 0x04000507 RID: 1287
	private List<RecipeInfo> _recipes = new List<RecipeInfo>();
}
