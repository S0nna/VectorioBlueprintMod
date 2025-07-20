using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Vectorio.PhasmaUI;

// Token: 0x020001F7 RID: 503
public class StartingAnimation : MonoBehaviour
{
	// Token: 0x06000F56 RID: 3926 RVA: 0x000480AC File Offset: 0x000462AC
	public void Start()
	{
		this._consoleAudioSource = new GameObject("Console Audio").AddComponent<AudioSource>();
		this._voiceAudioSource = new GameObject("Voice Audio").AddComponent<AudioSource>();
		this._ambienceAudioSource = new GameObject("Ambience Audio").AddComponent<AudioSource>();
		this._musicAudioSource = new GameObject("Music Audio").AddComponent<AudioSource>();
		this._soundAudioSource = new GameObject("Sound Audio").AddComponent<AudioSource>();
		this._ambienceAudioSource.clip = this.ambienceTrack;
		this._ambienceAudioSource.loop = true;
		this._ambienceAudioSource.Play();
		this._ambienceAudioSource.volume = 0.8f;
		this._musicAudioSource.playOnAwake = false;
		this._musicAudioSource.clip = this.musicTrack;
		this._musicAudioSource.loop = true;
		this._musicAudioSource.volume = 0.8f;
		this._soundAudioSource.volume = 1f;
		this._consoleAudioSource.volume = 0.5f;
		this._consoleAudioSource.loop = false;
		this._voiceAudioSource.loop = false;
		this._soundAudioSource.loop = false;
		this._consoleAudioSource.playOnAwake = false;
		this._voiceAudioSource.playOnAwake = false;
		this._soundAudioSource.playOnAwake = false;
		if (this.cam == null)
		{
			this.cam = Camera.main;
		}
		Singleton<Interface>.Instance.ToggleHUD();
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x00048220 File Offset: 0x00046420
	public void Update()
	{
		this.UpdateSize();
		if (this._initialStart > 0f)
		{
			this._initialStart -= Time.deltaTime;
			return;
		}
		if (this.blackScreen.color.a > 0f)
		{
			this.blackScreen.color = new Color(this.blackScreen.color.r, this.blackScreen.color.g, this.blackScreen.color.b, this.blackScreen.color.a - Time.deltaTime * 0.5f);
		}
		if (this._finished)
		{
			return;
		}
		if (this._changingColor)
		{
			this._colorTimer += Time.deltaTime * 2f;
			this.foreground.color = Color.Lerp(this._previousForegroundColor, this._foregroundColor, this._colorTimer);
			this.background.color = Color.Lerp(this._previousBackgroundColor, this._backgroundColor, this._colorTimer);
			if (this._colorTimer >= 1f)
			{
				this._changingColor = false;
			}
		}
		if (this._playingAudio)
		{
			if (this._delayedVoice)
			{
				this._voiceTimer -= Time.deltaTime;
				if (this._voiceTimer <= 0f)
				{
					this._voiceAudioSource.PlayOneShot(this._voiceClip, this._voiceVolume);
					this._delayedVoice = false;
				}
			}
			else
			{
				if (this._pitchVoiceToZero && this._voiceAudioSource.pitch > 0f)
				{
					this._voiceAudioSource.pitch -= Time.deltaTime * 0.25f;
				}
				this._playingAudio = this._voiceAudioSource.isPlaying;
			}
		}
		if (this._playingSound)
		{
			if (this._delayedSound)
			{
				this._soundTimer -= Time.deltaTime;
				if (this._soundTimer <= 0f)
				{
					this._soundAudioSource.Play();
					this._delayedSound = false;
				}
			}
			else
			{
				this._playingSound = this._soundAudioSource.isPlaying;
			}
		}
		if (this._clearingConsole)
		{
			if (this._textTimer <= 0f)
			{
				if (this.console.text.Length <= 0)
				{
					this._clearingConsole = false;
				}
				else
				{
					this.console.text = this.console.text.Remove(this.console.text.Length - 1, 1);
					this._consoleAudioSource.PlayOneShot(this.newCharSound, 0.3f);
					this._textTimer = this._textCooldown;
				}
			}
			else
			{
				this._textTimer -= Time.deltaTime;
			}
		}
		else if (this._printingToConsole)
		{
			if (this._textTimer <= 0f)
			{
				if (this._chars.Count == 0)
				{
					this._printingToConsole = false;
				}
				else
				{
					char c = this._chars[0];
					this.PrintCharToConsole(c);
					this._chars.RemoveAt(0);
				}
			}
			else
			{
				this._textTimer -= Time.deltaTime;
			}
		}
		if (!this._playingAudio && !this._playingSound && !this._printingToConsole && !this._changingColor && !this._clearingConsole)
		{
			this._timer -= Time.deltaTime;
			if (this._timer <= 0f)
			{
				this.NextStep();
			}
		}
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x00048580 File Offset: 0x00046780
	private void NextStep()
	{
		if (this._index + 1 == this.steps.Count)
		{
			this._finished = true;
			return;
		}
		this._index++;
		StartingAnimation.Step step = this.steps[this._index];
		if (step.changeColor)
		{
			this._changingColor = true;
			this._previousForegroundColor = this.foreground.color;
			this._previousBackgroundColor = this.background.color;
			this._foregroundColor = step.foregroundColor;
			this._backgroundColor = step.backgroundColor;
			this._colorTimer = 0f;
		}
		this._playingAudio = (step.voiceClip != null);
		if (this._playingAudio)
		{
			this._voiceClip = step.voiceClip;
			this._voiceVolume = step.voiceVolume;
			this._voiceAudioSource.pitch = step.voicePitch;
			this._pitchVoiceToZero = step.pitchDownVoiceToZero;
			if (step.voiceDelay <= 0f)
			{
				this._voiceAudioSource.PlayOneShot(this._voiceClip, this._voiceVolume);
			}
			else
			{
				this._voiceTimer = step.voiceDelay;
				this._delayedVoice = true;
			}
		}
		this._playingSound = (step.soundClip != null);
		if (this._playingSound)
		{
			this._soundAudioSource.clip = step.soundClip;
			if (step.soundDelay <= 0f)
			{
				this._soundAudioSource.Play();
			}
			else
			{
				this._soundTimer = step.soundDelay;
				this._delayedSound = true;
			}
		}
		this._printingToConsole = (step.text.Length > 0);
		if (this._printingToConsole)
		{
			this._chars = step.text.ToList<char>();
			this._textCooldown = step.textCooldown;
		}
		if (step.startMusic)
		{
			this._musicAudioSource.Play();
		}
		this._clearingConsole = step.clearConsole;
		this._timer = step.stepCooldown;
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00048764 File Offset: 0x00046964
	private void UpdateSize()
	{
		float num = 2f * this.cam.orthographicSize;
		float num2 = num * this.cam.aspect;
		this.background.transform.localScale = new Vector3(num2, num, 1f);
		this.foreground.transform.localScale = new Vector3(num2, num, 1f);
		this.titleBar.transform.localScale = new Vector3(num2, this.titleBar.transform.localScale.y, 1f);
		this.console.GetComponent<RectTransform>().localPosition = new Vector3(-(num2 / 2f), num / 2f, 0f);
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00048824 File Offset: 0x00046A24
	private void PrintCharToConsole(char c)
	{
		if (c == '+')
		{
			this.console.text = this.console.text + "<br>";
			this._consoleAudioSource.PlayOneShot(this.newLineSound, 0.5f);
			this._textTimer = 1f;
			return;
		}
		if (c == '-')
		{
			this.console.text = this.console.text + "<br>";
			this._textTimer = this._textCooldown;
			return;
		}
		if (c == '.')
		{
			this.console.text = this.console.text + c.ToString();
			this._textTimer = 1f;
			return;
		}
		this.console.text = this.console.text + c.ToString();
		this._consoleAudioSource.PlayOneShot(this.newCharSound, 0.5f);
		this._textTimer = this._textCooldown;
	}

	// Token: 0x04000CB3 RID: 3251
	public Camera cam;

	// Token: 0x04000CB4 RID: 3252
	public Transform simulationObj;

	// Token: 0x04000CB5 RID: 3253
	public SpriteRenderer background;

	// Token: 0x04000CB6 RID: 3254
	public SpriteRenderer foreground;

	// Token: 0x04000CB7 RID: 3255
	public SpriteRenderer blackScreen;

	// Token: 0x04000CB8 RID: 3256
	public SpriteRenderer titleBar;

	// Token: 0x04000CB9 RID: 3257
	public TextMeshPro title;

	// Token: 0x04000CBA RID: 3258
	public TextMeshPro console;

	// Token: 0x04000CBB RID: 3259
	private AudioSource _consoleAudioSource;

	// Token: 0x04000CBC RID: 3260
	private AudioSource _voiceAudioSource;

	// Token: 0x04000CBD RID: 3261
	private AudioSource _soundAudioSource;

	// Token: 0x04000CBE RID: 3262
	private AudioSource _ambienceAudioSource;

	// Token: 0x04000CBF RID: 3263
	private AudioSource _musicAudioSource;

	// Token: 0x04000CC0 RID: 3264
	public AudioClip newLineSound;

	// Token: 0x04000CC1 RID: 3265
	public AudioClip newCharSound;

	// Token: 0x04000CC2 RID: 3266
	public AudioClip ambienceTrack;

	// Token: 0x04000CC3 RID: 3267
	public AudioClip musicTrack;

	// Token: 0x04000CC4 RID: 3268
	[SerializeField]
	public List<StartingAnimation.Step> steps = new List<StartingAnimation.Step>();

	// Token: 0x04000CC5 RID: 3269
	private int _index = -1;

	// Token: 0x04000CC6 RID: 3270
	private AudioClip _voiceClip;

	// Token: 0x04000CC7 RID: 3271
	private Color _previousBackgroundColor;

	// Token: 0x04000CC8 RID: 3272
	private Color _previousForegroundColor;

	// Token: 0x04000CC9 RID: 3273
	private Color _backgroundColor;

	// Token: 0x04000CCA RID: 3274
	private Color _foregroundColor;

	// Token: 0x04000CCB RID: 3275
	private float _timer;

	// Token: 0x04000CCC RID: 3276
	private float _textTimer;

	// Token: 0x04000CCD RID: 3277
	private float _soundTimer;

	// Token: 0x04000CCE RID: 3278
	private float _voiceTimer;

	// Token: 0x04000CCF RID: 3279
	private float _initialStart = 3f;

	// Token: 0x04000CD0 RID: 3280
	private float _colorTimer;

	// Token: 0x04000CD1 RID: 3281
	private float _textCooldown = 0.05f;

	// Token: 0x04000CD2 RID: 3282
	private float _voiceVolume = 1f;

	// Token: 0x04000CD3 RID: 3283
	private bool _finished;

	// Token: 0x04000CD4 RID: 3284
	private bool _playingAudio;

	// Token: 0x04000CD5 RID: 3285
	private bool _playingSound;

	// Token: 0x04000CD6 RID: 3286
	private bool _changingColor;

	// Token: 0x04000CD7 RID: 3287
	private bool _pitchVoiceToZero;

	// Token: 0x04000CD8 RID: 3288
	private bool _printingToConsole;

	// Token: 0x04000CD9 RID: 3289
	private bool _delayedSound;

	// Token: 0x04000CDA RID: 3290
	private bool _delayedVoice;

	// Token: 0x04000CDB RID: 3291
	private bool _clearingConsole;

	// Token: 0x04000CDC RID: 3292
	private List<char> _chars = new List<char>();

	// Token: 0x020001F8 RID: 504
	[Serializable]
	public class Step
	{
		// Token: 0x04000CDD RID: 3293
		public AudioClip voiceClip;

		// Token: 0x04000CDE RID: 3294
		public AudioClip soundClip;

		// Token: 0x04000CDF RID: 3295
		public string text = "";

		// Token: 0x04000CE0 RID: 3296
		public float stepCooldown;

		// Token: 0x04000CE1 RID: 3297
		public float voiceDelay;

		// Token: 0x04000CE2 RID: 3298
		public float voicePitch = 1f;

		// Token: 0x04000CE3 RID: 3299
		public float voiceVolume = 1f;

		// Token: 0x04000CE4 RID: 3300
		public float soundDelay;

		// Token: 0x04000CE5 RID: 3301
		public float textCooldown = 0.05f;

		// Token: 0x04000CE6 RID: 3302
		public bool startMusic;

		// Token: 0x04000CE7 RID: 3303
		public bool changeColor;

		// Token: 0x04000CE8 RID: 3304
		public bool clearConsole;

		// Token: 0x04000CE9 RID: 3305
		public bool pitchDownVoiceToZero;

		// Token: 0x04000CEA RID: 3306
		public Color backgroundColor;

		// Token: 0x04000CEB RID: 3307
		public Color foregroundColor;
	}
}
