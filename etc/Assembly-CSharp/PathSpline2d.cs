using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class PathSpline2d : MonoBehaviour
{
	// Token: 0x06000076 RID: 118 RVA: 0x00006440 File Offset: 0x00004640
	private void Start()
	{
		Vector3[] array = new Vector3[]
		{
			this.cubes[0].position,
			this.cubes[1].position,
			this.cubes[2].position,
			this.cubes[3].position,
			this.cubes[4].position
		};
		this.visualizePath = new LTSpline(array);
		LeanTween.moveSpline(this.dude1, array, 10f).setOrientToPath2d(true).setSpeed(2f);
		LeanTween.moveSplineLocal(this.dude2, array, 10f).setOrientToPath2d(true).setSpeed(2f);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00006508 File Offset: 0x00004708
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (this.visualizePath != null)
		{
			this.visualizePath.gizmoDraw(-1f);
		}
	}

	// Token: 0x0400007A RID: 122
	public Transform[] cubes;

	// Token: 0x0400007B RID: 123
	public GameObject dude1;

	// Token: 0x0400007C RID: 124
	public GameObject dude2;

	// Token: 0x0400007D RID: 125
	private LTSpline visualizePath;
}
