using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200030A RID: 778
	[RequireComponent(typeof(TMP_InputField))]
	[RequireComponent(typeof(Animator))]
	public class CustomInputField : MonoBehaviour
	{
		// Token: 0x0600153B RID: 5435 RVA: 0x000618A8 File Offset: 0x0005FAA8
		private void Awake()
		{
			if (this.inputText == null)
			{
				this.inputText = base.gameObject.GetComponent<TMP_InputField>();
			}
			if (this.inputFieldAnimator == null)
			{
				this.inputFieldAnimator = base.gameObject.GetComponent<Animator>();
			}
			this.inputText.onSelect.AddListener(delegate(string <p0>)
			{
				this.AnimateIn();
			});
			this.inputText.onEndEdit.AddListener(delegate(string <p0>)
			{
				this.AnimateOut();
			});
			this.UpdateStateInstant();
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00061931 File Offset: 0x0005FB31
		private void OnEnable()
		{
			if (this.inputText == null)
			{
				return;
			}
			this.inputText.ForceLabelUpdate();
			this.UpdateStateInstant();
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine("DisableAnimator");
			}
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0006196C File Offset: 0x0005FB6C
		private void Update()
		{
			if (!this.processSubmit || string.IsNullOrEmpty(this.inputText.text) || EventSystem.current.currentSelectedGameObject != this.inputText.gameObject)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Return))
			{
				this.onSubmit.Invoke();
				this.inputText.text = "";
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x000619D4 File Offset: 0x0005FBD4
		public void AnimateIn()
		{
			base.StopCoroutine("DisableAnimator");
			if (this.inputFieldAnimator.gameObject.activeInHierarchy)
			{
				this.inputFieldAnimator.enabled = true;
				this.inputFieldAnimator.Play(this.inAnim);
				base.StartCoroutine("DisableAnimator");
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00061A28 File Offset: 0x0005FC28
		public void AnimateOut()
		{
			if (this.inputFieldAnimator.gameObject.activeInHierarchy)
			{
				this.inputFieldAnimator.enabled = true;
				if (this.inputText.text.Length == 0)
				{
					this.inputFieldAnimator.Play(this.outAnim);
				}
				base.StartCoroutine("DisableAnimator");
			}
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00061A82 File Offset: 0x0005FC82
		public void UpdateState()
		{
			if (this.inputText.text.Length == 0)
			{
				this.AnimateOut();
				return;
			}
			this.AnimateIn();
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00061AA3 File Offset: 0x0005FCA3
		public void UpdateStateInstant()
		{
			if (this.inputText.text.Length == 0)
			{
				this.inputFieldAnimator.Play(this.instaOutAnim);
				return;
			}
			this.inputFieldAnimator.Play(this.instaInAnim);
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00061ADA File Offset: 0x0005FCDA
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSeconds(1f);
			this.inputFieldAnimator.enabled = false;
			yield break;
		}

		// Token: 0x04001345 RID: 4933
		[Header("Resources")]
		public TMP_InputField inputText;

		// Token: 0x04001346 RID: 4934
		public Animator inputFieldAnimator;

		// Token: 0x04001347 RID: 4935
		[Header("Settings")]
		public bool processSubmit;

		// Token: 0x04001348 RID: 4936
		[Header("Events")]
		public UnityEvent onSubmit;

		// Token: 0x04001349 RID: 4937
		private string inAnim = "In";

		// Token: 0x0400134A RID: 4938
		private string outAnim = "Out";

		// Token: 0x0400134B RID: 4939
		private string instaInAnim = "Instant In";

		// Token: 0x0400134C RID: 4940
		private string instaOutAnim = "Instant Out";
	}
}
