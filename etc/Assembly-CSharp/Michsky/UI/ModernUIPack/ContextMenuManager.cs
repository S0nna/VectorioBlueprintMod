using System;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002DE RID: 734
	[RequireComponent(typeof(Animator))]
	public class ContextMenuManager : MonoBehaviour
	{
		// Token: 0x06001462 RID: 5218 RVA: 0x0005D66C File Offset: 0x0005B86C
		private void Awake()
		{
			if (this.mainCanvas == null)
			{
				this.mainCanvas = base.gameObject.GetComponentInParent<Canvas>();
			}
			if (this.contextAnimator == null)
			{
				this.contextAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.cameraSource == ContextMenuManager.CameraSource.Main)
			{
				this.targetCamera = Camera.main;
			}
			this.contextRect = base.gameObject.GetComponent<RectTransform>();
			this.contentRect = this.contextContent.GetComponent<RectTransform>();
			this.contentPos = new Vector3((float)this.vBorderTop, (float)this.hBorderLeft, 0f);
			base.gameObject.transform.SetAsLastSibling();
			this.subMenuBehaviour = ContextMenuManager.SubMenuBehaviour.Click;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0005D724 File Offset: 0x0005B924
		public void CheckForBounds()
		{
			if (this.uiPos.x <= -100f)
			{
				this.contentPos = new Vector3((float)this.hBorderLeft, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(0f, this.contentRect.pivot.y);
				this.bottomLeft = true;
			}
			else
			{
				this.bottomLeft = false;
			}
			if (this.uiPos.x >= 100f)
			{
				this.contentPos = new Vector3((float)this.hBorderRight, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(1f, this.contentRect.pivot.y);
				this.bottomRight = true;
			}
			else
			{
				this.bottomRight = false;
			}
			if (this.uiPos.y <= -75f)
			{
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderBottom, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 0f);
				this.topLeft = true;
			}
			else
			{
				this.topLeft = false;
			}
			if (this.uiPos.y >= 75f)
			{
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderTop, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 1f);
				this.topRight = true;
				return;
			}
			this.topRight = false;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0005D8D4 File Offset: 0x0005BAD4
		public void SetContextMenuPosition()
		{
			this.cursorPos = Input.mousePosition;
			this.uiPos = this.contextRect.anchoredPosition;
			this.CheckForBounds();
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || this.mainCanvas.renderMode == RenderMode.WorldSpace)
			{
				this.contextRect.position = this.targetCamera.ScreenToWorldPoint(this.cursorPos);
				this.contextRect.localPosition = new Vector3(this.contextRect.localPosition.x, this.contextRect.localPosition.y, 0f);
				this.contextContent.transform.localPosition = Vector3.SmoothDamp(this.contextContent.transform.localPosition, this.contentPos, ref this.contextVelocity, 0f);
				return;
			}
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.contextRect.position = this.cursorPos;
				this.contextContent.transform.position = new Vector3(this.cursorPos.x + this.contentPos.x, this.cursorPos.y + this.contentPos.y, 0f);
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0005DA10 File Offset: 0x0005BC10
		public void Open()
		{
			this.contextAnimator.Play("Menu Out");
			this.isOn = false;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0005DA10 File Offset: 0x0005BC10
		public void Close()
		{
			this.contextAnimator.Play("Menu Out");
			this.isOn = false;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0005DA29 File Offset: 0x0005BC29
		public void OpenContextMenu()
		{
			this.contextAnimator.Play("Menu In");
			this.isOn = true;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0005DA10 File Offset: 0x0005BC10
		public void CloseOnClick()
		{
			this.contextAnimator.Play("Menu Out");
			this.isOn = false;
		}

		// Token: 0x04001231 RID: 4657
		public Canvas mainCanvas;

		// Token: 0x04001232 RID: 4658
		public Camera targetCamera;

		// Token: 0x04001233 RID: 4659
		public GameObject contextContent;

		// Token: 0x04001234 RID: 4660
		public Animator contextAnimator;

		// Token: 0x04001235 RID: 4661
		public GameObject contextButton;

		// Token: 0x04001236 RID: 4662
		public GameObject contextSeparator;

		// Token: 0x04001237 RID: 4663
		public GameObject contextSubMenu;

		// Token: 0x04001238 RID: 4664
		public bool autoSubMenuPosition = true;

		// Token: 0x04001239 RID: 4665
		public ContextMenuManager.SubMenuBehaviour subMenuBehaviour;

		// Token: 0x0400123A RID: 4666
		public ContextMenuManager.CameraSource cameraSource;

		// Token: 0x0400123B RID: 4667
		[Range(-50f, 50f)]
		public int vBorderTop = -10;

		// Token: 0x0400123C RID: 4668
		[Range(-50f, 50f)]
		public int vBorderBottom = 10;

		// Token: 0x0400123D RID: 4669
		[Range(-50f, 50f)]
		public int hBorderLeft = 15;

		// Token: 0x0400123E RID: 4670
		[Range(-50f, 50f)]
		public int hBorderRight = -15;

		// Token: 0x0400123F RID: 4671
		private Vector2 uiPos;

		// Token: 0x04001240 RID: 4672
		private Vector3 cursorPos;

		// Token: 0x04001241 RID: 4673
		private Vector3 contentPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04001242 RID: 4674
		private Vector3 contextVelocity = Vector3.zero;

		// Token: 0x04001243 RID: 4675
		private RectTransform contextRect;

		// Token: 0x04001244 RID: 4676
		private RectTransform contentRect;

		// Token: 0x04001245 RID: 4677
		[HideInInspector]
		public bool isOn;

		// Token: 0x04001246 RID: 4678
		[HideInInspector]
		public bool bottomLeft;

		// Token: 0x04001247 RID: 4679
		[HideInInspector]
		public bool bottomRight;

		// Token: 0x04001248 RID: 4680
		[HideInInspector]
		public bool topLeft;

		// Token: 0x04001249 RID: 4681
		[HideInInspector]
		public bool topRight;

		// Token: 0x020002DF RID: 735
		public enum CameraSource
		{
			// Token: 0x0400124B RID: 4683
			Main,
			// Token: 0x0400124C RID: 4684
			Custom
		}

		// Token: 0x020002E0 RID: 736
		public enum SubMenuBehaviour
		{
			// Token: 0x0400124E RID: 4686
			Hover,
			// Token: 0x0400124F RID: 4687
			Click
		}
	}
}
