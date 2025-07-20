using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020000A4 RID: 164
public class BuildingData : ComponentData<Building>
{
	// Token: 0x0600065C RID: 1628 RVA: 0x0001F954 File Offset: 0x0001DB54
	public override void ApplyData(Building component)
	{
		component.OnInitialize(this);
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001F95D File Offset: 0x0001DB5D
	public int Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001F965 File Offset: 0x0001DB65
	public int Height
	{
		get
		{
			return this.height;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001F96D File Offset: 0x0001DB6D
	public bool RequireAnyResource
	{
		get
		{
			return this.requireAnyResource;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001F975 File Offset: 0x0001DB75
	public List<string> RequiredResources
	{
		get
		{
			return this.requiredResources;
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
	public override List<Stat> RetrieveStats()
	{
		return new List<Stat>();
	}

	// Token: 0x040003D2 RID: 978
	[SerializeField]
	private int width;

	// Token: 0x040003D3 RID: 979
	[SerializeField]
	private int height;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	private bool requireAnyResource;

	// Token: 0x040003D5 RID: 981
	[SerializeField]
	private List<string> requiredResources;

	// Token: 0x040003D6 RID: 982
	public bool useDeathAnimation = true;

	// Token: 0x040003D7 RID: 983
	public bool useUniqueAnimations;

	// Token: 0x040003D8 RID: 984
	public ParticleSystem deathParticle;

	// Token: 0x040003D9 RID: 985
	public AudioClip deathSound;
}
