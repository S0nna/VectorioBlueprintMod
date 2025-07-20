using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001A4 RID: 420
public class BossMusic : MonoBehaviour
{
	// Token: 0x06000DC7 RID: 3527 RVA: 0x0003D380 File Offset: 0x0003B580
	public void Start()
	{
		Singleton<Events>.Instance.onGuardianActivated.AddListener(new UnityAction<Guardian>(this.StartBossMusic));
		Singleton<Events>.Instance.onGuardianDeactivated.AddListener(new UnityAction<Guardian>(this.StopBossMusic));
		Singleton<Events>.Instance.onGuardianDestroyed.AddListener(new UnityAction<Guardian>(this.StopBossMusic));
		Singleton<Events>.Instance.onDecryptionStarted.AddListener(new UnityAction<Decryptor>(this.StartDecryptionMusic));
		Singleton<Events>.Instance.onDecryptionFailed.AddListener(new UnityAction<Decryptor>(this.StopDecryptionMusic));
		Singleton<Events>.Instance.onDecryptionFinished.AddListener(new UnityAction<Decryptor>(this.StopDecryptionMusic));
		Singleton<Events>.Instance.onMusicVolumeChanged.AddListener(new UnityAction(this.UpdateVolume));
		this._volume = Singleton<Settings>.Instance.GetMusicVolume();
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x0003D45A File Offset: 0x0003B65A
	public void StartDecryptionMusic(Decryptor decryptor)
	{
		if (!this._isPlaying)
		{
			this.musicPlayer.Pause();
			this.audioSource.volume = this._volume;
			this.audioSource.Play();
			this._isPlaying = true;
		}
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0003D492 File Offset: 0x0003B692
	public void StopDecryptionMusic(Decryptor decryptor)
	{
		if (this._isPlaying)
		{
			this.audioSource.Stop();
			this.musicPlayer.Play();
			this._isPlaying = false;
		}
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00003212 File Offset: 0x00001412
	public void StartBossMusic(Guardian guardian)
	{
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x0003D4B9 File Offset: 0x0003B6B9
	public void StopBossMusic(Guardian guardian)
	{
		if (this._isPlaying)
		{
			this.audioSource.Stop();
			this.musicPlayer.Play();
		}
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0003D4D9 File Offset: 0x0003B6D9
	public void UpdateVolume()
	{
		this._volume = Singleton<Settings>.Instance.GetMusicVolume();
		this.audioSource.volume = this._volume;
	}

	// Token: 0x04000A0B RID: 2571
	protected bool _isPlaying;

	// Token: 0x04000A0C RID: 2572
	public AudioSource audioSource;

	// Token: 0x04000A0D RID: 2573
	public MusicPlayer musicPlayer;

	// Token: 0x04000A0E RID: 2574
	public AudioClip guardianSound;

	// Token: 0x04000A0F RID: 2575
	protected float _volume = 1f;
}
