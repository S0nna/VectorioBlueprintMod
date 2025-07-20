using System;
using MK.Glow.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x02000207 RID: 519
public class VolumeManager : Singleton<global::VolumeManager>
{
	// Token: 0x06000F94 RID: 3988 RVA: 0x00049B90 File Offset: 0x00047D90
	public void SetBloom(float value)
	{
		MKGlowLite mkglowLite = (MKGlowLite)this._volume.profile.components[0];
		if (mkglowLite != null)
		{
			mkglowLite.bloomIntensity.value = value;
		}
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x00049BD0 File Offset: 0x00047DD0
	public void SetVignette(float value)
	{
		Vignette vignette = (Vignette)this._volume.profile.components[1];
		if (vignette != null)
		{
			vignette.intensity.value = value;
		}
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00049C10 File Offset: 0x00047E10
	public void SetFilmGrain(float value)
	{
		FilmGrain filmGrain = (FilmGrain)this._volume.profile.components[2];
		if (filmGrain != null)
		{
			filmGrain.intensity.value = value;
		}
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x00049C4E File Offset: 0x00047E4E
	public void SetDepthOfField(float value)
	{
		((DepthOfField)this._volume.profile.components[3]).focusDistance.value = value;
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x00049C78 File Offset: 0x00047E78
	public void ToggleVignette(bool toggle)
	{
		Vignette vignette = (Vignette)this._volume.profile.components[1];
		if (vignette != null)
		{
			vignette.active = toggle;
		}
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x00049CB4 File Offset: 0x00047EB4
	public void ToggleFilmGrain(bool toggle)
	{
		FilmGrain filmGrain = (FilmGrain)this._volume.profile.components[2];
		if (filmGrain != null)
		{
			filmGrain.active = toggle;
		}
	}

	// Token: 0x04000D57 RID: 3415
	private const int BLOOM_INDEX = 0;

	// Token: 0x04000D58 RID: 3416
	private const int VIGNETTE_INDEX = 1;

	// Token: 0x04000D59 RID: 3417
	private const int FILM_GRAIN_INDEX = 2;

	// Token: 0x04000D5A RID: 3418
	private const int DEPTH_OF_FIELD_INDEX = 3;

	// Token: 0x04000D5B RID: 3419
	[SerializeField]
	private Volume _volume;
}
