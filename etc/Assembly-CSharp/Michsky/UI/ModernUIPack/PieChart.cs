using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x020002D1 RID: 721
	public class PieChart : MaskableGraphic
	{
		// Token: 0x06001430 RID: 5168 RVA: 0x0005C335 File Offset: 0x0005A535
		protected override void Awake()
		{
			base.Awake();
			this.UpdateIndicators();
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0005C344 File Offset: 0x0005A544
		private void Update()
		{
			this.borderThickness = Mathf.Clamp(this.borderThickness, -75f, base.rectTransform.rect.width / 3.333f);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0005C384 File Offset: 0x0005A584
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this.chartData.Count == 0)
			{
				return;
			}
			float num = -base.rectTransform.pivot.x * base.rectTransform.rect.width;
			float num2 = -base.rectTransform.pivot.x * base.rectTransform.rect.width + this.borderThickness;
			float d = -base.rectTransform.pivot.x * base.rectTransform.rect.width * 0.6f;
			float d2 = -base.rectTransform.pivot.x * base.rectTransform.rect.width * 0.6f + this.borderThickness;
			vh.Clear();
			Vector2 vector = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			Vector2 vector3 = new Vector2(0f, 0f);
			Vector2 vector4 = new Vector2(0f, 1f);
			Vector2 vector5 = new Vector2(1f, 1f);
			Vector2 vector6 = new Vector2(1f, 0f);
			float num3 = this.fillAmount;
			float num4 = 360f / (float)this.segments;
			int num5 = (int)((float)(this.segments + 1) * num3);
			int num6 = 0;
			float total = 0f;
			float num7 = this.chartData[0].value;
			this.chartData.ForEach(delegate(PieChart.PieChartDataNode s)
			{
				total += s.value;
			});
			Color32 color = this.chartData[0].color;
			for (int i = 0; i < num5; i++)
			{
				float f = 0.017453292f * ((float)i * num4);
				float num8 = Mathf.Cos(f);
				float num9 = Mathf.Sin(f);
				vector3 = new Vector2(0f, 1f);
				vector4 = new Vector2(1f, 1f);
				vector5 = new Vector2(1f, 0f);
				vector6 = new Vector2(0f, 0f);
				Vector2 vector7 = vector;
				Vector2 vector8 = new Vector2(num * num8, num * num9);
				Vector2 vector9 = new Vector2(num2 * num8, num2 * num9);
				Vector2 vector10 = vector2;
				if ((float)i > num7 / total * (float)this.segments && num6 < this.chartData.Count - 1)
				{
					num6++;
					num7 += this.chartData[num6].value;
					color = this.chartData[num6].color;
				}
				vh.AddUIVertexQuad(this.SetVbo(new Vector2[]
				{
					vector7,
					vector8,
					vector9 * d2 / num2,
					vector10 * d2 / num2
				}, new Vector2[]
				{
					vector3,
					vector4,
					vector5,
					vector6
				}, color));
				if (this.enableBorderColor)
				{
					vh.AddUIVertexQuad(this.SetVbo(new Vector2[]
					{
						vector7,
						vector8,
						vector9,
						vector10
					}, new Vector2[]
					{
						vector3,
						vector4,
						vector5,
						vector6
					}, this.borderColor));
					vh.AddUIVertexQuad(this.SetVbo(new Vector2[]
					{
						vector7 * d / num,
						vector8 * d / num,
						vector9 * d2 / num2,
						vector10 * d2 / num2
					}, new Vector2[]
					{
						vector3,
						vector4,
						vector5,
						vector6
					}, this.borderColor));
				}
				vector = vector8;
				vector2 = vector9;
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0005C7B0 File Offset: 0x0005A9B0
		public void SetData(List<PieChart.PieChartDataNode> data)
		{
			this.chartData = data;
			this.SetVerticesDirty();
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0005C7C0 File Offset: 0x0005A9C0
		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs, Color32 color)
		{
			UIVertex[] array = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				UIVertex simpleVert = UIVertex.simpleVert;
				simpleVert.color = color;
				simpleVert.position = vertices[i];
				simpleVert.uv0 = uvs[i];
				array[i] = simpleVert;
			}
			return array;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0005C820 File Offset: 0x0005AA20
		public void UpdateIndicators()
		{
			for (int i = 0; i < this.chartData.Count; i++)
			{
				if (this.chartData[i].indicatorImage != null)
				{
					this.chartData[i].indicatorImage.color = this.chartData[i].color;
				}
				if (this.chartData[i].indicatorText != null && this.addValueToIndicator)
				{
					this.chartData[i].indicatorText.text = this.chartData[i].name + this.valuePrefix + this.chartData[i].value.ToString() + this.valueSuffix;
				}
				else if (this.chartData[i].indicatorText != null && !this.addValueToIndicator)
				{
					this.chartData[i].indicatorText.text = this.chartData[i].name;
				}
			}
			if (this.indicatorParent != null)
			{
				base.StartCoroutine("UpdateIndicatorLayout");
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0005C961 File Offset: 0x0005AB61
		public void ChangeValue(int itemIndex, float itemValue)
		{
			this.chartData[itemIndex].value = itemValue;
			base.enabled = false;
			base.enabled = true;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0005C984 File Offset: 0x0005AB84
		public void AddNewItem()
		{
			PieChart.PieChartDataNode pieChartDataNode = new PieChart.PieChartDataNode();
			if (this.indicatorParent.childCount != 0)
			{
				int index = this.indicatorParent.childCount - 1;
				GameObject gameObject = Object.Instantiate<GameObject>(this.indicatorParent.GetChild(index).gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.indicatorParent, false);
				gameObject.gameObject.name = "Item " + index.ToString() + " Indicator";
				pieChartDataNode.indicatorImage = gameObject.GetComponentInChildren<Image>();
				pieChartDataNode.indicatorText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
				pieChartDataNode.name = "Chart Item " + index.ToString();
			}
			this.chartData.Add(pieChartDataNode);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0005CA53 File Offset: 0x0005AC53
		private IEnumerator UpdateIndicatorLayout()
		{
			yield return new WaitForSeconds(0.1f);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.indicatorParent.GetComponentInParent<RectTransform>());
			yield break;
		}

		// Token: 0x040011EE RID: 4590
		[SerializeField]
		public List<PieChart.PieChartDataNode> chartData = new List<PieChart.PieChartDataNode>();

		// Token: 0x040011EF RID: 4591
		[Range(-75f, 150f)]
		public float borderThickness = 5f;

		// Token: 0x040011F0 RID: 4592
		[SerializeField]
		private Color borderColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x040011F1 RID: 4593
		public Transform indicatorParent;

		// Token: 0x040011F2 RID: 4594
		public string valuePrefix = "(";

		// Token: 0x040011F3 RID: 4595
		public string valueSuffix = ")";

		// Token: 0x040011F4 RID: 4596
		public bool addValueToIndicator = true;

		// Token: 0x040011F5 RID: 4597
		public bool enableBorderColor;

		// Token: 0x040011F6 RID: 4598
		private float fillAmount = 1f;

		// Token: 0x040011F7 RID: 4599
		private int segments = 720;

		// Token: 0x020002D2 RID: 722
		[Serializable]
		public class PieChartDataNode
		{
			// Token: 0x040011F8 RID: 4600
			public string name = "Chart Item";

			// Token: 0x040011F9 RID: 4601
			public float value = 10f;

			// Token: 0x040011FA RID: 4602
			public Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

			// Token: 0x040011FB RID: 4603
			public Image indicatorImage;

			// Token: 0x040011FC RID: 4604
			public TextMeshProUGUI indicatorText;
		}
	}
}
