using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200031F RID: 799
	[AddComponentMenu("Modern UI Pack/Notification/Notification Stacking")]
	public class NotificationStacking : MonoBehaviour
	{
		// Token: 0x06001591 RID: 5521 RVA: 0x00062CAC File Offset: 0x00060EAC
		private void Update()
		{
			if (this.enableUpdating)
			{
				try
				{
					this.notifications[this.currentNotification].gameObject.SetActive(true);
					if (this.notifications[this.currentNotification].notificationAnimator.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
					{
						this.notifications[this.currentNotification].OpenNotification();
						base.StartCoroutine("StartNotification");
						this.enableUpdating = false;
					}
					if (this.currentNotification >= this.notifications.Count)
					{
						this.enableUpdating = false;
						this.currentNotification = 0;
					}
				}
				catch
				{
					this.enableUpdating = false;
					this.currentNotification = 0;
					this.notifications.Clear();
				}
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00062D84 File Offset: 0x00060F84
		private IEnumerator StartNotification()
		{
			yield return new WaitForSeconds(this.notifications[this.currentNotification].timer + this.delay);
			Object.Destroy(this.notifications[this.currentNotification].gameObject);
			this.enableUpdating = true;
			this.currentNotification++;
			base.StopCoroutine("StartNotification");
			yield break;
		}

		// Token: 0x040013BC RID: 5052
		public List<NotificationManager> notifications = new List<NotificationManager>();

		// Token: 0x040013BD RID: 5053
		[HideInInspector]
		public bool enableUpdating;

		// Token: 0x040013BE RID: 5054
		[Header("Settings")]
		public float delay = 1f;

		// Token: 0x040013BF RID: 5055
		private int currentNotification;
	}
}
