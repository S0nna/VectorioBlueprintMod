using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FOW
{
	// Token: 0x0200038C RID: 908
	public class FogOfWarPass : ScriptableRenderPass
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x00072B3D File Offset: 0x00070D3D
		// (set) Token: 0x0600179A RID: 6042 RVA: 0x00072B45 File Offset: 0x00070D45
		public FilterMode filterMode { get; set; }

		// Token: 0x0600179B RID: 6043 RVA: 0x00072B4E File Offset: 0x00070D4E
		public FogOfWarPass(string tag)
		{
			this.m_ProfilerTag = tag;
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00072B70 File Offset: 0x00070D70
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			this.sourceId = -1;
			this.source = renderer.cameraColorTargetHandle;
			this.destinationId = this.temporaryRTId;
			cmd.GetTemporaryRT(this.destinationId, cameraTargetDescriptor, this.filterMode);
			this.destination = new RenderTargetIdentifier(this.destinationId);
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00072BE0 File Offset: 0x00070DE0
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (FogOfWarWorld.instance == null || !FogOfWarWorld.instance.enabled)
			{
				return;
			}
			if (renderingData.cameraData.camera.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay)
			{
				return;
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get(this.m_ProfilerTag);
			renderingData.cameraData.camera.depthTextureMode = DepthTextureMode.DepthNormals;
			if (!FogOfWarWorld.instance.is2D)
			{
				Matrix4x4 cameraToWorldMatrix = renderingData.cameraData.camera.cameraToWorldMatrix;
				FogOfWarWorld.instance.FogOfWarMaterial.SetMatrix("_camToWorldMatrix", cameraToWorldMatrix);
			}
			else
			{
				FogOfWarWorld.instance.FogOfWarMaterial.SetFloat("_cameraSize", renderingData.cameraData.camera.orthographicSize);
				FogOfWarWorld.instance.FogOfWarMaterial.SetVector("_cameraPosition", renderingData.cameraData.camera.transform.position);
				FogOfWarWorld.instance.FogOfWarMaterial.SetFloat("_cameraRotation", Mathf.DeltaAngle(0f, renderingData.cameraData.camera.transform.eulerAngles.z));
			}
			commandBuffer.Blit(this.source, this.destination, FogOfWarWorld.instance.FogOfWarMaterial, 0);
			commandBuffer.Blit(this.destination, this.source);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00072D3B File Offset: 0x00070F3B
		public override void FrameCleanup(CommandBuffer cmd)
		{
			if (this.destinationId != -1)
			{
				cmd.ReleaseTemporaryRT(this.destinationId);
			}
			if (this.source == this.destination && this.sourceId != -1)
			{
				cmd.ReleaseTemporaryRT(this.sourceId);
			}
		}

		// Token: 0x0400173E RID: 5950
		private RenderTargetIdentifier source;

		// Token: 0x0400173F RID: 5951
		private RenderTargetIdentifier destination;

		// Token: 0x04001740 RID: 5952
		private int temporaryRTId = Shader.PropertyToID("_FowTempRT");

		// Token: 0x04001741 RID: 5953
		private int sourceId;

		// Token: 0x04001742 RID: 5954
		private int destinationId;

		// Token: 0x04001743 RID: 5955
		private string m_ProfilerTag;
	}
}
