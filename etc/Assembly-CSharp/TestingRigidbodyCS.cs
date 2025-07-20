using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class TestingRigidbodyCS : MonoBehaviour
{
	// Token: 0x0600002B RID: 43 RVA: 0x00003340 File Offset: 0x00001540
	private void Start()
	{
		this.ball1 = GameObject.Find("Sphere1");
		LeanTween.rotateAround(this.ball1, Vector3.forward, -90f, 1f);
		LeanTween.move(this.ball1, new Vector3(2f, 0f, 7f), 1f).setDelay(1f).setRepeat(-1);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003212 File Offset: 0x00001412
	private void Update()
	{
	}

	// Token: 0x0400001E RID: 30
	private GameObject ball1;
}
