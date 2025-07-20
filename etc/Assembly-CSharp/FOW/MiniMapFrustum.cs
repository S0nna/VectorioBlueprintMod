using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace FOW
{
	// Token: 0x02000367 RID: 871
	public class MiniMapFrustum : MonoBehaviour
	{
		// Token: 0x060016E4 RID: 5860 RVA: 0x0006C9D7 File Offset: 0x0006ABD7
		public RenderTexture GetFrustumRT()
		{
			return this.Frustum_RT;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x0006C9E0 File Offset: 0x0006ABE0
		private void Start()
		{
			this.blitMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			this.blitMaterial.SetInt("_SrcBlend", 4);
			this.blitMaterial.SetInt("_DstBlend", 0);
			this.blitMaterial.SetInt("_Cull", 0);
			this.blitMaterial.SetInt("_ZWrite", 0);
			this.blitMaterial.SetInt("_ZTest", 8);
			this.points = new Vector3[4];
			this.screenPositions = new Vector2[4];
			this.UVs = new Vector2[4];
			this.InitFrustumRT();
			this.fallbackPlane = default(Plane);
			this.fallbackPlane.SetNormalAndPosition(FogOfWarWorld.UpVector, this.MapCollider.transform.position);
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0006CAB0 File Offset: 0x0006ACB0
		private void InitFrustumRT()
		{
			this.Frustum_RT = new RenderTexture(this.ResolutionX, this.ResolutionY, 0);
			this.Frustum_RT.format = RenderTextureFormat.ARGBHalf;
			this.Frustum_RT.antiAliasing = 8;
			this.Frustum_RT.filterMode = FilterMode.Trilinear;
			this.Frustum_RT.anisoLevel = 9;
			this.Frustum_RT.Create();
			RenderTexture.active = this.Frustum_RT;
			Material material = new Material(Shader.Find("Hidden/Internal-Colored"));
			material.SetInt("_SrcBlend", 4);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_Cull", 0);
			material.SetInt("_ZWrite", 0);
			material.SetInt("_ZTest", 8);
			material.SetPass(0);
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
			GL.End();
			if (this.RawImageComponent != null)
			{
				this.RawImageComponent.texture = this.Frustum_RT;
			}
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x0006CBBC File Offset: 0x0006ADBC
		private Vector3 GetWorldSpaceFrustomCorner(Vector2 ScreenPosition)
		{
			Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);
			if (this.MapCollider.Raycast(ray, out this.rayHit, this.RayDistance))
			{
				return this.rayHit.point;
			}
			float distance;
			this.fallbackPlane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0006CC18 File Offset: 0x0006AE18
		private void SetScreenPositions()
		{
			this.screenPositions[0].x = 0f;
			this.screenPositions[0].y = 0f;
			this.screenPositions[1].x = 0f;
			this.screenPositions[1].y = (float)Screen.height;
			this.screenPositions[2].x = (float)Screen.width;
			this.screenPositions[2].y = (float)Screen.height;
			this.screenPositions[3].x = (float)Screen.width;
			this.screenPositions[3].y = 0f;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x0006CCDC File Offset: 0x0006AEDC
		private void Update()
		{
			this.SetScreenPositions();
			this.points[0] = this.GetWorldSpaceFrustomCorner(this.screenPositions[0]);
			this.points[1] = this.GetWorldSpaceFrustomCorner(this.screenPositions[1]);
			this.points[2] = this.GetWorldSpaceFrustomCorner(this.screenPositions[2]);
			this.points[3] = this.GetWorldSpaceFrustomCorner(this.screenPositions[3]);
			this._worldBounds = FogOfWarWorld.instance.GetBoundsVectorForShader();
			this.frustumCenterUV.x = 0f;
			this.frustumCenterUV.y = 0f;
			for (int i = 0; i < 4; i++)
			{
				this.UVs[i] = this.GetUV(this.points[i]);
				this.frustumCenterUV += this.UVs[i];
			}
			this.frustumCenterUV /= 4f;
			this.DrawMiniMapFrustum();
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x0006CDFC File Offset: 0x0006AFFC
		private Vector2 GetUV(Vector3 WorldPosition)
		{
			Vector2 fowPositionFromWorldPosition = FogOfWarWorld.instance.GetFowPositionFromWorldPosition(WorldPosition);
			return new Vector2((fowPositionFromWorldPosition.x - this._worldBounds.y + this._worldBounds.x / 2f) / this._worldBounds.x, (fowPositionFromWorldPosition.y - this._worldBounds.w + this._worldBounds.z / 2f) / this._worldBounds.z);
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x0006CE7C File Offset: 0x0006B07C
		private void DrawMiniMapFrustum()
		{
			MiniMapFrustum.<>c__DisplayClass23_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			GL.PushMatrix();
			this.blitMaterial.SetPass(0);
			GL.LoadOrtho();
			RenderTexture.active = this.Frustum_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
			GL.End();
			GL.Begin(7);
			GL.Color(this.LineColor);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[0];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[1];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[0], this.UVs[1], ref CS$<>8__locals1);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[1];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[2];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[1], this.UVs[2], ref CS$<>8__locals1);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[2];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[3];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[2], this.UVs[3], ref CS$<>8__locals1);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[3];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[0];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[3], this.UVs[0], ref CS$<>8__locals1);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x0006D0C8 File Offset: 0x0006B2C8
		[CompilerGenerated]
		private void <DrawMiniMapFrustum>g__DrawLine|23_0(Vector2 uv1, Vector2 uv2, ref MiniMapFrustum.<>c__DisplayClass23_0 A_3)
		{
			GL.Vertex(new Vector3(uv1.x, uv1.y, 0f));
			GL.Vertex(new Vector3(uv2.x, uv2.y, 0f));
			GL.Vertex(new Vector3(uv2.x + A_3.CrossVector2.x, uv2.y + A_3.CrossVector2.y, 0f));
			GL.Vertex(new Vector3(uv1.x + A_3.CrossVector1.x, uv1.y + A_3.CrossVector1.y, 0f));
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x0006D171 File Offset: 0x0006B371
		[CompilerGenerated]
		private void <DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref MiniMapFrustum.<>c__DisplayClass23_0 A_1)
		{
			A_1.CrossVector1 *= this.LineWidth;
			A_1.CrossVector2 *= this.LineWidth;
		}

		// Token: 0x040015E3 RID: 5603
		public Collider MapCollider;

		// Token: 0x040015E4 RID: 5604
		public float RayDistance = 100f;

		// Token: 0x040015E5 RID: 5605
		public int ResolutionX = 256;

		// Token: 0x040015E6 RID: 5606
		public int ResolutionY = 256;

		// Token: 0x040015E7 RID: 5607
		public RawImage RawImageComponent;

		// Token: 0x040015E8 RID: 5608
		public Color LineColor = Color.white;

		// Token: 0x040015E9 RID: 5609
		public float LineWidth = 0.05f;

		// Token: 0x040015EA RID: 5610
		private Plane fallbackPlane;

		// Token: 0x040015EB RID: 5611
		private Material blitMaterial;

		// Token: 0x040015EC RID: 5612
		private RenderTexture Frustum_RT;

		// Token: 0x040015ED RID: 5613
		private Vector4 _worldBounds;

		// Token: 0x040015EE RID: 5614
		private Vector3[] points;

		// Token: 0x040015EF RID: 5615
		private Vector2[] screenPositions;

		// Token: 0x040015F0 RID: 5616
		private Vector2[] UVs;

		// Token: 0x040015F1 RID: 5617
		private Vector2 frustumCenterUV;

		// Token: 0x040015F2 RID: 5618
		private RaycastHit rayHit;
	}
}
