using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Vectorio.Serialization
{
	// Token: 0x0200025E RID: 606
	public static class FileOperations
	{
		// Token: 0x060011AF RID: 4527 RVA: 0x00051984 File Offset: 0x0004FB84
		public static string GetSavePath(string folder)
		{
			string persistentDataPath = Application.persistentDataPath;
			string path = SaveSystem.IS_EXPERIMENTAL ? "World Saves/Experimental" : "World Saves/Live";
			return Path.Combine(persistentDataPath, path, folder);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000519B1 File Offset: 0x0004FBB1
		public static string GetSavePath(GamemodeData gamemode)
		{
			return FileOperations.GetSavePath(gamemode.FormattedName);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x000519C0 File Offset: 0x0004FBC0
		public static string[] GetAllSaves(string folder)
		{
			string savePath = FileOperations.GetSavePath(folder);
			if (Directory.Exists(savePath))
			{
				return Directory.GetFiles(savePath, "*" + FileOperations.SAVE_FILE_EXTENSION);
			}
			return new string[0];
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x000519F8 File Offset: 0x0004FBF8
		public static string[] GetAllSaves(GamemodeData gamemode)
		{
			return FileOperations.GetAllSaves(gamemode.FormattedName);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00051A08 File Offset: 0x0004FC08
		public static int GetNextAvailableSaveNumber(string folder)
		{
			string[] allSaves = FileOperations.GetAllSaves(folder);
			if (allSaves.Length == 0)
			{
				return 1;
			}
			Regex regex = new Regex(FileOperations.SAVE_FILE_NAME + "(\\d+)\\.sav");
			List<int> list = (from save in allSaves
			select regex.Match(Path.GetFileName(save)) into match
			where match.Success
			select int.Parse(match.Groups[1].Value) into number
			orderby number
			select number).ToList<int>();
			for (int i = 1; i <= list.Count; i++)
			{
				if (!list.Contains(i))
				{
					return i;
				}
			}
			return list.Count + 1;
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00051AEA File Offset: 0x0004FCEA
		public static int GetNextAvailableSaveNumber(GamemodeData gamemode)
		{
			return FileOperations.GetNextAvailableSaveNumber(gamemode.FormattedName);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00051AF8 File Offset: 0x0004FCF8
		public static string GenerateSaveFileName(string folder)
		{
			int nextAvailableSaveNumber = FileOperations.GetNextAvailableSaveNumber(folder);
			return string.Format("{0}{1}{2}", FileOperations.SAVE_FILE_NAME, nextAvailableSaveNumber, FileOperations.SAVE_FILE_EXTENSION);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00051B26 File Offset: 0x0004FD26
		public static string GenerateSaveFileName(GamemodeData gamemode)
		{
			return FileOperations.GenerateSaveFileName(gamemode.FormattedName);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00051B33 File Offset: 0x0004FD33
		public static string GenerateSaveFileName()
		{
			return FileOperations.GenerateSaveFileName("Unknown");
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00051B40 File Offset: 0x0004FD40
		public static void WriteSaveToFile(SaveData save, string folder)
		{
			string savePath = FileOperations.GetSavePath(folder);
			DataProcessor.SerializeDataToFileAsync<SaveData>(save, savePath, save.FileName);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00051B61 File Offset: 0x0004FD61
		public static void WriteSaveToFile(SaveData save, GamemodeData gamemode)
		{
			FileOperations.WriteSaveToFile(save, gamemode.FormattedName);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00051B70 File Offset: 0x0004FD70
		public static bool DeleteSave(string folder, string fileName)
		{
			string savePath = FileOperations.GetSavePath(folder);
			string path = Path.Combine(savePath, fileName);
			if (File.Exists(path))
			{
				File.Delete(path);
				return true;
			}
			Debug.LogWarning("Save file " + fileName + " does not exist in " + savePath);
			return false;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00051BB3 File Offset: 0x0004FDB3
		public static bool DeleteSave(GamemodeData gamemode, string fileName)
		{
			return FileOperations.DeleteSave(gamemode.FormattedName, fileName);
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00051BC4 File Offset: 0x0004FDC4
		public static void OpenFile(string folder, string fileName)
		{
			string savePath = FileOperations.GetSavePath(folder);
			if (File.Exists(Path.Combine(savePath, fileName)))
			{
				try
				{
					Application.OpenURL("file://" + savePath);
				}
				catch
				{
					Debug.Log("[FILE OPERATIONS] Could not open file due to missing permissions!");
				}
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00051C18 File Offset: 0x0004FE18
		public static void OpenFile(GamemodeData gamemode, string fileName)
		{
			FileOperations.OpenFile(gamemode.FormattedName, fileName);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00051C26 File Offset: 0x0004FE26
		public static string GetUserFilePath(string path, string fileName)
		{
			return Path.Combine(Application.persistentDataPath, path, fileName);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00051C34 File Offset: 0x0004FE34
		public static bool UserFileExists(string path, string fileName)
		{
			return File.Exists(FileOperations.GetUserFilePath(path, fileName));
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00051C42 File Offset: 0x0004FE42
		public static T GetUserFile<T>(string path, string fileName) where T : SerializableData
		{
			return DataProcessor.DeserializeDataFromJsonFile<T>(FileOperations.GetUserFilePath(path, fileName));
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00051C50 File Offset: 0x0004FE50
		public static void WriteUserFile<T>(T data, string path, string fileName) where T : SerializableData
		{
			string path2 = Path.Combine(Application.persistentDataPath, path);
			DataProcessor.SerializeDataToJsonFile<T>(data, path2, fileName);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00051C74 File Offset: 0x0004FE74
		public static bool DeleteUserFile(string path, string fileName)
		{
			string userFilePath = FileOperations.GetUserFilePath(path, fileName);
			if (File.Exists(userFilePath))
			{
				File.Delete(userFilePath);
				return true;
			}
			Debug.LogWarning("User file " + fileName + " does not exist in " + path);
			return false;
		}

		// Token: 0x04000F14 RID: 3860
		public static string SAVE_FILE_NAME = "world_";

		// Token: 0x04000F15 RID: 3861
		public static string SAVE_FILE_EXTENSION = ".sav";

		// Token: 0x04000F16 RID: 3862
		public static string SETTINGS_FILE_EXTENSION = ".json";

		// Token: 0x04000F17 RID: 3863
		private const string SAVES_PATH = "World Saves/Live";

		// Token: 0x04000F18 RID: 3864
		private const string EXPERIMENTAL_PATH = "World Saves/Experimental";

		// Token: 0x04000F19 RID: 3865
		public const string UNKNOWN_FOLDER = "Unknown";
	}
}
