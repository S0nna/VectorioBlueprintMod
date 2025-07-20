using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000316 RID: 790
	[RequireComponent(typeof(CanvasGroup))]
	public class ModalWindowManager : MonoBehaviour
	{
		// Token: 0x06001571 RID: 5489 RVA: 0x00062640 File Offset: 0x00060840
		private void Awake()
		{
			this.isOn = false;
			if (this.mwAnimator == null)
			{
				this.mwAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.confirmButton != null)
			{
				this.confirmButton.onClick.AddListener(new UnityAction(this.onConfirm.Invoke));
			}
			if (this.cancelButton != null)
			{
				this.cancelButton.onClick.AddListener(new UnityAction(this.onCancel.Invoke));
			}
			if (!this.useCustomValues)
			{
				this.UpdateUI();
			}
			if (this.startBehaviour == ModalWindowManager.StartBehaviour.Disable)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x000626F4 File Offset: 0x000608F4
		public void UpdateUI()
		{
			try
			{
				this.windowIcon.sprite = this.icon;
				this.windowTitle.text = this.titleText;
				this.windowDescription.text = this.descriptionText;
			}
			catch
			{
				Debug.LogWarning("<b>[Modal Window]</b> Cannot update the content due to missing variables.", this);
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00062754 File Offset: 0x00060954
		public void OpenWindow()
		{
			if (!this.isOn)
			{
				base.StopCoroutine("DisableObject");
				base.gameObject.SetActive(true);
				this.isOn = true;
				if (!this.sharpAnimations)
				{
					this.mwAnimator.CrossFade("Fade-in", 0.1f);
					return;
				}
				this.mwAnimator.Play("Fade-in");
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000627B8 File Offset: 0x000609B8
		public void CloseWindow()
		{
			if (this.isOn)
			{
				base.StartCoroutine("DisableObject");
				this.isOn = false;
				if (!this.sharpAnimations)
				{
					this.mwAnimator.CrossFade("Fade-out", 0.1f);
					return;
				}
				this.mwAnimator.Play("Fade-out");
			}
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00062810 File Offset: 0x00060A10
		public void AnimateWindow()
		{
			if (!this.isOn)
			{
				base.StopCoroutine("DisableObject");
				base.gameObject.SetActive(true);
				this.isOn = true;
				if (!this.sharpAnimations)
				{
					this.mwAnimator.CrossFade("Fade-in", 0.1f);
					return;
				}
				this.mwAnimator.Play("Fade-in");
				return;
			}
			else
			{
				base.StartCoroutine("DisableObject");
				this.isOn = false;
				if (!this.sharpAnimations)
				{
					this.mwAnimator.CrossFade("Fade-out", 0.1f);
					return;
				}
				this.mwAnimator.Play("Fade-out");
				return;
			}
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000628B3 File Offset: 0x00060AB3
		private IEnumerator DisableObject()
		{
			yield return new WaitForSeconds(1f);
			if (this.closeBehaviour == ModalWindowManager.CloseBehaviour.Disable)
			{
				base.gameObject.SetActive(false);
			}
			else if (this.closeBehaviour == ModalWindowManager.CloseBehaviour.Destroy)
			{
				Object.Destroy(base.gameObject);
			}
			yield break;
		}

		// Token: 0x04001385 RID: 4997
		public Image windowIcon;

		// Token: 0x04001386 RID: 4998
		public TextMeshProUGUI windowTitle;

		// Token: 0x04001387 RID: 4999
		public TextMeshProUGUI windowDescription;

		// Token: 0x04001388 RID: 5000
		public Button confirmButton;

		// Token: 0x04001389 RID: 5001
		public Button cancelButton;

		// Token: 0x0400138A RID: 5002
		public Animator mwAnimator;

		// Token: 0x0400138B RID: 5003
		public Sprite icon;

		// Token: 0x0400138C RID: 5004
		public string titleText = "Title";

		// Token: 0x0400138D RID: 5005
		[TextArea]
		public string descriptionText = "Description here";

		// Token: 0x0400138E RID: 5006
		public UnityEvent onConfirm;

		// Token: 0x0400138F RID: 5007
		public UnityEvent onCancel;

		// Token: 0x04001390 RID: 5008
		public bool sharpAnimations;

		// Token: 0x04001391 RID: 5009
		public bool useCustomValues;

		// Token: 0x04001392 RID: 5010
		public bool isOn;

		// Token: 0x04001393 RID: 5011
		public ModalWindowManager.StartBehaviour startBehaviour = ModalWindowManager.StartBehaviour.Disable;

		// Token: 0x04001394 RID: 5012
		public ModalWindowManager.CloseBehaviour closeBehaviour = ModalWindowManager.CloseBehaviour.Disable;

		// Token: 0x02000317 RID: 791
		public enum StartBehaviour
		{
			// Token: 0x04001396 RID: 5014
			None,
			// Token: 0x04001397 RID: 5015
			Disable
		}

		// Token: 0x02000318 RID: 792
		public enum CloseBehaviour
		{
			// Token: 0x04001399 RID: 5017
			None,
			// Token: 0x0400139A RID: 5018
			Disable,
			// Token: 0x0400139B RID: 5019
			Destroy
		}
	}
}
