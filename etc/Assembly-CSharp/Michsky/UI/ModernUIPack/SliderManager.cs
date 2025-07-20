using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200032F RID: 815
	[RequireComponent(typeof(Slider))]
	public class SliderManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x00064A10 File Offset: 0x00062C10
		private void Awake()
		{
			if (this.enableSaving)
			{
				if (!PlayerPrefs.HasKey(this.sliderTag + "MUIPSliderValue"))
				{
					this.saveValue = this.mainSlider.value;
				}
				else
				{
					this.saveValue = PlayerPrefs.GetFloat(this.sliderTag + "MUIPSliderValue");
				}
				this.mainSlider.value = this.saveValue;
				this.mainSlider.onValueChanged.AddListener(delegate(float <p0>)
				{
					this.saveValue = this.mainSlider.value;
					PlayerPrefs.SetFloat(this.sliderTag + "MUIPSliderValue", this.saveValue);
				});
			}
			this.mainSlider.onValueChanged.AddListener(delegate(float <p0>)
			{
				this.sliderEvent.Invoke(this.mainSlider.value);
				this.UpdateUI();
			});
			try
			{
				if (this.sliderAnimator == null)
				{
					this.sliderAnimator = base.gameObject.GetComponent<Animator>();
				}
			}
			catch
			{
			}
			if (this.invokeOnAwake)
			{
				this.sliderEvent.Invoke(this.mainSlider.value);
			}
			this.UpdateUI();
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x00064B0C File Offset: 0x00062D0C
		public void UpdateUI()
		{
			if (this.useRoundValue)
			{
				if (this.usePercent)
				{
					if (this.valueText != null)
					{
						this.valueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString() + "%";
					}
					if (this.popupValueText != null)
					{
						this.popupValueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString() + "%";
						return;
					}
				}
				else
				{
					if (this.valueText != null)
					{
						this.valueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString();
					}
					if (this.popupValueText != null)
					{
						this.popupValueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString();
						return;
					}
				}
			}
			else if (this.usePercent)
			{
				if (this.valueText != null)
				{
					this.valueText.text = this.mainSlider.value.ToString("F1") + "%";
				}
				if (this.popupValueText != null)
				{
					this.popupValueText.text = this.mainSlider.value.ToString("F1") + "%";
					return;
				}
			}
			else
			{
				if (this.valueText != null)
				{
					this.valueText.text = this.mainSlider.value.ToString("F1");
				}
				if (this.popupValueText != null)
				{
					this.popupValueText.text = this.mainSlider.value.ToString("F1");
				}
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x00064D0B File Offset: 0x00062F0B
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.showPopupValue)
			{
				this.sliderAnimator.Play("Value In");
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00064D25 File Offset: 0x00062F25
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.showPopupValue)
			{
				this.sliderAnimator.Play("Value Out");
			}
		}

		// Token: 0x0400141E RID: 5150
		public Slider mainSlider;

		// Token: 0x0400141F RID: 5151
		public TextMeshProUGUI valueText;

		// Token: 0x04001420 RID: 5152
		public TextMeshProUGUI popupValueText;

		// Token: 0x04001421 RID: 5153
		public bool enableSaving;

		// Token: 0x04001422 RID: 5154
		public bool invokeOnAwake = true;

		// Token: 0x04001423 RID: 5155
		public string sliderTag = "My Slider";

		// Token: 0x04001424 RID: 5156
		public bool usePercent;

		// Token: 0x04001425 RID: 5157
		public bool showValue = true;

		// Token: 0x04001426 RID: 5158
		public bool showPopupValue = true;

		// Token: 0x04001427 RID: 5159
		public bool useRoundValue;

		// Token: 0x04001428 RID: 5160
		public float minValue;

		// Token: 0x04001429 RID: 5161
		public float maxValue;

		// Token: 0x0400142A RID: 5162
		[SerializeField]
		public SliderManager.SliderEvent onValueChanged = new SliderManager.SliderEvent();

		// Token: 0x0400142B RID: 5163
		[Space(8f)]
		public SliderManager.SliderEvent sliderEvent;

		// Token: 0x0400142C RID: 5164
		[HideInInspector]
		public Animator sliderAnimator;

		// Token: 0x0400142D RID: 5165
		[HideInInspector]
		public float saveValue;

		// Token: 0x02000330 RID: 816
		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}
	}
}
