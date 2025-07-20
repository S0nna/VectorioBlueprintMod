using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Stats;
using Vectorio.Utilities;

// Token: 0x020000B1 RID: 177
[CreateAssetMenu(fileName = "New Entity", menuName = "Vectorio/Entity")]
public class EntityData : BaseData
{
	// Token: 0x06000683 RID: 1667 RVA: 0x0001FB58 File Offset: 0x0001DD58
	public void Setup(ref Entity entity, bool fromSave)
	{
		ModelObject getModel = entity.GetModel;
		if (getModel != null)
		{
			getModel.ConnectRenderer(entity);
		}
		if (!entity.AreComponentsCreated)
		{
			entity.OnInitialize(this);
			foreach (IComponentData componentData in this.components)
			{
				componentData.CreateAndAddComponent(entity);
			}
			entity.AreComponentsCreated = true;
		}
		entity.OnSpawn();
		foreach (EntityComponent entityComponent in entity.GetComponentValues())
		{
			entityComponent.OnSpawn(fromSave);
		}
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x0001FC20 File Offset: 0x0001DE20
	public void SetupAsBlueprint(ref Entity entity, EntityCreationData creationData)
	{
		if (!this.HasComponent<BuildingData>())
		{
			Debug.Log("[ENTITY DATA] This entity does not have a building component!");
			return;
		}
		if (!entity.AreComponentsCreated)
		{
			entity.OnInitialize(this);
			entity.Add_EComponent<Building>().OnInitialize(this.GetComponent<BuildingData>());
			entity.Add_EComponent<Blueprint>();
			entity.AreComponentsCreated = true;
		}
		entity.OnSpawn();
		foreach (EntityComponent entityComponent in entity.GetComponentValues())
		{
			entityComponent.OnSpawn(SaveSystem.IS_LOADING);
		}
		entity.Get_EComponent<Blueprint>(false).SetCreationData(creationData);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0001FCD4 File Offset: 0x0001DED4
	public bool HasComponent<T>() where T : IComponentData
	{
		return this.components.Any((IComponentData c) => c is T);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0001FD00 File Offset: 0x0001DF00
	public T GetComponent<T>() where T : IComponentData
	{
		return this.components.OfType<T>().FirstOrDefault<T>();
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00003212 File Offset: 0x00001412
	public void BakeModel()
	{
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x0001FD14 File Offset: 0x0001DF14
	public List<Stat> RetrieveStats()
	{
		List<Stat> list = new List<Stat>();
		foreach (IComponentData componentData in this.components)
		{
			list.AddRange(componentData.RetrieveStats());
		}
		return list;
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001FD74 File Offset: 0x0001DF74
	public bool UseNormalCost
	{
		get
		{
			return this.useNormalCost;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001FD7C File Offset: 0x0001DF7C
	public Cost NormalCost
	{
		get
		{
			return this.normalCost;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001FD84 File Offset: 0x0001DF84
	public bool UseSpecialCost
	{
		get
		{
			return this.useSpecialCost;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x0600068C RID: 1676 RVA: 0x0001FD8C File Offset: 0x0001DF8C
	public Cost SpecialCost
	{
		get
		{
			return this.specialCost;
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0001FD94 File Offset: 0x0001DF94
	public Model GetModel(string id)
	{
		if (id == "")
		{
			return this.model;
		}
		if (this.cosmetics.ContainsKey(id))
		{
			return this.cosmetics[id];
		}
		return this.model;
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0001FDCB File Offset: 0x0001DFCB
	protected string GetID(string var, Type type)
	{
		return Utilities.GetVariableID(var, type);
	}

	// Token: 0x040003F3 RID: 1011
	[SerializeReference]
	public List<IComponentData> components;

	// Token: 0x040003F4 RID: 1012
	public Texture2D preview;

	// Token: 0x040003F5 RID: 1013
	public Category category;

	// Token: 0x040003F6 RID: 1014
	public int inventoryIndex = -1;

	// Token: 0x040003F7 RID: 1015
	public string sortingLayer = "building";

	// Token: 0x040003F8 RID: 1016
	[Range(10f, 25f)]
	public float previewSize = 5f;

	// Token: 0x040003F9 RID: 1017
	public Model model = new Model("default", new List<SpriteLayer>());

	// Token: 0x040003FA RID: 1018
	public Dictionary<string, Model> cosmetics = new Dictionary<string, Model>();

	// Token: 0x040003FB RID: 1019
	public bool useAdvancedSettings;

	// Token: 0x040003FC RID: 1020
	public CheckType checkType;

	// Token: 0x040003FD RID: 1021
	public bool useBuilderDrones = true;

	// Token: 0x040003FE RID: 1022
	public bool broadcastCreation = true;

	// Token: 0x040003FF RID: 1023
	public bool usePlacementAnimation = true;

	// Token: 0x04000400 RID: 1024
	public bool usePlacementSound = true;

	// Token: 0x04000401 RID: 1025
	public bool useCustomSound;

	// Token: 0x04000402 RID: 1026
	public AudioClip placeSound;

	// Token: 0x04000403 RID: 1027
	[SerializeField]
	private bool useNormalCost;

	// Token: 0x04000404 RID: 1028
	[SerializeField]
	private Cost normalCost;

	// Token: 0x04000405 RID: 1029
	[SerializeField]
	private bool useSpecialCost;

	// Token: 0x04000406 RID: 1030
	[SerializeField]
	private Cost specialCost;
}
