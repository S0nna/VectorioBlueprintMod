using System;

// Token: 0x02000105 RID: 261
public class DronePad : Building
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x000257B4 File Offset: 0x000239B4
	public string GetPadID
	{
		get
		{
			return this._padID;
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x000257BC File Offset: 0x000239BC
	public void SetPadID(string id)
	{
		this._padID = id;
	}

	// Token: 0x0400057C RID: 1404
	protected string _padID = "";

	// Token: 0x0400057D RID: 1405
	protected Drone _drone;
}
