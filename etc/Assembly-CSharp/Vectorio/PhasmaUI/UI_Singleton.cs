using System;
using UnityEngine;

namespace Vectorio.PhasmaUI
{
	// Token: 0x0200028C RID: 652
	[DefaultExecutionOrder(-1)]
	public class UI_Singleton<T> : UI_Window where T : Component
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00054490 File Offset: 0x00052690
		public static T Instance
		{
			get
			{
				if (!UI_Singleton<T>.instance)
				{
					Debug.Log("Failed to get " + typeof(T).Name + " instance.\nPlease make sure you are not trying to access the instance before start!");
				}
				return UI_Singleton<T>.instance;
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000544CC File Offset: 0x000526CC
		public virtual void Awake()
		{
			if (UI_Singleton<T>.instance)
			{
				Debug.LogError("There is more than one " + typeof(T).Name + " singleton in the scene!");
				return;
			}
			UI_Singleton<T>.instance = (this as T);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0005451E File Offset: 0x0005271E
		private void OnDestroy()
		{
			UI_Singleton<T>.instance = default(T);
		}

		// Token: 0x04001007 RID: 4103
		private static T instance;
	}
}
