using System;
using UnityEngine.Rendering.Universal;

namespace FOW
{
	// Token: 0x0200038D RID: 909
	public class FogOfWarRenderFeature : ScriptableRendererFeature
	{
		// Token: 0x0600179F RID: 6047 RVA: 0x00072D7A File Offset: 0x00070F7A
		public override void Create()
		{
			this.fowPass = new FogOfWarPass(base.name);
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00072D8D File Offset: 0x00070F8D
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			this.fowPass.renderPassEvent = this.renderPassEvent;
			this.fowPass.ConfigureInput(ScriptableRenderPassInput.Normal);
			renderer.EnqueuePass(this.fowPass);
		}

		// Token: 0x04001744 RID: 5956
		public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

		// Token: 0x04001745 RID: 5957
		private FogOfWarPass fowPass;
	}
}
