using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class PathSplines : MonoBehaviour
{
	// Token: 0x06000086 RID: 134 RVA: 0x00006DE0 File Offset: 0x00004FE0
	private void OnEnable()
	{
		this.cr = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00006E64 File Offset: 0x00005064
	private void Start()
	{
		this.avatar1 = GameObject.Find("Avatar1");
		LeanTween.move(this.avatar1, this.cr, 6.5f).setOrientToPath(true).setRepeat(1).setOnComplete(delegate()
		{
			Vector3[] to = new Vector3[]
			{
				this.trans[4].position,
				this.trans[3].position,
				this.trans[2].position,
				this.trans[1].position,
				this.trans[0].position
			};
			LeanTween.moveSpline(this.avatar1, to, 6.5f);
		}).setEase(LeanTweenType.easeOutQuad);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00006EBB File Offset: 0x000050BB
	private void Update()
	{
		this.iter += Time.deltaTime * 0.07f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00006EED File Offset: 0x000050ED
	private void OnDrawGizmos()
	{
		if (this.cr == null)
		{
			this.OnEnable();
		}
		Gizmos.color = Color.red;
		if (this.cr != null)
		{
			this.cr.gizmoDraw(-1f);
		}
	}

	// Token: 0x0400009B RID: 155
	public Transform[] trans;

	// Token: 0x0400009C RID: 156
	private LTSpline cr;

	// Token: 0x0400009D RID: 157
	private GameObject avatar1;

	// Token: 0x0400009E RID: 158
	private float iter;
}
