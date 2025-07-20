using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class LTSeq
{
	// Token: 0x17000032 RID: 50
	// (get) Token: 0x0600041E RID: 1054 RVA: 0x0001621E File Offset: 0x0001441E
	public int id
	{
		get
		{
			return (int)(this._id | this.counter << 16);
		}
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00016230 File Offset: 0x00014430
	public void reset()
	{
		this.previous = null;
		this.tween = null;
		this.totalDelay = 0f;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0001624B File Offset: 0x0001444B
	public void init(uint id, uint global_counter)
	{
		this.reset();
		this._id = id;
		this.counter = global_counter;
		this.current = this;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00016268 File Offset: 0x00014468
	private LTSeq addOn()
	{
		this.current.toggle = true;
		LTSeq ltseq = this.current;
		this.current = LeanTween.sequence(true);
		this.current.previous = ltseq;
		ltseq.toggle = false;
		this.current.totalDelay = ltseq.totalDelay;
		this.current.debugIter = ltseq.debugIter + 1;
		return this.current;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000162D4 File Offset: 0x000144D4
	private float addPreviousDelays()
	{
		LTSeq ltseq = this.current.previous;
		if (ltseq != null && ltseq.tween != null)
		{
			return this.current.totalDelay + ltseq.tween.time;
		}
		return this.current.totalDelay;
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0001631B File Offset: 0x0001451B
	public LTSeq append(float delay)
	{
		this.current.totalDelay += delay;
		return this.current;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00016338 File Offset: 0x00014538
	public LTSeq append(Action callback)
	{
		LTDescr ltdescr = LeanTween.delayedCall(0f, callback);
		return this.append(ltdescr);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00016358 File Offset: 0x00014558
	public LTSeq append(Action<object> callback, object obj)
	{
		this.append(LeanTween.delayedCall(0f, callback).setOnCompleteParam(obj));
		return this.addOn();
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x00016378 File Offset: 0x00014578
	public LTSeq append(GameObject gameObject, Action callback)
	{
		this.append(LeanTween.delayedCall(gameObject, 0f, callback));
		return this.addOn();
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00016393 File Offset: 0x00014593
	public LTSeq append(GameObject gameObject, Action<object> callback, object obj)
	{
		this.append(LeanTween.delayedCall(gameObject, 0f, callback).setOnCompleteParam(obj));
		return this.addOn();
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x000163B4 File Offset: 0x000145B4
	public LTSeq append(LTDescr tween)
	{
		this.current.tween = tween;
		this.current.totalDelay = this.addPreviousDelays();
		tween.setDelay(this.current.totalDelay);
		return this.addOn();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000163EB File Offset: 0x000145EB
	public LTSeq insert(LTDescr tween)
	{
		this.current.tween = tween;
		tween.setDelay(this.addPreviousDelays());
		return this.addOn();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001640C File Offset: 0x0001460C
	public LTSeq setScale(float timeScale)
	{
		this.setScaleRecursive(this.current, timeScale, 500);
		return this.addOn();
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00016428 File Offset: 0x00014628
	private void setScaleRecursive(LTSeq seq, float timeScale, int count)
	{
		if (count > 0)
		{
			this.timeScale = timeScale;
			seq.totalDelay *= timeScale;
			if (seq.tween != null)
			{
				if (seq.tween.time != 0f)
				{
					seq.tween.setTime(seq.tween.time * timeScale);
				}
				seq.tween.setDelay(seq.tween.delay * timeScale);
			}
			if (seq.previous != null)
			{
				this.setScaleRecursive(seq.previous, timeScale, count - 1);
			}
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x000164B2 File Offset: 0x000146B2
	public LTSeq reverse()
	{
		return this.addOn();
	}

	// Token: 0x04000211 RID: 529
	public LTSeq previous;

	// Token: 0x04000212 RID: 530
	public LTSeq current;

	// Token: 0x04000213 RID: 531
	public LTDescr tween;

	// Token: 0x04000214 RID: 532
	public float totalDelay;

	// Token: 0x04000215 RID: 533
	public float timeScale;

	// Token: 0x04000216 RID: 534
	private int debugIter;

	// Token: 0x04000217 RID: 535
	public uint counter;

	// Token: 0x04000218 RID: 536
	public bool toggle;

	// Token: 0x04000219 RID: 537
	private uint _id;
}
