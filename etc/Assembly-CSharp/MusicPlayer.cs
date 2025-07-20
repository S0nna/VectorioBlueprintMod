using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001D7 RID: 471
public class MusicPlayer : Singleton<MusicPlayer>
{
	// Token: 0x06000EA4 RID: 3748 RVA: 0x0004210E File Offset: 0x0004030E
	public override void Awake()
	{
		base.Awake();
		this._audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x00042122 File Offset: 0x00040322
	public void Start()
	{
		this._volume = Singleton<Settings>.Instance.GetMusicVolume();
		Singleton<Events>.Instance.onMusicVolumeChanged.AddListener(new UnityAction(this.UpdateVolume));
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x00042150 File Offset: 0x00040350
	public void Update()
	{
		if (this._audioSource.isPlaying)
		{
			if (this._isFadingIn)
			{
				this._audioSource.volume += Time.deltaTime * this.fadeInSpeed;
				if (this._audioSource.volume >= this._volume)
				{
					this._audioSource.volume = this._volume;
					this._isFadingIn = false;
				}
			}
			return;
		}
		if (this._waiting)
		{
			this._timer -= Time.deltaTime;
			if (this._timer <= 0f)
			{
				this.PlayNextTrack();
			}
			return;
		}
		this._timer = this.trackChangeDelay;
		this._waiting = true;
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x00042200 File Offset: 0x00040400
	public void ForcePlayTrack(AudioClip track)
	{
		this._audioSource.clip = track;
		this._audioSource.Play();
		this._audioSource.volume = 0f;
		this._isFadingIn = true;
		this._waiting = false;
		this._timer = 0f;
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x00042250 File Offset: 0x00040450
	public void PlayNextTrack()
	{
		if (!this._playNextTrack)
		{
			return;
		}
		this._audioSource.clip = this.tracks[this._trackIndex];
		this._audioSource.Play();
		this._trackIndex++;
		if (this._trackIndex >= this.tracks.Count)
		{
			this._trackIndex = 0;
		}
		this._audioSource.volume = 0f;
		this._isFadingIn = true;
		this._waiting = false;
		this._timer = 0f;
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x000422DE File Offset: 0x000404DE
	public void Play()
	{
		this._playNextTrack = true;
		this._audioSource.Play();
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x000422F2 File Offset: 0x000404F2
	public void Pause()
	{
		this._playNextTrack = false;
		this._audioSource.Pause();
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00042306 File Offset: 0x00040506
	private void UpdateVolume()
	{
		this._volume = Singleton<Settings>.Instance.GetMusicVolume();
		this._audioSource.volume = this._volume;
	}

	// Token: 0x04000BA7 RID: 2983
	public List<AudioClip> tracks;

	// Token: 0x04000BA8 RID: 2984
	public float fadeInSpeed = 0.01f;

	// Token: 0x04000BA9 RID: 2985
	public float trackChangeDelay = 5f;

	// Token: 0x04000BAA RID: 2986
	public AudioClip decryptionTrack;

	// Token: 0x04000BAB RID: 2987
	private float _timer;

	// Token: 0x04000BAC RID: 2988
	private bool _waiting;

	// Token: 0x04000BAD RID: 2989
	private AudioSource _audioSource;

	// Token: 0x04000BAE RID: 2990
	private bool _playNextTrack = true;

	// Token: 0x04000BAF RID: 2991
	private bool _isFadingIn;

	// Token: 0x04000BB0 RID: 2992
	private float _volume;

	// Token: 0x04000BB1 RID: 2993
	private int _trackIndex;
}
