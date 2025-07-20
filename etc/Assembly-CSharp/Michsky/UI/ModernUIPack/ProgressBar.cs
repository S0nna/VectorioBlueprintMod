using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000322 RID: 802
	public class ProgressBar : MonoBehaviour
	{
		// Token: 0x0600159D RID: 5533 RVA: 0x00062F81 File Offset: 0x00061181
		private void Start()
		{
			this.UpdateUI();
			this.InitializeEvents();
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00062F90 File Offset: 0x00061190
		private void Update()
		{
			if (!this.isOn)
			{
				return;
			}
			if (this.currentPercent <= this.maxValue && !this.invert)
			{
				this.currentPercent += (float)this.speed * Time.deltaTime;
			}
			else if (this.currentPercent >= this.minValue && this.invert)
			{
				this.currentPercent -= (float)this.speed * Time.deltaTime;
			}
			if (this.currentPercent >= this.maxValue && this.speed != 0 && this.restart && !this.invert)
			{
				this.currentPercent = 0f;
			}
			else if (this.currentPercent <= this.minValue && this.speed != 0 && this.restart && this.invert)
			{
				this.currentPercent = this.maxValue;
			}
			else if (this.currentPercent >= this.maxValue && this.speed != 0 && !this.restart && !this.invert)
			{
				this.currentPercent = this.maxValue;
			}
			else if (this.currentPercent <= this.minValue && this.speed != 0 && !this.restart && this.invert)
			{
				this.currentPercent = this.minValue;
			}
			this.UpdateUI();
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x000630E0 File Offset: 0x000612E0
		public void UpdateUI()
		{
			this.loadingBar.fillAmount = this.currentPercent / this.maxValue;
			if (this.addSuffix)
			{
				this.textPercent.text = this.currentPercent.ToString("F" + this.decimals.ToString()) + this.suffix;
			}
			else
			{
				this.textPercent.text = this.currentPercent.ToString("F" + this.decimals.ToString());
			}
			if (this.addPrefix)
			{
				this.textPercent.text = this.prefix + this.textPercent.text;
			}
			if (this.eventSource != null)
			{
				this.eventSource.value = this.currentPercent;
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x000631B8 File Offset: 0x000613B8
		public void InitializeEvents()
		{
			if (Application.isPlaying && this.onValueChanged.GetPersistentEventCount() != 0)
			{
				if (this.eventSource == null)
				{
					this.eventSource = (base.gameObject.AddComponent(typeof(Slider)) as Slider);
				}
				this.eventSource.transition = Selectable.Transition.None;
				this.eventSource.minValue = this.minValue;
				this.eventSource.maxValue = this.maxValue;
				this.eventSource.onValueChanged.AddListener(new UnityAction<float>(this.onValueChanged.Invoke));
			}
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00063259 File Offset: 0x00061459
		public void ClearEvents()
		{
			this.eventSource.onValueChanged.RemoveAllListeners();
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0006326B File Offset: 0x0006146B
		public void ChangeValue(float newValue)
		{
			this.currentPercent = newValue;
			this.UpdateUI();
		}

		// Token: 0x040013CA RID: 5066
		public float currentPercent;

		// Token: 0x040013CB RID: 5067
		[Range(0f, 100f)]
		public int speed;

		// Token: 0x040013CC RID: 5068
		public float minValue;

		// Token: 0x040013CD RID: 5069
		public float maxValue = 100f;

		// Token: 0x040013CE RID: 5070
		public float valueLimit = 100f;

		// Token: 0x040013CF RID: 5071
		public Image loadingBar;

		// Token: 0x040013D0 RID: 5072
		public TextMeshProUGUI textPercent;

		// Token: 0x040013D1 RID: 5073
		public bool isOn;

		// Token: 0x040013D2 RID: 5074
		public bool restart;

		// Token: 0x040013D3 RID: 5075
		public bool invert;

		// Token: 0x040013D4 RID: 5076
		public bool addPrefix;

		// Token: 0x040013D5 RID: 5077
		public bool addSuffix = true;

		// Token: 0x040013D6 RID: 5078
		public string prefix = "";

		// Token: 0x040013D7 RID: 5079
		public string suffix = "%";

		// Token: 0x040013D8 RID: 5080
		public bool isLooped;

		// Token: 0x040013D9 RID: 5081
		[Range(0f, 5f)]
		public int decimals;

		// Token: 0x040013DA RID: 5082
		public ProgressBar.ProgressBarEvent onValueChanged;

		// Token: 0x040013DB RID: 5083
		[HideInInspector]
		public Slider eventSource;

		// Token: 0x02000323 RID: 803
		[Serializable]
		public class ProgressBarEvent : UnityEvent<float>
		{
		}
	}
}
