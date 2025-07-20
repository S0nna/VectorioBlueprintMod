using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class PathBezier2d : MonoBehaviour
{
	// Token: 0x0600006F RID: 111 RVA: 0x000061AC File Offset: 0x000043AC
	private void Start()
	{
		Vector3[] array = new Vector3[]
		{
			this.cubes[0].position,
			this.cubes[1].position,
			this.cubes[2].position,
			this.cubes[3].position
		};
		this.visualizePath = new LTBezierPath(array);
		LeanTween.move(this.dude1, array, 10f).setOrientToPath2d(true);
		LeanTween.moveLocal(this.dude2, array, 10f).setOrientToPath2d(true);
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0000624C File Offset: 0x0000444C
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (this.visualizePath != null)
		{
			this.visualizePath.gizmoDraw(-1f);
		}
	}

	// Token: 0x04000071 RID: 113
	public Transform[] cubes;

	// Token: 0x04000072 RID: 114
	public GameObject dude1;

	// Token: 0x04000073 RID: 115
	public GameObject dude2;

	// Token: 0x04000074 RID: 116
	private LTBezierPath visualizePath;
}
