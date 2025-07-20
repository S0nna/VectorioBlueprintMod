using System;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class Indicator : MonoBehaviour
{
	// Token: 0x06000FD0 RID: 4048 RVA: 0x0004A7D4 File Offset: 0x000489D4
	public void Setup(Vector2 localPosition, string layer)
	{
		base.transform.localPosition = localPosition;
		this._boosted.sortingLayerName = layer;
		this._active.sortingLayerName = layer;
		this._waiting.sortingLayerName = layer;
		this._inactive.sortingLayerName = layer;
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x0004A824 File Offset: 0x00048A24
	public void SetStatus(Indicator.Status status)
	{
		switch (status)
		{
		case Indicator.Status.Boosted:
			this._boosted.gameObject.SetActive(true);
			this._active.gameObject.SetActive(false);
			this._waiting.gameObject.SetActive(false);
			this._inactive.gameObject.SetActive(false);
			return;
		case Indicator.Status.Active:
			this._boosted.gameObject.SetActive(false);
			this._active.gameObject.SetActive(true);
			this._waiting.gameObject.SetActive(false);
			this._inactive.gameObject.SetActive(false);
			return;
		case Indicator.Status.Waiting:
			this._boosted.gameObject.SetActive(false);
			this._active.gameObject.SetActive(false);
			this._waiting.gameObject.SetActive(true);
			this._inactive.gameObject.SetActive(false);
			return;
		case Indicator.Status.Inactive:
			this._boosted.gameObject.SetActive(false);
			this._active.gameObject.SetActive(false);
			this._waiting.gameObject.SetActive(false);
			this._inactive.gameObject.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x04000DDC RID: 3548
	[SerializeField]
	private SpriteRenderer _boosted;

	// Token: 0x04000DDD RID: 3549
	[SerializeField]
	private SpriteRenderer _active;

	// Token: 0x04000DDE RID: 3550
	[SerializeField]
	private SpriteRenderer _waiting;

	// Token: 0x04000DDF RID: 3551
	[SerializeField]
	private SpriteRenderer _inactive;

	// Token: 0x02000217 RID: 535
	public enum Status
	{
		// Token: 0x04000DE1 RID: 3553
		Boosted,
		// Token: 0x04000DE2 RID: 3554
		Active,
		// Token: 0x04000DE3 RID: 3555
		Waiting,
		// Token: 0x04000DE4 RID: 3556
		Inactive
	}
}
