using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000040 RID: 64
[Serializable]
public class LTSpline
{
	// Token: 0x06000207 RID: 519 RVA: 0x0000E830 File Offset: 0x0000CA30
	public LTSpline(Vector3[] pts)
	{
		this.init(pts, true);
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000E847 File Offset: 0x0000CA47
	public LTSpline(Vector3[] pts, bool constantSpeed)
	{
		this.constantSpeed = constantSpeed;
		this.init(pts, constantSpeed);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000E868 File Offset: 0x0000CA68
	private void init(Vector3[] pts, bool constantSpeed)
	{
		if (pts.Length < 4)
		{
			LeanTween.logError("LeanTween - When passing values for a spline path, you must pass four or more values!");
			return;
		}
		this.pts = new Vector3[pts.Length];
		Array.Copy(pts, this.pts, pts.Length);
		this.numSections = pts.Length - 3;
		float num = float.PositiveInfinity;
		Vector3 vector = this.pts[1];
		float num2 = 0f;
		for (int i = 1; i < this.pts.Length - 1; i++)
		{
			float num3 = Vector3.Distance(this.pts[i], vector);
			if (num3 < num)
			{
				num = num3;
			}
			num2 += num3;
		}
		if (constantSpeed)
		{
			num = num2 / (float)(this.numSections * LTSpline.SUBLINE_COUNT);
			float num4 = num / (float)LTSpline.SUBLINE_COUNT;
			int num5 = (int)Mathf.Ceil(num2 / num4) * LTSpline.DISTANCE_COUNT;
			if (num5 <= 1)
			{
				num5 = 2;
			}
			this.ptsAdj = new Vector3[num5];
			vector = this.interp(0f);
			int num6 = 1;
			this.ptsAdj[0] = vector;
			this.distance = 0f;
			for (int j = 0; j < num5 + 1; j++)
			{
				float num7 = (float)j / (float)num5;
				Vector3 vector2 = this.interp(num7);
				float num8 = Vector3.Distance(vector2, vector);
				if (num8 >= num4 || num7 >= 1f)
				{
					this.ptsAdj[num6] = vector2;
					this.distance += num8;
					vector = vector2;
					num6++;
				}
			}
			this.ptsAdjLength = num6;
		}
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000E9DC File Offset: 0x0000CBDC
	public Vector3 map(float u)
	{
		if (u >= 1f)
		{
			return this.pts[this.pts.Length - 2];
		}
		float num = u * (float)(this.ptsAdjLength - 1);
		int num2 = (int)Mathf.Floor(num);
		int num3 = (int)Mathf.Ceil(num);
		if (num2 < 0)
		{
			num2 = 0;
		}
		Vector3 vector = this.ptsAdj[num2];
		Vector3 a = this.ptsAdj[num3];
		float d = num - (float)num2;
		return vector + (a - vector) * d;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000EA60 File Offset: 0x0000CC60
	public Vector3 interp(float t)
	{
		this.currPt = Mathf.Min(Mathf.FloorToInt(t * (float)this.numSections), this.numSections - 1);
		float num = t * (float)this.numSections - (float)this.currPt;
		Vector3 a = this.pts[this.currPt];
		Vector3 a2 = this.pts[this.currPt + 1];
		Vector3 vector = this.pts[this.currPt + 2];
		Vector3 b = this.pts[this.currPt + 3];
		return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num * num * num) + (2f * a - 5f * a2 + 4f * vector - b) * (num * num) + (-a + vector) * num + 2f * a2);
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000EB98 File Offset: 0x0000CD98
	public float ratioAtPoint(Vector3 pt)
	{
		float num = float.MaxValue;
		int num2 = 0;
		for (int i = 0; i < this.ptsAdjLength; i++)
		{
			float num3 = Vector3.Distance(pt, this.ptsAdj[i]);
			if (num3 < num)
			{
				num = num3;
				num2 = i;
			}
		}
		return (float)num2 / (float)(this.ptsAdjLength - 1);
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
	public Vector3 point(float ratio)
	{
		float num = (ratio > 1f) ? 1f : ratio;
		if (!this.constantSpeed)
		{
			return this.interp(num);
		}
		return this.map(num);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000EC20 File Offset: 0x0000CE20
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

	// Token: 0x0600020F RID: 527 RVA: 0x0000EC8C File Offset: 0x0000CE8C
	public void placeLocal2d(Transform transform, float ratio)
	{
		if (transform.parent == null)
		{
			this.place2d(transform, ratio);
			return;
		}
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.localPosition;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.localEulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000ED0F File Offset: 0x0000CF0F
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000ED1E File Offset: 0x0000CF1E
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000ED4C File Offset: 0x0000CF4C
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000ED5B File Offset: 0x0000CF5B
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000ED94 File Offset: 0x0000CF94
	public void gizmoDraw(float t = -1f)
	{
		if (this.ptsAdj == null || this.ptsAdj.Length == 0)
		{
			return;
		}
		Vector3 from = this.ptsAdj[0];
		for (int i = 0; i < this.ptsAdjLength; i++)
		{
			Vector3 vector = this.ptsAdj[i];
			Gizmos.DrawLine(from, vector);
			from = vector;
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
	public void drawGizmo(Color color)
	{
		if (this.ptsAdjLength >= 4)
		{
			Vector3 from = this.ptsAdj[0];
			Color color2 = Gizmos.color;
			Gizmos.color = color;
			for (int i = 0; i < this.ptsAdjLength; i++)
			{
				Vector3 vector = this.ptsAdj[i];
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
			Gizmos.color = color2;
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000EE44 File Offset: 0x0000D044
	public static void drawGizmo(Transform[] arr, Color color)
	{
		if (arr.Length >= 4)
		{
			Vector3[] array = new Vector3[arr.Length];
			for (int i = 0; i < arr.Length; i++)
			{
				array[i] = arr[i].position;
			}
			LTSpline ltspline = new LTSpline(array);
			Vector3 from = ltspline.ptsAdj[0];
			Color color2 = Gizmos.color;
			Gizmos.color = color;
			for (int j = 0; j < ltspline.ptsAdjLength; j++)
			{
				Vector3 vector = ltspline.ptsAdj[j];
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
			Gizmos.color = color2;
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000EEDB File Offset: 0x0000D0DB
	public static void drawLine(Transform[] arr, float width, Color color)
	{
		int num = arr.Length;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000EEE4 File Offset: 0x0000D0E4
	public void drawLinesGLLines(Material outlineMaterial, Color color, float width)
	{
		GL.PushMatrix();
		outlineMaterial.SetPass(0);
		GL.LoadPixelMatrix();
		GL.Begin(1);
		GL.Color(color);
		if (this.constantSpeed)
		{
			if (this.ptsAdjLength >= 4)
			{
				Vector3 v = this.ptsAdj[0];
				for (int i = 0; i < this.ptsAdjLength; i++)
				{
					Vector3 vector = this.ptsAdj[i];
					GL.Vertex(v);
					GL.Vertex(vector);
					v = vector;
				}
			}
		}
		else if (this.pts.Length >= 4)
		{
			Vector3 v2 = this.pts[0];
			float num = 1f / ((float)this.pts.Length * 10f);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float t = num2 / 1f;
				Vector3 vector2 = this.interp(t);
				GL.Vertex(v2);
				GL.Vertex(vector2);
				v2 = vector2;
			}
		}
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000EFCC File Offset: 0x0000D1CC
	public Vector3[] generateVectors()
	{
		if (this.pts.Length >= 4)
		{
			List<Vector3> list = new List<Vector3>();
			Vector3 item = this.pts[0];
			list.Add(item);
			float num = 1f / ((float)this.pts.Length * 10f);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float t = num2 / 1f;
				Vector3 item2 = this.interp(t);
				list.Add(item2);
			}
			list.ToArray();
		}
		return null;
	}

	// Token: 0x04000195 RID: 405
	public static int DISTANCE_COUNT = 3;

	// Token: 0x04000196 RID: 406
	public static int SUBLINE_COUNT = 20;

	// Token: 0x04000197 RID: 407
	public float distance;

	// Token: 0x04000198 RID: 408
	public bool constantSpeed = true;

	// Token: 0x04000199 RID: 409
	public Vector3[] pts;

	// Token: 0x0400019A RID: 410
	[NonSerialized]
	public Vector3[] ptsAdj;

	// Token: 0x0400019B RID: 411
	public int ptsAdjLength;

	// Token: 0x0400019C RID: 412
	public bool orientToPath;

	// Token: 0x0400019D RID: 413
	public bool orientToPath2d;

	// Token: 0x0400019E RID: 414
	private int numSections;

	// Token: 0x0400019F RID: 415
	private int currPt;
}
