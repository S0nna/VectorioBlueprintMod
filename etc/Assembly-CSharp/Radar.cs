using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class Radar : EntityComponent, IComponent<Radar, RadarData>, IUpdateable
{
	// Token: 0x060009BF RID: 2495 RVA: 0x00029103 File Offset: 0x00027303
	public RadarData GetData()
	{
		return this._radarData;
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0002910B File Offset: 0x0002730B
	// (set) Token: 0x060009C1 RID: 2497 RVA: 0x00029113 File Offset: 0x00027313
	public Rotator RotatorPivot
	{
		get
		{
			return this._rotatorPivot;
		}
		set
		{
			this._rotatorPivot = value;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0002911C File Offset: 0x0002731C
	public void OnInitialize(RadarData data)
	{
		this._radarData = data;
		this._rotatorPivot = new GameObject("Rotator").AddComponent<Rotator>();
		this._rotatorPivot.transform.SetParent(base.transform);
		this._rotatorPivot.SetSpeed(data.rotationSpeed);
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00003212 File Offset: 0x00001412
	public void Tick(float time)
	{
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStartUpdating()
	{
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00003212 File Offset: 0x00001412
	public void OnStopUpdating()
	{
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00003212 File Offset: 0x00001412
	public override void OnReset()
	{
	}

	// Token: 0x040005FE RID: 1534
	private RadarData _radarData;

	// Token: 0x040005FF RID: 1535
	protected Rotator _rotatorPivot;
}
