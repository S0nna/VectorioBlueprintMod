using System;
using System.Linq;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000375 RID: 885
	public class GenerateInvertedCollider : MonoBehaviour
	{
		// Token: 0x06001714 RID: 5908 RVA: 0x0006E97D File Offset: 0x0006CB7D
		public Mesh GetFlippedMesh(Mesh mesh)
		{
			return new Mesh
			{
				vertices = mesh.vertices,
				triangles = mesh.triangles,
				triangles = mesh.triangles.Reverse<int>().ToArray<int>()
			};
		}

		// Token: 0x04001687 RID: 5767
		public bool IncludeChildren = true;

		// Token: 0x04001688 RID: 5768
		public bool DisableOldColliders;

		// Token: 0x04001689 RID: 5769
		public LayerMask LayersToFlip;
	}
}
