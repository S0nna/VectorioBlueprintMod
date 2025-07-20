using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vectorio.Formatting
{
	// Token: 0x0200025B RID: 603
	public class Formatter : MonoBehaviour
	{
		// Token: 0x0600119F RID: 4511 RVA: 0x000511F4 File Offset: 0x0004F3F4
		public static string Time(float time, bool milliseconds = false)
		{
			if (milliseconds)
			{
				return string.Format("{0:D1}", TimeSpan.FromMilliseconds((double)time).Milliseconds);
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			if (time < 3600f)
			{
				string arg = (timeSpan.Minutes < 10) ? ("0" + timeSpan.Minutes.ToString()) : timeSpan.Minutes.ToString();
				return string.Format("{0:D1}:{1:D2}", arg, timeSpan.Seconds);
			}
			string arg2 = (timeSpan.Minutes < 10) ? ("0" + timeSpan.Minutes.ToString()) : timeSpan.Minutes.ToString();
			string arg3 = (timeSpan.Hours < 10) ? ("0" + timeSpan.Hours.ToString()) : timeSpan.Hours.ToString();
			return string.Format("{0:D1}:{1:D2}:{2:D3}", arg3, arg2, timeSpan.Seconds);
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0005130C File Offset: 0x0004F50C
		public static string FormatPlayTime(float totalTimeInSeconds)
		{
			int num = Mathf.FloorToInt(totalTimeInSeconds / 86400f);
			int num2 = Mathf.FloorToInt(totalTimeInSeconds % 86400f / 3600f);
			int num3 = Mathf.FloorToInt(totalTimeInSeconds % 3600f / 60f);
			List<string> list = new List<string>();
			if (num > 0)
			{
				list.Add(string.Format("{0} {1}", num, (num == 1) ? "day" : "days"));
			}
			if (num2 > 0 || num > 0)
			{
				list.Add(string.Format("{0} {1}", num2, (num2 == 1) ? "hour" : "hours"));
			}
			list.Add(string.Format("{0} {1}", num3, (num3 == 1) ? "minute" : "minutes"));
			return string.Join(" ", list);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x000513DC File Offset: 0x0004F5DC
		public static string Round(float amount, int decimals = 2)
		{
			return Math.Round((double)amount, decimals).ToString();
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000513FC File Offset: 0x0004F5FC
		public static string Abbreviate(float number)
		{
			for (int i = Formatter.abbrevations.Count - 1; i >= 0; i--)
			{
				KeyValuePair<int, string> keyValuePair = Formatter.abbrevations.ElementAt(i);
				if (Mathf.Abs(number) >= (float)keyValuePair.Key)
				{
					return Mathf.FloorToInt(number / (float)keyValuePair.Key).ToString() + keyValuePair.Value;
				}
			}
			return number.ToString();
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00051467 File Offset: 0x0004F667
		public static string Number(float number)
		{
			return number.ToString("N0");
		}

		// Token: 0x04000F0B RID: 3851
		private static readonly SortedDictionary<int, string> abbrevations = new SortedDictionary<int, string>
		{
			{
				1000,
				"K"
			},
			{
				1000000,
				"M"
			},
			{
				1000000000,
				"B"
			}
		};
	}
}
