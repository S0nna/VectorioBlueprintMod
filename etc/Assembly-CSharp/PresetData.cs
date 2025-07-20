using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x0200009A RID: 154
[CreateAssetMenu(fileName = "New Preset", menuName = "Vectorio/Preset")]
public class PresetData : BaseData
{
	// Token: 0x040003A7 RID: 935
	public Sprite icon;

	// Token: 0x040003A8 RID: 936
	public Color glowColor;

	// Token: 0x040003A9 RID: 937
	public Color backgroundColor;

	// Token: 0x040003AA RID: 938
	public int sortingOrder;

	// Token: 0x040003AB RID: 939
	public List<PresetData.Modifier> allyModifiers;

	// Token: 0x040003AC RID: 940
	public List<PresetData.Modifier> enemyModifiers;

	// Token: 0x0200009B RID: 155
	[Serializable]
	public class Modifier
	{
		// Token: 0x040003AD RID: 941
		public StatType stat;

		// Token: 0x040003AE RID: 942
		[Range(0.5f, 4f)]
		public float value = 1f;
	}
}
