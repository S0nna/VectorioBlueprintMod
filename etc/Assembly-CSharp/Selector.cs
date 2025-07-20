using System;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Vectorio.Entities;
using Vectorio.PhasmaUI;
using Vectorio.Utilities;

// Token: 0x0200018C RID: 396
public class Selector : Singleton<Selector>
{
	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000D4C RID: 3404 RVA: 0x00039B90 File Offset: 0x00037D90
	public bool IsEnabled
	{
		get
		{
			return this._isEnabled;
		}
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x00039B98 File Offset: 0x00037D98
	public void Setup()
	{
		this._selectedDictionary = base.GetComponent<SelectedDictionary>();
		this.inputMap.gameObject.SetActive(false);
		this.outputMap.gameObject.SetActive(false);
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x00039BC8 File Offset: 0x00037DC8
	public void SetCoverageType(CoverageType type)
	{
		this._coverageType = type;
		bool flag = type == CoverageType.Pickup;
		this._currentSelectionMap = (flag ? this.inputMap : this.outputMap);
		this._currentSelectionColor = (flag ? this.inputColor : this.outputColor);
		if (flag)
		{
			this._currentSelectionMap = this.inputMap;
			this._currentSelectionColor = this.inputColor;
			this.inputTitle.color = this.inputColor;
			this.inputDesc.text = "CURRENTLY ACTIVE";
			this.inputBackground.color = this.inputColor;
			this.inputDigital.color = this.inputColor;
			this.inputIcon.color = this.inputColor;
			this.inputDigital.gameObject.SetActive(true);
			this.outputTitle.color = this.inactiveColor;
			this.outputDesc.text = "CLICK TO ACTIVATE";
			this.outputBackground.color = this.inactiveColor;
			this.outputDigital.color = this.inactiveColor;
			this.outputIcon.color = this.inactiveColor;
			this.outputDigital.gameObject.SetActive(false);
			if (this.inputSelectedSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.inputSelectedSound);
				return;
			}
		}
		else
		{
			this._currentSelectionMap = this.outputMap;
			this._currentSelectionColor = this.outputColor;
			this.outputTitle.color = this.outputColor;
			this.outputDesc.text = "CURRENTLY ACTIVE";
			this.outputBackground.color = this.outputColor;
			this.outputDigital.color = this.outputColor;
			this.outputIcon.color = this.outputColor;
			this.outputDigital.gameObject.SetActive(true);
			this.inputTitle.color = this.inactiveColor;
			this.inputDesc.text = "CLICK TO ACTIVATE";
			this.inputBackground.color = this.inactiveColor;
			this.inputDigital.color = this.inactiveColor;
			this.inputIcon.color = this.inactiveColor;
			this.inputDigital.gameObject.SetActive(false);
			if (this.outputSelectedSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.outputSelectedSound);
			}
		}
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00039E19 File Offset: 0x00038019
	public void SetInputCoverageType()
	{
		this.SetCoverageType(CoverageType.Pickup);
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x00039E22 File Offset: 0x00038022
	public void SetOutputCoverageType()
	{
		this.SetCoverageType(CoverageType.Dropoff);
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x00039E2B File Offset: 0x0003802B
	public void SetOppositeCoverageType()
	{
		if (this._coverageType == CoverageType.Pickup)
		{
			this.SetCoverageType(CoverageType.Dropoff);
			return;
		}
		if (this._coverageType == CoverageType.Dropoff)
		{
			this.SetCoverageType(CoverageType.Pickup);
		}
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x00039E50 File Offset: 0x00038050
	public void Open(CargoDrone cargoDrone)
	{
		for (int i = 0; i < this._filterButtons.Count; i++)
		{
			this._filterButtons[i].gameObject.SetActive(false);
		}
		this._cargoDrone = cargoDrone;
		DroneCoverage droneCoverage = this._cargoDrone.GetDroneCoverage(CoverageType.Pickup);
		DroneCoverage droneCoverage2 = this._cargoDrone.GetDroneCoverage(CoverageType.Dropoff);
		if (droneCoverage != null)
		{
			this.LoadCoverageArea(cargoDrone, droneCoverage.area, CoverageType.Pickup);
		}
		if (droneCoverage2 != null)
		{
			this.LoadCoverageArea(cargoDrone, droneCoverage2.area, CoverageType.Dropoff);
		}
		this.SetCoverageType(CoverageType.Pickup);
		this.Toggle(true);
		List<ResourceData> list = (from resource in Library.RequestAllDataOfType<ResourceData>()
		where !Singleton<ResourceManager>.Instance.IsResourceIgnored(resource) && Singleton<Research>.Instance.IsResourceUnlocked(resource)
		orderby resource.Order
		select resource).ToList<ResourceData>();
		for (int j = 0; j < list.Count; j++)
		{
			ResourceData resource2 = list[j];
			FilterButton filterButton;
			if (j < this._filterButtons.Count)
			{
				filterButton = this._filterButtons[j];
				filterButton.gameObject.SetActive(true);
			}
			else
			{
				filterButton = Object.Instantiate<FilterButton>(this.filterButtonPrefab);
				filterButton.transform.SetParent(this.filterList);
				filterButton.transform.localScale = Vector2.one;
				this._filterButtons.Add(filterButton);
			}
			Color color = (j % 2 == 0) ? this.filterColorOne : this.filterColorTwo;
			filterButton.Set(cargoDrone, resource2, color);
		}
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x00039FE4 File Offset: 0x000381E4
	public void Bake()
	{
		if (this._cargoDrone == null)
		{
			return;
		}
		this._cargoDrone.CreateCoverage(this._inputArea, this._outputArea);
		EntityMetadataEvent data = new EntityMetadataEvent
		{
			RuntimeID = this._cargoDrone.RuntimeID,
			Metadata = this._cargoDrone.Entity.ExtractMetadata(false, MetadataContext.Global),
			AsPipette = true
		};
		Singleton<EntityManager>.Instance.QueueMetadataEvent(data, SyncType.ClientInitiated);
		this.Toggle(false);
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x0003A060 File Offset: 0x00038260
	public void Close()
	{
		this.Toggle(false);
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0003A06C File Offset: 0x0003826C
	public void Toggle(bool toggle)
	{
		if (toggle && !this._isEnabled)
		{
			Singleton<Events>.Instance.onChangeBuildMode.Invoke(Hologram.BuildMode.Default);
			InputManager.OnPrimaryActionPressed.AddListener(new UnityAction(this.OnSelectionStart));
			InputManager.OnPrimaryActionReleased.AddListener(new UnityAction(this.OnSelectionEnd));
			InputManager.OnEditModeActionPressed.AddListener(new UnityAction(this.SetOppositeCoverageType));
			InputManager.OnBackActionPressed.AddListener(new UnityAction(this.Close));
		}
		else if (!toggle && this._isEnabled)
		{
			InputManager.OnPrimaryActionPressed.RemoveListener(new UnityAction(this.OnSelectionStart));
			InputManager.OnPrimaryActionReleased.RemoveListener(new UnityAction(this.OnSelectionEnd));
			InputManager.OnEditModeActionPressed.RemoveListener(new UnityAction(this.SetOppositeCoverageType));
			InputManager.OnBackActionPressed.RemoveListener(new UnityAction(this.Close));
		}
		this.buildingHotbar.alpha = (toggle ? 0f : 1f);
		this.buildingHotbar.interactable = !toggle;
		this.buildingHotbar.blocksRaycasts = !toggle;
		this.selectorHotbar.alpha = (toggle ? 1f : 0f);
		this.selectorHotbar.interactable = toggle;
		this.selectorHotbar.blocksRaycasts = toggle;
		this.inputMap.gameObject.SetActive(toggle);
		this.outputMap.gameObject.SetActive(toggle);
		if (!toggle)
		{
			if (this._inputLine != null)
			{
				Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._inputLine);
			}
			if (this._deliveryLine != null)
			{
				Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._deliveryLine);
			}
			if (this._outputLine != null)
			{
				Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._outputLine);
			}
		}
		this._isEnabled = toggle;
		if (!toggle)
		{
			this._cargoDrone = null;
		}
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x0003A23C File Offset: 0x0003843C
	public void Update()
	{
		if (!this._isEnabled)
		{
			return;
		}
		CargoDrone cargoDrone = this._cargoDrone;
		if (((cargoDrone != null) ? cargoDrone.Filter : null) != null)
		{
			if (!this.filterSelectedObject.activeSelf)
			{
				this.filterSelectedObject.SetActive(true);
				this.noFilterSelectedObject.SetActive(false);
				this.filterBackground.color = this.filterBackgroundColor;
				this.filterDigital.color = this.filterDigitalColor;
			}
			if (this._lastFilter == null || this._lastFilter != this._cargoDrone.Filter)
			{
				this._lastFilter = this._cargoDrone.Filter;
				this.filterIcon.sprite = this._lastFilter.IconSprite;
				this.filterName.text = this._lastFilter.Name.ToUpper();
			}
		}
		else if (this.filterSelectedObject.activeSelf)
		{
			this.filterSelectedObject.SetActive(false);
			this.noFilterSelectedObject.SetActive(true);
			this.filterBackground.color = this.noFilterBackgroundColor;
			this.filterDigital.color = this.noFilterDigitalColor;
		}
		if (!this._isSelecting)
		{
			return;
		}
		Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (this._isBoxSelection)
		{
			this._currentPos = vector;
			this.DrawSelection();
			return;
		}
		if (vector != this._currentPos)
		{
			this._currentPos = vector;
			if (Vector2.Distance(this._startPos, vector) > 1f)
			{
				this._isBoxSelection = true;
				this.DrawSelection();
			}
		}
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x0003A3D4 File Offset: 0x000385D4
	public void DrawSelection()
	{
		Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(this._startPos);
		Vector2Int vector2Int2 = Utilities.ConvertWorldPositionToCell(this._currentPos);
		if (this._startCell == vector2Int && this._endCell == vector2Int2)
		{
			return;
		}
		int num = (vector2Int2.x > vector2Int.x) ? vector2Int.x : vector2Int2.x;
		int num2 = (vector2Int2.x > vector2Int.x) ? vector2Int2.x : vector2Int.x;
		int num3 = (vector2Int2.y > vector2Int.y) ? vector2Int.y : vector2Int2.y;
		int num4 = (vector2Int2.y > vector2Int.y) ? vector2Int2.y : vector2Int.y;
		int num5 = num2 - num;
		int num6 = num4 - num3;
		int num7 = ((num5 == 0) ? 1 : num5) * ((num6 == 0) ? 1 : num6);
		if (num7 > this.maximumSelectionSize)
		{
			return;
		}
		this.DrawDeliveryLine(this._cargoDrone, this._coverageType, vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
		AudioClip audioClip = (num7 > this._size) ? this.areaIncreaseSound : this.areaDecreaseSound;
		if (audioClip != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(audioClip);
		}
		this._size = num7;
		this._startCell = vector2Int;
		this._endCell = vector2Int2;
		this._currentSelectionMap.ClearAllTiles();
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Vector3Int position = new Vector3Int(i, j, 0);
				this._currentSelectionMap.SetTile(position, this.selectionTile);
				this._currentSelectionMap.SetTileFlags(position, TileFlags.None);
				this._currentSelectionMap.SetColor(position, this._currentSelectionColor);
			}
		}
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x0003A5A9 File Offset: 0x000387A9
	public Vector2 CalculateCoverageAreaPosition(CoverageArea area)
	{
		return Utilities.ConvertCellPositionToWorld(new Vector2Int(area.startX + area.endX, area.startY + area.endY) / 2);
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x0003A5D8 File Offset: 0x000387D8
	public void DrawDeliveryLine(Drone drone, CoverageType coverage, int startX, int startY, int endX, int endY)
	{
		if (coverage == CoverageType.Pickup)
		{
			this._inputAreaCenterPos = Utilities.ConvertCellPositionToWorld(new Vector2Int(startX + endX, startY + endY)) / 2f;
			if (this._inputLine != null)
			{
				Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._inputLine);
			}
			this._inputLine = Singleton<EntityUtilities>.Instance.CreateConnectionLine(EntityUtilities.ConnectionType.DroneRoute, this._lineSprite, this._lineMaterial, Color.white, this._lineSortingOrder, drone.GetParentTransform().position, this._inputAreaCenterPos);
			if (this._outputArea != null)
			{
				if (this._deliveryLine != null)
				{
					Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._deliveryLine);
				}
				this._deliveryLine = Singleton<EntityUtilities>.Instance.CreateConnectionLine(EntityUtilities.ConnectionType.DroneRoute, this._lineSprite, this._lineMaterial, Color.white, this._lineSortingOrder, this._inputAreaCenterPos, this._outputAreaCenterPos);
				return;
			}
		}
		else if (coverage == CoverageType.Dropoff)
		{
			this._outputAreaCenterPos = Utilities.ConvertCellPositionToWorld(new Vector2Int(startX + endX, startY + endY)) / 2f;
			if (this._outputLine != null)
			{
				Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._outputLine);
			}
			this._outputLine = Singleton<EntityUtilities>.Instance.CreateConnectionLine(EntityUtilities.ConnectionType.DroneRoute, this._lineSprite, this._lineMaterial, Color.white, this._lineSortingOrder, this._outputAreaCenterPos, drone.GetParentTransform().position);
			if (this._inputArea != null)
			{
				if (this._deliveryLine != null)
				{
					Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._deliveryLine);
				}
				this._deliveryLine = Singleton<EntityUtilities>.Instance.CreateConnectionLine(EntityUtilities.ConnectionType.DroneRoute, this._lineSprite, this._lineMaterial, Color.white, this._lineSortingOrder, this._inputAreaCenterPos, this._outputAreaCenterPos);
			}
		}
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x0003A794 File Offset: 0x00038994
	public void LoadCoverageArea(Drone drone, CoverageArea area, CoverageType type)
	{
		this.DrawCoverageArea(area, type);
		this.DrawDeliveryLine(drone, type, area.startX, area.startY, area.endX, area.endY);
		if (type == CoverageType.Pickup)
		{
			this._inputArea = area;
			return;
		}
		if (type == CoverageType.Dropoff)
		{
			this._outputArea = area;
		}
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x0003A7E0 File Offset: 0x000389E0
	public void DrawCoverageArea(CoverageArea area, CoverageType type)
	{
		Tilemap tilemap = (type == CoverageType.Pickup) ? this.inputMap : this.outputMap;
		Color color = (type == CoverageType.Pickup) ? this.inputColor : this.outputColor;
		for (int i = area.startX; i <= area.endX; i++)
		{
			for (int j = area.startY; j <= area.endY; j++)
			{
				Vector3Int position = new Vector3Int(i, j, 0);
				tilemap.SetTile(position, this.selectionTile);
				tilemap.SetTileFlags(position, TileFlags.None);
				tilemap.SetColor(position, color);
			}
		}
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x0003A869 File Offset: 0x00038A69
	public void ClearActiveCoverageArea()
	{
		if (this._coverageType == CoverageType.Pickup)
		{
			this.ClearCoverageArea(CoverageType.Pickup);
			return;
		}
		if (this._coverageType == CoverageType.Dropoff)
		{
			this.ClearCoverageArea(CoverageType.Dropoff);
		}
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x0003A88C File Offset: 0x00038A8C
	public void ClearCoverageArea(CoverageType type)
	{
		if (type == CoverageType.Pickup)
		{
			if (this._inputLine != null)
			{
				Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._inputLine);
			}
			this.inputMap.ClearAllTiles();
			this._inputArea = null;
		}
		else if (type == CoverageType.Dropoff)
		{
			if (this._outputLine != null)
			{
				Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._outputLine);
			}
			this.outputMap.ClearAllTiles();
			this._outputArea = null;
		}
		if (this._deliveryLine != null)
		{
			Singleton<EntityUtilities>.Instance.DestroyConnectionLine(this._deliveryLine);
		}
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x0003A90F File Offset: 0x00038B0F
	public void OnSelectionStart()
	{
		if (!Singleton<Interface>.Instance.IsMouseOverUI)
		{
			this._startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this._isSelecting = true;
		}
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0003A93E File Offset: 0x00038B3E
	public void OnSelectionEnd()
	{
		if (this._isSelecting)
		{
			this._isSelecting = false;
			this.EndTileSelection();
		}
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x0003A955 File Offset: 0x00038B55
	protected virtual void EndTileSelection()
	{
		this.SetTiles(this._coverageType);
	}

	// Token: 0x06000D61 RID: 3425 RVA: 0x0003A964 File Offset: 0x00038B64
	public void SetTiles(CoverageType coverageType)
	{
		bool flag = coverageType == CoverageType.Pickup;
		Color color = flag ? this.inputColor : this.outputColor;
		Tilemap tilemap = flag ? this.inputMap : this.outputMap;
		if (!this._isBoxSelection)
		{
			Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(this._startPos);
			Vector3Int position = new Vector3Int(vector2Int.x, vector2Int.y, 0);
			tilemap.SetTile(position, this.selectionTile);
			tilemap.SetTileFlags(position, TileFlags.None);
			tilemap.SetColor(position, color);
			CoverageArea coverageArea = new CoverageArea(vector2Int, vector2Int);
			if (flag)
			{
				this._inputArea = coverageArea;
				return;
			}
			this._outputArea = coverageArea;
			return;
		}
		else
		{
			this._isBoxSelection = false;
			int num = (this._endCell.x > this._startCell.x) ? this._startCell.x : this._endCell.x;
			int num2 = (this._endCell.x > this._startCell.x) ? this._endCell.x : this._startCell.x;
			int num3 = (this._endCell.y > this._startCell.y) ? this._startCell.y : this._endCell.y;
			int num4 = (this._endCell.y > this._startCell.y) ? this._endCell.y : this._startCell.y;
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num4; j++)
				{
					Vector3Int position2 = new Vector3Int(i, j, 0);
					tilemap.SetTile(position2, this.selectionTile);
					tilemap.SetTileFlags(position2, TileFlags.None);
					tilemap.SetColor(position2, color);
				}
			}
			CoverageArea coverageArea2 = new CoverageArea(new Vector2Int(num, num3), new Vector2Int(num2, num4));
			if (flag)
			{
				this._inputArea = coverageArea2;
				return;
			}
			this._outputArea = coverageArea2;
			return;
		}
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x0003AB50 File Offset: 0x00038D50
	public void PreviewDroneCoverage(Drone drone, bool toggle)
	{
		if (this._isEnabled)
		{
			return;
		}
		this.ClearCoverageArea(CoverageType.Pickup);
		this.ClearCoverageArea(CoverageType.Dropoff);
		if (toggle)
		{
			DroneCoverage droneCoverage = drone.GetDroneCoverage(CoverageType.Pickup);
			DroneCoverage droneCoverage2 = drone.GetDroneCoverage(CoverageType.Dropoff);
			if (droneCoverage != null)
			{
				this.LoadCoverageArea(drone, droneCoverage.area, CoverageType.Pickup);
			}
			if (droneCoverage2 != null)
			{
				this.LoadCoverageArea(drone, droneCoverage2.area, CoverageType.Dropoff);
			}
		}
		this.inputMap.gameObject.SetActive(toggle);
		this.outputMap.gameObject.SetActive(toggle);
	}

	// Token: 0x0400093F RID: 2367
	public CanvasGroup buildingHotbar;

	// Token: 0x04000940 RID: 2368
	public CanvasGroup selectorHotbar;

	// Token: 0x04000941 RID: 2369
	protected CargoDrone _cargoDrone;

	// Token: 0x04000942 RID: 2370
	private ResourceData _lastFilter;

	// Token: 0x04000943 RID: 2371
	protected SelectedDictionary _selectedDictionary;

	// Token: 0x04000944 RID: 2372
	[SerializeField]
	protected Sprite _lineSprite;

	// Token: 0x04000945 RID: 2373
	[SerializeField]
	protected Material _lineMaterial;

	// Token: 0x04000946 RID: 2374
	[SerializeField]
	protected int _lineSortingOrder;

	// Token: 0x04000947 RID: 2375
	protected EntityUtilities.ConnectionLine _inputLine;

	// Token: 0x04000948 RID: 2376
	protected EntityUtilities.ConnectionLine _deliveryLine;

	// Token: 0x04000949 RID: 2377
	protected EntityUtilities.ConnectionLine _outputLine;

	// Token: 0x0400094A RID: 2378
	protected bool _isEnabled;

	// Token: 0x0400094B RID: 2379
	protected bool _isSelecting;

	// Token: 0x0400094C RID: 2380
	protected bool _isBoxSelection;

	// Token: 0x0400094D RID: 2381
	public Tilemap inputMap;

	// Token: 0x0400094E RID: 2382
	public Tilemap outputMap;

	// Token: 0x0400094F RID: 2383
	public TileBase selectionTile;

	// Token: 0x04000950 RID: 2384
	public Color inputColor;

	// Token: 0x04000951 RID: 2385
	public Color outputColor;

	// Token: 0x04000952 RID: 2386
	public AudioClip areaIncreaseSound;

	// Token: 0x04000953 RID: 2387
	public AudioClip areaDecreaseSound;

	// Token: 0x04000954 RID: 2388
	public int maximumSelectionSize = 100;

	// Token: 0x04000955 RID: 2389
	protected Vector2Int _startCell = Vector2Int.zero;

	// Token: 0x04000956 RID: 2390
	protected Vector2Int _endCell = Vector2Int.zero;

	// Token: 0x04000957 RID: 2391
	protected int _size;

	// Token: 0x04000958 RID: 2392
	protected Tilemap _currentSelectionMap;

	// Token: 0x04000959 RID: 2393
	protected Color _currentSelectionColor;

	// Token: 0x0400095A RID: 2394
	protected CoverageArea _inputArea;

	// Token: 0x0400095B RID: 2395
	protected CoverageArea _outputArea;

	// Token: 0x0400095C RID: 2396
	protected Vector2 _inputAreaCenterPos;

	// Token: 0x0400095D RID: 2397
	protected Vector2 _outputAreaCenterPos;

	// Token: 0x0400095E RID: 2398
	protected CoverageType _coverageType;

	// Token: 0x0400095F RID: 2399
	protected bool _isMultiSelection;

	// Token: 0x04000960 RID: 2400
	protected Vector2 _startPos;

	// Token: 0x04000961 RID: 2401
	protected Vector2 _currentPos;

	// Token: 0x04000962 RID: 2402
	public TextMeshProUGUI inputTitle;

	// Token: 0x04000963 RID: 2403
	public TextMeshProUGUI outputTitle;

	// Token: 0x04000964 RID: 2404
	public TextMeshProUGUI inputDesc;

	// Token: 0x04000965 RID: 2405
	public TextMeshProUGUI outputDesc;

	// Token: 0x04000966 RID: 2406
	public Image inputBackground;

	// Token: 0x04000967 RID: 2407
	public Image inputDigital;

	// Token: 0x04000968 RID: 2408
	public Image outputBackground;

	// Token: 0x04000969 RID: 2409
	public Image outputDigital;

	// Token: 0x0400096A RID: 2410
	public Color inactiveColor;

	// Token: 0x0400096B RID: 2411
	public AudioClip inputSelectedSound;

	// Token: 0x0400096C RID: 2412
	public AudioClip outputSelectedSound;

	// Token: 0x0400096D RID: 2413
	public FilterButton filterButtonPrefab;

	// Token: 0x0400096E RID: 2414
	public Transform filterList;

	// Token: 0x0400096F RID: 2415
	public GameObject noFilterSelectedObject;

	// Token: 0x04000970 RID: 2416
	public GameObject filterSelectedObject;

	// Token: 0x04000971 RID: 2417
	public Image filterIcon;

	// Token: 0x04000972 RID: 2418
	public Image filterBackground;

	// Token: 0x04000973 RID: 2419
	public Image filterDigital;

	// Token: 0x04000974 RID: 2420
	public Image inputIcon;

	// Token: 0x04000975 RID: 2421
	public Image outputIcon;

	// Token: 0x04000976 RID: 2422
	public TextMeshProUGUI filterName;

	// Token: 0x04000977 RID: 2423
	public Color filterColorOne;

	// Token: 0x04000978 RID: 2424
	public Color filterColorTwo;

	// Token: 0x04000979 RID: 2425
	public Color filterBackgroundColor;

	// Token: 0x0400097A RID: 2426
	public Color filterDigitalColor;

	// Token: 0x0400097B RID: 2427
	public Color noFilterBackgroundColor;

	// Token: 0x0400097C RID: 2428
	public Color noFilterDigitalColor;

	// Token: 0x0400097D RID: 2429
	public SwitchManager autoFilter;

	// Token: 0x0400097E RID: 2430
	private List<FilterButton> _filterButtons = new List<FilterButton>();
}
