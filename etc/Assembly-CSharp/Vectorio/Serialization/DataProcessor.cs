using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Vectorio.Serialization
{
	// Token: 0x0200025C RID: 604
	public static class DataProcessor
	{
		// Token: 0x060011A6 RID: 4518 RVA: 0x000514B4 File Offset: 0x0004F6B4
		public static void SerializeDataToFileAsync<T>(T data, string path, string fileName) where T : SerializableData
		{
			DataProcessor.<SerializeDataToFileAsync>d__1<T> <SerializeDataToFileAsync>d__;
			<SerializeDataToFileAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SerializeDataToFileAsync>d__.data = data;
			<SerializeDataToFileAsync>d__.path = path;
			<SerializeDataToFileAsync>d__.fileName = fileName;
			<SerializeDataToFileAsync>d__.<>1__state = -1;
			<SerializeDataToFileAsync>d__.<>t__builder.Start<DataProcessor.<SerializeDataToFileAsync>d__1<T>>(ref <SerializeDataToFileAsync>d__);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000514FC File Offset: 0x0004F6FC
		public static T DeserializeDataFromFile<T>(string path) where T : SerializableData
		{
			T result;
			try
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					T t = DataProcessor.DecompressAndDeserializeObject<T>(array);
					if (t != null)
					{
						Debug.Log("[SAVE] Successfully deserialized file from " + path);
					}
					else
					{
						Debug.Log("[SAVE] Could not cast file to specified type from " + path);
					}
					result = t;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[SAVE] Ran into error while attempting to deserialize!\n\n" + ex.Message);
				result = default(T);
			}
			return result;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x000515B0 File Offset: 0x0004F7B0
		public static byte[] SerializeAndCompressObject<T>(T obj)
		{
			string s = JsonConvert.SerializeObject(obj, DataProcessor.SerializerSettings);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00051634 File Offset: 0x0004F834
		public static T DecompressAndDeserializeObject<T>(byte[] compressedData)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream(compressedData))
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					using (StreamReader streamReader = new StreamReader(gzipStream))
					{
						result = JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd(), DataProcessor.SerializerSettings);
					}
				}
			}
			return result;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x000516B4 File Offset: 0x0004F8B4
		public static void SerializeDataToJsonFile<T>(T data, string path, string fileName) where T : SerializableData
		{
			string path2 = Path.Combine(path, fileName);
			string directoryName = Path.GetDirectoryName(path2);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			try
			{
				string contents = JsonConvert.SerializeObject(data, Formatting.Indented, DataProcessor.SerializerSettings);
				File.WriteAllText(path2, contents);
				Debug.Log("[SAVE] Successfully wrote JSON file " + fileName + " to " + path);
			}
			catch (Exception ex)
			{
				Debug.LogError("[SAVE] Ran into error while attempting to serialize to JSON!\n\n" + ex.Message);
			}
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0005173C File Offset: 0x0004F93C
		public static T DeserializeDataFromJsonFile<T>(string path) where T : SerializableData
		{
			T result;
			try
			{
				T t = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), DataProcessor.SerializerSettings);
				if (t != null)
				{
					Debug.Log("[SAVE] Successfully deserialized JSON file from " + path);
				}
				else
				{
					Debug.Log("[SAVE] Could not cast JSON file to specified type from " + path);
				}
				result = t;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[SAVE] Ran into error while attempting to deserialize JSON!\n\n" + ex.Message);
				result = default(T);
			}
			return result;
		}

		// Token: 0x04000F0C RID: 3852
		public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto
		};
	}
}
