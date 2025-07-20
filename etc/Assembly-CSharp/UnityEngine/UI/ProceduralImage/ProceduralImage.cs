using System;
using UnityEngine.Events;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x020002B2 RID: 690
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Procedural Image")]
	public class ProceduralImage : Image
	{
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x000584FF File Offset: 0x000566FF
		// (set) Token: 0x0600133E RID: 4926 RVA: 0x00058527 File Offset: 0x00056727
		private static Material DefaultProceduralImageMaterial
		{
			get
			{
				if (ProceduralImage.materialInstance == null)
				{
					ProceduralImage.materialInstance = new Material(Shader.Find("UI/Procedural UI Image"));
				}
				return ProceduralImage.materialInstance;
			}
			set
			{
				ProceduralImage.materialInstance = value;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x0005852F File Offset: 0x0005672F
		// (set) Token: 0x06001340 RID: 4928 RVA: 0x00058537 File Offset: 0x00056737
		public float BorderWidth
		{
			get
			{
				return this.borderWidth;
			}
			set
			{
				this.borderWidth = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x00058546 File Offset: 0x00056746
		// (set) Token: 0x06001342 RID: 4930 RVA: 0x0005854E File Offset: 0x0005674E
		public float FalloffDistance
		{
			get
			{
				return this.falloffDistance;
			}
			set
			{
				this.falloffDistance = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001343 RID: 4931 RVA: 0x0005855D File Offset: 0x0005675D
		// (set) Token: 0x06001344 RID: 4932 RVA: 0x0005859D File Offset: 0x0005679D
		protected ProceduralImageModifier Modifier
		{
			get
			{
				if (this.modifier == null)
				{
					this.modifier = base.GetComponent<ProceduralImageModifier>();
					if (this.modifier == null)
					{
						this.ModifierType = typeof(FreeModifier);
					}
				}
				return this.modifier;
			}
			set
			{
				this.modifier = value;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x000585A6 File Offset: 0x000567A6
		// (set) Token: 0x06001346 RID: 4934 RVA: 0x000585B4 File Offset: 0x000567B4
		public System.Type ModifierType
		{
			get
			{
				return this.Modifier.GetType();
			}
			set
			{
				if (this.modifier != null && this.modifier.GetType() != value)
				{
					if (base.GetComponent<ProceduralImageModifier>() != null)
					{
						Object.DestroyImmediate(base.GetComponent<ProceduralImageModifier>());
					}
					base.gameObject.AddComponent(value);
					this.Modifier = base.GetComponent<ProceduralImageModifier>();
					this.SetAllDirty();
					return;
				}
				if (this.modifier == null)
				{
					base.gameObject.AddComponent(value);
					this.Modifier = base.GetComponent<ProceduralImageModifier>();
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00058648 File Offset: 0x00056848
		protected override void OnEnable()
		{
			base.OnEnable();
			this.Init();
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00058656 File Offset: 0x00056856
		protected override void OnDisable()
		{
			base.OnDisable();
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyVertsCallback, new UnityAction(this.OnVerticesDirty));
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00058680 File Offset: 0x00056880
		private void Init()
		{
			this.FixTexCoordsInCanvas();
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyVertsCallback, new UnityAction(this.OnVerticesDirty));
			base.preserveAspect = false;
			this.material = null;
			if (base.sprite == null)
			{
				base.sprite = EmptySprite.Get();
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000586DC File Offset: 0x000568DC
		protected void OnVerticesDirty()
		{
			if (base.sprite == null)
			{
				base.sprite = EmptySprite.Get();
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x000586F8 File Offset: 0x000568F8
		protected void FixTexCoordsInCanvas()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent != null)
			{
				this.FixTexCoordsInCanvas(componentInParent);
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0005871C File Offset: 0x0005691C
		protected void FixTexCoordsInCanvas(Canvas c)
		{
			c.additionalShaderChannels |= (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3);
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0005872C File Offset: 0x0005692C
		private Vector4 FixRadius(Vector4 vec)
		{
			Rect rect = base.rectTransform.rect;
			vec = new Vector4(Mathf.Max(vec.x, 0f), Mathf.Max(vec.y, 0f), Mathf.Max(vec.z, 0f), Mathf.Max(vec.w, 0f));
			float d = Mathf.Min(Mathf.Min(Mathf.Min(Mathf.Min(rect.width / (vec.x + vec.y), rect.width / (vec.z + vec.w)), rect.height / (vec.x + vec.w)), rect.height / (vec.z + vec.y)), 1f);
			return vec * d;
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00058801 File Offset: 0x00056A01
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			base.OnPopulateMesh(toFill);
			this.EncodeAllInfoIntoVertices(toFill, this.CalculateInfo());
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00058817 File Offset: 0x00056A17
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.FixTexCoordsInCanvas();
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00058828 File Offset: 0x00056A28
		private ProceduralImageInfo CalculateInfo()
		{
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			float pixelSize = 1f / Mathf.Max(0f, this.falloffDistance);
			Vector4 a = this.FixRadius(this.Modifier.CalculateRadius(pixelAdjustedRect));
			float num = Mathf.Min(pixelAdjustedRect.width, pixelAdjustedRect.height);
			return new ProceduralImageInfo(pixelAdjustedRect.width + this.falloffDistance, pixelAdjustedRect.height + this.falloffDistance, this.falloffDistance, pixelSize, a / num, this.borderWidth / num * 2f);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x000588B8 File Offset: 0x00056AB8
		private void EncodeAllInfoIntoVertices(VertexHelper vh, ProceduralImageInfo info)
		{
			UIVertex uivertex = default(UIVertex);
			Vector2 v = new Vector2(info.width, info.height);
			Vector2 v2 = new Vector2(this.EncodeFloats_0_1_16_16(info.radius.x, info.radius.y), this.EncodeFloats_0_1_16_16(info.radius.z, info.radius.w));
			Vector2 v3 = new Vector2((info.borderWidth == 0f) ? 1f : Mathf.Clamp01(info.borderWidth), info.pixelSize);
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				vh.PopulateUIVertex(ref uivertex, i);
				uivertex.position += (uivertex.uv0 - new Vector3(0.5f, 0.5f)) * info.fallOffDistance;
				uivertex.uv1 = v;
				uivertex.uv2 = v2;
				uivertex.uv3 = v3;
				vh.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x000589E4 File Offset: 0x00056BE4
		private float EncodeFloats_0_1_16_16(float a, float b)
		{
			Vector2 rhs = new Vector2(1f, 1.5259022E-05f);
			return Vector2.Dot(new Vector2(Mathf.Floor(a * 65534f) / 65535f, Mathf.Floor(b * 65534f) / 65535f), rhs);
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x00058A31 File Offset: 0x00056C31
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x00058A4D File Offset: 0x00056C4D
		public override Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					return ProceduralImage.DefaultProceduralImageMaterial;
				}
				return base.material;
			}
			set
			{
				base.material = value;
			}
		}

		// Token: 0x040010DE RID: 4318
		[SerializeField]
		private float borderWidth;

		// Token: 0x040010DF RID: 4319
		private ProceduralImageModifier modifier;

		// Token: 0x040010E0 RID: 4320
		private static Material materialInstance;

		// Token: 0x040010E1 RID: 4321
		[SerializeField]
		private float falloffDistance = 1f;
	}
}
