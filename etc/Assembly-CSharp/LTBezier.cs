using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class LTBezier
{
	// Token: 0x060001F6 RID: 502 RVA: 0x0000E280 File Offset: 0x0000C480
	public LTBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float precision)
	{
		this.a = a;
		this.aa = -a + 3f * (b - c) + d;
		this.bb = 3f * (a + c) - 6f * b;
		this.cc = 3f * (b - a);
		this.len = 1f / precision;
		this.arcLengths = new float[(int)this.len + 1];
		this.arcLengths[0] = 0f;
		Vector3 vector = a;
		float num = 0f;
		int num2 = 1;
		while ((float)num2 <= this.len)
		{
			Vector3 vector2 = this.bezierPoint((float)num2 * precision);
			num += (vector - vector2).magnitude;
			this.arcLengths[num2] = num;
			vector = vector2;
			num2++;
		}
		this.length = num;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000E37C File Offset: 0x0000C57C
	private float map(float u)
	{
		float num = u * this.arcLengths[(int)this.len];
		int i = 0;
		int num2 = (int)this.len;
		int num3 = 0;
		while (i < num2)
		{
			num3 = i + ((int)((float)(num2 - i) / 2f) | 0);
			if (this.arcLengths[num3] < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		if (this.arcLengths[num3] > num)
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		return ((float)num3 + (num - this.arcLengths[num3]) / (this.arcLengths[num3 + 1] - this.arcLengths[num3])) / this.len;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000E40C File Offset: 0x0000C60C
	private Vector3 bezierPoint(float t)
	{
		return ((this.aa * t + this.bb) * t + this.cc) * t + this.a;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000E447 File Offset: 0x0000C647
	public Vector3 point(float t)
	{
		return this.bezierPoint(this.map(t));
	}

	// Token: 0x04000186 RID: 390
	public float length;

	// Token: 0x04000187 RID: 391
	private Vector3 a;

	// Token: 0x04000188 RID: 392
	private Vector3 aa;

	// Token: 0x04000189 RID: 393
	private Vector3 bb;

	// Token: 0x0400018A RID: 394
	private Vector3 cc;

	// Token: 0x0400018B RID: 395
	private float len;

	// Token: 0x0400018C RID: 396
	private float[] arcLengths;
}
