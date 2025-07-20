using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Grid;
using Vectorio.Serialization;
using Vectorio.Utilities;

// Token: 0x020001D0 RID: 464
[DefaultExecutionOrder(10)]
public class MenuScene : Singleton<MenuScene>
{
	// Token: 0x06000E8F RID: 3727 RVA: 0x000418CC File Offset: 0x0003FACC
	public void Start()
	{
		this._colorPalette = new List<Color>();
		this.sceneTilePosition = Utilities.ConvertWorldPositionToCell(this.scenePosition);
		for (float num = 0.95f; num < 1f; num += 0.01f)
		{
			this._colorPalette.Add(new Color(this.tileColor.r * num, this.tileColor.g * num, this.tileColor.b * num, 1f));
		}
		for (int i = -25; i < 25; i++)
		{
			for (int j = -15; j < 15; j++)
			{
				TileDesign design = new TileDesign(this.tile, this._colorPalette[Random.Range(0, this._colorPalette.Count)]);
				this.tileGrid.SetTileDesign(design, new Vector3Int(this.sceneTilePosition.x + i, this.sceneTilePosition.y + j, 0), true);
			}
		}
		ServerSingleton<ServerSyncManager>.Instance.ClearIDMap();
		OutpostData outpostData = DataProcessor.DecompressAndDeserializeObject<OutpostData>(this.outpostToSpawn.bytes);
		this._entitesToSpawn = new Stack<OutpostData.Entity>(outpostData.entities);
		foreach (KeyValuePair<string, List<Vector2Int>> keyValuePair in outpostData.resources)
		{
			ResourceData resourceData = Library.RequestData<ResourceData>(keyValuePair.Key);
			if (resourceData != null)
			{
				foreach (Vector2Int b in keyValuePair.Value)
				{
					MenuScene.ResourceToSpawn item = new MenuScene.ResourceToSpawn
					{
						Data = resourceData,
						TilePos = this.sceneTilePosition + b
					};
					this._resourcesToSpawn.Push(item);
				}
			}
		}
		foreach (KeyValuePair<string, List<DecorationData>> keyValuePair2 in outpostData.tiles)
		{
			TileDesignData x = Library.RequestData<TileDesignData>(keyValuePair2.Key);
			if (x != null)
			{
				foreach (DecorationData data in keyValuePair2.Value)
				{
					MenuScene.TileToSpawn tileToSpawn = new MenuScene.TileToSpawn
					{
						Tile = x,
						Data = data
					};
					tileToSpawn.Data.X += this.sceneTilePosition.x;
					tileToSpawn.Data.Y += this.sceneTilePosition.y;
					this._tilesToSpawn.Push(tileToSpawn);
				}
			}
		}
		if (this.useFog)
		{
			Singleton<FogOfWarHandler>.Instance.Enable(this.regionToReference, Vector2.zero, new Vector2(1000f, 1000f));
		}
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x00041BDC File Offset: 0x0003FDDC
	public void Update()
	{
		if (!this.startPlacing)
		{
			return;
		}
		if (this._outpostSpawned)
		{
			using (List<MenuScene.MenuEnemy>.Enumerator enumerator = this.menuEnemies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MenuScene.MenuEnemy menuEnemy = enumerator.Current;
					if (menuEnemy.Timer(Time.deltaTime) <= 0f)
					{
						menuEnemy.ResetTimer();
						Vector2 position = this.scenePosition + this.GetRandomDirection() * 5f;
						EntityCreationData creationData = EventBuilder.BuildCreationData(menuEnemy.data.ID, menuEnemy.factionID, position, SyncType.None);
						EventBuilder.ApplyAccentToCreationData(ref creationData, new AccentData(this.enemyFaction.accent));
						EventBuilder.ApplyCallbackToCreationData(ref creationData, CallbackType.ManagerCallback, 7U, 0);
						Singleton<EntityManager>.Instance.QueueCreationEvent(creationData);
					}
				}
				return;
			}
		}
		if (this._buildingTimer > 0f)
		{
			this._buildingTimer -= Time.deltaTime;
			return;
		}
		if (this._resourcesToSpawn.Count > 0)
		{
			this._buildingTimer = this._decorationCooldown;
			MenuScene.ResourceToSpawn resourceToSpawn = this._resourcesToSpawn.Pop();
			Vector3Int coords = new Vector3Int(resourceToSpawn.TilePos.x, resourceToSpawn.TilePos.y, 0);
			Singleton<TileGrid>.Instance.SetResourceTile(coords, resourceToSpawn.Data);
			return;
		}
		if (this._entitesToSpawn.Count > 0)
		{
			this._buildingTimer = this._buildingCooldown;
			OutpostData.Entity entity = this._entitesToSpawn.Pop();
			EntityCreationData entityCreationData;
			if (EventBuilder.BuildCreationDataWithMetadata(entity.metadata, out entityCreationData, this.buildingFaction.ID, entity.posX + this.scenePosition.x, entity.posY + this.scenePosition.y))
			{
				entityCreationData.FromSave = true;
				Singleton<EntityManager>.Instance.QueueCreationEvent(entityCreationData);
			}
			return;
		}
		if (this._tilesToSpawn.Count > 0)
		{
			this._buildingTimer = this._decorationCooldown;
			MenuScene.TileToSpawn tileToSpawn = this._tilesToSpawn.Pop();
			TileDesign design = new TileDesign(tileToSpawn.Tile, Utilities.IntToColor(tileToSpawn.Data.TileColor), Utilities.IntToColor(tileToSpawn.Data.MapColor));
			Singleton<TileGrid>.Instance.SetTileDesign(design, new Vector3Int(tileToSpawn.Data.X, tileToSpawn.Data.Y, 0), false);
			return;
		}
		this._outpostSpawned = true;
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x00041E40 File Offset: 0x00040040
	public void OnEntityCreated(Entity entity, byte type)
	{
		if (type == 0 && entity.Has_EComponent<Unit>())
		{
			Unit unit = entity.Get_EComponent<Unit>(false);
			unit.SetTargetPosition(this.scenePosition);
			unit.SetBehaviour(Unit.Behaviour.Normal);
			unit.FaceTarget();
		}
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x00041E6C File Offset: 0x0004006C
	protected Vector2 GetRandomDirection()
	{
		switch (Random.Range(0, 5))
		{
		case 0:
			return new Vector2((float)Random.Range(-25, 25), 15f);
		case 1:
			return new Vector2(25f, (float)Random.Range(-15, 15));
		case 2:
			return new Vector2((float)Random.Range(-25, 25), -15f);
		default:
			return new Vector2(-25f, (float)Random.Range(-15, 15));
		}
	}

	// Token: 0x04000B7C RID: 2940
	public bool startPlacing;

	// Token: 0x04000B7D RID: 2941
	protected const int VERTICAL_TILE_DISTANCE = 15;

	// Token: 0x04000B7E RID: 2942
	protected const int HORIZONTAL_TILE_DISTANCE = 25;

	// Token: 0x04000B7F RID: 2943
	protected List<Color> _colorPalette;

	// Token: 0x04000B80 RID: 2944
	public bool useFog;

	// Token: 0x04000B81 RID: 2945
	public RegionData regionToReference;

	// Token: 0x04000B82 RID: 2946
	public FactionData buildingFaction;

	// Token: 0x04000B83 RID: 2947
	private List<string> _values;

	// Token: 0x04000B84 RID: 2948
	public Vector2 scenePosition;

	// Token: 0x04000B85 RID: 2949
	private Vector2Int sceneTilePosition;

	// Token: 0x04000B86 RID: 2950
	public TextAsset outpostToSpawn;

	// Token: 0x04000B87 RID: 2951
	private Stack<OutpostData.Entity> _entitesToSpawn = new Stack<OutpostData.Entity>();

	// Token: 0x04000B88 RID: 2952
	private Stack<MenuScene.ResourceToSpawn> _resourcesToSpawn = new Stack<MenuScene.ResourceToSpawn>();

	// Token: 0x04000B89 RID: 2953
	private Stack<MenuScene.TileToSpawn> _tilesToSpawn = new Stack<MenuScene.TileToSpawn>();

	// Token: 0x04000B8A RID: 2954
	private bool _outpostSpawned;

	// Token: 0x04000B8B RID: 2955
	public FactionData enemyFaction;

	// Token: 0x04000B8C RID: 2956
	public List<MenuScene.MenuEnemy> menuEnemies;

	// Token: 0x04000B8D RID: 2957
	public Transform menuTarget;

	// Token: 0x04000B8E RID: 2958
	public Hub hub;

	// Token: 0x04000B8F RID: 2959
	public TileDesignData tile;

	// Token: 0x04000B90 RID: 2960
	public InfiniteGrid tileGrid;

	// Token: 0x04000B91 RID: 2961
	public Color tileColor;

	// Token: 0x04000B92 RID: 2962
	protected float _buildingCooldown = 0.005f;

	// Token: 0x04000B93 RID: 2963
	protected float _buildingTimer;

	// Token: 0x04000B94 RID: 2964
	protected float _decorationCooldown = 0.005f;

	// Token: 0x04000B95 RID: 2965
	protected float _decorationTimer;

	// Token: 0x020001D1 RID: 465
	[Serializable]
	public class MenuEnemy
	{
		// Token: 0x06000E94 RID: 3732 RVA: 0x00041F28 File Offset: 0x00040128
		public float Timer(float time)
		{
			return this.timer -= time;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00041F46 File Offset: 0x00040146
		public void ResetTimer()
		{
			this.timer = this.cooldown;
		}

		// Token: 0x04000B96 RID: 2966
		public EntityData data;

		// Token: 0x04000B97 RID: 2967
		public string factionID;

		// Token: 0x04000B98 RID: 2968
		public float cooldown;

		// Token: 0x04000B99 RID: 2969
		protected float timer;
	}

	// Token: 0x020001D2 RID: 466
	public class ResourceToSpawn
	{
		// Token: 0x04000B9A RID: 2970
		public ResourceData Data;

		// Token: 0x04000B9B RID: 2971
		public Vector2Int TilePos;
	}

	// Token: 0x020001D3 RID: 467
	public class TileToSpawn
	{
		// Token: 0x04000B9C RID: 2972
		public TileDesignData Tile;

		// Token: 0x04000B9D RID: 2973
		public DecorationData Data;

		// Token: 0x04000B9E RID: 2974
		public Vector2Int Position;
	}
}
