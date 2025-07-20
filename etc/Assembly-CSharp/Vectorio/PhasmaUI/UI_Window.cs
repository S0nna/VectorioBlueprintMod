using System;
using UnityEngine;

namespace Vectorio.PhasmaUI
{
	// Token: 0x02000292 RID: 658
	public class UI_Window : MonoBehaviour
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x00056128 File Offset: 0x00054328
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x00056130 File Offset: 0x00054330
		public bool AllowScrolling
		{
			get
			{
				return this._allowScrolling;
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00056138 File Offset: 0x00054338
		public void SetIsOpen(bool toggle)
		{
			this._isOpen = toggle;
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00056141 File Offset: 0x00054341
		public virtual void Toggle()
		{
			if (this._isOpen)
			{
				this.Close();
				return;
			}
			this.Open();
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00056158 File Offset: 0x00054358
		public virtual void Open()
		{
			this.SetIsOpen(true);
			this.canvasGroup.alpha = 1f;
			this.canvasGroup.interactable = true;
			this.canvasGroup.blocksRaycasts = true;
			Singleton<Interface>.Instance.SetCurrentlyOpen(this);
			if (this.openSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.openSound);
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000561BD File Offset: 0x000543BD
		public virtual void ForceOpen()
		{
			this.SetIsOpen(true);
			this.canvasGroup.alpha = 1f;
			this.canvasGroup.interactable = true;
			this.canvasGroup.blocksRaycasts = true;
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x000561F0 File Offset: 0x000543F0
		public virtual void Close()
		{
			this.SetIsOpen(false);
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			Singleton<Interface>.Instance.SetCurrentlyOpen(null);
			if (this.closeSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.closeSound);
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00056255 File Offset: 0x00054455
		public virtual void ForceClose()
		{
			this.SetIsOpen(false);
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
		}

		// Token: 0x04001054 RID: 4180
		public CanvasGroup canvasGroup;

		// Token: 0x04001055 RID: 4181
		public AudioClip openSound;

		// Token: 0x04001056 RID: 4182
		public AudioClip closeSound;

		// Token: 0x04001057 RID: 4183
		protected bool _isOpen;

		// Token: 0x04001058 RID: 4184
		protected bool _allowScrolling;
	}
}
