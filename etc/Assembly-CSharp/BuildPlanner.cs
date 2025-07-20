using System;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001A5 RID: 421
public class BuildPlanner : MonoBehaviour
{
	// Token: 0x06000DCE RID: 3534 RVA: 0x0003D50F File Offset: 0x0003B70F
	public void Start()
	{
		Singleton<Events>.Instance.onBlueprintAdded.AddListener(new UnityAction<Blueprint>(this.OnBlueprintAdded));
		Singleton<Events>.Instance.onBlueprintRemoved.AddListener(new UnityAction<Blueprint>(this.OnBlueprintRemoved));
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0003D548 File Offset: 0x0003B748
	public void Update()
	{
		if (this._enabled)
		{
			this.totalQueued.text = Singleton<DroneManager>.Instance.TotalBlueprintsQueued.ToString();
			this.dronesReady.text = Singleton<DroneManager>.Instance.TotalDronesAvailable.ToString();
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x0003D597 File Offset: 0x0003B797
	public void OnBlueprintAdded(Blueprint blueprint)
	{
		this._queuedBlueprints.ContainsKey(blueprint.EntityID);
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0003D597 File Offset: 0x0003B797
	public void OnBlueprintRemoved(Blueprint blueprint)
	{
		this._queuedBlueprints.ContainsKey(blueprint.EntityID);
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x0003D5AC File Offset: 0x0003B7AC
	public void Toggle()
	{
		if (this._enabled)
		{
			LeanTween.cancel(this.plannerWindow.group.gameObject);
			this.plannerWindow.group.GetComponent<RectTransform>().localPosition = this.plannerWindow.normalPos;
			LeanTween.moveLocal(this.plannerWindow.group.gameObject, this.plannerWindow.outPos, 0.25f).setEase(LeanTweenType.easeOutExpo);
			this._enabled = false;
			this.enabledIcon.SetActive(false);
			this.disabledIcon.SetActive(true);
			return;
		}
		LeanTween.cancel(this.plannerWindow.group.gameObject);
		this.plannerWindow.group.GetComponent<RectTransform>().localPosition = this.plannerWindow.inPos;
		LeanTween.moveLocal(this.plannerWindow.group.gameObject, this.plannerWindow.normalPos, 0.25f).setEase(LeanTweenType.easeOutExpo);
		this._enabled = true;
		this.enabledIcon.SetActive(true);
		this.disabledIcon.SetActive(false);
	}

	// Token: 0x04000A10 RID: 2576
	public MenuButton plannerWindow;

	// Token: 0x04000A11 RID: 2577
	public GameObject enabledIcon;

	// Token: 0x04000A12 RID: 2578
	public GameObject disabledIcon;

	// Token: 0x04000A13 RID: 2579
	public RectTransform toggleButton;

	// Token: 0x04000A14 RID: 2580
	public TextMeshProUGUI totalQueued;

	// Token: 0x04000A15 RID: 2581
	public TextMeshProUGUI dronesReady;

	// Token: 0x04000A16 RID: 2582
	public HorizontalSelector priorityMode;

	// Token: 0x04000A17 RID: 2583
	public Transform resourceList;

	// Token: 0x04000A18 RID: 2584
	public QueuedBlueprint resourcePrefab;

	// Token: 0x04000A19 RID: 2585
	protected Dictionary<string, QueuedBlueprint> _queuedBlueprints = new Dictionary<string, QueuedBlueprint>();

	// Token: 0x04000A1A RID: 2586
	protected bool _enabled = true;
}
