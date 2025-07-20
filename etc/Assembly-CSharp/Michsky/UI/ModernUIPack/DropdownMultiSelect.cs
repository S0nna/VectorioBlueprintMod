using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002FA RID: 762
	public class DropdownMultiSelect : MonoBehaviour, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x060014FE RID: 5374 RVA: 0x0005FCC4 File Offset: 0x0005DEC4
		private void OnEnable()
		{
			if (this.animationType == DropdownMultiSelect.AnimationType.Stylish)
			{
				return;
			}
			if (this.animationType == DropdownMultiSelect.AnimationType.Modular && this.dropdownAnimator != null)
			{
				Object.Destroy(this.dropdownAnimator);
			}
			if (this.listCG == null)
			{
				this.listCG = base.gameObject.GetComponentInChildren<CanvasGroup>();
			}
			this.listCG.alpha = 0f;
			this.listCG.interactable = false;
			this.listCG.blocksRaycasts = false;
			if (this.listRect == null)
			{
				this.listRect = this.listCG.GetComponent<RectTransform>();
			}
			this.closeOn = base.gameObject.GetComponent<RectTransform>().sizeDelta.y;
			this.listRect.sizeDelta = new Vector2(this.listRect.sizeDelta.x, this.closeOn);
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0005FDA4 File Offset: 0x0005DFA4
		private void Awake()
		{
			try
			{
				if (this.initAtStart)
				{
					this.SetupDropdown();
				}
				this.currentListParent = base.transform.parent;
				if (this.enableTrigger && this.triggerObject != null)
				{
					this.triggerEvent = this.triggerObject.AddComponent<EventTrigger>();
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.eventID = EventTriggerType.PointerClick;
					entry.callback.AddListener(delegate(BaseEventData eventData)
					{
						this.Animate();
					});
					this.triggerEvent.GetComponent<EventTrigger>().triggers.Add(entry);
				}
			}
			catch
			{
				Debug.LogError("<b>[Dropdown]</b> Cannot initalize the object due to missing resources.", this);
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0005FE54 File Offset: 0x0005E054
		private void Update()
		{
			if (!this.isInTransition)
			{
				return;
			}
			this.ProcessModularAnimation();
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0005FE68 File Offset: 0x0005E068
		private void ProcessModularAnimation()
		{
			if (this.isOn)
			{
				this.listCG.alpha += Time.unscaledDeltaTime * this.transitionSmoothness;
				this.listRect.sizeDelta = Vector2.Lerp(this.listRect.sizeDelta, new Vector2(this.listRect.sizeDelta.x, this.panelSize), Time.unscaledDeltaTime * this.sizeSmoothness);
				if (this.listRect.sizeDelta.y >= this.panelSize - 0.1f && this.listCG.alpha >= 1f)
				{
					this.isInTransition = false;
					return;
				}
			}
			else
			{
				this.listCG.alpha -= Time.unscaledDeltaTime * this.transitionSmoothness;
				this.listRect.sizeDelta = Vector2.Lerp(this.listRect.sizeDelta, new Vector2(this.listRect.sizeDelta.x, this.closeOn), Time.unscaledDeltaTime * this.sizeSmoothness);
				if (this.listRect.sizeDelta.y <= this.closeOn + 0.1f && this.listCG.alpha <= 0f)
				{
					this.isInTransition = false;
					base.enabled = false;
				}
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0005FFBC File Offset: 0x0005E1BC
		public void SetupDropdown()
		{
			if (this.dropdownAnimator == null)
			{
				this.dropdownAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (!this.enableScrollbar && this.scrollbar != null)
			{
				Object.Destroy(this.scrollbar);
			}
			if (this.setHighPriorty)
			{
				base.transform.SetAsLastSibling();
			}
			if (this.itemList == null)
			{
				this.itemList = this.itemParent.GetComponent<VerticalLayoutGroup>();
			}
			this.UpdateItemLayout();
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.dropdownItems.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.itemParent, false);
				this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
				this.textHelper = this.dropdownItems[i].itemName;
				this.setItemText.text = this.textHelper;
				this.dropdownItems[i].itemIndex = i;
				DropdownMultiSelect.Item mainItem = this.dropdownItems[i];
				Toggle component = gameObject.GetComponent<Toggle>();
				component.onValueChanged.AddListener(delegate(bool <p0>)
				{
					this.UpdateToggleData(mainItem.itemIndex);
				});
				component.onValueChanged.AddListener(new UnityAction<bool>(this.UpdateToggle));
				component.onValueChanged.AddListener(new UnityAction<bool>(this.dropdownItems[i].onValueChanged.Invoke));
				if (this.dropdownItems[i].isOn)
				{
					component.isOn = true;
				}
				else
				{
					component.isOn = false;
				}
				if (this.invokeAtStart)
				{
					if (this.dropdownItems[i].isOn)
					{
						this.dropdownItems[i].onValueChanged.Invoke(true);
					}
					else
					{
						this.dropdownItems[i].onValueChanged.Invoke(false);
					}
				}
			}
			this.currentListParent = base.transform.parent;
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0006022C File Offset: 0x0005E42C
		private void UpdateToggle(bool value)
		{
			if (value)
			{
				this.currentToggle.isOn = true;
				this.dropdownItems[this.currentIndex].isOn = true;
				return;
			}
			this.currentToggle.isOn = false;
			this.dropdownItems[this.currentIndex].isOn = false;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00060283 File Offset: 0x0005E483
		private void UpdateToggleData(int itemIndex)
		{
			this.currentIndex = itemIndex;
			this.currentToggle = this.itemParent.GetChild(this.currentIndex).GetComponent<Toggle>();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000602A8 File Offset: 0x0005E4A8
		public void Animate()
		{
			if (!this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Modular)
			{
				this.isOn = true;
				this.isInTransition = true;
				base.enabled = true;
				this.listCG.blocksRaycasts = true;
				this.listCG.interactable = true;
				if (this.isListItem)
				{
					this.siblingIndex = base.transform.GetSiblingIndex();
					base.gameObject.transform.SetParent(this.listParent, true);
				}
			}
			else if (this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Modular)
			{
				this.isOn = false;
				this.isInTransition = true;
				base.enabled = true;
				this.listCG.blocksRaycasts = false;
				this.listCG.interactable = false;
				if (this.isListItem)
				{
					base.gameObject.transform.SetParent(this.currentListParent, true);
					base.gameObject.transform.SetSiblingIndex(this.siblingIndex);
				}
			}
			else if (!this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Stylish)
			{
				this.dropdownAnimator.Play("Stylish In");
				this.isOn = true;
				if (this.isListItem)
				{
					this.siblingIndex = base.transform.GetSiblingIndex();
					base.gameObject.transform.SetParent(this.listParent, true);
				}
			}
			else if (this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Stylish)
			{
				this.dropdownAnimator.Play("Stylish Out");
				this.isOn = false;
				if (this.isListItem)
				{
					base.gameObject.transform.SetParent(this.currentListParent, true);
					base.gameObject.transform.SetSiblingIndex(this.siblingIndex);
				}
			}
			if (this.enableTrigger && !this.isOn)
			{
				this.triggerObject.SetActive(false);
			}
			else if (this.enableTrigger && this.isOn)
			{
				this.triggerObject.SetActive(true);
			}
			if (this.enableTrigger && this.outOnPointerExit)
			{
				this.triggerObject.SetActive(false);
			}
			if (this.setHighPriorty)
			{
				base.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x000604C8 File Offset: 0x0005E6C8
		public void CreateNewItem(string title, bool value)
		{
			DropdownMultiSelect.Item item = new DropdownMultiSelect.Item();
			item.itemName = title;
			item.isOn = value;
			this.dropdownItems.Add(item);
			this.SetupDropdown();
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x000604FC File Offset: 0x0005E6FC
		public void CreateNewItemFast(string title, bool value)
		{
			DropdownMultiSelect.Item item = new DropdownMultiSelect.Item();
			item.itemName = title;
			item.isOn = value;
			this.dropdownItems.Add(item);
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0006052C File Offset: 0x0005E72C
		public void RemoveItem(string itemTitle)
		{
			DropdownMultiSelect.Item item = this.dropdownItems.Find((DropdownMultiSelect.Item x) => x.itemName == itemTitle);
			this.dropdownItems.Remove(item);
			this.SetupDropdown();
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00060574 File Offset: 0x0005E774
		public void AddNewItem()
		{
			DropdownMultiSelect.Item item = new DropdownMultiSelect.Item();
			this.dropdownItems.Add(item);
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00060594 File Offset: 0x0005E794
		public void UpdateItemLayout()
		{
			if (this.itemList != null)
			{
				this.itemList.spacing = (float)this.itemSpacing;
				this.itemList.padding.top = this.itemPaddingTop;
				this.itemList.padding.bottom = this.itemPaddingBottom;
				this.itemList.padding.left = this.itemPaddingLeft;
				this.itemList.padding.right = this.itemPaddingRight;
			}
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00060619 File Offset: 0x0005E819
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.outOnPointerExit && this.isOn)
			{
				this.Animate();
				this.isOn = false;
				if (this.isListItem)
				{
					base.gameObject.transform.SetParent(this.currentListParent, true);
				}
			}
		}

		// Token: 0x040012DC RID: 4828
		public GameObject triggerObject;

		// Token: 0x040012DD RID: 4829
		public Transform itemParent;

		// Token: 0x040012DE RID: 4830
		public GameObject itemObject;

		// Token: 0x040012DF RID: 4831
		public GameObject scrollbar;

		// Token: 0x040012E0 RID: 4832
		private VerticalLayoutGroup itemList;

		// Token: 0x040012E1 RID: 4833
		private Transform currentListParent;

		// Token: 0x040012E2 RID: 4834
		public Transform listParent;

		// Token: 0x040012E3 RID: 4835
		private Animator dropdownAnimator;

		// Token: 0x040012E4 RID: 4836
		public TextMeshProUGUI setItemText;

		// Token: 0x040012E5 RID: 4837
		public bool initAtStart = true;

		// Token: 0x040012E6 RID: 4838
		public bool enableIcon = true;

		// Token: 0x040012E7 RID: 4839
		public bool enableTrigger = true;

		// Token: 0x040012E8 RID: 4840
		public bool enableScrollbar = true;

		// Token: 0x040012E9 RID: 4841
		public bool setHighPriorty = true;

		// Token: 0x040012EA RID: 4842
		public bool outOnPointerExit;

		// Token: 0x040012EB RID: 4843
		public bool isListItem;

		// Token: 0x040012EC RID: 4844
		[Range(1f, 50f)]
		public int itemPaddingTop = 8;

		// Token: 0x040012ED RID: 4845
		[Range(1f, 50f)]
		public int itemPaddingBottom = 8;

		// Token: 0x040012EE RID: 4846
		[Range(1f, 50f)]
		public int itemPaddingLeft = 8;

		// Token: 0x040012EF RID: 4847
		[Range(1f, 50f)]
		public int itemPaddingRight = 25;

		// Token: 0x040012F0 RID: 4848
		[Range(1f, 50f)]
		public int itemSpacing = 8;

		// Token: 0x040012F1 RID: 4849
		public DropdownMultiSelect.AnimationType animationType;

		// Token: 0x040012F2 RID: 4850
		[Range(1f, 25f)]
		public float transitionSmoothness = 10f;

		// Token: 0x040012F3 RID: 4851
		[Range(1f, 25f)]
		public float sizeSmoothness = 15f;

		// Token: 0x040012F4 RID: 4852
		public float panelSize = 200f;

		// Token: 0x040012F5 RID: 4853
		public RectTransform listRect;

		// Token: 0x040012F6 RID: 4854
		public CanvasGroup listCG;

		// Token: 0x040012F7 RID: 4855
		private bool isInTransition;

		// Token: 0x040012F8 RID: 4856
		private float closeOn;

		// Token: 0x040012F9 RID: 4857
		public bool invokeAtStart;

		// Token: 0x040012FA RID: 4858
		public string toggleTag = "Multi Dropdown";

		// Token: 0x040012FB RID: 4859
		[SerializeField]
		public List<DropdownMultiSelect.Item> dropdownItems = new List<DropdownMultiSelect.Item>();

		// Token: 0x040012FC RID: 4860
		private int currentIndex;

		// Token: 0x040012FD RID: 4861
		private Toggle currentToggle;

		// Token: 0x040012FE RID: 4862
		private string textHelper;

		// Token: 0x040012FF RID: 4863
		private bool isOn;

		// Token: 0x04001300 RID: 4864
		public int siblingIndex;

		// Token: 0x04001301 RID: 4865
		private EventTrigger triggerEvent;

		// Token: 0x020002FB RID: 763
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}

		// Token: 0x020002FC RID: 764
		public enum AnimationType
		{
			// Token: 0x04001303 RID: 4867
			Modular,
			// Token: 0x04001304 RID: 4868
			Stylish
		}

		// Token: 0x020002FD RID: 765
		[Serializable]
		public class Item
		{
			// Token: 0x04001305 RID: 4869
			public string itemName = "Dropdown Item";

			// Token: 0x04001306 RID: 4870
			public bool isOn;

			// Token: 0x04001307 RID: 4871
			[HideInInspector]
			public int itemIndex;

			// Token: 0x04001308 RID: 4872
			[SerializeField]
			public DropdownMultiSelect.ToggleEvent onValueChanged = new DropdownMultiSelect.ToggleEvent();
		}
	}
}
