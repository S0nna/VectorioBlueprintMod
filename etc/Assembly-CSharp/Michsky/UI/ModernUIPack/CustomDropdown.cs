using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002F4 RID: 756
	public class CustomDropdown : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IPointerClickHandler
	{
		// Token: 0x060014E7 RID: 5351 RVA: 0x0005F064 File Offset: 0x0005D264
		private void OnEnable()
		{
			if (this.animationType == CustomDropdown.AnimationType.Stylish)
			{
				return;
			}
			if (this.animationType == CustomDropdown.AnimationType.Modular && this.dropdownAnimator != null)
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

		// Token: 0x060014E8 RID: 5352 RVA: 0x0005F144 File Offset: 0x0005D344
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

		// Token: 0x060014E9 RID: 5353 RVA: 0x0005F1F4 File Offset: 0x0005D3F4
		private void Update()
		{
			if (!this.isInTransition)
			{
				return;
			}
			this.ProcessModularAnimation();
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0005F208 File Offset: 0x0005D408
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

		// Token: 0x060014EB RID: 5355 RVA: 0x0005F35C File Offset: 0x0005D55C
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
			this.index = 0;
			for (int i = 0; i < this.dropdownItems.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.itemParent, false);
				this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
				this.textHelper = this.dropdownItems[i].itemName;
				this.setItemText.text = this.textHelper;
				Transform transform = gameObject.gameObject.transform.Find("Icon");
				this.setItemImage = transform.GetComponent<Image>();
				this.imageHelper = this.dropdownItems[i].itemIcon;
				this.setItemImage.sprite = this.imageHelper;
				this.dropdownItems[i].itemIndex = i;
				CustomDropdown.Item mainItem = this.dropdownItems[i];
				Button component = gameObject.GetComponent<Button>();
				component.onClick.AddListener(new UnityAction(this.Animate));
				component.onClick.AddListener(delegate()
				{
					this.ChangeDropdownInfo(this.index = mainItem.itemIndex);
					this.dropdownEvent.Invoke(this.index = mainItem.itemIndex);
					if (this.saveSelected)
					{
						PlayerPrefs.SetInt("Dropdown" + this.dropdownTag, mainItem.itemIndex);
					}
				});
				component.onClick.AddListener(new UnityAction(this.dropdownItems[i].OnItemSelection.Invoke));
				if (this.invokeAtStart)
				{
					this.dropdownItems[i].OnItemSelection.Invoke();
				}
			}
			if (this.selectedImage != null && !this.enableIcon)
			{
				this.selectedImage.gameObject.SetActive(false);
			}
			try
			{
				this.selectedText.text = this.dropdownItems[this.selectedItemIndex].itemName;
				this.selectedImage.sprite = this.dropdownItems[this.selectedItemIndex].itemIcon;
				this.currentListParent = base.transform.parent;
			}
			catch
			{
				this.selectedText.text = this.dropdownTag;
				this.currentListParent = base.transform.parent;
				Debug.LogWarning("<b>[Dropdown]</b> There is no dropdown items in the list.", this);
				return;
			}
			if (this.saveSelected)
			{
				if (this.invokeAtStart)
				{
					this.dropdownItems[PlayerPrefs.GetInt("Dropdown" + this.dropdownTag)].OnItemSelection.Invoke();
					return;
				}
				this.ChangeDropdownInfo(PlayerPrefs.GetInt("Dropdown" + this.dropdownTag));
			}
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0005F6C8 File Offset: 0x0005D8C8
		public void ChangeDropdownInfo(int itemIndex)
		{
			if (this.selectedImage != null && this.enableIcon)
			{
				this.selectedImage.sprite = this.dropdownItems[itemIndex].itemIcon;
			}
			if (this.selectedText != null)
			{
				this.selectedText.text = this.dropdownItems[itemIndex].itemName;
			}
			if (this.enableDropdownSounds && this.useClickSound)
			{
				this.soundSource.PlayOneShot(this.clickSound);
			}
			this.selectedItemIndex = itemIndex;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0005F75C File Offset: 0x0005D95C
		public void Animate()
		{
			if (!this.isOn && this.animationType == CustomDropdown.AnimationType.Modular)
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
			else if (this.isOn && this.animationType == CustomDropdown.AnimationType.Modular)
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
			else if (!this.isOn && this.animationType == CustomDropdown.AnimationType.Stylish)
			{
				this.dropdownAnimator.Play("Stylish In");
				this.isOn = true;
				if (this.isListItem)
				{
					this.siblingIndex = base.transform.GetSiblingIndex();
					base.gameObject.transform.SetParent(this.listParent, true);
				}
			}
			else if (this.isOn && this.animationType == CustomDropdown.AnimationType.Stylish)
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

		// Token: 0x060014EE RID: 5358 RVA: 0x0005F97C File Offset: 0x0005DB7C
		public void CreateNewItem(string title, Sprite icon)
		{
			CustomDropdown.Item item = new CustomDropdown.Item();
			item.itemName = title;
			item.itemIcon = icon;
			this.dropdownItems.Add(item);
			this.SetupDropdown();
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0005F9B0 File Offset: 0x0005DBB0
		public void CreateNewItemFast(string title, Sprite icon)
		{
			CustomDropdown.Item item = new CustomDropdown.Item();
			item.itemName = title;
			item.itemIcon = icon;
			this.dropdownItems.Add(item);
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0005F9E0 File Offset: 0x0005DBE0
		public void RemoveItem(string itemTitle)
		{
			CustomDropdown.Item item = this.dropdownItems.Find((CustomDropdown.Item x) => x.itemName == itemTitle);
			this.dropdownItems.Remove(item);
			this.SetupDropdown();
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0005FA28 File Offset: 0x0005DC28
		public void AddNewItem()
		{
			CustomDropdown.Item item = new CustomDropdown.Item();
			this.dropdownItems.Add(item);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0005FA48 File Offset: 0x0005DC48
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

		// Token: 0x060014F3 RID: 5363 RVA: 0x0005FACD File Offset: 0x0005DCCD
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.enableDropdownSounds && this.useClickSound)
			{
				this.soundSource.PlayOneShot(this.clickSound);
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0005FAF0 File Offset: 0x0005DCF0
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableDropdownSounds && this.useHoverSound)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0005FB13 File Offset: 0x0005DD13
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

		// Token: 0x040012A0 RID: 4768
		public Animator dropdownAnimator;

		// Token: 0x040012A1 RID: 4769
		public GameObject triggerObject;

		// Token: 0x040012A2 RID: 4770
		public TextMeshProUGUI selectedText;

		// Token: 0x040012A3 RID: 4771
		public Image selectedImage;

		// Token: 0x040012A4 RID: 4772
		public Transform itemParent;

		// Token: 0x040012A5 RID: 4773
		public GameObject itemObject;

		// Token: 0x040012A6 RID: 4774
		public GameObject scrollbar;

		// Token: 0x040012A7 RID: 4775
		public VerticalLayoutGroup itemList;

		// Token: 0x040012A8 RID: 4776
		public Transform listParent;

		// Token: 0x040012A9 RID: 4777
		public AudioSource soundSource;

		// Token: 0x040012AA RID: 4778
		[HideInInspector]
		public Transform currentListParent;

		// Token: 0x040012AB RID: 4779
		public bool enableIcon = true;

		// Token: 0x040012AC RID: 4780
		public bool enableTrigger = true;

		// Token: 0x040012AD RID: 4781
		public bool enableScrollbar = true;

		// Token: 0x040012AE RID: 4782
		public bool setHighPriorty = true;

		// Token: 0x040012AF RID: 4783
		public bool outOnPointerExit;

		// Token: 0x040012B0 RID: 4784
		public bool isListItem;

		// Token: 0x040012B1 RID: 4785
		public bool invokeAtStart;

		// Token: 0x040012B2 RID: 4786
		public bool initAtStart = true;

		// Token: 0x040012B3 RID: 4787
		public bool enableDropdownSounds;

		// Token: 0x040012B4 RID: 4788
		public bool useHoverSound = true;

		// Token: 0x040012B5 RID: 4789
		public bool useClickSound = true;

		// Token: 0x040012B6 RID: 4790
		[Range(1f, 50f)]
		public int itemPaddingTop = 8;

		// Token: 0x040012B7 RID: 4791
		[Range(1f, 50f)]
		public int itemPaddingBottom = 8;

		// Token: 0x040012B8 RID: 4792
		[Range(1f, 50f)]
		public int itemPaddingLeft = 8;

		// Token: 0x040012B9 RID: 4793
		[Range(1f, 50f)]
		public int itemPaddingRight = 25;

		// Token: 0x040012BA RID: 4794
		[Range(1f, 50f)]
		public int itemSpacing = 8;

		// Token: 0x040012BB RID: 4795
		public int selectedItemIndex;

		// Token: 0x040012BC RID: 4796
		public CustomDropdown.AnimationType animationType;

		// Token: 0x040012BD RID: 4797
		[Range(1f, 25f)]
		public float transitionSmoothness = 10f;

		// Token: 0x040012BE RID: 4798
		[Range(1f, 25f)]
		public float sizeSmoothness = 15f;

		// Token: 0x040012BF RID: 4799
		public float panelSize = 200f;

		// Token: 0x040012C0 RID: 4800
		public RectTransform listRect;

		// Token: 0x040012C1 RID: 4801
		public CanvasGroup listCG;

		// Token: 0x040012C2 RID: 4802
		private bool isInTransition;

		// Token: 0x040012C3 RID: 4803
		private float closeOn;

		// Token: 0x040012C4 RID: 4804
		public bool saveSelected;

		// Token: 0x040012C5 RID: 4805
		public string dropdownTag = "Dropdown";

		// Token: 0x040012C6 RID: 4806
		[SerializeField]
		public List<CustomDropdown.Item> dropdownItems = new List<CustomDropdown.Item>();

		// Token: 0x040012C7 RID: 4807
		[Space(8f)]
		public CustomDropdown.DropdownEvent dropdownEvent;

		// Token: 0x040012C8 RID: 4808
		public AudioClip hoverSound;

		// Token: 0x040012C9 RID: 4809
		public AudioClip clickSound;

		// Token: 0x040012CA RID: 4810
		[HideInInspector]
		public bool isOn;

		// Token: 0x040012CB RID: 4811
		[HideInInspector]
		public int index;

		// Token: 0x040012CC RID: 4812
		[HideInInspector]
		public int siblingIndex;

		// Token: 0x040012CD RID: 4813
		[HideInInspector]
		public TextMeshProUGUI setItemText;

		// Token: 0x040012CE RID: 4814
		[HideInInspector]
		public Image setItemImage;

		// Token: 0x040012CF RID: 4815
		private EventTrigger triggerEvent;

		// Token: 0x040012D0 RID: 4816
		private Sprite imageHelper;

		// Token: 0x040012D1 RID: 4817
		private string textHelper;

		// Token: 0x020002F5 RID: 757
		[Serializable]
		public class DropdownEvent : UnityEvent<int>
		{
		}

		// Token: 0x020002F6 RID: 758
		public enum AnimationType
		{
			// Token: 0x040012D3 RID: 4819
			Modular,
			// Token: 0x040012D4 RID: 4820
			Stylish
		}

		// Token: 0x020002F7 RID: 759
		[Serializable]
		public class Item
		{
			// Token: 0x040012D5 RID: 4821
			public string itemName = "Dropdown Item";

			// Token: 0x040012D6 RID: 4822
			public Sprite itemIcon;

			// Token: 0x040012D7 RID: 4823
			[HideInInspector]
			public int itemIndex;

			// Token: 0x040012D8 RID: 4824
			public UnityEvent OnItemSelection = new UnityEvent();
		}
	}
}
