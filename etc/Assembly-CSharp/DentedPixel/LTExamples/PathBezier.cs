using System;
using UnityEngine;

namespace DentedPixel.LTExamples
{
	// Token: 0x0200035D RID: 861
	public class PathBezier : MonoBehaviour
	{
		// Token: 0x0600168F RID: 5775 RVA: 0x00069878 File Offset: 0x00067A78
		private void OnEnable()
		{
			this.cr = new LTBezierPath(new Vector3[]
			{
				this.trans[0].position,
				this.trans[2].position,
				this.trans[1].position,
				this.trans[3].position,
				this.trans[3].position,
				this.trans[5].position,
				this.trans[4].position,
				this.trans[6].position
			});
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00069938 File Offset: 0x00067B38
		private void Start()
		{
			this.avatar1 = GameObject.Find("Avatar1");
			LTDescr ltdescr = LeanTween.move(this.avatar1, this.cr.pts, 6.5f).setOrientToPath(true).setRepeat(-1);
			Debug.Log("length of path 1:" + this.cr.length.ToString());
			Debug.Log("length of path 2:" + ltdescr.optional.path.length.ToString());
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000699C0 File Offset: 0x00067BC0
		private void Update()
		{
			this.iter += Time.deltaTime * 0.07f;
			if (this.iter > 1f)
			{
				this.iter = 0f;
			}
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x000699F2 File Offset: 0x00067BF2
		private void OnDrawGizmos()
		{
			if (this.cr != null)
			{
				this.OnEnable();
			}
			Gizmos.color = Color.red;
			if (this.cr != null)
			{
				this.cr.gizmoDraw(-1f);
			}
		}

		// Token: 0x04001582 RID: 5506
		public Transform[] trans;

		// Token: 0x04001583 RID: 5507
		private LTBezierPath cr;

		// Token: 0x04001584 RID: 5508
		private GameObject avatar1;

		// Token: 0x04001585 RID: 5509
		private float iter;
	}
}
