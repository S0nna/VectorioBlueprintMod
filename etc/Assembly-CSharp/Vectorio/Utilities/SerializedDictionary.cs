using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Utilities
{
	// Token: 0x02000286 RID: 646
	public abstract class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		// Token: 0x06001238 RID: 4664 RVA: 0x00053994 File Offset: 0x00051B94
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

		// Token: 0x06001239 RID: 4665 RVA: 0x000539EC File Offset: 0x00051BEC
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

		// Token: 0x04000FFE RID: 4094
		[SerializeField]
		[HideInInspector]
		private List<TKey> keyData = new List<TKey>();

		// Token: 0x04000FFF RID: 4095
		[SerializeField]
		[HideInInspector]
		private List<TValue> valueData = new List<TValue>();
	}
}
