using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vectorio.PhasmaUI
{
	// Token: 0x0200028D RID: 653
	public class Button : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0005452B File Offset: 0x0005272B
		public bool CheckImage
		{
			get
			{
				return this.hoverGroup == null;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x00054539 File Offset: 0x00052739
		public bool CheckGroup
		{
			get
			{
				return this.buttonImage == null;
			}
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00054548 File Offset: 0x00052748
		public virtual void Start()
		{
			this._rect = base.GetComponent<RectTransform>();
			this._originalSize = this._rect.localScale;
			this._scaledSize = this._rect.localScale * (1f + this.scaleAdjustment);
			if (this.buttonImage != null)
			{
				this.originalAlpha = this.buttonImage.color.a;
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x000545B8 File Offset: 0x000527B8
		public virtual void ToggleHover(bool toggle)
		{
			if (toggle && this.onHoverSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.onHoverSound);
			}
			if (!this._freezeHover)
			{
				if (this.hoverGroup != null)
				{
					this.hoverGroup.alpha = (toggle ? 1f : 0f);
					this.originalGroup.alpha = (toggle ? 0f : 1f);
				}
				else if (this.buttonImage != null)
				{
					this.buttonImage.color = new Color(this.buttonImage.color.r, this.buttonImage.color.g, this.buttonImage.color.b, toggle ? (this.originalAlpha + this.alphaAdjustment) : this.originalAlpha);
				}
			}
			this.isMouseHovering = toggle;
			if (toggle)
			{
				if (this.onHoverEvent != null)
				{
					this.onHoverEvent.Invoke();
				}
				this._rect.localScale = this._scaledSize;
				return;
			}
			this._rect.localScale = this._originalSize;
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x000546DB File Offset: 0x000528DB
		public virtual void OnClick()
		{
			if (this.onClickSound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.onClickSound);
			}
			if (this.onClickEvent != null)
			{
				this.onClickEvent.Invoke();
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0005470E File Offset: 0x0005290E
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.isMouseHovering)
			{
				this.ToggleHover(true);
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0005471F File Offset: 0x0005291F
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.isMouseHovering)
			{
				this.ToggleHover(false);
			}
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00054730 File Offset: 0x00052930
		public void OnPointerClick(PointerEventData eventData)
		{
			this.OnClick();
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00054738 File Offset: 0x00052938
		public void FreezeHoverGroup(bool toggle)
		{
			if (this._freezeHover && !toggle)
			{
				if (this.hoverGroup != null)
				{
					this.hoverGroup.alpha = 0f;
					this.originalGroup.alpha = 1f;
				}
				else
				{
					this.buttonImage.color = new Color(this.buttonImage.color.r, this.buttonImage.color.g, this.buttonImage.color.b, this.originalAlpha);
				}
			}
			else if (!this._freezeHover && toggle)
			{
				if (this.hoverGroup != null)
				{
					this.hoverGroup.alpha = 1f;
					this.originalGroup.alpha = 0f;
				}
				else
				{
					this.buttonImage.color = new Color(this.buttonImage.color.r, this.buttonImage.color.g, this.buttonImage.color.b, this.originalAlpha + this.alphaAdjustment);
				}
			}
			this._freezeHover = toggle;
		}

		// Token: 0x04001008 RID: 4104
		public UnityEvent onClickEvent;

		// Token: 0x04001009 RID: 4105
		public UnityEvent onHoverEvent;

		// Token: 0x0400100A RID: 4106
		[Range(0f, 1f)]
		public float scaleAdjustment = 0.05f;

		// Token: 0x0400100B RID: 4107
		public Image buttonImage;

		// Token: 0x0400100C RID: 4108
		public CanvasGroup hoverGroup;

		// Token: 0x0400100D RID: 4109
		public CanvasGroup originalGroup;

		// Token: 0x0400100E RID: 4110
		public float alphaAdjustment = 0.1f;

		// Token: 0x0400100F RID: 4111
		public AudioClip onHoverSound;

		// Token: 0x04001010 RID: 4112
		public AudioClip onClickSound;

		// Token: 0x04001011 RID: 4113
		protected Vector3 _originalSize;

		// Token: 0x04001012 RID: 4114
		protected Vector3 _scaledSize;

		// Token: 0x04001013 RID: 4115
		protected RectTransform _rect;

		// Token: 0x04001014 RID: 4116
		protected bool isMouseHovering;

		// Token: 0x04001015 RID: 4117
		protected float originalAlpha;

		// Token: 0x04001016 RID: 4118
		protected bool _freezeHover;
	}
}
