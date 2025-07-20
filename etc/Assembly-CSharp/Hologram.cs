using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Entities;
using Vectorio.PhasmaUI;
using Vectorio.Utilities;

// Token: 0x02000180 RID: 384
public class Hologram : MonoBehaviour
{
	// Token: 0x06000C9E RID: 3230 RVA: 0x000367B8 File Offset: 0x000349B8
	public void Setup()
	{
		if (this._player.IsOwner)
		{
			if (this.entityInfoPrefab != null && this.resourceInfoPrefab != null)
			{
				this._entityInfo = Object.Instantiate<EntityInfo>(this.entityInfoPrefab, Singleton<Interface>.Instance.transform);
				this._resourceInfo = Object.Instantiate<ResourceTooltip>(this.resourceInfoPrefab, Singleton<Interface>.Instance.transform);
				this._entityInfo.GetComponent<RectTransform>().SetSiblingIndex(0);
				this._entityInfo.Setup(Singleton<Interface>.Instance.gameObject.GetComponent<Canvas>(), this._resourceInfo);
				this._resourceInfo.GetComponent<RectTransform>().SetSiblingIndex(0);
				this._resourceInfo.Setup(Singleton<Interface>.Instance.gameObject.GetComponent<Canvas>(), this._entityInfo);
			}
			Singleton<Events>.Instance.onHotbarElementUsed.AddListener(new UnityAction<HotbarElement>(this.OnHotbarUsed));
			Singleton<Events>.Instance.onInventoryButtonClicked.AddListener(new UnityAction<EntityData>(this.OnInventoryButtonClicked));
			Singleton<Events>.Instance.onChangeBuildMode.AddListener(new UnityAction<Hologram.BuildMode>(this.ChangeBuildMode));
			InputManager.OnPrimaryActionPressed.AddListener(new UnityAction(this.OnPrimaryActionStarted));
			InputManager.OnPrimaryActionReleased.AddListener(new UnityAction(this.OnPrimaryActionStopped));
			InputManager.OnPanCameraActionPressed.AddListener(new UnityAction(this.OnSecondaryActionStarted));
			InputManager.OnPanCameraActionReleased.AddListener(new UnityAction(this.OnSecondaryActionStopped));
			InputManager.OnEditModeActionPressed.AddListener(new UnityAction(this.OnQuickModeAction));
			InputManager.OnBuildModeActionPressed.AddListener(new UnityAction(this.OnBuildModeAction));
			InputManager.OnDeleteModeActionPressed.AddListener(new UnityAction(this.OnDeleteModeAction));
			InputManager.OnPipetteActionPressed.AddListener(new UnityAction(this.OnPipetteAction));
			InputManager.OnDeselectModePressed.AddListener(new UnityAction(this.OnDeselectModeAction));
			Singleton<Events>.Instance.onJumpSequenceStarted.AddListener(new UnityAction<Gateway>(this.OnJumpSequenceStarted));
			Singleton<Events>.Instance.onJumpSequenceFinished.AddListener(new UnityAction(this.OnJumpSequenceFinished));
			this.ChangeBuildMode(Hologram.BuildMode.Default);
			Debug.Log("[HOLOGRAM] Is setup and listening for player ID " + this._player.OwnerId.ToString());
		}
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x00036A05 File Offset: 0x00034C05
	private void OnJumpSequenceStarted(Gateway gateway)
	{
		if (this._entityInfo != null)
		{
			this._entityInfo.gameObject.SetActive(false);
		}
		if (this._resourceInfo != null)
		{
			this._resourceInfo.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x00036A45 File Offset: 0x00034C45
	private void OnJumpSequenceFinished()
	{
		if (this._entityInfo != null)
		{
			this._entityInfo.gameObject.SetActive(true);
		}
		if (this._resourceInfo != null)
		{
			this._resourceInfo.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x00036A85 File Offset: 0x00034C85
	private void OnQuickModeAction()
	{
		if (Singleton<Selector>.Instance.IsEnabled)
		{
			return;
		}
		this.ChangeBuildMode(Hologram.BuildMode.Editing);
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00036A9B File Offset: 0x00034C9B
	private void OnBuildModeAction()
	{
		if (Singleton<Selector>.Instance.IsEnabled)
		{
			return;
		}
		this.ChangeBuildMode(Hologram.BuildMode.Building);
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00036AB1 File Offset: 0x00034CB1
	private void OnDeleteModeAction()
	{
		if (Singleton<Selector>.Instance.IsEnabled)
		{
			return;
		}
		this.ChangeBuildMode(Hologram.BuildMode.Deleting);
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x00036AC7 File Offset: 0x00034CC7
	private void OnDeselectModeAction()
	{
		if (Singleton<Selector>.Instance.IsEnabled)
		{
			return;
		}
		this.ChangeBuildMode(Hologram.BuildMode.Default);
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00036AE0 File Offset: 0x00034CE0
	public void ChangeBuildMode(Hologram.BuildMode buildMode)
	{
		this.StopCurrentMode();
		this._lastPosition = Vector2.zero;
		this._lastDelete = 0U;
		if (buildMode == this._buildMode)
		{
			buildMode = Hologram.BuildMode.Default;
		}
		this._buildMode = buildMode;
		Hologram.MODE_SELECTED = (this._buildMode > Hologram.BuildMode.Default);
		switch (this._buildMode)
		{
		case Hologram.BuildMode.Editing:
			Singleton<TileGrid>.Instance.ToggleBuildingLayer(true);
			break;
		case Hologram.BuildMode.Deleting:
			this._deleteModeIcon.gameObject.SetActive(true);
			break;
		}
		Singleton<Events>.Instance.onBuildModeChanged.Invoke(this._buildMode);
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x00036B78 File Offset: 0x00034D78
	private void StopCurrentMode()
	{
		switch (this._buildMode)
		{
		case Hologram.BuildMode.Default:
			if (this._primaryAction)
			{
				Singleton<Events>.Instance.onStopDraggingCamera.Invoke();
			}
			if (this._hoveringEntity != null)
			{
				IMouseListener[] components = this._hoveringEntity.GetComponents<IMouseListener>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].OnMouseHover(false);
				}
				this._hoveringEntity = null;
				return;
			}
			break;
		case Hologram.BuildMode.Editing:
			Singleton<TileGrid>.Instance.ToggleBuildingLayer(false);
			if (this._hoveringEntity != null)
			{
				IMouseListener[] components = this._hoveringEntity.GetComponents<IMouseListener>();
				for (int j = 0; j < components.Length; j++)
				{
					components[j].OnMouseHover(false);
				}
				this._hoveringEntity = null;
				return;
			}
			break;
		case Hologram.BuildMode.Building:
			this.Unequip();
			return;
		case Hologram.BuildMode.Deleting:
			this._deleteModeIcon.gameObject.SetActive(false);
			if (this._hoveringEntity != null)
			{
				if (this._previousAccent != null)
				{
					this._hoveringEntity.GetModel.ApplyAccent(this._previousAccent);
					this._previousAccent = null;
				}
				else
				{
					this._hoveringEntity.GetModel.ResetAccent();
				}
				this._hoveringEntity = null;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00036CA4 File Offset: 0x00034EA4
	private void OnPrimaryActionStarted()
	{
		if (Singleton<Interface>.Instance.IsMouseOverUI)
		{
			return;
		}
		this._primaryAction = true;
		switch (this._buildMode)
		{
		case Hologram.BuildMode.Default:
			if (!this.CheckConditions())
			{
				return;
			}
			if (Singleton<Settings>.Instance.UseDragMovement)
			{
				Singleton<Events>.Instance.onStartDraggingCamera.Invoke();
				return;
			}
			break;
		case Hologram.BuildMode.Editing:
		case Hologram.BuildMode.Building:
			break;
		case Hologram.BuildMode.Deleting:
			this._deletingEntities = (Physics2D.RaycastAll(base.transform.position, -Vector3.forward, float.PositiveInfinity, this._entityLayer).Length != 0);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x00036D48 File Offset: 0x00034F48
	private void OnPrimaryActionStopped()
	{
		this._primaryAction = false;
		switch (this._buildMode)
		{
		case Hologram.BuildMode.Default:
			Singleton<Events>.Instance.onStopDraggingCamera.Invoke();
			break;
		case Hologram.BuildMode.Editing:
		case Hologram.BuildMode.Building:
		case Hologram.BuildMode.Deleting:
			break;
		default:
			return;
		}
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00036D89 File Offset: 0x00034F89
	private void OnSecondaryActionStarted()
	{
		Singleton<Events>.Instance.onStartDraggingCamera.Invoke();
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x00036D9A File Offset: 0x00034F9A
	private void OnSecondaryActionStopped()
	{
		Singleton<Events>.Instance.onStopDraggingCamera.Invoke();
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x00036DAC File Offset: 0x00034FAC
	private void OnPipetteAction()
	{
		switch (this._buildMode)
		{
		case Hologram.BuildMode.Default:
			this.Pipette();
			return;
		case Hologram.BuildMode.Editing:
			this.Pipette();
			return;
		case Hologram.BuildMode.Building:
			this.Pipette();
			return;
		case Hologram.BuildMode.Deleting:
			this.Pipette();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x00036DF4 File Offset: 0x00034FF4
	public void Tick()
	{
		if (this._buildMode != Hologram.BuildMode.Editing)
		{
			this._entityInfo.CustomUpdate(null);
			if (this._buildMode != Hologram.BuildMode.Default)
			{
				this._resourceInfo.CustomUpdate(null);
			}
		}
		else if (this._buildMode != Hologram.BuildMode.Default)
		{
			this._resourceInfo.CustomUpdate(null);
		}
		if (this._interactionOnCooldown)
		{
			if (this._interactionCooldown <= 0f)
			{
				this._interactionEntity = null;
				this._interactionOnCooldown = false;
				this._interactionCooldown = 1f;
			}
			else
			{
				this._interactionCooldown -= Time.deltaTime;
			}
		}
		Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		base.transform.position = vector;
		switch (this._buildMode)
		{
		case Hologram.BuildMode.Default:
			this.HoverEntity(false);
			if (this._primaryAction)
			{
				this.ClickEntity(false);
				return;
			}
			break;
		case Hologram.BuildMode.Editing:
			this.HoverEntity(true);
			if (this._primaryAction)
			{
				this.ClickEntity(true);
				return;
			}
			break;
		case Hologram.BuildMode.Building:
			if (this._useSnapping)
			{
				base.transform.position = Utilities.CalculateBuildingPosition(vector, this._snapWidth, this._snapHeight);
			}
			if (this._rangeVisualizer.gameObject.activeSelf)
			{
				this._rangeVisualizer.transform.position = base.transform.position;
			}
			if (this._primaryAction)
			{
				this.UseBuildMode();
				return;
			}
			break;
		case Hologram.BuildMode.Deleting:
			if (this._primaryAction)
			{
				this.Delete();
			}
			foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(base.transform.position, -Vector3.forward, float.PositiveInfinity, this._entityLayer))
			{
				if (raycastHit2D.collider.gameObject.layer == LayerMask.NameToLayer(Layers.ALLY_BUILDING_LAYER))
				{
					Entity component = raycastHit2D.collider.GetComponent<Entity>();
					if (component != null && !component.Has_EFlag_IsDead && component.Has_EFlag_IsEditable)
					{
						if (component == this._hoveringEntity)
						{
							return;
						}
						if (this._hoveringEntity != null)
						{
							if (this._previousAccent != null)
							{
								this._hoveringEntity.GetModel.ApplyAccent(this._previousAccent);
							}
							else
							{
								this._hoveringEntity.GetModel.ResetAccent();
							}
						}
						this._previousAccent = component.GetModel.GetAccent();
						component.GetModel.ApplyAccent(this._deleteAccent);
						this._hoveringEntity = component;
						return;
					}
				}
			}
			if (this._hoveringEntity != null)
			{
				if (this._previousAccent != null)
				{
					this._hoveringEntity.GetModel.ApplyAccent(this._previousAccent);
				}
				else
				{
					this._hoveringEntity.GetModel.ResetAccent();
				}
				this._hoveringEntity = null;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x000370D3 File Offset: 0x000352D3
	private bool CheckConditions()
	{
		return !Singleton<Selector>.Instance.IsEnabled && !Singleton<Interface>.Instance.IsPanelOpen && !Singleton<Gamemode>.Instance.IsPaused;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x000370FC File Offset: 0x000352FC
	private bool CheckFaction(Entity entity)
	{
		return DevTools.ALLOW_ENEMY_EDITING || entity.IsAlly(this._player.GetFactionData().ID);
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x00037120 File Offset: 0x00035320
	private void HoverEntity(bool showInfo)
	{
		if (showInfo)
		{
			this._entityInfo.CustomUpdate(this._hoveringEntity);
		}
		if (this._hoveringEntity == null)
		{
			if (this.CheckConditions())
			{
				Vector2Int cell = Utilities.ConvertWorldPositionToCell(base.transform.position);
				this._resourceInfo.CustomUpdate(Singleton<TileGrid>.Instance.GetResource(cell));
			}
			else
			{
				this._resourceInfo.CustomUpdate(null);
			}
		}
		else
		{
			this._resourceInfo.CustomUpdate(null);
		}
		if (this.CheckConditions())
		{
			RaycastHit2D[] array = Physics2D.RaycastAll(base.transform.position, -Vector3.forward, float.PositiveInfinity, this._entityLayer);
			int i = 0;
			while (i < array.Length)
			{
				RaycastHit2D raycastHit2D = array[i];
				Entity component = raycastHit2D.collider.GetComponent<Entity>();
				if (component != null && !component.Has_EFlag_IsDead && this.CheckFaction(component))
				{
					if (component == this._hoveringEntity)
					{
						return;
					}
					IMouseListener[] components;
					if (this._hoveringEntity != null)
					{
						components = this._hoveringEntity.GetComponents<IMouseListener>();
						for (int j = 0; j < components.Length; j++)
						{
							components[j].OnMouseHover(false);
						}
					}
					components = component.GetComponents<IMouseListener>();
					for (int k = 0; k < components.Length; k++)
					{
						components[k].OnMouseHover(true);
					}
					this._hoveringEntity = component;
					return;
				}
				else
				{
					i++;
				}
			}
		}
		if (this._hoveringEntity != null)
		{
			IMouseListener[] components2 = this._hoveringEntity.GetComponents<IMouseListener>();
			for (int l = 0; l < components2.Length; l++)
			{
				components2[l].OnMouseHover(false);
			}
			this._hoveringEntity = null;
		}
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x000372E8 File Offset: 0x000354E8
	private void ClickEntity(bool quickEdit)
	{
		if (!this.CheckConditions())
		{
			return;
		}
		foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(base.transform.position, -Vector3.forward, float.PositiveInfinity, this._entityLayer))
		{
			Entity component = raycastHit2D.collider.GetComponent<Entity>();
			if (component != null && component != this._interactionEntity && this.CheckFaction(component))
			{
				this._interactionEntity = component;
				this._interactionOnCooldown = true;
				if (quickEdit)
				{
					component.QuickEdit();
				}
				else
				{
					Singleton<Events>.Instance.onEntityClicked.Invoke(component);
				}
			}
		}
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x000373A0 File Offset: 0x000355A0
	private void UseBuildMode()
	{
		if (!this.CheckConditions())
		{
			return;
		}
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y);
		if (vector == this._lastPosition)
		{
			return;
		}
		this._lastPosition = vector;
		if (this._pipetteData != null && this._buildingData != null)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			Utilities.CalculateBuildingCells(ref list, base.transform.position, this._buildingData.Width, this._buildingData.Height);
			Building building = null;
			bool flag = false;
			foreach (Vector2Int coords in list)
			{
				Building building2 = Singleton<TileGrid>.Instance.GetBuilding(coords);
				if (building2 != null)
				{
					if (flag && building.RuntimeID != building2.RuntimeID)
					{
						return;
					}
					if (building2.RuntimeID == this._lastPipetteID)
					{
						return;
					}
					if (building2.EntityID == this._pipetteData.EntityID || (building2.Entity.HasBridge && building2.Entity.PipetteBridge.ID == this._pipetteData.EntityID))
					{
						flag = true;
						building = building2;
					}
				}
				else if (flag)
				{
					return;
				}
			}
			if (flag)
			{
				this._lastPipetteID = building.RuntimeID;
				Singleton<EntityManager>.Instance.QueueMetadataEvent(EventBuilder.BuildMetadataEvent(this._lastPipetteID, this._pipetteData, true), SyncType.ClientInitiated);
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this._pasteSound);
				Singleton<AnimationManager>.Instance.CreatePasteSquare(building.transform.position, new Vector2((float)building.Width, (float)building.Height));
				return;
			}
		}
		if (this._entityData != null)
		{
			EntityCreationData entityCreationData = EventBuilder.BuildCreationData(this._entityData.ID, this._player.GetFactionData().ID, base.transform.position, SyncType.ClientInitiated);
			EventBuilder.ApplyChecksToCreationData(ref entityCreationData, CheckFlags.CheckForEntity | CheckFlags.CheckBuildingComponents | CheckFlags.CheckForClaim);
			if (this._pipetteData != null)
			{
				entityCreationData.PipetteData = this._pipetteData;
			}
			if (!Singleton<Gamemode>.Instance.UseBuilderDrones || !this._entityData.useBuilderDrones || DevTools.INSTANT_BUILD)
			{
				EventBuilder.ApplyCostsToCreationData(ref entityCreationData, true);
			}
			else
			{
				EventBuilder.SetDataAsBlueprint(ref entityCreationData);
			}
			Singleton<EntityManager>.Instance.QueueCreationEvent(entityCreationData);
			return;
		}
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00037628 File Offset: 0x00035828
	private void Delete()
	{
		if (!this.CheckConditions())
		{
			return;
		}
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y);
		if (vector == this._lastPosition)
		{
			return;
		}
		this._lastPosition = vector;
		if (this._deletingEntities)
		{
			foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(base.transform.position, -Vector3.forward, float.PositiveInfinity, this._entityLayer))
			{
				if (raycastHit2D.collider != null)
				{
					Entity component = raycastHit2D.collider.GetComponent<Entity>();
					if (!(component == null) && this._lastDelete != component.RuntimeID && this.CheckFaction(component) && component.Has_EFlag_IsEditable)
					{
						EntityDestroyEvent data = EventBuilder.BuildDestroyEvent(component, null);
						Singleton<EntityManager>.Instance.QueueDestroyEvent(data, SyncType.ClientInitiated);
					}
				}
			}
			return;
		}
		Vector2Int coords = Utilities.ConvertWorldPositionToCell(base.transform.position);
		if (Singleton<TileGrid>.Instance.IsCellClaimed(coords))
		{
			Singleton<TileGrid>.Instance.ClearTileDesign(new Vector3Int(coords.x, coords.y, 0));
		}
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00037778 File Offset: 0x00035978
	private void Pipette()
	{
		if (!this.CheckConditions())
		{
			return;
		}
		Vector2Int coords = Utilities.ConvertWorldPositionToCell(base.transform.position);
		Building building = Singleton<TileGrid>.Instance.GetBuilding(coords);
		if (building != null && !building.Entity.Has_EFlag_IsDead && this.CheckFaction(building.Entity) && building.Entity.Has_EFlag_IsEditable)
		{
			EntityData entity = Library.RequestData<EntityData>(building.EntityID);
			this.EquipEntity(entity, building.Entity.ExtractMetadata(true, MetadataContext.Global));
			Singleton<AudioPlayer>.Instance.PlayClipAtPoint(this._pipetteSound, "sound_pipette", base.transform.position, 1f, false, 1f, 1f, true);
			Singleton<AnimationManager>.Instance.CreateCopySquare(building.transform.position, new Vector2((float)building.Width, (float)building.Height));
		}
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x00037870 File Offset: 0x00035A70
	private void OnInventoryButtonClicked(EntityData entity)
	{
		this.EquipEntity(entity, null);
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0003787C File Offset: 0x00035A7C
	private void OnHotbarUsed(HotbarElement element)
	{
		if (Singleton<Interface>.Instance.IsPanelOpen)
		{
			if (this._entityData != null)
			{
				element.SetEntity(this._entityData);
				return;
			}
		}
		else if (element.EntityData != null)
		{
			this.EquipEntity(element.EntityData, null);
		}
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x000378CC File Offset: 0x00035ACC
	private void EquipEntity(EntityData entity, EntityMetadata metadata)
	{
		if (Singleton<Selector>.Instance.IsEnabled)
		{
			return;
		}
		this._pipetteData = metadata;
		if (entity.HasComponent<BuildingData>())
		{
			this._buildingData = entity.GetComponent<BuildingData>();
			this._useSnapping = true;
			this._snapWidth = this._buildingData.Width;
			this._snapHeight = this._buildingData.Height;
			if (entity.HasComponent<TurretData>())
			{
				this._rangeVisualizer.gameObject.SetActive(true);
				this._rangeVisualizer.sprite = LegacyLibrary.CIRCLE_RANGE_SPRITE;
				TurretData component = entity.GetComponent<TurretData>();
				this._rangeVisualizer.transform.localScale = new Vector2(component.range * 2f + 1f, component.range * 2f + 1f);
			}
			else if (entity.HasComponent<ClaimerData>())
			{
				this._rangeVisualizer.gameObject.SetActive(true);
				this._rangeVisualizer.sprite = LegacyLibrary.SQUARE_RANGE_SPRITE;
				ClaimerData component2 = entity.GetComponent<ClaimerData>();
				this._rangeVisualizer.transform.localScale = new Vector2((float)component2.range * 2f + 1f, (float)component2.range * 2f + 1f);
			}
			else
			{
				this._rangeVisualizer.gameObject.SetActive(false);
			}
		}
		else
		{
			if (entity.HasComponent<TilePlacerData>())
			{
				this._useSnapping = true;
				this._snapWidth = 1;
				this._snapHeight = 1;
			}
			else
			{
				this._useSnapping = false;
			}
			this._buildingData = null;
			this._rangeVisualizer.gameObject.SetActive(false);
		}
		if (this._model != null)
		{
			Object.Destroy(this._model.gameObject);
		}
		this._model = Singleton<ModelConstructor>.Instance.BuildBlueprintModel(entity.model, base.transform);
		this._model.transform.localPosition = Vector3.zero;
		this._entityData = entity;
		if (this._buildMode != Hologram.BuildMode.Building)
		{
			this.ChangeBuildMode(Hologram.BuildMode.Building);
		}
		Singleton<Events>.Instance.onEntityEquipped.Invoke(this._entityData);
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x00037AE0 File Offset: 0x00035CE0
	private void Unequip()
	{
		this._pipetteData = null;
		this._rangeVisualizer.gameObject.SetActive(false);
		this._entityData = null;
		this._buildingData = null;
		if (this._model != null)
		{
			Object.Destroy(this._model.gameObject);
		}
		Singleton<Interface>.Instance.SetCanPauseFlag(true);
		Singleton<TileGrid>.Instance.ToggleBuildingLayer(false);
		Singleton<Events>.Instance.onEntityUnequip.Invoke();
	}

	// Token: 0x04000876 RID: 2166
	public static bool MODE_SELECTED;

	// Token: 0x04000877 RID: 2167
	private Hologram.BuildMode _buildMode;

	// Token: 0x04000878 RID: 2168
	private Entity _hoveringEntity;

	// Token: 0x04000879 RID: 2169
	private Entity _interactionEntity;

	// Token: 0x0400087A RID: 2170
	private float _interactionCooldown = 0.5f;

	// Token: 0x0400087B RID: 2171
	private bool _interactionOnCooldown;

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	private SpriteRenderer _deleteModeIcon;

	// Token: 0x0400087D RID: 2173
	[SerializeField]
	private SpriteRenderer _rangeVisualizer;

	// Token: 0x0400087E RID: 2174
	[SerializeField]
	private AudioClip _pipetteSound;

	// Token: 0x0400087F RID: 2175
	[SerializeField]
	private AudioClip _pasteSound;

	// Token: 0x04000880 RID: 2176
	[SerializeField]
	private AudioClip _blueprintSound;

	// Token: 0x04000881 RID: 2177
	[SerializeField]
	private Accent _editAccent;

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private Accent _deleteAccent;

	// Token: 0x04000883 RID: 2179
	private Accent _previousAccent;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private Player _player;

	// Token: 0x04000885 RID: 2181
	[SerializeField]
	private LayerMask _entityLayer;

	// Token: 0x04000886 RID: 2182
	private bool _useSnapping;

	// Token: 0x04000887 RID: 2183
	public int _snapWidth;

	// Token: 0x04000888 RID: 2184
	public int _snapHeight;

	// Token: 0x04000889 RID: 2185
	private BuildingData _buildingData;

	// Token: 0x0400088A RID: 2186
	private EntityData _entityData;

	// Token: 0x0400088B RID: 2187
	private ModelObject _model;

	// Token: 0x0400088C RID: 2188
	private EntityMetadata _pipetteData;

	// Token: 0x0400088D RID: 2189
	private uint _lastPipetteID;

	// Token: 0x0400088E RID: 2190
	private bool _primaryAction;

	// Token: 0x0400088F RID: 2191
	private bool _deletingEntities;

	// Token: 0x04000890 RID: 2192
	public EntityInfo entityInfoPrefab;

	// Token: 0x04000891 RID: 2193
	public ResourceTooltip resourceInfoPrefab;

	// Token: 0x04000892 RID: 2194
	private EntityInfo _entityInfo;

	// Token: 0x04000893 RID: 2195
	private ResourceTooltip _resourceInfo;

	// Token: 0x04000894 RID: 2196
	private Vector2 _lastPosition;

	// Token: 0x04000895 RID: 2197
	private uint _lastDelete;

	// Token: 0x02000181 RID: 385
	public enum BuildMode
	{
		// Token: 0x04000897 RID: 2199
		Default,
		// Token: 0x04000898 RID: 2200
		Editing,
		// Token: 0x04000899 RID: 2201
		Building,
		// Token: 0x0400089A RID: 2202
		Deleting,
		// Token: 0x0400089B RID: 2203
		Commanding
	}
}
