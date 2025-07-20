using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class LeanAudioStream
{
	// Token: 0x060000DB RID: 219 RVA: 0x00008850 File Offset: 0x00006A50
	public LeanAudioStream(float[] audioArr)
	{
		this.audioArr = audioArr;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00008860 File Offset: 0x00006A60
	public void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = this.audioArr[this.position];
			this.position++;
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00008899 File Offset: 0x00006A99
	public void OnAudioSetPosition(int newPosition)
	{
		this.position = newPosition;
	}

	// Token: 0x040000D0 RID: 208
	public int position;

	// Token: 0x040000D1 RID: 209
	public AudioClip audioClip;

	// Token: 0x040000D2 RID: 210
	public float[] audioArr;
}
