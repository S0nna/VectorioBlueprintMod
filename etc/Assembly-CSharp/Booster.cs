using System;
using Vectorio.Stats;

// Token: 0x020000F7 RID: 247
public class Booster : BuildingComponent, IComponent<Booster, BoosterData>
{
	// Token: 0x060007CC RID: 1996 RVA: 0x00022AD9 File Offset: 0x00020CD9
	public BoosterData GetData()
	{
		return this._data;
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00022AE1 File Offset: 0x00020CE1
	public void OnInitialize(BoosterData data)
	{
		this._boostRange = data.range;
		this._variableID = data.variableID;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00003212 File Offset: 0x00001412
	public override void OnSpawn(bool fromSave)
	{
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00003212 File Offset: 0x00001412
	public override void OnReset()
	{
	}

	// Token: 0x04000531 RID: 1329
	protected BoosterData _data;

	// Token: 0x04000532 RID: 1330
	protected StatModifier _modifier;

	// Token: 0x04000533 RID: 1331
	protected int _boostRange;

	// Token: 0x04000534 RID: 1332
	protected string _variableID;
}
