using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class TestingZLegacyExt : MonoBehaviour
{
	// Token: 0x060000B6 RID: 182 RVA: 0x00003212 File Offset: 0x00001412
	private void Awake()
	{
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00007DBC File Offset: 0x00005FBC
	private void Start()
	{
		this.ltLogo = GameObject.Find("LeanTweenLogo").transform;
		LeanTween.delayedCall(1f, new Action(this.cycleThroughExamples));
		this.origin = this.ltLogo.position;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00007322 File Offset: 0x00005522
	private void pauseNow()
	{
		Time.timeScale = 0f;
		Debug.Log("pausing");
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00007DFC File Offset: 0x00005FFC
	private void OnGUI()
	{
		string text = this.useEstimatedTime ? "useEstimatedTime" : ("timeScale:" + Time.timeScale.ToString());
		GUI.Label(new Rect(0.03f * (float)Screen.width, 0.03f * (float)Screen.height, 0.5f * (float)Screen.width, 0.3f * (float)Screen.height), text);
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000073A7 File Offset: 0x000055A7
	private void endlessCallback()
	{
		Debug.Log("endless");
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00007E6C File Offset: 0x0000606C
	private void cycleThroughExamples()
	{
		if (this.exampleIter == 0)
		{
			int num = (int)(this.timingType + 1);
			if (num > 4)
			{
				num = 0;
			}
			this.timingType = (TestingZLegacyExt.TimingType)num;
			this.useEstimatedTime = (this.timingType == TestingZLegacyExt.TimingType.IgnoreTimeScale);
			Time.timeScale = (this.useEstimatedTime ? 0f : 1f);
			if (this.timingType == TestingZLegacyExt.TimingType.HalfTimeScale)
			{
				Time.timeScale = 0.5f;
			}
			if (this.timingType == TestingZLegacyExt.TimingType.VariableTimeScale)
			{
				this.descrTimeScaleChangeId = base.gameObject.LeanValue(0.01f, 10f, 3f).setOnUpdate(delegate(float val)
				{
					Time.timeScale = val;
				}).setEase(LeanTweenType.easeInQuad).setUseEstimatedTime(true).setRepeat(-1).id;
			}
			else
			{
				Debug.Log("cancel variable time");
				LeanTween.cancel(this.descrTimeScaleChangeId);
			}
		}
		base.gameObject.BroadcastMessage(this.exampleFunctions[this.exampleIter]);
		float delayTime = 1.1f;
		base.gameObject.LeanDelayedCall(delayTime, new Action(this.cycleThroughExamples)).setUseEstimatedTime(this.useEstimatedTime);
		this.exampleIter = ((this.exampleIter + 1 >= this.exampleFunctions.Length) ? 0 : (this.exampleIter + 1));
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00007FB8 File Offset: 0x000061B8
	public void updateValue3Example()
	{
		Debug.Log("updateValue3Example Time:" + Time.time.ToString());
		base.gameObject.LeanValue(new Action<Vector3>(this.updateValue3ExampleCallback), new Vector3(0f, 270f, 0f), new Vector3(30f, 270f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(new Action<Vector3>(this.updateValue3ExampleUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00003212 File Offset: 0x00001412
	public void updateValue3ExampleUpdate(Vector3 val)
	{
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00008054 File Offset: 0x00006254
	public void updateValue3ExampleCallback(Vector3 val)
	{
		this.ltLogo.transform.eulerAngles = val;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00008068 File Offset: 0x00006268
	public void loopTestClamp()
	{
		Debug.Log("loopTestClamp Time:" + Time.time.ToString());
		Transform transform = GameObject.Find("Cube1").transform;
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.LeanScaleZ(4f, 1f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x000080E8 File Offset: 0x000062E8
	public void loopTestPingPong()
	{
		Debug.Log("loopTestPingPong Time:" + Time.time.ToString());
		Transform transform = GameObject.Find("Cube2").transform;
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.LeanScaleY(4f, 1f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00008164 File Offset: 0x00006364
	public void colorExample()
	{
		GameObject.Find("LCharacter").LeanColor(new Color(1f, 0f, 0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000081BC File Offset: 0x000063BC
	public void moveOnACurveExample()
	{
		Debug.Log("moveOnACurveExample Time:" + Time.time.ToString());
		Vector3[] to = new Vector3[]
		{
			this.origin,
			this.pt1.position,
			this.pt2.position,
			this.pt3.position,
			this.pt3.position,
			this.pt4.position,
			this.pt5.position,
			this.origin
		};
		this.ltLogo.LeanMove(to, 1f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000829C File Offset: 0x0000649C
	public void customTweenExample()
	{
		string str = "customTweenExample starting pos:";
		string str2 = this.ltLogo.position.ToString();
		string str3 = " origin:";
		Vector3 vector = this.origin;
		Debug.Log(str + str2 + str3 + vector.ToString());
		this.ltLogo.LeanMoveX(-10f, 0.5f).setEase(this.customAnimationCurve).setUseEstimatedTime(this.useEstimatedTime);
		this.ltLogo.LeanMoveX(0f, 0.5f).setEase(this.customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000834C File Offset: 0x0000654C
	public void moveExample()
	{
		Debug.Log("moveExample");
		this.ltLogo.LeanMove(new Vector3(-2f, -1f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		this.ltLogo.LeanMove(this.origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x000083C0 File Offset: 0x000065C0
	public void rotateExample()
	{
		Debug.Log("rotateExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("yo", 5.0);
		this.ltLogo.LeanRotate(new Vector3(0f, 360f, 0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(new Action<object>(this.rotateFinished)).setOnCompleteParam(hashtable).setOnUpdate(new Action<float>(this.rotateOnUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00003212 File Offset: 0x00001412
	public void rotateOnUpdate(float val)
	{
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00008454 File Offset: 0x00006654
	public void rotateFinished(object hash)
	{
		Hashtable hashtable = hash as Hashtable;
		string str = "rotateFinished hash:";
		object obj = hashtable["yo"];
		Debug.Log(str + ((obj != null) ? obj.ToString() : null));
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00008490 File Offset: 0x00006690
	public void scaleExample()
	{
		Debug.Log("scaleExample");
		Vector3 localScale = this.ltLogo.localScale;
		this.ltLogo.LeanScale(new Vector3(localScale.x + 0.2f, localScale.y + 0.2f, localScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00008500 File Offset: 0x00006700
	public void updateValueExample()
	{
		Debug.Log("updateValueExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("message", "hi");
		base.gameObject.LeanValue(new Action<float, object>(this.updateValueExampleCallback), this.ltLogo.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(hashtable).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00008578 File Offset: 0x00006778
	public void updateValueExampleCallback(float val, object hash)
	{
		Vector3 eulerAngles = this.ltLogo.eulerAngles;
		eulerAngles.y = val;
		this.ltLogo.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000085AA File Offset: 0x000067AA
	public void delayedCallExample()
	{
		Debug.Log("delayedCallExample");
		LeanTween.delayedCall(0.5f, new Action(this.delayedCallExampleCallback)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000085D8 File Offset: 0x000067D8
	public void delayedCallExampleCallback()
	{
		Debug.Log("Delayed function was called");
		Vector3 localScale = this.ltLogo.localScale;
		this.ltLogo.LeanScale(new Vector3(localScale.x - 0.2f, localScale.y - 0.2f, localScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00008648 File Offset: 0x00006848
	public void alphaExample()
	{
		Debug.Log("alphaExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		gameObject.LeanAlpha(0f, 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		gameObject.LeanAlpha(1f, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000086AC File Offset: 0x000068AC
	public void moveLocalExample()
	{
		Debug.Log("moveLocalExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		Vector3 localPosition = gameObject.transform.localPosition;
		gameObject.LeanMoveLocal(new Vector3(0f, 2f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		gameObject.LeanMoveLocal(localPosition, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00008725 File Offset: 0x00006925
	public void rotateAroundExample()
	{
		Debug.Log("rotateAroundExample");
		GameObject.Find("LCharacter").LeanRotateAround(Vector3.up, 360f, 1f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x0000875B File Offset: 0x0000695B
	public void loopPause()
	{
		GameObject.Find("Cube1").LeanPause();
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x0000876C File Offset: 0x0000696C
	public void loopResume()
	{
		GameObject.Find("Cube1").LeanResume();
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000877D File Offset: 0x0000697D
	public void punchTest()
	{
		this.ltLogo.LeanMoveX(7f, 1f).setEase(LeanTweenType.punch).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x040000BB RID: 187
	public AnimationCurve customAnimationCurve;

	// Token: 0x040000BC RID: 188
	public Transform pt1;

	// Token: 0x040000BD RID: 189
	public Transform pt2;

	// Token: 0x040000BE RID: 190
	public Transform pt3;

	// Token: 0x040000BF RID: 191
	public Transform pt4;

	// Token: 0x040000C0 RID: 192
	public Transform pt5;

	// Token: 0x040000C1 RID: 193
	private int exampleIter;

	// Token: 0x040000C2 RID: 194
	private string[] exampleFunctions = new string[]
	{
		"updateValue3Example",
		"loopTestClamp",
		"loopTestPingPong",
		"moveOnACurveExample",
		"customTweenExample",
		"moveExample",
		"rotateExample",
		"scaleExample",
		"updateValueExample",
		"delayedCallExample",
		"alphaExample",
		"moveLocalExample",
		"rotateAroundExample",
		"colorExample"
	};

	// Token: 0x040000C3 RID: 195
	public bool useEstimatedTime = true;

	// Token: 0x040000C4 RID: 196
	private Transform ltLogo;

	// Token: 0x040000C5 RID: 197
	private TestingZLegacyExt.TimingType timingType;

	// Token: 0x040000C6 RID: 198
	private int descrTimeScaleChangeId;

	// Token: 0x040000C7 RID: 199
	private Vector3 origin;

	// Token: 0x0200002A RID: 42
	// (Invoke) Token: 0x060000D5 RID: 213
	public delegate void NextFunc();

	// Token: 0x0200002B RID: 43
	public enum TimingType
	{
		// Token: 0x040000C9 RID: 201
		SteadyNormalTime,
		// Token: 0x040000CA RID: 202
		IgnoreTimeScale,
		// Token: 0x040000CB RID: 203
		HalfTimeScale,
		// Token: 0x040000CC RID: 204
		VariableTimeScale,
		// Token: 0x040000CD RID: 205
		Length
	}
}
