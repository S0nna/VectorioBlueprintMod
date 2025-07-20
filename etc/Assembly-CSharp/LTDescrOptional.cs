using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class LTDescrOptional
{
	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00015F8E File Offset: 0x0001418E
	// (set) Token: 0x060003F2 RID: 1010 RVA: 0x00015F96 File Offset: 0x00014196
	public Transform toTrans { get; set; }

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00015F9F File Offset: 0x0001419F
	// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00015FA7 File Offset: 0x000141A7
	public Vector3 point { get; set; }

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00015FB0 File Offset: 0x000141B0
	// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00015FB8 File Offset: 0x000141B8
	public Vector3 axis { get; set; }

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00015FC1 File Offset: 0x000141C1
	// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00015FC9 File Offset: 0x000141C9
	public float lastVal { get; set; }

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00015FD2 File Offset: 0x000141D2
	// (set) Token: 0x060003FA RID: 1018 RVA: 0x00015FDA File Offset: 0x000141DA
	public Quaternion origRotation { get; set; }

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x00015FE3 File Offset: 0x000141E3
	// (set) Token: 0x060003FC RID: 1020 RVA: 0x00015FEB File Offset: 0x000141EB
	public LTBezierPath path { get; set; }

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x00015FF4 File Offset: 0x000141F4
	// (set) Token: 0x060003FE RID: 1022 RVA: 0x00015FFC File Offset: 0x000141FC
	public LTSpline spline { get; set; }

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x00016005 File Offset: 0x00014205
	// (set) Token: 0x06000400 RID: 1024 RVA: 0x0001600D File Offset: 0x0001420D
	public LTRect ltRect { get; set; }

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000401 RID: 1025 RVA: 0x00016016 File Offset: 0x00014216
	// (set) Token: 0x06000402 RID: 1026 RVA: 0x0001601E File Offset: 0x0001421E
	public Action<float> onUpdateFloat { get; set; }

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000403 RID: 1027 RVA: 0x00016027 File Offset: 0x00014227
	// (set) Token: 0x06000404 RID: 1028 RVA: 0x0001602F File Offset: 0x0001422F
	public Action<float, float> onUpdateFloatRatio { get; set; }

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x00016038 File Offset: 0x00014238
	// (set) Token: 0x06000406 RID: 1030 RVA: 0x00016040 File Offset: 0x00014240
	public Action<float, object> onUpdateFloatObject { get; set; }

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x00016049 File Offset: 0x00014249
	// (set) Token: 0x06000408 RID: 1032 RVA: 0x00016051 File Offset: 0x00014251
	public Action<Vector2> onUpdateVector2 { get; set; }

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001605A File Offset: 0x0001425A
	// (set) Token: 0x0600040A RID: 1034 RVA: 0x00016062 File Offset: 0x00014262
	public Action<Vector3> onUpdateVector3 { get; set; }

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x0600040B RID: 1035 RVA: 0x0001606B File Offset: 0x0001426B
	// (set) Token: 0x0600040C RID: 1036 RVA: 0x00016073 File Offset: 0x00014273
	public Action<Vector3, object> onUpdateVector3Object { get; set; }

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001607C File Offset: 0x0001427C
	// (set) Token: 0x0600040E RID: 1038 RVA: 0x00016084 File Offset: 0x00014284
	public Action<Color> onUpdateColor { get; set; }

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001608D File Offset: 0x0001428D
	// (set) Token: 0x06000410 RID: 1040 RVA: 0x00016095 File Offset: 0x00014295
	public Action<Color, object> onUpdateColorObject { get; set; }

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000411 RID: 1041 RVA: 0x0001609E File Offset: 0x0001429E
	// (set) Token: 0x06000412 RID: 1042 RVA: 0x000160A6 File Offset: 0x000142A6
	public Action onComplete { get; set; }

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000413 RID: 1043 RVA: 0x000160AF File Offset: 0x000142AF
	// (set) Token: 0x06000414 RID: 1044 RVA: 0x000160B7 File Offset: 0x000142B7
	public Action<object> onCompleteObject { get; set; }

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000415 RID: 1045 RVA: 0x000160C0 File Offset: 0x000142C0
	// (set) Token: 0x06000416 RID: 1046 RVA: 0x000160C8 File Offset: 0x000142C8
	public object onCompleteParam { get; set; }

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000417 RID: 1047 RVA: 0x000160D1 File Offset: 0x000142D1
	// (set) Token: 0x06000418 RID: 1048 RVA: 0x000160D9 File Offset: 0x000142D9
	public object onUpdateParam { get; set; }

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000419 RID: 1049 RVA: 0x000160E2 File Offset: 0x000142E2
	// (set) Token: 0x0600041A RID: 1050 RVA: 0x000160EA File Offset: 0x000142EA
	public Action onStart { get; set; }

	// Token: 0x0600041B RID: 1051 RVA: 0x000160F4 File Offset: 0x000142F4
	public void reset()
	{
		this.animationCurve = null;
		this.onUpdateFloat = null;
		this.onUpdateFloatRatio = null;
		this.onUpdateVector2 = null;
		this.onUpdateVector3 = null;
		this.onUpdateFloatObject = null;
		this.onUpdateVector3Object = null;
		this.onUpdateColor = null;
		this.onComplete = null;
		this.onCompleteObject = null;
		this.onCompleteParam = null;
		this.onStart = null;
		this.point = Vector3.zero;
		this.initFrameCount = 0;
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00016168 File Offset: 0x00014368
	public void callOnUpdate(float val, float ratioPassed)
	{
		if (this.onUpdateFloat != null)
		{
			this.onUpdateFloat(val);
		}
		if (this.onUpdateFloatRatio != null)
		{
			this.onUpdateFloatRatio(val, ratioPassed);
			return;
		}
		if (this.onUpdateFloatObject != null)
		{
			this.onUpdateFloatObject(val, this.onUpdateParam);
			return;
		}
		if (this.onUpdateVector3Object != null)
		{
			this.onUpdateVector3Object(LTDescr.newVect, this.onUpdateParam);
			return;
		}
		if (this.onUpdateVector3 != null)
		{
			this.onUpdateVector3(LTDescr.newVect);
			return;
		}
		if (this.onUpdateVector2 != null)
		{
			this.onUpdateVector2(new Vector2(LTDescr.newVect.x, LTDescr.newVect.y));
		}
	}

	// Token: 0x04000200 RID: 512
	public AnimationCurve animationCurve;

	// Token: 0x04000201 RID: 513
	public int initFrameCount;

	// Token: 0x04000202 RID: 514
	public Color color;
}
