using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
[CreateAssetMenu(fileName = "New Resource", menuName = "Vectorio/Resource")]
public class ResourceData : BaseData
{
	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00020458 File Offset: 0x0001E658
	public TileDesignData Tile
	{
		get
		{
			return this.tile;
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00020460 File Offset: 0x0001E660
	public Sprite IconSprite
	{
		get
		{
			return this.iconSprite;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00020468 File Offset: 0x0001E668
	public bool UseUISprite
	{
		get
		{
			return this.useUISprite;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00020470 File Offset: 0x0001E670
	public Sprite IconUISprite
	{
		get
		{
			return this.iconUISprite;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00020478 File Offset: 0x0001E678
	public Accent Accent
	{
		get
		{
			return this.accent;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060006D6 RID: 1750 RVA: 0x00020480 File Offset: 0x0001E680
	public int Tier
	{
		get
		{
			return this.tier;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00020488 File Offset: 0x0001E688
	public string PostFix
	{
		get
		{
			return this.postfix;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00020490 File Offset: 0x0001E690
	public int Order
	{
		get
		{
			return this.order;
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00020498 File Offset: 0x0001E698
	public int Power
	{
		get
		{
			return this.power;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060006DA RID: 1754 RVA: 0x000204A0 File Offset: 0x0001E6A0
	public float BurnTime
	{
		get
		{
			return this.burnTime;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060006DB RID: 1755 RVA: 0x000204A8 File Offset: 0x0001E6A8
	public bool CanBeStoredGlobally
	{
		get
		{
			return this.canBeStoredGlobally;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060006DC RID: 1756 RVA: 0x000204B0 File Offset: 0x0001E6B0
	public bool UseSounds
	{
		get
		{
			return this.useSounds;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x060006DD RID: 1757 RVA: 0x000204B8 File Offset: 0x0001E6B8
	public AudioClip PickupSound
	{
		get
		{
			return this.pickupSound;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060006DE RID: 1758 RVA: 0x000204C0 File Offset: 0x0001E6C0
	public AudioClip DropoffSound
	{
		get
		{
			return this.dropoffSound;
		}
	}

	// Token: 0x04000482 RID: 1154
	[SerializeField]
	private TileDesignData tile;

	// Token: 0x04000483 RID: 1155
	[SerializeField]
	private Sprite iconSprite;

	// Token: 0x04000484 RID: 1156
	[SerializeField]
	private bool useUISprite;

	// Token: 0x04000485 RID: 1157
	[SerializeField]
	private Sprite iconUISprite;

	// Token: 0x04000486 RID: 1158
	[SerializeField]
	private Accent accent;

	// Token: 0x04000487 RID: 1159
	[SerializeField]
	[Range(1f, 3f)]
	private int tier;

	// Token: 0x04000488 RID: 1160
	[SerializeField]
	private string postfix = "";

	// Token: 0x04000489 RID: 1161
	[SerializeField]
	private int order;

	// Token: 0x0400048A RID: 1162
	[SerializeField]
	private int power;

	// Token: 0x0400048B RID: 1163
	[SerializeField]
	private float burnTime = 1f;

	// Token: 0x0400048C RID: 1164
	[SerializeField]
	private bool canBeStoredGlobally = true;

	// Token: 0x0400048D RID: 1165
	[SerializeField]
	private bool useSounds;

	// Token: 0x0400048E RID: 1166
	[SerializeField]
	private AudioClip pickupSound;

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	private AudioClip dropoffSound;
}
