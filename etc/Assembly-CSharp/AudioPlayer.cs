using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class AudioPlayer : Singleton<AudioPlayer>
{
	// Token: 0x0600045A RID: 1114 RVA: 0x00016F3C File Offset: 0x0001513C
	public void SetSpatialBlending(float value)
	{
		AudioPlayer.SPATIAL_BLEND = value;
		if (Math.Abs(value - AudioPlayer.SPATIAL_BLEND) > 0.05f)
		{
			foreach (AudioSource audioSource in this.activeSources)
			{
				audioSource.spatialBlend = AudioPlayer.SPATIAL_BLEND;
			}
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00016FAC File Offset: 0x000151AC
	public override void Awake()
	{
		base.Awake();
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.interfaceSource == null)
		{
			this.interfaceSource = new GameObject("Interface Source").AddComponent<AudioSource>();
			this.interfaceSource.playOnAwake = false;
			this.interfaceSource.transform.SetParent(base.transform);
		}
		if (this.prioritySource == null)
		{
			this.prioritySource = new GameObject("Priority Source").AddComponent<AudioSource>();
			this.prioritySource.playOnAwake = false;
			this.prioritySource.transform.SetParent(base.transform);
		}
		if (this.placementSource == null)
		{
			this.placementSource = new GameObject("Placement Source").AddComponent<AudioSource>();
			this.placementSource.playOnAwake = false;
			this.placementSource.transform.SetParent(base.transform);
		}
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0001709C File Offset: 0x0001529C
	public void Update()
	{
		for (int i = 0; i < this.instances.Count; i++)
		{
			if (this.instances[i].Update)
			{
				this.instances.RemoveAt(i);
				i--;
			}
		}
		for (int j = 0; j < this.activeSources.Count; j++)
		{
			if (!this.activeSources[j].isPlaying)
			{
				this.activeSources[j].gameObject.SetActive(false);
				this.pooledSources.Add(this.activeSources[j]);
				this.activeSources.RemoveAt(j);
				j--;
			}
		}
		for (int k = 0; k < this.delayedSources.Count; k++)
		{
			if (this.delayedSources[k].delay <= 0f)
			{
				this.PlayInterfaceSound(this.delayedSources[k].clip);
				this.delayedSources.RemoveAt(k);
			}
			else
			{
				this.delayedSources[k].delay -= Time.deltaTime;
			}
		}
		if (this._setupFilter)
		{
			this._setupFilter = false;
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x000171C8 File Offset: 0x000153C8
	public void PlayBlueprintCreateSound(Vector2 pos)
	{
		if (this.blueprintCreateSound != null)
		{
			this.PlayClipAtPoint(this.blueprintCreateSound, "sbc", pos, 1f, true, 0.9f, 1.1f, false);
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00017208 File Offset: 0x00015408
	public void PlayBlueprintRemoveSound(Vector2 pos)
	{
		if (this.blueprintRemoveSound != null)
		{
			this.PlayClipAtPoint(this.blueprintRemoveSound, "sbr", pos, 1f, true, 0.9f, 1.1f, false);
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x00017248 File Offset: 0x00015448
	public void PlayPlacementSound(Vector2 pos)
	{
		if (this.placementSound != null)
		{
			this.PlayClipAtPoint(this.placementSound, "sp", pos, 1f, true, 0.9f, 1.1f, false);
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00017288 File Offset: 0x00015488
	public void PlayDestructionSound(Vector2 pos)
	{
		if (this.destroyedSound != null)
		{
			this.PlayClipAtPoint(this.destroyedSound, "sd", pos, 1f, true, 0.9f, 1.1f, false);
		}
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x000172C6 File Offset: 0x000154C6
	public void PlayInterfaceSound(AudioClip clip)
	{
		if (Singleton<Gamemode>.Instance.ForceDisableSounds)
		{
			return;
		}
		if (this._setupFilter)
		{
			return;
		}
		this.interfaceSource.volume = Singleton<Settings>.Instance.GetInterfaceVolume();
		this.interfaceSource.PlayOneShot(clip, 1f);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x00017304 File Offset: 0x00015504
	public void PlayDelayedInterfaceSound(AudioClip clip, float delay)
	{
		if (Singleton<Gamemode>.Instance.ForceDisableSounds)
		{
			return;
		}
		if (this._setupFilter)
		{
			return;
		}
		this.delayedSources.Add(new AudioPlayer.DelayedAudio(clip, delay));
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00017330 File Offset: 0x00015530
	public void Play(AudioClip clip, string id, float audioScale = 1f, bool randomizePitch = true, float minPitch = 0.9f, float maxPitch = 1.1f, bool overrideCooldown = false)
	{
		if (!Singleton<Gamemode>.Instance.UseEntitySounds)
		{
			return;
		}
		if (!overrideCooldown && this.IsClipPlaying(id))
		{
			return;
		}
		this.audioSource.volume = Singleton<Settings>.Instance.GetEffectsVolume();
		if (randomizePitch)
		{
			this.audioSource.pitch = Random.Range(minPitch, maxPitch);
		}
		else
		{
			this.audioSource.pitch = 1f;
		}
		this.audioSource.PlayOneShot(clip, audioScale);
		this.instances.Add(new AudioPlayer.AudioInstance(id, 0.05f));
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x000173BC File Offset: 0x000155BC
	public void PlayClipAtPoint(AudioClip clip, string id, Vector2 position, float multiplier = 1f, bool randomizePitch = true, float minRange = 0.9f, float maxRange = 1.1f, bool overrideCooldown = false)
	{
		if (!Singleton<Gamemode>.Instance.UseEntitySounds)
		{
			return;
		}
		if (!overrideCooldown && this.IsClipPlaying(id))
		{
			return;
		}
		if (this.pooledSources.Count > 0)
		{
			AudioSource audioSource = this.pooledSources[0];
			audioSource.clip = clip;
			if (randomizePitch)
			{
				audioSource.pitch = Random.Range(minRange, maxRange);
			}
			else
			{
				audioSource.pitch = 1f;
			}
			audioSource.volume = Singleton<Settings>.Instance.GetEffectsVolume() * multiplier;
			audioSource.spatialBlend = AudioPlayer.SPATIAL_BLEND;
			audioSource.transform.position = position;
			audioSource.gameObject.SetActive(true);
			this.activeSources.Add(audioSource);
			this.pooledSources.RemoveAt(0);
			audioSource.Play();
			return;
		}
		AudioSource audioSource2 = new GameObject("Audio Instance").AddComponent<AudioSource>();
		audioSource2.spatialBlend = AudioPlayer.SPATIAL_BLEND;
		audioSource2.clip = clip;
		if (randomizePitch)
		{
			audioSource2.pitch = Random.Range(minRange, maxRange);
		}
		else
		{
			audioSource2.pitch = 1f;
		}
		audioSource2.volume = Singleton<Settings>.Instance.GetEffectsVolume();
		audioSource2.transform.position = position;
		audioSource2.gameObject.SetActive(true);
		audioSource2.loop = false;
		audioSource2.Play();
		this.activeSources.Add(audioSource2);
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001750C File Offset: 0x0001570C
	public bool IsClipPlaying(string id)
	{
		using (List<AudioPlayer.AudioInstance>.Enumerator enumerator = this.instances.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.id == id)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0400023E RID: 574
	private List<AudioPlayer.AudioInstance> instances = new List<AudioPlayer.AudioInstance>();

	// Token: 0x0400023F RID: 575
	private List<AudioSource> activeSources = new List<AudioSource>();

	// Token: 0x04000240 RID: 576
	private List<AudioSource> pooledSources = new List<AudioSource>();

	// Token: 0x04000241 RID: 577
	private List<AudioPlayer.DelayedAudio> delayedSources = new List<AudioPlayer.DelayedAudio>();

	// Token: 0x04000242 RID: 578
	public AudioSource audioSource;

	// Token: 0x04000243 RID: 579
	public AudioClip blueprintCreateSound;

	// Token: 0x04000244 RID: 580
	public AudioClip blueprintRemoveSound;

	// Token: 0x04000245 RID: 581
	public AudioClip placementSound;

	// Token: 0x04000246 RID: 582
	public AudioClip destroyedSound;

	// Token: 0x04000247 RID: 583
	protected AudioSource interfaceSource;

	// Token: 0x04000248 RID: 584
	protected AudioSource prioritySource;

	// Token: 0x04000249 RID: 585
	protected AudioSource placementSource;

	// Token: 0x0400024A RID: 586
	protected bool _setupFilter = true;

	// Token: 0x0400024B RID: 587
	private static float SPATIAL_BLEND = 1f;

	// Token: 0x0400024C RID: 588
	public const float MIN_SPATIAL_BLEND = 0.5f;

	// Token: 0x0400024D RID: 589
	public const float MAX_SPATIAL_BLEND = 1f;

	// Token: 0x0400024E RID: 590
	public static bool ALLOW_INFLUENCE_ON_SPATIAL_BLENDING = true;

	// Token: 0x02000056 RID: 86
	public class AudioInstance
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x000175B9 File Offset: 0x000157B9
		public AudioInstance(string id, float cooldown)
		{
			this.id = id;
			this.cooldown = cooldown;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000175D0 File Offset: 0x000157D0
		public bool Update
		{
			get
			{
				return (this.cooldown -= Time.deltaTime) <= 0f;
			}
		}

		// Token: 0x0400024F RID: 591
		public string id;

		// Token: 0x04000250 RID: 592
		public float cooldown;
	}

	// Token: 0x02000057 RID: 87
	public class DelayedAudio
	{
		// Token: 0x0600046A RID: 1130 RVA: 0x000175FC File Offset: 0x000157FC
		public DelayedAudio(AudioClip clip, float delay)
		{
			this.clip = clip;
			this.delay = delay;
		}

		// Token: 0x04000251 RID: 593
		public AudioClip clip;

		// Token: 0x04000252 RID: 594
		public float delay;
	}
}
