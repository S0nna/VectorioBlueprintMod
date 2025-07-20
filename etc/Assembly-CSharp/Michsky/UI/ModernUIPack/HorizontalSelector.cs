using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000300 RID: 768
	[RequireComponent(typeof(Animator))]
	public class HorizontalSelector : MonoBehaviour
	{
		// Token: 0x06001514 RID: 5396 RVA: 0x00060744 File Offset: 0x0005E944
		private void Awake()
		{
			if (this.selectorAnimator == null)
			{
				this.selectorAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.label == null || this.labelHelper == null)
			{
				Debug.LogError("<b>[Horizontal Selector]</b> Cannot initalize the object due to missing resources.", this);
				return;
			}
			this.SetupSelector();
			this.UpdateContentLayout();
			if (this.invokeAtStart)
			{
				this.itemList[this.index].onItemSelect.Invoke();
				this.onValueChanged.Invoke(this.index);
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000607D8 File Offset: 0x0005E9D8
		private void OnEnable()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine("DisableAnimator");
			}
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x000607F4 File Offset: 0x0005E9F4
		public void SetupSelector()
		{
			if (this.itemList.Count == 0)
			{
				return;
			}
			if (this.saveValue)
			{
				if (PlayerPrefs.HasKey("HorizontalSelector" + this.selectorTag))
				{
					this.defaultIndex = PlayerPrefs.GetInt("HorizontalSelector" + this.selectorTag);
				}
				else
				{
					PlayerPrefs.SetInt("HorizontalSelector" + this.selectorTag, this.defaultIndex);
				}
			}
			this.label.text = this.itemList[this.defaultIndex].itemTitle;
			this.labelHelper.text = this.label.text;
			if (this.labelIcon != null && this.enableIcon)
			{
				this.labelIcon.sprite = this.itemList[this.defaultIndex].itemIcon;
				this.labelIconHelper.sprite = this.labelIcon.sprite;
			}
			else if (!this.enableIcon)
			{
				if (this.labelIcon != null)
				{
					this.labelIcon.gameObject.SetActive(false);
				}
				if (this.labelIconHelper != null)
				{
					this.labelIconHelper.gameObject.SetActive(false);
				}
			}
			this.index = this.defaultIndex;
			if (this.enableIndicators)
			{
				this.UpdateIndicators();
				return;
			}
			Object.Destroy(this.indicatorParent.gameObject);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00060960 File Offset: 0x0005EB60
		public void PreviousClick()
		{
			base.StopCoroutine("DisableAnimator");
			this.selectorAnimator.enabled = true;
			if (!this.loopSelection)
			{
				if (this.index != 0)
				{
					this.labelHelper.text = this.label.text;
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIconHelper.sprite = this.labelIcon.sprite;
					}
					if (this.index == 0)
					{
						this.index = this.itemList.Count - 1;
					}
					else
					{
						this.index--;
					}
					this.label.text = this.itemList[this.index].itemTitle;
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIcon.sprite = this.itemList[this.index].itemIcon;
					}
					this.itemList[this.index].onItemSelect.Invoke();
					this.onValueChanged.Invoke(this.index);
					this.selectorAnimator.Play(null);
					this.selectorAnimator.StopPlayback();
					if (this.invertAnimation)
					{
						this.selectorAnimator.Play("Forward");
					}
					else
					{
						this.selectorAnimator.Play("Previous");
					}
					if (this.saveValue)
					{
						PlayerPrefs.SetInt("HorizontalSelector" + this.selectorTag, this.index);
					}
				}
			}
			else
			{
				this.labelHelper.text = this.label.text;
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIconHelper.sprite = this.labelIcon.sprite;
				}
				if (this.index == 0)
				{
					this.index = this.itemList.Count - 1;
				}
				else
				{
					this.index--;
				}
				this.label.text = this.itemList[this.index].itemTitle;
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIcon.sprite = this.itemList[this.index].itemIcon;
				}
				this.itemList[this.index].onItemSelect.Invoke();
				this.onValueChanged.Invoke(this.index);
				this.selectorAnimator.Play(null);
				this.selectorAnimator.StopPlayback();
				if (this.invertAnimation)
				{
					this.selectorAnimator.Play("Forward");
				}
				else
				{
					this.selectorAnimator.Play("Previous");
				}
				if (this.saveValue)
				{
					PlayerPrefs.SetInt("HorizontalSelector" + this.selectorTag, this.index);
				}
			}
			if (this.saveValue)
			{
				PlayerPrefs.SetInt("HorizontalSelector" + this.selectorTag, this.index);
			}
			if (this.enableIndicators)
			{
				for (int i = 0; i < this.itemList.Count; i++)
				{
					GameObject gameObject = this.indicatorParent.GetChild(i).gameObject;
					Transform transform = gameObject.transform.Find("On");
					Transform transform2 = gameObject.transform.Find("Off");
					if (i == this.index)
					{
						transform.gameObject.SetActive(true);
						transform2.gameObject.SetActive(false);
					}
					else
					{
						transform.gameObject.SetActive(false);
						transform2.gameObject.SetActive(true);
					}
				}
			}
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00060D0C File Offset: 0x0005EF0C
		public void ForwardClick()
		{
			base.StopCoroutine("DisableAnimator");
			this.selectorAnimator.enabled = true;
			if (!this.loopSelection)
			{
				if (this.index != this.itemList.Count - 1)
				{
					this.labelHelper.text = this.label.text;
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIconHelper.sprite = this.labelIcon.sprite;
					}
					if (this.index + 1 >= this.itemList.Count)
					{
						this.index = 0;
					}
					else
					{
						this.index++;
					}
					this.label.text = this.itemList[this.index].itemTitle;
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIcon.sprite = this.itemList[this.index].itemIcon;
					}
					this.itemList[this.index].onItemSelect.Invoke();
					this.onValueChanged.Invoke(this.index);
					this.selectorAnimator.Play(null);
					this.selectorAnimator.StopPlayback();
					if (this.invertAnimation)
					{
						this.selectorAnimator.Play("Previous");
					}
					else
					{
						this.selectorAnimator.Play("Forward");
					}
					if (this.saveValue)
					{
						PlayerPrefs.SetInt("HorizontalSelector" + this.selectorTag, this.index);
					}
				}
			}
			else
			{
				this.labelHelper.text = this.label.text;
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIconHelper.sprite = this.labelIcon.sprite;
				}
				if (this.index + 1 >= this.itemList.Count)
				{
					this.index = 0;
				}
				else
				{
					this.index++;
				}
				this.label.text = this.itemList[this.index].itemTitle;
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIcon.sprite = this.itemList[this.index].itemIcon;
				}
				this.itemList[this.index].onItemSelect.Invoke();
				this.onValueChanged.Invoke(this.index);
				this.selectorAnimator.Play(null);
				this.selectorAnimator.StopPlayback();
				if (this.invertAnimation)
				{
					this.selectorAnimator.Play("Previous");
				}
				else
				{
					this.selectorAnimator.Play("Forward");
				}
				if (this.saveValue)
				{
					PlayerPrefs.SetInt("HorizontalSelector" + this.selectorTag, this.index);
				}
			}
			if (this.saveValue)
			{
				PlayerPrefs.SetInt("HorizontalSelector" + this.selectorTag, this.index);
			}
			if (this.enableIndicators)
			{
				for (int i = 0; i < this.itemList.Count; i++)
				{
					GameObject gameObject = this.indicatorParent.GetChild(i).gameObject;
					Transform transform = gameObject.transform.Find("On");
					Transform transform2 = gameObject.transform.Find("Off");
					if (i == this.index)
					{
						transform.gameObject.SetActive(true);
						transform2.gameObject.SetActive(false);
					}
					else
					{
						transform.gameObject.SetActive(false);
						transform2.gameObject.SetActive(true);
					}
				}
			}
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x000610C4 File Offset: 0x0005F2C4
		public void CreateNewItem(string title)
		{
			HorizontalSelector.Item item = new HorizontalSelector.Item();
			this.newItemTitle = title;
			item.itemTitle = this.newItemTitle;
			this.itemList.Add(item);
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000610F8 File Offset: 0x0005F2F8
		public void RemoveItem(string itemTitle)
		{
			HorizontalSelector.Item item = this.itemList.Find((HorizontalSelector.Item x) => x.itemTitle == itemTitle);
			this.itemList.Remove(item);
			this.SetupSelector();
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00061140 File Offset: 0x0005F340
		public void AddNewItem()
		{
			HorizontalSelector.Item item = new HorizontalSelector.Item();
			this.itemList.Add(item);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00061160 File Offset: 0x0005F360
		public void UpdateUI()
		{
			this.selectorAnimator.enabled = true;
			this.label.text = this.itemList[this.index].itemTitle;
			if (this.labelIcon != null && this.enableIcon)
			{
				this.labelIcon.sprite = this.itemList[this.index].itemIcon;
			}
			this.UpdateContentLayout();
			this.UpdateIndicators();
			base.StartCoroutine("DisableAnimator");
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x000611EC File Offset: 0x0005F3EC
		public void UpdateIndicators()
		{
			if (!this.enableIndicators)
			{
				return;
			}
			foreach (object obj in this.indicatorParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.itemList.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.indicatorObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.indicatorParent, false);
				gameObject.name = this.itemList[i].itemTitle;
				Transform transform = gameObject.transform.Find("On");
				Transform transform2 = gameObject.transform.Find("Off");
				if (i == this.index)
				{
					transform.gameObject.SetActive(true);
					transform2.gameObject.SetActive(false);
				}
				else
				{
					transform.gameObject.SetActive(false);
					transform2.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00061318 File Offset: 0x0005F518
		public void UpdateContentLayout()
		{
			if (this.contentLayout != null)
			{
				this.contentLayout.spacing = (float)this.contentSpacing;
			}
			if (this.contentLayoutHelper != null)
			{
				this.contentLayoutHelper.spacing = (float)this.contentSpacing;
			}
			if (this.labelIcon != null)
			{
				this.labelIcon.transform.localScale = new Vector3(this.iconScale, this.iconScale, this.iconScale);
				this.labelIconHelper.transform.localScale = new Vector3(this.iconScale, this.iconScale, this.iconScale);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.label.transform.parent.GetComponent<RectTransform>());
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x000613DB File Offset: 0x0005F5DB
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSeconds(0.5f);
			this.selectorAnimator.enabled = false;
			yield break;
		}

		// Token: 0x0400130C RID: 4876
		public TextMeshProUGUI label;

		// Token: 0x0400130D RID: 4877
		public TextMeshProUGUI labelHelper;

		// Token: 0x0400130E RID: 4878
		public Image labelIcon;

		// Token: 0x0400130F RID: 4879
		public Image labelIconHelper;

		// Token: 0x04001310 RID: 4880
		public Transform indicatorParent;

		// Token: 0x04001311 RID: 4881
		public GameObject indicatorObject;

		// Token: 0x04001312 RID: 4882
		public Animator selectorAnimator;

		// Token: 0x04001313 RID: 4883
		public HorizontalLayoutGroup contentLayout;

		// Token: 0x04001314 RID: 4884
		public HorizontalLayoutGroup contentLayoutHelper;

		// Token: 0x04001315 RID: 4885
		private string newItemTitle;

		// Token: 0x04001316 RID: 4886
		public bool enableIcon = true;

		// Token: 0x04001317 RID: 4887
		public bool saveValue;

		// Token: 0x04001318 RID: 4888
		public string selectorTag = "Tag Text";

		// Token: 0x04001319 RID: 4889
		public bool enableIndicators = true;

		// Token: 0x0400131A RID: 4890
		public bool invokeAtStart;

		// Token: 0x0400131B RID: 4891
		public bool invertAnimation;

		// Token: 0x0400131C RID: 4892
		public bool loopSelection;

		// Token: 0x0400131D RID: 4893
		[Range(0.25f, 2.5f)]
		public float iconScale = 1f;

		// Token: 0x0400131E RID: 4894
		[Range(1f, 50f)]
		public int contentSpacing = 15;

		// Token: 0x0400131F RID: 4895
		public int defaultIndex;

		// Token: 0x04001320 RID: 4896
		[HideInInspector]
		public int index;

		// Token: 0x04001321 RID: 4897
		public List<HorizontalSelector.Item> itemList = new List<HorizontalSelector.Item>();

		// Token: 0x04001322 RID: 4898
		[Space(8f)]
		public HorizontalSelector.SelectorEvent onValueChanged;

		// Token: 0x02000301 RID: 769
		[Serializable]
		public class SelectorEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000302 RID: 770
		[Serializable]
		public class Item
		{
			// Token: 0x04001323 RID: 4899
			public string itemTitle = "Item Title";

			// Token: 0x04001324 RID: 4900
			public Sprite itemIcon;

			// Token: 0x04001325 RID: 4901
			public UnityEvent onItemSelect = new UnityEvent();
		}
	}
}
