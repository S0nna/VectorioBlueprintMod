using System;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000168 RID: 360
[DefaultExecutionOrder(-1)]
public class NetworkEvents : Singleton<NetworkEvents>
{
	// Token: 0x04000805 RID: 2053
	[HideInInspector]
	public UnityEvent<NetworkObject, NetworkConnection> onPlayerSpawned;

	// Token: 0x04000806 RID: 2054
	[HideInInspector]
	public UnityEvent<bool> onLocalClientReadyToLoad;

	// Token: 0x04000807 RID: 2055
	[HideInInspector]
	public UnityEvent onPlayerStartedLoading;

	// Token: 0x04000808 RID: 2056
	[HideInInspector]
	public UnityEvent onPlayerStoppedLoading;
}
