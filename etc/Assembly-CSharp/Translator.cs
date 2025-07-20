using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020001FC RID: 508
[DefaultExecutionOrder(-1)]
public class Translator : Singleton<Translator>
{
	// Token: 0x06000F67 RID: 3943 RVA: 0x00048CC1 File Offset: 0x00046EC1
	public override void Awake()
	{
		if (!Translator.isGenerated)
		{
			Translator.translations = new Dictionary<string, string>();
			this.ActivateLanguageFile("");
		}
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00048CE0 File Offset: 0x00046EE0
	public void ActivateLanguageFile(string file_name = "")
	{
		string text = "";
		if (file_name != "")
		{
			File.Exists("Assets/StreamingAssets/Translations/" + file_name + ".json");
		}
		if (text == "")
		{
			text = this.defaultLanguageFile.ToString();
		}
		LanguageData languageData = JsonUtility.FromJson<LanguageData>(text);
		if (languageData != null)
		{
			Debug.Log("[TRANSLATOR] Language file " + languageData.ToString() + " has been activated!");
			Translator.isGenerated = true;
			return;
		}
		Debug.Log("[TRANSLATOR] Ran into an error while generating translations!");
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x00048D64 File Offset: 0x00046F64
	public static string GetString(string string_id)
	{
		if (Translator.translations.ContainsKey(string_id))
		{
			return Translator.translations[string_id];
		}
		return string_id;
	}

	// Token: 0x04000CFD RID: 3325
	private static Dictionary<string, string> translations;

	// Token: 0x04000CFE RID: 3326
	private static bool isGenerated;

	// Token: 0x04000CFF RID: 3327
	private const string LANGUAGES_PATH = "Assets/StreamingAssets/Translations/";

	// Token: 0x04000D00 RID: 3328
	[SerializeField]
	protected TextAsset defaultLanguageFile;
}
