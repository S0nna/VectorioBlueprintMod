using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class LTUtility
{
	// Token: 0x060001F4 RID: 500 RVA: 0x0000E23C File Offset: 0x0000C43C
	public static Vector3[] reverse(Vector3[] arr)
	{
		int num = arr.Length;
		int i = 0;
		int num2 = num - 1;
		while (i < num2)
		{
			Vector3 vector = arr[i];
			arr[i] = arr[num2];
			arr[num2] = vector;
			i++;
			num2--;
		}
		return arr;
	}
}
