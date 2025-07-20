using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class GeneralEventsListeners : MonoBehaviour
{
	// Token: 0x06000052 RID: 82 RVA: 0x000052CA File Offset: 0x000034CA
	private void Awake()
	{
		LeanTween.LISTENERS_MAX = 100;
		LeanTween.EVENTS_MAX = 2;
		this.fromColor = base.GetComponent<Renderer>().material.color;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000052EF File Offset: 0x000034EF
	private void Start()
	{
		LeanTween.addListener(base.gameObject, 0, new Action<LTEvent>(this.changeColor));
		LeanTween.addListener(base.gameObject, 1, new Action<LTEvent>(this.jumpUp));
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00005321 File Offset: 0x00003521
	private void jumpUp(LTEvent e)
	{
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300f);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00005340 File Offset: 0x00003540
	private void changeColor(LTEvent e)
	{
		float num = Vector3.Distance(((Transform)e.data).position, base.transform.position);
		Color to = new Color(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
		LeanTween.value(base.gameObject, this.fromColor, to, 0.8f).setLoopPingPong(1).setDelay(num * 0.05f).setOnUpdate(delegate(Color col)
		{
			base.GetComponent<Renderer>().material.color = col;
		});
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000053D3 File Offset: 0x000035D3
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.towardsRotation = new Vector3(0f, (float)Random.Range(-180, 180), 0f);
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00005408 File Offset: 0x00003608
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.turnForIter = 0f;
			this.turnForLength = Random.Range(0.5f, 1.5f);
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00005438 File Offset: 0x00003638
	private void FixedUpdate()
	{
		if (this.turnForIter < this.turnForLength)
		{
			base.GetComponent<Rigidbody>().MoveRotation(base.GetComponent<Rigidbody>().rotation * Quaternion.Euler(this.towardsRotation * Time.deltaTime));
			this.turnForIter += Time.deltaTime;
		}
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4.5f);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x000054AF File Offset: 0x000036AF
	private void OnMouseDown()
	{
		if (Input.GetKey(KeyCode.J))
		{
			LeanTween.dispatchEvent(1);
			return;
		}
		LeanTween.dispatchEvent(0, base.transform);
	}

	// Token: 0x0400004F RID: 79
	private Vector3 towardsRotation;

	// Token: 0x04000050 RID: 80
	private float turnForLength = 0.5f;

	// Token: 0x04000051 RID: 81
	private float turnForIter;

	// Token: 0x04000052 RID: 82
	private Color fromColor;

	// Token: 0x02000017 RID: 23
	public enum MyEvents
	{
		// Token: 0x04000054 RID: 84
		CHANGE_COLOR,
		// Token: 0x04000055 RID: 85
		JUMP,
		// Token: 0x04000056 RID: 86
		LENGTH
	}
}
