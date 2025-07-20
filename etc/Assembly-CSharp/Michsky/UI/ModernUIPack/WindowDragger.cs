using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000355 RID: 853
	public class WindowDragger : UIBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06001669 RID: 5737 RVA: 0x00068814 File Offset: 0x00066A14
		public new void Start()
		{
			if (this.dragArea == null)
			{
				try
				{
					Canvas canvas = (Canvas)Object.FindObjectsOfType(typeof(Canvas))[0];
					this.dragArea = canvas.GetComponent<RectTransform>();
				}
				catch
				{
					Debug.LogError("Movable Window - Drag Area has not been assigned.");
				}
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x00068874 File Offset: 0x00066A74
		private RectTransform DragObjectInternal
		{
			get
			{
				if (this.dragObject == null)
				{
					return base.transform as RectTransform;
				}
				return this.dragObject;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x00068898 File Offset: 0x00066A98
		private RectTransform DragAreaInternal
		{
			get
			{
				if (this.dragArea == null)
				{
					RectTransform rectTransform = base.transform as RectTransform;
					while (rectTransform.parent != null && rectTransform.parent is RectTransform)
					{
						rectTransform = (rectTransform.parent as RectTransform);
					}
					return rectTransform;
				}
				return this.dragArea;
			}
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x000688F0 File Offset: 0x00066AF0
		public void OnBeginDrag(PointerEventData data)
		{
			this.originalPanelLocalPosition = this.DragObjectInternal.localPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.DragAreaInternal, data.position, data.pressEventCamera, out this.originalLocalPointerPosition);
			base.gameObject.transform.SetAsLastSibling();
			if (this.topOnClick)
			{
				this.dragObject.transform.SetAsLastSibling();
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00068954 File Offset: 0x00066B54
		public void OnDrag(PointerEventData data)
		{
			Vector2 a;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.DragAreaInternal, data.position, data.pressEventCamera, out a))
			{
				Vector3 b = a - this.originalLocalPointerPosition;
				this.DragObjectInternal.localPosition = this.originalPanelLocalPosition + b;
			}
			this.ClampToArea();
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x000689AC File Offset: 0x00066BAC
		private void ClampToArea()
		{
			Vector3 localPosition = this.DragObjectInternal.localPosition;
			Vector3 vector = this.DragAreaInternal.rect.min - this.DragObjectInternal.rect.min;
			Vector3 vector2 = this.DragAreaInternal.rect.max - this.DragObjectInternal.rect.max;
			localPosition.x = Mathf.Clamp(this.DragObjectInternal.localPosition.x, vector.x, vector2.x);
			localPosition.y = Mathf.Clamp(this.DragObjectInternal.localPosition.y, vector.y, vector2.y);
			this.DragObjectInternal.localPosition = localPosition;
		}

		// Token: 0x0400155F RID: 5471
		[Header("Resources")]
		public RectTransform dragArea;

		// Token: 0x04001560 RID: 5472
		public RectTransform dragObject;

		// Token: 0x04001561 RID: 5473
		[Header("Settings")]
		public bool topOnClick = true;

		// Token: 0x04001562 RID: 5474
		private Vector2 originalLocalPointerPosition;

		// Token: 0x04001563 RID: 5475
		private Vector3 originalPanelLocalPosition;
	}
}
