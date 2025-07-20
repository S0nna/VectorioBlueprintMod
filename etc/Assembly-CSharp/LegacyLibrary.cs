using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000068 RID: 104
[DefaultExecutionOrder(0)]
public class LegacyLibrary : MonoBehaviour
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x0001A23C File Offset: 0x0001843C
	public void Awake()
	{
		LegacyLibrary.MENU_VOLUME_PROFILE = this.menuVolumeProfile;
		LegacyLibrary.MAIN_VOLUME_PROFILE = this.mainVolumeProfile;
		LegacyLibrary.HIVE_SPRITE_ICON = this.hiveSpriteIcon;
		LegacyLibrary.HIVE_PARTICLE_EFFECT = this.hiveParticleEffect;
		LegacyLibrary.UNIT_DEATH_PARTICLE = this.unitDeathParticle;
		LegacyLibrary.UNIT_DEATH_SOUND = this.unitDeathSound;
		LegacyLibrary.BUILDING_DEATH_PARTICLE = this.buildingDeathParticle;
		LegacyLibrary.BUILDING_DEATH_SOUND = this.buildingDeathSound;
		LegacyLibrary.CIRCLE_RANGE_SPRITE = this.circleRangeSprite;
		LegacyLibrary.SQUARE_RANGE_SPRITE = this.squareRangeSprite;
		LegacyLibrary.UNIT_ICON_MARKER = this.unitIconMarker;
		LegacyLibrary.RECLAIMER_PREVIEW_PREFAB = this.reclaimerPreviewPrefab;
		LegacyLibrary.DEBUG_MARKER_ONE = this.debugMarkerOne;
		LegacyLibrary.DEBUG_MARKER_TWO = this.debugMarkerTwo;
		LegacyLibrary.DEBUG_MARKER_THREE = this.debugMarketThree;
		LegacyLibrary.OUTPUT_RESOURCE_PREFAB = this.outputResourcePrefab;
		LegacyLibrary.ENTITY_STATUS_PREFAB = this.turretAmmoPrefab;
		LegacyLibrary.PLACEMENT_SOUND = this.placementSound;
		LegacyLibrary.POWER_ICON = this.powerIcon;
		LegacyLibrary.AMMO_ICON = this.ammoIcon;
		LegacyLibrary.NO_FILTER = this.noFilter;
		LegacyLibrary.DRONE_PREVIEW_SPRITE = this.dronePreviewSprite;
		LegacyLibrary.DRONE_PREVIEW_MATERIAL = this.dronePreviewMaterial;
		LegacyLibrary.DECRYPTION_START_SOUND = this.decryptionStartSound;
	}

	// Token: 0x040002AE RID: 686
	public static Volume MENU_VOLUME_PROFILE;

	// Token: 0x040002AF RID: 687
	public static Volume MAIN_VOLUME_PROFILE;

	// Token: 0x040002B0 RID: 688
	public static Sprite HIVE_SPRITE_ICON;

	// Token: 0x040002B1 RID: 689
	public static ParticleSystem HIVE_PARTICLE_EFFECT;

	// Token: 0x040002B2 RID: 690
	public static ParticleSystem UNIT_DEATH_PARTICLE;

	// Token: 0x040002B3 RID: 691
	public static AudioClip UNIT_DEATH_SOUND;

	// Token: 0x040002B4 RID: 692
	public static ParticleSystem BUILDING_DEATH_PARTICLE;

	// Token: 0x040002B5 RID: 693
	public static AudioClip BUILDING_DEATH_SOUND;

	// Token: 0x040002B6 RID: 694
	public static Sprite CIRCLE_RANGE_SPRITE;

	// Token: 0x040002B7 RID: 695
	public static Sprite SQUARE_RANGE_SPRITE;

	// Token: 0x040002B8 RID: 696
	public static GameObject UNIT_ICON_MARKER;

	// Token: 0x040002B9 RID: 697
	public static GameObject RECLAIMER_PREVIEW_PREFAB;

	// Token: 0x040002BA RID: 698
	public static GameObject DEBUG_MARKER_ONE;

	// Token: 0x040002BB RID: 699
	public static GameObject DEBUG_MARKER_TWO;

	// Token: 0x040002BC RID: 700
	public static GameObject DEBUG_MARKER_THREE;

	// Token: 0x040002BD RID: 701
	public static SpriteRenderer OUTPUT_RESOURCE_PREFAB;

	// Token: 0x040002BE RID: 702
	public static SpriteRenderer ENTITY_STATUS_PREFAB;

	// Token: 0x040002BF RID: 703
	public static AudioClip PLACEMENT_SOUND;

	// Token: 0x040002C0 RID: 704
	public static Sprite POWER_ICON;

	// Token: 0x040002C1 RID: 705
	public static Sprite AMMO_ICON;

	// Token: 0x040002C2 RID: 706
	public static Sprite NO_FILTER;

	// Token: 0x040002C3 RID: 707
	public static Sprite DRONE_PREVIEW_SPRITE;

	// Token: 0x040002C4 RID: 708
	public static Material DRONE_PREVIEW_MATERIAL;

	// Token: 0x040002C5 RID: 709
	public static AudioClip DECRYPTION_START_SOUND;

	// Token: 0x040002C6 RID: 710
	public Volume menuVolumeProfile;

	// Token: 0x040002C7 RID: 711
	public Volume mainVolumeProfile;

	// Token: 0x040002C8 RID: 712
	public Sprite hiveSpriteIcon;

	// Token: 0x040002C9 RID: 713
	public ParticleSystem hiveParticleEffect;

	// Token: 0x040002CA RID: 714
	public ParticleSystem unitDeathParticle;

	// Token: 0x040002CB RID: 715
	public AudioClip unitDeathSound;

	// Token: 0x040002CC RID: 716
	public ParticleSystem buildingDeathParticle;

	// Token: 0x040002CD RID: 717
	public AudioClip buildingDeathSound;

	// Token: 0x040002CE RID: 718
	public Sprite circleRangeSprite;

	// Token: 0x040002CF RID: 719
	public Sprite squareRangeSprite;

	// Token: 0x040002D0 RID: 720
	public GameObject unitIconMarker;

	// Token: 0x040002D1 RID: 721
	public GameObject reclaimerPreviewPrefab;

	// Token: 0x040002D2 RID: 722
	public GameObject debugMarkerOne;

	// Token: 0x040002D3 RID: 723
	public GameObject debugMarkerTwo;

	// Token: 0x040002D4 RID: 724
	public GameObject debugMarketThree;

	// Token: 0x040002D5 RID: 725
	public SpriteRenderer outputResourcePrefab;

	// Token: 0x040002D6 RID: 726
	public SpriteRenderer turretAmmoPrefab;

	// Token: 0x040002D7 RID: 727
	public AudioClip placementSound;

	// Token: 0x040002D8 RID: 728
	public Sprite powerIcon;

	// Token: 0x040002D9 RID: 729
	public Sprite ammoIcon;

	// Token: 0x040002DA RID: 730
	public Sprite noFilter;

	// Token: 0x040002DB RID: 731
	public Sprite dronePreviewSprite;

	// Token: 0x040002DC RID: 732
	public Material dronePreviewMaterial;

	// Token: 0x040002DD RID: 733
	public AudioClip decryptionStartSound;
}
