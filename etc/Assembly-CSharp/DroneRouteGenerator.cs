using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200005F RID: 95
[DefaultExecutionOrder(0)]
public class DroneRouteGenerator : Singleton<DroneRouteGenerator>
{
	// Token: 0x0600048D RID: 1165 RVA: 0x00018064 File Offset: 0x00016264
	public bool ScheduleRouteGeneration(int maxActions, int maxStorage, IEnumerable<Entity> dropoffTargets, IEnumerable<Entity> pickupTargets, MetadataContext ctx, ResourceData filter, out DroneActionPackage actionPackage)
	{
		actionPackage = null;
		ResourceData resourceData;
		if (filter == null)
		{
			StoredResource storedResource = this.FindBestResource(pickupTargets);
			if (storedResource == null)
			{
				return false;
			}
			resourceData = storedResource.ResourceData;
		}
		else
		{
			resourceData = filter;
		}
		List<ResourceAction> list = this.GenerateDropoffs(maxActions, dropoffTargets, resourceData, maxStorage).ToList<ResourceAction>();
		int b = list.Sum((ResourceAction d) => d.Amount);
		int maxStorage2 = Mathf.Min(maxStorage, b);
		List<ResourceAction> list2 = this.GeneratePickups(maxActions, pickupTargets, resourceData, maxStorage2).ToList<ResourceAction>();
		int totalPickupAmount = list2.Sum((ResourceAction p) => p.Amount);
		List<ResourceAction> list3 = this.AdjustDropoffs(list, totalPickupAmount).ToList<ResourceAction>();
		bool flag = list3.Any<ResourceAction>();
		bool flag2 = list2.Any<ResourceAction>();
		if (flag && flag2)
		{
			actionPackage = new DroneActionPackage(new Queue<ResourceAction>(list2.Concat(list3)), resourceData, ctx);
			return true;
		}
		return false;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00018158 File Offset: 0x00016358
	private StoredResource FindBestResource(IEnumerable<Entity> targets)
	{
		return (from resource in targets.Select(delegate(Entity entity)
		{
			ResourceModule resourceModule = entity.Get_EComponent<ResourceModule>(false);
			ResourceContainer resourceContainer = (resourceModule != null) ? resourceModule.GetOutputContainer() : null;
			if (resourceContainer == null)
			{
				return null;
			}
			return resourceContainer.GetBiggestResource(null);
		})
		where resource != null && resource.AmountAfterPending > 0
		orderby resource.AmountAfterPending descending
		select resource).FirstOrDefault<StoredResource>();
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x000181D7 File Offset: 0x000163D7
	private IEnumerable<ResourceAction> GenerateDropoffs(int maxActions, IEnumerable<Entity> targets, ResourceData filter, int maxStorage)
	{
		int actionsGenerated = 0;
		int storageUsed = 0;
		Func<Entity, <>f__AnonymousType0<ResourceModule, int, bool>> <>9__0;
		var selector;
		if ((selector = <>9__0) == null)
		{
			selector = (<>9__0 = delegate(Entity entity)
			{
				ResourceModule resourceModule = entity.Get_EComponent<ResourceModule>(false);
				ResourceContainer resourceContainer = (resourceModule != null) ? resourceModule.GetInputContainer() : null;
				int space2 = (resourceContainer != null) ? resourceContainer.GetSpace(filter) : 0;
				bool hasPendingAction = resourceContainer != null && resourceContainer.HasPendingAction(filter);
				return new
				{
					ResourceModule = resourceModule,
					Space = space2,
					HasPendingAction = hasPendingAction
				};
			});
		}
		foreach (var <>f__AnonymousType in from item in targets.Select(selector)
		where item.ResourceModule != null && item.Space > 0 && !item.HasPendingAction
		orderby item.Space descending
		select item)
		{
			if (actionsGenerated >= maxActions || storageUsed >= maxStorage)
			{
				break;
			}
			int space = Mathf.Min(<>f__AnonymousType.Space, maxStorage - storageUsed);
			yield return new ResourceAction(<>f__AnonymousType.ResourceModule, filter, space, false);
			int num = actionsGenerated;
			actionsGenerated = num + 1;
			storageUsed += space;
		}
		var enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x000181FD File Offset: 0x000163FD
	private IEnumerable<ResourceAction> GeneratePickups(int maxActions, IEnumerable<Entity> targets, ResourceData filter, int maxStorage)
	{
		int actionsGenerated = 0;
		int storageUsed = 0;
		Func<Entity, <>f__AnonymousType1<ResourceModule, int, bool>> <>9__0;
		var selector;
		if ((selector = <>9__0) == null)
		{
			selector = (<>9__0 = delegate(Entity entity)
			{
				ResourceModule resourceModule = entity.Get_EComponent<ResourceModule>(false);
				ResourceContainer resourceContainer = (resourceModule != null) ? resourceModule.GetOutputContainer() : null;
				StoredResource storedResource = (resourceContainer != null) ? resourceContainer.GetBiggestResource(filter) : null;
				int amount2 = (storedResource != null) ? storedResource.AmountStored : 0;
				bool hasPendingAction = resourceContainer != null && resourceContainer.HasPendingAction(filter);
				return new
				{
					ResourceModule = resourceModule,
					Amount = amount2,
					HasPendingAction = hasPendingAction
				};
			});
		}
		foreach (var <>f__AnonymousType in from item in targets.Select(selector)
		where item.ResourceModule != null && item.Amount > 0 && !item.HasPendingAction
		orderby item.Amount descending
		select item)
		{
			if (actionsGenerated >= maxActions || storageUsed >= maxStorage)
			{
				break;
			}
			int amount = Mathf.Min(<>f__AnonymousType.Amount, maxStorage - storageUsed);
			yield return new ResourceAction(<>f__AnonymousType.ResourceModule, filter, amount, true);
			int num = actionsGenerated;
			actionsGenerated = num + 1;
			storageUsed += amount;
		}
		var enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00018223 File Offset: 0x00016423
	private IEnumerable<ResourceAction> AdjustDropoffs(List<ResourceAction> potentialDropoffs, int totalPickupAmount)
	{
		int remainingAmount = totalPickupAmount;
		int adjustedDropoffs = 0;
		foreach (ResourceAction resourceAction in potentialDropoffs)
		{
			if (remainingAmount <= 0)
			{
				break;
			}
			int num = Mathf.Min(resourceAction.Amount, remainingAmount);
			remainingAmount -= num;
			yield return new ResourceAction(resourceAction.Target, resourceAction.Resource, num, false);
			int num2 = adjustedDropoffs;
			adjustedDropoffs = num2 + 1;
		}
		List<ResourceAction>.Enumerator enumerator = default(List<ResourceAction>.Enumerator);
		yield break;
		yield break;
	}
}
