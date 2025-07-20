using System;
using System.Collections.Generic;

// Token: 0x020000BE RID: 190
public class LanguageData
{
	// Token: 0x060006AE RID: 1710 RVA: 0x00020098 File Offset: 0x0001E298
	public Dictionary<string, string> GetTranslations()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (LanguageData.Translation translation in this.translations)
		{
			dictionary.Add(translation.string_id, translation.output);
		}
		return dictionary;
	}

	// Token: 0x04000424 RID: 1060
	public LanguageData.Translation[] translations;

	// Token: 0x020000BF RID: 191
	public class Translation
	{
		// Token: 0x04000425 RID: 1061
		public string string_id;

		// Token: 0x04000426 RID: 1062
		public string output;
	}
}
