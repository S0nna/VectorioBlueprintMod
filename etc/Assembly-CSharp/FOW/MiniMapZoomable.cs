using System;
using UnityEngine;
using UnityEngine.UI;

namespace FOW
{
	// Token: 0x02000369 RID: 873
	public class MiniMapZoomable : MonoBehaviour
	{
		// Token: 0x060016EF RID: 5871 RVA: 0x0006D1A1 File Offset: 0x0006B3A1
		public RenderTexture GetMiniMapRT()
		{
			return this.Minimap_RT;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x0006D1AC File Offset: 0x0006B3AC
		private void Start()
		{
			this.blitMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			this.blitMaterial.SetInt("_SrcBlend", 4);
			this.blitMaterial.SetInt("_DstBlend", 0);
			this.blitMaterial.SetInt("_Cull", 0);
			this.blitMaterial.SetInt("_ZWrite", 0);
			this.blitMaterial.SetInt("_ZTest", 8);
			this.InitMinimapRT();
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x0006D22C File Offset: 0x0006B42C
		private void InitMinimapRT()
		{
			this.Minimap_RT = new RenderTexture(this.ResolutionX, this.ResolutionY, 0);
			this.Minimap_RT.format = RenderTextureFormat.ARGBHalf;
			this.Minimap_RT.antiAliasing = 8;
			this.Minimap_RT.filterMode = FilterMode.Trilinear;
			this.Minimap_RT.anisoLevel = 9;
			this.Minimap_RT.Create();
			RenderTexture.active = this.Minimap_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
			GL.End();
			if (this.RawImageComponent != null)
			{
				this.RawImageComponent.texture = this.Minimap_RT;
			}
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x0006D2E3 File Offset: 0x0006B4E3
		private void Update()
		{
			this.DrawMiniMapFrustum();
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00003212 File Offset: 0x00001412
		private void DrawMiniMapFrustum()
		{
		}

		// Token: 0x040015F6 RID: 5622
		public int ResolutionX = 256;

		// Token: 0x040015F7 RID: 5623
		public int ResolutionY = 256;

		// Token: 0x040015F8 RID: 5624
		public RawImage RawImageComponent;

		// Token: 0x040015F9 RID: 5625
		private Material blitMaterial;

		// Token: 0x040015FA RID: 5626
		private RenderTexture Minimap_RT;

		// Token: 0x040015FB RID: 5627
		private Vector4 _worldBounds;
	}
}
