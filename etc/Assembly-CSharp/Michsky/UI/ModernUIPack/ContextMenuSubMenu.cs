using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002E1 RID: 737
	public class ContextMenuSubMenu : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x0600146A RID: 5226 RVA: 0x0005DAA3 File Offset: 0x0005BCA3
		private void OnEnable()
		{
			if (this.itemParent == null)
			{
				Debug.Log("<b>[Context Menu]</b> Item Parent is missing.", this);
				return;
			}
			this.listParent = this.itemParent.parent.gameObject.GetComponent<RectTransform>();
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0005DADC File Offset: 0x0005BCDC
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.cmManager.subMenuBehaviour == ContextMenuManager.SubMenuBehaviour.Click)
			{
				if (this.subMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Menu In"))
				{
					this.subMenuAnimator.Play("Menu Out");
					if (this.trigger != null)
					{
						this.trigger.SetActive(false);
						return;
					}
				}
				else
				{
					this.subMenuAnimator.Play("Menu In");
					if (this.trigger != null)
					{
						this.trigger.SetActive(true);
					}
				}
			}
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0005DB68 File Offset: 0x0005BD68
		public void OnPointerEnter(PointerEventData eventData)
		{
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.cmContent.contexItems[this.subMenuIndex].subMenuItems.Count; i++)
			{
				bool flag = false;
				if (this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].contextItemType == ContextMenuContent.ContextItemType.Button && this.cmManager.contextButton != null)
				{
					this.selectedItem = this.cmManager.contextButton;
				}
				else if (this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].contextItemType == ContextMenuContent.ContextItemType.Separator && this.cmManager.contextSeparator != null)
				{
					this.selectedItem = this.cmManager.contextSeparator;
				}
				else
				{
					Debug.LogError("<b>[Context Menu]</b> At least one of the item presets is missing. You can assign a new variable in Resources (Context Menu) tab. All default presets can be found in <b>Modern UI Pack > Prefabs > Context Menu</b> folder.", this);
					flag = true;
				}
				if (!flag)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.selectedItem, new Vector3(0f, 0f, 0f), Quaternion.identity);
					gameObject.transform.SetParent(this.itemParent, false);
					if (this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].contextItemType == ContextMenuContent.ContextItemType.Button)
					{
						this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
						this.textHelper = this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].itemText;
						this.setItemText.text = this.textHelper;
						Transform transform = gameObject.gameObject.transform.Find("Icon");
						this.setItemImage = transform.GetComponent<Image>();
						this.imageHelper = this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].itemIcon;
						this.setItemImage.sprite = this.imageHelper;
						if (this.imageHelper == null)
						{
							this.setItemImage.color = new Color(0f, 0f, 0f, 0f);
						}
						Button component = gameObject.GetComponent<Button>();
						component.onClick.AddListener(new UnityAction(this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].onClick.Invoke));
						component.onClick.AddListener(new UnityAction(this.CloseOnClick));
						base.StartCoroutine(this.ExecuteAfterTime(0.01f));
					}
				}
			}
			if (this.cmManager.autoSubMenuPosition)
			{
				if (this.cmManager.bottomLeft)
				{
					this.listParent.pivot = new Vector2(0f, this.listParent.pivot.y);
				}
				if (this.cmManager.bottomRight)
				{
					this.listParent.pivot = new Vector2(1f, this.listParent.pivot.y);
				}
				if (this.cmManager.topLeft)
				{
					this.listParent.pivot = new Vector2(this.listParent.pivot.x, 0f);
				}
				if (this.cmManager.topRight)
				{
					this.listParent.pivot = new Vector2(this.listParent.pivot.x, 1f);
				}
			}
			if (this.cmManager.subMenuBehaviour == ContextMenuManager.SubMenuBehaviour.Hover)
			{
				this.subMenuAnimator.Play("Menu In");
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00003212 File Offset: 0x00001412
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0005DF48 File Offset: 0x0005C148
		private IEnumerator ExecuteAfterTime(float time)
		{
			yield return new WaitForSecondsRealtime(time);
			this.itemParent.gameObject.SetActive(false);
			this.itemParent.gameObject.SetActive(true);
			base.StopCoroutine(this.ExecuteAfterTime(0.01f));
			base.StopCoroutine("ExecuteAfterTime");
			yield break;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0005DF5E File Offset: 0x0005C15E
		public void CloseOnClick()
		{
			this.cmManager.contextAnimator.Play("Menu Out");
			this.cmManager.isOn = false;
			this.trigger.SetActive(false);
		}

		// Token: 0x04001250 RID: 4688
		public ContextMenuManager cmManager;

		// Token: 0x04001251 RID: 4689
		public ContextMenuContent cmContent;

		// Token: 0x04001252 RID: 4690
		public Animator subMenuAnimator;

		// Token: 0x04001253 RID: 4691
		public Transform itemParent;

		// Token: 0x04001254 RID: 4692
		public GameObject trigger;

		// Token: 0x04001255 RID: 4693
		[HideInInspector]
		public int subMenuIndex;

		// Token: 0x04001256 RID: 4694
		private GameObject selectedItem;

		// Token: 0x04001257 RID: 4695
		private Image setItemImage;

		// Token: 0x04001258 RID: 4696
		private TextMeshProUGUI setItemText;

		// Token: 0x04001259 RID: 4697
		private Sprite imageHelper;

		// Token: 0x0400125A RID: 4698
		private string textHelper;

		// Token: 0x0400125B RID: 4699
		private RectTransform listParent;

		// Token: 0x0400125C RID: 4700
		[HideInInspector]
		public bool enableFadeOut = true;
	}
}
