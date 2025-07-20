using System;
using HeathenEngineering.SteamworksIntegration;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class CustomSteamworks : SteamworksBehaviour
{
	// Token: 0x06000DDF RID: 3551 RVA: 0x0003D8A9 File Offset: 0x0003BAA9
	public void Awake()
	{
		if (this.dontDestroyOnLoad)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	// Token: 0x04000A2B RID: 2603
	public bool dontDestroyOnLoad;
}
