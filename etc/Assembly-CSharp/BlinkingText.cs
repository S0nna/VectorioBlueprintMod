using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class BlinkingText : MonoBehaviour
{
	// Token: 0x06000AC0 RID: 2752 RVA: 0x0002D43E File Offset: 0x0002B63E
	private void Start()
	{
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		if (this.canvasGroup == null)
		{
			this.canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
		}
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0002D46C File Offset: 0x0002B66C
	private void Update()
	{
		if (this._increaseAlpha)
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha + this.adjustmentSpeed * Time.deltaTime;
			if (this.canvasGroup.alpha >= 1f)
			{
				this._increaseAlpha = false;
				return;
			}
		}
		else
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha - this.adjustmentSpeed * Time.deltaTime;
			if (this.canvasGroup.alpha <= 0f)
			{
				this._increaseAlpha = true;
			}
		}
	}

	// Token: 0x040006DC RID: 1756
	public float adjustmentSpeed = 0.5f;

	// Token: 0x040006DD RID: 1757
	public CanvasGroup canvasGroup;

	// Token: 0x040006DE RID: 1758
	protected bool _increaseAlpha;
}
