using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class ExampleSpline : MonoBehaviour
{
	// Token: 0x06000072 RID: 114 RVA: 0x00006270 File Offset: 0x00004470
	private void Start()
	{
		this.spline = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
		this.ltLogo = GameObject.Find("LeanTweenLogo1");
		this.ltLogo2 = GameObject.Find("LeanTweenLogo2");
		LeanTween.moveSpline(this.ltLogo2, this.spline.pts, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(true);
		LeanTween.moveSpline(this.ltLogo2, new Vector3[]
		{
			Vector3.zero,
			Vector3.zero,
			new Vector3(1f, 1f, 1f),
			new Vector3(2f, 1f, 1f),
			new Vector3(2f, 1f, 1f)
		}, 1.5f).setUseEstimatedTime(true);
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000063C8 File Offset: 0x000045C8
	private void Update()
	{
		this.ltLogo.transform.position = this.spline.point(this.iter);
		this.iter += Time.deltaTime * 0.1f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00006426 File Offset: 0x00004626
	private void OnDrawGizmos()
	{
		if (this.spline != null)
		{
			this.spline.gizmoDraw(-1f);
		}
	}

	// Token: 0x04000075 RID: 117
	public Transform[] trans;

	// Token: 0x04000076 RID: 118
	private LTSpline spline;

	// Token: 0x04000077 RID: 119
	private GameObject ltLogo;

	// Token: 0x04000078 RID: 120
	private GameObject ltLogo2;

	// Token: 0x04000079 RID: 121
	private float iter;
}
