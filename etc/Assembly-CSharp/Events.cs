using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vectorio;

// Token: 0x02000211 RID: 529
[DefaultExecutionOrder(-1)]
public class Events : Singleton<Events>
{
	// Token: 0x04000D83 RID: 3459
	[HideInInspector]
	public UnityEvent onFinsihLoading;

	// Token: 0x04000D84 RID: 3460
	[HideInInspector]
	public UnityEvent onRegionGoalCompleted;

	// Token: 0x04000D85 RID: 3461
	[HideInInspector]
	public UnityEvent<Gateway> onGatewayClicked;

	// Token: 0x04000D86 RID: 3462
	[HideInInspector]
	public UnityEvent onPauseStateUpdated;

	// Token: 0x04000D87 RID: 3463
	[HideInInspector]
	public UnityEvent<Blueprint> onBlueprintAdded;

	// Token: 0x04000D88 RID: 3464
	[HideInInspector]
	public UnityEvent<Blueprint> onBlueprintRemoved;

	// Token: 0x04000D89 RID: 3465
	[HideInInspector]
	public UnityEvent<Gateway> onJumpSequenceStarted;

	// Token: 0x04000D8A RID: 3466
	[HideInInspector]
	public UnityEvent onJumpSequenceFinished;

	// Token: 0x04000D8B RID: 3467
	[HideInInspector]
	public UnityEvent<Entity> onEntityCreated;

	// Token: 0x04000D8C RID: 3468
	[HideInInspector]
	public UnityEvent<Entity> onEntityClicked;

	// Token: 0x04000D8D RID: 3469
	[HideInInspector]
	public UnityEvent<Entity> onEntityDestroyed;

	// Token: 0x04000D8E RID: 3470
	[HideInInspector]
	public UnityEvent<Building, float> onBuildingDamaged;

	// Token: 0x04000D8F RID: 3471
	[HideInInspector]
	public UnityEvent<Building> onBuildingDestroyed;

	// Token: 0x04000D90 RID: 3472
	[HideInInspector]
	public UnityEvent<ResourceData, int> onResourceUpdated;

	// Token: 0x04000D91 RID: 3473
	[HideInInspector]
	public UnityEvent<int> onTileClaimUpdated;

	// Token: 0x04000D92 RID: 3474
	[HideInInspector]
	public UnityEvent<string, bool> onStartTutorial;

	// Token: 0x04000D93 RID: 3475
	[HideInInspector]
	public UnityEvent onUnlockAllTutorials;

	// Token: 0x04000D94 RID: 3476
	[HideInInspector]
	public UnityEvent<EntityData> onInventoryButtonClicked;

	// Token: 0x04000D95 RID: 3477
	[HideInInspector]
	public UnityEvent<HotbarElement> onHotbarElementUsed;

	// Token: 0x04000D96 RID: 3478
	[HideInInspector]
	public UnityEvent<float> onInterfaceScaleChanged;

	// Token: 0x04000D97 RID: 3479
	[HideInInspector]
	public UnityEvent<Transform> onMoveCameraToTarget;

	// Token: 0x04000D98 RID: 3480
	[HideInInspector]
	public UnityEvent<Vector2> onMoveCameraToPosition;

	// Token: 0x04000D99 RID: 3481
	[HideInInspector]
	public UnityEvent<Vector2> onEnemyGroupSpawned;

	// Token: 0x04000D9A RID: 3482
	[HideInInspector]
	public UnityEvent onStartDraggingCamera;

	// Token: 0x04000D9B RID: 3483
	[HideInInspector]
	public UnityEvent onStopDraggingCamera;

	// Token: 0x04000D9C RID: 3484
	[HideInInspector]
	public UnityEvent<Hologram.BuildMode> onChangeBuildMode;

	// Token: 0x04000D9D RID: 3485
	[HideInInspector]
	public UnityEvent<Hologram.BuildMode> onBuildModeChanged;

	// Token: 0x04000D9E RID: 3486
	[HideInInspector]
	public UnityEvent<Vector3Int, TileDesign> onTileDesignPlaced;

	// Token: 0x04000D9F RID: 3487
	[HideInInspector]
	public UnityEvent<Vector3Int, TileDesign> onTileDesignUpdate;

	// Token: 0x04000DA0 RID: 3488
	[HideInInspector]
	public UnityEvent<Vector3Int> onTileDesignRemoved;

	// Token: 0x04000DA1 RID: 3489
	[HideInInspector]
	public UnityEvent<Vector3Int, ResourceData> onResourceTilePlaced;

	// Token: 0x04000DA2 RID: 3490
	[HideInInspector]
	public UnityEvent<Vector3Int> onResourceTileRemoved;

	// Token: 0x04000DA3 RID: 3491
	[HideInInspector]
	public UnityEvent<Decryptor> onOpenDecryption;

	// Token: 0x04000DA4 RID: 3492
	[HideInInspector]
	public UnityEvent<Decryptor> onDecryptionStarted;

