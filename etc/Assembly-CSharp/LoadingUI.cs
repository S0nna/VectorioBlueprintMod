using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C5 RID: 453
public class LoadingUI : MonoBehaviour
{
	// Token: 0x06000E55 RID: 3669 RVA: 0x000405EF File Offset: 0x0003E7EF
	public void Start()
	{
		Singleton<Events>.Instance.onFinsihLoading.AddListener(new UnityAction(this.Hide));
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0004060C File Offset: 0x0003E80C
	public void Hide()
	{
		base.gameObject.SetActive(false);
		this.loadingIcon.SetActive(false);
	}

	// Token: 0x04000B34 RID: 2868
	public GameObject loadingIcon;
}
