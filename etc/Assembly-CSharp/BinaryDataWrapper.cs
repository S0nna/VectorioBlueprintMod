using System;
using UnityEngine;
using Vectorio.Serialization;

// Token: 0x0200007A RID: 122
[CreateAssetMenu(fileName = "Binary Data", menuName = "Vectorio/Binary Data")]
public class BinaryDataWrapper : ScriptableObject
{
	// Token: 0x0600059A RID: 1434 RVA: 0x0001E618 File Offset: 0x0001C818
	public T GetData<T>() where T : SerializableData
	{
		if (this.binaryData == null)
		{
			Debug.LogError("[BINARY] No valid data has been set.");
			return default(T);
		}
		return DataProcessor.DecompressAndDeserializeObject<T>(this.binaryData.bytes);
	}

	// Token: 0x0400032C RID: 812
	public TextAsset binaryData;
}