	// Token: 0x04000DA5 RID: 3493
	[HideInInspector]
	public UnityEvent<Decryptor> onDecryptionFailed;

	// Token: 0x04000DA6 RID: 3494
	[HideInInspector]
	public UnityEvent<Decryptor> onDecryptionFinished;

	// Token: 0x04000DA7 RID: 3495
	[HideInInspector]
	public UnityEvent onOpenPause;

	// Token: 0x04000DA8 RID: 3496
	[HideInInspector]
	public UnityEvent onClosePause;

	// Token: 0x04000DA9 RID: 3497
	[HideInInspector]
	public UnityEvent<InputController> onControllerReady;

	// Token: 0x04000DAA RID: 3498
	[HideInInspector]
	public UnityEvent onDisableActionMap;

	// Token: 0x04000DAB RID: 3499
	[HideInInspector]
	public UnityEvent<bool> onToggleGrid;

	// Token: 0x04000DAC RID: 3500
	[HideInInspector]
	public UnityEvent<EntityData> onAddEntityToInventory;

	// Token: 0x04000DAD RID: 3501
	[HideInInspector]
	public UnityEvent<EntityData> onEntityUnlocked;

	// Token: 0x04000DAE RID: 3502
	[HideInInspector]
	public UnityEvent<Category> onSortCategory;

	// Token: 0x04000DAF RID: 3503
	[HideInInspector]
	public UnityEvent onSortAllCategories;

	// Token: 0x04000DB0 RID: 3504
	[HideInInspector]
	public UnityEvent<Guardian> onGuardianActivated;

	// Token: 0x04000DB1 RID: 3505
	[HideInInspector]
	public UnityEvent<Guardian> onGuardianDeactivated;

	// Token: 0x04000DB2 RID: 3506
	[HideInInspector]
	public UnityEvent<Guardian> onGuardianDestroyed;

	// Token: 0x04000DB3 RID: 3507
	[HideInInspector]
	public UnityEvent<int> onHeatAmountUpdated;

	// Token: 0x04000DB4 RID: 3508
	[HideInInspector]
	public UnityEvent<int, int> onHeatThresholdUpdated;

	// Token: 0x04000DB5 RID: 3509
	[HideInInspector]
	public UnityEvent onHeatLimitReached;

	// Token: 0x04000DB6 RID: 3510
	[HideInInspector]
	public UnityEvent onPowerExceeded;

	// Token: 0x04000DB7 RID: 3511
	[HideInInspector]
	public UnityEvent onPowerRecovered;

	// Token: 0x04000DB8 RID: 3512
	[HideInInspector]
	public UnityEvent<EntityData> onEntityEquipped;

	// Token: 0x04000DB9 RID: 3513
	[HideInInspector]
	public UnityEvent onEntityUnequip;

	// Token: 0x04000DBA RID: 3514
	[HideInInspector]
	public UnityEvent<FactionData> onChangeFaction;

	// Token: 0x04000DBB RID: 3515
	[HideInInspector]
	public UnityEvent<EntityData> openInventoryToEntity;

	// Token: 0x04000DBC RID: 3516
	[HideInInspector]
	public UnityEvent<int> onCategorySelected;

	// Token: 0x04000DBD RID: 3517
	[HideInInspector]
	public UnityEvent<ulong> onSectorEntityDestroyed;

	// Token: 0x04000DBE RID: 3518
	[HideInInspector]
	public UnityEvent<InputActions> onUpdatedKeybinds;

	// Token: 0x04000DBF RID: 3519
	[HideInInspector]
	public UnityEvent onMusicVolumeChanged;

	// Token: 0x04000DC0 RID: 3520
	[HideInInspector]
	public UnityEvent<ResearchTechData> onResearchTechAvailable;

	// Token: 0x04000DC1 RID: 3521
	[HideInInspector]
	public UnityEvent<ResearchTechData> onResearchTechClicked;

	// Token: 0x04000DC2 RID: 3522
	[HideInInspector]
	public UnityEvent<ResearchTechData, List<Cost>> onResearchTechActivated;

	// Token: 0x04000DC3 RID: 3523
	[HideInInspector]
	public UnityEvent<ResearchTechData> onResearchTechFinished;

	// Token: 0x04000DC4 RID: 3524
	[HideInInspector]
	public UnityEvent<RecipeData> onRecipeUnlocked;

	// Token: 0x04000DC5 RID: 3525
	[HideInInspector]
	public UnityEvent<string, int> onResearchResourceTechAdded;

	// Token: 0x04000DC6 RID: 3526
	[HideInInspector]
	public UnityEvent onResearchUnlockAccepted;

	// Token: 0x04000DC7 RID: 3527
	[HideInInspector]
	public UnityEvent onInventoryOpen;

	// Token: 0x04000DC8 RID: 3528
	[HideInInspector]
	public UnityEvent onInventoryClose;
}
