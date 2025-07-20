using System;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002F3 RID: 755
	public class LaunchURL : MonoBehaviour
	{
		// Token: 0x060014E5 RID: 5349 RVA: 0x000305F3 File Offset: 0x0002E7F3
		public void GoToURL(string URL)
		{
			Application.OpenURL(URL);
		}
	}
}
