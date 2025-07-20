using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200031A RID: 794
	[RequireComponent(typeof(Animator))]
	public class NotificationManager : MonoBehaviour
	{
		// Token: 0x0600157E RID: 5502 RVA: 0x00062980 File Offset: 0x00060B80
		private void Awake()
		{
			this.isOn = false;
			if (!this.useCustomContent)
			{
				try
				{
					this.UpdateUI();
				}
				catch
				{
					Debug.LogError("<b>[Notification]</b> Cannot initalize the object due to missing components.", this);
				}
			}
			if (this.useStacking)
			{
				try
				{
					NotificationStacking componentInParent = base.transform.GetComponentInParent<NotificationStacking>();
					componentInParent.notifications.Add(this);
					componentInParent.enableUpdating = true;
				}
				catch
				{
					Debug.LogError("<b>[Notification]</b> 'Stacking' is enabled but 'Notification Stacking' cannot be found in parent.", this);
				}
			}
			if (this.notificationAnimator == null)
			{
				this.notificationAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.startBehaviour == NotificationManager.StartBehaviour.Disable)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00062A38 File Offset: 0x00060C38
		public void OpenNotification()
		{
			if (this.isOn)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.isOn = true;
			base.StopCoroutine("StartTimer");
			base.StopCoroutine("DisableNotification");
			this.notificationAnimator.Play("In");
			this.onOpen.Invoke();
			if (this.enableTimer)
			{
				base.StartCoroutine("StartTimer");
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00062AA6 File Offset: 0x00060CA6
		public void CloseNotification()
		{
			if (!this.isOn)
			{
				return;
			}
			this.isOn = false;
			this.notificationAnimator.Play("Out");
			this.onClose.Invoke();
			base.StartCoroutine("DisableNotification");
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00062AE0 File Offset: 0x00060CE0
		public void UpdateUI()
		{
			try
			{
				this.iconObj.sprite = this.icon;
				this.titleObj.text = this.title;
				this.descriptionObj.text = this.description;
			}
			catch
			{
				Debug.LogError("<b>[Notification]</b> Cannot update the component due to missing variables.", this);
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00062B40 File Offset: 0x00060D40
		private IEnumerator StartTimer()
		{
			yield return new WaitForSeconds(this.timer);
			this.CloseNotification();
			base.StartCoroutine("DisableNotification");
			yield break;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00062B4F File Offset: 0x00060D4F
		private IEnumerator DisableNotification()
		{
			yield return new WaitForSeconds(1f);
			if (this.closeBehaviour == NotificationManager.CloseBehaviour.Disable)
			{
				base.gameObject.SetActive(false);
				this.isOn = false;
			}
			else if (this.closeBehaviour == NotificationManager.CloseBehaviour.Destroy)
			{
				Object.Destroy(base.gameObject);
			}
			yield break;
		}

		// Token: 0x0400139F RID: 5023
		public Sprite icon;

		// Token: 0x040013A0 RID: 5024
		public string title = "Notification Title";

		// Token: 0x040013A1 RID: 5025
		[TextArea]
		public string description = "Notification description";

		// Token: 0x040013A2 RID: 5026
		public Animator notificationAnimator;

		// Token: 0x040013A3 RID: 5027
		public Image iconObj;

		// Token: 0x040013A4 RID: 5028
		public TextMeshProUGUI titleObj;

		// Token: 0x040013A5 RID: 5029
		public TextMeshProUGUI descriptionObj;

		// Token: 0x040013A6 RID: 5030
		public bool enableTimer = true;

		// Token: 0x040013A7 RID: 5031
		public float timer = 3f;

		// Token: 0x040013A8 RID: 5032
		public bool useCustomContent;

		// Token: 0x040013A9 RID: 5033
		public bool useStacking;

		// Token: 0x040013AA RID: 5034
		[HideInInspector]
		public bool isOn;

		// Token: 0x040013AB RID: 5035
		public NotificationManager.StartBehaviour startBehaviour = NotificationManager.StartBehaviour.Disable;

		// Token: 0x040013AC RID: 5036
		public NotificationManager.CloseBehaviour closeBehaviour = NotificationManager.CloseBehaviour.Disable;

		// Token: 0x040013AD RID: 5037
		public UnityEvent onOpen;

		// Token: 0x040013AE RID: 5038
		public UnityEvent onClose;

		// Token: 0x0200031B RID: 795
		public enum StartBehaviour
		{
			// Token: 0x040013B0 RID: 5040
			None,
			// Token: 0x040013B1 RID: 5041
			Disable
		}

		// Token: 0x0200031C RID: 796
		public enum CloseBehaviour
		{
			// Token: 0x040013B3 RID: 5043
			None,
			// Token: 0x040013B4 RID: 5044
			Disable,
			// Token: 0x040013B5 RID: 5045
			Destroy
		}
	}
}
