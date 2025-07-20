using System;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.Formatting;

namespace Vectorio.PhasmaUI
{
	// Token: 0x0200028F RID: 655
	[DefaultExecutionOrder(1)]
	public class UI_ResearchWindow : UI_Singleton<UI_ResearchWindow>
	{
		// Token: 0x06001281 RID: 4737 RVA: 0x00054D9C File Offset: 0x00052F9C
		public void Setup()
		{
			if (!this._isListening)
			{
				Singleton<Events>.Instance.onResearchTechClicked.AddListener(new UnityAction<ResearchTechData>(this.SyncTech));
				InputManager.OnResearchActionPressed.AddListener(new UnityAction(this.Toggle));
				this._isListening = true;
			}
			this.activeTechUI.Setup();
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00054DF8 File Offset: 0x00052FF8
		public void Update()
		{
			if (!base.IsOpen)
			{
				return;
			}
			foreach (ResearchTechButton researchTechButton in this._techs)
			{
				if (researchTechButton.IsActive)
				{
					researchTechButton.CustomUpdate(this.buttonDropDownSpeed);
				}
			}
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00054E64 File Offset: 0x00053064
		public void CreateTreeButton(ResearchTreeData tree)
		{
			ResearchTreeButton researchTreeButton = Object.Instantiate<ResearchTreeButton>(this.treePrefab, this.treeList);
			researchTreeButton.Setup(tree, this._alternate ? this.treeColorOne : this.treeColorTwo);
			this._trees.Add(researchTreeButton);
			this._alternate = !this._alternate;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00054EBC File Offset: 0x000530BC
		public void SetTree(ResearchTreeData tree)
		{
			if (tree == this._selectedTree)
			{
				return;
			}
			this._selectedTree = tree;
			foreach (ResearchTechButton researchTechButton in this._techs)
			{
				researchTechButton.Enabled = false;
			}
			for (int i = 0; i < tree.nodes.Count; i++)
			{
				ResearchTechData researchTechData = (ResearchTechData)tree.nodes[i];
				if (researchTechData == null)
				{
					Debug.Log("[RESEARCH UI] Invalid tech conversion at node index " + i.ToString());
				}
				else if (Singleton<Research>.Instance.IsTechUnlocked(researchTechData))
				{
					Debug.Log("[RESEARCH UI] Tech " + researchTechData.ID + " is already unlocked!");
				}
				else
				{
					ResearchTechButton researchTechButton2;
					if (i < this._techs.Count)
					{
						researchTechButton2 = this._techs[i];
						researchTechButton2.Enabled = true;
					}
					else
					{
						researchTechButton2 = Object.Instantiate<ResearchTechButton>(this.techPrefab, this.techList);
						researchTechButton2.transform.SetParent(this.techList);
						this._techs.Add(researchTechButton2);
					}
					researchTechButton2.Set(researchTechData);
				}
			}
			this.categoryTitle.text = "<b>AVAILABLE TECHS:</b> " + tree.name.ToUpper();
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00055018 File Offset: 0x00053218
		public void UpdateTreeButtons()
		{
			foreach (ResearchTreeButton researchTreeButton in this._trees)
			{
				researchTreeButton.UpdateButton();
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00055068 File Offset: 0x00053268
		public void SyncTech(ResearchTechData tech)
		{
			if (!NetworkPlayerManager.ONLY_CLIENT_ON_SERVER)
			{
				NetworkSingleton<ClientStateManager>.Instance.Cmd_QueueResearchChange(new ResearchChangeEvent
				{
					ID = tech.ID
				});
				return;
			}
			this.SetActiveTech(tech, tech.costs);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0005509C File Offset: 0x0005329C
		public void UpdateTechButtons()
		{
			for (int i = 0; i < this._techs.Count; i++)
			{
				if (this._techs[i].Enabled)
				{
					ResearchTechData data = this._techs[i].Data;
					if (data == null || Singleton<Research>.Instance.IsTechUnlocked(data))
					{
						Object.Destroy(this._techs[i].gameObject);
						this._techs.RemoveAt(i);
						i--;
					}
					else
					{
						this._techs[i].UpdateButton();
					}
				}
			}
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00055138 File Offset: 0x00053338
		public void SetActiveTech(ResearchTechData tech, List<Cost> costs)
		{
			Singleton<Research>.Instance.SetActiveTech(tech, costs, true);
			if (this._model != null)
			{
				Object.Destroy(this._model);
			}
			foreach (KeyValuePair<ResourceData, UI_ResourceTechRequirement> keyValuePair in this._resources)
			{
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this._resources = new Dictionary<ResourceData, UI_ResourceTechRequirement>();
			this.progressBar.currentPercent = 0f;
			this.progressBar.maxValue = 0f;
			foreach (Cost cost in costs)
			{
				foreach (Cost cost2 in tech.costs)
				{
					if (cost2.resource == cost.resource)
					{
						UI_ResourceTechRequirement ui_ResourceTechRequirement = Object.Instantiate<UI_ResourceTechRequirement>(this.requirementPrefab, this.resourceList);
						ui_ResourceTechRequirement.Set(cost.resource.Name.ToUpper(), cost.resource.IconSprite, cost.amount, cost2.amount);
						this._resources.Add(cost.resource, ui_ResourceTechRequirement);
						this.progressBar.currentPercent += (float)(cost2.amount - cost.amount);
						this.progressBar.maxValue += (float)cost2.amount;
					}
				}
			}
			this.title.text = tech.Name.ToUpper();
			this.activeTechIcon.gameObject.SetActive(true);
			switch (tech.rewardType)
			{
			case ResearchTechData.RewardType.Entity:
			{
				EntityData reward = tech.GetReward<EntityData>();
				if (reward != null)
				{
					this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(reward.model, this.activeTechIconSize, this.activeTechIcon.transform, null);
					this.activeTechIcon.enabled = false;
				}
				break;
			}
			case ResearchTechData.RewardType.Resource:
			{
				ResourceData reward2 = tech.GetReward<ResourceData>();
				if (reward2 != null)
				{
					this.activeTechIcon.sprite = reward2.IconSprite;
					this.activeTechIcon.enabled = true;
				}
				break;
			}
			case ResearchTechData.RewardType.Recipe:
			{
				RecipeData reward3 = tech.GetReward<RecipeData>();
				if (reward3 != null)
				{
					this.activeTechIcon.sprite = reward3.outputResource.IconSprite;
					this.activeTechIcon.enabled = true;
				}
				break;
			}
			case ResearchTechData.RewardType.Tree:
			{
				ResearchTreeData reward4 = tech.GetReward<ResearchTreeData>();
				if (reward4 != null)
				{
					this.activeTechIcon.sprite = reward4.icon;
					this.activeTechIcon.enabled = true;
				}
				break;
			}
			}
			this.progressBar.UpdateUI();
			if (this._newTechs != null && this._newTechs.Count > 0)
			{
				foreach (GameObject obj in this._newTechs)
				{
					Object.Destroy(obj);
				}
				this._newTechs.Clear();
			}
			this.BuildUnlockList(tech, this.newTechSize, this.newTechsList, ref this._newTechs);
			this.UpdateTechButtons();
			this.ToggleActiveTech(true);
			Singleton<Events>.Instance.onResearchTechActivated.Invoke(tech, costs);
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x000554E8 File Offset: 0x000536E8
		public void UpdateActiveTech(ResourceData resource, int amount, int changed)
		{
			if (this._resources.ContainsKey(resource))
			{
				this._resources[resource].UpdateUI(amount);
				this.progressBar.currentPercent += (float)changed;
				this.progressBar.UpdateUI();
				string progress = Formatter.Round(this.progressBar.currentPercent / this.progressBar.maxValue * 100f, 0) + "%";
				if (this.activeResearchTechUI != null)
				{
					this.activeResearchTechUI.UpdateProgress(changed, progress);
				}
			}
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00055580 File Offset: 0x00053780
		public void ToggleActiveTech(bool toggle)
		{
			if (!toggle)
			{
				this.activeTech.SetActive(false);
				this.scanEffectObject.SetActive(false);
				this.noActiveTech.SetActive(true);
				if (this._model != null)
				{
					Object.Destroy(this._model);
				}
				this.activeTechIcon.gameObject.SetActive(false);
				return;
			}
			this.activeTech.SetActive(true);
			this.scanEffectObject.SetActive(true);
			this.noActiveTech.SetActive(false);
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00055604 File Offset: 0x00053804
		public override void Open()
		{
			if (!Singleton<Gamemode>.Instance.UseResearch || !Singleton<Interface>.Instance.CanOpenUI)
			{
				return;
			}
			base.SetIsOpen(true);
			Singleton<Interface>.Instance.SetCurrentlyOpen(this);
			if (this.canvasGroup.interactable)
			{
				return;
			}
			float num = 0f;
			foreach (MenuButton menuButton in this.menuButtons)
			{
				menuButton.group.GetComponent<RectTransform>().localPosition = menuButton.inPos;
				menuButton.group.alpha = 0f;
				LeanTween.alphaCanvas(menuButton.group, 1f, this.buttonAnimationSpeed).setDelay(num);
				LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, this.buttonAnimationSpeed).setEase(LeanTweenType.easeOutExpo).setDelay(num += this.buttonAlphaCooldown);
			}
			this.canvasGroup.interactable = true;
			this.canvasGroup.blocksRaycasts = true;
			if (this.openSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.openSound);
			}
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0005574C File Offset: 0x0005394C
		public override void ForceOpen()
		{
			base.SetIsOpen(true);
			foreach (MenuButton menuButton in this.menuButtons)
			{
				menuButton.group.alpha = 1f;
				menuButton.group.transform.localPosition = menuButton.normalPos;
			}
			this.canvasGroup.interactable = true;
			this.canvasGroup.blocksRaycasts = true;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x000557E4 File Offset: 0x000539E4
		public override void Close()
		{
			base.SetIsOpen(false);
			Singleton<Interface>.Instance.SetCurrentlyOpen(null);
			if (!this.canvasGroup.interactable)
			{
				return;
			}
			float num = 0f;
			foreach (MenuButton menuButton in this.menuButtons)
			{
				LeanTween.alphaCanvas(menuButton.group, 0f, this.buttonAnimationSpeed).setDelay(num);
				LeanTween.moveLocal(menuButton.group.gameObject, menuButton.outPos, this.buttonAnimationSpeed).setEase(LeanTweenType.easeOutExpo).setDelay(num += this.buttonAlphaCooldown);
			}
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			if (this.closeSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.closeSound);
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x000558E4 File Offset: 0x00053AE4
		public override void ForceClose()
		{
			base.SetIsOpen(false);
			foreach (MenuButton menuButton in this.menuButtons)
			{
				if (LeanTween.isTweening(menuButton.group.gameObject))
				{
					LeanTween.cancel(menuButton.group.gameObject);
				}
				menuButton.group.alpha = 0f;
				menuButton.group.transform.localPosition = menuButton.outPos;
			}
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0005599C File Offset: 0x00053B9C
		public void BuildUnlockList(ResearchTechData data, Vector2 size, Transform list, ref List<GameObject> objects)
		{
			foreach (ResearchTechData researchTechData in data.GetOutputNodes())
			{
				switch (researchTechData.rewardType)
				{
				case ResearchTechData.RewardType.Entity:
				{
					EntityData reward = researchTechData.GetReward<EntityData>();
					if (!(reward == null))
					{
						GameObject gameObject = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(reward.model, size, list, null);
						gameObject.AddComponent<RectTransform>().sizeDelta = this.newTechSize;
						gameObject.transform.localScale = Vector2.one;
						objects.Add(gameObject);
					}
					break;
				}
				case ResearchTechData.RewardType.Resource:
				{
					ResourceData reward2 = researchTechData.GetReward<ResourceData>();
					if (!(reward2 == null))
					{
						Image image = new GameObject(reward2.ID).AddComponent<Image>();
						image.transform.SetParent(list);
						image.transform.localScale = Vector2.one;
						image.GetComponent<RectTransform>().sizeDelta = size;
						image.sprite = reward2.IconSprite;
						objects.Add(image.gameObject);
					}
					break;
				}
				case ResearchTechData.RewardType.Recipe:
				{
					RecipeData reward3 = researchTechData.GetReward<RecipeData>();
					if (!(reward3 == null))
					{
						Image image = new GameObject(reward3.ID).AddComponent<Image>();
						image.transform.SetParent(list);
						image.transform.localScale = Vector2.one;
						image.GetComponent<RectTransform>().sizeDelta = size;
						image.sprite = reward3.outputResource.IconSprite;
						objects.Add(image.gameObject);
					}
					break;
				}
				case ResearchTechData.RewardType.Tree:
				{
					ResearchTreeData reward4 = researchTechData.GetReward<ResearchTreeData>();
					if (!(reward4 == null))
					{
						Image image = new GameObject(reward4.ID).AddComponent<Image>();
						image.transform.SetParent(list);
						image.transform.localScale = Vector2.one;
						image.GetComponent<RectTransform>().sizeDelta = size;
						image.sprite = reward4.icon;
						objects.Add(image.gameObject);
					}
					break;
				}
				}
			}
		}

		// Token: 0x04001022 RID: 4130
		public ResearchTechButton techPrefab;

		// Token: 0x04001023 RID: 4131
		public ResearchTreeButton treePrefab;

		// Token: 0x04001024 RID: 4132
		public Color treeColorOne;

		// Token: 0x04001025 RID: 4133
		public Color treeColorTwo;

		// Token: 0x04001026 RID: 4134
		private bool _alternate;

		// Token: 0x04001027 RID: 4135
		public ResearchTreeData _selectedTree;

		// Token: 0x04001028 RID: 4136
		private Dictionary<ResourceData, UI_ResourceTechRequirement> _resources = new Dictionary<ResourceData, UI_ResourceTechRequirement>();

		// Token: 0x04001029 RID: 4137
		private List<ResearchTreeButton> _trees = new List<ResearchTreeButton>();

		// Token: 0x0400102A RID: 4138
		private List<ResearchTechButton> _techs = new List<ResearchTechButton>();

		// Token: 0x0400102B RID: 4139
		public Image activeTechIcon;

		// Token: 0x0400102C RID: 4140
		public GameObject scanEffectObject;

		// Token: 0x0400102D RID: 4141
		public Vector2 activeTechIconSize = new Vector2(40f, 40f);

		// Token: 0x0400102E RID: 4142
		public ActiveResearchTechUI activeTechUI;

		// Token: 0x0400102F RID: 4143
		public List<MenuButton> menuButtons;

		// Token: 0x04001030 RID: 4144
		public float buttonAnimationSpeed;

		// Token: 0x04001031 RID: 4145
		public float buttonAlphaCooldown;

		// Token: 0x04001032 RID: 4146
		public float buttonDropDownSpeed = 500f;

		// Token: 0x04001033 RID: 4147
		public TextMeshProUGUI categoryTitle;

		// Token: 0x04001034 RID: 4148
		public GameObject activeTech;

		// Token: 0x04001035 RID: 4149
		public GameObject noActiveTech;

		// Token: 0x04001036 RID: 4150
		public Transform treeList;

		// Token: 0x04001037 RID: 4151
		public Transform techList;

		// Token: 0x04001038 RID: 4152
		public Transform resourceList;

		// Token: 0x04001039 RID: 4153
		public TextMeshProUGUI title;

		// Token: 0x0400103A RID: 4154
		public ProgressBar progressBar;

		// Token: 0x0400103B RID: 4155
		public ActiveResearchTechUI activeResearchTechUI;

		// Token: 0x0400103C RID: 4156
		public UI_ResourceTechRequirement requirementPrefab;

		// Token: 0x0400103D RID: 4157
		protected GameObject _model;

		// Token: 0x0400103E RID: 4158
		public Vector2 modelSize;

		// Token: 0x0400103F RID: 4159
		public Transform newTechsList;

		// Token: 0x04001040 RID: 4160
		public Vector2 newTechSize;

		// Token: 0x04001041 RID: 4161
		protected List<GameObject> _newTechs = new List<GameObject>();

		// Token: 0x04001042 RID: 4162
		private bool _isListening;
	}
}
