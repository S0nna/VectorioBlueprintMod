using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000204 RID: 516
[DefaultExecutionOrder(10)]
public class UIScaler : MonoBehaviour
{
	// Token: 0x06000F8E RID: 3982 RVA: 0x00049A42 File Offset: 0x00047C42
	public void Start()
	{
		this._rectTransform = base.GetComponent<RectTransform>();
		this._defaultScale = this._rectTransform.localScale;
		Singleton<Events>.Instance.onInterfaceScaleChanged.AddListener(new UnityAction<float>(this.Scale));
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00049A81 File Offset: 0x00047C81
	public void Scale(float value)
	{
		this._rectTransform.localScale = this._defaultScale * value;
	}

	// Token: 0x04000D46 RID: 3398
	private RectTransform _rectTransform;

	// Token: 0x04000D47 RID: 3399
	private Vector2 _defaultScale;
}
