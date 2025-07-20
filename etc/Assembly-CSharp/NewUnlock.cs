using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001D8 RID: 472
public class NewUnlock : MonoBehaviour
{
	// Token: 0x06000EAD RID: 3757 RVA: 0x0004234E File Offset: 0x0004054E
	public void Start()
	{
		Singleton<Events>.Instance.onResearchTechFinished.AddListener(new UnityAction<ResearchTechData>(this.OnTechFinished));
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x0004236B File Offset: 0x0004056B
	public void OnTechFinished(ResearchTechData data)
	{
		this.Open(data, "UNLOCKED BY RESEARCH");
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x0004237C File Offset: 0x0004057C
	public void Open(ResearchTechData data, string unlock)
	{
		if (this._model != null)
		{
			Object.Destroy(this._model);
		}
		this._techData = data;
		switch (data.rewardType)
		{
		case ResearchTechData.RewardType.Entity:
			this._entity = data.GetReward<EntityData>();
			if (this._entity == null)
			{
				return;
			}
			this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(this._entity.model, new Vector2(50f, 50f), this.modelParent.transform, null);
			this.modelParent.enabled = false;
			this._model.transform.localScale = Vector2.one;
			this.title.text = this._entity.Name.ToUpper();
			this.subtitle.text = unlock.ToUpper();
			this.description.text = this._entity.Description;
			if (this._entity.UseSpecialCost)
			{
				this.typeIcon.enabled = true;
				this.typeIcon.sprite = this._entity.SpecialCost.resource.IconSprite;
			}
			else
			{
				this.typeIcon.enabled = false;
			}
			if (this._entity.model == null)
			{
				this.glowImg.color = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				this.glowImg.color = this._entity.model.GetAccentColor(AccentType.PrimaryColor);
			}
			break;
		case ResearchTechData.RewardType.Resource:
		{
			ResourceData reward = data.GetReward<ResourceData>();
			if (reward == null)
			{
				return;
			}
			this.modelParent.sprite = reward.IconSprite;
			this.modelParent.enabled = true;
			this.title.text = reward.Name.ToUpper();
			this.subtitle.text = unlock.ToUpper();
			this.description.text = reward.Description;
			this.typeIcon.enabled = false;
			this.glowImg.color = reward.Accent.primaryColor;
			break;
		}
		case ResearchTechData.RewardType.Recipe:
		{
			RecipeData reward2 = data.GetReward<RecipeData>();
			if (reward2 == null)
			{
				return;
			}
			this.modelParent.sprite = reward2.outputResource.IconSprite;
			this.modelParent.enabled = true;
			this.title.text = reward2.Name.ToUpper();
			this.subtitle.text = unlock.ToUpper();
			this.description.text = reward2.Description;
			this.typeIcon.enabled = false;
			this.glowImg.color = reward2.outputResource.Accent.primaryColor;
			break;
		}
		case ResearchTechData.RewardType.Tree:
		{
			ResearchTreeData reward3 = data.GetReward<ResearchTreeData>();
			if (reward3 == null)
			{
				return;
			}
			this.modelParent.sprite = reward3.icon;
			this.modelParent.enabled = true;
			this.title.text = reward3.Name.ToUpper();
			this.subtitle.text = unlock.ToUpper();
			this.description.text = reward3.Description;
			this.typeIcon.enabled = false;
			this.glowImg.color = Color.white;
			break;
		}
		}
		this.subtitle.color = this.glowImg.color;
		float num = 0f;
		foreach (MenuButton menuButton in this.buttons)
		{
			menuButton.group.GetComponent<RectTransform>().localPosition = menuButton.inPos;
			if (menuButton.sound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayDelayedInterfaceSound(menuButton.sound, num);
			}
			menuButton.group.alpha = 0f;
			LeanTween.alphaCanvas(menuButton.group, 1f, 0.15f).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, 0.15f).setEase(LeanTweenType.easeOutExpo).setDelay(num += 0.05f);
		}
		this.glowGroup.alpha = 0f;
		this.glowTransform.sizeDelta = new Vector2(0f, 50f);
		LeanTween.alphaCanvas(this.glowGroup, 1f, 0.5f).setDelay(num);
		LeanTween.size(this.glowTransform, new Vector2(60f, 50f), 0.5f).setDelay(num).setEase(LeanTweenType.easeOutExpo);
		this.canvasGroup.alpha = 1f;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.openSound);
		Singleton<Gamemode>.Instance.IsGamePaused = NetworkPlayerManager.ONLY_CLIENT_ON_SERVER;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00042890 File Offset: 0x00040A90
	public void Close()
	{
		Singleton<Gamemode>.Instance.IsGamePaused = false;
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		if (this._techData.tutorialID != "")
		{
			Singleton<Events>.Instance.onStartTutorial.Invoke(this._techData.tutorialID, false);
		}
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x00042902 File Offset: 0x00040B02
	public void CloseAndOpenInventory()
	{
		this.Close();
		Singleton<Events>.Instance.openInventoryToEntity.Invoke(this._entity);
	}

	// Token: 0x04000BB2 RID: 2994
	protected EntityData _entity;

	// Token: 0x04000BB3 RID: 2995
	protected ResearchTechData _techData;

	// Token: 0x04000BB4 RID: 2996
	public Image modelParent;

	// Token: 0x04000BB5 RID: 2997
	public Image typeIcon;

	// Token: 0x04000BB6 RID: 2998
	public Image glowImg;

	// Token: 0x04000BB7 RID: 2999
	public TextMeshProUGUI title;

	// Token: 0x04000BB8 RID: 3000
	public TextMeshProUGUI subtitle;

	// Token: 0x04000BB9 RID: 3001
	public TextMeshProUGUI description;

	// Token: 0x04000BBA RID: 3002
	public CanvasGroup canvasGroup;

	// Token: 0x04000BBB RID: 3003
	public CanvasGroup glowGroup;

	// Token: 0x04000BBC RID: 3004
	public RectTransform glowTransform;

	// Token: 0x04000BBD RID: 3005
	public AudioClip openSound;

	// Token: 0x04000BBE RID: 3006
	public List<MenuButton> buttons;

	// Token: 0x04000BBF RID: 3007
	protected GameObject _model;
}
