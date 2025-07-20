using System;
using System.Collections.Generic;

// Token: 0x02000075 RID: 117
public class SaveValidator : Singleton<SaveValidator>
{
	// Token: 0x0600054F RID: 1359 RVA: 0x0001C930 File Offset: 0x0001AB30
	public SaveValidationResult CheckSave(SaveData saveData)
	{
		SaveValidationResult result = new SaveValidationResult
		{
			IsSaveOutdated = false,
			CanSaveBeLoaded = true,
			SaveStatus = "Save is valid"
		};
		foreach (SaveValidator.VersionRule versionRule in this.versionRules)
		{
			if (saveData.Version == versionRule.version)
			{
				result.IsSaveOutdated = true;
				result.CanSaveBeLoaded = versionRule.canBeLoaded;
				result.SaveStatus = versionRule.statusMessage;
				break;
			}
		}
		return result;
	}

	// Token: 0x0400030C RID: 780
	public List<SaveValidator.VersionRule> versionRules = new List<SaveValidator.VersionRule>();

	// Token: 0x02000076 RID: 118
	[Serializable]
	public class VersionRule
	{
		// Token: 0x0400030D RID: 781
		public string version;

		// Token: 0x0400030E RID: 782
		public bool canBeLoaded;

		// Token: 0x0400030F RID: 783
		public string statusMessage;
	}
}
