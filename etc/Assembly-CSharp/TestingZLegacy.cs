using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class TestingZLegacy : MonoBehaviour
{
	// Token: 0x06000091 RID: 145 RVA: 0x00003212 File Offset: 0x00001412
	private void Awake()
	{
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000072E3 File Offset: 0x000054E3
	private void Start()
	{
		this.ltLogo = GameObject.Find("LeanTweenLogo");
		LeanTween.delayedCall(1f, new Action(this.cycleThroughExamples));
		this.origin = this.ltLogo.transform.position;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00007322 File Offset: 0x00005522
	private void pauseNow()
	{
		Time.timeScale = 0f;
		Debug.Log("pausing");
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00007338 File Offset: 0x00005538
	private void OnGUI()
	{
		string text = this.useEstimatedTime ? "useEstimatedTime" : ("timeScale:" + Time.timeScale.ToString());
		GUI.Label(new Rect(0.03f * (float)Screen.width, 0.03f * (float)Screen.height, 0.5f * (float)Screen.width, 0.3f * (float)Screen.height), text);
	}

	// Token: 0x06000095 RID: 149 RVA: 0x000073A7 File Offset: 0x000055A7
	private void endlessCallback()
	{
		Debug.Log("endless");
	}

	// Token: 0x06000096 RID: 150 RVA: 0x000073B4 File Offset: 0x000055B4
	private void cycleThroughExamples()
	{
		if (this.exampleIter == 0)
		{
			int num = (int)(this.timingType + 1);
			if (num > 4)
			{
				num = 0;
			}
			this.timingType = (TestingZLegacy.TimingType)num;
			this.useEstimatedTime = (this.timingType == TestingZLegacy.TimingType.IgnoreTimeScale);
			Time.timeScale = (this.useEstimatedTime ? 0f : 1f);
			if (this.timingType == TestingZLegacy.TimingType.HalfTimeScale)
			{
				Time.timeScale = 0.5f;
			}
			if (this.timingType == TestingZLegacy.TimingType.VariableTimeScale)
			{
				this.descrTimeScaleChangeId = LeanTween.value(base.gameObject, 0.01f, 10f, 3f).setOnUpdate(delegate(float val)
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
		LeanTween.delayedCall(base.gameObject, delayTime, new Action(this.cycleThroughExamples)).setUseEstimatedTime(this.useEstimatedTime);
		this.exampleIter = ((this.exampleIter + 1 >= this.exampleFunctions.Length) ? 0 : (this.exampleIter + 1));
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00007500 File Offset: 0x00005700
	public void updateValue3Example()
	{
		Debug.Log("updateValue3Example Time:" + Time.time.ToString());
		LeanTween.value(base.gameObject, new Action<Vector3>(this.updateValue3ExampleCallback), new Vector3(0f, 270f, 0f), new Vector3(30f, 270f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(new Action<Vector3>(this.updateValue3ExampleUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00003212 File Offset: 0x00001412
	public void updateValue3ExampleUpdate(Vector3 val)
	{
	}

	// Token: 0x06000099 RID: 153 RVA: 0x0000759C File Offset: 0x0000579C
	public void updateValue3ExampleCallback(Vector3 val)
	{
		this.ltLogo.transform.eulerAngles = val;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x000075B0 File Offset: 0x000057B0
	public void loopTestClamp()
	{
		Debug.Log("loopTestClamp Time:" + Time.time.ToString());
		GameObject gameObject = GameObject.Find("Cube1");
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		LeanTween.scaleZ(gameObject, 4f, 1f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00007630 File Offset: 0x00005830
	public void loopTestPingPong()
	{
		Debug.Log("loopTestPingPong Time:" + Time.time.ToString());
		GameObject gameObject = GameObject.Find("Cube2");
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		LeanTween.scaleY(gameObject, 4f, 1f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000076AC File Offset: 0x000058AC
	public void colorExample()
	{
		LeanTween.color(GameObject.Find("LCharacter"), new Color(1f, 0f, 0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00007704 File Offset: 0x00005904
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
		LeanTween.move(this.ltLogo, to, 1f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000077E4 File Offset: 0x000059E4
	public void customTweenExample()
	{
		string str = "customTweenExample starting pos:";
		string str2 = this.ltLogo.transform.position.ToString();
		string str3 = " origin:";
		Vector3 vector = this.origin;
		Debug.Log(str + str2 + str3 + vector.ToString());
		LeanTween.moveX(this.ltLogo, -10f, 0.5f).setEase(this.customAnimationCurve).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.moveX(this.ltLogo, 0f, 0.5f).setEase(this.customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0000789C File Offset: 0x00005A9C
	public void moveExample()
	{
		Debug.Log("moveExample");
		LeanTween.move(this.ltLogo, new Vector3(-2f, -1f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.move(this.ltLogo, this.origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00007910 File Offset: 0x00005B10
	public void rotateExample()
	{
		Debug.Log("rotateExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("yo", 5.0);
		LeanTween.rotate(this.ltLogo, new Vector3(0f, 360f, 0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(new Action<object>(this.rotateFinished)).setOnCompleteParam(hashtable).setOnUpdate(new Action<float>(this.rotateOnUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00003212 File Offset: 0x00001412
	public void rotateOnUpdate(float val)
	{
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000079A4 File Offset: 0x00005BA4
	public void rotateFinished(object hash)
	{
		Hashtable hashtable = hash as Hashtable;
		string str = "rotateFinished hash:";
		object obj = hashtable["yo"];
		Debug.Log(str + ((obj != null) ? obj.ToString() : null));
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x000079E0 File Offset: 0x00005BE0
	public void scaleExample()
	{
		Debug.Log("scaleExample");
		Vector3 localScale = this.ltLogo.transform.localScale;
		LeanTween.scale(this.ltLogo, new Vector3(localScale.x + 0.2f, localScale.y + 0.2f, localScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00007A54 File Offset: 0x00005C54
	public void updateValueExample()
	{
		Debug.Log("updateValueExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("message", "hi");
		LeanTween.value(base.gameObject, new Action<float, object>(this.updateValueExampleCallback), this.ltLogo.transform.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(hashtable).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00007AD0 File Offset: 0x00005CD0
	public void updateValueExampleCallback(float val, object hash)
	{
		Vector3 eulerAngles = this.ltLogo.transform.eulerAngles;
		eulerAngles.y = val;
		this.ltLogo.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00007B07 File Offset: 0x00005D07
	public void delayedCallExample()
	{
		Debug.Log("delayedCallExample");
		LeanTween.delayedCall(0.5f, new Action(this.delayedCallExampleCallback)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00007B38 File Offset: 0x00005D38
	public void delayedCallExampleCallback()
	{
		Debug.Log("Delayed function was called");
		Vector3 localScale = this.ltLogo.transform.localScale;
		LeanTween.scale(this.ltLogo, new Vector3(localScale.x - 0.2f, localScale.y - 0.2f, localScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00007BAC File Offset: 0x00005DAC
	public void alphaExample()
	{
		Debug.Log("alphaExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		LeanTween.alpha(gameObject, 0f, 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.alpha(gameObject, 1f, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00007C10 File Offset: 0x00005E10
	public void moveLocalExample()
	{
		Debug.Log("moveLocalExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		Vector3 localPosition = gameObject.transform.localPosition;
		LeanTween.moveLocal(gameObject, new Vector3(0f, 2f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.moveLocal(gameObject, localPosition, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00007C89 File Offset: 0x00005E89
	public void rotateAroundExample()
	{
		Debug.Log("rotateAroundExample");
		LeanTween.rotateAround(GameObject.Find("LCharacter"), Vector3.up, 360f, 1f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00007CBF File Offset: 0x00005EBF
	public void loopPause()
	{
		LeanTween.pause(GameObject.Find("Cube1"));
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00007CD0 File Offset: 0x00005ED0
	public void loopResume()
	{
		LeanTween.resume(GameObject.Find("Cube1"));
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00007CE1 File Offset: 0x00005EE1
	public void punchTest()
	{
		LeanTween.moveX(this.ltLogo, 7f, 1f).setEase(LeanTweenType.punch).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x040000A6 RID: 166
	public AnimationCurve customAnimationCurve;

	// Token: 0x040000A7 RID: 167
	public Transform pt1;

	// Token: 0x040000A8 RID: 168
	public Transform pt2;

	// Token: 0x040000A9 RID: 169
	public Transform pt3;

	// Token: 0x040000AA RID: 170
	public Transform pt4;

	// Token: 0x040000AB RID: 171
	public Transform pt5;

	// Token: 0x040000AC RID: 172
	private int exampleIter;

	// Token: 0x040000AD RID: 173
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

	// Token: 0x040000AE RID: 174
	public bool useEstimatedTime = true;

	// Token: 0x040000AF RID: 175
	private GameObject ltLogo;

	// Token: 0x040000B0 RID: 176
	private TestingZLegacy.TimingType timingType;

	// Token: 0x040000B1 RID: 177
	private int descrTimeScaleChangeId;

	// Token: 0x040000B2 RID: 178
	private Vector3 origin;

	// Token: 0x02000026 RID: 38
	// (Invoke) Token: 0x060000B0 RID: 176
	public delegate void NextFunc();

	// Token: 0x02000027 RID: 39
	public enum TimingType
	{
		// Token: 0x040000B4 RID: 180
		SteadyNormalTime,
		// Token: 0x040000B5 RID: 181
		IgnoreTimeScale,
		// Token: 0x040000B6 RID: 182
		HalfTimeScale,
		// Token: 0x040000B7 RID: 183
		VariableTimeScale,
		// Token: 0x040000B8 RID: 184
		Length
	}
}
