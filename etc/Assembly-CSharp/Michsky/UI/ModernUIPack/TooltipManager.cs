using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000339 RID: 825
	public class TooltipManager : MonoBehaviour
	{
		// Token: 0x06001621 RID: 5665 RVA: 0x000658F4 File Offset: 0x00063AF4
		private void Awake()
		{
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			if (component == null)
			{
				Debug.LogError("<b>[Tooltip]</b> Rect Transform is missing from the object.", this);
				return;
			}
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMin = new Vector2(0f, 0f);
			component.offsetMax = new Vector2(0f, 0f);
			this.tooltipContent.GetComponent<RectTransform>().pivot = new Vector2(0f, this.tooltipContent.GetComponent<RectTransform>().pivot.y);
			this.tooltipContent.GetComponent<RectTransform>().pivot = new Vector2(this.tooltipContent.GetComponent<RectTransform>().pivot.x, 0f);
			if (this.mainCanvas == null)
			{
				this.mainCanvas = base.gameObject.GetComponentInParent<Canvas>();
			}
			if (this.cameraSource == TooltipManager.CameraSource.Main)
			{
				this.targetCamera = Camera.main;
			}
			this.contentRect = this.tooltipContent.GetComponentInParent<RectTransform>();
			this.tooltipRect = this.tooltipObject.GetComponent<RectTransform>();
			this.contentPos = new Vector3((float)this.vBorderTop, (float)this.hBorderLeft, 0f);
			base.gameObject.transform.SetAsLastSibling();
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00065A56 File Offset: 0x00063C56
		private void Update()
		{
			if (!this.allowUpdating)
			{
				return;
			}
			this.CheckForPosition();
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00065A68 File Offset: 0x00063C68
		private void CheckForPosition()
		{
			this.cursorPos = Input.mousePosition;
			this.uiPos = this.tooltipRect.anchoredPosition;
			this.CheckForBounds();
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || this.mainCanvas.renderMode == RenderMode.WorldSpace)
			{
				this.tooltipRect.position = this.targetCamera.ScreenToWorldPoint(this.cursorPos);
				this.tooltipRect.localPosition = new Vector3(this.tooltipRect.localPosition.x, this.tooltipRect.localPosition.y, 0f);
				this.tooltipContent.transform.localPosition = Vector3.SmoothDamp(this.tooltipContent.transform.localPosition, this.contentPos, ref this.tooltipVelocity, this.tooltipSmoothness, this.dampSpeed * 1000f, Time.unscaledDeltaTime);
				return;
			}
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.tooltipRect.position = this.cursorPos;
				this.tooltipContent.transform.position = Vector3.SmoothDamp(this.tooltipContent.transform.position, this.cursorPos + this.contentPos, ref this.tooltipVelocity, this.tooltipSmoothness, this.dampSpeed * 1000f, Time.unscaledDeltaTime);
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00065BC4 File Offset: 0x00063DC4
		private void CheckForBounds()
		{
			if (this.uiPos.x <= (float)this.xLeft)
			{
				this.contentPos = new Vector3((float)this.hBorderLeft, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(0f, this.contentRect.pivot.y);
			}
			else if (this.uiPos.x >= (float)this.xRight)
			{
				this.contentPos = new Vector3((float)this.hBorderRight, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(1f, this.contentRect.pivot.y);
			}
			if (this.uiPos.y <= (float)this.yTop)
			{
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderBottom, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 0f);
				return;
			}
			if (this.uiPos.y >= (float)this.yBottom)
			{
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderTop, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 1f);
			}
		}

		// Token: 0x04001456 RID: 5206
		public Canvas mainCanvas;

		// Token: 0x04001457 RID: 5207
		public GameObject tooltipObject;

		// Token: 0x04001458 RID: 5208
		public GameObject tooltipContent;

		// Token: 0x04001459 RID: 5209
		public Camera targetCamera;

		// Token: 0x0400145A RID: 5210
		[Range(0.01f, 0.5f)]
		public float tooltipSmoothness = 0.1f;

		// Token: 0x0400145B RID: 5211
		[Range(5f, 10f)]
		public float dampSpeed = 10f;

		// Token: 0x0400145C RID: 5212
		public float preferredWidth = 375f;

		// Token: 0x0400145D RID: 5213
		public bool allowUpdating;

		// Token: 0x0400145E RID: 5214
		public TooltipManager.CameraSource cameraSource;

		// Token: 0x0400145F RID: 5215
		[Range(-50f, 50f)]
		public int vBorderTop = -15;

		// Token: 0x04001460 RID: 5216
		[Range(-50f, 50f)]
		public int vBorderBottom = 10;

		// Token: 0x04001461 RID: 5217
		[Range(-50f, 50f)]
		public int hBorderLeft = 20;

		// Token: 0x04001462 RID: 5218
		[Range(-50f, 50f)]
		public int hBorderRight = -15;

		// Token: 0x04001463 RID: 5219
		[SerializeField]
		private int xLeft = -400;

		// Token: 0x04001464 RID: 5220
		[SerializeField]
		private int xRight = 400;

		// Token: 0x04001465 RID: 5221
		[SerializeField]
		private int yTop = -325;

		// Token: 0x04001466 RID: 5222
		[SerializeField]
		private int yBottom = 325;

		// Token: 0x04001467 RID: 5223
		private Vector2 uiPos;

		// Token: 0x04001468 RID: 5224
		private Vector3 cursorPos;

		// Token: 0x04001469 RID: 5225
		private Vector3 contentPos = new Vector3(0f, 0f, 0f);

		// Token: 0x0400146A RID: 5226
		private Vector3 tooltipVelocity = Vector3.zero;

		// Token: 0x0400146B RID: 5227
		private RectTransform contentRect;

		// Token: 0x0400146C RID: 5228
		private RectTransform tooltipRect;

		// Token: 0x0400146D RID: 5229
		[HideInInspector]
		public LayoutElement contentLE;

		// Token: 0x0200033A RID: 826
		public enum CameraSource
		{
			// Token: 0x0400146F RID: 5231
			Main,
			// Token: 0x04001470 RID: 5232
			Custom
		}
	}
}
