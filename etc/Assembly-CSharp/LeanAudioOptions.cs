using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class LeanAudioOptions
{
	// Token: 0x060000EE RID: 238 RVA: 0x00008EDF File Offset: 0x000070DF
	public LeanAudioOptions setFrequency(int frequencyRate)
	{
		this.frequencyRate = frequencyRate;
		return this;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00008EE9 File Offset: 0x000070E9
	public LeanAudioOptions setVibrato(Vector3[] vibrato)
	{
		this.vibrato = vibrato;
		return this;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00008EF3 File Offset: 0x000070F3
	public LeanAudioOptions setWaveSine()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Sine;
		return this;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00008EFD File Offset: 0x000070FD
	public LeanAudioOptions setWaveSquare()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Square;
		return this;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00008F07 File Offset: 0x00007107
	public LeanAudioOptions setWaveSawtooth()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Sawtooth;
		return this;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00008F11 File Offset: 0x00007111
	public LeanAudioOptions setWaveNoise()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Noise;
		return this;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00008F1B File Offset: 0x0000711B
	public LeanAudioOptions setWaveStyle(LeanAudioOptions.LeanAudioWaveStyle style)
	{
		this.waveStyle = style;
		return this;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00008F25 File Offset: 0x00007125
	public LeanAudioOptions setWaveNoiseScale(float waveScale)
	{
		this.waveNoiseScale = waveScale;
		return this;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00008F2F File Offset: 0x0000712F
	public LeanAudioOptions setWaveNoiseInfluence(float influence)
	{
		this.waveNoiseInfluence = influence;
		return this;
	}

	// Token: 0x040000D8 RID: 216
	public LeanAudioOptions.LeanAudioWaveStyle waveStyle;

	// Token: 0x040000D9 RID: 217
	public Vector3[] vibrato;

	// Token: 0x040000DA RID: 218
	public Vector3[] modulation;

	// Token: 0x040000DB RID: 219
	public int frequencyRate = 44100;

	// Token: 0x040000DC RID: 220
	public float waveNoiseScale = 1000f;

	// Token: 0x040000DD RID: 221
	public float waveNoiseInfluence = 1f;

	// Token: 0x040000DE RID: 222
	public bool useSetData = true;

	// Token: 0x040000DF RID: 223
	public LeanAudioStream stream;

	// Token: 0x02000030 RID: 48
	public enum LeanAudioWaveStyle
	{
		// Token: 0x040000E1 RID: 225
		Sine,
		// Token: 0x040000E2 RID: 226
		Square,
		// Token: 0x040000E3 RID: 227
		Sawtooth,
		// Token: 0x040000E4 RID: 228
		Noise
	}
}
