using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200037C RID: 892
	public class PartialHider : MonoBehaviour
	{
		// Token: 0x06001735 RID: 5941 RVA: 0x0006EE1D File Offset: 0x0006D01D
		private void OnEnable()
		{
			FogOfWarWorld.PartialHiders.Add(this);
			if (!this.initialized)
			{
				this.InitializeMaterial();
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0006EE38 File Offset: 0x0006D038
		private void OnDisable()
		{
			FogOfWarWorld.PartialHiders.Remove(this);
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0006EE46 File Offset: 0x0006D046
		private void InitializeMaterial()
		{
			this.initialized = true;
			FogOfWarWorld.instance.InitializeFogProperties(this.HiderMaterial);
			FogOfWarWorld.instance.UpdateMaterialProperties(this.HiderMaterial);
			FogOfWarWorld.instance.SetNumRevealers(this.HiderMaterial);
		}

		// Token: 0x04001696 RID: 5782
		public Material HiderMaterial;

		// Token: 0x04001697 RID: 5783
		private bool initialized;
	}
}
