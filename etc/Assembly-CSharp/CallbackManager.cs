using System;
using System.Collections.Generic;
using UnityEngine.Events;

// Token: 0x0200005C RID: 92
public class CallbackManager : Singleton<CallbackManager>
{
	// Token: 0x06000479 RID: 1145 RVA: 0x00017AEA File Offset: 0x00015CEA
	public void CreateDetectorQuery(HitDetector hitDetector)
	{
		this._detectorQueries.Enqueue(hitDetector);
		hitDetector.WaitingForQuery = true;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00017B00 File Offset: 0x00015D00
	public void Update()
	{
		for (int i = 0; i < 10; i++)
		{
			if (this._detectorQueries.Count > 0)
			{
				HitDetector hitDetector = this._detectorQueries.Dequeue();
				if (!hitDetector.Parent.Entity.Has_EFlag_IsDead)
				{
					hitDetector.WaitingForQuery = false;
					hitDetector.HardCheckUnits(true);
				}
			}
		}
		for (int j = 0; j < 10; j++)
		{
			if (this._callbacks.Count > 0)
			{
				this._callbacks.Dequeue()();
			}
		}
	}

	// Token: 0x0400025E RID: 606
	public const int TOTAL_QUERIES_EACH_FRAME = 10;

	// Token: 0x0400025F RID: 607
	public const int TOTAL_CALLBACKS_EACH_FRAME = 10;

	// Token: 0x04000260 RID: 608
	protected Queue<HitDetector> _detectorQueries = new Queue<HitDetector>();

	// Token: 0x04000261 RID: 609
	protected Queue<UnityAction> _callbacks = new Queue<UnityAction>();
}
