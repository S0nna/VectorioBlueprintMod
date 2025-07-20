using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001AC RID: 428
public class DecryptionInterface : MonoBehaviour
{
	// Token: 0x06000DE1 RID: 3553 RVA: 0x0003D8C8 File Offset: 0x0003BAC8
	public void Start()
	{
		Singleton<Events>.Instance.onDecryptionStarted.AddListener(new UnityAction<Decryptor>(this.OnDecryptionStarted));
		Singleton<Events>.Instance.onDecryptionFailed.AddListener(new UnityAction<Decryptor>(this.OnDecryptionFailed));
		Singleton<Events>.Instance.onDecryptionFinished.AddListener(new UnityAction<Decryptor>(this.OnDecryptionPassed));
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x0003D928 File Offset: 0x0003BB28
	public void Update()
	{
		if (this._isActive)
		{
			this._timePassed += Time.deltaTime * this.animationSpeed;
			switch (this.stage)
			{
			case DecryptionInterface.Stage.Decrypting:
				this.title.color = Color.Lerp(this.titleColorOne, this.titleColorTwo, Mathf.PingPong(this._timePassed, 1f));
				this.timeLeft.text = ((int)this._decryptor.DecryptionTimer).ToString() + " seconds remaining";
				return;
			case DecryptionInterface.Stage.Failed:
			{
				this._countdown -= Time.deltaTime;
				float t = Mathf.PingPong(this._timePassed, 1f);
				this.title.color = Color.Lerp(this.titleColorOne, this.titleColorTwo, t);
				this.SetBackgroundColor(Color.Lerp(this.redLight, this.redFlash, t));
				if (this._countdown <= 0f)
				{
					this.DisableDecryption();
					return;
				}
				if (!this._hasDisabled && this._countdown <= 1f)
				{
					foreach (MenuButton menuButton in this.animatedComponents)
					{
						LeanTween.moveLocal(menuButton.group.gameObject, menuButton.outPos, 1f).setEase(LeanTweenType.easeOutExpo);
					}
					this._hasDisabled = true;
				}
				break;
			}
			case DecryptionInterface.Stage.Passed:
			{
				this._countdown -= Time.deltaTime;
				float t2 = Mathf.PingPong(this._timePassed, 1f);
				this.title.color = Color.Lerp(this.titleColorThree, this.titleColorFour, t2);
				this.SetBackgroundColor(Color.Lerp(this.greenLight, this.greenFlash, t2));
				if (this._countdown <= 0f)
				{
					this.DisableDecryption();
					return;
				}
				if (!this._hasDisabled && this._countdown <= 1f)
				{
					foreach (MenuButton menuButton2 in this.animatedComponents)
					{
						LeanTween.moveLocal(menuButton2.group.gameObject, menuButton2.outPos, 1f).setEase(LeanTweenType.easeOutExpo);
					}
					this._hasDisabled = true;
					return;
				}
				break;
			}
			default:
				return;
			}
		}
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x0003DBB4 File Offset: 0x0003BDB4
	public void OnDecryptionStarted(Decryptor decryptor)
	{
		Singleton<Interface>.Instance.ToggleEncryption(true);
		Singleton<Interface>.Instance.SetCanOpenFlag(false);
		if (LegacyLibrary.DECRYPTION_START_SOUND != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(LegacyLibrary.DECRYPTION_START_SOUND);
		}
		this.title.text = "DECRYPTING";
		this.titleForeground.text = "DECRYPTING";
		this.stage = DecryptionInterface.Stage.Decrypting;
		this._decryptor = decryptor;
		this.SetBackgroundColor(this.redLight);
		this.titleForeground.color = this.redLight;
		this.encryptionIcon.color = this.redDark;
		foreach (MenuButton menuButton in this.animatedComponents)
		{
			menuButton.group.GetComponent<RectTransform>().localPosition = menuButton.inPos;
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, 1f).setEase(LeanTweenType.easeOutExpo);
		}
		this._isActive = true;
		this._hasDisabled = false;
		this._timePassed = 0f;
		this._countdown = 4f;
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x0003DCF8 File Offset: 0x0003BEF8
	public void OnDecryptionPassed(Decryptor decryptor)
	{
		Singleton<Interface>.Instance.SetCanOpenFlag(true);
		this.stage = DecryptionInterface.Stage.Passed;
		this.titleForeground.color = this.greenLight;
		this.encryptionIcon.color = this.greenDark;
		this.title.text = "DECRYPTION PASSED";
		this.titleForeground.text = "DECRYPTION PASSED";
		this.timeLeft.text = "Tech now researchable";
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x0003DD6C File Offset: 0x0003BF6C
	public void OnDecryptionFailed(Decryptor decryptor)
	{
		Singleton<Interface>.Instance.SetCanOpenFlag(true);
		this.stage = DecryptionInterface.Stage.Failed;
		this.titleForeground.color = this.redLight;
		this.encryptionIcon.color = this.redDark;
		this.title.text = "DECRYPTION FAILED";
		this.titleForeground.text = "DECRYPTION FAILED";
		this.timeLeft.text = "Process was interupted";
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x0003DDDD File Offset: 0x0003BFDD
	public void DisableDecryption()
	{
		Singleton<Interface>.Instance.ToggleEncryption(false);
		this._isActive = false;
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x0003DDF1 File Offset: 0x0003BFF1
	protected void SetBackgroundColor(Color color)
	{
		this.backgroundOne.color = color;
		this.backgroundTwo.color = color;
		this.backgroundThree.color = color;
		this.backgroundFour.color = color;
		this.panelBackground.color = color;
	}

	// Token: 0x04000A2C RID: 2604
	protected DecryptionInterface.Stage stage;

	// Token: 0x04000A2D RID: 2605
	protected Decryptor _decryptor;

	// Token: 0x04000A2E RID: 2606
	public List<MenuButton> animatedComponents;

	// Token: 0x04000A2F RID: 2607
	public float animationSpeed;

	// Token: 0x04000A30 RID: 2608
	public Image backgroundOne;

	// Token: 0x04000A31 RID: 2609
	public Image backgroundTwo;

	// Token: 0x04000A32 RID: 2610
	public Image backgroundThree;

	// Token: 0x04000A33 RID: 2611
	public Image backgroundFour;

	// Token: 0x04000A34 RID: 2612
	public Image panelBackground;

	// Token: 0x04000A35 RID: 2613
	public Image encryptionIcon;

	// Token: 0x04000A36 RID: 2614
	public TextMeshProUGUI title;

	// Token: 0x04000A37 RID: 2615
	public TextMeshProUGUI titleForeground;

	// Token: 0x04000A38 RID: 2616
	public TextMeshProUGUI timeLeft;

	// Token: 0x04000A39 RID: 2617
	public Color titleColorOne;

	// Token: 0x04000A3A RID: 2618
	public Color titleColorTwo;

	// Token: 0x04000A3B RID: 2619
	public Color titleColorThree;

	// Token: 0x04000A3C RID: 2620
	public Color titleColorFour;

	// Token: 0x04000A3D RID: 2621
	public Color redLight;

	// Token: 0x04000A3E RID: 2622
	public Color redDark;

	// Token: 0x04000A3F RID: 2623
	public Color redFlash;

	// Token: 0x04000A40 RID: 2624
	public Color greenLight;

	// Token: 0x04000A41 RID: 2625
	public Color greenDark;

	// Token: 0x04000A42 RID: 2626
	public Color greenFlash;

	// Token: 0x04000A43 RID: 2627
	protected CanvasGroup _canvasGroup;

	// Token: 0x04000A44 RID: 2628
	protected bool _isActive;

	// Token: 0x04000A45 RID: 2629
	protected bool _hasDisabled;

	// Token: 0x04000A46 RID: 2630
	protected float _timePassed;

	// Token: 0x04000A47 RID: 2631
	protected float _countdown = 4f;

	// Token: 0x020001AD RID: 429
	public enum Stage
	{
		// Token: 0x04000A49 RID: 2633
		Decrypting,
		// Token: 0x04000A4A RID: 2634
		Failed,
		// Token: 0x04000A4B RID: 2635
		Passed
	}
}
