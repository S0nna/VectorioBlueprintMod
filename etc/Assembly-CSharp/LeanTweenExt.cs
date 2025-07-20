using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public static class LeanTweenExt
{
	// Token: 0x0600024B RID: 587 RVA: 0x00010010 File Offset: 0x0000E210
	public static LTDescr LeanAlpha(this GameObject gameObject, float to, float time)
	{
		return LeanTween.alpha(gameObject, to, time);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0001001A File Offset: 0x0000E21A
	public static LTDescr LeanAlphaVertex(this GameObject gameObject, float to, float time)
	{
		return LeanTween.alphaVertex(gameObject, to, time);
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00010024 File Offset: 0x0000E224
	public static LTDescr LeanAlpha(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.alpha(rectTransform, to, time);
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0001002E File Offset: 0x0000E22E
	public static LTDescr LeanAlpha(this CanvasGroup canvas, float to, float time)
	{
		return LeanTween.alphaCanvas(canvas, to, time);
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00010038 File Offset: 0x0000E238
	public static LTDescr LeanAlphaText(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.alphaText(rectTransform, to, time);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00010042 File Offset: 0x0000E242
	public static void LeanCancel(this GameObject gameObject)
	{
		LeanTween.cancel(gameObject);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0001004A File Offset: 0x0000E24A
	public static void LeanCancel(this GameObject gameObject, bool callOnComplete)
	{
		LeanTween.cancel(gameObject, callOnComplete);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00010053 File Offset: 0x0000E253
	public static void LeanCancel(this GameObject gameObject, int uniqueId, bool callOnComplete = false)
	{
		LeanTween.cancel(gameObject, uniqueId, callOnComplete);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0001005D File Offset: 0x0000E25D
	public static void LeanCancel(this RectTransform rectTransform)
	{
		LeanTween.cancel(rectTransform);
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00010065 File Offset: 0x0000E265
	public static LTDescr LeanColor(this GameObject gameObject, Color to, float time)
	{
		return LeanTween.color(gameObject, to, time);
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0001006F File Offset: 0x0000E26F
	public static LTDescr LeanColorText(this RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.colorText(rectTransform, to, time);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x00010079 File Offset: 0x0000E279
	public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, Action callback)
	{
		return LeanTween.delayedCall(gameObject, delayTime, callback);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00010083 File Offset: 0x0000E283
	public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, Action<object> callback)
	{
		return LeanTween.delayedCall(gameObject, delayTime, callback);
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0001008D File Offset: 0x0000E28D
	public static bool LeanIsPaused(this GameObject gameObject)
	{
		return LeanTween.isPaused(gameObject);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00010095 File Offset: 0x0000E295
	public static bool LeanIsPaused(this RectTransform rectTransform)
	{
		return LeanTween.isPaused(rectTransform);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0001009D File Offset: 0x0000E29D
	public static bool LeanIsTweening(this GameObject gameObject)
	{
		return LeanTween.isTweening(gameObject);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x000100A5 File Offset: 0x0000E2A5
	public static LTDescr LeanMove(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x0600025C RID: 604 RVA: 0x000100AF File Offset: 0x0000E2AF
	public static LTDescr LeanMove(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x000100BE File Offset: 0x0000E2BE
	public static LTDescr LeanMove(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.move(rectTransform, to, time);
	}

	// Token: 0x0600025E RID: 606 RVA: 0x000100C8 File Offset: 0x0000E2C8
	public static LTDescr LeanMove(this GameObject gameObject, Vector2 to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x000100D2 File Offset: 0x0000E2D2
	public static LTDescr LeanMove(this Transform transform, Vector2 to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x06000260 RID: 608 RVA: 0x000100E1 File Offset: 0x0000E2E1
	public static LTDescr LeanMove(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x06000261 RID: 609 RVA: 0x000100EB File Offset: 0x0000E2EB
	public static LTDescr LeanMove(this GameObject gameObject, LTBezierPath to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x06000262 RID: 610 RVA: 0x000100F5 File Offset: 0x0000E2F5
	public static LTDescr LeanMove(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.move(gameObject, to, time);
	}

	// Token: 0x06000263 RID: 611 RVA: 0x000100FF File Offset: 0x0000E2FF
	public static LTDescr LeanMove(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0001010E File Offset: 0x0000E30E
	public static LTDescr LeanMove(this Transform transform, LTBezierPath to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0001011D File Offset: 0x0000E31D
	public static LTDescr LeanMove(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.move(transform.gameObject, to, time);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0001012C File Offset: 0x0000E32C
	public static LTDescr LeanMoveLocal(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00010136 File Offset: 0x0000E336
	public static LTDescr LeanMoveLocal(this GameObject gameObject, LTBezierPath to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00010140 File Offset: 0x0000E340
	public static LTDescr LeanMoveLocal(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.moveLocal(gameObject, to, time);
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0001014A File Offset: 0x0000E34A
	public static LTDescr LeanMoveLocal(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00010159 File Offset: 0x0000E359
	public static LTDescr LeanMoveLocal(this Transform transform, LTBezierPath to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00010168 File Offset: 0x0000E368
	public static LTDescr LeanMoveLocal(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.moveLocal(transform.gameObject, to, time);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00010177 File Offset: 0x0000E377
	public static LTDescr LeanMoveLocalX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalX(gameObject, to, time);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00010181 File Offset: 0x0000E381
	public static LTDescr LeanMoveLocalY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalY(gameObject, to, time);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0001018B File Offset: 0x0000E38B
	public static LTDescr LeanMoveLocalZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveLocalZ(gameObject, to, time);
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00010195 File Offset: 0x0000E395
	public static LTDescr LeanMoveLocalX(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalX(transform.gameObject, to, time);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x000101A4 File Offset: 0x0000E3A4
	public static LTDescr LeanMoveLocalY(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalY(transform.gameObject, to, time);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x000101B3 File Offset: 0x0000E3B3
	public static LTDescr LeanMoveLocalZ(this Transform transform, float to, float time)
	{
		return LeanTween.moveLocalZ(transform.gameObject, to, time);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x000101C2 File Offset: 0x0000E3C2
	public static LTDescr LeanMoveSpline(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.moveSpline(gameObject, to, time);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x000101CC File Offset: 0x0000E3CC
	public static LTDescr LeanMoveSpline(this GameObject gameObject, LTSpline to, float time)
	{
		return LeanTween.moveSpline(gameObject, to, time);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x000101D6 File Offset: 0x0000E3D6
	public static LTDescr LeanMoveSpline(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.moveSpline(transform.gameObject, to, time);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x000101E5 File Offset: 0x0000E3E5
	public static LTDescr LeanMoveSpline(this Transform transform, LTSpline to, float time)
	{
		return LeanTween.moveSpline(transform.gameObject, to, time);
	}

	// Token: 0x06000276 RID: 630 RVA: 0x000101F4 File Offset: 0x0000E3F4
	public static LTDescr LeanMoveSplineLocal(this GameObject gameObject, Vector3[] to, float time)
	{
		return LeanTween.moveSplineLocal(gameObject, to, time);
	}

	// Token: 0x06000277 RID: 631 RVA: 0x000101FE File Offset: 0x0000E3FE
	public static LTDescr LeanMoveSplineLocal(this Transform transform, Vector3[] to, float time)
	{
		return LeanTween.moveSplineLocal(transform.gameObject, to, time);
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0001020D File Offset: 0x0000E40D
	public static LTDescr LeanMoveX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveX(gameObject, to, time);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00010217 File Offset: 0x0000E417
	public static LTDescr LeanMoveX(this Transform transform, float to, float time)
	{
		return LeanTween.moveX(transform.gameObject, to, time);
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00010226 File Offset: 0x0000E426
	public static LTDescr LeanMoveX(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveX(rectTransform, to, time);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00010230 File Offset: 0x0000E430
	public static LTDescr LeanMoveY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveY(gameObject, to, time);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0001023A File Offset: 0x0000E43A
	public static LTDescr LeanMoveY(this Transform transform, float to, float time)
	{
		return LeanTween.moveY(transform.gameObject, to, time);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00010249 File Offset: 0x0000E449
	public static LTDescr LeanMoveY(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveY(rectTransform, to, time);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00010253 File Offset: 0x0000E453
	public static LTDescr LeanMoveZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.moveZ(gameObject, to, time);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0001025D File Offset: 0x0000E45D
	public static LTDescr LeanMoveZ(this Transform transform, float to, float time)
	{
		return LeanTween.moveZ(transform.gameObject, to, time);
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0001026C File Offset: 0x0000E46C
	public static LTDescr LeanMoveZ(this RectTransform rectTransform, float to, float time)
	{
		return LeanTween.moveZ(rectTransform, to, time);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00010276 File Offset: 0x0000E476
	public static void LeanPause(this GameObject gameObject)
	{
		LeanTween.pause(gameObject);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0001027E File Offset: 0x0000E47E
	public static LTDescr LeanPlay(this RectTransform rectTransform, Sprite[] sprites)
	{
		return LeanTween.play(rectTransform, sprites);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00010287 File Offset: 0x0000E487
	public static void LeanResume(this GameObject gameObject)
	{
		LeanTween.resume(gameObject);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0001028F File Offset: 0x0000E48F
	public static LTDescr LeanRotate(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.rotate(gameObject, to, time);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00010299 File Offset: 0x0000E499
	public static LTDescr LeanRotate(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.rotate(transform.gameObject, to, time);
	}

	// Token: 0x06000286 RID: 646 RVA: 0x000102A8 File Offset: 0x0000E4A8
	public static LTDescr LeanRotate(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.rotate(rectTransform, to, time);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x000102B2 File Offset: 0x0000E4B2
	public static LTDescr LeanRotateAround(this GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(gameObject, axis, add, time);
	}

	// Token: 0x06000288 RID: 648 RVA: 0x000102BD File Offset: 0x0000E4BD
	public static LTDescr LeanRotateAround(this Transform transform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(transform.gameObject, axis, add, time);
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000102CD File Offset: 0x0000E4CD
	public static LTDescr LeanRotateAround(this RectTransform rectTransform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAround(rectTransform, axis, add, time);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x000102D8 File Offset: 0x0000E4D8
	public static LTDescr LeanRotateAroundLocal(this GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(gameObject, axis, add, time);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x000102E3 File Offset: 0x0000E4E3
	public static LTDescr LeanRotateAroundLocal(this Transform transform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(transform.gameObject, axis, add, time);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x000102F3 File Offset: 0x0000E4F3
	public static LTDescr LeanRotateAroundLocal(this RectTransform rectTransform, Vector3 axis, float add, float time)
	{
		return LeanTween.rotateAroundLocal(rectTransform, axis, add, time);
	}

	// Token: 0x0600028D RID: 653 RVA: 0x000102FE File Offset: 0x0000E4FE
	public static LTDescr LeanRotateX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateX(gameObject, to, time);
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00010308 File Offset: 0x0000E508
	public static LTDescr LeanRotateX(this Transform transform, float to, float time)
	{
		return LeanTween.rotateX(transform.gameObject, to, time);
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00010317 File Offset: 0x0000E517
	public static LTDescr LeanRotateY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateY(gameObject, to, time);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00010321 File Offset: 0x0000E521
	public static LTDescr LeanRotateY(this Transform transform, float to, float time)
	{
		return LeanTween.rotateY(transform.gameObject, to, time);
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00010330 File Offset: 0x0000E530
	public static LTDescr LeanRotateZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.rotateZ(gameObject, to, time);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0001033A File Offset: 0x0000E53A
	public static LTDescr LeanRotateZ(this Transform transform, float to, float time)
	{
		return LeanTween.rotateZ(transform.gameObject, to, time);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00010349 File Offset: 0x0000E549
	public static LTDescr LeanScale(this GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.scale(gameObject, to, time);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00010353 File Offset: 0x0000E553
	public static LTDescr LeanScale(this Transform transform, Vector3 to, float time)
	{
		return LeanTween.scale(transform.gameObject, to, time);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00010362 File Offset: 0x0000E562
	public static LTDescr LeanScale(this RectTransform rectTransform, Vector3 to, float time)
	{
		return LeanTween.scale(rectTransform, to, time);
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0001036C File Offset: 0x0000E56C
	public static LTDescr LeanScaleX(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleX(gameObject, to, time);
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00010376 File Offset: 0x0000E576
	public static LTDescr LeanScaleX(this Transform transform, float to, float time)
	{
		return LeanTween.scaleX(transform.gameObject, to, time);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00010385 File Offset: 0x0000E585
	public static LTDescr LeanScaleY(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleY(gameObject, to, time);
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0001038F File Offset: 0x0000E58F
	public static LTDescr LeanScaleY(this Transform transform, float to, float time)
	{
		return LeanTween.scaleY(transform.gameObject, to, time);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0001039E File Offset: 0x0000E59E
	public static LTDescr LeanScaleZ(this GameObject gameObject, float to, float time)
	{
		return LeanTween.scaleZ(gameObject, to, time);
	}

	// Token: 0x0600029B RID: 667 RVA: 0x000103A8 File Offset: 0x0000E5A8
	public static LTDescr LeanScaleZ(this Transform transform, float to, float time)
	{
		return LeanTween.scaleZ(transform.gameObject, to, time);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x000103B7 File Offset: 0x0000E5B7
	public static LTDescr LeanSize(this RectTransform rectTransform, Vector2 to, float time)
	{
		return LeanTween.size(rectTransform, to, time);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x000103C1 File Offset: 0x0000E5C1
	public static LTDescr LeanValue(this GameObject gameObject, Color from, Color to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x000103CC File Offset: 0x0000E5CC
	public static LTDescr LeanValue(this GameObject gameObject, float from, float to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x0600029F RID: 671 RVA: 0x000103D7 File Offset: 0x0000E5D7
	public static LTDescr LeanValue(this GameObject gameObject, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x000103E2 File Offset: 0x0000E5E2
	public static LTDescr LeanValue(this GameObject gameObject, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.value(gameObject, from, to, time);
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x000103ED File Offset: 0x0000E5ED
	public static LTDescr LeanValue(this GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x000103FA File Offset: 0x0000E5FA
	public static LTDescr LeanValue(this GameObject gameObject, Action<float, float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00010407 File Offset: 0x0000E607
	public static LTDescr LeanValue(this GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00010414 File Offset: 0x0000E614
	public static LTDescr LeanValue(this GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00010421 File Offset: 0x0000E621
	public static LTDescr LeanValue(this GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0001042E File Offset: 0x0000E62E
	public static LTDescr LeanValue(this GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.value(gameObject, callOnUpdate, from, to, time);
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0001043B File Offset: 0x0000E63B
	public static void LeanSetPosX(this Transform transform, float val)
	{
		transform.position = new Vector3(val, transform.position.y, transform.position.z);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0001045F File Offset: 0x0000E65F
	public static void LeanSetPosY(this Transform transform, float val)
	{
		transform.position = new Vector3(transform.position.x, val, transform.position.z);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00010483 File Offset: 0x0000E683
	public static void LeanSetPosZ(this Transform transform, float val)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, val);
	}

	// Token: 0x060002AA RID: 682 RVA: 0x000104A7 File Offset: 0x0000E6A7
	public static void LeanSetLocalPosX(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(val, transform.localPosition.y, transform.localPosition.z);
	}

	// Token: 0x060002AB RID: 683 RVA: 0x000104CB File Offset: 0x0000E6CB
	public static void LeanSetLocalPosY(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, val, transform.localPosition.z);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x000104EF File Offset: 0x0000E6EF
	public static void LeanSetLocalPosZ(this Transform transform, float val)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, val);
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00010513 File Offset: 0x0000E713
	public static Color LeanColor(this Transform transform)
	{
		return transform.GetComponent<Renderer>().material.color;
	}
}
