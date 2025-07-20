using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000183 RID: 387
public class HotbarElement : MonoBehaviour
{
	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00037EB3 File Offset: 0x000360B3
	public EntityData EntityData
	{
		get
		{
			return this._entity;
		}
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x00037EBB File Offset: 0x000360BB
	public void ToggleCost(bool toggle)
	{
		this.resourceObj.SetActive(toggle);
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x00037ECC File Offset: 0x000360CC
	public void SetEntity(EntityData entity)
	{
		if (this._model != null)
		{
			Object.Destroy(this._model);
		}
		this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(entity.model, new Vector2(25f, 25f), base.transform, null);
		this._model.transform.localScale = Vector2.one;
		this._model.transform.SetSiblingIndex(1);
		this._entity = entity;
		if (entity.NormalCost.resource != null)
		{
			this.resourceIcon.sprite = entity.NormalCost.resource.IconSprite;
			this.resourceAmount.text = entity.NormalCost.amount.ToString();
			this.resourceObj.SetActive(Singleton<Gamemode>.Instance.UseResources);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.resourceObj.GetComponent<RectTransform>());
		}
		else
		{
			this.resourceObj.SetActive(false);
		}
		Singleton<Hotbar>.Instance.OnSetHotbarElement(this.index, entity.ID);
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x00037FE4 File Offset: 0x000361E4
	public void SetEntity(string entityID)
	{
		if (entityID == "")
		{
			this.Clear();
			return;
		}
		EntityData entityData = Library.RequestData<EntityData>(entityID);
		if (entityData != null)
		{
			this.SetEntity(entityData);
		}
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0003801C File Offset: 0x0003621C
	public void Clear()
	{
		if (this._model != null)
		{
			Object.Destroy(this._model);
		}
		this.resourceObj.SetActive(false);
		this._entity = null;
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0003804A File Offset: 0x0003624A
	public void Use()
	{
		Singleton<Events>.Instance.onHotbarElementUsed.Invoke(this);
	}

	// Token: 0x040008A8 RID: 2216
	public int index;

	// Token: 0x040008A9 RID: 2217
	public GameObject resourceObj;

	// Token: 0x040008AA RID: 2218
	public TextMeshProUGUI resourceAmount;

	// Token: 0x040008AB RID: 2219
	public Image resourceIcon;

	// Token: 0x040008AC RID: 2220
	private EntityData _entity;

	// Token: 0x040008AD RID: 2221
	private GameObject _model;
}
