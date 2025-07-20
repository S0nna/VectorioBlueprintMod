using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class GeneralEasingTypes : MonoBehaviour
{
	// Token: 0x0600004C RID: 76 RVA: 0x00004F47 File Offset: 0x00003147
	private void Start()
	{
		this.demoEaseTypes();
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00004F50 File Offset: 0x00003150
	private void demoEaseTypes()
	{
		for (int i = 0; i < this.easeTypes.Length; i++)
		{
			string text = this.easeTypes[i];
			Transform obj1 = GameObject.Find(text).transform.Find("Line");
			float obj1val = 0f;
			LTDescr ltdescr = LeanTween.value(obj1.gameObject, 0f, 1f, 5f).setOnUpdate(delegate(float val)
			{
				Vector3 localPosition = obj1.localPosition;
				localPosition.x = obj1val * this.lineDrawScale;
				localPosition.y = val * this.lineDrawScale;
				obj1.localPosition = localPosition;
				obj1val += Time.deltaTime / 5f;
				if (obj1val > 1f)
				{
					obj1val = 0f;
				}
			});
			if (text.IndexOf("AnimationCurve") >= 0)
			{
				ltdescr.setEase(this.animationCurve);
			}
			else
			{
				ltdescr.GetType().GetMethod("set" + text).Invoke(ltdescr, null);
			}
			if (text.IndexOf("EasePunch") >= 0)
			{
				ltdescr.setScale(1f);
			}
			else if (text.IndexOf("EaseOutBounce") >= 0)
			{
				ltdescr.setOvershoot(2f);
			}
		}
		LeanTween.delayedCall(base.gameObject, 10f, new Action(this.resetLines));
		LeanTween.delayedCall(base.gameObject, 10.1f, new Action(this.demoEaseTypes));
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00005090 File Offset: 0x00003290
	private void resetLines()
	{
		for (int i = 0; i < this.easeTypes.Length; i++)
		{
			GameObject.Find(this.easeTypes[i]).transform.Find("Line").localPosition = new Vector3(0f, 0f, 0f);
		}
	}

	// Token: 0x04000049 RID: 73
	public float lineDrawScale = 10f;

	// Token: 0x0400004A RID: 74
	public AnimationCurve animationCurve;

	// Token: 0x0400004B RID: 75
	private string[] easeTypes = new string[]
	{
		"EaseLinear",
		"EaseAnimationCurve",
		"EaseSpring",
		"EaseInQuad",
		"EaseOutQuad",
		"EaseInOutQuad",
		"EaseInCubic",
		"EaseOutCubic",
		"EaseInOutCubic",
		"EaseInQuart",
		"EaseOutQuart",
		"EaseInOutQuart",
		"EaseInQuint",
		"EaseOutQuint",
		"EaseInOutQuint",
		"EaseInSine",
		"EaseOutSine",
		"EaseInOutSine",
		"EaseInExpo",
		"EaseOutExpo",
		"EaseInOutExpo",
		"EaseInCirc",
		"EaseOutCirc",
		"EaseInOutCirc",
		"EaseInBounce",
		"EaseOutBounce",
		"EaseInOutBounce",
		"EaseInBack",
		"EaseOutBack",
		"EaseInOutBack",
		"EaseInElastic",
		"EaseOutElastic",
		"EaseInOutElastic",
		"EasePunch",
		"EaseShake"
	};
}
