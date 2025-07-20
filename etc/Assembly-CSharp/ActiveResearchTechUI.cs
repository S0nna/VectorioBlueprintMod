using System;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x02000142 RID: 322
public class ActiveResearchTechUI : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000AAE RID: 2734 RVA: 0x0002CBB4 File Offset: 0x0002ADB4
	public void Setup()
	{
		if (!this._isListening)
		{
			Singleton<Events>.Instance.onResearchTechActivated.AddListener(new UnityAction<ResearchTechData, List<Cost>>(this.Set));
			Singleton<Events>.Instance.onResearchTechFinished.AddListener(new UnityAction<ResearchTechData>(this.Complete));
			this._isListening = true;
		}
		if (!Singleton<Gamemode>.Instance.UseResearch)
		{
			base.gameObject.SetActive(false);
		}
		base.Start();
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x0002CC24 File Offset: 0x0002AE24
	public void Update()
	{
		if (!this._isActive)
		{
			this._timePassed += Time.deltaTime * this.colorLerpSpeed;
			this.background.color = Color.Lerp(this.colorOne, this.colorTwo, Mathf.PingPong(this._timePassed, 1f));
		}
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0002CC80 File Offset: 0x0002AE80
	public void Set(ResearchTechData tech, List<Cost> costs)
	{
		this.background.color = this.colorOne;
		this.active.SetActive(true);
		this.digitalEffect.SetActive(true);
		this.inactive.SetActive(false);
		this._isActive = true;
		this.techName.text = ((tech.Name == "") ? tech.reward.name : tech.Name.ToUpper());
		this.techProgress.text = "0% RESEARCHED";
		this.progressBar.currentPercent = 0f;
		this.progressBar.maxValue = 0f;
		foreach (Cost cost in costs)
		{
			foreach (Cost cost2 in tech.costs)
			{
				if (cost2.resource == cost.resource)
				{
					this.progressBar.currentPercent += (float)(cost2.amount - cost.amount);
					this.progressBar.maxValue += (float)cost2.amount;
				}
			}
		}
		this.progressBar.UpdateUI();
		if (this._model != null)
		{
			Object.Destroy(this._model);
		}
		switch (tech.rewardType)
		{
		case ResearchTechData.RewardType.Entity:
			this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(tech.GetReward<EntityData>().model, new Vector2(25f, 25f), this.icon.transform, null);
			this._model.transform.localScale = Vector2.one;
			this.icon.enabled = false;
			return;
		case ResearchTechData.RewardType.Resource:
			this.icon.sprite = tech.GetReward<ResourceData>().IconSprite;
			this.icon.enabled = true;
			return;
		case ResearchTechData.RewardType.Recipe:
			this.icon.sprite = tech.GetReward<RecipeData>().outputResource.IconSprite;
			this.icon.enabled = true;
			return;
		case ResearchTechData.RewardType.Tree:
			this.icon.sprite = tech.GetReward<ResearchTreeData>().icon;
			this.icon.enabled = true;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0002CF08 File Offset: 0x0002B108
	public void UpdateProgress(int amount, string progress)
	{
		this.progressBar.currentPercent += (float)amount;
		this.progressBar.UpdateUI();
		this.techProgress.text = progress + " RESEARCHED";
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0002CF3F File Offset: 0x0002B13F
	public void Complete(ResearchTechData data)
	{
		if (this._isActive)
		{
			this.active.SetActive(false);
			this.digitalEffect.SetActive(false);
			this.inactive.SetActive(true);
			this._isActive = false;
		}
	}

	// Token: 0x040006AC RID: 1708
	public GameObject active;

	// Token: 0x040006AD RID: 1709
	public GameObject inactive;

	// Token: 0x040006AE RID: 1710
	public GameObject digitalEffect;

	// Token: 0x040006AF RID: 1711
	public TextMeshProUGUI techName;

	// Token: 0x040006B0 RID: 1712
	public TextMeshProUGUI techProgress;

	// Token: 0x040006B1 RID: 1713
	public Image icon;

	// Token: 0x040006B2 RID: 1714
	public Image background;

	// Token: 0x040006B3 RID: 1715
	public Color colorOne;

	// Token: 0x040006B4 RID: 1716
	public Color colorTwo;

	// Token: 0x040006B5 RID: 1717
	public ProgressBar progressBar;

	// Token: 0x040006B6 RID: 1718
	public float animationSpeed = 0.5f;

	// Token: 0x040006B7 RID: 1719
	public float colorLerpSpeed = 1f;

	// Token: 0x040006B8 RID: 1720
	protected GameObject _model;

	// Token: 0x040006B9 RID: 1721
	protected int _resourcesRequired;

	// Token: 0x040006BA RID: 1722
	protected int _resourcesTracked;

	// Token: 0x040006BB RID: 1723
	protected bool _isActive;

	// Token: 0x040006BC RID: 1724
	protected float _timePassed;

	// Token: 0x040006BD RID: 1725
	private bool _isListening;
}
