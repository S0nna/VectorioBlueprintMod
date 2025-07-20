using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000324 RID: 804
	public class Ripple : MonoBehaviour
	{
		// Token: 0x060015A5 RID: 5541 RVA: 0x000632C0 File Offset: 0x000614C0
		private void Start()
		{
			base.transform.localScale = new Vector3(0f, 0f, 0f);
			this.colorImg = base.GetComponent<Image>();
			this.colorImg.raycastTarget = false;
			this.colorImg.color = new Color(this.startColor.r, this.startColor.g, this.startColor.b, this.startColor.a);
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00063340 File Offset: 0x00061540
		private void Update()
		{
			if (!this.unscaledTime)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(this.maxSize, this.maxSize, this.maxSize), Time.deltaTime * this.speed);
				this.colorImg.color = Color.Lerp(this.colorImg.color, new Color(this.transitionColor.r, this.transitionColor.g, this.transitionColor.b, this.transitionColor.a), Time.deltaTime * this.speed);
				if ((double)base.transform.localScale.x >= (double)this.maxSize * 0.998)
				{
					if (base.transform.parent.childCount == 1)
					{
						base.transform.parent.gameObject.SetActive(false);
					}
					Object.Destroy(base.gameObject);
					return;
				}
			}
			else
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(this.maxSize, this.maxSize, this.maxSize), Time.unscaledDeltaTime * this.speed);
				this.colorImg.color = Color.Lerp(this.colorImg.color, new Color(this.transitionColor.r, this.transitionColor.g, this.transitionColor.b, this.transitionColor.a), Time.unscaledDeltaTime * this.speed);
				if ((double)base.transform.localScale.x >= (double)this.maxSize * 0.998)
				{
					if (base.transform.parent.childCount == 1)
					{
						base.transform.parent.gameObject.SetActive(false);
					}
					Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x040013DC RID: 5084
		public bool unscaledTime;

		// Token: 0x040013DD RID: 5085
		public float speed;

		// Token: 0x040013DE RID: 5086
		public float maxSize;

		// Token: 0x040013DF RID: 5087
		public Color startColor;

		// Token: 0x040013E0 RID: 5088
		public Color transitionColor;

		// Token: 0x040013E1 RID: 5089
		private Image colorImg;
	}
}
