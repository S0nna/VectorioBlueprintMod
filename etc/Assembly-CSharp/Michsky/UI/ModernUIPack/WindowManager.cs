using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000356 RID: 854
	public class WindowManager : MonoBehaviour
	{
		// Token: 0x06001670 RID: 5744 RVA: 0x00068A92 File Offset: 0x00066C92
		private void Awake()
		{
			if (this.windows.Count == 0)
			{
				return;
			}
			this.InitializeWindows();
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00068AA8 File Offset: 0x00066CA8
		private void OnEnable()
		{
			if (this.isInitialized && this.nextWindowAnimator == null)
			{
				this.currentWindowAnimator.Play(this.windowFadeIn);
				if (this.currentButtonAnimator != null)
				{
					this.currentButtonAnimator.Play(this.buttonFadeIn);
					return;
				}
			}
			else if (this.isInitialized && this.nextWindowAnimator != null)
			{
				this.nextWindowAnimator.Play(this.windowFadeIn);
				if (this.nextButtonAnimator != null)
				{
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00068B44 File Offset: 0x00066D44
		public void InitializeWindows()
		{
			if (this.windows[this.currentWindowIndex].buttonObject != null)
			{
				this.currentButton = this.windows[this.currentWindowIndex].buttonObject;
				this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
				this.currentButtonAnimator.Play(this.buttonFadeIn);
			}
			this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
			this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
			this.currentWindowAnimator.Play(this.windowFadeIn);
			this.onWindowChange.Invoke(this.currentWindowIndex);
			this.isInitialized = true;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (i != this.currentWindowIndex && this.cullWindows)
				{
					this.windows[i].windowObject.SetActive(false);
				}
				if (this.windows[i].buttonObject != null && this.initializeButtons)
				{
					string tempName = this.windows[i].windowName;
					Button component = this.windows[i].buttonObject.GetComponent<Button>();
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(delegate()
					{
						this.OpenPanel(tempName);
					});
				}
			}
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00068CC4 File Offset: 0x00066EC4
		public void OpenFirstTab()
		{
			if (this.currentWindowIndex != 0)
			{
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				if (this.windows[this.currentWindowIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentWindowIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
				}
				this.currentWindowIndex = 0;
				this.currentButtonIndex = 0;
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeIn);
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
				return;
			}
			if (this.currentWindowIndex == 0)
			{
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeIn);
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeIn);
				}
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00068ED8 File Offset: 0x000670D8
		public void OpenWindow(string newWindow)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].windowName == newWindow)
				{
					this.newWindowIndex = i;
					break;
				}
			}
			if (this.newWindowIndex != this.currentWindowIndex)
			{
				if (this.cullWindows)
				{
					base.StopCoroutine("DisablePreviousWindow");
				}
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				if (this.windows[this.currentWindowIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentWindowIndex].buttonObject;
				}
				this.currentWindowIndex = this.newWindowIndex;
				this.nextWindow = this.windows[this.currentWindowIndex].windowObject;
				this.nextWindow.SetActive(true);
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.nextWindowAnimator = this.nextWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				this.nextWindowAnimator.Play(this.windowFadeIn);
				if (this.cullWindows)
				{
					base.StartCoroutine("DisablePreviousWindow");
				}
				this.currentButtonIndex = this.newWindowIndex;
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.nextButton = this.windows[this.currentButtonIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.nextButtonAnimator = this.nextButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
			}
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000690BE File Offset: 0x000672BE
		public void OpenPanel(string newPanel)
		{
			this.OpenWindow(newPanel);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x000690C8 File Offset: 0x000672C8
		public void OpenWindowByIndex(int windowIndex)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].windowName == this.windows[windowIndex].windowName)
				{
					this.OpenWindow(this.windows[windowIndex].windowName);
					return;
				}
			}
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x0006912C File Offset: 0x0006732C
		public void NextWindow()
		{
			if (this.currentWindowIndex <= this.windows.Count - 2)
			{
				if (this.cullWindows)
				{
					base.StopCoroutine("DisablePreviousWindow");
				}
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindow.gameObject.SetActive(true);
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.nextButton = this.windows[this.currentButtonIndex + 1].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
				}
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				this.currentWindowIndex++;
				this.currentButtonIndex++;
				this.nextWindow = this.windows[this.currentWindowIndex].windowObject;
				this.nextWindow.gameObject.SetActive(true);
				this.nextWindowAnimator = this.nextWindow.GetComponent<Animator>();
				this.nextWindowAnimator.Play(this.windowFadeIn);
				if (this.cullWindows)
				{
					base.StartCoroutine("DisablePreviousWindow");
				}
				if (this.nextButton != null)
				{
					this.nextButtonAnimator = this.nextButton.GetComponent<Animator>();
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
			}
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x000692F0 File Offset: 0x000674F0
		public void PrevWindow()
		{
			if (this.currentWindowIndex >= 1)
			{
				if (this.cullWindows)
				{
					base.StopCoroutine("DisablePreviousWindow");
				}
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindow.gameObject.SetActive(true);
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.nextButton = this.windows[this.currentButtonIndex - 1].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
				}
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				this.currentWindowIndex--;
				this.currentButtonIndex--;
				this.nextWindow = this.windows[this.currentWindowIndex].windowObject;
				this.nextWindow.gameObject.SetActive(true);
				this.nextWindowAnimator = this.nextWindow.GetComponent<Animator>();
				this.nextWindowAnimator.Play(this.windowFadeIn);
				if (this.cullWindows)
				{
					base.StartCoroutine("DisablePreviousWindow");
				}
				if (this.nextButton != null)
				{
					this.nextButtonAnimator = this.nextButton.GetComponent<Animator>();
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
			}
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x000694A5 File Offset: 0x000676A5
		public void ShowCurrentWindow()
		{
			if (this.nextWindowAnimator == null)
			{
				this.currentWindowAnimator.Play(this.windowFadeIn);
				return;
			}
			this.nextWindowAnimator.Play(this.windowFadeIn);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x000694D8 File Offset: 0x000676D8
		public void HideCurrentWindow()
		{
			if (this.nextWindowAnimator == null)
			{
				this.currentWindowAnimator.Play(this.windowFadeOut);
				return;
			}
			this.nextWindowAnimator.Play(this.windowFadeOut);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x0006950B File Offset: 0x0006770B
		public void ShowCurrentButton()
		{
			if (this.nextButtonAnimator == null)
			{
				this.currentButtonAnimator.Play(this.buttonFadeIn);
				return;
			}
			this.nextButtonAnimator.Play(this.buttonFadeIn);
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0006953E File Offset: 0x0006773E
		public void HideCurrentButton()
		{
			if (this.nextButtonAnimator == null)
			{
				this.currentButtonAnimator.Play(this.buttonFadeOut);
				return;
			}
			this.nextButtonAnimator.Play(this.buttonFadeOut);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00069574 File Offset: 0x00067774
		public void AddNewItem()
		{
			WindowManager.WindowItem windowItem = new WindowManager.WindowItem();
			if (this.windows.Count != 0 && this.windows[this.windows.Count - 1].windowObject != null)
			{
				int index = this.windows.Count - 1;
				GameObject gameObject = Object.Instantiate<GameObject>(this.windows[index].windowObject.transform.parent.GetChild(index).gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.windows[index].windowObject.transform.parent, false);
				gameObject.gameObject.name = "New Window " + index.ToString();
				windowItem.windowName = "New Window " + index.ToString();
				windowItem.windowObject = gameObject;
				if (this.windows[index].buttonObject != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.windows[index].buttonObject.transform.parent.GetChild(index).gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
					gameObject2.transform.SetParent(this.windows[index].buttonObject.transform.parent, false);
					gameObject2.gameObject.name = "New Window " + index.ToString();
					windowItem.buttonObject = gameObject2;
				}
			}
			this.windows.Add(windowItem);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x0006972A File Offset: 0x0006792A
		private IEnumerator DisablePreviousWindow()
		{
			yield return new WaitForSeconds(0.4f);
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (i != this.currentWindowIndex)
				{
					this.windows[i].windowObject.SetActive(false);
				}
			}
			yield break;
		}

		// Token: 0x04001564 RID: 5476
		public List<WindowManager.WindowItem> windows = new List<WindowManager.WindowItem>();

		// Token: 0x04001565 RID: 5477
		public int currentWindowIndex;

		// Token: 0x04001566 RID: 5478
		private int currentButtonIndex;

		// Token: 0x04001567 RID: 5479
		private int newWindowIndex;

		// Token: 0x04001568 RID: 5480
		public bool cullWindows = true;

		// Token: 0x04001569 RID: 5481
		public bool initializeButtons = true;

		// Token: 0x0400156A RID: 5482
		private bool isInitialized;

		// Token: 0x0400156B RID: 5483
		public WindowManager.WindowChangeEvent onWindowChange;

		// Token: 0x0400156C RID: 5484
		private GameObject currentWindow;

		// Token: 0x0400156D RID: 5485
		private GameObject nextWindow;

		// Token: 0x0400156E RID: 5486
		private GameObject currentButton;

		// Token: 0x0400156F RID: 5487
		private GameObject nextButton;

		// Token: 0x04001570 RID: 5488
		private Animator currentWindowAnimator;

		// Token: 0x04001571 RID: 5489
		private Animator nextWindowAnimator;

		// Token: 0x04001572 RID: 5490
		private Animator currentButtonAnimator;

		// Token: 0x04001573 RID: 5491
		private Animator nextButtonAnimator;

		// Token: 0x04001574 RID: 5492
		private string windowFadeIn = "In";

		// Token: 0x04001575 RID: 5493
		private string windowFadeOut = "Out";

		// Token: 0x04001576 RID: 5494
		private string buttonFadeIn = "Normal to Pressed";

		// Token: 0x04001577 RID: 5495
		private string buttonFadeOut = "Pressed to Normal";

		// Token: 0x02000357 RID: 855
		[Serializable]
		public class WindowChangeEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000358 RID: 856
		[Serializable]
		public class WindowItem
		{
			// Token: 0x04001578 RID: 5496
			public string windowName = "My Window";

			// Token: 0x04001579 RID: 5497
			public GameObject windowObject;

			// Token: 0x0400157A RID: 5498
			public GameObject buttonObject;
		}
	}
}
