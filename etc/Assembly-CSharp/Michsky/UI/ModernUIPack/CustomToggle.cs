using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000333 RID: 819
	[RequireComponent(typeof(Toggle))]
	[RequireComponent(typeof(Animator))]
	public class CustomToggle : MonoBehaviour
	{
		// Token: 0x060015F7 RID: 5623 RVA: 0x00065278 File Offset: 0x00063478
		private void Awake()
		{
			if (this.toggleObject == null)
			{
				this.toggleObject = base.gameObject.GetComponent<Toggle>();
			}
			if (this.toggleAnimator == null)
			{
				this.toggleAnimator = this.toggleObject.GetComponent<Animator>();
			}
			this.toggleObject.onValueChanged.AddListener(new UnityAction<bool>(this.UpdateStateDynamic));
			this.UpdateState();
			if (this.invokeOnAwake)
			{
				this.toggleObject.onValueChanged.Invoke(this.toggleObject.isOn);
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00065308 File Offset: 0x00063508
		public void UpdateState()
		{
			base.StopCoroutine("DisableAnimator");
			this.toggleAnimator.enabled = true;
			if (this.toggleObject.isOn)
			{
				this.toggleAnimator.Play("On Instant");
			}
			else
			{
				this.toggleAnimator.Play("Off Instant");
			}
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00065368 File Offset: 0x00063568
		public void UpdateStateDynamic(bool value)
		{
			base.StopCoroutine("DisableAnimator");
			this.toggleAnimator.enabled = true;
			if (this.toggleObject.isOn)
			{
				this.toggleAnimator.Play("Toggle On");
			}
			else
			{
				this.toggleAnimator.Play("Toggle Off");
			}
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x000653C7 File Offset: 0x000635C7
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSeconds(0.5f);
			this.toggleAnimator.enabled = false;
			yield break;
		}

		// Token: 0x0400143F RID: 5183
		[HideInInspector]
		public Toggle toggleObject;

		// Token: 0x04001440 RID: 5184
		[HideInInspector]
		public Animator toggleAnimator;

		// Token: 0x04001441 RID: 5185
		[Header("Settings")]
		public bool invokeOnAwake;
	}
}
