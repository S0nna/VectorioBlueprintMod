using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000331 RID: 817
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Button))]
	public class SwitchManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x060015E8 RID: 5608 RVA: 0x00064DC0 File Offset: 0x00062FC0
		private void Awake()
		{
			if (this.switchAnimator == null)
			{
				this.switchAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.switchButton == null)
			{
				this.switchButton = base.gameObject.GetComponent<Button>();
				this.switchButton.onClick.AddListener(new UnityAction(this.AnimateSwitch));
				if (this.enableSwitchSounds && this.useClickSound)
				{
					this.switchButton.onClick.AddListener(delegate()
					{
						this.soundSource.PlayOneShot(this.clickSound);
					});
				}
			}
			if (this.saveValue)
			{
				this.GetSavedData();
			}
			else
			{
				base.StopCoroutine("DisableAnimator");
				this.switchAnimator.enabled = true;
				if (this.isOn)
				{
					this.switchAnimator.Play("On Instant");
				}
				else
				{
					this.switchAnimator.Play("Off Instant");
				}
				base.StartCoroutine("DisableAnimator");
			}
			if (this.invokeAtStart && this.isOn)
			{
				this.OnEvents.Invoke();
				return;
			}
			if (this.invokeAtStart && !this.isOn)
			{
				this.OffEvents.Invoke();
			}
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00064EE7 File Offset: 0x000630E7
		private void OnDisable()
		{
			base.StopCoroutine("DisableAnimator");
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x00064EF4 File Offset: 0x000630F4
		private void GetSavedData()
		{
			base.StopCoroutine("DisableAnimator");
			this.switchAnimator.enabled = true;
			if (PlayerPrefs.GetString(this.switchTag + "Switch") == "" || !PlayerPrefs.HasKey(this.switchTag + "Switch"))
			{
				if (this.isOn)
				{
					this.switchAnimator.Play("Switch On");
					PlayerPrefs.SetString(this.switchTag + "Switch", "true");
				}
				else
				{
					this.switchAnimator.Play("Switch Off");
					PlayerPrefs.SetString(this.switchTag + "Switch", "false");
				}
			}
			else if (PlayerPrefs.GetString(this.switchTag + "Switch") == "true")
			{
				this.switchAnimator.Play("Switch On");
				this.isOn = true;
			}
			else if (PlayerPrefs.GetString(this.switchTag + "Switch") == "false")
			{
				this.switchAnimator.Play("Switch Off");
				this.isOn = false;
			}
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00065034 File Offset: 0x00063234
		public void AnimateSwitch()
		{
			base.StopCoroutine("DisableAnimator");
			this.switchAnimator.enabled = true;
			if (this.isOn)
			{
				this.switchAnimator.Play("Switch Off");
				this.isOn = false;
				this.OffEvents.Invoke();
				if (this.saveValue)
				{
					PlayerPrefs.SetString(this.switchTag + "Switch", "false");
				}
			}
			else
			{
				this.switchAnimator.Play("Switch On");
				this.isOn = true;
				this.OnEvents.Invoke();
				if (this.saveValue)
				{
					PlayerPrefs.SetString(this.switchTag + "Switch", "true");
				}
			}
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x000650F8 File Offset: 0x000632F8
		public void UpdateUI()
		{
			base.StopCoroutine("DisableAnimator");
			this.switchAnimator.enabled = true;
			if (this.isOn && this.switchAnimator.gameObject.activeInHierarchy)
			{
				this.switchAnimator.Play("Switch On");
			}
			else if (!this.isOn && this.switchAnimator.gameObject.activeInHierarchy)
			{
				this.switchAnimator.Play("Switch Off");
			}
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0006517E File Offset: 0x0006337E
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableSwitchSounds && this.useHoverSound && this.switchButton.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x000651AE File Offset: 0x000633AE
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSeconds(0.5f);
			this.switchAnimator.enabled = false;
			yield break;
		}

		// Token: 0x0400142E RID: 5166
		public UnityEvent OnEvents;

		// Token: 0x0400142F RID: 5167
		public UnityEvent OffEvents;

		// Token: 0x04001430 RID: 5168
		public bool saveValue = true;

		// Token: 0x04001431 RID: 5169
		public string switchTag = "Switch";

		// Token: 0x04001432 RID: 5170
		public bool isOn = true;

		// Token: 0x04001433 RID: 5171
		public bool invokeAtStart = true;

		// Token: 0x04001434 RID: 5172
		public bool enableSwitchSounds;

		// Token: 0x04001435 RID: 5173
		public bool useHoverSound = true;

		// Token: 0x04001436 RID: 5174
		public bool useClickSound = true;

		// Token: 0x04001437 RID: 5175
		public Animator switchAnimator;

		// Token: 0x04001438 RID: 5176
		public Button switchButton;

		// Token: 0x04001439 RID: 5177
		public AudioSource soundSource;

		// Token: 0x0400143A RID: 5178
		public AudioClip hoverSound;

		// Token: 0x0400143B RID: 5179
		public AudioClip clickSound;
	}
}
