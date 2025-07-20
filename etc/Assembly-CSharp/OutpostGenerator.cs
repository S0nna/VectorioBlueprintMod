using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.Entities;
using Vectorio.Grid;
using Vectorio.Utilities;

// Token: 0x020001DA RID: 474
[DefaultExecutionOrder(25)]
public class OutpostGenerator : Singleton<OutpostGenerator>
{
	// Token: 0x06000EB7 RID: 3767 RVA: 0x00042C40 File Offset: 0x00040E40
	public void Start()
	{
		this._canvasGroup = base.GetComponent<CanvasGroup>();
		InputManager.OnConsoleActionPressed.AddListener(new UnityAction(this.Toggle));
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x00042C64 File Offset: 0x00040E64
	public void Toggle()
	{
		if (Singleton<Gamemode>.Instance.AllowDeveloperTools)
		{
			this._isEnabled = !this._isEnabled;
			this._canvasGroup.alpha = (this._isEnabled ? 1f : 0f);
			this._canvasGroup.interactable = this._isEnabled;
			this._canvasGroup.blocksRaycasts = this._isEnabled;
			if (this._isEnabled)
			{
				Singleton<Events>.Instance.onEntityCreated.AddListener(new UnityAction<Entity>(this.OnEntityAdded));
				Singleton<Events>.Instance.onEntityDestroyed.AddListener(new UnityAction<Entity>(this.OnEntityRemoved));
				Singleton<Events>.Instance.onTileDesignPlaced.AddListener(new UnityAction<Vector3Int, TileDesign>(this.OnTileDesignAdded));
				Singleton<Events>.Instance.onTileDesignRemoved.AddListener(new UnityAction<Vector3Int>(this.OnTileDesignRemoved));
				Singleton<Events>.Instance.onResourceTilePlaced.AddListener(new UnityAction<Vector3Int, ResourceData>(this.OnResourceAdded));
				Singleton<Events>.Instance.onResourceTileRemoved.AddListener(new UnityAction<Vector3Int>(this.OnResourceRemoved));
				this.outpostID.text = "new_outpost";
				this.factionID.text = "faction_redscar";
				this.CreateCore();
				return;
			}
			EntityDestroyEvent data = EventBuilder.BuildDestroyEvent(this._activeCore.Entity, null);
			Singleton<EntityManager>.Instance.QueueDestroyEvent(data, SyncType.ServerInitiated);
			this.Clear();
			Singleton<Events>.Instance.onEntityCreated.RemoveListener(new UnityAction<Entity>(this.OnEntityAdded));
			Singleton<Events>.Instance.onEntityDestroyed.RemoveListener(new UnityAction<Entity>(this.OnEntityRemoved));
			Singleton<Events>.Instance.onTileDesignPlaced.RemoveListener(new UnityAction<Vector3Int, TileDesign>(this.OnTileDesignAdded));
			Singleton<Events>.Instance.onTileDesignRemoved.RemoveListener(new UnityAction<Vector3Int>(this.OnTileDesignRemoved));
			Singleton<Events>.Instance.onResourceTilePlaced.RemoveListener(new UnityAction<Vector3Int, ResourceData>(this.OnResourceAdded));
			Singleton<Events>.Instance.onResourceTileRemoved.RemoveListener(new UnityAction<Vector3Int>(this.OnResourceRemoved));
		}
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x00042E6C File Offset: 0x0004106C
	public void CreateCore()
	{
		EntityCreationData creationData = EventBuilder.BuildCreationData(this.coreID, Singleton<FactionManager>.Instance.PlayerFactionID, Singleton<WorldGenerator>.Instance.CenterWorldPos, SyncType.ServerInitiated);
		EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.ManagerCallback, 5U, 0);
		Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x00042EAF File Offset: 0x000410AF
	public void OnEntityCreated(Entity entity)
	{
		if (entity.Has_EComponent<OutpostCore>())
		{
			this._activeCore = entity.Get_EComponent<OutpostCore>(false);
		}
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x00042EC8 File Offset: 0x000410C8
	public void OnEntityAdded(Entity entity)
	{
		if (!this._entities.ContainsKey((ulong)entity.RuntimeID))
		{
			this._entities.Add((ulong)entity.RuntimeID, entity);
			this.count.text = string.Concat(new string[]
			{
				this._entities.Count.ToString(),
				" E | ",
				this._resources.Count.ToString(),
				" R | ",
				this._tiles.Count.ToString(),
				" T"
			});
			Debug.Log(string.Concat(new string[]
			{
				"[OUTPOST] Added entity with ID ",
				entity.GetData().ID,
				" (",
				entity.RuntimeID.ToString(),
				")"
			}));
			return;
		}
		Debug.Log("[OUTPOST] Duplicate entry with runtime ID " + entity.RuntimeID.ToString() + "!");
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x00042FDC File Offset: 0x000411DC
	public void OnEntityRemoved(Entity entity)
	{
		if (this._entities.ContainsKey((ulong)entity.RuntimeID))
		{
			this._entities.Remove((ulong)entity.RuntimeID);
			this.count.text = string.Concat(new string[]
			{
				this._entities.Count.ToString(),
				" E | ",
				this._resources.Count.ToString(),
				" R | ",
				this._tiles.Count.ToString(),
				" T"
			});
			Debug.Log(string.Concat(new string[]
			{
				"[OUTPOST] Removed entity with ID ",
				entity.GetData().ID,
				" (",
				entity.RuntimeID.ToString(),
				")"
			}));
		}
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x000430CC File Offset: 0x000412CC
	public void OnResourceAdded(Vector3Int coords, ResourceData resource)
	{
		Vector2Int vector2Int = new Vector2Int(coords.x, coords.y);
		Vector2Int vector2Int2;
		if (this._resources.ContainsKey(vector2Int))
		{
			this._resources[vector2Int] = resource;
			this.count.text = string.Concat(new string[]
			{
				this._entities.Count.ToString(),
				" E | ",
				this._resources.Count.ToString(),
				" R | ",
				this._tiles.Count.ToString(),
				" T"
			});
			string str = "[OUTPOST] Replaced resource at ";
			vector2Int2 = vector2Int;
			Debug.Log(str + vector2Int2.ToString() + " with new resource " + resource.FormattedName);
			return;
		}
		this._resources.Add(vector2Int, resource);
		string str2 = "[OUTPOST] Added new resource at ";
		vector2Int2 = vector2Int;
		Debug.Log(str2 + vector2Int2.ToString() + " with ID " + resource.FormattedName);
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x000431DC File Offset: 0x000413DC
	public void OnResourceRemoved(Vector3Int coords)
	{
		Vector2Int vector2Int = new Vector2Int(coords.x, coords.y);
		if (this._resources.ContainsKey(vector2Int))
		{
			this._resources.Remove(vector2Int);
			this.count.text = string.Concat(new string[]
			{
				this._entities.Count.ToString(),
				" E | ",
				this._resources.Count.ToString(),
				" R | ",
				this._tiles.Count.ToString(),
				" T"
			});
			string str = "[OUTPOST] Removed resource tile at ";
			Vector2Int vector2Int2 = vector2Int;
			Debug.Log(str + vector2Int2.ToString());
		}
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x000432AC File Offset: 0x000414AC
	public void OnTileDesignAdded(Vector3Int coords, TileDesign design)
	{
		Vector2Int vector2Int = new Vector2Int(coords.x, coords.y);
		Vector2Int vector2Int2;
		if (this._tiles.ContainsKey(vector2Int))
		{
			this._tiles[vector2Int] = design;
			this.count.text = string.Concat(new string[]
			{
				this._entities.Count.ToString(),
				" E | ",
				this._resources.Count.ToString(),
				" R | ",
				this._tiles.Count.ToString(),
				" T"
			});
			string str = "[OUTPOST] Replaced tile design at ";
			vector2Int2 = vector2Int;
			Debug.Log(str + vector2Int2.ToString() + " with new design " + design.data.FormattedName);
			return;
		}
		this._tiles.Add(vector2Int, design);
		string str2 = "[OUTPOST] Added new tile design at ";
		vector2Int2 = vector2Int;
		Debug.Log(str2 + vector2Int2.ToString() + " with design " + design.data.FormattedName);
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x000433C8 File Offset: 0x000415C8
	public void OnTileDesignRemoved(Vector3Int coords)
	{
		Vector2Int vector2Int = new Vector2Int(coords.x, coords.y);
		if (this._tiles.ContainsKey(vector2Int))
		{
			this._tiles.Remove(vector2Int);
			this.count.text = string.Concat(new string[]
			{
				this._entities.Count.ToString(),
				" E | ",
				this._resources.Count.ToString(),
				" R | ",
				this._tiles.Count.ToString(),
				" T"
			});
			string str = "[OUTPOST] Removed tile design at ";
			Vector2Int vector2Int2 = vector2Int;
			Debug.Log(str + vector2Int2.ToString());
		}
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x00043496 File Offset: 0x00041696
	public void Load()
	{
		this.Clear();
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x000434A0 File Offset: 0x000416A0
	public void Clear()
	{
		if (this._isEnabled)
		{
			Singleton<Events>.Instance.onEntityDestroyed.RemoveListener(new UnityAction<Entity>(this.OnEntityRemoved));
			Singleton<Events>.Instance.onResourceTileRemoved.RemoveListener(new UnityAction<Vector3Int>(this.OnResourceRemoved));
			Singleton<Events>.Instance.onTileDesignRemoved.RemoveListener(new UnityAction<Vector3Int>(this.OnTileDesignRemoved));
			foreach (KeyValuePair<ulong, Entity> keyValuePair in this._entities)
			{
				if (keyValuePair.Value != null)
				{
					Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(keyValuePair.Value, null), SyncType.ServerInitiated);
				}
			}
			foreach (KeyValuePair<Vector2Int, ResourceData> keyValuePair2 in this._resources)
			{
				Singleton<TileGrid>.Instance.ClearResourceTile(new Vector3Int(keyValuePair2.Key.x, keyValuePair2.Key.y, 0));
			}
			foreach (KeyValuePair<Vector2Int, TileDesign> keyValuePair3 in this._tiles)
			{
				Singleton<TileGrid>.Instance.ClearTileDesign(new Vector3Int(keyValuePair3.Key.x, keyValuePair3.Key.y, 0));
			}
			this._entities.Clear();
			this._resources.Clear();
			this._tiles.Clear();
			Singleton<Events>.Instance.onEntityDestroyed.AddListener(new UnityAction<Entity>(this.OnEntityRemoved));
			Singleton<Events>.Instance.onResourceTileRemoved.AddListener(new UnityAction<Vector3Int>(this.OnResourceRemoved));
			Singleton<Events>.Instance.onTileDesignRemoved.AddListener(new UnityAction<Vector3Int>(this.OnTileDesignRemoved));
			this.count.text = string.Concat(new string[]
			{
				this._entities.Count.ToString(),
				" E | ",
				this._resources.Count.ToString(),
				" R | ",
				this._tiles.Count.ToString(),
				" T"
			});
			Debug.Log("[CREATOR] Cleared all entities from registry!");
		}
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00043738 File Offset: 0x00041938
	public void Link()
	{
		int num = 0;
		foreach (KeyValuePair<ulong, Entity> keyValuePair in this._entities)
		{
			if (keyValuePair.Value != null)
			{
				this._activeCore.Entity.Link(keyValuePair.Value);
				num++;
			}
		}
		Debug.Log(string.Concat(new string[]
		{
			"[CREATOR] Linked ",
			num.ToString(),
			" entities out of ",
			this._entities.Count.ToString(),
			"!"
		}));
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x000437F8 File Offset: 0x000419F8
	public void Bake()
	{
		FactionData factionData = Library.RequestData<FactionData>(this.factionID.text);
		if (factionData == null)
		{
			Debug.Log("[CREATOR] No faction with ID " + this.factionID.text + " exists!");
			return;
		}
		int num = 0;
		Singleton<HeatManager>.Instance.SetFactionData(factionData);
		Accent accent = factionData.accent;
		foreach (KeyValuePair<ulong, Entity> keyValuePair in this._entities)
		{
			if (keyValuePair.Value != null)
			{
				keyValuePair.Value.GetModel.ApplyAccent(accent);
				num++;
			}
		}
		foreach (KeyValuePair<Vector2Int, TileDesign> keyValuePair2 in this._tiles)
		{
			TileDesign design = new TileDesign(keyValuePair2.Value.data, accent.secondaryColor);
			Singleton<TileGrid>.Instance.SetTileDesign(design, new Vector3Int(keyValuePair2.Key.x, keyValuePair2.Key.y, 0), false);
		}
		Debug.Log(string.Concat(new string[]
		{
			"[CREATOR] Baked ",
			num.ToString(),
			" entities out of ",
			this._entities.Count.ToString(),
			"!"
		}));
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0004398C File Offset: 0x00041B8C
	public void Export()
	{
		Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(this._activeCore.transform.position);
		this._newOutpostData = new OutpostData
		{
			Version = Application.version,
			ID = this.outpostID.text.ToLower().Replace(' ', '_'),
			Name = this.outpostID.text,
			Description = "Outpost data",
			entities = new List<OutpostData.Entity>(),
			resources = new Dictionary<string, List<Vector2Int>>(),
			tiles = new Dictionary<string, List<DecorationData>>()
		};
		foreach (KeyValuePair<ulong, Entity> keyValuePair in this._entities)
		{
			if (!keyValuePair.Value.IsSaveable)
			{
				Debug.Log("[CREATOR] Entity " + keyValuePair.Value.ID + " marked as not saveable!");
			}
			else
			{
				OutpostData.Entity item = new OutpostData.Entity
				{
					posX = keyValuePair.Value.transform.position.x - this._activeCore.transform.position.x,
					posY = keyValuePair.Value.transform.position.y - this._activeCore.transform.position.y,
					metadata = keyValuePair.Value.ExtractMetadata(false, MetadataContext.Local)
				};
				this._newOutpostData.entities.Add(item);
			}
		}
		foreach (KeyValuePair<Vector2Int, ResourceData> keyValuePair2 in this._resources)
		{
			if (!this._newOutpostData.resources.ContainsKey(keyValuePair2.Value.ID))
			{
				this._newOutpostData.resources.Add(keyValuePair2.Value.ID, new List<Vector2Int>());
			}
			Vector2Int item2 = new Vector2Int(keyValuePair2.Key.x - vector2Int.x, keyValuePair2.Key.y - vector2Int.y);
			this._newOutpostData.resources[keyValuePair2.Value.ID].Add(item2);
		}
		foreach (KeyValuePair<Vector2Int, TileDesign> keyValuePair3 in this._tiles)
		{
			if (!(keyValuePair3.Value.data == null))
			{
				if (!this._newOutpostData.tiles.ContainsKey(keyValuePair3.Value.data.ID))
				{
					this._newOutpostData.tiles.Add(keyValuePair3.Value.data.ID, new List<DecorationData>());
				}
				Vector2Int vector2Int2 = new Vector2Int(keyValuePair3.Key.x - vector2Int.x, keyValuePair3.Key.y - vector2Int.y);
				int tileColor = Utilities.ColorToInt(keyValuePair3.Value.color);
				int mapColor = Utilities.ColorToInt(keyValuePair3.Value.map);
				DecorationData item3 = new DecorationData(vector2Int2.x, vector2Int2.y, tileColor, mapColor);
				this._newOutpostData.tiles[keyValuePair3.Value.data.ID].Add(item3);
			}
		}
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x00043D70 File Offset: 0x00041F70
	public void PreviewOutpost()
	{
		if (this._newOutpostData == null)
		{
			return;
		}
		Vector2Int centerTilePosition = Singleton<WorldGenerator>.Instance.CenterTilePosition;
		Vector2Int tileCoords = centerTilePosition;
		switch (this._previewDirection)
		{
		case CardinalDirection.North:
			tileCoords = new Vector2Int(centerTilePosition.x, centerTilePosition.y + this._previewOffset);
			break;
		case CardinalDirection.East:
			tileCoords = new Vector2Int(centerTilePosition.x + this._previewOffset, centerTilePosition.y);
			break;
		case CardinalDirection.South:
			tileCoords = new Vector2Int(centerTilePosition.x, centerTilePosition.y - this._previewOffset);
			break;
		case CardinalDirection.West:
			tileCoords = new Vector2Int(centerTilePosition.x - this._previewOffset, centerTilePosition.y);
			break;
		}
		Singleton<Events>.Instance.onEntityCreated.RemoveListener(new UnityAction<Entity>(this.OnEntityAdded));
		Singleton<Events>.Instance.onResourceTilePlaced.RemoveListener(new UnityAction<Vector3Int, ResourceData>(this.OnResourceAdded));
		Singleton<Events>.Instance.onTileDesignPlaced.RemoveListener(new UnityAction<Vector3Int, TileDesign>(this.OnTileDesignAdded));
		Singleton<OutpostCreator>.Instance.CreateOutpost(this._newOutpostData, tileCoords);
		Singleton<Events>.Instance.onEntityCreated.AddListener(new UnityAction<Entity>(this.OnEntityAdded));
		Singleton<Events>.Instance.onResourceTilePlaced.AddListener(new UnityAction<Vector3Int, ResourceData>(this.OnResourceAdded));
		Singleton<Events>.Instance.onTileDesignPlaced.AddListener(new UnityAction<Vector3Int, TileDesign>(this.OnTileDesignAdded));
		if (this._previewDirection == CardinalDirection.West)
		{
			this._previewDirection = CardinalDirection.North;
			this._previewOffset += 50;
			return;
		}
		this._previewDirection++;
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x00043F0C File Offset: 0x0004210C
	public void ToggleWindow()
	{
		if (this._isOpen)
		{
			this.button.group.GetComponent<RectTransform>().localPosition = this.button.normalPos;
			if (this.button.sound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.button.sound);
			}
			LeanTween.moveLocal(this.button.group.gameObject, this.button.outPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
		}
		else
		{
			this.button.group.GetComponent<RectTransform>().localPosition = this.button.inPos;
			if (this.button.sound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.button.sound);
			}
			LeanTween.moveLocal(this.button.group.gameObject, this.button.normalPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
		}
		this._isOpen = !this._isOpen;
		this.arrowOne.SetActive(this._isOpen);
		this.arrowTwo.SetActive(!this._isOpen);
	}

	// Token: 0x04000BC1 RID: 3009
	public const string EXPORT_PATH = "Assets/Vectorio/Exports";

	// Token: 0x04000BC2 RID: 3010
	public const string FILE_EXTENSION = ".bytes";

	// Token: 0x04000BC3 RID: 3011
	public MenuButton button;

	// Token: 0x04000BC4 RID: 3012
	public GameObject arrowOne;

	// Token: 0x04000BC5 RID: 3013
	public GameObject arrowTwo;

	// Token: 0x04000BC6 RID: 3014
	private bool _isOpen = true;

	// Token: 0x04000BC7 RID: 3015
	public string coreID;

	// Token: 0x04000BC8 RID: 3016
	public TMP_InputField loadID;

	// Token: 0x04000BC9 RID: 3017
	public TMP_InputField factionID;

	// Token: 0x04000BCA RID: 3018
	public TMP_InputField outpostID;

	// Token: 0x04000BCB RID: 3019
	public TextMeshProUGUI count;

	// Token: 0x04000BCC RID: 3020
	private CanvasGroup _canvasGroup;

	// Token: 0x04000BCD RID: 3021
	private int _previewOffset = 50;

	// Token: 0x04000BCE RID: 3022
	private CardinalDirection _previewDirection;

	// Token: 0x04000BCF RID: 3023
	private Dictionary<ulong, Entity> _entities = new Dictionary<ulong, Entity>();

	// Token: 0x04000BD0 RID: 3024
	private Dictionary<Vector2Int, ResourceData> _resources = new Dictionary<Vector2Int, ResourceData>();

	// Token: 0x04000BD1 RID: 3025
	private Dictionary<Vector2Int, TileDesign> _tiles = new Dictionary<Vector2Int, TileDesign>();

	// Token: 0x04000BD2 RID: 3026
	private bool _isEnabled;

	// Token: 0x04000BD3 RID: 3027
	protected OutpostData _newOutpostData;

	// Token: 0x04000BD4 RID: 3028
	protected OutpostCore _activeCore;

	// Token: 0x04000BD5 RID: 3029
	protected OutpostCore _previewCore;
}
