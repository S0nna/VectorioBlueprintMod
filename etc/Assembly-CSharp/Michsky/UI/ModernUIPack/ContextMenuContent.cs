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
	// Token: 0x020002D5 RID: 725
	[AddComponentMenu("Modern UI Pack/Context Menu/Context Menu Content")]
	public class ContextMenuContent : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001443 RID: 5187 RVA: 0x0005CBAC File Offset: 0x0005ADAC
		private void Awake()
		{
			if (this.contextManager == null)
			{
				try
				{
					this.contextManager = (ContextMenuManager)Object.FindObjectsOfType(typeof(ContextMenuManager))[0];
					this.itemParent = this.contextManager.transform.Find("Content/Item List").transform;
				}
				catch
				{
					Debug.LogError("<b>[Context Menu]</b> Context Manager is missing.", this);
					return;
				}
			}
			this.contextAnimator = this.contextManager.contextAnimator;
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0005CC80 File Offset: 0x0005AE80
		private void ProcessClick()
		{
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.contexItems.Count; i++)
			{
				bool flag = false;
				if (this.contexItems[i].contextItemType == ContextMenuContent.ContextItemType.Button && this.contextManager.contextButton != null)
				{
					this.selectedItem = this.contextManager.contextButton;
				}
				else if (this.contexItems[i].contextItemType == ContextMenuContent.ContextItemType.Separator && this.contextManager.contextSeparator != null)
				{
					this.selectedItem = this.contextManager.contextSeparator;
				}
				else
				{
					Debug.LogError("<b>[Context Menu]</b> At least one of the item presets is missing. You can assign a new variable in Resources (Context Menu) tab. All default presets can be found in <b>Modern UI Pack > Prefabs > Context Menu</b> folder.", this);
					flag = true;
				}
				if (!flag)
				{
					if (this.contexItems[i].subMenuItems.Count == 0)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.selectedItem, new Vector3(0f, 0f, 0f), Quaternion.identity);
						gameObject.transform.SetParent(this.itemParent, false);
						if (this.contexItems[i].contextItemType == ContextMenuContent.ContextItemType.Button)
						{
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
						}
					}
					else if (this.contextManager.contextSubMenu != null && this.contexItems[i].subMenuItems.Count != 0)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(this.contextManager.contextSubMenu, new Vector3(0f, 0f, 0f), Quaternion.identity);
						gameObject2.transform.SetParent(this.itemParent, false);
						ContextMenuSubMenu component2 = gameObject2.GetComponent<ContextMenuSubMenu>();
						component2.cmManager = this.contextManager;
						component2.cmContent = this;
						component2.subMenuIndex = i;
						this.setItemText = gameObject2.GetComponentInChildren<TextMeshProUGUI>();
						this.textHelper = this.contexItems[i].itemText;
						this.setItemText.text = this.textHelper;
						Transform transform2 = gameObject2.gameObject.transform.Find("Icon");
						this.setItemImage = transform2.GetComponent<Image>();
						this.imageHelper = this.contexItems[i].itemIcon;
						this.setItemImage.sprite = this.imageHelper;
					}
					base.StopCoroutine("ExecuteAfterTime");
					base.StartCoroutine("ExecuteAfterTime", 0.01f);
				}
			}
			this.contextManager.SetContextMenuPosition();
			this.contextAnimator.Play("Menu In");
			this.contextManager.isOn = true;
			this.contextManager.SetContextMenuPosition();
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0005D04C File Offset: 0x0005B24C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.contextManager.isOn)
			{
				this.contextAnimator.Play("Menu Out");
				this.contextManager.isOn = false;
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right && !this.contextManager.isOn)
			{
				this.ProcessClick();
			}
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0005D09F File Offset: 0x0005B29F
		private IEnumerator ExecuteAfterTime(float time)
		{
			yield return new WaitForSecondsRealtime(time);
			this.itemParent.gameObject.SetActive(false);
			this.itemParent.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0005D0B5 File Offset: 0x0005B2B5
		public void OnMouseOver()
		{
			if (this.useIn3D && Input.GetMouseButtonDown(1))
			{
				this.ProcessClick();
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0005D0CD File Offset: 0x0005B2CD
		public void CloseOnClick()
		{
			this.contextAnimator.Play("Menu Out");
			this.contextManager.isOn = false;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0005D0EC File Offset: 0x0005B2EC
		public void AddNewItem()
		{
			ContextMenuContent.ContextItem item = new ContextMenuContent.ContextItem();
			this.contexItems.Add(item);
		}

		// Token: 0x04001201 RID: 4609
		public ContextMenuManager contextManager;

		// Token: 0x04001202 RID: 4610
		public Transform itemParent;

		// Token: 0x04001203 RID: 4611
		public bool useIn3D;

		// Token: 0x04001204 RID: 4612
		public List<ContextMenuContent.ContextItem> contexItems = new List<ContextMenuContent.ContextItem>();

		// Token: 0x04001205 RID: 4613
		private Animator contextAnimator;

		// Token: 0x04001206 RID: 4614
		private GameObject selectedItem;

		// Token: 0x04001207 RID: 4615
		private Image setItemImage;

		// Token: 0x04001208 RID: 4616
		private TextMeshProUGUI setItemText;

		// Token: 0x04001209 RID: 4617
		private Sprite imageHelper;

		// Token: 0x0400120A RID: 4618
		private string textHelper;

		// Token: 0x020002D6 RID: 726
		[Serializable]
		public class ContextItem
		{
			// Token: 0x0400120B RID: 4619
			[Header("Information")]
			[Space(-5f)]
			public string itemText = "Item Text";

			// Token: 0x0400120C RID: 4620
			public Sprite itemIcon;

			// Token: 0x0400120D RID: 4621
			public ContextMenuContent.ContextItemType contextItemType;

			// Token: 0x0400120E RID: 4622
			[Header("Sub Menu")]
			public List<ContextMenuContent.SubMenuItem> subMenuItems = new List<ContextMenuContent.SubMenuItem>();

			// Token: 0x0400120F RID: 4623
			[Header("Events")]
			public UnityEvent onClick;
		}

		// Token: 0x020002D7 RID: 727
		[Serializable]
		public class SubMenuItem
		{
			// Token: 0x04001210 RID: 4624
			public string itemText = "Item Text";

			// Token: 0x04001211 RID: 4625
			public Sprite itemIcon;

			// Token: 0x04001212 RID: 4626
			public ContextMenuContent.ContextItemType contextItemType;

			// Token: 0x04001213 RID: 4627
			public UnityEvent onClick;
		}

		// Token: 0x020002D8 RID: 728
		public enum ContextItemType
		{
			// Token: 0x04001215 RID: 4629
			Button,
			// Token: 0x04001216 RID: 4630
			Separator
		}
	}
}
