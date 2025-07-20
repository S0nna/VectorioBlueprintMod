using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x0200019E RID: 414
[DefaultExecutionOrder(0)]
public class StatLibrary : MonoBehaviour
{
	// Token: 0x06000DAF RID: 3503 RVA: 0x0003C938 File Offset: 0x0003AB38
	public void Awake()
	{
		if (StatLibrary._isSetup)
		{
			return;
		}
		foreach (StatInfo statInfo in this.stats)
		{
			int type = (int)statInfo.type;
			if (StatLibrary._library.ContainsKey(type))
			{
				Debug.Log("[STAT LIBRARY] Duplicate stat with ID " + type.ToString());
				return;
			}
			StatLibrary._library.Add(type, statInfo);
		}
		StatLibrary._isSetup = true;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x0003C9CC File Offset: 0x0003ABCC
	public static StatInfo FetchInfo(int code)
	{
		if (StatLibrary._library.ContainsKey(code))
		{
			return StatLibrary._library[code];
		}
		return null;
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x0003C9E8 File Offset: 0x0003ABE8
	public static StatInfo FetchInfo(StatType type)
	{
		if (StatLibrary._library.ContainsKey((int)type))
		{
			return StatLibrary._library[(int)type];
		}
		return null;
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x0003CA14 File Offset: 0x0003AC14
	public void DebugUnset()
	{
		List<StatType> list = new List<StatType>();
		foreach (StatInfo statInfo in this.stats)
		{
			if (list.Contains(statInfo.type))
			{
				Debug.Log("[STAT LIBRARY] Duplicate registered stat: " + statInfo.type.ToString());
			}
			else
			{
				list.Add(statInfo.type);
			}
		}
		List<StatType> list2 = new List<StatType>();
		foreach (object obj in Enum.GetValues(typeof(StatType)))
		{
			StatType item = (StatType)obj;
			if (!list.Contains(item))
			{
				list2.Add(item);
			}
		}
		string text = "[STAT LIBRARY] The following stats are undefined...\n\n";
		foreach (StatType statType in list2)
		{
			string[] array = new string[6];
			array[0] = text;
			array[1] = "- ";
			array[2] = statType.ToString();
			array[3] = " (";
			int num = 4;
			int num2 = (int)statType;
			array[num] = num2.ToString();
			array[5] = ")\n";
			text = string.Concat(array);
		}
		Debug.Log(text);
	}

	// Token: 0x040009DE RID: 2526
	private static bool _isSetup = false;

	// Token: 0x040009DF RID: 2527
	private static Dictionary<int, StatInfo> _library = new Dictionary<int, StatInfo>();

	// Token: 0x040009E0 RID: 2528
	public List<StatInfo> stats;
}
