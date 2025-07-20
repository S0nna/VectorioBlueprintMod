using System;
using System.Collections.Generic;
using UnityEngine.Events;
using Vectorio.Entities;
using Vectorio.Stats;

// Token: 0x02000101 RID: 257
public class Crafter : ResourceComponent, ICallbackListener, IComponent<Crafter, CrafterData>
{
	// Token: 0x06000846 RID: 2118 RVA: 0x00024965 File Offset: 0x00022B65
	public CrafterData GetData()
	{
		return this._crafterData;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0002496D File Offset: 0x00022B6D
	public List<RecipeData> GetAvailableRecipes()
	{
		return this._availableRecipes;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00024975 File Offset: 0x00022B75
	public void OnInitialize(CrafterData data)
	{
		this._crafterData = data;
		this._availableRecipes = data.recipes;
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0002498C File Offset: 0x00022B8C
	public override void OnSpawn(bool fromSave)
	{
		if (base.Entity.IsPlayerFaction)
		{
			Singleton<Events>.Instance.onRecipeUnlocked.AddListener(new UnityAction<RecipeData>(this.OnRecipeUnlocked));
		}
		Singleton<StatManager>.Instance.CreateStatFloat(ref this._multiplier, StatType.CraftingSpeed, 1f, this);
		this.CreateContainers();
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x000249E0 File Offset: 0x00022BE0
	protected override void CreateContainers()
	{
		if (this._crafterData.useIndicator)
		{
			base.CreateIndicator(ContainerType.Output, this._crafterData.indicatorPosition.x, this._crafterData.indicatorPosition.y);
		}
		base.CreateContainer(ContainerType.Input, StorageMode.LocalizedStorage, this._crafterData.storage, StatType.CollectorCapacity, ContainerFlags.None);
		base.CreateContainer(ContainerType.Output, StorageMode.LocalizedStorage, this._crafterData.storage, StatType.CollectorCapacity, ContainerFlags.Xray | ContainerFlags.GeneratesResources);
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00024A4D File Offset: 0x00022C4D
	public override void OnAddResource(ContainerType type, ResourceData resource, int amount)
	{
		if (type == ContainerType.Input)
		{
			this.CheckForRecipe();
		}
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00024A58 File Offset: 0x00022C58
	public override void OnTakeResource(ContainerType type, ResourceData resource, int requestedAmount)
	{
		this.CheckForRecipe();
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00024A60 File Offset: 0x00022C60
	private void OnRecipeUnlocked(RecipeData recipe)
	{
		if (!base.IsUpdating && this._availableRecipes.Contains(recipe))
		{
			this.CheckForRecipe();
		}
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00024A80 File Offset: 0x00022C80
	protected void CheckForRecipe()
	{
		if (base.IsUpdating || !NetworkPlayerManager.IS_HOST)
		{
			return;
		}
		ResourceContainer inputContainer = this._resourceModule.GetInputContainer();
		if (inputContainer == null)
		{
			return;
		}
		foreach (RecipeData recipeData in this._availableRecipes)
		{
			if (Singleton<Research>.Instance.IsRecipeUnlocked(recipeData) && !this._resourceModule.GetOutputContainer().IsFull(recipeData.outputResource))
			{
				bool flag = true;
				foreach (ResourceData resource in recipeData.inputResources)
				{
					if (!inputContainer.HasResourceAvailable(resource))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					if (DevTools.INSTANT_CRAFT)
					{
						Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, 0f, new VariableContainer(0, recipeData.ID)));
						break;
					}
					Singleton<EntityManager>.Instance.QueueEntityCallback(EventBuilder.BuildCallbackEvent(base.Entity, base.ComponentIndex, recipeData.time * this._multiplier.Value, new VariableContainer(0, recipeData.ID)));
					break;
				}
			}
		}
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartCallback(EntityCallbackEvent callback)
	{
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00024BE0 File Offset: 0x00022DE0
	public void OnEndCallback(EntityCallbackEvent callback)
	{
		string id;
		if (callback.Variable != null && callback.Variable.TryGetString(0, out id))
		{
			RecipeData recipeData = Library.RequestData<RecipeData>(id);
			if (recipeData != null)
			{
				foreach (ResourceData resource in recipeData.inputResources)
				{
					this._resourceModule.TakeResource(ContainerType.Input, resource, 1);
				}
				this._resourceModule.Indicator.SetStatus(Indicator.Status.Active);
				this._resourceModule.AddResource(ContainerType.Output, recipeData.outputResource, 1);
			}
		}
		this.CheckForRecipe();
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00024C94 File Offset: 0x00022E94
	public override void OnReset()
	{
		if (base.Entity.IsPlayerFaction)
		{
			Singleton<Events>.Instance.onRecipeUnlocked.RemoveListener(new UnityAction<RecipeData>(this.OnRecipeUnlocked));
		}
		base.OnReset();
	}

	// Token: 0x04000566 RID: 1382
	private CrafterData _crafterData;

	// Token: 0x04000567 RID: 1383
	protected List<RecipeData> _availableRecipes;

	// Token: 0x04000568 RID: 1384
	protected StatFloat _multiplier;
}
