using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class LeanTest
{
	// Token: 0x0600010D RID: 269 RVA: 0x0000960B File Offset: 0x0000780B
	public static void debug(string name, bool didPass, string failExplaination = null)
	{
		LeanTest.expect(didPass, name, failExplaination);
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00009618 File Offset: 0x00007818
	public static void expect(bool didPass, string definition, string failExplaination = null)
	{
		float num = LeanTest.printOutLength(definition);
		int totalWidth = 40 - (int)(num * 1.05f);
		string text = "".PadRight(totalWidth, "_"[0]);
		string text2 = string.Concat(new string[]
		{
			LeanTest.formatB(definition),
			" ",
			text,
			" [ ",
			didPass ? LeanTest.formatC("pass", "green") : LeanTest.formatC("fail", "red"),
			" ]"
		});
		if (!didPass && failExplaination != null)
		{
			text2 = text2 + " - " + failExplaination;
		}
		Debug.Log(text2);
		if (didPass)
		{
			LeanTest.passes++;
		}
		LeanTest.tests++;
		if (LeanTest.tests == LeanTest.expected && !LeanTest.testsFinished)
		{
			LeanTest.overview();
		}
		else if (LeanTest.tests > LeanTest.expected)
		{
			Debug.Log(LeanTest.formatB("Too many tests for a final report!") + " set LeanTest.expected = " + LeanTest.tests.ToString());
		}
		if (!LeanTest.timeoutStarted)
		{
			LeanTest.timeoutStarted = true;
			GameObject gameObject = new GameObject();
			gameObject.name = "~LeanTest";
			(gameObject.AddComponent(typeof(LeanTester)) as LeanTester).timeout = LeanTest.timeout;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00009768 File Offset: 0x00007968
	public static string padRight(int len)
	{
		string text = "";
		for (int i = 0; i < len; i++)
		{
			text += "_";
		}
		return text;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00009794 File Offset: 0x00007994
	public static float printOutLength(string str)
	{
		float num = 0f;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == "I"[0])
			{
				num += 0.5f;
			}
			else if (str[i] == "J"[0])
			{
				num += 0.85f;
			}
			else
			{
				num += 1f;
			}
		}
		return num;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000097FD File Offset: 0x000079FD
	public static string formatBC(string str, string color)
	{
		return LeanTest.formatC(LeanTest.formatB(str), color);
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0000980B File Offset: 0x00007A0B
	public static string formatB(string str)
	{
		return "<b>" + str + "</b>";
	}

	// Token: 0x06000113 RID: 275 RVA: 0x0000981D File Offset: 0x00007A1D
	public static string formatC(string str, string color)
	{
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">",
			str,
			"</color>"
		});
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000984C File Offset: 0x00007A4C
	public static void overview()
	{
		LeanTest.testsFinished = true;
		int num = LeanTest.expected - LeanTest.passes;
		string text = (num > 0) ? LeanTest.formatBC(num.ToString() ?? "", "red") : (num.ToString() ?? "");
		Debug.Log(string.Concat(new string[]
		{
			LeanTest.formatB("Final Report:"),
			" _____________________ PASSED: ",
			LeanTest.formatBC(LeanTest.passes.ToString() ?? "", "green"),
			" FAILED: ",
			text,
			" "
		}));
	}

	// Token: 0x040000EA RID: 234
	public static int expected = 0;

	// Token: 0x040000EB RID: 235
	private static int tests = 0;

	// Token: 0x040000EC RID: 236
	private static int passes = 0;

	// Token: 0x040000ED RID: 237
	public static float timeout = 15f;

	// Token: 0x040000EE RID: 238
	public static bool timeoutStarted = false;

	// Token: 0x040000EF RID: 239
	public static bool testsFinished = false;
}
