using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000157 RID: 343
public class PowerOutageUI : MonoBehaviour
{
	// Token: 0x06000B35 RID: 2869 RVA: 0x000309FB File Offset: 0x0002EBFB
	private void Start()
	{
		Singleton<Events>.Instance.onPowerExceeded.AddListener(new UnityAction(this.Enable));
		Singleton<Events>.Instance.onPowerRecovered.AddListener(new UnityAction(this.Disable));
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x00030A34 File Offset: 0x0002EC34
	public void Enable()
	{
		base.gameObject.transform.localScale = new Vector2(0.5f, 0.5f);
		LeanTween.alphaCanvas(base.gameObject.GetComponent<CanvasGroup>(), 1f, 0.2f);
		LeanTween.scale(base.gameObject, new Vector2(0.6f, 0.6f), 0.2f).setEase(LeanTweenType.easeOutExpo);
		if (this.powerOutageSound != null && !Singleton<EntityManager>.Instance.IsClearingEntities())
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.powerOutageSound);
		}
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x00030AD8 File Offset: 0x0002ECD8
	public void Disable()
	{
		LeanTween.alphaCanvas(base.gameObject.GetComponent<CanvasGroup>(), 0f, 0.2f);
		LeanTween.scale(base.gameObject, new Vector2(0.5f, 0.5f), 0.2f).setEase(LeanTweenType.easeOutExpo);
		if (this.powerRegainedSound != null && !Singleton<EntityManager>.Instance.IsClearingEntities())
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.powerRegainedSound);
		}
	}

	// Token: 0x040007A5 RID: 1957
	public AudioClip powerOutageSound;

	// Token: 0x040007A6 RID: 1958
	public AudioClip powerRegainedSound;
}
