using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200022F RID: 559
public abstract class SerializiableDict<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	// Token: 0x0600105C RID: 4188 RVA: 0x0004CCB8 File Offset: 0x0004AEB8
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		base.Clear();
		int num = 0;
		while (num < this.keyData.Count && num < this.valueData.Count)
		{
			base[this.keyData[num]] = this.valueData[num];
			num++;
		}
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0004CD10 File Offset: 0x0004AF10
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this.keyData.Clear();
		this.valueData.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
		{
			this.keyData.Add(keyValuePair.Key);
			this.valueData.Add(keyValuePair.Value);
		}
	}

	// Token: 0x04000E54 RID: 3668
	[SerializeField]
	[HideInInspector]
	private List<TKey> keyData = new List<TKey>();

	// Token: 0x04000E55 RID: 3669
	[SerializeField]
	[HideInInspector]
	private List<TValue> valueData = new List<TValue>();
}
