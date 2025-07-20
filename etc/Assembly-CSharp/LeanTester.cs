using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class LeanTester : MonoBehaviour
{
	// Token: 0x06000104 RID: 260 RVA: 0x00009538 File Offset: 0x00007738
	public void Start()
	{
		base.StartCoroutine(this.timeoutCheck());
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00009547 File Offset: 0x00007747
	private IEnumerator timeoutCheck()
	{
		float pauseEndTime = Time.realtimeSinceStartup + this.timeout;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		if (!LeanTest.testsFinished)
		{
			Debug.Log(LeanTest.formatB("Tests timed out!"));
			LeanTest.overview();
		}
		yield break;
	}

	// Token: 0x040000E5 RID: 229
	public float timeout = 15f;
}
