using System;
using Vectorio.Stats;

// Token: 0x02000141 RID: 321
[Serializable]
public class ModifierData
{
	// Token: 0x06000AAB RID: 2731 RVA: 0x0002CB66 File Offset: 0x0002AD66
	public ModifierData(StatType statType, float value)
	{
		this.id = (int)statType;
		this.value = value;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0002CB7C File Offset: 0x0002AD7C
	public ModifierData(int id, StatModifier modifier)
	{
		this.id = id;
		this.value = modifier.Value;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0002CB97 File Offset: 0x0002AD97
	public ModifierData()
	{
		this.id = 0;
		this.value = 0f;
	}

	// Token: 0x040006AA RID: 1706
	public int id;

	// Token: 0x040006AB RID: 1707
	public float value;
}
