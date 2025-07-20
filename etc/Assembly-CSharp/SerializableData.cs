using System;

// Token: 0x0200022E RID: 558
[Serializable]
public class SerializableData
{
	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x0600104F RID: 4175 RVA: 0x0004CBC9 File Offset: 0x0004ADC9
	// (set) Token: 0x06001050 RID: 4176 RVA: 0x0004CBD1 File Offset: 0x0004ADD1
	public string Version { get; set; }

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06001051 RID: 4177 RVA: 0x0004CBDA File Offset: 0x0004ADDA
	// (set) Token: 0x06001052 RID: 4178 RVA: 0x0004CBE2 File Offset: 0x0004ADE2
	public string ID { get; set; }

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06001053 RID: 4179 RVA: 0x0004CBEB File Offset: 0x0004ADEB
	// (set) Token: 0x06001054 RID: 4180 RVA: 0x0004CBF3 File Offset: 0x0004ADF3
	public string Name { get; set; }

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06001055 RID: 4181 RVA: 0x0004CBFC File Offset: 0x0004ADFC
	// (set) Token: 0x06001056 RID: 4182 RVA: 0x0004CC04 File Offset: 0x0004AE04
	public string Description { get; set; }

	// Token: 0x06001057 RID: 4183 RVA: 0x0004CC0D File Offset: 0x0004AE0D
	public int GetReleaseStatus()
	{
		if (string.IsNullOrEmpty(this.Version))
		{
			throw new InvalidOperationException("Version string is not set.");
		}
		return int.Parse(this.Version.Substring(1, 1));
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x0004CC39 File Offset: 0x0004AE39
	public int GetMajorUpdate()
	{
		if (string.IsNullOrEmpty(this.Version))
		{
			throw new InvalidOperationException("Version string is not set.");
		}
		return int.Parse(this.Version.Substring(3, 1));
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0004CC65 File Offset: 0x0004AE65
	public int GetMinorUpdate()
	{
		if (string.IsNullOrEmpty(this.Version))
		{
			throw new InvalidOperationException("Version string is not set.");
		}
		return int.Parse(this.Version.Substring(5, 1));
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0004CC91 File Offset: 0x0004AE91
	public char GetHotfix()
	{
		if (string.IsNullOrEmpty(this.Version))
		{
			throw new InvalidOperationException("Version string is not set.");
		}
		return this.Version[6];
	}
}
