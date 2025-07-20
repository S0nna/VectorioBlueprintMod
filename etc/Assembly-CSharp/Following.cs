using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class Following : MonoBehaviour
{
	// Token: 0x0600002E RID: 46 RVA: 0x000033B0 File Offset: 0x000015B0
	private void Start()
	{
		this.followArrow.gameObject.LeanDelayedCall(3f, new Action(this.moveArrow)).setOnStart(new Action(this.moveArrow)).setRepeat(-1);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.localY, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.localY, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.localY, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.localY, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.localY, 50f);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.color, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.color, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.color, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.color, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.color, 0.5f);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.scale, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.scale, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.scale, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.scale, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.scale, 5f);
		Vector3 offset = new Vector3(0f, -20f, -18f);
		LeanTween.followDamp(this.dude1Title, this.dude1, LeanProp.localPosition, 0.6f, -1f).setOffset(offset);
		LeanTween.followSpring(this.dude2Title, this.dude2, LeanProp.localPosition, 0.6f, -1f, 2f, 0.5f).setOffset(offset);
		LeanTween.followBounceOut(this.dude3Title, this.dude3, LeanProp.localPosition, 0.6f, -1f, 2f, 0.5f, 0.9f).setOffset(offset);
		LeanTween.followSpring(this.dude4Title, this.dude4, LeanProp.localPosition, 0.6f, -1f, 1.5f, 0.8f).setOffset(offset);
		LeanTween.followLinear(this.dude5Title, this.dude5, LeanProp.localPosition, 30f).setOffset(offset);
		Vector3 point = Camera.main.transform.InverseTransformPoint(this.planet.transform.position);
		LeanTween.rotateAround(Camera.main.gameObject, Vector3.left, 360f, 300f).setPoint(point).setRepeat(-1);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00003738 File Offset: 0x00001938
	private void Update()
	{
		this.fromY = LeanSmooth.spring(this.fromY, this.followArrow.localPosition.y, ref this.velocityY, 1.1f, -1f, -1f, 2f, 0.5f);
		this.fromVec3 = LeanSmooth.spring(this.fromVec3, this.dude5Title.localPosition, ref this.velocityVec3, 1.1f, -1f, -1f, 2f, 0.5f);
		this.fromColor = LeanSmooth.spring(this.fromColor, this.dude1.GetComponent<Renderer>().material.color, ref this.velocityColor, 1.1f, -1f, -1f, 2f, 0.5f);
		string[] array = new string[6];
		array[0] = "Smoothed y:";
		array[1] = this.fromY.ToString();
		array[2] = " vec3:";
		int num = 3;
		Vector3 vector = this.fromVec3;
		array[num] = vector.ToString();
		array[4] = " color:";
		int num2 = 5;
		Color color = this.fromColor;
		array[num2] = color.ToString();
		Debug.Log(string.Concat(array));
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0000386C File Offset: 0x00001A6C
	private void moveArrow()
	{
		LeanTween.moveLocalY(this.followArrow.gameObject, Random.Range(-100f, 100f), 0f);
		Color to = new Color(Random.value, Random.value, Random.value);
		LeanTween.color(this.followArrow.gameObject, to, 0f);
		float d = Random.Range(5f, 10f);
		this.followArrow.localScale = Vector3.one * d;
	}

	// Token: 0x0400001F RID: 31
	public Transform planet;

	// Token: 0x04000020 RID: 32
	public Transform followArrow;

	// Token: 0x04000021 RID: 33
	public Transform dude1;

	// Token: 0x04000022 RID: 34
	public Transform dude2;

	// Token: 0x04000023 RID: 35
	public Transform dude3;

	// Token: 0x04000024 RID: 36
	public Transform dude4;

	// Token: 0x04000025 RID: 37
	public Transform dude5;

	// Token: 0x04000026 RID: 38
	public Transform dude1Title;

	// Token: 0x04000027 RID: 39
	public Transform dude2Title;

	// Token: 0x04000028 RID: 40
	public Transform dude3Title;

	// Token: 0x04000029 RID: 41
	public Transform dude4Title;

	// Token: 0x0400002A RID: 42
	public Transform dude5Title;

	// Token: 0x0400002B RID: 43
	private Color dude1ColorVelocity;

	// Token: 0x0400002C RID: 44
	private Vector3 velocityPos;

	// Token: 0x0400002D RID: 45
	private float fromY;

	// Token: 0x0400002E RID: 46
	private float velocityY;

	// Token: 0x0400002F RID: 47
	private Vector3 fromVec3;

	// Token: 0x04000030 RID: 48
	private Vector3 velocityVec3;

	// Token: 0x04000031 RID: 49
	private Color fromColor;

	// Token: 0x04000032 RID: 50
	private Color velocityColor;
}
