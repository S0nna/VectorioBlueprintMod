using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002DA RID: 730
	[AddComponentMenu("Modern UI Pack/Context Menu/Context Menu Content (Mobile)")]
	public class ContextMenuContentMobile : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x0005D1D8 File Offset: 0x0005B3D8
		private void Start()
		{
			if (this.contextManager == null)
			{
				try
				{
					this.contextManager = GameObject.Find("Context Menu").GetComponent<ContextMenuManager>();
					this.itemParent = this.contextManager.transform.Find("Content/Item List").transform;
				}
				catch
				{
					Debug.Log("<b>[Context Menu]</b> Context Manager is missing.", this);
					return;
				}
			}
			this.contextAnimator = this.contextManager.contextAnimator;
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0005D2A4 File Offset: 0x0005B4A4
		private void Update()
		{
			if (this.timerEnabled)
			{
				this.timer += Time.deltaTime;
				if (this.timer >= this.holdToOpen)
				{
					this.CheckForTimer();
					this.timerEnabled = false;
					this.timer = 0f;
				}
			}
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0005D2F1 File Offset: 0x0005B4F1
		public void OnPointerDown(PointerEventData eventData)
		{
			this.timerEnabled = true;
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0005D2FA File Offset: 0x0005B4FA
		public void OnPointerUp(PointerEventData eventData)
		{
			this.timerEnabled = false;
			this.timer = 0f;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0005D310 File Offset: 0x0005B510
		public void CheckForTimer()
		{
			if (this.timer <= this.holdToOpen)
			{
				return;
			}
			if (this.contextManager.isOn)
			{
				this.contextAnimator.Play("Menu Out");
				this.contextManager.isOn = false;
				return;
			}
			if (!this.contextManager.isOn)
			{
				foreach (object obj in this.itemParent)
				{
					Object.Destroy(((Transform)obj).gameObject);
				}
				for (int i = 0; i < this.contexItems.Count; i++)
				{
					if (this.contexItems[i].contextItemType == ContextMenuContentMobile.ContextItemType.BUTTON)
					{
						this.selectedItem = this.contextManager.contextButton;
					}
					GameObject gameObject = Object.Instantiate<GameObject>(this.selectedItem, new Vector3(0f, 0f, 0f), Quaternion.identity);
					gameObject.transform.SetParent(this.itemParent, false);
					this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
					this.textHelper = this.contexItems[i].itemText;
					this.setItemText.text = this.textHelper;
					Transform transform = gameObject.gameObject.transform.Find("Icon");
					this.setItemImage = transform.GetComponent<Image>();
					this.imageHelper = this.contexItems[i].itemIcon;
					this.setItemImage.sprite = this.imageHelper;
					if (this.imageHelper == null)
					{
						this.setItemImage.color = new Color(0f, 0f, 0f, 0f);
					}
					Button component = gameObject.GetComponent<Button>();
					component.onClick.AddListener(new UnityAction(this.contexItems[i].onClick.Invoke));
					component.onClick.AddListener(new UnityAction(this.CloseOnClick));
					base.StartCoroutine(this.ExecuteAfterTime(0.01f));
				}
				this.contextManager.SetContextMenuPosition();
				this.contextAnimator.Play("Menu In");
				this.contextManager.isOn = true;
				this.contextManager.SetContextMenuPosition();
			}
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0005D564 File Offset: 0x0005B764
		private IEnumerator ExecuteAfterTime(float time)
		{
			yield return new WaitForSeconds(time);
			this.itemParent.gameObject.SetActive(false);
			this.itemParent.gameObject.SetActive(true);
			base.StopCoroutine(this.ExecuteAfterTime(0.01f));
			base.StopCoroutine("ExecuteAfterTime");
			yield break;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0005D57A File Offset: 0x0005B77A
		public void CloseOnClick()
		{
			this.contextAnimator.Play("Menu Out");
			this.contextManager.isOn = false;
		}

		// Token: 0x0400121B RID: 4635
		[Header("Resources")]
		public ContextMenuManager contextManager;

		// Token: 0x0400121C RID: 4636
		public Transform itemParent;

		// Token: 0x0400121D RID: 4637
		[Header("Settings")]
		[Range(0.1f, 6f)]
		public float holdToOpen = 0.75f;

		// Token: 0x0400121E RID: 4638
		[Header("Items")]
		public List<ContextMenuContentMobile.ContextItem> contexItems = new List<ContextMenuContentMobile.ContextItem>();

		// Token: 0x0400121F RID: 4639
		private Animator contextAnimator;

		// Token: 0x04001220 RID: 4640
		private GameObject selectedItem;

		// Token: 0x04001221 RID: 4641
		private Image setItemImage;

		// Token: 0x04001222 RID: 4642
		private TextMeshProUGUI setItemText;

		// Token: 0x04001223 RID: 4643
		private Sprite imageHelper;

		// Token: 0x04001224 RID: 4644
		private string textHelper;

		// Token: 0x04001225 RID: 4645
		private float timer;

		// Token: 0x04001226 RID: 4646
		private bool timerEnabled;

		// Token: 0x020002DB RID: 731
		[Serializable]
		public class ContextItem
		{
			// Token: 0x04001227 RID: 4647
			public string itemText = "Item Text";

			// Token: 0x04001228 RID: 4648
			public Sprite itemIcon;

			// Token: 0x04001229 RID: 4649
			public ContextMenuContentMobile.ContextItemType contextItemType;

			// Token: 0x0400122A RID: 4650
			public UnityEvent onClick;
		}

		// Token: 0x020002DC RID: 732
		public enum ContextItemType
		{
			// Token: 0x0400122C RID: 4652
			BUTTON
		}
	}
}
