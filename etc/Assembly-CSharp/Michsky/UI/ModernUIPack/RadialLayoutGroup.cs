using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200030C RID: 780
	[AddComponentMenu("Modern UI Pack/Layout Group/Radial Layout Group")]
	public class RadialLayoutGroup : LayoutGroup
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00061B9C File Offset: 0x0005FD9C
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x00061BA4 File Offset: 0x0005FDA4
		public RadialLayoutGroup.Direction layoutDir
		{
			get
			{
				return this.refLayoutDir;
			}
			set
			{
				base.SetProperty<RadialLayoutGroup.Direction>(ref this.refLayoutDir, value);
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00061BB3 File Offset: 0x0005FDB3
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x00061BBB File Offset: 0x0005FDBB
		public float radiusStart
		{
			get
			{
				return this.refRadiusStart;
			}
			set
			{
				base.SetProperty<float>(ref this.refRadiusStart, value);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x00061BCA File Offset: 0x0005FDCA
		// (set) Token: 0x06001551 RID: 5457 RVA: 0x00061BD2 File Offset: 0x0005FDD2
		public float radiusDelta
		{
			get
			{
				return this.refRadiusDelta;
			}
			set
			{
				base.SetProperty<float>(ref this.refRadiusDelta, value);
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x00061BE1 File Offset: 0x0005FDE1
		// (set) Token: 0x06001553 RID: 5459 RVA: 0x00061BE9 File Offset: 0x0005FDE9
		public float radiusRange
		{
			get
			{
				return this.refRadiusRange;
			}
			set
			{
				base.SetProperty<float>(ref this.refRadiusRange, value);
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x00061BF8 File Offset: 0x0005FDF8
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x00061C00 File Offset: 0x0005FE00
		public float angleDelta
		{
			get
			{
				return this.refAngleDelta;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleDelta, value);
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x00061C0F File Offset: 0x0005FE0F
		// (set) Token: 0x06001557 RID: 5463 RVA: 0x00061C17 File Offset: 0x0005FE17
		public float angleStart
		{
			get
			{
				return this.refAngleStart;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleStart, value);
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x00061C26 File Offset: 0x0005FE26
		// (set) Token: 0x06001559 RID: 5465 RVA: 0x00061C2E File Offset: 0x0005FE2E
		public float angleCenter
		{
			get
			{
				return this.refAngleCenter;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleCenter, value);
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x00061C3D File Offset: 0x0005FE3D
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x00061C45 File Offset: 0x0005FE45
		public float angleRange
		{
			get
			{
				return this.refAngleRange;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleRange, value);
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x00061C54 File Offset: 0x0005FE54
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x00061C5C File Offset: 0x0005FE5C
		public bool childRotate
		{
			get
			{
				return this.refChildRotate;
			}
			set
			{
				base.SetProperty<bool>(ref this.refChildRotate, value);
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00003212 File Offset: 0x00001412
		public override void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00003212 File Offset: 0x00001412
		public override void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x00061C6B File Offset: 0x0005FE6B
		public override void SetLayoutHorizontal()
		{
			this.CalculateChildrenPositions();
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00061C6B File Offset: 0x0005FE6B
		public override void SetLayoutVertical()
		{
			this.CalculateChildrenPositions();
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00061C74 File Offset: 0x0005FE74
		private void CalculateChildrenPositions()
		{
			this.m_Tracker.Clear();
			this.childList.Clear();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = base.transform.GetChild(i) as RectTransform;
				if (rectTransform.gameObject.activeSelf)
				{
					this.ignoreList.Clear();
					rectTransform.GetComponents<ILayoutIgnorer>(this.ignoreList);
					if (this.ignoreList.Count == 0)
					{
						this.childList.Add(rectTransform);
					}
					else
					{
						for (int j = 0; j < this.ignoreList.Count; j++)
						{
							if (!this.ignoreList[j].ignoreLayout)
							{
								this.childList.Add(rectTransform);
								break;
							}
						}
						this.ignoreList.Clear();
					}
				}
			}
			this.EnsureParameters(this.childList.Count);
			for (int k = 0; k < this.childList.Count; k++)
			{
				RectTransform child = this.childList[k];
				float num = (float)k * this.angleDelta;
				float angle = (this.layoutDir == RadialLayoutGroup.Direction.Clockwise) ? (this.angleStart - num) : (this.angleStart + num);
				this.ProcessOneChild(child, angle, this.radiusStart + (float)k * this.radiusDelta);
			}
			this.childList.Clear();
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00061DC9 File Offset: 0x0005FFC9
		private void EnsureParameters(int childCount)
		{
			this.EnsureAngleParameters(childCount);
			this.EnsureRadiusParameters(childCount);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00061DDC File Offset: 0x0005FFDC
		private void EnsureAngleParameters(int childCount)
		{
			int num = childCount - 1;
			switch (this.layoutDir)
			{
			case RadialLayoutGroup.Direction.Clockwise:
				if (num > 0)
				{
					this.angleDelta = this.angleRange / (float)num;
					return;
				}
				this.angleDelta = 0f;
				return;
			case RadialLayoutGroup.Direction.Counterclockwise:
				if (num > 0)
				{
					this.angleDelta = this.angleRange / (float)num;
					return;
				}
				this.angleDelta = 0f;
				return;
			case RadialLayoutGroup.Direction.Bidirectional:
				if (num > 0)
				{
					this.angleDelta = this.angleRange / (float)num;
				}
				else
				{
					this.angleDelta = 0f;
				}
				this.angleStart = this.angleCenter - this.angleRange * 0.5f;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00061E80 File Offset: 0x00060080
		private void EnsureRadiusParameters(int childCount)
		{
			int num = childCount - 1;
			RadialLayoutGroup.Direction layoutDir = this.layoutDir;
			if (layoutDir != RadialLayoutGroup.Direction.Clockwise)
			{
				if (layoutDir - RadialLayoutGroup.Direction.Counterclockwise > 1)
				{
					return;
				}
				if (num > 0)
				{
					this.radiusDelta = this.radiusRange / (float)num;
					return;
				}
				this.radiusDelta = 0f;
				return;
			}
			else
			{
				if (num > 0)
				{
					this.radiusDelta = this.radiusRange / (float)num;
					return;
				}
				this.radiusDelta = 0f;
				return;
			}
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00061EE4 File Offset: 0x000600E4
		private void ProcessOneChild(RectTransform child, float angle, float radius)
		{
			Vector3 a = new Vector3(Mathf.Cos(angle * 0.017453292f), Mathf.Sin(angle * 0.017453292f), 0f);
			child.localPosition = a * radius;
			DrivenTransformProperties drivenProperties = DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.Rotation | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY;
			this.m_Tracker.Add(this, child, drivenProperties);
			child.anchorMin = RadialLayoutGroup.center;
			child.anchorMax = RadialLayoutGroup.center;
			child.pivot = RadialLayoutGroup.center;
			if (this.childRotate)
			{
				child.localEulerAngles = new Vector3(0f, 0f, angle);
				return;
			}
			child.localEulerAngles = Vector3.zero;
		}

		// Token: 0x04001350 RID: 4944
		[SerializeField]
		private RadialLayoutGroup.Direction refLayoutDir;

		// Token: 0x04001351 RID: 4945
		[SerializeField]
		private float refRadiusStart = 200f;

		// Token: 0x04001352 RID: 4946
		[SerializeField]
		private float refRadiusDelta;

		// Token: 0x04001353 RID: 4947
		[SerializeField]
		private float refRadiusRange;

		// Token: 0x04001354 RID: 4948
		[SerializeField]
		private float refAngleDelta;

		// Token: 0x04001355 RID: 4949
		[SerializeField]
		private float refAngleStart;

		// Token: 0x04001356 RID: 4950
		[SerializeField]
		private float refAngleCenter;

		// Token: 0x04001357 RID: 4951
		[SerializeField]
		private float refAngleRange = 200f;

		// Token: 0x04001358 RID: 4952
		[SerializeField]
		private bool refChildRotate;

		// Token: 0x04001359 RID: 4953
		private List<RectTransform> childList = new List<RectTransform>();

		// Token: 0x0400135A RID: 4954
		private List<ILayoutIgnorer> ignoreList = new List<ILayoutIgnorer>();

		// Token: 0x0400135B RID: 4955
		private static readonly Vector2 center = new Vector2(0.5f, 0.5f);

		// Token: 0x0200030D RID: 781
		public enum Direction
		{
			// Token: 0x0400135D RID: 4957
			Clockwise,
			// Token: 0x0400135E RID: 4958
			Counterclockwise,
			// Token: 0x0400135F RID: 4959
			Bidirectional
		}

		// Token: 0x0200030E RID: 782
		public enum ConstraintMode
		{
			// Token: 0x04001361 RID: 4961
			Interval,
			// Token: 0x04001362 RID: 4962
			Range
		}
	}
}
