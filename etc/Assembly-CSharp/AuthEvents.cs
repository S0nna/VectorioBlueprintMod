using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200013B RID: 315
public class AuthEvents : Singleton<AuthEvents>
{
	// Token: 0x04000686 RID: 1670
	[HideInInspector]
	public UnityEvent<string> onAuthenticationFailed;

	// Token: 0x04000687 RID: 1671
	[HideInInspector]
	public UnityEvent<string> onAuthenticationSuccessful;
}
