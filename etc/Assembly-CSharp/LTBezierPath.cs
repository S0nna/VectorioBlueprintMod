using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class LTBezierPath
{
	// Token: 0x060001FA RID: 506 RVA: 0x000031FE File Offset: 0x000013FE
	public LTBezierPath()
	{
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000E456 File Offset: 0x0000C656
	public LTBezierPath(Vector3[] pts_)
	{
		this.setPoints(pts_);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000E468 File Offset: 0x0000C668
	public void setPoints(Vector3[] pts_)
	{
		if (pts_.Length < 4)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, you must pass four or more values!");
		}
		if (pts_.Length % 4 != 0)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, they must be in sets of four: controlPoint1, controlPoint2, endPoint2, controlPoint2, controlPoint2...");
		}
		this.pts = pts_;
		int num = 0;
		this.beziers = new LTBezier[this.pts.Length / 4];
		this.lengthRatio = new float[this.beziers.Length];
		this.length = 0f;
		for (int i = 0; i < this.pts.Length; i += 4)
		{
			this.beziers[num] = new LTBezier(this.pts[i], this.pts[i + 2], this.pts[i + 1], this.pts[i + 3], 0.05f);
			this.length += this.beziers[num].length;
			num++;
		}
		for (int i = 0; i < this.beziers.Length; i++)
		{
			this.lengthRatio[i] = this.beziers[i].length / this.length;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060001FD RID: 509 RVA: 0x0000E57C File Offset: 0x0000C77C
	public float distance
	{
		get
		{
			return this.length;
		}
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000E584 File Offset: 0x0000C784
	public Vector3 point(float ratio)
	{
		float num = 0f;
		for (int i = 0; i < this.lengthRatio.Length; i++)
		{
			num += this.lengthRatio[i];
			if (num >= ratio)
			{
				return this.beziers[i].point((ratio - (num - this.lengthRatio[i])) / this.lengthRatio[i]);
			}
		}
		return this.beziers[this.lengthRatio.Length - 1].point(1f);
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
	public void place2d(Transform transform, float ratio)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.eulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000E664 File Offset: 0x0000C864
	public void placeLocal2d(Transform transform, float ratio)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.localPosition;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.localEulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000E6D0 File Offset: 0x0000C8D0
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000E6DF File Offset: 0x0000C8DF
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000E70D File Offset: 0x0000C90D
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000E71C File Offset: 0x0000C91C
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		ratio = Mathf.Clamp01(ratio);
		transform.localPosition = this.point(ratio);
		ratio = Mathf.Clamp01(ratio + 0.001f);
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000E770 File Offset: 0x0000C970
	public void gizmoDraw(float t = -1f)
	{
		Vector3 to = this.point(0f);
		for (int i = 1; i <= 120; i++)
		{
			float ratio = (float)i / 120f;
			Vector3 vector = this.point(ratio);
			Gizmos.color = ((this.previousBezier == this.currentBezier) ? Color.magenta : Color.grey);
			Gizmos.DrawLine(vector, to);
			to = vector;
			this.previousBezier = this.currentBezier;
		}
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000E7DC File Offset: 0x0000C9DC
	public float ratioAtPoint(Vector3 pt, float precision = 0.01f)
	{
		float num = float.MaxValue;
		int num2 = 0;
		int num3 = Mathf.RoundToInt(1f / precision);
		for (int i = 0; i < num3; i++)
		{
			float ratio = (float)i / (float)num3;
			float num4 = Vector3.Distance(pt, this.point(ratio));
			if (num4 < num)
			{
				num = num4;
				num2 = i;
			}
		}
		return (float)num2 / (float)num3;
	}

	// Token: 0x0400018D RID: 397
	public Vector3[] pts;

	// Token: 0x0400018E RID: 398
	public float length;

	// Token: 0x0400018F RID: 399
	public bool orientToPath;

	// Token: 0x04000190 RID: 400
	public bool orientToPath2d;

	// Token: 0x04000191 RID: 401
	private LTBezier[] beziers;

	// Token: 0x04000192 RID: 402
	private float[] lengthRatio;

	// Token: 0x04000193 RID: 403
	private int currentBezier;

	// Token: 0x04000194 RID: 404
	private int previousBezier;
}
