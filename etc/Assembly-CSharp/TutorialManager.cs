using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000200 RID: 512
public class TutorialManager : Singleton<TutorialManager>
{
	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000F75 RID: 3957 RVA: 0x00048FF1 File Offset: 0x000471F1
	// (set) Token: 0x06000F76 RID: 3958 RVA: 0x00048FF9 File Offset: 0x000471F9
	public List<string> CompletedSegments
	{
		get
		{
			return this._completedSegments;
		}
		set
		{
			this._completedSegments = value;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000F77 RID: 3959 RVA: 0x00049002 File Offset: 0x00047202
	public bool IsOpen
	{
		get
		{
			return this._isOpen;
		}
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x0004900C File Offset: 0x0004720C
	public void Setup()
	{
		if (!this.isEnabled)
		{
			return;
		}
		this._segments = new Dictionary<string, TutorialManager.Segment>();
		foreach (TutorialManager.Segment segment in this.segmentPrefabs)
		{
			if (!this._segments.ContainsKey(segment.id))
			{
				if (!segment.useColor)
				{
					segment.buttonTitleColor = this.defaultTitleColor;
					segment.buttonBorderColor = this.defaultBorderColor;
					segment.buttonBackgroundColor = this.defaultBackgroundColor;
				}
				this._segments.Add(segment.id, segment);
				if (this._completedSegments.Contains(segment.id))
				{
					if (segment.tutorialButton != null)
					{
						segment.tutorialButton.Unlock(segment);
					}
				}
				else
				{
					TutorialManager.SegmentType listenType = (TutorialManager.SegmentType)segment.listenType;
					if (listenType != TutorialManager.SegmentType.None)
					{
						if (!this._availableSegments.ContainsKey(listenType))
						{
							this._availableSegments.Add(listenType, new List<TutorialManager.Segment>());
							this.RegisterSegmentEvent(listenType);
						}
						this._availableSegments[listenType].Add(segment);
					}
				}
			}
		}
		Singleton<Events>.Instance.onStartTutorial.AddListener(new UnityAction<string, bool>(this.StartSegment));
		Singleton<Events>.Instance.onResearchUnlockAccepted.AddListener(new UnityAction(this.OnConfirmed));
		Singleton<Events>.Instance.onUnlockAllTutorials.AddListener(new UnityAction(this.UnlockAllSegments));
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x00049188 File Offset: 0x00047388
	public void UnlockAllSegments()
	{
		if (!this.isEnabled)
		{
			return;
		}
		foreach (TutorialManager.Segment segment in this.segmentPrefabs)
		{
			this._completedSegments.Add(segment.id);
			if (segment.tutorialButton != null)
			{
				segment.tutorialButton.Unlock(segment);
			}
			TutorialManager.SegmentType listenType = (TutorialManager.SegmentType)segment.listenType;
			if (this._availableSegments.ContainsKey(listenType))
			{
				this._availableSegments[listenType].Remove(segment);
				if (this._availableSegments[listenType].Count == 0)
				{
					this._availableSegments.Remove(listenType);
					this.UnregisterSegmentEvent(listenType);
				}
			}
		}
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x00049260 File Offset: 0x00047460
	public void OnConfirmed()
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (this._confirmedSegment != null)
		{
			if (Singleton<Settings>.Instance.UseHints)
			{
				this.StartSegment(this._confirmedSegment);
			}
			this._confirmedSegment = null;
		}
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x00049294 File Offset: 0x00047494
	public void StartSegment(string id, bool overrideGamemodePreference)
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (!overrideGamemodePreference && !Singleton<Settings>.Instance.UseHints)
		{
			Debug.Log("[TUTORIAL] Gamemode has hints disabled, ignoring request for segment " + id);
			return;
		}
		if (!this._segments.ContainsKey(id))
		{
			Debug.Log(string.Concat(new string[]
			{
				"[TUTORIAL] Could not find segment with ID ",
				id,
				" out of ",
				this._segments.Count.ToString(),
				" available tutorials!"
			}));
			return;
		}
		TutorialManager.Segment segment = this._segments[id];
		if (segment.tutorialButton != null)
		{
			segment.tutorialButton.Unlock(segment);
		}
		if (!this._isOpen)
		{
			this.StartSegment(segment);
			return;
		}
		if (this._queuedSegments.Contains(segment))
		{
			Debug.Log("[TUTORIAL] This tutorial has already been queued!");
			return;
		}
		this._queuedSegments.Enqueue(segment);
		Debug.Log("[TUTORIAL] A tutorial is already active, " + id + " is now in queue position " + this._queuedSegments.Count.ToString());
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x000493A4 File Offset: 0x000475A4
	protected void StartSegment(TutorialManager.Segment segment)
	{
		if (!this.isEnabled)
		{
			return;
		}
		Debug.Log("[TUTORIAL] Activated tutorial with ID " + segment.id);
		this.title.text = segment.title.ToUpper();
		this.tutorialName.text = "<b>TUTORIAL |</b> <size=8>" + segment.tutorialName;
		this._activeSegment = segment;
		if (segment.useColor)
		{
			this.border.color = segment.borderColor;
			this.background.color = segment.backgroundColor;
		}
		else
		{
			this.border.color = this.defaultBorderColor;
			this.background.color = this.defaultBackgroundColor;
		}
		this._stepIndex = 0;
		if (!this._isOpen)
		{
			this.Open();
		}
		this.SetStep(0);
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x00049474 File Offset: 0x00047674
	public void NextStep()
	{
		int stepIndex = this._stepIndex + 1;
		this._stepIndex = stepIndex;
		this.SetStep(stepIndex);
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x00049498 File Offset: 0x00047698
	public void PreviousStep()
	{
		int stepIndex = this._stepIndex - 1;
		this._stepIndex = stepIndex;
		this.SetStep(stepIndex);
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x000494BC File Offset: 0x000476BC
	public void SetStep(int index)
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (index < this._activeSegment.steps.Count - 1)
		{
			if (this._finishedShown)
			{
				this.nextButton.SetActive(true);
				this.finishButton.SetActive(false);
				this._finishedShown = false;
			}
			this.DisplayStep(index);
			return;
		}
		if (!this._finishedShown)
		{
			this.DisplayStep(index);
			this.nextButton.SetActive(false);
			this.finishButton.SetActive(true);
			this._finishedShown = true;
		}
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x00049544 File Offset: 0x00047744
	private void DisplayStep(int index)
	{
		if (!this.isEnabled)
		{
			return;
		}
		this.backButton.SetActive(index != 0);
		TutorialManager.Segment.Step step = this._activeSegment.steps[index];
		this.description.text = step.description;
		this.step.text = "PART " + (index + 1).ToString() + "/" + this._activeSegment.steps.Count.ToString();
		this.videoPlayer.clip = step.clip;
		if (this.stepCompleteSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.stepCompleteSound);
		}
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x000495F8 File Offset: 0x000477F8
	public void Open()
	{
		if (!this.isEnabled)
		{
			return;
		}
		LeanTween.alphaCanvas(this.button.group, 1f, 0.5f);
		this.button.group.interactable = true;
		this.button.group.blocksRaycasts = true;
		this._isOpen = true;
		Singleton<Gamemode>.Instance.IsGamePaused = NetworkPlayerManager.ONLY_CLIENT_ON_SERVER;
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x00049664 File Offset: 0x00047864
	public void Close()
	{
		if (!this.isEnabled)
		{
			return;
		}
		this._completedSegments.Add(this._activeSegment.id);
		if (this._activeSegment.nextTutorialID != "")
		{
			if (this._segments.ContainsKey(this._activeSegment.nextTutorialID))
			{
				TutorialManager.Segment segment = this._segments[this._activeSegment.nextTutorialID];
				this.StartSegment(segment);
				return;
			}
			Debug.Log("[TUTORIAL] Next tutorial ID " + this._activeSegment.nextTutorialID + " does not exist!");
		}
		if (this._queuedSegments.Count > 0)
		{
			TutorialManager.Segment segment2 = this._queuedSegments.Dequeue();
			Debug.Log("[TUTORIAL] Queued segment available, starting " + segment2.id);
			this.StartSegment(segment2);
			return;
		}
		LeanTween.alphaCanvas(this.button.group, 0f, 0.5f);
		this.button.group.interactable = false;
		this.button.group.blocksRaycasts = false;
		this._isOpen = false;
		Singleton<Gamemode>.Instance.IsGamePaused = false;
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x00049784 File Offset: 0x00047984
	public void RegisterSegmentEvent(TutorialManager.SegmentType type)
	{
		if (!this.isEnabled)
		{
			return;
		}
		switch (type)
		{
		case TutorialManager.SegmentType.ResourceAdded:
			Singleton<Events>.Instance.onResourceUpdated.AddListener(new UnityAction<ResourceData, int>(this.OnResourceUpdated));
			return;
		case TutorialManager.SegmentType.ResearchFinished:
			Singleton<Events>.Instance.onResearchTechFinished.AddListener(new UnityAction<ResearchTechData>(this.OnResearchFinished));
			return;
		case TutorialManager.SegmentType.EntityPlaced:
			Singleton<Events>.Instance.onEntityCreated.AddListener(new UnityAction<Entity>(this.OnEntityPlaced));
			return;
		case TutorialManager.SegmentType.EntityUnlocked:
			Singleton<Events>.Instance.onEntityUnlocked.AddListener(new UnityAction<EntityData>(this.OnEntityUnlocked));
			return;
		default:
			return;
		}
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x00049824 File Offset: 0x00047A24
	public void UnregisterSegmentEvent(TutorialManager.SegmentType type)
	{
		if (!this.isEnabled)
		{
			return;
		}
		switch (type)
		{
		case TutorialManager.SegmentType.ResourceAdded:
			Singleton<Events>.Instance.onResourceUpdated.RemoveListener(new UnityAction<ResourceData, int>(this.OnResourceUpdated));
			return;
		case TutorialManager.SegmentType.ResearchFinished:
			Singleton<Events>.Instance.onResearchTechFinished.RemoveListener(new UnityAction<ResearchTechData>(this.OnResearchFinished));
			return;
		case TutorialManager.SegmentType.EntityPlaced:
			Singleton<Events>.Instance.onEntityCreated.RemoveListener(new UnityAction<Entity>(this.OnEntityPlaced));
			return;
		case TutorialManager.SegmentType.EntityUnlocked:
			Singleton<Events>.Instance.onEntityUnlocked.RemoveListener(new UnityAction<EntityData>(this.OnEntityUnlocked));
			return;
		default:
			return;
		}
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x000498C2 File Offset: 0x00047AC2
	public void OnResourceUpdated(ResourceData resourceData, int amount)
	{
		this.CheckSegment(TutorialManager.SegmentType.ResourceAdded, resourceData.ID, false);
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x000498D2 File Offset: 0x00047AD2
	public void OnResearchFinished(ResearchTechData techData)
	{
		this.CheckSegment(TutorialManager.SegmentType.ResearchFinished, techData.ID, false);
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x000498E2 File Offset: 0x00047AE2
	public void OnEntityPlaced(Entity entity)
	{
		this.CheckSegment(TutorialManager.SegmentType.EntityPlaced, entity.ID, false);
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x000498F2 File Offset: 0x00047AF2
	public void OnEntityUnlocked(EntityData entity)
	{
		this.CheckSegment(TutorialManager.SegmentType.EntityUnlocked, entity.ID, true);
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x00049904 File Offset: 0x00047B04
	private void CheckSegment(TutorialManager.SegmentType type, string id, bool wait)
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (!this._availableSegments.ContainsKey(type))
		{
			this.UnregisterSegmentEvent(type);
			return;
		}
		for (int i = 0; i < this._availableSegments[type].Count; i++)
		{
			TutorialManager.Segment segment = this._availableSegments[type][i];
			if (!(segment.listenTypeID != id))
			{
				if (this.PassSegment(segment, wait))
				{
					return;
				}
				i--;
			}
		}
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x0004997C File Offset: 0x00047B7C
	private bool PassSegment(TutorialManager.Segment segment, bool wait)
	{
		if (!this.isEnabled)
		{
			return false;
		}
		if (wait)
		{
			this._confirmedSegment = segment;
		}
		else
		{
			this.StartSegment(segment.id, false);
		}
		TutorialManager.SegmentType listenType = (TutorialManager.SegmentType)segment.listenType;
		this._availableSegments[listenType].Remove(segment);
		if (this._availableSegments[listenType].Count == 0)
		{
			this._availableSegments.Remove(listenType);
			this.UnregisterSegmentEvent(listenType);
			return true;
		}
		return false;
	}

	// Token: 0x04000D13 RID: 3347
	public List<TutorialManager.Segment> segmentPrefabs = new List<TutorialManager.Segment>();

	// Token: 0x04000D14 RID: 3348
	protected Dictionary<string, TutorialManager.Segment> _segments;

	// Token: 0x04000D15 RID: 3349
	protected List<string> _completedSegments = new List<string>();

	// Token: 0x04000D16 RID: 3350
	protected Queue<TutorialManager.Segment> _queuedSegments = new Queue<TutorialManager.Segment>();

	// Token: 0x04000D17 RID: 3351
	protected Dictionary<TutorialManager.SegmentType, List<TutorialManager.Segment>> _availableSegments = new Dictionary<TutorialManager.SegmentType, List<TutorialManager.Segment>>();

	// Token: 0x04000D18 RID: 3352
	protected TutorialManager.Segment _activeSegment;

	// Token: 0x04000D19 RID: 3353
	protected int _stepIndex;

	// Token: 0x04000D1A RID: 3354
	public bool isEnabled;

	// Token: 0x04000D1B RID: 3355
	public TextMeshProUGUI title;

	// Token: 0x04000D1C RID: 3356
	public TextMeshProUGUI tutorialName;

	// Token: 0x04000D1D RID: 3357
	public TextMeshProUGUI description;

	// Token: 0x04000D1E RID: 3358
	public TextMeshProUGUI step;

	// Token: 0x04000D1F RID: 3359
	public Image border;

	// Token: 0x04000D20 RID: 3360
	public Image background;

	// Token: 0x04000D21 RID: 3361
	public VideoPlayer videoPlayer;

	// Token: 0x04000D22 RID: 3362
	public MenuButton button;

	// Token: 0x04000D23 RID: 3363
	public CanvasGroup canvasGroup;

	// Token: 0x04000D24 RID: 3364
	public AudioClip stepCompleteSound;

	// Token: 0x04000D25 RID: 3365
	public Color defaultTitleColor;

	// Token: 0x04000D26 RID: 3366
	public Color defaultBorderColor;

	// Token: 0x04000D27 RID: 3367
	public Color defaultBackgroundColor;

	// Token: 0x04000D28 RID: 3368
	public GameObject finishButton;

	// Token: 0x04000D29 RID: 3369
	public GameObject nextButton;

	// Token: 0x04000D2A RID: 3370
	public GameObject backButton;

	// Token: 0x04000D2B RID: 3371
	protected bool _isOpen;

	// Token: 0x04000D2C RID: 3372
	protected bool _finishedShown;

	// Token: 0x04000D2D RID: 3373
	protected bool _waitConfirmation;

	// Token: 0x04000D2E RID: 3374
	protected TutorialManager.Segment _confirmedSegment;

	// Token: 0x02000201 RID: 513
	public enum SegmentType
	{
		// Token: 0x04000D30 RID: 3376
		None,
		// Token: 0x04000D31 RID: 3377
		ResourceAdded,
		// Token: 0x04000D32 RID: 3378
		ResearchFinished,
		// Token: 0x04000D33 RID: 3379
		EntityPlaced,
		// Token: 0x04000D34 RID: 3380
		EntityUnlocked
	}

	// Token: 0x02000202 RID: 514
	[Serializable]
	public class Segment
	{
		// Token: 0x04000D35 RID: 3381
		public string id;

		// Token: 0x04000D36 RID: 3382
		public int listenType;

		// Token: 0x04000D37 RID: 3383
		public string listenTypeID;

		// Token: 0x04000D38 RID: 3384
		[SerializeField]
		public List<TutorialManager.Segment.Step> steps;

		// Token: 0x04000D39 RID: 3385
		public string title;

		// Token: 0x04000D3A RID: 3386
		public string tutorialName = "The Basics";

		// Token: 0x04000D3B RID: 3387
		public string nextTutorialID = "";

		// Token: 0x04000D3C RID: 3388
		public bool useColor;

		// Token: 0x04000D3D RID: 3389
		public float cooldown;

		// Token: 0x04000D3E RID: 3390
		public TutoralButton tutorialButton;

		// Token: 0x04000D3F RID: 3391
		public Color borderColor;

		// Token: 0x04000D40 RID: 3392
		public Color backgroundColor;

		// Token: 0x04000D41 RID: 3393
		public Color buttonTitleColor;

		// Token: 0x04000D42 RID: 3394
		public Color buttonBorderColor;

		// Token: 0x04000D43 RID: 3395
		public Color buttonBackgroundColor;

		// Token: 0x02000203 RID: 515
		[Serializable]
		public class Step
		{
			// Token: 0x04000D44 RID: 3396
			public VideoClip clip;

			// Token: 0x04000D45 RID: 3397
			[TextArea]
			public string description;
		}
	}
}
