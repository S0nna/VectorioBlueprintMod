using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Vectorio.PhasmaUI
{
	// Token: 0x0200028E RID: 654
	public class Interface : Singleton<Interface>
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x00054880 File Offset: 0x00052A80
		public bool IsMouseOverUI
		{
			get
			{
				return this._isMouseOverUI;
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00054888 File Offset: 0x00052A88
		public void SetCanPauseFlag(bool toggle)
		{
			this._canPause = toggle;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00054891 File Offset: 0x00052A91
		public void SetCanOpenFlag(bool toggle)
		{
			this._canOpen = toggle;
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x0005489A File Offset: 0x00052A9A
		public bool CanOpenUI
		{
			get
			{
				return this._canOpen;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x000548A2 File Offset: 0x00052AA2
		public UI_Window CurrentlyOpen
		{
			get
			{
				return this._currentlyOpen;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x000548AA File Offset: 0x00052AAA
		public bool IsPanelOpen
		{
			get
			{
				return this._currentlyOpen != null;
			}
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x000548B8 File Offset: 0x00052AB8
		public void Start()
		{
			this.canvasGroup = base.GetComponent<CanvasGroup>();
			InputManager.OnPrimaryActionPressed.AddListener(new UnityAction(this.OnPrimaryAction));
			InputManager.OnBackActionPressed.AddListener(new UnityAction(this.OnEscapePressed));
			InputManager.OnHudActionPressed.AddListener(new UnityAction(this.Toggle));
			Singleton<Events>.Instance.onFinsihLoading.AddListener(new UnityAction(this.Enable));
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00054930 File Offset: 0x00052B30
		public void Update()
		{
			PointerEventData eventData = new PointerEventData(this.eventSystem)
			{
				position = Mouse.current.position.ReadValue()
			};
			List<RaycastResult> list = new List<RaycastResult>();
			this.eventRaycaster.Raycast(eventData, list);
			foreach (RaycastResult raycastResult in list)
			{
				if (raycastResult.gameObject.GetComponent<ClickThru>() == null)
				{
					CanvasGroup component = raycastResult.gameObject.GetComponent<CanvasGroup>();
					if (component != null && component.alpha > 0f && component.blocksRaycasts)
					{
						this._isMouseOverUI = true;
						return;
					}
					if (raycastResult.gameObject.activeSelf)
					{
						this._isMouseOverUI = true;
						return;
					}
				}
			}
			this._isMouseOverUI = false;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00054A18 File Offset: 0x00052C18
		public void Toggle()
		{
			this._toggle = !this._toggle;
			this.canvasGroup.alpha = (this._toggle ? 1f : 0f);
			this.canvasGroup.blocksRaycasts = this._toggle;
			this.canvasGroup.interactable = this._toggle;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00054A75 File Offset: 0x00052C75
		public void Enable()
		{
			this.mainGroup.alpha = 1f;
			this.mainGroup.interactable = true;
			this.mainGroup.blocksRaycasts = true;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00054AA0 File Offset: 0x00052CA0
		public void ToggleEncryption(bool toggle)
		{
			if (toggle)
			{
				this.mainGroup.alpha = 0f;
				this.mainGroup.interactable = false;
				this.mainGroup.blocksRaycasts = false;
				this.encryptionGroup.alpha = 1f;
				this.encryptionGroup.interactable = true;
				this.encryptionGroup.blocksRaycasts = true;
				return;
			}
			this.mainGroup.alpha = 1f;
			this.mainGroup.interactable = true;
			this.mainGroup.blocksRaycasts = true;
			this.encryptionGroup.alpha = 0f;
			this.encryptionGroup.interactable = false;
			this.encryptionGroup.blocksRaycasts = false;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00054B54 File Offset: 0x00052D54
		public void SetCurrentlyOpen(UI_Window ui)
		{
			if (ui != null)
			{
				if (this._currentlyOpen != null)
				{
					if (LeanTween.isTweening(this._currentlyOpen.gameObject))
					{
						LeanTween.cancel(this._currentlyOpen.gameObject);
					}
					this._currentlyOpen.ForceClose();
				}
				if (this._previouslyOpen != ui)
				{
					this.CheckPreviouslyOpen();
				}
				else
				{
					this._previouslyOpen = null;
				}
			}
			else if (this._currentlyOpen != null)
			{
				this.CheckPreviouslyOpen(this._currentlyOpen);
			}
			this._currentlyOpen = ui;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00054BE8 File Offset: 0x00052DE8
		public void ToggleHUD()
		{
			this.canvasGroup.alpha = (this.canvasGroup.interactable ? 0f : 1f);
			this.canvasGroup.interactable = !this.canvasGroup.interactable;
			this.canvasGroup.blocksRaycasts = !this.canvasGroup.blocksRaycasts;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00054C4B File Offset: 0x00052E4B
		public void OnPrimaryAction()
		{
			if (!this.IsMouseOverUI && this._currentlyOpen != null)
			{
				this._currentlyOpen.Close();
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00054C70 File Offset: 0x00052E70
		public void OnEscapePressed()
		{
			if (this.CurrentlyOpen != null)
			{
				this._currentlyOpen.Close();
				return;
			}
			if (Singleton<Selector>.Instance.IsEnabled)
			{
				Singleton<Selector>.Instance.Close();
				return;
			}
			if (this._canPause)
			{
				Singleton<Events>.Instance.onOpenPause.Invoke();
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00054CC5 File Offset: 0x00052EC5
		public void CloseCurrentlyOpen()
		{
			if (this.CurrentlyOpen != null)
			{
				this._currentlyOpen.Close();
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00054CE0 File Offset: 0x00052EE0
		protected void CheckPreviouslyOpen(UI_Window newPreviouslyOpen)
		{
			if (this._previouslyOpen != null && LeanTween.isTweening(this._previouslyOpen.gameObject))
			{
				LeanTween.cancel(this._previouslyOpen.gameObject);
				this._previouslyOpen.ForceClose();
			}
			this._previouslyOpen = newPreviouslyOpen;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00054D30 File Offset: 0x00052F30
		protected void CheckPreviouslyOpen()
		{
			if (this._previouslyOpen != null && LeanTween.isTweening(this._previouslyOpen.gameObject))
			{
				LeanTween.cancel(this._previouslyOpen.gameObject);
				this._previouslyOpen.ForceClose();
			}
			this._previouslyOpen = null;
		}

		// Token: 0x04001017 RID: 4119
		private bool _canPause = true;

		// Token: 0x04001018 RID: 4120
		private bool _canOpen = true;

		// Token: 0x04001019 RID: 4121
		private bool _isMouseOverUI;

		// Token: 0x0400101A RID: 4122
		private CanvasGroup canvasGroup;

		// Token: 0x0400101B RID: 4123
		private UI_Window _currentlyOpen;

		// Token: 0x0400101C RID: 4124
		private UI_Window _previouslyOpen;

		// Token: 0x0400101D RID: 4125
		public EventSystem eventSystem;

		// Token: 0x0400101E RID: 4126
		public GraphicRaycaster eventRaycaster;

		// Token: 0x0400101F RID: 4127
		public CanvasGroup mainGroup;

		// Token: 0x04001020 RID: 4128
		public CanvasGroup encryptionGroup;

		// Token: 0x04001021 RID: 4129
		private bool _toggle = true;
	}
}
