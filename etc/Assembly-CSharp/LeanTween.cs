using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000038 RID: 56
public class LeanTween : MonoBehaviour
{
	// Token: 0x06000117 RID: 279 RVA: 0x0000991F File Offset: 0x00007B1F
	public static void init()
	{
		LeanTween.init(LeanTween.maxTweens);
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000118 RID: 280 RVA: 0x0000992B File Offset: 0x00007B2B
	public static int maxSearch
	{
		get
		{
			return LeanTween.tweenMaxSearch;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000119 RID: 281 RVA: 0x00009932 File Offset: 0x00007B32
	public static int maxSimulataneousTweens
	{
		get
		{
			return LeanTween.maxTweens;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600011A RID: 282 RVA: 0x0000993C File Offset: 0x00007B3C
	public static int tweensRunning
	{
		get
		{
			int num = 0;
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000996E File Offset: 0x00007B6E
	public static void init(int maxSimultaneousTweens)
	{
		LeanTween.init(maxSimultaneousTweens, LeanTween.maxSequences);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0000997C File Offset: 0x00007B7C
	public static void init(int maxSimultaneousTweens, int maxSimultaneousSequences)
	{
		if (LeanTween.tweens == null)
		{
			LeanTween.maxTweens = maxSimultaneousTweens;
			LeanTween.tweens = new LTDescr[LeanTween.maxTweens];
			LeanTween.tweensFinished = new int[LeanTween.maxTweens];
			LeanTween.tweensFinishedIds = new int[LeanTween.maxTweens];
			LeanTween._tweenEmpty = new GameObject();
			LeanTween._tweenEmpty.name = "~LeanTween";
			LeanTween._tweenEmpty.AddComponent(typeof(LeanTween));
			LeanTween._tweenEmpty.isStatic = true;
			LeanTween._tweenEmpty.hideFlags = HideFlags.HideAndDontSave;
			Object.DontDestroyOnLoad(LeanTween._tweenEmpty);
			for (int i = 0; i < LeanTween.maxTweens; i++)
			{
				LeanTween.tweens[i] = new LTDescr();
			}
			SceneManager.sceneLoaded += LeanTween.onLevelWasLoaded54;
			LeanTween.sequences = new LTSeq[maxSimultaneousSequences];
			for (int j = 0; j < maxSimultaneousSequences; j++)
			{
				LeanTween.sequences[j] = new LTSeq();
			}
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00009A68 File Offset: 0x00007C68
	public static void reset()
	{
		if (LeanTween.tweens != null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i] != null)
				{
					LeanTween.tweens[i].toggle = false;
				}
			}
		}
		LeanTween.tweens = null;
		Object.Destroy(LeanTween._tweenEmpty);
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00009AB2 File Offset: 0x00007CB2
	public void Update()
	{
		LeanTween.update();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00009AB9 File Offset: 0x00007CB9
	private static void onLevelWasLoaded54(Scene scene, LoadSceneMode mode)
	{
		LeanTween.internalOnLevelWasLoaded(scene.buildIndex);
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00009AC7 File Offset: 0x00007CC7
	private static void internalOnLevelWasLoaded(int lvl)
	{
		LTGUI.reset();
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00009AD0 File Offset: 0x00007CD0
	public static void update()
	{
		if (LeanTween.frameRendered != Time.frameCount)
		{
			LeanTween.init();
			LeanTween.dtEstimated = ((LeanTween.dtEstimated < 0f) ? 0f : (LeanTween.dtEstimated = Time.unscaledDeltaTime));
			LeanTween.dtActual = Time.deltaTime;
			LeanTween.maxTweenReached = 0;
			LeanTween.finishedCnt = 0;
			int num = 0;
			while (num <= LeanTween.tweenMaxSearch && num < LeanTween.maxTweens)
			{
				LeanTween.tween = LeanTween.tweens[num];
				if (LeanTween.tween.toggle)
				{
					LeanTween.maxTweenReached = num;
					if (LeanTween.tween.updateInternal())
					{
						LeanTween.tweensFinished[LeanTween.finishedCnt] = num;
						LeanTween.tweensFinishedIds[LeanTween.finishedCnt] = LeanTween.tweens[num].id;
						LeanTween.finishedCnt++;
					}
				}
				num++;
			}
			LeanTween.tweenMaxSearch = LeanTween.maxTweenReached;
			LeanTween.frameRendered = Time.frameCount;
			for (int i = 0; i < LeanTween.finishedCnt; i++)
			{
				LeanTween.j = LeanTween.tweensFinished[i];
				LeanTween.tween = LeanTween.tweens[LeanTween.j];
				if (LeanTween.tween.id == LeanTween.tweensFinishedIds[i])
				{
					LeanTween.removeTween(LeanTween.j);
					if (LeanTween.tween.hasExtraOnCompletes && LeanTween.tween.trans != null)
					{
						LeanTween.tween.callOnCompletes();
					}
				}
			}
		}
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00009C20 File Offset: 0x00007E20
	public static void removeTween(int i, int uniqueId)
	{
		if (LeanTween.tweens[i].uniqueId == uniqueId)
		{
			LeanTween.removeTween(i);
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00009C38 File Offset: 0x00007E38
	public static void removeTween(int i)
	{
		if (LeanTween.tweens[i].toggle)
		{
			LeanTween.tweens[i].toggle = false;
			LeanTween.tweens[i].counter = uint.MaxValue;
			if (LeanTween.tweens[i].destroyOnComplete)
			{
				if (LeanTween.tweens[i]._optional.ltRect != null)
				{
					LTGUI.destroy(LeanTween.tweens[i]._optional.ltRect.id);
				}
				else if (LeanTween.tweens[i].trans != null && LeanTween.tweens[i].trans.gameObject != LeanTween._tweenEmpty)
				{
					Object.Destroy(LeanTween.tweens[i].trans.gameObject);
				}
			}
			LeanTween.startSearch = i;
			if (i + 1 >= LeanTween.tweenMaxSearch)
			{
				LeanTween.startSearch = 0;
			}
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00009D0C File Offset: 0x00007F0C
	public static Vector3[] add(Vector3[] a, Vector3 b)
	{
		Vector3[] array = new Vector3[a.Length];
		LeanTween.i = 0;
		while (LeanTween.i < a.Length)
		{
			array[LeanTween.i] = a[LeanTween.i] + b;
			LeanTween.i++;
		}
		return array;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00009D60 File Offset: 0x00007F60
	public static float closestRot(float from, float to)
	{
		float num = 0f - (360f - to);
		float num2 = 360f + to;
		float num3 = Mathf.Abs(to - from);
		float num4 = Mathf.Abs(num - from);
		float num5 = Mathf.Abs(num2 - from);
		if (num3 < num4 && num3 < num5)
		{
			return to;
		}
		if (num4 < num5)
		{
			return num;
		}
		return num2;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00009DB2 File Offset: 0x00007FB2
	public static void cancelAll()
	{
		LeanTween.cancelAll(false);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00009DBC File Offset: 0x00007FBC
	public static void cancelAll(bool callComplete)
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans != null)
			{
				if (callComplete && LeanTween.tweens[i].optional.onComplete != null)
				{
					LeanTween.tweens[i].optional.onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00009E24 File Offset: 0x00008024
	public static void cancel(GameObject gameObject)
	{
		LeanTween.cancel(gameObject, false);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00009E30 File Offset: 0x00008030
	public static void cancel(GameObject gameObject, bool callOnComplete)
	{
		LeanTween.init();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LTDescr ltdescr = LeanTween.tweens[i];
			if (ltdescr != null && ltdescr.toggle && ltdescr.trans == transform)
			{
				if (callOnComplete && ltdescr.optional.onComplete != null)
				{
					ltdescr.optional.onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00009EA0 File Offset: 0x000080A0
	public static void cancel(RectTransform rect)
	{
		LeanTween.cancel(rect.gameObject, false);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00009EB0 File Offset: 0x000080B0
	public static void cancel(GameObject gameObject, int uniqueId, bool callOnComplete = false)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num].trans == null || (LeanTween.tweens[num].trans.gameObject == gameObject && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2)))
			{
				if (callOnComplete && LeanTween.tweens[num].optional.onComplete != null)
				{
					LeanTween.tweens[num].optional.onComplete();
				}
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00009F48 File Offset: 0x00008148
	public static void cancel(LTRect ltRect, int uniqueId)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num]._optional.ltRect == ltRect && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00009F96 File Offset: 0x00008196
	public static void cancel(int uniqueId)
	{
		LeanTween.cancel(uniqueId, false);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00009FA0 File Offset: 0x000081A0
	public static void cancel(int uniqueId, bool callOnComplete)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (num > LeanTween.tweens.Length - 1)
			{
				int num3 = num - LeanTween.tweens.Length;
				LTSeq ltseq = LeanTween.sequences[num3];
				for (int i = 0; i < LeanTween.maxSequences; i++)
				{
					if (ltseq.current.tween != null)
					{
						LeanTween.removeTween(ltseq.current.tween.uniqueId & 65535);
					}
					if (ltseq.current.previous == null)
					{
						return;
					}
					ltseq.current = ltseq.current.previous;
				}
				return;
			}
			if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				if (callOnComplete && LeanTween.tweens[num].optional.onComplete != null)
				{
					LeanTween.tweens[num].optional.onComplete();
				}
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000A084 File Offset: 0x00008284
	public static LTDescr descr(int uniqueId)
	{
		LeanTween.init();
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if (LeanTween.tweens[num] != null && LeanTween.tweens[num].uniqueId == uniqueId && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			return LeanTween.tweens[num];
		}
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].uniqueId == uniqueId && (ulong)LeanTween.tweens[i].counter == (ulong)((long)num2))
			{
				return LeanTween.tweens[i];
			}
		}
		return null;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000A10D File Offset: 0x0000830D
	public static LTDescr description(int uniqueId)
	{
		return LeanTween.descr(uniqueId);
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000A118 File Offset: 0x00008318
	public static LTDescr[] descriptions(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			return null;
		}
		List<LTDescr> list = new List<LTDescr>();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].trans == transform)
			{
				list.Add(LeanTween.tweens[i]);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000A182 File Offset: 0x00008382
	[Obsolete("Use 'pause( id )' instead")]
	public static void pause(GameObject gameObject, int uniqueId)
	{
		LeanTween.pause(uniqueId);
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000A18C File Offset: 0x0000838C
	public static void pause(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].pause();
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000A1C4 File Offset: 0x000083C4
	public static void pause(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].pause();
			}
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000A20C File Offset: 0x0000840C
	public static void pauseAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].pause();
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000A23C File Offset: 0x0000843C
	public static void resumeAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].resume();
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000A26B File Offset: 0x0000846B
	[Obsolete("Use 'resume( id )' instead")]
	public static void resume(GameObject gameObject, int uniqueId)
	{
		LeanTween.resume(uniqueId);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000A274 File Offset: 0x00008474
	public static void resume(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].resume();
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000A2AC File Offset: 0x000084AC
	public static void resume(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].resume();
			}
		}
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000A2F4 File Offset: 0x000084F4
	public static bool isPaused(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (object.Equals(LeanTween.tweens[i].direction, 0f))
				{
					return true;
				}
			}
			return false;
		}
		Transform transform = gameObject.transform;
		for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
		{
			if (object.Equals(LeanTween.tweens[j].direction, 0f) && LeanTween.tweens[j].trans == transform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000A390 File Offset: 0x00008590
	public static bool isPaused(RectTransform rect)
	{
		return LeanTween.isTweening(rect.gameObject);
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000A3A0 File Offset: 0x000085A0
	public static bool isPaused(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && object.Equals(LeanTween.tweens[LeanTween.i].direction, 0f));
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000A404 File Offset: 0x00008604
	public static bool isTweening(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					return true;
				}
			}
			return false;
		}
		Transform transform = gameObject.transform;
		for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
		{
			if (LeanTween.tweens[j].toggle && LeanTween.tweens[j].trans == transform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000A390 File Offset: 0x00008590
	public static bool isTweening(RectTransform rect)
	{
		return LeanTween.isTweening(rect.gameObject);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000A478 File Offset: 0x00008678
	public static bool isTweening(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && LeanTween.tweens[num].toggle);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000A4C4 File Offset: 0x000086C4
	public static bool isTweening(LTRect ltRect)
	{
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i]._optional.ltRect == ltRect)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000A508 File Offset: 0x00008708
	public static void drawBezierPath(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float arrowSize = 0f, Transform arrowTransform = null)
	{
		Vector3 vector = a;
		Vector3 a2 = -a + 3f * (b - c) + d;
		Vector3 b2 = 3f * (a + c) - 6f * b;
		Vector3 b3 = 3f * (b - a);
		if (arrowSize > 0f)
		{
			Vector3 position = arrowTransform.position;
			Quaternion rotation = arrowTransform.rotation;
			float num = 0f;
			for (float num2 = 1f; num2 <= 120f; num2 += 1f)
			{
				float num3 = num2 / 120f;
				Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
				Gizmos.DrawLine(vector, vector2);
				num += (vector2 - vector).magnitude;
				if (num > 1f)
				{
					num -= 1f;
					arrowTransform.position = vector2;
					arrowTransform.LookAt(vector, Vector3.forward);
					Vector3 a3 = arrowTransform.TransformDirection(Vector3.right);
					Vector3 normalized = (vector - vector2).normalized;
					Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
					a3 = arrowTransform.TransformDirection(-Vector3.right);
					Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
				}
				vector = vector2;
			}
			arrowTransform.position = position;
			arrowTransform.rotation = rotation;
			return;
		}
		for (float num4 = 1f; num4 <= 30f; num4 += 1f)
		{
			float num3 = num4 / 30f;
			Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000A70A File Offset: 0x0000890A
	public static object logError(string error)
	{
		if (LeanTween.throwErrors)
		{
			Debug.LogError(error);
		}
		else
		{
			Debug.Log(error);
		}
		return null;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000A722 File Offset: 0x00008922
	public static LTDescr options(LTDescr seed)
	{
		Debug.LogError("error this function is no longer used");
		return null;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000A730 File Offset: 0x00008930
	public static LTDescr options()
	{
		LeanTween.init();
		bool flag = false;
		LeanTween.j = 0;
		LeanTween.i = LeanTween.startSearch;
		while (LeanTween.j <= LeanTween.maxTweens)
		{
			if (LeanTween.j >= LeanTween.maxTweens)
			{
				return LeanTween.logError("LeanTween - You have run out of available spaces for tweening. To avoid this error increase the number of spaces to available for tweening when you initialize the LeanTween class ex: LeanTween.init( " + (LeanTween.maxTweens * 2).ToString() + " );") as LTDescr;
			}
			if (LeanTween.i >= LeanTween.maxTweens)
			{
				LeanTween.i = 0;
			}
			if (!LeanTween.tweens[LeanTween.i].toggle)
			{
				if (LeanTween.i + 1 > LeanTween.tweenMaxSearch && LeanTween.i + 1 < LeanTween.maxTweens)
				{
					LeanTween.tweenMaxSearch = LeanTween.i + 1;
				}
				LeanTween.startSearch = LeanTween.i + 1;
				flag = true;
				break;
			}
			LeanTween.j++;
			LeanTween.i++;
		}
		if (!flag)
		{
			LeanTween.logError("no available tween found!");
		}
		LeanTween.tweens[LeanTween.i].reset();
		LeanTween.global_counter += 1U;
		if (LeanTween.global_counter > 32768U)
		{
			LeanTween.global_counter = 0U;
		}
		LeanTween.tweens[LeanTween.i].setId((uint)LeanTween.i, LeanTween.global_counter);
		return LeanTween.tweens[LeanTween.i];
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000145 RID: 325 RVA: 0x0000A873 File Offset: 0x00008A73
	public static GameObject tweenEmpty
	{
		get
		{
			LeanTween.init(LeanTween.maxTweens);
			return LeanTween._tweenEmpty;
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000A884 File Offset: 0x00008A84
	private static LTDescr pushNewTween(GameObject gameObject, Vector3 to, float time, LTDescr tween)
	{
		LeanTween.init(LeanTween.maxTweens);
		if (gameObject == null || tween == null)
		{
			return null;
		}
		tween.trans = gameObject.transform;
		tween.to = to;
		tween.time = time;
		if (tween.time <= 0f)
		{
			tween.updateInternal();
		}
		return tween;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000A8D8 File Offset: 0x00008AD8
	public static LTDescr play(RectTransform rectTransform, Sprite[] sprites)
	{
		float time = 0.25f * (float)sprites.Length;
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3((float)sprites.Length - 1f, 0f, 0f), time, LeanTween.options().setCanvasPlaySprite().setSprites(sprites).setRepeat(-1));
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000A92C File Offset: 0x00008B2C
	public static LTSeq sequence(bool initSequence = true)
	{
		LeanTween.init(LeanTween.maxTweens);
		for (int i = 0; i < LeanTween.sequences.Length; i++)
		{
			if ((LeanTween.sequences[i].tween == null || !LeanTween.sequences[i].tween.toggle) && !LeanTween.sequences[i].toggle)
			{
				LTSeq ltseq = LeanTween.sequences[i];
				if (initSequence)
				{
					ltseq.init((uint)(i + LeanTween.tweens.Length), LeanTween.global_counter);
					LeanTween.global_counter += 1U;
					if (LeanTween.global_counter > 32768U)
					{
						LeanTween.global_counter = 0U;
					}
				}
				else
				{
					ltseq.reset();
				}
				return ltseq;
			}
		}
		return null;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000A9D0 File Offset: 0x00008BD0
	public static LTDescr alpha(GameObject gameObject, float to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setAlpha());
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000AA0C File Offset: 0x00008C0C
	public static LTDescr alpha(LTRect ltRect, float to, float time)
	{
		ltRect.alphaEnabled = true;
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setGUIAlpha().setRect(ltRect));
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000AA40 File Offset: 0x00008C40
	public static LTDescr textAlpha(RectTransform rectTransform, float to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setTextAlpha());
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000AA40 File Offset: 0x00008C40
	public static LTDescr alphaText(RectTransform rectTransform, float to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setTextAlpha());
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000AA68 File Offset: 0x00008C68
	public static LTDescr alphaCanvas(CanvasGroup canvasGroup, float to, float time)
	{
		return LeanTween.pushNewTween(canvasGroup.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasGroupAlpha());
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000AA90 File Offset: 0x00008C90
	public static LTDescr alphaVertex(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setAlphaVertex());
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000AAB4 File Offset: 0x00008CB4
	public static LTDescr color(GameObject gameObject, Color to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setColor().setPoint(new Vector3(to.r, to.g, to.b)));
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000AB14 File Offset: 0x00008D14
	public static LTDescr textColor(RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setTextColor().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000AB68 File Offset: 0x00008D68
	public static LTDescr colorText(RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setTextColor().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000ABBC File Offset: 0x00008DBC
	public static LTDescr delayedCall(float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000ABDE File Offset: 0x00008DDE
	public static LTDescr delayedCall(float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000AC00 File Offset: 0x00008E00
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000AC1E File Offset: 0x00008E1E
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000AC3C File Offset: 0x00008E3C
	public static LTDescr destroyAfter(LTRect rect, float delayTime)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setRect(rect).setDestroyOnComplete(true));
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000AC64 File Offset: 0x00008E64
	public static LTDescr move(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setMove());
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000AC78 File Offset: 0x00008E78
	public static LTDescr move(GameObject gameObject, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, gameObject.transform.position.z), time, LeanTween.options().setMove());
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000ACAC File Offset: 0x00008EAC
	public static LTDescr move(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurved();
		if (LeanTween.d.optional.path == null)
		{
			LeanTween.d.optional.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.optional.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000AD28 File Offset: 0x00008F28
	public static LTDescr move(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurved();
		LeanTween.d.optional.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000AD74 File Offset: 0x00008F74
	public static LTDescr move(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000ADC0 File Offset: 0x00008FC0
	public static LTDescr moveSpline(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000AE14 File Offset: 0x00009014
	public static LTDescr moveSpline(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000AE60 File Offset: 0x00009060
	public static LTDescr moveSplineLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSplineLocal();
		LeanTween.d.optional.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000AEB1 File Offset: 0x000090B1
	public static LTDescr move(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIMove().setRect(ltRect));
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000AED4 File Offset: 0x000090D4
	public static LTDescr moveMargin(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIMoveMargin().setRect(ltRect));
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000AEF7 File Offset: 0x000090F7
	public static LTDescr moveX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveX());
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000AF1A File Offset: 0x0000911A
	public static LTDescr moveY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveY());
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000AF3D File Offset: 0x0000913D
	public static LTDescr moveZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveZ());
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000AF60 File Offset: 0x00009160
	public static LTDescr moveLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setMoveLocal());
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000AF74 File Offset: 0x00009174
	public static LTDescr moveLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurvedLocal();
		if (LeanTween.d.optional.path == null)
		{
			LeanTween.d.optional.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.optional.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000AFED File Offset: 0x000091ED
	public static LTDescr moveLocalX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalX());
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000B010 File Offset: 0x00009210
	public static LTDescr moveLocalY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalY());
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000B033 File Offset: 0x00009233
	public static LTDescr moveLocalZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalZ());
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000B058 File Offset: 0x00009258
	public static LTDescr moveLocal(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurvedLocal();
		LeanTween.d.optional.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000B0A4 File Offset: 0x000092A4
	public static LTDescr moveLocal(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSplineLocal();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000B0F0 File Offset: 0x000092F0
	public static LTDescr move(GameObject gameObject, Transform to, float time)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, time, LeanTween.options().setTo(to).setMoveToTransform());
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000B10E File Offset: 0x0000930E
	public static LTDescr rotate(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setRotate());
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000B122 File Offset: 0x00009322
	public static LTDescr rotate(LTRect ltRect, float to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setGUIRotate().setRect(ltRect));
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000B14F File Offset: 0x0000934F
	public static LTDescr rotateLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setRotateLocal());
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000B163 File Offset: 0x00009363
	public static LTDescr rotateX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateX());
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000B186 File Offset: 0x00009386
	public static LTDescr rotateY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateY());
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000B1A9 File Offset: 0x000093A9
	public static LTDescr rotateZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateZ());
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000B1CC File Offset: 0x000093CC
	public static LTDescr rotateAround(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, LeanTween.options().setAxis(axis).setRotateAround());
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000B1F5 File Offset: 0x000093F5
	public static LTDescr rotateAroundLocal(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, LeanTween.options().setRotateAroundLocal().setAxis(axis));
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000B21E File Offset: 0x0000941E
	public static LTDescr scale(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setScale());
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000B232 File Offset: 0x00009432
	public static LTDescr scale(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIScale().setRect(ltRect));
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000B255 File Offset: 0x00009455
	public static LTDescr scaleX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleX());
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000B278 File Offset: 0x00009478
	public static LTDescr scaleY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleY());
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000B29B File Offset: 0x0000949B
	public static LTDescr scaleZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleZ());
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000B2BE File Offset: 0x000094BE
	public static LTDescr value(GameObject gameObject, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setFrom(new Vector3(from, 0f, 0f)));
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000B2F6 File Offset: 0x000094F6
	public static LTDescr value(float from, float to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setFrom(new Vector3(from, 0f, 0f)));
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000B334 File Offset: 0x00009534
	public static LTDescr value(GameObject gameObject, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, LeanTween.options().setValue3().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)));
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000B39E File Offset: 0x0000959E
	public static LTDescr value(GameObject gameObject, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setValue3().setFrom(from));
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000B3B8 File Offset: 0x000095B8
	public static LTDescr value(GameObject gameObject, Color from, Color to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setFromColor(from).setHasInitialized(false));
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000B424 File Offset: 0x00009624
	public static LTDescr value(GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate));
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000B484 File Offset: 0x00009684
	public static LTDescr value(GameObject gameObject, Action<float, float> callOnUpdateRatio, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdateRatio(callOnUpdateRatio));
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000B4E4 File Offset: 0x000096E4
	public static LTDescr value(GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f)).setHasInitialized(false).setOnUpdateColor(callOnUpdate));
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000B578 File Offset: 0x00009778
	public static LTDescr value(GameObject gameObject, Action<Color, object> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f)).setHasInitialized(false).setOnUpdateColor(callOnUpdate));
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000B60C File Offset: 0x0000980C
	public static LTDescr value(GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, LeanTween.options().setValue3().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)).setOnUpdateVector2(callOnUpdate));
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000B67D File Offset: 0x0000987D
	public static LTDescr value(GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setValue3().setTo(to).setFrom(from).setOnUpdateVector3(callOnUpdate));
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000B6A4 File Offset: 0x000098A4
	public static LTDescr value(GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate, gameObject));
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000B704 File Offset: 0x00009904
	public static LTDescr delayedSound(AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, pos, 0f, LeanTween.options().setDelayedSound().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000B741 File Offset: 0x00009941
	public static LTDescr delayedSound(GameObject gameObject, AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(gameObject, pos, 0f, LeanTween.options().setDelayedSound().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000B77A File Offset: 0x0000997A
	public static LTDescr move(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasMove().setRect(rectTrans));
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000B799 File Offset: 0x00009999
	public static LTDescr moveX(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveX().setRect(rectTrans));
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000B7C7 File Offset: 0x000099C7
	public static LTDescr moveY(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveY().setRect(rectTrans));
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000B7F5 File Offset: 0x000099F5
	public static LTDescr moveZ(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveZ().setRect(rectTrans));
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000B823 File Offset: 0x00009A23
	public static LTDescr rotate(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(Vector3.forward));
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000B85B File Offset: 0x00009A5B
	public static LTDescr rotate(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(Vector3.forward));
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000B884 File Offset: 0x00009A84
	public static LTDescr rotateAround(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000B8B8 File Offset: 0x00009AB8
	public static LTDescr rotateAroundLocal(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAroundLocal().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000B8EC File Offset: 0x00009AEC
	public static LTDescr scale(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasScale().setRect(rectTrans));
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000B90B File Offset: 0x00009B0B
	public static LTDescr size(RectTransform rectTrans, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasSizeDelta().setRect(rectTrans));
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000B92F File Offset: 0x00009B2F
	public static LTDescr alpha(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasAlpha().setRect(rectTrans));
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000B960 File Offset: 0x00009B60
	public static LTDescr color(RectTransform rectTrans, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCanvasColor().setRect(rectTrans).setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000B9BA File Offset: 0x00009BBA
	public static float tweenOnCurve(LTDescr tweenDescr, float ratioPassed)
	{
		return tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.optional.animationCurve.Evaluate(ratioPassed);
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000B9E8 File Offset: 0x00009BE8
	public static Vector3 tweenOnCurveVector(LTDescr tweenDescr, float ratioPassed)
	{
		return new Vector3(tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.optional.animationCurve.Evaluate(ratioPassed), tweenDescr.from.y + tweenDescr.diff.y * tweenDescr.optional.animationCurve.Evaluate(ratioPassed), tweenDescr.from.z + tweenDescr.diff.z * tweenDescr.optional.animationCurve.Evaluate(ratioPassed));
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000BA75 File Offset: 0x00009C75
	public static float easeOutQuadOpt(float start, float diff, float ratioPassed)
	{
		return -diff * ratioPassed * (ratioPassed - 2f) + start;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000BA85 File Offset: 0x00009C85
	public static float easeInQuadOpt(float start, float diff, float ratioPassed)
	{
		return diff * ratioPassed * ratioPassed + start;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000BA90 File Offset: 0x00009C90
	public static float easeInOutQuadOpt(float start, float diff, float ratioPassed)
	{
		ratioPassed /= 0.5f;
		if (ratioPassed < 1f)
		{
			return diff / 2f * ratioPassed * ratioPassed + start;
		}
		ratioPassed -= 1f;
		return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000BAE0 File Offset: 0x00009CE0
	public static Vector3 easeInOutQuadOpt(Vector3 start, Vector3 diff, float ratioPassed)
	{
		ratioPassed /= 0.5f;
		if (ratioPassed < 1f)
		{
			return diff / 2f * ratioPassed * ratioPassed + start;
		}
		ratioPassed -= 1f;
		return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000BB4F File Offset: 0x00009D4F
	public static float linear(float start, float end, float val)
	{
		return Mathf.Lerp(start, end, val);
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000BB5C File Offset: 0x00009D5C
	public static float clerp(float start, float end, float val)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * val;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * val;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * val;
		}
		return result;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000BBC8 File Offset: 0x00009DC8
	public static float spring(float start, float end, float val)
	{
		val = Mathf.Clamp01(val);
		val = (Mathf.Sin(val * 3.1415927f * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
		return start + (end - start) * val;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000BC2C File Offset: 0x00009E2C
	public static float easeInQuad(float start, float end, float val)
	{
		end -= start;
		return end * val * val + start;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000BC3A File Offset: 0x00009E3A
	public static float easeOutQuad(float start, float end, float val)
	{
		end -= start;
		return -end * val * (val - 2f) + start;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000BC50 File Offset: 0x00009E50
	public static float easeInOutQuad(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val + start;
		}
		val -= 1f;
		return -end / 2f * (val * (val - 2f) - 1f) + start;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000BCA4 File Offset: 0x00009EA4
	public static float easeInOutQuadOpt2(float start, float diffBy2, float val, float val2)
	{
		val /= 0.5f;
		if (val < 1f)
		{
			return diffBy2 * val2 + start;
		}
		val -= 1f;
		return -diffBy2 * (val2 - 2f - 1f) + start;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000BCD8 File Offset: 0x00009ED8
	public static float easeInCubic(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val + start;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000BCE8 File Offset: 0x00009EE8
	public static float easeOutCubic(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val + 1f) + start;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000BD08 File Offset: 0x00009F08
	public static float easeInOutCubic(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val + 2f) + start;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000BD59 File Offset: 0x00009F59
	public static float easeInQuart(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val + start;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000BD6B File Offset: 0x00009F6B
	public static float easeOutQuart(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return -end * (val * val * val * val - 1f) + start;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000BD90 File Offset: 0x00009F90
	public static float easeInOutQuart(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val + start;
		}
		val -= 2f;
		return -end / 2f * (val * val * val * val - 2f) + start;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000BDE6 File Offset: 0x00009FE6
	public static float easeInQuint(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val * val + start;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000BDFA File Offset: 0x00009FFA
	public static float easeOutQuint(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val * val * val + 1f) + start;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000BE20 File Offset: 0x0000A020
	public static float easeInOutQuint(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val * val * val + 2f) + start;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000BE79 File Offset: 0x0000A079
	public static float easeInSine(float start, float end, float val)
	{
		end -= start;
		return -end * Mathf.Cos(val / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000BE99 File Offset: 0x0000A099
	public static float easeOutSine(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Sin(val / 1f * 1.5707964f) + start;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000BEB6 File Offset: 0x0000A0B6
	public static float easeInOutSine(float start, float end, float val)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * val / 1f) - 1f) + start;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000BEE0 File Offset: 0x0000A0E0
	public static float easeInExpo(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000BF08 File Offset: 0x0000A108
	public static float easeOutExpo(float start, float end, float val)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000BF34 File Offset: 0x0000A134
	public static float easeInOutExpo(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
		}
		val -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * val) + 2f) + start;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
	public static float easeInCirc(float start, float end, float val)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - val * val) - 1f) + start;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
	public static float easeOutCirc(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - val * val) + start;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
	public static float easeInOutCirc(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}
		val -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000C054 File Offset: 0x0000A254
	public static float easeInBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		return end - LeanTween.easeOutBounce(0f, end, num - val) + start;
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000C080 File Offset: 0x0000A280
	public static float easeOutBounce(float start, float end, float val)
	{
		val /= 1f;
		end -= start;
		if (val < 0.36363637f)
		{
			return end * (7.5625f * val * val) + start;
		}
		if (val < 0.72727275f)
		{
			val -= 0.54545456f;
			return end * (7.5625f * val * val + 0.75f) + start;
		}
		if ((double)val < 0.9090909090909091)
		{
			val -= 0.8181818f;
			return end * (7.5625f * val * val + 0.9375f) + start;
		}
		val -= 0.95454544f;
		return end * (7.5625f * val * val + 0.984375f) + start;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000C11C File Offset: 0x0000A31C
	public static float easeInOutBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		if (val < num / 2f)
		{
			return LeanTween.easeInBounce(0f, end, val * 2f) * 0.5f + start;
		}
		return LeanTween.easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000C180 File Offset: 0x0000A380
	public static float easeInBack(float start, float end, float val, float overshoot = 1f)
	{
		end -= start;
		val /= 1f;
		float num = 1.70158f * overshoot;
		return end * val * val * ((num + 1f) * val - num) + start;
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
	public static float easeOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val = val / 1f - 1f;
		return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000C1FC File Offset: 0x0000A3FC
	public static float easeInOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val /= 0.5f;
		if (val < 1f)
		{
			num *= 1.525f * overshoot;
			return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
		}
		val -= 2f;
		num *= 1.525f * overshoot;
		return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000C280 File Offset: 0x0000A480
	public static float easeInElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val > 0.6f)
		{
			overshoot = 1f + (1f - val) / 0.4f * (overshoot - 1f);
		}
		val -= 1f;
		return start - num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000C344 File Offset: 0x0000A544
	public static float easeOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val < 0.4f)
		{
			overshoot = 1f + val / 0.4f * (overshoot - 1f);
		}
		return start + end + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000C3FC File Offset: 0x0000A5FC
	public static float easeInOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		val /= 0.5f;
		if (val == 2f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f)
		{
			if (val < 0.2f)
			{
				overshoot = 1f + val / 0.2f * (overshoot - 1f);
			}
			else if (val > 0.8f)
			{
				overshoot = 1f + (1f - val) / 0.2f * (overshoot - 1f);
			}
		}
		if (val < 1f)
		{
			val -= 1f;
			return start - 0.5f * (num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period)) * overshoot;
		}
		val -= 1f;
		return end + start + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * 0.5f * overshoot;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000C534 File Offset: 0x0000A734
	public static LTDescr followDamp(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.diff = d.trans.position;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.damp(d.optional.axis, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.position = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.damp(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.damp(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.damp(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.damp(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.damp(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.damp(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.damp(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.damp(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.damp(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000C6FC File Offset: 0x0000A8FC
	public static LTDescr followSpring(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.diff = d.trans.position;
			d.easeInternal = delegate()
			{
				d.diff = LeanSmooth.spring(d.diff, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.position = d.diff;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.spring(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.spring(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.spring(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.spring(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.spring(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.spring(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.spring(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.spring(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.spring(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
	public static LTDescr followBounceOut(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.bounceOut(d.optional.axis, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.position = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.bounceOut(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.bounceOut(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.bounceOut(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.bounceOut(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.bounceOut(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.bounceOut(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.bounceOut(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.bounceOut(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.bounceOut(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000CA98 File Offset: 0x0000AC98
	public static LTDescr followLinear(Transform trans, Transform target, LeanProp prop, float moveSpeed)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.easeInternal = delegate()
			{
				d.trans.position = LeanSmooth.linear(d.trans.position, d.toTrans.position, moveSpeed, -1f);
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.linear(d.optional.axis, d.toTrans.localPosition, moveSpeed, -1f);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.linear(d.trans.position.x, d.toTrans.position.x, moveSpeed, -1f));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.linear(d.trans.position.y, d.toTrans.position.y, moveSpeed, -1f));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.linear(d.trans.position.z, d.toTrans.position.z, moveSpeed, -1f));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.linear(d.trans.localPosition.x, d.toTrans.localPosition.x, moveSpeed, -1f));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.linear(d.trans.localPosition.y, d.toTrans.localPosition.y, moveSpeed, -1f));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.linear(d.trans.localPosition.z, d.toTrans.localPosition.z, moveSpeed, -1f));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.linear(d.trans.localScale, d.toTrans.localScale, moveSpeed, -1f);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.linear(d.trans.LeanColor(), d.toTrans.LeanColor(), moveSpeed);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000CC3A File Offset: 0x0000AE3A
	public static void addListener(int eventId, Action<LTEvent> callback)
	{
		LeanTween.addListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000CC48 File Offset: 0x0000AE48
	public static void addListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		if (LeanTween.eventListeners == null)
		{
			LeanTween.INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
			LeanTween.eventListeners = new Action<LTEvent>[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
			LeanTween.goListeners = new GameObject[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
		}
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.INIT_LISTENERS_MAX)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == null || LeanTween.eventListeners[num] == null)
			{
				LeanTween.eventListeners[num] = callback;
				LeanTween.goListeners[num] = caller;
				if (LeanTween.i >= LeanTween.eventsMaxSearch)
				{
					LeanTween.eventsMaxSearch = LeanTween.i + 1;
				}
				return;
			}
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				return;
			}
			LeanTween.i++;
		}
		Debug.LogError("You ran out of areas to add listeners, consider increasing LISTENERS_MAX, ex: LeanTween.LISTENERS_MAX = " + (LeanTween.LISTENERS_MAX * 2).ToString());
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000CD3F File Offset: 0x0000AF3F
	public static bool removeListener(int eventId, Action<LTEvent> callback)
	{
		return LeanTween.removeListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000CD50 File Offset: 0x0000AF50
	public static bool removeListener(int eventId)
	{
		int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
		LeanTween.eventListeners[num] = null;
		LeanTween.goListeners[num] = null;
		return true;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000CD7C File Offset: 0x0000AF7C
	public static bool removeListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.eventsMaxSearch)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				LeanTween.eventListeners[num] = null;
				LeanTween.goListeners[num] = null;
				return true;
			}
			LeanTween.i++;
		}
		return false;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
	public static void dispatchEvent(int eventId)
	{
		LeanTween.dispatchEvent(eventId, null);
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
	public static void dispatchEvent(int eventId, object data)
	{
		for (int i = 0; i < LeanTween.eventsMaxSearch; i++)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + i;
			if (LeanTween.eventListeners[num] != null)
			{
				if (LeanTween.goListeners[num])
				{
					LeanTween.eventListeners[num](new LTEvent(eventId, data));
				}
				else
				{
					LeanTween.eventListeners[num] = null;
				}
			}
		}
	}

	// Token: 0x04000157 RID: 343
	public static bool throwErrors = true;

	// Token: 0x04000158 RID: 344
	public static float tau = 6.2831855f;

	// Token: 0x04000159 RID: 345
	public static float PI_DIV2 = 1.5707964f;

	// Token: 0x0400015A RID: 346
	private static LTSeq[] sequences;

	// Token: 0x0400015B RID: 347
	private static LTDescr[] tweens;

	// Token: 0x0400015C RID: 348
	private static int[] tweensFinished;

	// Token: 0x0400015D RID: 349
	private static int[] tweensFinishedIds;

	// Token: 0x0400015E RID: 350
	private static LTDescr tween;

	// Token: 0x0400015F RID: 351
	private static int tweenMaxSearch = -1;

	// Token: 0x04000160 RID: 352
	private static int maxTweens = 400;

	// Token: 0x04000161 RID: 353
	private static int maxSequences = 400;

	// Token: 0x04000162 RID: 354
	private static int frameRendered = -1;

	// Token: 0x04000163 RID: 355
	private static GameObject _tweenEmpty;

	// Token: 0x04000164 RID: 356
	public static float dtEstimated = -1f;

	// Token: 0x04000165 RID: 357
	public static float dtManual;

	// Token: 0x04000166 RID: 358
	public static float dtActual;

	// Token: 0x04000167 RID: 359
	private static uint global_counter = 0U;

	// Token: 0x04000168 RID: 360
	private static int i;

	// Token: 0x04000169 RID: 361
	private static int j;

	// Token: 0x0400016A RID: 362
	private static int finishedCnt;

	// Token: 0x0400016B RID: 363
	public static AnimationCurve punch = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.112586f, 0.9976035f),
		new Keyframe(0.3120486f, -0.1720615f),
		new Keyframe(0.4316337f, 0.07030682f),
		new Keyframe(0.5524869f, -0.03141804f),
		new Keyframe(0.6549395f, 0.003909959f),
		new Keyframe(0.770987f, -0.009817753f),
		new Keyframe(0.8838775f, 0.001939224f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x0400016C RID: 364
	public static AnimationCurve shake = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f),
		new Keyframe(0.75f, -1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x0400016D RID: 365
	private static int maxTweenReached;

	// Token: 0x0400016E RID: 366
	public static int startSearch = 0;

	// Token: 0x0400016F RID: 367
	public static LTDescr d;

	// Token: 0x04000170 RID: 368
	private static Action<LTEvent>[] eventListeners;

	// Token: 0x04000171 RID: 369
	private static GameObject[] goListeners;

	// Token: 0x04000172 RID: 370
	private static int eventsMaxSearch = 0;

	// Token: 0x04000173 RID: 371
	public static int EVENTS_MAX = 10;

	// Token: 0x04000174 RID: 372
	public static int LISTENERS_MAX = 10;

	// Token: 0x04000175 RID: 373
	private static int INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
}
