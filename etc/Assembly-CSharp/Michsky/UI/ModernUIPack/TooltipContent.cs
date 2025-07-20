using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000335 RID: 821
	[AddComponentMenu("Modern UI Pack/Tooltip/Tooltip Content")]
	public class TooltipContent : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06001602 RID: 5634 RVA: 0x00065448 File Offset: 0x00063648
		private void Start()
		{
			if (this.tooltipRect == null || this.descriptionText == null)
			{
				try
				{
					this.tooltipRect = GameObject.Find("Tooltip Rect");
					this.descriptionText = this.tooltipRect.transform.GetComponentInChildren<TextMeshProUGUI>();
				}
				catch
				{
					Debug.LogError("<b>[Tooltip Content]</b> Tooltip Rect is missing.", this);
					return;
				}
			}
			if (this.tooltipRect != null)
			{
				this.tpManager = this.tooltipRect.GetComponentInParent<TooltipManager>();
				this.tooltipAnimator = this.tooltipRect.GetComponentInParent<Animator>();
			}
			if (this.tpManager.contentLE == null)
			{
				this.tpManager.contentLE = this.descriptionText.GetComponent<LayoutElement>();
			}
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00065514 File Offset: 0x00063714
		private void ProcessEnter()
		{
			if (this.tooltipRect == null)
			{
				return;
			}
			this.descriptionText.text = this.description;
			this.tpManager.allowUpdating = true;
			this.CheckForContentWidth();
			base.StopCoroutine("DisableAnimator");
			this.tooltipAnimator.gameObject.SetActive(false);
			this.tooltipAnimator.gameObject.SetActive(true);
			if (this.delay == 0f)
			{
				this.tooltipAnimator.Play("In");
			}
			else
			{
				base.StartCoroutine("ShowTooltip");
			}
			if (this.forceToUpdate)
			{
				base.StartCoroutine("UpdateLayoutPosition");
			}
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x000655C0 File Offset: 0x000637C0
		private void ProcessExit()
		{
			if (this.tooltipRect == null)
			{
				return;
			}
			if (this.delay != 0f)
			{
				base.StopCoroutine("ShowTooltip");
				if (this.tooltipAnimator.GetCurrentAnimatorStateInfo(0).IsName("In"))
				{
					this.tooltipAnimator.Play("Out");
				}
			}
			else
			{
				this.tooltipAnimator.Play("Out");
			}
			this.tpManager.allowUpdating = false;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0006563D File Offset: 0x0006383D
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.ProcessEnter();
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00065645 File Offset: 0x00063845
		public void OnPointerExit(PointerEventData eventData)
		{
			this.ProcessExit();
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0006564D File Offset: 0x0006384D
		public void OnMouseEnter()
		{
			if (this.useIn3D)
			{
				this.ProcessEnter();
			}
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0006565D File Offset: 0x0006385D
		public void OnMouseExit()
		{
			if (this.useIn3D)
			{
				this.ProcessExit();
			}
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0006566D File Offset: 0x0006386D
		public void CheckForContentWidth()
		{
			this.LayoutElementCreator();
			base.StartCoroutine("CalculateContentWidth");
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00065684 File Offset: 0x00063884
		private void LayoutElementCreator()
		{
			if (this.tpManager.contentLE == null)
			{
				this.descriptionText.gameObject.AddComponent<LayoutElement>();
				this.tpManager.contentLE = this.descriptionText.GetComponent<LayoutElement>();
			}
			this.tpManager.contentLE.preferredWidth = this.tpManager.preferredWidth;
			this.tpManager.contentLE.enabled = false;
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x000656F7 File Offset: 0x000638F7
		private IEnumerator CalculateContentWidth()
		{
			yield return new WaitForSecondsRealtime(0.05f);
			if (this.descriptionText.GetComponent<RectTransform>().sizeDelta.x >= this.tpManager.preferredWidth + 1f)
			{
				this.tpManager.contentLE.enabled = true;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.tpManager.contentLE.gameObject.GetComponent<RectTransform>());
			this.tpManager.contentLE.preferredWidth = this.tpManager.preferredWidth;
			yield break;
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00065706 File Offset: 0x00063906
		private IEnumerator ShowTooltip()
		{
			yield return new WaitForSeconds(this.delay);
			this.tooltipAnimator.Play("In");
			base.StopCoroutine("ShowTooltip");
			yield break;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00065715 File Offset: 0x00063915
		private IEnumerator UpdateLayoutPosition()
		{
			yield return new WaitForSecondsRealtime(0.05f);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.tooltipAnimator.gameObject.GetComponent<RectTransform>());
			yield break;
		}

		// Token: 0x04001445 RID: 5189
		[Header("Content")]
		[TextArea]
		public string description;

		// Token: 0x04001446 RID: 5190
		public float delay;

		// Token: 0x04001447 RID: 5191
		[Header("Resources")]
		public GameObject tooltipRect;

		// Token: 0x04001448 RID: 5192
		public TextMeshProUGUI descriptionText;

		// Token: 0x04001449 RID: 5193
		[Header("Settings")]
		public bool forceToUpdate;

		// Token: 0x0400144A RID: 5194
		public bool useIn3D;

		// Token: 0x0400144B RID: 5195
		private TooltipManager tpManager;

		// Token: 0x0400144C RID: 5196
		[HideInInspector]
		public Animator tooltipAnimator;
	}
}
