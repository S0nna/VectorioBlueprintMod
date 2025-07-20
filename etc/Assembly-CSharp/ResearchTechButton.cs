using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x02000159 RID: 345
public class ResearchTechButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00030BCC File Offset: 0x0002EDCC
	// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00030BD4 File Offset: 0x0002EDD4
	public bool Enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			this._enabled = value;
			base.gameObject.SetActive(this._enabled);
		}
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x00030BEE File Offset: 0x0002EDEE
	public TechStatus GetStatus()
	{
		return this._status;
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00030BF6 File Offset: 0x0002EDF6
	public bool IsActive
	{
		get
		{
			return this._isActive;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00030BFE File Offset: 0x0002EDFE
	public ResearchTechData Data
	{
		get
		{
			return this._data;
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x00030C08 File Offset: 0x0002EE08
	public void Set(ResearchTechData data)
	{
		this._data = data;
		this._rectTransform = base.GetComponent<RectTransform>();
		if (this._model != null)
		{
			Object.Destroy(this._model);
		}
		switch (data.rewardType)
		{
		case ResearchTechData.RewardType.Entity:
		{
			EntityData reward = data.GetReward<EntityData>();
			this.title.text = ((data.Name == "") ? reward.name : data.Name);
			this.desc.text = reward.Description;
			this.background.color = this.normalColor;
			this.shortDesc.color = reward.model.GetAccentColor(AccentType.PrimaryColor);
			this.shortDesc.text = "NEW " + reward.category.ToString();
			this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(reward.model, new Vector2(35f, 35f), this.iconParent.transform, null);
			this._model.transform.localScale = Vector2.one;
			this.iconParent.enabled = false;
			break;
		}
		case ResearchTechData.RewardType.Resource:
		{
			ResourceData reward2 = data.GetReward<ResourceData>();
			this.title.text = reward2.Name.ToUpper();
			this.desc.text = reward2.Description;
			Color primaryColor = reward2.Accent.primaryColor;
			this.shortDesc.color = primaryColor;
			this.shortDesc.text = "NEW RESOURCE";
			this.background.color = new Color(primaryColor.r, primaryColor.g, primaryColor.b, 0.1f);
			this.iconParent.sprite = reward2.IconSprite;
			this.iconParent.enabled = true;
			break;
		}
		case ResearchTechData.RewardType.Recipe:
		{
			RecipeData reward3 = data.GetReward<RecipeData>();
			this.title.text = reward3.Name.ToUpper();
			this.desc.text = reward3.Description;
			this.shortDesc.color = reward3.outputResource.Accent.primaryColor;
			this.shortDesc.text = "NEW RECIPE";
			this.background.color = new Color(this.shortDesc.color.r, this.shortDesc.color.g, this.shortDesc.color.b, 0.1f);
			this.iconParent.sprite = reward3.outputResource.IconSprite;
			this.iconParent.enabled = true;
			break;
		}
		case ResearchTechData.RewardType.Tree:
		{
			ResearchTreeData reward4 = data.GetReward<ResearchTreeData>();
			this.title.text = reward4.Name.ToUpper();
			this.desc.text = reward4.Description;
			this.background.color = this.normalColor;
			this.shortDesc.color = Color.white;
			this.shortDesc.text = "NEW TREE";
			this.iconParent.sprite = reward4.icon;
			this.iconParent.enabled = true;
			break;
		}
		}
		if (this._requirements != null && this._requirements.Count > 0)
		{
			foreach (UI_ResourceTechRequirement ui_ResourceTechRequirement in this._requirements.Values)
			{
				Object.Destroy(ui_ResourceTechRequirement.gameObject);
			}
			this._requirements.Clear();
		}
		foreach (Cost cost in data.costs)
		{
			UI_ResourceTechRequirement ui_ResourceTechRequirement2 = Object.Instantiate<UI_ResourceTechRequirement>(this.requirementPrefab, this.resourceList);
			if (cost.resource != null && !this._requirements.ContainsKey(cost.resource))
			{
				ui_ResourceTechRequirement2.Set(cost.resource.Name, cost.resource.IconSprite, cost.amount, cost.amount);
				this._requirements.Add(cost.resource, ui_ResourceTechRequirement2);
			}
			else
			{
				Debug.Log("[RESEARCH] Invalid resource for " + this.title.text + "!");
			}
		}
		if (this._newTechs != null && this._newTechs.Count > 0)
		{
			foreach (GameObject obj in this._newTechs)
			{
				Object.Destroy(obj);
			}
			this._newTechs.Clear();
		}
		if (data.HasOutputNodes())
		{
			UI_Singleton<UI_ResearchWindow>.Instance.BuildUnlockList(data, this.newTechSize, this.newTechsList, ref this._newTechs);
			this.noNewUnlocks.SetActive(false);
		}
		else
		{
			this.noNewUnlocks.SetActive(true);
		}
		if (data.HasInputNodes())
		{
			ResearchTechData inputNode = data.GetInputNode();
			this.unlockReq.text = "COMPLETE " + inputNode.Name.ToUpper() + " TECH TO DISCOVER";
		}
		this.UpdateButton();
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x00031160 File Offset: 0x0002F360
	public void UpdateButton()
	{
		this.SetStatus(Singleton<Research>.Instance.GetTechStatus(this.Data));
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00031178 File Offset: 0x0002F378
	public void SetStatus(TechStatus status)
	{
		this._status = status;
		if (status == TechStatus.Active)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		switch (status)
		{
		case TechStatus.Locked:
			this.normalObj.SetActive(false);
			this.encryptedObj.SetActive(false);
			this.quantumObj.SetActive(false);
			this.lockedObj.SetActive(true);
			this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, this.lockedButtonY);
			return;
		case TechStatus.Encrypted:
			this.normalObj.SetActive(false);
			this.encryptedObj.SetActive(!this._regionBound);
			this.quantumObj.SetActive(this._regionBound);
			this.lockedObj.SetActive(false);
			this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, this.normalButtonY);
			base.transform.SetSiblingIndex(1);
			return;
		case TechStatus.Available:
			this.normalObj.SetActive(true);
			this.encryptedObj.SetActive(false);
			this.quantumObj.SetActive(false);
			this.lockedObj.SetActive(false);
			this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, this.normalButtonY);
			base.transform.SetSiblingIndex(0);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x000312F4 File Offset: 0x0002F4F4
	public void CustomUpdate(float speed)
	{
		if (this._status != TechStatus.Available)
		{
			return;
		}
		if (this._expand)
		{
			if (this._rectTransform.sizeDelta.y < this.expandedButtonY)
			{
				this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, this._rectTransform.sizeDelta.y + speed * Time.deltaTime);
				return;
			}
			this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, this.expandedButtonY);
			this._isActive = false;
			return;
		}
		else
		{
			if (this._rectTransform.sizeDelta.y > this.normalButtonY)
			{
				this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, this._rectTransform.sizeDelta.y + -speed * Time.deltaTime);
				return;
			}
			this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, this.normalButtonY);
			this._isActive = false;
			return;
		}
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00031414 File Offset: 0x0002F614
	public void SetActiveTech()
	{
		if (this._status == TechStatus.Encrypted)
		{
			if (!this._regionBound)
			{
				Decryptor decryptor = Singleton<Research>.Instance.FindDecryptor(this._data);
				if (decryptor != null)
				{
					UI_Singleton<UI_ResearchWindow>.Instance.Close();
					Singleton<Events>.Instance.onMoveCameraToTarget.Invoke(decryptor.transform);
					return;
				}
			}
		}
		else if (this._status == TechStatus.Available)
		{
			Singleton<Events>.Instance.onResearchTechClicked.Invoke(this._data);
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x00031496 File Offset: 0x0002F696
	public override void ToggleHover(bool toggle)
	{
		if (this._status == TechStatus.Locked)
		{
			return;
		}
		if (this._status == TechStatus.Available)
		{
			this._isActive = true;
			this._expand = toggle;
		}
		base.ToggleHover(toggle);
	}

	// Token: 0x040007A7 RID: 1959
	private bool _enabled = true;

	// Token: 0x040007A8 RID: 1960
	protected TechStatus _status;

	// Token: 0x040007A9 RID: 1961
	protected bool _regionBound;

	// Token: 0x040007AA RID: 1962
	public GameObject quantumObj;

	// Token: 0x040007AB RID: 1963
	public GameObject encryptedObj;

	// Token: 0x040007AC RID: 1964
	public GameObject normalObj;

	// Token: 0x040007AD RID: 1965
	public GameObject lockedObj;

	// Token: 0x040007AE RID: 1966
	public TextMeshProUGUI title;

	// Token: 0x040007AF RID: 1967
	public TextMeshProUGUI shortDesc;

	// Token: 0x040007B0 RID: 1968
	public TextMeshProUGUI desc;

	// Token: 0x040007B1 RID: 1969
	public TextMeshProUGUI unlockReq;

	// Token: 0x040007B2 RID: 1970
	public Image background;

	// Token: 0x040007B3 RID: 1971
	public Image iconParent;

	// Token: 0x040007B4 RID: 1972
	public Transform resourceList;

	// Token: 0x040007B5 RID: 1973
	public UI_ResourceTechRequirement requirementPrefab;

	// Token: 0x040007B6 RID: 1974
	public GameObject noNewUnlocks;

	// Token: 0x040007B7 RID: 1975
	public Color normalColor;

	// Token: 0x040007B8 RID: 1976
	public float lockedButtonY;

	// Token: 0x040007B9 RID: 1977
	public float normalButtonY;

	// Token: 0x040007BA RID: 1978
	public float expandedButtonY;

	// Token: 0x040007BB RID: 1979
	protected Dictionary<ResourceData, UI_ResourceTechRequirement> _requirements = new Dictionary<ResourceData, UI_ResourceTechRequirement>();

	// Token: 0x040007BC RID: 1980
	protected ResearchTechData _data;

	// Token: 0x040007BD RID: 1981
	protected GameObject _model;

	// Token: 0x040007BE RID: 1982
	protected int _tier;

	// Token: 0x040007BF RID: 1983
	protected RectTransform _rectTransform;

	// Token: 0x040007C0 RID: 1984
	protected bool _isActive;

	// Token: 0x040007C1 RID: 1985
	protected bool _expand = true;

	// Token: 0x040007C2 RID: 1986
	public Transform newTechsList;

	// Token: 0x040007C3 RID: 1987
	public Vector2 newTechSize;

	// Token: 0x040007C4 RID: 1988
	protected List<GameObject> _newTechs = new List<GameObject>();
}
